# Skill: Xamarin AppPe / Pedido Eletrônico Mobile

Este skill é um agente de orientação para alterações no app Xamarin.Forms localizado em `c:\PeMobileXamarim`, com foco em manter a consistência com o backend do projeto `backlog`.

## 1. Objetivo

- Entender os pontos de integração entre o app Xamarin e o backend do `backlog`.
- Identificar o que depende do quê para aplicar alterações em paralelo com segurança.
- Manter o padrão existente do app e do backend.
- Buscar melhorias estruturais sem quebrar o fluxo atual.
- Apontar falhas comuns e validar sempre antes de concluir.

## 2. Pontos comuns entre Xamarin e backlog

- O app Xamarin usa a DLL externa `dllExtern/Hlp.PedidoEletronico.Domain.Business.dll` para lógica de negócio compartilhada, incluindo cálculo de pedido, planos e regras comerciais.
- O app depende dos endpoints WebAPI do backend para login, sincronização e upload/download de dados.
- A sincronização offline depende da mesma modelagem de dados usada no backend: clientes, produtos, tabela de preço, pedidos, estoque, agenda e regras comerciais.
- A arquitetura do app segue o padrão MVVM com repositories estáticos e `sqlite-net-pcl` para banco local.
- O `MAPEAMENTO.md` do Xamarin descreve exatamente o fluxo de inicialização, os URLs de ambiente e os mecanismos de sync que devem ser mantidos.

## 3. Dependências críticas

### 3.1 Dependências do Xamarin sobre o backend

- Endpoints REST/API: `App.UrlWebApi`, `App.UrlWebApiMobile`, `App.UrlApiImage`, `App.UrlPortal`.
- Contratos de API: rotas `api/{controller}/{action}` e payloads JSON esperados pelos métodos de `UtilHttp`.
- A ordem e os nomes dos controllers de sync definidos em `TableMobile.GetApiRegistroByModel<T>()` e `SincronizacaoDownload<TModel>()`.
- DTOs que o app usa para serializar/deserializar (`Model/*.cs`, `RetornoSalvar<T>`, `LoginMobile`, etc.).
- Regras de negócio de pedido e preço quando armazenadas na DLL externa. Se o backend mudar regras, o app deve receber DLL atualizada.

### 3.2 Dependências do backlog sobre o Xamarin

- O backend precisa preservar contratos de API consumidos pelo app, mas não depende do app compilação.
- A UX e o fluxo offline do app orientam quais APIs de sync precisam ser suportadas pelo backend.
- Correções no backend de `Ionic.MobileWebApi3` e `WebApiPeMobile` impactam diretamente o app, especialmente em filtros de agenda, estoque e pedidos.

### 3.3 Dependência da DLL compartilhada

- `Xamarin.HLP.Mobile.AppPE` referencia a DLL `Hlp.PedidoEletronico.Domain.Business.dll` em `AppPe/dllExtern/`.
- Isso significa que mudanças de regra no backend em `Hlp.PedidoEletronico.Domain.Business` exigem atualização manual ou nova cópia da DLL no app.
- Se houver divergência entre backend e app no cálculo de pedido, desconto ou comissão, é mais provável que a DLL esteja desatualizada.

## 4. Checklist de alteração em paralelo

1. Identificar o fluxo completo afetado (ex: cadastro de preço, pedido, sync de cliente).
2. Mapear quais projetos backend precisam mudar: `Hlp.PedidoEletronico.Ionic.MobileWebApi3`, `Hlp.PedidoEletronico.WebApiPeMobile`, `Hlp.PedidoEletronico.Domain.Business`, e possivelmente `Hlp.PedidoEletronico.Data`.
3. Verificar se o app usa a mesma tabela/entidade no SQLite local e no backend.
4. Atualizar a API ou DTOs no backend e, em seguida, atualizar o app somente se o contrato JSON mudar.
5. Validar a versão da DLL externa e sincronizar com o app Xamarin se houver mudanças de regra.
6. Testar build do app Xamarin e do backend relevante.
7. Executar o fluxo de sync local: login, download incremental, upload de pedidos, exclusões e assinaturas.
8. Verificar o ambiente correto (Producao/Homologacao/Local) e URLs no `App.AmbienteApp`.

## 5. Principais riscos e falhas comuns

- `Xamarin.HLP.Mobile.AppPE.Droid` antigo: não editar, pois o projeto ativo é `AppPe/AppPe.Android`.
- Contrato de API quebrado por rota/parametro diferente em backend.
- Erros de serialização JSON por modelos divergentes entre app e API.
- Falta de tratamento de `null` em sync e no `TableMobile.GetInfoModel<T>()`.
- Uso de string interpolada em SQL no app e backend — cuidado com injeção e dados inesperados.
- Mudanças no backend não acompanhadas de atualização da DLL `Hlp.PedidoEletronico.Domain.Business.dll` no app.
- Banco SQLite local sem migração explícita: adição de colunas funciona, mas alterações de estrutura como rename/drop precisam de tratamento manual.
- Dependências de serviços nativos: verificações de `DependencyService.Get<...>()` e `Permissões Android/iOS` podem falhar em novos dispositivos.
- Histórico de duplicação: não replicar alterações no projeto legado `Xamarin.HLP.Mobile.AppPE.Droid` ou outras pastas antigas.

## 6. Validação obrigatória

- Build completo da solução Xamarin (`AppPe.sln`) e build dos projetos backend afetados no `backlog`.
- Teste de sincronização real: login, download incremental, upload de pedidos e exclusões.
- Verificar se URLs de ambiente apontam para o endpoint correto para o propósito da alteração.
- Revisar qualquer novo arquivo XAML/CS/JSON para garantir consistência com padrões MVVM existentes.
- Conferir se `App.Data.PrimeiraAnalise()` inclui novas tabelas ou colunas necessárias.
- Validar se o `UtilHttp` continua usando os mesmos clientes HTTP corretos e se nenhum `HttpClient` é instanciado desnecessariamente.

## 7. Boas práticas de código para este agente

- Preserve padrão MVVM: lógica de negócio no ViewModel/Repository, UI em View.
- Reutilize `ViewModelComum<T>` e `Utils` existentes sempre que possível.
- Evite duplicação entre backend e app; prefira extrair a lógica comum para a DLL compartilhada.
- Quando precisar criar novo model, alinhe nome e tabela com `TableMobile`.
- Se alterar sync de tabela nova, atualize a ordem de download em `InitSincronizacaoDownload()` e o registro em `TableMobile.GetInfoModel<T>(TipoRetornoInfoClass)`.
- Prefira `Task`/`async` sem `Result`/`Wait()` para não bloquear UI.

## 8. Quando usar este agente

- Ao fazer alterações que atravessam app Xamarin e backend simultaneamente.
- Ao revisar bug de sync, login, ambiente ou lógica de pedido no app.
- Ao validar contratos de API e dependências entre `PeMobileXamarim` e `backlog`.
- Ao propor melhorias no app que não devem quebrar o padrão existente nem gerar regressão.
