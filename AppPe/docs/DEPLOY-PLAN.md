# Planejamento de Deploy — AppPe (Android + iOS)

## Situação Atual do Projeto

| Item | Android | iOS |
|------|---------|-----|
| Package/Bundle ID | `com.ptbr.pedidoeletronico` | `com.ptbr.pedidoeletronico` |
| Versão atual | 15.0.103 (versionCode 353) | 19.79 |
| Min SDK / OS | API 21 (Android 5.0) | iOS 8.1 (desatualizado) |
| Target SDK | API 35 (Android 15) | SDK 17.5 |
| Arquiteturas | armeabi-v7a, x86, x86_64, arm64-v8a | ARM64 |
| Signing | **Sem keystore no repo** | Apple Distribution: H.L.P. MOBILE SERVICE LTDA - ME (B79L5K3424) |
| Provisioning | N/A | `AppDist` (Release) / `VS: WildCard Development` (Debug) |
| Build IPA/APK | EmbedAssembliesIntoApk: true | BuildIpa: true (Release) |
| Linker | None (sem otimização) | None |
| CI/CD | **Não configurado** | **Não configurado** |
| Firebase/FCM | **Não configurado** | **Não configurado** |
| Splash | SplashActivity.cs + splash_screen.xml | LaunchScreen.storyboard |
| Ícones | mipmap-hdpi a xxxhdpi + launcher_foreground | Assets.xcassets/AppIcon.appiconset (18 tamanhos) |

---

## PARTE 1 — DEPLOY ANDROID (Google Play)

### Recursos Necessários

| Recurso | Status | Ação |
|---------|--------|------|
| Conta Google Play Console | Verificar se já existe | Taxa única US$25 |
| Keystore (.jks ou .keystore) | **NÃO encontrado no repo** | Criar ou localizar existente |
| Senha do keystore + alias | **Necessário** | Definir e guardar em local seguro |
| Visual Studio 2022 (Windows) | Necessário | Com workload Xamarin instalado |
| Android SDK API 35 | Necessário | Instalar via SDK Manager |
| Ficha da loja (textos, screenshots) | Verificar se já existe no Play Console | Criar se necessário |
| Política de privacidade (URL) | **Obrigatório pelo Google** | Publicar se não existir |

### Passo a Passo — Android

#### Fase 1: Preparação (estimativa: 1-2 horas)

**1.1 — Localizar ou criar o Keystore**
- Verificar se já existe um keystore usado em deploys anteriores (versionCode 353 indica que o app JÁ foi publicado)
- Se perdido, será necessário usar o Play App Signing (upload key) para continuar atualizando
- Se for criar novo (app novo):
  ```
  keytool -genkey -v -keystore pedidoeletronico.keystore -alias pedidoeletronico -keyalg RSA -keysize 2048 -validity 10000
  ```
- **CRÍTICO**: Guardar keystore, senha e alias em local seguro. Perder = impossível atualizar o app

**1.2 — Verificar/Atualizar versão**
- Arquivo: `AppPe.Android/Properties/AndroidManifest.xml`
- Incrementar `android:versionCode` (atualmente 353 → 354)
- Atualizar `android:versionName` conforme necessário (ex: 15.0.104)

**1.3 — Verificar configurações de Release**
- Arquivo: `AppPe.Android/Xamarin.HLP.Mobile.AppPE.Droid.csproj`
- Confirmar que Release config tem:
  - `AndroidUseSharedRuntime` = false
  - `EmbedAssembliesIntoApk` = true
  - `DebugSymbols` = false

**1.4 — Verificar permissões no AndroidManifest.xml**
- Remover permissões não utilizadas (Google rejeita permissões desnecessárias)
- Permissões sensíveis (SMS, Call Log, Location) precisam de justificativa no Play Console

#### Fase 2: Build Release (estimativa: 30-60 min)

**2.1 — Configurar signing no .csproj (ou via Visual Studio)**
Adicionar na configuração Release do .csproj:
```xml
<AndroidKeyStore>true</AndroidKeyStore>
<AndroidSigningKeyStore>pedidoeletronico.keystore</AndroidSigningKeyStore>
<AndroidSigningStorePass>SUA_SENHA</AndroidSigningStorePass>
<AndroidSigningKeyAlias>pedidoeletronico</AndroidSigningKeyAlias>
<AndroidSigningKeyPass>SUA_SENHA</AndroidSigningKeyPass>
```
Ou via Visual Studio: Project Properties → Android Package Signing

**2.2 — Gerar AAB (Android App Bundle) — formato obrigatório desde 2021**
- Visual Studio: Build → Archive → Distribute → Google Play
- Ou via CLI:
  ```
  msbuild /t:SignAndroidPackage /p:Configuration=Release /p:AndroidPackageFormat=aab
  ```
- Output: `bin/Release/com.ptbr.pedidoeletronico-Signed.aab`

**2.3 — Testar o AAB localmente**
- Usar `bundletool` para gerar APKs a partir do AAB e testar em dispositivo:
  ```
  bundletool build-apks --bundle=app.aab --output=app.apks --ks=pedidoeletronico.keystore
  bundletool install-apks --apks=app.apks
  ```

#### Fase 3: Upload Google Play (estimativa: 1-2 horas primeira vez, 30 min atualizações)

**3.1 — Criar/acessar o app no Google Play Console**
- URL: https://play.google.com/console
- Se app novo: Create App → preencher detalhes (nome, idioma, tipo)

**3.2 — Configurar ficha da loja**
- Título: `pedidoeletronico.com`
- Descrição curta (80 chars) e completa (4000 chars) em português
- Screenshots: mínimo 2 (phone), recomendado 8
  - Tamanho: 1080x1920 (portrait) ou 1920x1080 (landscape)
- Ícone da loja: 512x512 PNG
- Feature graphic: 1024x500 PNG
- Categoria: Business
- Classificação etária: preencher questionário IARC

**3.3 — Configurar requisitos da loja**
- Política de privacidade: URL obrigatória
- Declaração de permissões: justificar cada permissão sensível
- Anúncios: declarar se o app contém anúncios
- Target audience: declarar faixa etária
- Data safety: preencher formulário de segurança de dados

**3.4 — Upload do AAB**
- Production → Create new release
- Upload o arquivo .aab
- Adicionar release notes em português
- Review → Start rollout to production

**3.5 — Revisão do Google (1-7 dias)**
- Primeira publicação: pode levar até 7 dias
- Atualizações: geralmente 1-3 dias
- Se rejeitado: corrigir issues e resubmeter

#### Fase 4: Otimizações Recomendadas (opcional, 2-4 horas)

- **Habilitar Linker**: Mudar `AndroidLinkMode` de `None` para `SdkOnly` para reduzir tamanho do APK
- **Remover x86/x86_64**: Se não precisa suportar emuladores, remover ABIs desnecessárias
- **Habilitar ProGuard/R8**: Ofuscar e otimizar código
- **Play App Signing**: Migrar para Google gerenciar a signing key (recomendado)
- **App Bundle**: Já é o formato requerido, mas confirmar que está gerando .aab e não .apk

---

## PARTE 2 — DEPLOY iOS (App Store)

### Recursos Necessários

| Recurso | Status | Ação |
|---------|--------|------|
| Apple Developer Account | Existe (H.L.P. MOBILE SERVICE LTDA - ME) | Renovação anual US$99 |
| Mac com macOS + Xcode | **Obrigatório** para build iOS | Xcode 15+ recomendado |
| Certificado Distribution | Existe (B79L5K3424) | Verificar validade |
| Provisioning Profile | `AppDist` configurado | Verificar se está atualizado |
| Visual Studio for Mac ou VS + Mac conectado | Necessário | Configurar build remoto se usando Windows |
| App Store Connect | Verificar se app já existe | Configurar se necessário |
| Política de privacidade (URL) | **Obrigatório pela Apple** | Mesma URL do Android |

### Passo a Passo — iOS

#### Fase 1: Preparação (estimativa: 2-4 horas)

**1.1 — Verificar certificados e provisioning profiles**
- Acessar: https://developer.apple.com/account
- Verificar se certificado `Apple Distribution: H.L.P. MOBILE SERVICE LTDA - ME (B79L5K3424)` está válido
- Verificar se provisioning profile `AppDist` inclui o bundle ID `com.ptbr.pedidoeletronico`
- Se expirados: renovar via Xcode ou portal Apple Developer

**1.2 — CRÍTICO: Atualizar MinimumOSVersion**
- Arquivo: `AppPe.iOS/Info.plist`
- Atualmente: `8.1` — **Apple exige iOS 16.0+ para novos envios (2024/2025)**
- Atualizar para pelo menos `16.0` (recomendado: `16.0`)
- **Impacto**: Usuários com iOS < 16 não poderão instalar/atualizar

**1.3 — Verificar/Atualizar versão**
- Arquivo: `AppPe.iOS/Info.plist`
- `CFBundleShortVersionString`: versão visível (ex: 19.80)
- `CFBundleVersion`: build number — deve ser MAIOR que o anterior (atualmente 19.79)
- **Ambos devem ser incrementados a cada envio**

**1.4 — Verificar App Transport Security**
- Atualmente: `NSAllowsArbitraryLoads = true` (permite HTTP)
- Apple pode rejeitar apps com HTTP sem justificativa
- Idealmente migrar APIs para HTTPS ou adicionar exceções específicas:
  ```xml
  <key>NSExceptionDomains</key>
  <dict>
    <key>pedidoeletronico.com</key>
    <dict>
      <key>NSExceptionAllowsInsecureHTTPLoads</key>
      <true/>
    </dict>
  </dict>
  ```

**1.5 — Verificar permissões (Usage Descriptions)**
- Arquivo: `Info.plist`
- Já configurados: Camera, Photo Library, Calendar, Location
- Verificar se as descrições estão em **português** (idioma principal do app)
- Apple rejeita se a descrição não justificar claramente o uso

#### Fase 2: Build Release (estimativa: 1-2 horas)

**2.1 — Configurar build no Mac**
- Conectar Visual Studio (Windows) ao Mac via remote build, ou usar VS for Mac
- Selecionar configuração: `Release | iPhone`
- Verificar que o .csproj tem:
  - `CodesignKey` = Apple Distribution certificate
  - `CodesignProvision` = AppDist
  - `BuildIpa` = true

**2.2 — Gerar o arquivo IPA**
- Visual Studio: Build → Archive → Distribute → App Store
- O arquivo .ipa será gerado em `bin/iPhone/Release/`

**2.3 — Validar o IPA antes do upload**
- No Mac: Abrir Xcode → Window → Organizer → Archives
- Ou usar `xcrun altool --validate-app`
- Verificar warnings de API deprecadas, permissões, etc.

#### Fase 3: Upload App Store (estimativa: 2-4 horas primeira vez, 1 hora atualizações)

**3.1 — Criar/acessar o app no App Store Connect**
- URL: https://appstoreconnect.apple.com
- Se app novo: My Apps → + → New App
  - Bundle ID: `com.ptbr.pedidoeletronico`
  - Nome: `pedidoeletronico.com`
  - Idioma principal: Português (Brasil)

**3.2 — Upload do IPA**
- Via Xcode: Organizer → Upload to App Store
- Via CLI: `xcrun altool --upload-app -f arquivo.ipa -u APPLE_ID -p APP_SPECIFIC_PASSWORD`
- Via Transporter (app da Apple)

**3.3 — Configurar ficha da loja**
- Screenshots obrigatórios:
  - iPhone 6.7" (1290x2796) — iPhone 15 Pro Max
  - iPhone 6.5" (1284x2778) — iPhone 11 Pro Max
  - iPad 12.9" (2048x2732) — se suportar iPad
- Textos: descrição, keywords, what's new (em português)
- Categoria: Business
- Classificação etária: preencher questionário
- Política de privacidade: URL obrigatória
- App Privacy: preencher formulário de dados coletados

**3.4 — Submeter para revisão**
- Version Information → preencher "What's New"
- Build → selecionar o build uploaded
- Submit for Review

**3.5 — Revisão da Apple (1-3 dias)**
- Primeira publicação: pode levar 3-7 dias
- Atualizações: geralmente 24-48 horas
- Apple é mais rigorosa que Google — razões comuns de rejeição:
  - HTTP sem justificativa (ATS)
  - Permissões sem uso real no app
  - Crash em funcionalidade básica
  - UI não adaptada para iPad (se universal)
  - Minimum deployment target desatualizado

---

## PARTE 3 — CRONOGRAMA CONSOLIDADO

### Primeira Publicação (app nunca publicado)

| Etapa | Android | iOS | Paralelo? |
|-------|---------|-----|-----------|
| Preparar contas (Play Console / Apple Developer) | 1-2h | 1-2h | Sim |
| Criar keystore / verificar certificados | 30min | 1h | Sim |
| Ajustar configs de versão e build | 1h | 2h (MinOS, ATS) | Sim |
| Build Release | 30-60min | 1-2h | Sim |
| Testar build em dispositivo real | 1-2h | 1-2h | Sim |
| Preparar ficha da loja (textos, screenshots) | 2-4h | 2-4h | Sim (mesmos assets) |
| Upload e configurar no console | 1-2h | 2-4h | Sim |
| Aguardar revisão | 1-7 dias | 1-7 dias | Sim |
| **Total trabalho ativo** | **~6-11h** | **~8-15h** | |
| **Total com revisão** | **2-8 dias** | **2-8 dias** | |

### Atualizações Subsequentes

| Etapa | Android | iOS |
|-------|---------|-----|
| Incrementar versão | 5min | 5min |
| Build Release | 30min | 1h |
| Testar em dispositivo | 30min | 30min |
| Upload + release notes | 15min | 30min |
| Revisão | 1-3 dias | 1-3 dias |
| **Total trabalho ativo** | **~1.5h** | **~2h** |

---

## PARTE 4 — PROBLEMAS IDENTIFICADOS E AÇÕES RECOMENDADAS

### Críticos (bloquantes para deploy)

| # | Problema | Plataforma | Ação |
|---|---------|-----------|------|
| 1 | Keystore não encontrado no repo | Android | Localizar keystore usado nos builds anteriores ou criar novo se app nunca foi publicado |
| 2 | MinimumOSVersion 8.1 | iOS | Atualizar para 16.0 em Info.plist — Apple rejeita iOS < 16 |
| 3 | NSAllowsArbitraryLoads = true | iOS | Adicionar exceções de domínio específicas ou migrar APIs para HTTPS |
| 4 | Sem CI/CD configurado | Ambos | Build manual funciona, mas CI/CD é recomendado para consistência |

### Importantes (podem causar rejeição)

| # | Problema | Plataforma | Ação |
|---|---------|-----------|------|
| 5 | APIs usando HTTP (não HTTPS) | Ambos | Google e Apple pressionam por HTTPS — migrar quando possível |
| 6 | Permissões excessivas no AndroidManifest | Android | Auditar e remover não utilizadas |
| 7 | Entitlements.plist vazio | iOS | Verificar se precisa de capabilities (push notifications, etc.) |
| 8 | Linker desabilitado (APK/IPA grande) | Ambos | Habilitar SdkOnly para reduzir tamanho |
| 9 | 4 ABIs no Android | Android | Considerar remover x86/x86_64 para reduzir tamanho |
| 10 | Descrições de permissão em inglês | iOS | Traduzir para português no Info.plist |

### Recomendados (melhores práticas)

| # | Melhoria | Impacto |
|---|---------|---------|
| 11 | Configurar Play App Signing | Segurança da signing key gerenciada pelo Google |
| 12 | Adicionar CI/CD (Azure DevOps / GitHub Actions) | Builds automatizados e consistentes |
| 13 | Implementar versionamento automático | Evitar erros manuais de versão |
| 14 | Configurar Firebase Crashlytics | Monitoramento de crashes em produção |
| 15 | Adicionar Push Notifications (FCM) | Engajamento com usuários |

---

## PARTE 5 — CHECKLIST PRÉ-DEPLOY

### Android
- [ ] Keystore localizado/criado e senha documentada
- [ ] versionCode incrementado no AndroidManifest.xml
- [ ] versionName atualizado no AndroidManifest.xml
- [ ] Build em modo Release sem erros
- [ ] AAB assinado gerado com sucesso
- [ ] App testado em dispositivo físico (não emulador)
- [ ] Permissões auditadas (remover não utilizadas)
- [ ] Ficha da loja preenchida (textos + screenshots)
- [ ] Política de privacidade publicada com URL válida
- [ ] Formulário de segurança de dados preenchido

### iOS
- [ ] Certificado Distribution válido e instalado no Mac
- [ ] Provisioning profile atualizado e baixado
- [ ] MinimumOSVersion atualizado para 16.0+
- [ ] CFBundleVersion e CFBundleShortVersionString incrementados
- [ ] App Transport Security configurado corretamente
- [ ] Descrições de permissão em português
- [ ] Build em modo Release|iPhone sem erros
- [ ] IPA validado via Xcode/altool
- [ ] App testado em dispositivo físico
- [ ] Ficha da loja preenchida (textos + screenshots)
- [ ] App Privacy form preenchido no App Store Connect
- [ ] Política de privacidade publicada com URL válida
