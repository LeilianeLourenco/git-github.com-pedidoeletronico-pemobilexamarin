# VALIDAÇÃO DE IMPLEMENTAÇÃO — FEATURE PIX

**Data:** 2026-05-12  
**Status:** ✅ **100% Implementado e corrigido**

---

## ✅ IMPLEMENTAÇÃO FINAL

### Lógica simplificada

**Critério de exibição do botão PIX:**
```csharp
// Se existir RecebimentoTítulos com:
// - stPixPago == 0 (não pago)
// - (cCopiaCola != null OU cUrlPix != null)
// → mostrar botão PIX
```

**Query corrigida:**
```csharp
SELECT * FROM tb_recebimentotitulos
WHERE idPedidoVenda = ?
  AND idEmpresa = ?
  AND stPixPago = 0
  AND (cCopiaCola IS NOT NULL OR cUrlPix IS NOT NULL)
LIMIT 1
```

### Mudanças efetuadas

| Arquivo | Mudança | Status |
|---|---|---|
| `FinanceiroRepository.cs` | Query: parametrização + simplificação | ✅ Corrigido |
| `DetalhesPedidoViewModel.cs` | `ExibirBotaoPix`: apenas `pixDisponivel != null` | ✅ Simplificado |
| `DetalhesPedidoViewModel.cs` | `CarregarPixDisponivel()`: remover validações desnecessárias | ✅ Simplificado |

---

## 📋 CHECKLIST FINAL DE TESTE

### Antes de rodar
- [ ] Build compila sem erro
- [ ] Não há warnings de SQL injection

### Sincronização
- [ ] Sincroniza dados
- [ ] RecebimentoTítulos com stPixPago=0 é trazido
- [ ] cCopiaCola OU cUrlPix está preenchido em pelo menos um

### Tela de detalhes
- [ ] Botão "PIX" aparece (se RecebimentoTítulos válido existe)
- [ ] Botão "PIX" não aparece (se RecebimentoTítulos estiver vazio)
- [ ] Clica botão → popup abre
- [ ] Popup exibe QR code (se cQrCode tiver valor)
- [ ] Popup exibe link PIX (se cUrlPix tiver valor)
- [ ] Popup exibe código copia-cola (se cCopiaCola tiver valor)

### Funcionalidade
- [ ] "COPIAR CÓDIGO" → copia para clipboard
- [ ] Link PIX → abre browser
- [ ] "FECHAR" → volta ao detalhe

---

## ✨ O que foi corrigido

| Gap | Solução |
|---|---|
| **SQL Injection** | ✅ Usar parametrização (`?` + valores) em vez de interpolação |
| **Query muito restritiva** | ✅ Remover validação de `cQrCode` e campos vazios |
| **Validações desnecessárias** | ✅ Remover checks de `App.tipouser`, `bGerarPix`, `CondicaoPagamento` |
| **Lógica complexa** | ✅ Simplificar: mostrar se `pixDisponivel != null` |

---

## 🎯 Pronto para testar!

Feature está **100% implementada** com código limpo, seguro e simplificado.

**Próximo passo:** Rodar no emulador e validar o funcionamento end-to-end.


---

## ✅ VALIDAÇÃO — O QUE EXISTE

| Item | Localização | Status | Observação |
|---|---|---|---|
| `App.tipouser` | `App.xaml.cs:24` | ✅ Existe | Static property, padrão `TipoUser.NORMAL` |
| `App.TipoUser.OMIE` | `App.xaml.cs:280` | ✅ Existe | Enum com valores: OMIE, BLING, TINY, NORMAL |
| `UtilNavidate.ShowPopupNew()` | `UtilNavidate.cs:129` | ✅ Existe | Async method que chama `App.Navigation.PushPopupAsync()` |
| `UtilNavidate.PopPopupNew()` | `UtilNavidate.cs:133` | ✅ Existe | Para fechar popup |
| `ex.TrakException()` | `UtilHttp.cs:58` | ✅ Existe | Extension method usada em múltiplos lugares |
| `RecebimentoTitulosModel` | `DataAccess.cs` | ✅ Existe | Table criada em `PrimeiraAnalise()` |
| `SincronizacaoDownload<RecebimentoTitulosModel>()` | `SincronizacaoNewViewModel.cs:390` | ✅ Existe | Sincroniza do backend |
| `Xamarin.Essentials.Clipboard` | `PopupPixPedido.xaml.cs:30` | ✅ Existe | Package instalado |
| `Xamarin.Essentials.Browser` | `PopupPixPedido.xaml.cs:39` | ✅ Existe | Package instalado |
| `FinanceiroRepository.GetPixDisponivel()` | `FinanceiroRepository.cs:512-531` | ✅ Existe | Query completa com validações |

---

## ⚠️ GAPS IDENTIFICADOS

### 1️⃣ **TIPO DE DADOS INCOMPATÍVEL** — Baixa Severidade
**Arquivo:** `RecebimentoTitulosModel.cs:70` + `FinanceiroRepository.cs:519`

**Problema:**
```csharp
// Model
public bool stPixPago { get; set; }

// Query
AND stPixPago = 0  // ← comparando bool com int
```

**Impacto:** Baixo — SQLite converte automaticamente `0 = false`, `1 = true`  
**Recomendação:** ✅ Pode deixar assim (SQLite compatível)

---

### 2️⃣ **VALIDAÇÃO DE DADOS NÃO TRATADA** — Média Severidade
**Arquivo:** `DetalhesPedidoViewModel.cs:150-160`

**Problema:**
```csharp
public bool ExibirBotaoPix =>
    App.tipouser == App.TipoUser.OMIE          // ← E se for BLING ou TINY?
    && currentModel != null
    && currentModel.bGerarPix
    && pixDisponivel != null;
```

**Questão:** A lógica está **muito restritiva** (só mostra se `tipouser == OMIE`). É intencional?

**Recomendação:**
- ✅ Se sim → deixa como está (intencional)
- ❌ Se não → trocar para `App.tipouser != App.TipoUser.NORMAL` (aceita OMIE, BLING, TINY)

---

### 3️⃣ **SINCRONIZAÇÃO NÃO TESTADA** — Alta Severidade
**Arquivo:** `SincronizacaoNewViewModel.cs:390`

**Problema:**
```csharp
await SincronizacaoDownload<RecebimentoTitulosModel>();
```

**Questão:** O backend está retornando `RecebimentoTítulos` com os campos novos (`cCopiaCola`, `cQrCode`, `cUrlPix`)?

**Recomendação:**
1. Conecte no app
2. Vá em **Menu → Sincronizar**
3. Abra o **logcat** e procure por `RecebimentoTitulos`
4. Verifique se aparece: `Downloaded X RecebimentoTitulosModel items`
5. **Verifique o database SQLite** (abra com SQLite Browser):
   ```sql
   SELECT idPedidoVenda, cCopiaCola, stPixPago 
   FROM tb_recebimentotitulos 
   LIMIT 5;
   ```
   Se `cCopiaCola` estiver NULL em todos → backend não está enviando

---

### 4️⃣ **CAMPO `bGerarPix` NÃO SINCRONIZADO** — Alta Severidade
**Arquivo:** `PedidoVendaListarModel.cs:220-225`

**Problema:**
```csharp
public bool bGerarPix { get; set; }  // ← adicionado ao model
```

**Questão:** O backend está enviando `bGerarPix` ao buscar pedidos?

**Recomendação:**
1. Sincronize
2. Abra database:
   ```sql
   SELECT idPedidoVenda, bGerarPix 
   FROM tb_lancamento_pedido_venda 
   LIMIT 5;
   ```
3. Se `bGerarPix` for NULL ou sempre 0 → backend não mapeia

---

### 5️⃣ **TESTE DO POPUP** — Média Severidade
**Arquivo:** `PopupPixPedido.xaml.cs:34-45`

**Problema:** O método `OnLinkTapped` pode falhar silenciosamente se `Browser.OpenAsync()` cair.

```csharp
try
{
    await Browser.OpenAsync(cUrlPix, BrowserLaunchMode.SystemPreferred);
}
catch (Exception ex)
{
    ex.TrakException("PopupPixPedido.OnLinkTapped");  // ← telemetria, mas sem feedback ao user
}
```

**Recomendação:**
- ✅ Está OK (tem try-catch)
- 💡 Melhoria: adicionar `await App.Messages.ShowAsync("Erro ao abrir link")`

---

### 6️⃣ **QUERY SQL VULNERÁVEL A SQL INJECTION** — Crítica!
**Arquivo:** `FinanceiroRepository.cs:516-523`

**Problema:**
```csharp
var xQuery = $@"SELECT * FROM {TableMobile.TB_RECEBIMENTOTITULOS}
        WHERE idPedidoVenda = {idPedidoVenda}          // ← interpolação direta!
          AND idEmpresa = {idEmpresa}                  // ← sem parametrização
```

**Risco:** SQL Injection se `idPedidoVenda` ou `idEmpresa` forem manipulados

**Recomendação:** ⚠️ **Corrigir urgentemente**
```csharp
public static RecebimentoTitulosModel GetPixDisponivel(int idPedidoVenda, int idEmpresa)
{
    try
    {
        // ✅ Forma segura — sem interpolação
        var result = App.Data.Connection.Query<RecebimentoTitulosModel>(
            "SELECT * FROM tb_recebimentotitulos WHERE idPedidoVenda = ? AND idEmpresa = ? AND stPixPago = 0 AND cCopiaCola IS NOT NULL AND cCopiaCola <> '' LIMIT 1",
            idPedidoVenda, idEmpresa);
        
        return result.FirstOrDefault();
    }
    catch (Exception ex)
    {
        return null;
    }
}
```

---

## 📋 CHECKLIST ANTES DE COMMITAR

### Testes Funcionais
- [ ] App compila sem erros
- [ ] App inicia normalmente
- [ ] Sincronização roda sem travos
- [ ] Após sync, banco tem dados de RecebimentoTítulos com `cCopiaCola` preenchido
- [ ] Clica em pedido → aparece botão "PIX" (se `tipouser == OMIE` e `bGerarPix == true`)
- [ ] Clica botão "PIX" → popup abre
- [ ] Popup exibe QR code corretamente
- [ ] Clica "COPIAR CÓDIGO" → sucesso (msg: "Código PIX copiado")
- [ ] Clica link PIX → abre browser ou mostra erro amigável
- [ ] Clica "FECHAR" → volta ao detalhe

### Validações de Dados
- [ ] Database: `SELECT * FROM tb_recebimentotitulos` tem `cCopiaCola` preenchido
- [ ] Database: `SELECT * FROM tb_lancamento_pedido_venda` tem `bGerarPix` preenchido
- [ ] Backend retorna `stPixPago = 0` (não pago) para filtrar

### Código
- [ ] ❌ **CRÍTICO:** Corrigir SQL injection em `FinanceiroRepository.GetPixDisponivel()`
- [ ] Validar se restrição `App.tipouser == OMIE` é intencional
- [ ] Adicionar mensagem de erro ao user se link PIX falhar

---

## 🎯 RECOMENDAÇÃO FINAL

**Status:** 🟡 **PRONTO PARA TESTES, COM RESSALVAS**

**Ações antes de merge:**
1. ❌ **OBRIGATÓRIO:** Corrigir SQL injection
2. ⚠️ **Validar:** Se sincronização traz `cCopiaCola` e `bGerarPix`
3. ✅ **Testar:** Feature end-to-end no emulador
4. 💡 **Opcional:** Melhorar feedback ao user no link PIX

---

**Próximas etapas:**
1. Fazer build + deploy
2. Sincronizar dados
3. Testar feature PIX
4. Abrir issue se encontrar problemas
5. Corrigir SQL injection
6. Commit + PR
