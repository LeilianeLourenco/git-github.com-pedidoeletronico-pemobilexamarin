# Otimização das rotinas de sincronização — AppPe (Xamarin)

> Documento de planejamento. Gatilho do trabalho: base com ~40 mil pedidos, sincronização
> muito lenta e tela de Pedidos travando em "Pesquisando...". Data: 2026-06-15.

## 1. Já implementado (entrou no APK de teste)

### Tela "Pedidos" (não travar mais)
- `AppPe/DataAccess.cs` — 3 índices SQLite: `ix_pv_listagem` (`tb_pedidovenda`:
  idEmpresa, idRepresentantePedido, idClientesOffLine), `ix_pv_idpedidodisplay`,
  `ix_estoque_insuf_pv` (`TB_ESTOQUE_INSUFICIENTE`.idPedidoVendaOffLine).
- `AppPe/ViewModel/Pedido/ListarPedidoViewModelNew.cs` — `LoadItens` reescrito: consulta
  pesada fora da UI thread (`await Task.Run`), `try/finally` garantindo que nunca fica preso
  em "Pesquisando..." (mesmo com exceção), e render incremental (de cima pra baixo).

### Sincronização (quick-win client-only)
- `AppPe/DataAccess.cs` — `PRAGMA journal_mode=WAL` + `synchronous=NORMAL` na criação da
  conexão. **Beneficia TODAS as tabelas** (fsync deixa de ocorrer por insert; só em checkpoint).
- `AppPe/Common/UtilHttp.cs` — GZip (`AutomaticDecompression`) em `CurrentHttpClient` e
  `CurrentApiMobileHttpClient`. Só rende se a **compressão dinâmica do IIS** estiver ligada.
- `AppPe/ViewModel/Sincronizacao/SincronizacaoNewViewModel.cs` — `SincronizacaoDownloadPedido`:
  `BeginTransaction/Commit/Rollback` por página (1 commit por página, não por registro).
  Localizado no caminho de pedido, sem tocar no `SaveSincronizacao` compartilhado.

## 2. Diagnóstico das rotinas pesadas (pendentes)

Arquivo central: `AppPe/ViewModel/Sincronizacao/SincronizacaoNewViewModel.cs`.

1. **Padrão por-registro é sistêmico.** `SavePrivate` (L2527) e `SavePrivatePaginado` (L2611)
   fazem `foreach → await SaveSincronizacao`. Cada `SaveSincronizacao` (L1962) faz
   `await Task.Run` + `SELECT COUNT(*)` (existência) + `Insert` em autocommit. Logo,
   **produtos, clientes, contatos, endereços, estoque** sofrem o mesmo que os pedidos sofriam.
   (O WAL já mitiga parcialmente todos.)
2. **Produtos e clientes NÃO paginam.** `SincronizacaoDownload<T>` (L960) baixa a tabela
   inteira num único GET (`GetListRegistroSync`). Base grande → JSON gigante, pico de memória,
   risco de timeout/OOM.
3. **Imagem por produto dentro do loop de save.** `SavePrivate` (L2585): para cada produto com
   imagem, chama `GetRegistroSyncImagem` (HTTP sequencial) no meio da gravação. Com milhares de
   produtos, isso domina o tempo da sync.
4. **`SELECT COUNT(*)` por registro** só pra decidir insert/update → trocável por upsert
   (`InsertOrReplace`) quando a PK permite → corta ~metade das queries.
5. **Um `Task.Run` por registro** → overhead em escala (N saltos de threadpool).

## 3. Plano — Client-only (sem mexer no server)

| Item | Descrição | Impacto | Risco | Esforço |
|------|-----------|---------|-------|---------|
| **A** | Transação por lote em `SavePrivate`/`SavePrivatePaginado` (imagem FORA da transação, pra não segurar lock durante rede) | Alto (todas as tabelas) | Alto (caminho central, testar tabela a tabela) | Médio |
| **B** | Eliminar `Task.Run` por registro — gravar o lote numa transação única | Médio | Médio | Baixo |
| **C** | Upsert (`InsertOrReplace`) no lugar de `COUNT(*)` + `Insert` onde a PK permitir | Médio | Médio | Médio |
| **D** | Desacoplar imagens de produto do loop: download em lote/limite paralelo, ou lazy (baixar só quando a tela do produto abrir) | **Alto** (catálogo grande) | Baixo (cirúrgico, só produtos) | Médio |
| E | (Feito) WAL/synchronous, índices, GZip | — | — | — |

## 4. Plano — Server-dependent (resolve base gigante de vez)

| Item | Descrição | Impacto |
|------|-----------|---------|
| **F** | Paginar `SincronizacaoDownload<T>` de produtos/clientes (hoje vem tudo num GET) | Alto |
| **G** | Endpoint de COUNT para o pedido (eliminar pré-download dos 40k IDs em `GetRegistroIDSync`) | Médio/Alto |
| **H** | Janela de data: baixar só os últimos N meses / abertos+recentes (ajuste em `ApiPedidoVendaMobile`) | **Alto** (mata o "40 mil") |
| **I** | Endpoint de imagem em lote (1 chamada por página, não 1 por produto) | Alto (catálogo) |

## 5. Ordem recomendada

- **Catálogo grande (muitos produtos):** D → A/B → I/F.
- **Base grande de pedidos:** H + G (server) — a transação por página já está feita no client.
- Começar por **D** (cirúrgico, alto impacto, baixo risco) é o melhor custo-benefício client-only.

## 6. Observações de segurança (deploy)

- `SavePrivate` é o caminho de save de quase toda tabela → qualquer mudança em A/B/C exige
  varredura completa (build full + teste de cada tabela na sync), sem regressão em produção.
- Transação NÃO pode envolver chamadas de rede (download de imagem) — manter I/O fora do escopo
  da transação para não segurar write-lock.
- WAL: o `.db3` existente migra automaticamente para WAL na primeira abertura.
