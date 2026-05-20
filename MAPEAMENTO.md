# MAPEAMENTO — AppPe (Pedido Eletrônico Mobile)

> Documento de referência para entender a arquitetura do projeto, padrões de código, fluxo de dados e regras de sincronização. **Manter este arquivo atualizado quando alterações estruturais forem feitas.**

---

## 1. Visão geral

App de **força de venda em campo** para representantes comerciais (sistema "Pedido Eletrônico" / `pedidoeletronico.com`), construído em **Xamarin.Forms 5** com targets Android e iOS. A operação é **offline-first**: o representante baixa toda a base do cliente/produto/preço, opera sem internet emitindo pedidos, e sincroniza quando há conexão.

- **Solução**: `C:\PeMobileXamarim\AppPe.sln`
- **Package Android**: `com.ptbr.pedidoeletronico`
- **Versão atual**: AndroidManifest `versionCode=353`, `versionName=15.0.103`
- **Min SDK**: 21 (Android 5.0) — **Target SDK**: 35 (Android 15)

---

## 2. Estrutura da solução

A `.sln` contém **3 projetos** + 1 pasta legada:

| Projeto | Caminho | Tipo | Função |
|---|---|---|---|
| `Xamarin.HLP.Mobile.AppPE` | [AppPe/AppPe/](AppPe/AppPe/) | netstandard2.0 | **Compartilhado** (Forms, MVVM, lógica) |
| `Xamarin.HLP.Mobile.AppPE.Droid` | [AppPe/AppPe.Android/](AppPe/AppPe.Android/) | Xamarin.Android | Target Android |
| `Xamarin.HLP.Mobile.AppPE.iOS` | [AppPe/AppPe.iOS/](AppPe/AppPe.iOS/) | Xamarin.iOS | Target iOS |
| ⚠️ `Xamarin.HLP.Mobile.AppPE.Droid` (legado) | [AppPe/Xamarin.HLP.Mobile.AppPE.Droid/](AppPe/Xamarin.HLP.Mobile.AppPE.Droid/) | — | **Cópia antiga** — não está no `.sln`, considerar remoção |

> **Atenção:** a pasta `AppPe/Xamarin.HLP.Mobile.AppPE.Droid` parece ser um snapshot anterior do projeto Android. Não editar ali — o projeto ativo é `AppPe/AppPe.Android`.

---

## 3. Stack & dependências

### Pacotes NuGet principais (projeto compartilhado)

| Pacote | Versão | Uso |
|---|---|---|
| `Xamarin.Forms` | 5.0.0.2662 | UI cross-platform |
| `Xamarin.Essentials` | 1.8.1 | Connectivity, permissões |
| `sqlite-net-pcl` | 1.9.172 | ORM/banco local |
| `Newtonsoft.Json` | 13.0.3 | Serialização REST |
| `Plugin.BLE` / `Plugin.BluetoothLE` | 3.1.0 / 6.3.0.19 | Impressora térmica via Bluetooth |
| `Plugin.Permissions` | 6.0.1 | Permissões runtime |
| `Xam.Plugin.Connectivity` | 3.2.0 | Detecção de queda de conexão |
| `Rg.Plugins.Popup` | 2.1.0 | Popups modais |
| `ZXing.Net.Mobile.Forms` | 2.4.1 | Leitura de barcode |
| `PdfSharpCore` | 1.3.65 | Geração de PDF (boletos/pedidos) |
| `Xamarin.Controls.SignaturePad.Forms` | 3.0.0 | Assinatura digital de pedido |
| `Xamarin.FFImageLoading.*` | 2.4.11.982 | Cache/loading de imagens + SVG |
| `CarouselView.FormsPlugin` | 6.0.0 | Carrossel de imagens |

### DLLs externas

- `dllExtern/Hlp.PedidoEletronico.Domain.Business.dll` — **lógica de negócio compartilhada com o backend** (cálculo de pedido, regras comerciais, planos). Veja `using Hlp.PedidoEletronico.Domain.Business.Bo;` no [App.xaml.cs](AppPe/AppPe/App.xaml.cs#L1).
- `dllExtern/TEditor.dll` — editor de texto rico.

---

## 4. Arquitetura — MVVM com Repository

```
┌──────────────────────────────────────────────────────────────┐
│  View (XAML) ◄─Binding─► ViewModel ◄─chama─► Repository      │
│                              │                    │          │
│                              ▼                    ▼          │
│                          UtilHttp          App.Data.Connection│
│                              │                    │          │
│                              ▼                    ▼          │
│                          API REST            SQLite local    │
└──────────────────────────────────────────────────────────────┘
```

### 4.1 Camadas no projeto compartilhado

| Pasta | Responsabilidade |
|---|---|
| [View/](AppPe/AppPe/View/) | `*.xaml` + `*.xaml.cs` — telas (ContentPage). Subdividida por feature: `Login`, `Home`, `Cliente`, `Pedido`, `Produto`, `Sincronizacao`, `Agenda`, `DashBoard`, `Empresa`, `ListaPreco`, `Popup`, etc. |
| [ViewModel/](AppPe/AppPe/ViewModel/) | Lógica de tela. Mesma divisão por feature. Herdam de [`ViewModelComum<T>`](AppPe/AppPe/Common/ViewModelComum.cs) ou [`SearchCommom`](AppPe/AppPe/Common/). |
| [Model/](AppPe/AppPe/Model/) | POCOs/DTOs com atributos `sqlite-net` (`[Table]`, `[PrimaryKey]`). |
| [Model/Repository/](AppPe/AppPe/Model/Repository/) | Acesso ao SQLite — classes **estáticas** com queries SQL e LINQ via `App.Data.Connection`. |
| [Common/](AppPe/AppPe/Common/) | Utilitários: `UtilHttp`, `UtilNavidate`, `UtilMessages`, `UtilMethods`, `TableMobile`, `ViewModelComum`, `ColorStaticModel`. |
| [Core/](AppPe/AppPe/Core/) | Lógica de negócio interna (criptografia, cálculo de PedidoVenda). |
| [Controls/](AppPe/AppPe/Controls/) | Controles customizados (`xaml/` para UserControls, `custom/` para Renderers). |
| [Abstractions/](AppPe/AppPe/Abstractions/), [Services/](AppPe/AppPe/Services/) | Interfaces para serviços nativos (`IConfig`, `IPicture`, `IVersion`, `IBluetoothLE`, `IEncoded`, `IbackButtonPressed`, `IBackgroundSyncService`). |
| [Images/](AppPe/AppPe/Images/) | SVGs e PNGs como `EmbeddedResource` (ver `.csproj`). |

### 4.2 ViewModel base — [`ViewModelComum<T>`](AppPe/AppPe/Common/ViewModelComum.cs)

Define o padrão MVVM:
- Implementa `INotifyPropertyChanged` com helper `NotifyPropertyChanged([CallerMemberName])`.
- Property `currentModel` (T) — model bindado à View.
- Property `IsBusy`, `Title`.
- `RegistrosAll` / `RegistrosResearched` / `RegistrosAgrupados` para listagens.
- Commands prontos: `NavigateToCommand`, `NavidateToBackCommand`, `CancelCommand`, `PesquisarCommand`.
- Helpers de imagens via `Device.OnPlatform(...)` (`ImageIconBack`, `ImageIconAdd`, `ImageIconSave`, etc.).

### 4.3 Convenção de nomes

- **Views**: `Page<Acao><Entidade>.xaml` (ex: `PageListarPedidos`, `PagePedidoNew`, `PageInfoCliente`).
- **ViewModels**: `<Entidade><Acao>ViewModel.cs` (ex: `PedidoNewViewModel`, `ListarProdutosNewViewModel`).
- **Models**: `<Entidade>Model.cs` (ex: `ClientesModel`, `PedidoVendaModel`, `EnderecoModel`).
- **Repositories**: `<Entidade>Repository.cs` — sempre **classe estática** (ex: `ClienteRepository`, `PedidoRepository`, `LoginRepository`).
- **Constantes de tabela**: em [`TableMobile`](AppPe/AppPe/Common/TableMobile.cs), `TB_<NOME>`.
- **PKs offline**: campos `id<Entidade>OffLine` (autoincrement local SQLite) **separado** do `id<Entidade>` (PK do servidor).

---

## 5. Fluxo de inicialização (Android)

[`MainActivity.OnCreate`](AppPe/AppPe.Android/MainActivity.cs#L29) →
1. Inicializa: `ImageCircleRenderer`, `CachedImageRenderer` (FFImageLoading), `Rg.Plugins.Popup`, `GAService`, `Xamarin.Essentials.Platform`, `Xamarin.Forms.Forms`, `TEditorDroid`.
2. Solicita permissões: `BLUETOOTH_CONNECT`, `POST_NOTIFICATIONS` (Android 13+).
3. Configura window insets, status bar (`#1565C0`), `KeepScreenOn`.
4. `LoadApplication(new App())`.

[`App.Iniciar`](AppPe/AppPe/App.xaml.cs#L211) →
1. Resolve serviços nativos via `DependencyService.Get<...>()`: `IMessageService`, `IPicture`, `IEncoded`, `IVersion`, `IBluetoothLE`, `IbackButtonPressed`.
2. Cria `Data = new DataAccess()` → abre conexão SQLite e roda `PrimeiraAnalise()` (`CreateTable<...>()` para todos os models).
3. Decide a tela inicial:
   - `LoginRepository.StatusBloqueio()` → `PageLogBloqueioSync`
   - Empresa do user inativa → tenta trocar para outra ativa, abre `RootPage`
   - `LoginRepository.HasLogin()` → `RootPage`
   - Senão → `PageBeforeLogin`

`MainApplication` apenas registra `IActivityLifecycleCallbacks` para o `Plugin.CurrentActivity`.

---

## 6. Banco local (SQLite)

- **Arquivo**: `DB_PEDIDOELETRONICO.db3` em `Environment.SpecialFolder.Personal` ([Config.cs](AppPe/AppPe.Android/Config.cs#L16)).
- **Conexão**: `App.Data.Connection` (`SQLiteConnection`, `ReadWrite | Create | FullMutex`, `storeDateTimeAsTicks=true`).
- **Criação de tabelas**: [`DataAccess.PrimeiraAnalise()`](AppPe/AppPe/DataAccess.cs#L40) — chama `CreateTable<TModel>()` para **todas** as classes anotadas, dentro de `try/catch` individuais (erro de uma tabela não derruba o resto). Nomes/listas em [`TableMobile`](AppPe/AppPe/Common/TableMobile.cs).
- **Migrações**: `sqlite-net-pcl` adiciona colunas novas automaticamente em `CreateTable`. Não há sistema de migration explícito — alterações destrutivas (drop/rename) precisam ser tratadas manualmente.

### Padrão de Repository

```csharp
// Exemplo: LoginRepository.cs
public static bool HasLogin()
{
    var icount = App.Data.Connection.ExecuteScalar<int>(
        $"select count(*) from {TableMobile.CurrentUserLogin} where bLogado = 1");
    return icount > 0;
}

public static void Save(ClientesModel model)
{
    if (model.idClientesOffLine == null)
        App.Data.Connection.Insert(model);
    else
        App.Data.Connection.Update(model);
}
```

- Classes estáticas, métodos estáticos.
- Mistura **SQL string interpolado** (queries complexas com JOIN) e **LINQ via `Table<T>()`** (acesso simples).
- `App.Data.Connection.Query<T>(sql)` retorna `List<T>` mapeada.
- `Execute(sql)` para INSERT/UPDATE/DELETE em massa.
- **Não há parametrização** (uso de `string` interpolada) — cuidado ao receber input externo (mas, sendo offline e por usuário autenticado, o risco prático é baixo).

### Models principais (entidades de negócio)

| Model | Tabela | Descrição |
|---|---|---|
| `AspNetUsersModel` | `AspNetUsers` | User/representante (importado do Identity .NET) |
| `EmpresaModel`, `EmpresaAspnetUsersModel` | `TB_EMPRESA`, `TB_EMPRESA_ASPNETUSERS` | Empresa contratante e vínculo user↔empresa |
| `ClientesModel` | `TB_CLIENTES` | Carteira de clientes |
| `EnderecoModel`, `ContatoModel` | `TB_ENDERECO`, `TB_CONTATOS` | Filhos de cliente |
| `ProdutoModel`, `CategoriaProdutoModel` | `TB_PRODUTO`, `TB_CATEGORIA` | Catálogo |
| `EstoqueModel`, `LocalEstoqueModel` | `TB_MOVIMENTOESTOQUE`, `TB_LOCAL_ESTOQUE` | Estoque (pago, plano não-grátis) |
| `TabelaPrecoModel`, `TabelaPrecoItemModel` | `TB_TABELAPRECO`, `TB_TABELAPRECOITEM` | Preços |
| `PedidoVendaModel`, `PedidoVendaItensModel` | `TB_PEDIDOVENDA`, `TB_PEDIDOVENDAITENS` | Pedidos (objeto principal) |
| `RecebimentoTitulosModel` | `TB_RECEBIMENTOTITULOS` | Financeiro |
| `RegrasComerciaisModel` (+ `RcFaixas`, `RcCriterios`, ...) | `TB_REGRAS_COMERCIAIS*` | Descontos/regras |
| `AtividadeAgendaModel` | `TB_ATIVIDADES` | CRM/visitas |

---

## 7. Backend / API

Tudo concentrado em [`UtilHttp`](AppPe/AppPe/Common/UtilHttp.cs) com **três `HttpClient` singletons**:

| Cliente | Base URL (Produção) | Uso |
|---|---|---|
| `CurrentHttpClient` | `App.UrlWebApi` = `https://pedidoeletronico.com/` | API legada principal (login, sync GET clássico, post pedidos) |
| `CurrentApiMobileHttpClient` | `App.UrlWebApiMobile` = `http://apimobile.pedidoeletronico.com/` | API mobile otimizada (paginação, contagem, agenda, estoque) |
| `CurrentApiImageHttpClient` | `App.UrlApiImage` = `http://apidataimage.pedidoeletronico.com/` | Upload/download de imagens e anexos |

### Ambientes

`App.AmbienteApp` (enum `Ambiente`) controla todas as URLs em [App.xaml.cs](AppPe/AppPe/App.xaml.cs#L88). Valores: `Producao`, `Homologacao`, `Local`, `HlpHom`, `HomologacaoProducao`. URLs adicionais: `UrlReport` (relatórios), `UrlPortal` (PagSeguro/portal de pagamentos).

> **Para mudar ambiente** ⇒ alterar `App.AmbienteApp = Ambiente.Homologacao;` em `App.xaml.cs`.

### Mapping Model ↔ API ↔ Tabela

[`TableMobile.GetInfoModel<T>(TipoRetornoInfoClass)`](AppPe/AppPe/Common/TableMobile.cs#L122) — **switch gigante** que devolve, dado um Model:
- `TableName` (ex.: `TB_PEDIDOVENDA`)
- `PrimaryKey` (ex.: `idPedidoVenda`)
- `ApiRegistro` (controller no servidor — ex.: `APIpedidoVenda`)

> **Convenção:** ao adicionar um Model novo, **sempre incluir o case correspondente em `TableMobile.GetInfoModel<T>`** e a chamada `CreateTable<TModel>()` em `DataAccess.PrimeiraAnalise()`.

### Métodos genéricos de IO

| Método | O que faz |
|---|---|
| `PostRegistroToCloud<T>(obj, "Post")` | POST genérico → `api/{controller}/{Post}` — retorna `RetornoSalvar<T>` |
| `GetListRegistroSync<T>(idEmpresa, dtUltimaAlteracao, ...)` | GET delta (apenas registros alterados após data) |
| `GetListRegistroPaginadoSync<T>(...)` | Versão paginada (Page=1..N) — usado em tabelas grandes |
| `GetRegistroSync<T>(idEmpresa, idPK)` | GET registro único |
| `GetPedidosVendas<T>`, `GetEstoque<T>`, `GetAtividadesAgenda<T>` | Endpoints específicos com paginação |
| `GetRegistroToRemoveSync<T>(date)` | Lista de exclusões a aplicar localmente |
| `DeleteAsync<T>(idPk)` | DELETE remoto |
| `GetDateServer()` | Sincroniza relógio (data/hora do servidor) |

### Tratamento de erro

Todo método trata 3 cenários:
1. `System.Net.WebException` → `SincronizacaoNewViewModel.bFalhaConexao = true` (silencioso, telinha de fim de sync trata).
2. Mensagem contém `"THE OPERATION WAS CANCELED"` ou `IsConected()` false → mesmo flag.
3. Outro `Exception` → `ex.TrakException(...)` (log via Google Insights / Application Insights).

### APIs externas usadas

- `https://www.receitaws.com.br/v1/cnpj/{cnpj}` — consulta CNPJ ao cadastrar cliente.
- `https://maps.googleapis.com/maps/api/geocode/json` — geocodificação de endereço (chave hardcoded em `UtilHttp.GetInfoEndereco`, **considerar movê-la**).
- `https://servicodados.ibge.gov.br/api/v1/localidades/municipios` — lista de cidades.

---

## 8. Sincronização

A peça crítica do app. Toda em [`SincronizacaoNewViewModel`](AppPe/AppPe/ViewModel/Sincronizacao/SincronizacaoNewViewModel.cs) (~2.660 linhas).

### Fluxo principal — `InitSyncComplete()`

```
1. AcessoPermitido() → valida plano do usuário no UrlPortal (PagSeguro)
2. Define App.planoAtual ∈ { plfree, plstarter, plsbus, plbus, plprem, pldeg, nenhum }
3. PermiteSincronizacao() → checa se representante está ativo
4. Se já sincronizou antes (lastDateServerSync.Year > 2000):
     a. InitSyncExclusaoUpload()   ← envia exclusões locais (deletes)
     b. InitExclusoesDownload()    ← aplica exclusões vindas do servidor
     c. UploadAll()                ← envia inserts/updates locais
5. InitSincronizacaoDownload()     ← baixa todas as tabelas (delta por data)
6. SyncAssnaturaPedido()           ← upload final de assinaturas pendentes
```

### Download — ordem topológica

`InitSincronizacaoDownload()` chama `SincronizacaoDownload<TModel>()` numa **ordem específica que respeita dependências FK** (Empresa → Status → Representada → Cliente → Produto → Pedido). Cada chamada:
1. `UtilHttp.GetListRegistroSync<T>(idEmpresa, ultimaSync, ...)` — busca delta.
2. Itera lista, faz `Insert` ou `Update` no SQLite.
3. `EnvironmentRepository.ExcluirRegistrosNecessarios(...)` — remove órfãos.

> **Padrão:** ao adicionar uma entidade nova ao sync, inserir a chamada `SincronizacaoDownload<MeuModel>()` em **ordem de dependência** dentro de `InitSincronizacaoDownload()`.

### Upload — `UploadAll()`

```csharp
await PostUpload(ClienteRepository.GetClientesModelsToSync());
await PostUpload(ContatoRepository.GetAllContatoModelsToSync());
await PostUpload(EnderecoRepository.GetAllEnderecoModelsToSync());
await PostUploadAgenda(AgendaRepository.GetAtividadeAgendaParaUploadModel());
await PostUploadAnexos(AnexosRepository.GetAnexosParaUploadModel());
await PostUpload(ProdutoRepository.GetAllToSync());
await PostUploadPedido();
```

Cada `Get<...>ToSync` retorna registros locais com `dtUltimaAlteracao > lastDateSync`. Após sucesso do POST:
- Atualiza `id<Entidade>` (PK do servidor) no registro local.
- Atualiza `dtUltimaAlteracao` para o `dtServer`.
- Casos especiais: pedido com **estoque insuficiente** (`RetornoSalvar.EstoqueInsuficiente`) salva mensagem em `xErroPedido` para exibir na tela.

### Sincronização em background (Android)

[`SincronizacaoService`](AppPe/AppPe.Android/Services/SincronizacaoService.cs) — **Foreground Service** com canal de notificação `sync_channel`:

```
[IBackgroundSyncService.StartSync()]
   ↓
[BackgroundSyncService_Android] StartForegroundService(SincronizacaoService)
   ↓
[SincronizacaoService.OnStartCommand]
   - Cria notification channel (Android 8+)
   - Posta notificação persistente "Sincronizando..."
   - Roda SincronizacaoNewViewModel.InitSyncComplete() em Task.Run
   - Hooks OnMensagemChanged/OnCountChanged → MessagingCenter para UI
   - StopForeground + StopSelf ao terminar
```

Configurado no [AndroidManifest.xml](AppPe/AppPe.Android/Properties/AndroidManifest.xml#L28) com `foregroundServiceType="dataSync"` (requisito Android 14+).

### Detecção de conectividade

- `App.IsConected()` ⇒ `Xamarin.Essentials.Connectivity.NetworkAccess == Internet`.
- `Plugin.Connectivity.CrossConnectivity.Current.ConnectivityChanged` ⇒ se cair durante sync, marca `bFalhaConexao = true` e aborta na próxima checagem.

---

## 9. Padrão Offline-First (chave!)

Cada entidade tem **duas chaves**:

| Coluna | Significado |
|---|---|
| `id<Entidade>OffLine` | PK local (autoincrement SQLite). Existe sempre, desde o momento da criação. |
| `id<Entidade>` | PK do servidor. Fica `null`/`0` até o registro ser sincronizado. |

### Fluxos:

- **Criar registro offline**: `Save(model)` → `Insert` → SQLite atribui `idClientesOffLine`. `idClientes` permanece `null`.
- **Modificar offline**: atualiza `dtUltimaAlteracao = DateTime.Now.ToUniversalTime()`. Esse campo é a "âncora" para o próximo sync detectar o que enviar.
- **Após upload**: API retorna `idClientes` (PK do servidor) → repository atualiza a linha local.
- **Pedido referencia cliente sem id online?** Antes do upload do pedido: `pedido.idClientes = ClienteRepository.GetIdClienteNuvem(pedido.idClientesOffLine)` resolve a referência.
- **Exclusão**: registrada em `TB_LOGEXCLUSAO` (`LogExclusaoModel`) e enviada no upload de exclusões.

### Regras de conexão na UI

- Telas que **exigem internet** (ex.: trocar de empresa) chamam `await App.IsConected()` antes de navegar (ex: [UtilNavidate logic](AppPe/AppPe/Common/UtilNavidate.cs)).
- A inicialização do app **não exige internet** — usa apenas o SQLite e o `LoginRepository`.

---

## 9.1 Mapa de telas financeiras (importante)

> Não há entrada de "Financeiro" no menu lateral. O acesso é sempre **contextual** (cliente ou pedido).

| Tela | Arquivo | Title | Origem da navegação |
|---|---|---|---|
| **Histórico financeiro / Títulos a receber** | [PageFinanceiroCliente.xaml](AppPe/AppPe/View/Pedido/PageFinanceiroCliente.xaml) | "Títulos a receber" | (1) Botão **"Histórico Financeiro"** em `PageApresentacaoClienteNew` ([`FinanceiroCommand`](AppPe/AppPe/ViewModel/Cadastro/ClienteApresentacaoNewViewModel.cs#L119)). (2) **Dentro do pedido**: botão **"Financeiro"** em [PagePedidoNew.xaml:126](AppPe/AppPe/View/Pedido/PagePedidoNew.xaml#L126) → [`GoToFinanceiroCommand`](AppPe/AppPe/ViewModel/Pedido/PedidoNewViewModel.cs#L816) → [`NavigateToFinanceiro`](AppPe/AppPe/ViewModel/Pedido/PedidoNewViewModel.cs#L939). |
| **Tabela de faturas (parcelas a gerar)** | [PageTabelaFaturas.xaml](AppPe/AppPe/View/Pedido/PageTabelaFaturas.xaml) | — | Botão dentro de `PagePedidoNew` ([`GoToFaturasCommand`](AppPe/AppPe/ViewModel/Pedido/PedidoNewViewModel.cs#L659)). Recebe `PedidoNewViewModel` no construtor — só funciona durante criação/edição. |
| **Bloco de fatura individual** | [PageFaturas.xaml](AppPe/AppPe/View/Pedido/PageFaturas.xaml) | — | Componente `StackLayout` instanciado dinamicamente dentro de `PageTabelaFaturas` (não é Page navegável). |

**Detalhe de UX importante:** o botão **"Financeiro"** dentro do pedido (`PagePedidoNew`) abre a tela "**Títulos a receber**" (`PageFinanceiroCliente`) — não os títulos só daquele pedido. A tela mostra **todos os títulos em aberto do cliente do pedido**, agregando duplicatas de todos os pedidos sincronizados (`FinanceiroRepository.BuscarTitulosEmAberto` filtra por `idClientesOffLine`, não por `idPedidoVenda`).

> **Não existe view dedicada a "títulos do pedido X".** Caso seja preciso, o `FinanceiroRepository` já tem `GetByIdPedidoVenda`, `GetFaturas` e `GetFaturasManual` (filtram por `idPedidoVenda`) prontos — falta apenas a View/ViewModel de consulta.

---

## 9.2 PIX Omie Cash (geração assistida pelo backend)

> O app **não fala com a Omie diretamente**. Ele só salva o flag `bGerarPix` no pedido e, depois do sync, lê o título com QR Code que o backend (web/`pe.backlog`) gerou via Omie Cash e gravou em `tb_recebimentotitulos`.

### Quando o checkbox "Gerar Pix" aparece

Em [PagePedidoNew.xaml](AppPe/AppPe/View/Pedido/PagePedidoNew.xaml), logo abaixo do controle de **Condição de Pagamento**. Visibilidade controlada por [`PedidoNewViewModel.ExibirCheckGerarPix`](AppPe/AppPe/ViewModel/Pedido/PedidoNewViewModel.cs):

```
App.tipouser == App.TipoUser.OMIE
  && OmieConfiguracaoGeralModel.bUtilizaPixOmieCash (empresa atual)
  && CondicaoPagamentoModel.nParcelas == 1 (da condição selecionada)
```

O `ViewCell` usa `Height` bindado a `HeightCellGerarPix` (50 quando exibe, 0 quando não) — assim a linha colapsa visualmente. Quando `ExibirCheckGerarPix` vira `false` (ex.: usuário troca pra cond. de mais parcelas), o setter de `ItemCondicaoPgto` zera `currentModel.bGerarPix` automaticamente.

### Botão "PIX »" na tela de detalhes

Em [PageDetalhesPedido.xaml](AppPe/AppPe/View/Pedido/PageDetalhesPedido.xaml), entre **ASSINATURA PEDIDO** e **MUDAR STATUS** (Grid.Row=1). Visibilidade controlada por [`DetalhesPedidoViewModel.ExibirBotaoPix`](AppPe/AppPe/ViewModel/Pedido/DetalhesPedidoViewModel.cs):

```
App.tipouser == App.TipoUser.OMIE
  && currentModel.bGerarPix
  && pixDisponivel != null  ← FinanceiroRepository.GetPixDisponivel(idPedido, idEmpresa)
```

`GetPixDisponivel` retorna o primeiro `RecebimentoTitulosModel` do pedido com `stPixPago=0` e `cCopiaCola/cQrCode/cUrlPix` preenchidos. É chamado em `Initialize()` e `RecarregaCurrentModel()` do VM.

### Popup de exibição

[PopupPixPedido](AppPe/AppPe/View/Popup/PopupPixPedido.xaml) (`PopupPage` do `Rg.Plugins.Popup`, aberto via `UtilNavidate.ShowPopupNew`):
- **QR Code**: `Image` com `Source="{Binding CQrCode, Converter={StaticResource Base64ToImageConverter}}"`. O converter (em [View/Converter/Base64ToImageConverter.cs](AppPe/AppPe/View/Converter/Base64ToImageConverter.cs)) aceita base64 puro ou data URI (`data:image/png;base64,...`).
- **Link de pagamento**: `Label` com `TapGestureRecognizer` → `Browser.OpenAsync(cUrlPix)`.
- **Copia‑e‑cola**: `Label` no Frame + botão "COPIAR CÓDIGO" → `Clipboard.SetTextAsync` + `App.Messages.ShowAsync("Código PIX copiado")`.

### Campos novos nos models

| Model | Campos adicionados |
|---|---|
| [PedidoVendaModel](AppPe/AppPe/Model/Lancamento/PedidoVendaModel.cs) | `bool bGerarPix` (sobe no upload normal de pedido) |
| [PedidoVendaListarModel](AppPe/AppPe/Model/Lancamento/PedidoVendaListarModel.cs) | `bool bGerarPix` — incluído também no `SELECT` de [`PedidoRepository.GetInfinit`](AppPe/AppPe/Model/Repository/PedidoRepository.cs#L22) |
| [RecebimentoTitulosModel](AppPe/AppPe/Model/Financeiro/RecebimentoTitulosModel.cs) | `string cCopiaCola, cQrCode, cUrlPix; bool stPixPago; long? nIdPixOmie` (apenas download — backend popula) |
| [OmieConfiguracaoGeralModel](AppPe/AppPe/Model/OmieConfiguracaoGeralModel.cs) | `bool bUtilizaPixOmieCash` |

`RecebimentoTitulosPostModel` **não** recebe os campos PIX — eles são populados pelo backend e descem via `SincronizacaoDownload<RecebimentoTitulosModel>`.

### Helpers novos

- [`ConfiguracaoGeralRepositorio.GetUtilizaPixOmieCash(idEmpresa)`](AppPe/AppPe/Model/Repository/ConfiguracaoGeralRepositorio.cs)
- [`CondicaoPagamentoRepository.GetNParcelas(idCondicaoPagamento)`](AppPe/AppPe/Model/Repository/CondicaoPagamentoRepository.cs)
- [`FinanceiroRepository.GetPixDisponivel(idPedidoVenda, idEmpresa)`](AppPe/AppPe/Model/Repository/FinanceiroRepository.cs)

### Detecção "empresa Omie"

Reutiliza o flag global `App.tipouser == App.TipoUser.OMIE`, setado em [`LoginRepository.RefreshTipoUsuario()`](AppPe/AppPe/Model/Repository/LoginRepository.cs#L10) com base em `EmpresaModel.xOmieAppKey`. **Não há fallback de Representada Omie no app** (o web tem; aqui não — `RepresentadaModel` não carrega `xOmieAppKey/xOmieAppSecret`).

---

## 10. Navegação

Tudo via [`UtilNavidate`](AppPe/AppPe/Common/UtilNavidate.cs):

| Método | Quando usar |
|---|---|
| `UtilNavidate.PushAsync(page)` | Navegação normal (push em `RootPage.Detail.Navigation`) |
| `UtilNavidate.PopAsync()` | Voltar |
| `UtilNavidate.PushModalAsync(page)` | Modal sobre o RootPage |
| `UtilNavidate.PushModalAsync2(page)` | Modal sobre `NavigationPage` (telas pré-login) |
| `UtilNavidate.ShowPopupNew(popupPage)` | Popup `Rg.Plugins.Popup` |
| `UtilNavidate.GoToHome()` | Reset para a Home |
| `UtilNavidate.Sincronizar(page)` | Inicia tela de sync com overlay |
| `UtilNavidate.Logoff()` / `EfetivarLogoff()` | Encerra sessão e volta para `PageBeforeLogin` |

### Estrutura raiz

`RootPage : MasterDetailPage` — menu lateral + área de conteúdo (`Detail`). `MainPage = new RootPage()` em estado logado; `MainPage = new NavigationPage(new PageBeforeLogin())` em estado deslogado.

---

## 11. Serviços nativos (DependencyService)

Padrão para acessar APIs nativas: definir interface no projeto compartilhado, implementar no Droid/iOS com `[assembly: Dependency(typeof(...))]`, resolver com `DependencyService.Get<I>()`.

| Interface | Implementação Android | Função |
|---|---|---|
| [`IConfig`](AppPe/AppPe/IConfig.cs) | [Config.cs](AppPe/AppPe.Android/Config.cs) | Caminho do diretório do banco |
| `IPicture` | `Picture_Droid.cs` | Salvar/recuperar imagens em disco |
| `IVersion` | `Version.cs` | Versão do app |
| `IBluetoothLE` | `Services/BluetoothLE.cs` (+ `BluetoothManager.cs`) | Pareamento e impressão térmica |
| `IEncoded` | `Encoded.cs` | Encoding helpers |
| `IbackButtonPressed` | `Services/backButtonPressed.cs` | Controle do botão voltar |
| `IBackgroundSyncService` | `Services/BackgroundSyncService_Android.cs` | Inicia o foreground sync |
| `IFileService` | `Services/FileService.cs` | Operações de arquivo |
| `IGAService` | `Services/GAService.cs` | Google Analytics |
| `IMessageService` | `View/Service/MessageService.cs` (compartilhado) | Alerts/Confirms |

### Custom Renderers (Android)

[AppPe/AppPe.Android/ExtendRender/](AppPe/AppPe.Android/ExtendRender/):
- `CustomWebViewRenderer.cs` — WebView com config customizada.
- `SvgImageRenderer.cs` — Render de SVGs (`Abstractions/SvgImage.cs`).

---

## 12. Logging / telemetria

- `GoogleInsightsReportingConstants.TrakException(origem, msg, isWarning)` — log centralizado para erros.
- `ex.TrakException("metodo", false)` (extension method) — atalho.
- `GoogleInsightsReportingConstants.TrakPage(InPage.<NOME>)` — em `OnAppearing` das pages.
- Crash handler nativo grava `Fatal.log` em `Environment.SpecialFolder.Personal` ([MainActivity:125](AppPe/AppPe.Android/MainActivity.cs#L125)) — exibido em diálogo na próxima abertura (apenas em `DEBUG`).

---

## 13. Convenções para manter o padrão

> Quando me pedir para adicionar/alterar algo, eu vou seguir estas regras. Se quiser quebrar alguma, me avise.

### Adicionar uma entidade nova (CRUD completo)

1. **Model** em `Model/<feature>/<Entidade>Model.cs` com atributos `[Table("TB_X")]`, `[PrimaryKey, AutoIncrement]` no `id<E>OffLine`, e campo `id<E>` (sem PK) para o id do servidor.
2. **Constante de tabela** em `TableMobile.cs` (`public const string TB_X = "TB_X";`).
3. **Case no `GetInfoModel<T>`** retornando `PrimaryKey`, `ApiRegistro`, `TableName`.
4. **`CreateTable<MeuModel>()`** em `DataAccess.PrimeiraAnalise()` (cada um no seu try/catch).
5. **Repository** estático em `Model/Repository/<Entidade>Repository.cs` com pelo menos `GetAll`, `GetById`, `Save`, `Delete`, e — se sincronizar — `Get<E>ModelsToSync()`.
6. **Sync** (se aplicável):
   - Adicionar `await SincronizacaoDownload<MeuModel>()` em `InitSincronizacaoDownload()` na ordem correta de dependência.
   - Adicionar `await PostUpload(<E>Repository.Get<E>ToSync())` em `UploadAll()`.
7. **ViewModel** em `ViewModel/<feature>/<Entidade>ViewModel.cs` herdando de `ViewModelComum<MeuModel>`.
8. **View** em `View/<feature>/Page<Acao><Entidade>.xaml(.cs)` com `BindingContext = new MeuViewModel()` ou via `ContentPage.BindingContext` no XAML.

### Acessar o banco

- ✅ Use `App.Data.Connection.Query<T>(sql)` ou `Table<T>().Where(...)`.
- ❌ Não crie nova `SQLiteConnection` — sempre usar a singleton em `App.Data`.

### Chamar API

- ✅ Use os métodos de `UtilHttp` (eles já tratam erro/conexão e atualizam `bFalhaConexao`).
- ❌ Não instancie `HttpClient` ad-hoc, exceto para serviços externos (CNPJ, IBGE, Google Maps).

### Navegar

- ✅ Use `UtilNavidate.PushAsync(new MinhaPage())`.
- ❌ Não chame `Application.Current.MainPage.Navigation.PushAsync(...)` direto.

### Strings de URL

- ✅ Use `App.UrlWebApi`, `App.UrlWebApiMobile`, `App.UrlApiImage`.
- ❌ Nunca hardcode `https://pedidoeletronico.com/...` no código de feature.

### Feedback ao usuário

- ✅ `await App.Messages.ShowAsync("mensagem")` para alert.
- ✅ `await App.Messages.ShowConfirmAsync("?")` para confirm (retorna `bool`).
- ❌ Não usar `DisplayAlert` direto da página (perde portabilidade da camada VM).

### Mexer em sync

- Toda alteração em `SincronizacaoNewViewModel` precisa preservar:
  - Setagem de `bFalhaConexao` / `ocorreuErro` em todos os caminhos de erro.
  - Flag `IsBusy` para evitar double-tap.
  - Chamada de `AnaliseFinalSincronizacao(...)` para fechar o ciclo (esconder loading).

---

## 14. Pontos de atenção / dívida técnica

- ⚠️ [`SincronizacaoHelper.cs`](AppPe/AppPe/SincronizacaoHelper.cs) — classe **vazia** (método `ExecutarSincronizacaoAsync` sem corpo). Toda lógica está em `SincronizacaoNewViewModel`. Pode ser removida ou refatorada.
- ⚠️ Pasta legada [Xamarin.HLP.Mobile.AppPE.Droid/](AppPe/Xamarin.HLP.Mobile.AppPE.Droid/) duplica o projeto Android — **não está no `.sln`**, mas confunde quem busca arquivos. Considerar remover.
- ⚠️ **Xamarin.Forms está em fim de vida** (suporte oficial encerrou em maio/2024). Nova versão Android 35 funciona, mas migração para .NET MAUI é uma evolução natural a planejar.
- ⚠️ Várias URLs HTTP (sem TLS) ainda em uso (`http://apimobile...`). Avaliar TLS em todos os endpoints para conformidade.
- ⚠️ Chave de Google Maps **hardcoded** em `UtilHttp.GetInfoEndereco`. Mover para configuração.
- ⚠️ Queries SQL via **string interpolada** em todos os repositories. Funciona pelo modelo offline-only, mas qualquer feature que receba input de uma tela externa precisa ter o input sanitizado antes.
- ⚠️ `CurrentApiImageHttpClient` (linha ~1207 de `UtilHttp.cs`) tem bug aparente: o getter atribui em `_currentApiMobileHttpClient` em vez de `_currentApiImageHttpClient`. Validar.
- ⚠️ `lastDateServerSyncCliente` (linha ~112 de `SincronizacaoNewViewModel`) tem getter/setter retornando o campo `_lastDateServerSync` (não o `_lastDateServerSyncCliente`). Provável bug de copy-paste.

---

## 15. Comandos úteis

### Build (no Visual Studio)

- Abrir `C:\PeMobileXamarim\AppPe.sln`.
- Setar `Xamarin.HLP.Mobile.AppPE.Droid` como Startup Project.
- Configuração `Debug|Any CPU` para emulador.
- `Release|Any CPU` para gerar APK.

### Banco local em runtime

- Localização: `/data/data/com.ptbr.pedidoeletronico/files/DB_PEDIDOELETRONICO.db3`
- Para extrair em dispositivo debug: `adb pull /data/data/com.ptbr.pedidoeletronico/files/DB_PEDIDOELETRONICO.db3 .`

### Limpar tudo (reset offline)

`EnvironmentRepository.ExcluirTodosRegistros()` — apaga dados do `idEmpresa` corrente. Útil para forçar `bForcarSyncInit`.

---

_Última atualização: 2026-05-01 — adicionada seção 9.2 (PIX Omie Cash)._
