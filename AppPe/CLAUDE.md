# AppPe - Pedido Eletrônico (pedidoeletronico.com)

Sistema de pedido eletrônico para representantes comerciais brasileiros.
Xamarin.Forms 5.0 | NetStandard 2.0 | Android 13 + iOS
Namespace raiz: `Xamarin.HLP.Mobile.AppPE`

## Estrutura da Solution

- `AppPe/` — Projeto shared (core logic, ViewModels, Models, Views, Common)
- `AppPe.Android/` — Projeto Android **ativo**
- `AppPe.iOS/` — Projeto iOS
- `Xamarin.HLP.Mobile.AppPE.Droid/` — Projeto Android **legado** (NÃO modificar sem pedido explícito)

## Convenções de Nomenclatura

- Todo código usa **português** (variáveis, comentários, commits)
- Prefixos Hungarian-style:
  - `x` = string, `b` = bool, `st` = status/byte, `id` = identificador
  - `d` = DateTime, `v` = decimal/valor, `l` = lista, `i` = int, `obj` = objeto
- Sufixos: `*Model`, `*Repository`, `*ViewModel`, `Page*` (páginas)
- Tabelas SQLite: constantes `TB_*` em `TableMobile.cs`

## Convenções de Git

- Branch: `#ISSUE_NUMBER` (ex: `#5452`)
- Commit: `#ISSUE_ID N° commit descricao_em_portugues`
  - Exemplo: `#5452 1° commit exibindo xDisplayIntegracao na listagem de pedidos`
  - N é o ordinal do commit na branch (1°, 2°, 3°...)
- Mensagens de commit SEMPRE em português

## Regras Críticas

- **Nunca adicionar Dependency Injection** — o projeto inteiro usa padrões estáticos
- **Nunca atualizar Xamarin.Forms ou NuGet** sem pedido explícito do usuário
- **API calls** exclusivamente via `UtilHttp` (classe estática em `Common/UtilHttp.cs`)
- **Navegação** exclusivamente via `UtilNavidate` (Common/UtilNavidate.cs)
- **Mensagens ao usuário** via `App.Messages` (IMessageService)
- **Serialização JSON** via `Newtonsoft.Json` (nunca System.Text.Json)
- **Culture** sempre `pt-BR` para moeda, datas e números
- **NÃO corrigir typos em nomes existentes**: `ExecuttingAnyCommand`, `UtilNavidate`, `SearchCommom`, `NavidateToBackCommand` — são nomes consolidados no projeto

## URLs de API (definidas em App.xaml.cs)

- `App.UrlWebApi` — API principal
- `App.UrlWebApiMobile` — API mobile
- `App.UrlApiImage` — API de imagens
- `App.UrlReport` — Relatórios
- `App.UrlPortal` — Portal de pagamentos

## Contexto Global

- Usuário logado: `App.CurrentAspnetUserModel`
- Empresa atual: `App.CurrentAspnetUserModel.objEmpresaAspnetUsersModel`
- Banco local: `App.Data.Connection` (SQLiteConnection síncrono)
- Tipo de integração: `App.tipouser` (NORMAL, OMIE, BLING, TINY, CAPOLI)
- Conectividade: `await App.IsConected()`
