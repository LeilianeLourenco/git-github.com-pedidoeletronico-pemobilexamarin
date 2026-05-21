---
name: manutencao
description: "Manutencao do AppPe (pedidoeletronico.com) - guia para correcoes, features, sincronizacao, UI, e integracoes no sistema de pedido eletronico Xamarin.Forms e base web MVC ASP.NET."
---

# Skill de Manutenção — AppPe (pedidoeletronico.com)

Você é um especialista em manutenção do aplicativo AppPe, um sistema de pedido eletrônico em Xamarin.Forms para representantes comerciais. Analise a tarefa abaixo e execute seguindo rigorosamente os padrões do projeto.

## Tarefa

$ARGUMENTS

---

## 1. Classificação da Tarefa

Antes de iniciar, classifique a tarefa:
- **Tipo**: Bug Fix | Nova Feature | Sync Issue | UI Change | API Integration | Refactoring
- **Camadas afetadas**: Model | Repository | ViewModel | View (XAML) | View (code-behind) | Common/Util | Platform-specific | Core business logic

Leia os arquivos relevantes ANTES de propor qualquer alteração.

---

## 2. Arquitetura e Padrões

### MVVM — Hierarquia de Base Classes

```
INotifyPropertyChanged
├── NotifyCommon                      (Common/NotifyCommon.cs)
│   ├── canExecuteInicial, ExecuttingAnyCommand
│   ├── Ícones toolbar (ImageIconSearch, ImageIconSave, etc.)
│   └── SearchCommom                  (Common/NotifyCommon.cs)
│       ├── SearchCommand, LoadItensCommand
│       ├── bFind, IsBusy, xFiltro
│       └── Usado para ViewModels de LISTAGEM com busca
│
├── ViewModelComum<T>                 (Common/ViewModelComum.cs)
│   ├── currentModel (T), NavigateToCommand, CancelCommand
│   ├── isVisibleListView, canExecuteInicial
│   └── Usado para ViewModels de CRUD/detalhe
│
└── ModelComum                        (Common/ModelComum.cs)
    ├── SaveCommand, DeleteCommand, AddCommand
    ├── needRefresh, transformações FFImageLoading
    └── Base para TODOS os Models persistidos em SQLite
```

**Padrão de propriedade bindable:**
```csharp
private ObservableCollection<MeuModel> _lista;
public ObservableCollection<MeuModel> Lista
{
    get { return _lista; }
    set { _lista = value; NotifyPropertyChanged(); }
}
```

**Padrão de inicialização do ViewModel:**
```csharp
public async void Initialize()
{
    if (!canExecuteInicial) return;
    canExecuteInicial = false;
    // carregar dados...
}
```

**Padrão de command com bloqueio:**
```csharp
async void MeuCommand()
{
    if (ExecuttingAnyCommand) return;
    ExecuttingAnyCommand = true;
    try { /* lógica */ }
    catch (Exception ex) { ex.TrakException(); }
    finally { ExecuttingAnyCommand = false; }
}
```

### Repository — Padrão Estático

- Todos os métodos são `static` — **nunca criar instâncias**
- Acesso ao banco: `App.Data.Connection` (SQLiteConnection síncrono)
- SQL raw é comum (não apenas LINQ), especialmente em queries com JOIN
- Retorno sempre `List<T>`, nunca `IEnumerable<T>`
- Nomes de tabelas via `TableMobile.TB_*` constantes
- **Filtro de empresa é obrigatório** em quase toda query:
  ```csharp
  where idEmpresa = {App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa}
  ```
- Filtro de acesso a clientes: verificar `stAcessoTodosClientes`

### Navegação (UtilNavidate)

| Método | Uso |
|--------|-----|
| `UtilNavidate.PushAsync(page)` | Navegar para frente |
| `UtilNavidate.PopAsync()` | Voltar |
| `UtilNavidate.PushModalAsync(page)` | Modal |
| `UtilNavidate.PopModalAsync()` | Fechar modal |
| `UtilNavidate.ShowPopupNew(page)` | Popup (Rg.Plugins.Popup) |
| `UtilNavidate.GoToHome()` | Voltar ao home |

App root: `RootPage` (MasterDetailPage com menu hamburger)

### HTTP/API (UtilHttp)

- Classe estática `UtilHttp` com `HttpClient`
- Serialização: `JsonConvert.SerializeObject()` / `DeserializeObject<T>()`
- Error handling: `WebException` → `SincronizacaoNewViewModel.bFalhaConexao = true`
- Exceções genéricas: `ex.TrakException()`
- URLs base definidas em `App.xaml.cs`:
  - `App.UrlWebApi` — API principal (CRUD, autenticação)
  - `App.UrlWebApiMobile` — API mobile (sync, dados específicos)
  - `App.UrlApiImage` — Upload/download de imagens
  - `App.UrlReport` — Relatórios
  - `App.UrlPortal` — Portal de pagamentos

### Sincronização (Offline-First)

- Orquestrador: `SincronizacaoNewViewModel` (ViewModel/Sincronizacao/)
- Tabelas criadas em: `DataAccess.PrimeiraAnalise()`
- Novos campos em tabelas existentes: `ALTER TABLE` em `DataAccess.cs`
- Progresso: `SincronizacaoNewModel` com `Display`, `xDetail`, `iCount`
- Flags de erro: `bFalhaConexao`, `ocorreuErro`
- Force sync: `ForceAtualizacaoModel`
- Setup inicial: `UpdateInitRepository`

### Error Handling

- Extension method: `ex.TrakException(detail, bShowMessage)`
- Analytics: `GoogleInsightsReportingConstants.TrakException(display, message, isFatalError)`
- Erros de sync: `SincronizacaoNewViewModel.ocorreuErro = true`

---

## 3. Playbooks de Tarefas Comuns

### Bug Fix
1. Identificar View/ViewModel/Repository/Model afetados
2. Traçar fluxo: UI binding → ViewModel property/command → Repository → SQLite/API
3. Verificar filtro de empresa (`idEmpresa`) e acesso (`stAcessoTodosClientes`)
4. Aplicar fix seguindo os padrões de error handling existentes
5. Testar cenários: dados locais (offline) e dados da API (online)

### Novo Campo em Entidade Existente
1. Adicionar propriedade no Model (`AppPe/Model/`)
2. Se persistido em SQLite: adicionar migration em `DataAccess.cs`:
   ```csharp
   try { Connection.Execute("ALTER TABLE TB_TABELA ADD COLUMN xNovoCampo TEXT"); } catch { }
   ```
3. Atualizar queries no Repository para incluir o campo
4. Adicionar binding no ViewModel
5. Adicionar XAML binding na View
6. Se sincronizado: atualizar modelo de sync e serialização

### Nova Página
1. Criar Model em `AppPe/Model/{Dominio}/` (herdar `ModelComum` se persistido)
2. Criar ViewModel em `AppPe/ViewModel/{Dominio}/`:
   - Listagem → herdar `SearchCommom`
   - Detalhe/CRUD → herdar `ViewModelComum<MeuModel>`
3. Criar XAML em `AppPe/View/{Dominio}/Page{Nome}.xaml`
4. Configurar BindingContext no XAML ou code-behind
5. Adicionar navegação no parent (menu, botão, command)

### Problema de Sincronização
1. Verificar `SincronizacaoNewViewModel` para identificar o step que falha
2. Verificar se endpoint API está acessível e retorna formato esperado
3. Checar flags `bFalhaConexao` e `ocorreuErro`
4. Verificar deserialização de `RetornoSalvar<T>` com resposta da API
5. Verificar mapeamento em `TableMobile.GetApiRegistroByModel<T>()`

### Alteração de UI
1. Identificar a página XAML e seu BindingContext
2. Usar valores do resource dictionary de `App.xaml` (cores, tamanhos)
3. Cores padrão: `ColorStaticModel` (Common/)
4. Converters existentes: `View/Converter/Generic/`
5. Controles custom: `Controls/custom/` e `Controls/xaml/`
6. SVG: `FFImageLoading.Svg` com path assembly `Xamarin.HLP.Mobile.AppPE.Images.{nome}.svg`
7. Tamanhos por plataforma: usar `OnPlatform` em XAML

### Integração API
1. Adicionar chamada em `UtilHttp.cs` seguindo padrão existente
2. Wrap em try/catch:
   - `WebException` → `bFalhaConexao = true`
   - `Exception` genérica → `ex.TrakException()`
3. Usar `JsonConvert.SerializeObject/DeserializeObject`
4. Escolher URL base correta: `UrlWebApi`, `UrlWebApiMobile`, `UrlApiImage`

---

## 4. Referência de Domínio

| Conceito | Classe | Descrição |
|----------|--------|-----------|
| Pedido | `PedidoVendaModel` | Pedido de venda (transação principal) |
| Itens | `PedidoVendaItensModel` | Itens do pedido |
| Cliente | `ClientesModel` | Clientes (CPF pessoa física, CNPJ jurídica) |
| Produto | `ProdutoModel` | Produtos com suporte a variações |
| Tabela Preço | `TabelaPrecoModel` | Tabelas de preço (geral, default, específica, campanha) |
| Representada | `RepresentadaModel` | Empresa representada (fornecedor/marca) |
| Cond. Pagamento | `CondicaoPagamentoModel` | Condições de pagamento |
| Agenda | `AgendaModel` / `AtividadeModel` | Atividades CRM/calendário |
| Estoque | `EstoqueModel` | Inventário/estoque |

**Status de Pedido** (`stPedidoVenda`): `0` = aberto, `1` = cancelado, `2` = vendido
**Tipo de Lançamento** (`stLancamento`): diferencia pedido vs. orçamento
**Tipo de Usuário** (`App.tipouser`): OMIE, BLING, TINY, CAPOLI, NORMAL (integrações ERP)
**Multi-empresa**: usuário pertence a múltiplas empresas, empresa atual em `App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel`

---

## 5. Localização Rápida de Arquivos

| Concern | Path |
|---------|------|
| Base classes MVVM | `AppPe/Common/NotifyCommon.cs`, `ViewModelComum.cs`, `ModelComum.cs` |
| HTTP/API | `AppPe/Common/UtilHttp.cs` |
| Navegação | `AppPe/Common/UtilNavidate.cs` |
| Mensagens | `AppPe/Common/UtilMessages.cs` |
| Utilitários | `AppPe/Common/UtilMethods.cs` |
| Extensions | `AppPe/Extensions.cs` |
| Enums | `AppPe/Common/Enums.cs` |
| Cores | `AppPe/Common/ColorStaticModel.cs` |
| Config fontes | `AppPe/Common/SettingsModel.cs` |
| App global | `AppPe/App.xaml.cs` |
| Banco de dados | `AppPe/DataAccess.cs` |
| Tabelas SQLite | `AppPe/TableMobile.cs` |
| Analytics | `AppPe/GoogleInsightsReportingConstants.cs` |
| ViewModels | `AppPe/ViewModel/{Dominio}/` |
| Models | `AppPe/Model/{Dominio}/` |
| Repositories | `AppPe/Model/Repository/` |
| Views/Páginas | `AppPe/View/{Dominio}/` |
| Converters | `AppPe/View/Converter/` |
| Controles custom | `AppPe/Controls/custom/`, `AppPe/Controls/xaml/` |
| Business logic | `AppPe/Core/PedidoVenda/` |
| Criptografia | `AppPe/Core/Criptografia/` |
| Services interfaces | `AppPe/Services/` |
| Sync | `AppPe/ViewModel/Sincronizacao/`, `AppPe/View/Sincronizacao/` |
| Android renderers | `AppPe.Android/ExtendRender/` |
| iOS renderers | `AppPe.iOS/ExtendRender/` |
| Android services | `AppPe.Android/Services/` |
| iOS services | `AppPe.iOS/Services/` |

---

## 6. NuGet Packages Chave

| Package | Versão | Uso |
|---------|--------|-----|
| Xamarin.Forms | 5.0.0.2662 | Framework UI |
| sqlite-net-pcl | 1.9.172 | Banco local (SQLiteConnection síncrono) |
| Newtonsoft.Json | 13.0.3 | Serialização JSON |
| FFImageLoading + SVG | 2.4.11.982 | Imagens, cache, SVG |
| Rg.Plugins.Popup | 2.1.0 | Páginas popup/modal |
| ZXing.Net.Mobile.Forms | 2.4.1 | Leitura de código de barras |
| SignaturePad.Forms | 3.0.0 | Captura de assinatura |
| Plugin.BLE | 3.1.0 | Bluetooth |
| Xamarin.Essentials | 1.8.1 | Conectividade, permissões, device info |
| PdfSharpCore | 1.3.65 | Geração de PDF |
| CarouselView.FormsPlugin | 6.0.0 | Carrossel de imagens |

---

## 7. Mapa de ViewModels e Páginas

| ViewModel | Base | Página XAML | Propósito |
|-----------|------|-------------|-----------|
| `PedidoNewViewModel` | SearchCommom | `PagePedidoNew.xaml` (TabbedPage) | Criar/editar pedido |
| `ListarPedidoViewModelNew` | SearchCommom | `PageListarPedidos.xaml` | Listar pedidos (infinite scroll) |
| `DetalhesPedidoViewModel` | NotifyCommon | `PageDetalhesPedido.xaml` | Detalhe do pedido (popup) |
| `EditarItemViewModel` | ViewModelComum | `PageEditarItem.xaml` | Editar item do pedido |
| `ListarProdutosNewViewModel` | SearchCommom | `PageListarProdutosNew.xaml` | Catálogo de produtos na venda |
| `ListarVariacoesPedidoViewModel` | SearchCommom | `PageSelecionarVariacao.xaml` | Seleção de variações/grades |
| `ShowItensPedidoViewModel` | - | `PageShowItensPedido.xaml` | Exibir itens do pedido |
| `SignaturePedidoVendaViewModel` | - | `PageSignaturePedidoVenda.xaml` | Assinatura digital |
| `PageFinanceiroClienteViewModel` | SearchCommom | `PageFinanceiroCliente.xaml` | Financeiro do cliente |
| `ListarTabelaEscalonadaViewModel` | SearchCommom | `PageListarTabelaEscalonada.xaml` | Tabela escalonada |
| `ClienteViewModel` | ViewModelComum | `PageCliente.xaml` (TabbedPage) | Cadastro de cliente |
| `ClienteApresentacaoNewViewModel` | - | `PageApresentacaoClienteNew.xaml` | Apresentação do cliente |
| `ClientInfinitListViewModel` | SearchCommom | `PageInfinitListClientes.xaml` | Lista infinita de clientes |
| `ProdutoInfinitListViewModel` | SearchCommom | `PageInfinitListProdutos.xaml` | Lista infinita de produtos |
| `PesquisaPadraoViewModel` | SearchCommom | `PagePesquisaPadrao.xaml` | Pesquisa genérica por tabela |
| `SincronizacaoNewViewModel` | SearchCommom | `PageSyncNew.xaml` (PopupPage) | Sincronização completa |
| `HomeNewViewModel` | - | `PageHomeNew.xaml` | Tela principal/dashboard |
| `DashBoardViewModel` | - | `PageDashBoard.xaml` | Dashboard de vendas |
| `EmpresaViewModel` | - | `PageEmpresa.xaml` | Seleção de empresa |
| `ListarEventosViewModel` | SearchCommom | `PageListagemEventos.xaml` | Agenda/CRM |
| `EventoCadastroViewModel` | - | `PageEventoNew.xaml` | Cadastro de evento |
| `MenuViewModel` | ViewModelComum | `RootPage.xaml` (MasterDetail) | Menu lateral |

### Fluxo de Navegação Principal

```
PageLogin → RootPage (MasterDetail)
  ├── Master: Menu lateral (MenuViewModel)
  └── Detail:
      ├── PageHomeNew → Dashboard com botões
      │   ├── PageListarPedidos → Lista de pedidos
      │   │   ├── PagePedidoNew → Criar/editar pedido
      │   │   │   ├── PagePesquisaPadrao → Selecionar cliente/status/cond.pgto
      │   │   │   ├── PageListarProdutosNew → Adicionar itens
      │   │   │   │   └── PageEditarItem → Editar item
      │   │   │   ├── PageDescontoPedido → Aplicar desconto
      │   │   │   ├── PageComplementosPedido → Frete/seguro/outras
      │   │   │   └── PageFaturas → Parcelas/duplicatas
      │   │   └── PageDetalhesPedido (Popup) → Visualizar/Imprimir/Compartilhar
      │   ├── PageInfinitListClientes → Lista de clientes
      │   │   └── PageCliente → Cadastro (Contatos, Endereços, Telefones)
      │   ├── PageInfinitListProdutos → Lista de produtos
      │   ├── PageListagemEventos → Agenda CRM
      │   └── PageSyncNew (Popup) → Sincronização
      ├── PageDashBoard → Métricas de vendas
      ├── PageEmpresa → Trocar empresa
      └── PageSobre → Sobre o app
```

### Pesquisa Padrão (PesquisaPadraoViewModel.Tabela)

Enum que define qual tabela buscar em `PagePesquisaPadrao`:
- `TB_CLIENTE` → ClienteRepository
- `TB_ENDERECO` → EnderecoRepository
- `TB_CONDICAO_PAGAMENTO` → CondicaoPagamentoRepository
- `TB_FORMA_PAGAMENTO` → CondicaoPagamentoRepository.BuscaFormasPagamento
- `TB_TRANSPORTADORA` → TransportadoraRepository
- `TB_REPRESENTANTE` / `TB_REPRESENTANTE_MAIS_TODOS` → EmpresaAspnetUsersRepository
- `TB_REPRESENTADA` → RepresentadaRepository
- `TB_TABELA_PRECO` → TabelaPrecoRepository
- `STATUS_PEDIDO` / `STATUS_PEDIDO_APRESENTACAO` → StatusRepository
- `RAMO_ATIVIDADE` → RamoAtividadeRepository
- `TB_TIPOATIVIDADESCRM` → AgendaRepository

---

## 8. Tabelas SQLite e Models

| Constante TableMobile | Model | API Controller | PK |
|------------------------|-------|----------------|-----|
| `TB_PEDIDOVENDA` | PedidoVendaModel | APIpedidoVenda | idPedidoVendaOffLine |
| `TB_PEDIDOVENDAITENS` | PedidoVendaItensModel | - | idPedidoVendaItensOffline |
| `TB_CLIENTES` | ClientesModel | APIcliente | idClientesOffLine |
| `TB_CONTATOS` | ContatoModel | APIcontato | idContatoOffLine |
| `TB_ENDERECO` | EnderecoModel | APIendereco | idEnderecoOffLine |
| `TB_PRODUTO` | ProdutoModel | APIproduto | idProdutoOffLine |
| `TB_CATEGORIA` | CategoriaProdutoModel | APIcategoria | idCategoria |
| `TB_CONDICAOPAGAMENTO` | CondicaoPagamentoModel | APIprazo | idCondicaoPagamento |
| `TB_FORMA_PAGAMENTO` | FormaPagamentoModel | - | idFormaPagamento |
| `TB_TABELAPRECO` | TabelaPrecoModel | APITabelaPreco | idTabelaPreco |
| `TB_TABELAPRECOITEM` | TabelaPrecoItemModel | - | idTabelaPrecoItem |
| `TB_MOVIMENTOESTOQUE` | EstoqueModel | APIestoque | idMovimentoEstoqueMobile |
| `TB_LOCAL_ESTOQUE` | LocalEstoqueModel | - | idLocalEstoqueOffline |
| `TB_STATUS` | StatusModel | - | idStatus |
| `TB_TRANSPORTADORAS` | TransportadorasModel | APItransportadora | idTransportadora |
| `TB_RAMOATIVIDADE` | RamoAtividadeModel | - | idRamoAtividade |
| `TB_RECEBIMENTOTITULOS` | RecebimentoTitulosModel | ApiRecebimentoTitulos | idRecebimentoTituloOffLine |
| `TB_ATIVIDADES` | AtividadeAgendaModel | ApiAtividadeAgendaMobile | idAtividadeOffline |
| `TB_CONFIGURACOES_GERAIS` | ConfiguracaoGeralModel | - | idConfiguracaoGeral |
| `TB_OMIE_CONFIGURACOES_GERAIS` | OmieConfiguracaoGeralModel | - | idOmieConfigGeral |
| `TB_CONFIGURACOES_ESPECIFICAS` | ConfiguracaoEspecificaModel | - | idConfiguracaoEspecifica |
| `TB_EMPRESA` | EmpresaModel | - | idEmpresa |
| `TB_EMPRESA_ASPNETUSERS` | EmpresaAspnetUsersModel | - | idEmpresa_aspnetUsers |
| `TB_REGRAS_COMERCIAIS` | RegrasComerciaisModel | - | idRegraComercial |
| `TB_GRADES` | GradesModel | - | idGrade |
| `TB_GRADECOR` | GradeCorModel | - | idGradeCor |
| `TB_GRADETAMANHO` | GradeTamanhoModel | - | idGradeTamanho |
| `TB_ANEXOS` | AnexosModel | - | idAnexo |

---

## 9. Repositórios — Métodos Principais

### PedidoRepository (92KB — maior do projeto)
- `GetInfinit(skip, take, xFiltro, ...)` → Lista paginada de pedidos
- `SavePedidoVenda(model)` → Salvar pedido + itens
- `GetPedidoVendaModel(idOffLine)` → Pedido completo para edição
- `Delete(idOffLine, idOnline)` → Excluir pedido
- `DuplicarPedido(model)` → Duplicar pedido existente
- `GerarPedidoByOrcamentoNew(model)` → Converter orçamento em pedido
- `UpdateStatus(idOffLine, newStatus, oldStatus, stVenda, motivo)` → Mudar status
- `ValidarDescontoMaximo(model)` → Validar desconto máximo
- `GetAllPedidosToSync()` → Pedidos pendentes de sincronização
- `GetFaturamento(...)` → Dados de faturamento para dashboard

### ClienteRepository (29KB)
- `GetClienteModel(idOffLine, bFull)` → Cliente com endereços/contatos
- `Save(model)` → Salvar cliente
- `GetIdClienteNuvem(idOffLine)` → ID nuvem a partir do offline
- `GetValorLimiteCredito(idOffLine)` → Limite de crédito
- `CpfCnpjClienteJaExiste(valor, idClientes)` → Validar duplicidade

### ProdutoRepository (75KB)
- `Get(skip, take, xFiltro)` → Produtos paginados (retorna PedidoVendaItensModel)
- `GetProdutoToDisplay(idProdutoOffLine, idClienteOffLine)` → Produto para exibição na venda
- `GetGradeItem(item)` → Grades/variações do produto
- `ObterEstoqueProduto(idEmpresa, idProduto)` → Estoque disponível
- `GradeAtiva(idEmpresa, idProduto)` → Produto tem grades?

### TabelaPrecoRepository
- `GetValorProdutoTabelaPreco(idTabela, idProduto)` → Valor do produto na tabela
- `GetTabelaPrecoSimplificadas(...)` → Tabelas disponíveis para o contexto
- `GetTabelaPrecoParaPedidoVenda(idTabela)` → Tipo da tabela (auto/manual)
- `VerificaSeUtilizaEscalonada(...)` → Verifica tabela escalonada

### ConfiguracaoGeralRepositorio
- `GetConfiguracaoEmpresa()` → Configurações gerais da empresa
- `GetUtilizaPixOmieCash(idEmpresa)` → PIX Omie habilitado?
- `GetExibeNF(idEmpresa)` → Exibir info NF?
- `ValidaConfiguracoesGerais(model, condicao)` → Validar configs antes de salvar
- `ObterIdStatusAberto()` → ID do status padrão "aberto"

### FinanceiroRepository
- `ValidaLimiteCredito(...)` → Validar crédito do cliente
- `BuscarTitulosEmAberto(idPedido)` → Títulos em aberto
- `GetFaturas(idPedidoOffLine)` → Parcelas do pedido
- `SalvarFaturas(lista)` → Salvar parcelas manuais

### CondicaoPagamentoRepository
- `GetItem(idCondicao, idClienteOffLine)` → ListItemModel da condição
- `GetCondicaoCompleta(idCondicao)` → CondicaoPagamentoModel completo
- `BuscaFormasPagamentoPorCondicao(filtro, idCondicao)` → Formas de pagamento

---

## 10. App.xaml.cs — Propriedades Globais

| Propriedade | Tipo | Descrição |
|-------------|------|-----------|
| `CurrentAspnetUserModel` | AspNetUsersModel | Usuário logado |
| `tipouser` | TipoUser enum | OMIE, BLING, TINY, CAPOLI, NORMAL |
| `planoAtual` | Planos | Plano de assinatura |
| `Data` | DataAccess | Conexão SQLite |
| `Messages` | IMessageService | Diálogos/alertas |
| `UrlWebApi` | string | URL API principal |
| `UrlWebApiMobile` | string | URL API mobile |
| `UrlApiImage` | string | URL API imagens |
| `UrlReport` | string | URL relatórios |
| `UrlPortal` | string | URL portal pagamentos |
| `ForcarAtualizacao` | bool | Força atualização (UltimaSyncDateTime.Year < 2000) |
| `IsConected()` | Task<bool> | Verifica conectividade via Xamarin.Essentials |

### Empresa Atual
```csharp
App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa      // ID da empresa
App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.stAdministrador // É admin?
App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.idEmpresa_aspnetUsers // ID do representante
App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel.stAcessoTodosClientes // Acessa todos clientes?
```

---

## 11. Core Business Logic

### Motor de Preço (Core/PedidoVenda/)
- `BuscaPreco.Buscar()` — Busca preço por prioridade: campanha > específica > default > geral
- `BuscaPrecoRepositorio` + 22 repositórios especializados em `Model/Repository/BuscaPreco/`
- Suporta: tabela automática, tabela manual, escalonada, por cliente, por UF, por ramo

### Cálculos de Pedido (PedidoVendaCalculos)
- `CalculoValorUnitarioComImpostos()` — Valor + IPI + ST
- `CalculoValorSubTotal()` — Quantidade * Valor
- `CalculoValorComissao()` — Base comissão * percentual
- `CalculoDescontoPorPorcent()` / `CalculoDescontoPorValor()` — Desconto
- `AtualizaValores()` — Recalcula item completo

### Regras Comerciais (Model/RegrasComerciais/)
- Regras com faixas e critérios (desconto, comissão)
- Critérios: por cliente, produto, categoria, condição pgto, ramo, representada, tabela preço, UF
- Aplicadas em `ListarProdutosNewViewModel.AplicaRegrasComerciaisProduto()`

---

## 12. Checklist Final

Antes de finalizar qualquer alteração, verifique:
- [ ] Código segue naming conventions do projeto (português, prefixos Hungarian)
- [ ] Novos campos persistidos têm migration `ALTER TABLE` em `DataAccess.cs`
- [ ] Queries incluem filtro `idEmpresa` quando aplicável
- [ ] Error handling usa `ex.TrakException()`, não `Console.WriteLine`
- [ ] Navegação usa `UtilNavidate`, não `Navigation.PushAsync` diretamente
- [ ] API calls usam `UtilHttp`, não `HttpClient` direto
- [ ] Nenhum typo existente foi "corrigido" (ExecuttingAnyCommand, UtilNavidate, SearchCommom)
- [ ] Commit message no formato: `#ISSUE_ID N° commit descricao`

---

## 13. Base Web MVC — Referência Completa (d:\Backlog)

### Arquitetura Web

O sistema web (`Hlp.PedidoEletronico.Mvc`) é ASP.NET MVC 5 com Entity Framework, NInject para DI, e ~65 projetos na solution. Multi-tenant via `idEmpresa`.

### Tabelas Principais (SQL Server)

| Tabela | PK | Campos-chave para relatórios |
|--------|-----|------|
| `tb_pedidovenda` | idPedidoVenda | idEmpresa, idClientes, idRepresentantePedido, dEmissao, dtFaturamento, vSubTotal, stPedidoVenda (0=aberto, 1=cancelado, 2=vendido), stLancamento (0=orçamento, 1=pedido) |
| `tb_pedidovendaitens` | idPedidoVendaItem | idPedidoVenda, idProduto, idEmpresa, vSubTotal (valor item), vQtdItem, vUnitarioVenda, pDesconto, vDesconto, pComissao, vComissao |
| `tb_clientes` | idClientes | idEmpresa, xFantasia, xRazaoSocial, xCpfCnpj, dtCadastro, stAtivo, stProspeccao ("CE"=efetivado, "CP"=prospecção), vLimiteCredito, idEmpresa_aspnetUsers |
| `tb_produto` | idProduto | idEmpresa, xNome |
| `tb_empresa_aspnetusers` | idEmpresa_aspnetUsers | idEmpresa, xNome, xApelido, imUsuario, stAtivo, stAdministrador, vMetaFaturamento |
| `tb_empresa_aspnetusers_metas` | — | idEmpresa_aspnetUsers, idEmpresa, dtInicioMeta, vMeta |
| `tb_empresa` | idEmpresa | stDataRelatorios (0=dEmissao, 1=dtFaturamento) |
| `tb_configuracoes_gerais` | — | idEmpresa, bBloquearPedidoClienteComTituloVencido, bPermitirRepresentanteAprovarLimiteExcedente, bMostraRazaoSocial, etc. |
| `tb_notificacao` | idNotificacao | idEmpresaAspNetUsers, xNotificacao, xConteudoNotificacao, xHrefNotificacao, dtNotificacao, bVisualizado |
| `tb_git_releases` | — | TagName, Name, Body, Created |
| `tb_equipe_representantes` | — | idEquipe, idEmpresa_aspnetusers (hierarquia de representantes) |
| `tb_status` | idStatus | xStatus, xSigla, xCorStatus |
| `tb_recebimentotitulos` | — | Títulos/financeiro do cliente |

### Filtro Universal de Relatórios/Dashboard

TODA query de faturamento DEVE respeitar:

1. **`stDataRelatorios`** — `tb_empresa.stDataRelatorios`: se `!= 1` usa `dEmissao`, se `== 1` usa `dtFaturamento`
2. **Filtro de venda confirmada**: `stLancamento = 1 AND stPedidoVenda = 2`
3. **Multi-tenant**: `WHERE idEmpresa = @idEmpresa`
4. **Admin vs Rep**: Admin vê tudo, representante vê só dele + filhos hierarquia (`GetFilhosHierarquia()`)

```sql
-- Padrão SQL para faturamento (quando stDataRelatorios != 1):
WHERE pv.idEmpresa = @idEmpresa
  AND pv.stLancamento = 1 AND pv.stPedidoVenda = 2
  AND pv.dEmissao BETWEEN @dtInicial AND @dtFinal

-- Quando stDataRelatorios == 1:
WHERE pv.idEmpresa = @idEmpresa
  AND pv.stLancamento = 1 AND pv.stPedidoVenda = 2
  AND pv.dtFaturamento IS NOT NULL
  AND pv.dtFaturamento BETWEEN @dtInicial AND @dtFinal
```

### Controle de Acesso — Planos

```csharp
// Enum em BaseController.cs:
public enum SituacaoAcessoSistema { ok, vencido, plgratis, pldegustacao, errocomunicacao }

// Session:
Session["acessosistemapermitido"] // SituacaoAcessoSistema — plano do usuário
Session["isadmempresa"]           // bool — se é admin da empresa
Session["isomieempresa"]          // bool — se empresa usa Omie
Session["IsBlingEmpresa"]         // bool — se empresa usa Bling
Session["IsTinyEmpresa"]          // bool — se empresa usa Tiny
Session["iscigamempresa"]         // bool — se empresa usa Cigam
Session["ssForcaAlteracaoSenha"]  // bool — força alteração de senha
```

**Padrão Razor para verificar plano:**
```razor
@{
    var planoGratis = Hlp.PedidoEletronico.Mvc.Areas.Sistema.Controllers.SituacaoAcessoSistema.plgratis;
    var planoErro = Hlp.PedidoEletronico.Mvc.Areas.Sistema.Controllers.SituacaoAcessoSistema.errocomunicacao;
    var acessoPermitidoObj = Session["acessosistemapermitido"];
    var acessoPermitido = acessoPermitidoObj != null
        ? (Hlp.PedidoEletronico.Mvc.Areas.Sistema.Controllers.SituacaoAcessoSistema)acessoPermitidoObj
        : planoErro;
    bool bPlanoAtivo = (acessoPermitido != planoGratis && acessoPermitido != planoErro);
}
```

**Padrão Razor para verificar admin:**
```razor
@if (Session[name: "isadmempresa"] != null && (bool)Session[name: "isadmempresa"] == true)
```

### Repositórios de Relatórios (Mvc/Data/ImplementationRepository/Relatorios/)

| Repositório | Métodos-chave |
|------------|---------------|
| `ClientesReportRepository` | `GetGrafTop10()`, `GetCliMaiorPartMesAnterior()`, `GetFaturamentoTodosClientes()`, `GetGrafCliNovosExistentes()` |
| `VendasRepository` | Top10 produtos, ticket médio (`GetGrafTicketMedioParaHome()`), comissões |
| `MetasRepository` | `GetGrafTop10FatRepresentantes()`, `GetGrafMetasRankingRepresentantes()`, faturamento ano vs ano anterior |
| `RecebimentoTitulosRepository` | Títulos em aberto, atraso, período médio recebimento |
| `RelatorioMenusRepositorio` (Data project) | `ObterFaturamentoMensalAtual()`, `ObterFaturamentoMetaAtual()` |

### Dashboard Web — Arquivos

| Arquivo | Função |
|---------|--------|
| `Mvc/Areas/Sistema/Controllers/HomeSistemaController.cs` | Endpoints: `ObterDadosDashboard`, `ObterNotificacoes`, `MarcarNotificacoesLidas` |
| `Mvc/Areas/Sistema/ViewModels/HomeSistema/DashboardViewModel.cs` | Models: KPI, Ranking, Top10Cliente, Top10Produto, Carteira, FaturamentoMensal |
| `Mvc/Areas/Sistema/Views/HomeSistema/Home.cshtml` | View principal com KPIs, ranking, gráficos, ações rápidas, painel notificações |
| `Mvc/Scripts/PedidoEletronicoScripts/HomeSistema/jquery-dashboard.js` | Chart.js: faturamento mensal, top10 clientes (bar horizontal), top10 produtos (bar horizontal), carteira (doughnut) |
| `Mvc/Content/css/dashboard.css` | Estilos do dashboard |
| `Mvc/Scripts/Templates/chartjs/chart.umd.min.js` | Chart.js 4.4.0 local (não CDN) |

### Padrão de sum em faturamento

- **Header-level**: `SUM(tb_pedidovenda.vSubTotal)` — rápido, usado para totais
- **Item-level**: `SUM(tb_pedidovendaitens.vSubTotal)` — preciso, usado para breakdown por produto

### DTOs reutilizáveis existentes

| DTO | Namespace | Campos |
|-----|-----------|--------|
| `ClientesTop10` | Mvc/Data | idCliente, xCliente, valor, idRepresentantePedido |
| `MetasFaturamentoRepresentantes` | Mvc/Data | idRepresentante, xNomeRepresentante, vFaturado, vMeta, pAtingidaMeta |
| `ListagemClientesPorFaixaValorVendas` | Mvc/Data | idCliente, xCliente, vTotal, cAlternativo, stAtivo |
| `GrafHomeModel` | Mvc/Models | valorCliNovosExistentes, valorTicketMedio, lClientesTop3, lCampanhas |

### Duplicações conhecidas no projeto web

1. `Aplication.Bling` ≈ `Services.Integracoes.Bling` (mesmos métodos)
2. `ConsoleIntegracaoSisplan` ≈ `ConsoleSincronizacaoSisPlan` (60+ DTOs iguais)
3. `WebApiPeMobile` ≈ `WebApiMobilePe` (nomes invertidos)
4. `Support/Calculos/` ≈ `Business/Calculos/` (Pedido.cs e Produto.cs idênticos)
5. AccountController (8x), HomeController (11x), ValuesController (6x - código morto)
6. 3+ versões de `ArredondarValorDecimal`, `RemoveAcentos`, `RemoveSpecialCharacters`
