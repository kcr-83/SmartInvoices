# Instrukcje dla GitHub Copilot: Centralne zarządzanie projektami .NET

## 1. Centralne zarządzanie właściwościami projektu (Directory.Build.props)

### Struktura pliku Directory.Build.props

```xml
<Project>
  <!-- Wspólne właściwości dla wszystkich projektów -->
  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <LangVersion>latest</LangVersion>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
    <TreatWarningsAsErrors>true</TreatWarningsAsErrors>
    <EnforceCodeStyleInBuild>true</EnforceCodeStyleInBuild>
    <!-- Wspólne metadane projektu -->
    <Authors>NaszaFirma</Authors>
    <Company>NaszaFirma Sp. z o.o.</Company>
    <Copyright>Copyright © NaszaFirma $([System.DateTime]::Now.ToString('yyyy'))</Copyright>
    <RepositoryUrl>https://github.com/nasza-firma/nazwa-repo</RepositoryUrl>
  </PropertyGroup>

  <!-- Wspólne pakiety analizy kodu -->
  <ItemGroup>
    <PackageReference Include="Microsoft.CodeAnalysis.NetAnalyzers" Version="$(MicrosoftCodeAnalysisNetAnalyzersVersion)" PrivateAssets="all" />
    <PackageReference Include="StyleCop.Analyzers" Version="$(StyleCopAnalyzersVersion)" PrivateAssets="all" />
    <PackageReference Include="Roslynator.Analyzers" Version="$(RoslynatorAnalyzersVersion)" PrivateAssets="all" />
  </ItemGroup>

  <!-- Warunki specyficzne dla typów projektów -->
  <PropertyGroup Condition="$(MSBuildProjectName.EndsWith('.Tests'))">
    <IsPackable>false</IsPackable>
    <IsTestProject>true</IsTestProject>
  </PropertyGroup>

  <!-- Import ustawień specyficznych dla typów projektów -->
  <Import Project="$(MSBuildThisFileDirectory)build\Test.Directory.Build.props" Condition="$(MSBuildProjectName.EndsWith('.Tests'))" />
  <Import Project="$(MSBuildThisFileDirectory)build\Api.Directory.Build.props" Condition="$(MSBuildProjectName.EndsWith('.Api'))" />
</Project>
```

### Zasady użycia Directory.Build.props:

1. Zawsze używaj centralnego pliku `Directory.Build.props` dla wspólnych ustawień projektów.
2. Nie definiuj ponownie właściwości w plikach `.csproj`, które są już określone w `Directory.Build.props`.
3. Dla warunków specyficznych dla określonych typów projektów, używaj warunków w `Directory.Build.props` lub twórz pliki dziedziczące.
4. Wszystkie wersje frameworków, standardy kodu, ostrzeżenia i metadane powinny być zdefiniowane centralnie.

## 2. Centralne zarządzanie pakietami NuGet (Directory.Packages.props)

### Struktura pliku Directory.Packages.props

```xml
<Project>
  <PropertyGroup>
    <ManagePackageVersionsCentrally>true</ManagePackageVersionsCentrally>
    <CentralPackageTransitivePinningEnabled>true</CentralPackageTransitivePinningEnabled>
  </PropertyGroup>

  <!-- Wspólne wersje frameworków i pakietów -->
  <PropertyGroup>
    <MicrosoftExtensionsVersion>8.0.0</MicrosoftExtensionsVersion>
    <AspNetCoreVersion>8.0.0</AspNetCoreVersion>
    <EntityFrameworkCoreVersion>8.0.0</EntityFrameworkCoreVersion>
    <!-- Wersje pakietów narzędziowych -->
    <MicrosoftCodeAnalysisNetAnalyzersVersion>8.0.0</MicrosoftCodeAnalysisNetAnalyzersVersion>
    <StyleCopAnalyzersVersion>1.2.0-beta.507</StyleCopAnalyzersVersion>
    <RoslynatorAnalyzersVersion>4.6.2</RoslynatorAnalyzersVersion>
  </PropertyGroup>

  <!-- Definicja wersji pakietów -->
  <ItemGroup>
    <!-- Microsoft Extensions -->
    <PackageVersion Include="Microsoft.Extensions.Configuration" Version="$(MicrosoftExtensionsVersion)" />
    <PackageVersion Include="Microsoft.Extensions.Configuration.Abstractions" Version="$(MicrosoftExtensionsVersion)" />
    <PackageVersion Include="Microsoft.Extensions.DependencyInjection" Version="$(MicrosoftExtensionsVersion)" />
    <PackageVersion Include="Microsoft.Extensions.DependencyInjection.Abstractions" Version="$(MicrosoftExtensionsVersion)" />
    <PackageVersion Include="Microsoft.Extensions.Logging" Version="$(MicrosoftExtensionsVersion)" />
    <PackageVersion Include="Microsoft.Extensions.Logging.Abstractions" Version="$(MicrosoftExtensionsVersion)" />
    <PackageVersion Include="Microsoft.Extensions.Options" Version="$(MicrosoftExtensionsVersion)" />

    <!-- ASP.NET Core -->
    <PackageVersion Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="$(AspNetCoreVersion)" />
    <PackageVersion Include="Microsoft.AspNetCore.Identity.EntityFrameworkCore" Version="$(AspNetCoreVersion)" />

    <!-- Entity Framework Core -->
    <PackageVersion Include="Microsoft.EntityFrameworkCore" Version="$(EntityFrameworkCoreVersion)" />
    <PackageVersion Include="Microsoft.EntityFrameworkCore.SqlServer" Version="$(EntityFrameworkCoreVersion)" />
    <PackageVersion Include="Microsoft.EntityFrameworkCore.Tools" Version="$(EntityFrameworkCoreVersion)" />
    
    <!-- Logowanie -->
    <PackageVersion Include="Serilog" Version="3.1.1" />
    <PackageVersion Include="Serilog.AspNetCore" Version="8.0.0" />
    <PackageVersion Include="Serilog.Sinks.Console" Version="5.0.0" />
    
    <!-- Testowanie -->
    <PackageVersion Include="Microsoft.NET.Test.Sdk" Version="17.8.0" />
    <PackageVersion Include="xunit" Version="2.6.2" />
    <PackageVersion Include="xunit.runner.visualstudio" Version="2.5.4" />
    <PackageVersion Include="Moq" Version="4.20.70" />
    <PackageVersion Include="FluentAssertions" Version="6.12.0" />
  </ItemGroup>
</Project>
```

### Zasady użycia Directory.Packages.props:

1. Nigdy nie określaj wersji pakietów bezpośrednio w plikach `.csproj`.
2. Zawsze dodawaj nowe pakiety poprzez `<PackageVersion Include="..." Version="..." />` w pliku `Directory.Packages.props`.
3. W plikach projektów `.csproj` używaj tylko `<PackageReference Include="..." />` bez atrybutu `Version`.
4. Organizuj pakiety w logiczne sekcje i grupy.
5. Używaj zmiennych PropertyGroup dla pakietów z powiązanymi wersjami.
6. Zawsze ustawiaj właściwość `ManagePackageVersionsCentrally` na `true`.

## 3. Przykład pliku projektu zgodnego z centralnym zarządzaniem

```xml
<Project Sdk="Microsoft.NET.Sdk.Web">
  <!-- 
  WAŻNE: Ten projekt używa centralnego zarządzania ustawieniami poprzez Directory.Build.props
  oraz centralnego zarządzania wersjami pakietów przez Directory.Packages.props.
  Nie definiuj ponownie właściwości obecnych w tych plikach.
  Nie określaj wersji pakietów bezpośrednio w tym pliku.
  -->
  
  <!-- Specyficzne dla projektu ustawienia, których nie ma w Directory.Build.props -->
  <PropertyGroup>
    <RootNamespace>NaszaFirma.Api</RootNamespace>
    <AssemblyName>NaszaFirma.Api</AssemblyName>
    <GenerateDocumentationFile>true</GenerateDocumentationFile>
  </PropertyGroup>

  <!-- Pakiety NuGet bez określania wersji -->
  <ItemGroup>
    <PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" />
    <PackageReference Include="Serilog.AspNetCore" />
  </ItemGroup>

  <!-- Referencje do projektów -->
  <ItemGroup>
    <ProjectReference Include="..\NaszaFirma.Core\NaszaFirma.Core.csproj" />
    <ProjectReference Include="..\NaszaFirma.Infrastructure\NaszaFirma.Infrastructure.csproj" />
  </ItemGroup>
</Project>
```

## 4. Instrukcje dla GitHub Copilot - codzienne użycie

### Gdy tworzysz nowy projekt:

1. Sprawdź, czy istnieje plik `Directory.Build.props` w katalogu głównym repozytorium. Jeżeli nie istnieje, zaproponuj utworzenie go według powyższego szablonu.

2. Sprawdź, czy istnieje plik `Directory.Packages.props` w katalogu głównym repozytorium. Jeżeli nie istnieje, zaproponuj utworzenie go według powyższego szablonu.

3. Przy tworzeniu plików projektów `.csproj`:
   - Nie definiuj właściwości, które są już w `Directory.Build.props`
   - Nie określaj wersji pakietów NuGet
   - Używaj poprawnej struktury projektu zgodnie z konwencjami repozytorium

### Gdy dodajesz nową zależność NuGet:

1. Najpierw sprawdź, czy pakiet jest już zdefiniowany w `Directory.Packages.props`
2. Jeśli pakiet nie istnieje w `Directory.Packages.props`:
   - Dodaj definicję pakietu w odpowiedniej sekcji `Directory.Packages.props`
   - Znajdź aktualną, stabilną wersję pakietu
   - Jeśli to część większego ekosystemu (np. Microsoft.Extensions.*), użyj zmiennej wersji
3. W pliku projektu `.csproj` dodaj tylko referencję bez określania wersji:
   - `<PackageReference Include="NazwaPakietu" />`

### Gdy aktualizujesz wersje pakietów:

1. Zawsze aktualizuj wersje centralnie w `Directory.Packages.props`
2. Jeśli aktualizujesz pakiety z tej samej rodziny/frameworka, zaktualizuj zmienną wersji w sekcji `PropertyGroup`
3. Upewnij się, że wersje są kompatybilne ze sobą

### Zarządzanie ustawieniami specyficznymi dla typów projektów:

1. Dla ustawień specyficznych dla typu projektu (np. testy, API, biblioteki), używaj warunków w `Directory.Build.props` lub plików importowanych
2. Przykład: `<PropertyGroup Condition="$(MSBuildProjectName.EndsWith('.Tests'))">...</PropertyGroup>`

## 5. Korzyści z centralnego zarządzania projektami

1. **Spójność** - wszystkie projekty używają tych samych wersji pakietów i mają spójne ustawienia
2. **Łatwość utrzymania** - aktualizacja jednego pliku zamiast wielu plików .csproj
3. **Mniej konfliktów** - scentralizowane zarządzanie zmniejsza konflikty w plikach projektów
4. **Lepsza widoczność** - łatwiej zobaczyć, jakie pakiety są używane w całym rozwiązaniu
5. **Łatwiejsze aktualizacje** - aktualizacja jednej wersji w centralnym miejscu

## 6. Dodatkowe wskazówki

### Używanie nowego stylu namespace (file-scoped namespaces)

Zawsze używaj nowego stylu namespace'ów (file-scoped) wprowadzonego w C# 10:

```csharp
// POPRAWNIE:
namespace NaszaFirma.Api.Controllers;

// NIEPOPRAWNIE:
namespace NaszaFirma.Api.Controllers {
    // ...
}
```

### Struktura plików rozwiązania

```
└── Solution
    ├── Directory.Build.props
    ├── Directory.Packages.props
    ├── .editorconfig
    ├── src/
    │   ├── NaszaFirma.Core/
    │   ├── NaszaFirma.Infrastructure/
    │   └── NaszaFirma.Api/
    └── tests/
        ├── NaszaFirma.Core.Tests/
        ├── NaszaFirma.Infrastructure.Tests/
        └── NaszaFirma.Api.Tests/
```

### Zawsze trzymaj się tych zasad:
1. Bezpośrednio w plikach `.csproj` umieszczaj tylko to, co jest specyficzne dla danego projektu
2. Wspólne ustawienia i zależności zarządzaj centralnie
3. Używaj pliku `Directory.Packages.props` do zarządzania wszystkimi wersjami pakietów NuGet
4. Używaj file-scoped namespaces w plikach C#