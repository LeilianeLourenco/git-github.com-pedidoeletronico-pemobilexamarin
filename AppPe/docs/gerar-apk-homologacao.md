# Gerar APK da Sandbox apontando para Homologação

Guia para gerar um APK do **AppPe (Xamarin.Forms)** a partir da branch **sandbox**,
apontando para o ambiente de **homologação** (`homologacao.pedidoeletronico.com`)
em vez de produção (master).

> Máquina de referência: Windows 11, Visual Studio 2022 Community, JDK 11.

---

## 0. Pré-requisitos (verificados nesta máquina)

| Ferramenta | Caminho |
|-----------|---------|
| MSBuild (VS 2022) | `C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe` |
| Xamarin.Android | instalado (targets `MSBuild\Xamarin\Android\`) |
| **JDK 11** ⚠️ | `C:\Program Files\Microsoft\jdk-11.0.31.11-hotspot` (`JAVA_HOME` já aponta) |
| Android SDK | `C:\Program Files (x86)\Android\android-sdk` |
| Keystore de assinatura | `C:\Users\wesle\AppData\Local\Xamarin\Mono for Android\Keystore\pedidoeletronico - 4\pedidoeletronico.keystore` |
| Alias do keystore | `pedidoeletronico` |

> ⚠️ **JDK 11 é obrigatório.** JDK 8 quebra no `d8`/`aapt2`; JDK 17+ não é suportado
> pelo Xamarin clássico. Confirme com `echo %JAVA_HOME%` antes de buildar.

Projeto a compilar (o **.Droid** é quem gera o APK, não o projeto compartilhado):
`AppPe.Android\Xamarin.HLP.Mobile.AppPE.Droid.csproj`

---

## 1. Preparar a branch (baseado na sandbox)

O código deve sair da branch `sandbox`. A partir dela cria-se uma branch de publish
para não "sujar" a sandbox com o ajuste de ambiente:

```bash
cd d:\Xamarin\AppPe
git fetch origin
git checkout -b publish-homolog-android origin/sandbox
```

> Se você já está numa branch de publish montada a partir da sandbox
> (ex.: `master-publish`), pode buildar direto dela — só garanta que ela contém
> o conteúdo da sandbox que você quer testar.

---

## 2. Apontar o app para HOMOLOGAÇÃO

Ambiente é controlado por **uma única linha** em
[`AppPe/App.xaml.cs`](../AppPe/App.xaml.cs) (~linha 75):

```csharp
// ANTES (produção / master):
public static Ambiente AmbienteApp = Ambiente.Producao;

// DEPOIS (homologação = homologacao.pedidoeletronico.com):
public static Ambiente AmbienteApp = Ambiente.HomologacaoProducao;
```

### Para onde cada enum aponta

| Enum | UrlWebApi | UrlWebApiMobile |
|------|-----------|-----------------|
| `Producao` | `pedidoeletronico.com` | `apimobile.pedidoeletronico.com` |
| **`HomologacaoProducao`** ✅ | **`homologacao.pedidoeletronico.com`** | `prodhomapimobile.azurewebsites.net` |
| `Homologacao` | `homologacaope.azurewebsites.net` | `homologacaoapimobile.azurewebsites.net` |

> ⚠️ **Não confundir `Homologacao` com `HomologacaoProducao`.**
> "homologacaopedidoeletronico" = `homologacao.pedidoeletronico.com` = **`HomologacaoProducao`**.
> `Homologacao` aponta para o Azure (`homologacaope.azurewebsites.net`), que é outro servidor.
>
> Em `HomologacaoProducao`, `UrlApiImage`/`UrlReport`/`UrlPortal` continuam em **produção**
> (só Web e API Mobile vão para homologação).

**⚠️ Reverter depois:** essa linha NÃO pode ir para master/produção. Volte para
`Ambiente.Producao` antes de gerar o APK de produção.

---

## 3. (Opcional) Bump de versão

Só é necessário se o APK vai para a loja / substituir instalação existente.
Para teste interno de homologação pode pular. Em
`AppPe.Android\Properties\AndroidManifest.xml`:

```xml
<manifest ... android:versionCode="364" android:versionName="15.0.107" ... >
```

(`versionCode` deve ser **maior** que o instalado — atual: 363 / 15.0.106.)

---

## 4A. Gerar o APK pela interface do Visual Studio (recomendado)

Usa o keystore já configurado e lembra as senhas — não precisa digitar senha em linha de comando.

1. Abrir a solution `AppPe.sln` no VS 2022.
2. Barra de configuração: **Release** + **Any CPU**.
3. **Solution Explorer** → botão direito no projeto **`...AppPE.Droid`** → **Propriedades**
   → aba **Android Options** → marcar formato **APK** (não "bundle/AAB").
4. Menu **Build** → **Archive...** (arquiva o projeto Droid).
5. No **Archive Manager** que abre → **Distribute...** → **Ad Hoc**.
6. Selecionar a assinatura **`pedidoeletronico`** (ou importar o `.keystore` acima) → informar a senha.
7. **Save As** → salvar o `.apk` na pasta desejada.

> ⚠️ **Onde o APK assinado realmente cai (pegadinha).**
> Pelo Archive Manager o assinado **NÃO** vai para `bin\Release` e **NÃO** ganha sufixo `-Signed`.
> Ele fica dentro da pasta do archive, com o **mesmo nome** do não-assinado:
> ```
> %LOCALAPPDATA%\Xamarin\Mono for Android\Archives\<data>\<projeto> <data>.apkarchive\signed-apks\com.ptbr.pedidoeletronico.apk
> ```
> O `com.ptbr.pedidoeletronico.apk` na raiz do `.apkarchive` (e o de `bin\Release`) é o **não-assinado**.
> Use o que está em `signed-apks\`. Verifique a assinatura com:
> `& "C:\Program Files (x86)\Android\android-sdk\build-tools\35.0.0\apksigner.bat" verify --print-certs "<apk>"`
> (deve mostrar `CN=... O=HLP Estratégia em software`).

---

## 4B. Gerar o APK por linha de comando (MSBuild)

No **PowerShell** (a partir de `d:\Xamarin\AppPe`):

```powershell
$msbuild = "C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe"
$proj    = "d:\Xamarin\AppPe\AppPe.Android\Xamarin.HLP.Mobile.AppPE.Droid.csproj"
$ks      = "C:\Users\wesle\AppData\Local\Xamarin\Mono for Android\Keystore\pedidoeletronico - 4\pedidoeletronico.keystore"

# 1) Restaurar pacotes NuGet (só na 1ª vez ou após mudar dependências)
& $msbuild $proj /t:Restore

# 2) Build + assinatura do APK
& $msbuild $proj `
  /t:SignAndroidPackage `
  /p:Configuration=Release `
  /p:AndroidPackageFormat=apk `
  /p:AndroidKeyStore=true `
  /p:AndroidSigningKeyStore="$ks" `
  /p:AndroidSigningKeyAlias=pedidoeletronico `
  /p:AndroidSigningStorePass=SENHA_DO_KEYSTORE `
  /p:AndroidSigningKeyPass=SENHA_DA_CHAVE `
  /v:minimal
```

> Se o build não achar o SDK/JDK, adicione:
> `/p:AndroidSdkDirectory="C:\Program Files (x86)\Android\android-sdk"`
> `/p:JavaSdkDirectory="C:\Program Files\Microsoft\jdk-11.0.31.11-hotspot"`

### Saída

O APK assinado fica em:

```
AppPe.Android\bin\Release\com.ptbr.pedidoeletronico-Signed.apk
```

(o build também gera `com.ptbr.pedidoeletronico.apk` não assinado — use o **`-Signed`**.)

---

## 5. Instalar / validar

```powershell
& "C:\Program Files (x86)\Android\android-sdk\platform-tools\adb.exe" install -r `
  "d:\Xamarin\AppPe\AppPe.Android\bin\Release\com.ptbr.pedidoeletronico-Signed.apk"
```

Validação: abrir o app, logar e confirmar (via tela Sobre / logs de sync) que as chamadas
estão indo para `homologacao.pedidoeletronico.com`, não para produção.

---

## Checklist rápido

- [ ] Branch criada a partir de `origin/sandbox`
- [ ] `App.xaml.cs` → `AmbienteApp = Ambiente.HomologacaoProducao`
- [ ] (Opcional) `versionCode` incrementado no `AndroidManifest.xml`
- [ ] `JAVA_HOME` = JDK **11**
- [ ] Build **Release** do projeto **.Droid** com `/t:SignAndroidPackage`
- [ ] APK `-Signed` gerado em `bin\Release`
- [ ] Testado que o tráfego vai para homologação
- [ ] **Reverter `AmbienteApp` para `Producao`** antes de qualquer build de produção
