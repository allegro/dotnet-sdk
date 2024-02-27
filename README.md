# Allegro .NET SDK

## About

The Allegro .NET SDK provides common and importable project settings, such as build properties, coding styles, analyzers configuration etc.

The SDK is versioned and published on nuget. It can be imported into dotnet projects.

## Using the SDK

The SDK is meant to be easily importable - only a few initial config lines are required to bring its benefits. The SDK also makes it possible to override any of its default settings. Version 2+ will work only with .NET SDK v8+.

## Importing

The sections below list the changes required in order to import the SDK.

Additional information can be found in the docs - [Reference a project SDK](https://docs.microsoft.com/en-us/visualstudio/msbuild/how-to-use-project-sdk?view=vs-2022#reference-a-project-sdk).

### global.json

It's necessary to include `Allegro.DotnetSdk` with the desired version in `global.json` in the repository root:

```json
{
  "sdk": {
    "version": "8.0.100",
    "rollForward": "latestFeature"
  },
  "msbuild-sdks": {
    "Allegro.DotnetSdk": "2.1.0"
  }
}

```

### Directory.Build.props

The `Directory.Build.props` file should be updated in order to actually import the SDK:

```xml
<Project>
    <Sdk Name="Allegro.DotnetSdk" />
    
    <!-- other project-specific properties -->
</Project>
```

## Project settings

Most properties are only set in the SDK if not configured by the project.

To configure the SDK, the following properties can be added into `Directory.Build.props` or `.*proj` files:

- `AllegroDotnetSdkEnableXmlDocAdjustments` (default: true) - enable doc file generation, suppress missing-comments warning (1591) - completely in test projects, as errors otherwise (keep as warning).
- `AllegroDotnetSdkTreatWarningsAsErrors` (default: true in CI and Rider IDE) - to enable in local shell or other IDEs, just set your own user environment variable `TreatWarningsAsErrors=true`.
- `AllegroDotnetSdkEnableGlobalEditorConfig` (default: true) - add `editorconfig.global` analyzer configuration file.
- `AllegroDotnetSdkEnablePackAdjustments` (default: true) - sets PublishRepositoryUrl and enables `.snupkg` symbols.
- `AllegroDotnetSdkEnableAzureArtifactsAdjustments` (default: false) - disables `.snupkg` and instead embeds PDBs in `.nupkg`.

For `C#` only:

- `AllegroDotnetSdkEnableImplicitUsingsAdjustments` (default: true for C#) - add `System.Collections.Immutable` and remove `Microsoft.Extensions.Logging` and `System.Net.Http` implicit usings.
- `UseAllegroDotnetSdkDefaultAnalyzers` (default: true for C# when no CPM) - reference and configure several external analyzers - StyleCop, AsyncFixer, Meziantou.
- `AllegroDotnetSdkEnableAsyncFixer` (default: `UseAllegroDotnetSdkDefaultAnalyzers`) - references AsyncFixer package.
- `AllegroDotnetSdkEnableMeziantou` (default: `UseAllegroDotnetSdkDefaultAnalyzers`) - references Meziantou.Analyzers package.
- `AllegroDotnetSdkEnableStyleCop` (default: `UseAllegroDotnetSdkDefaultAnalyzers`) - references StyleCop.Analyzers package and a preset `stylecop.json`.

Configure in `Directory.Build.props` - repo wide:

```xml
<Project>

    <Sdk Name="Allegro.DotnetSdk" />

    <PropertyGroup>
        <!-- The line below disables nullable reference types -->
        <Nullable>disable</Nullable>
        <!-- The line below disables the StyleCop -->
        <AllegroDotnetSdkEnableStyleCop>false</AllegroDotnetSdkEnableStyleCop>
    </PropertyGroup>

</Project>
```

Configure in `*.csproj` - project settings:

```xml
<Project Sdk="Microsoft.NET.Sdk">

    <PropertyGroup>
        <!-- The line below disables implicit usings adjustments -->
        <AllegroDotnetSdkEnableImplicitUsingsAdjustments>false</AllegroDotnetSdkEnableImplicitUsingsAdjustments>
    </PropertyGroup>

</Project>
```

## Editor config

The SDK contains a global analyzer config file containing the default configuration for various analyzers and formatters.

Its entries can be also overridden by entries from `.editorconfig`. Such editor config files are merged with the imported global config file.

More about the analyzer and editor config files can be found in [the docs](https://docs.microsoft.com/en-us/dotnet/fundamentals/code-analysis/configuration-files).

## IDEs support

### Rider  

Make sure you have enabled:

- Preferences -> Editor -> Code Style -> Enable StyleCop support (Ruleset files)
- Preferences -> Editor -> Code Style -> Enable EditorConfig support
- Preferences -> Editor -> Inspection Settings -> Read settings from editorconfig, project settings and rule sets
- Preferences -> Editor -> Inspection Settings -> Roslyn -> Enable Roslyn analyzers and Source Generators
- Preferences -> Editor -> Inspection Settings -> Roslyn -> Include Roslyn analyzers in Solution-Wide Analysis

**Be aware!**
Only some analyzers's warnings can be addressed by auto-format or code cleanup. Some of the warnings are not covered by Rider, and its settings need to be adjusted.  
For that, in editor.globalconfig is `# ReSharper properties` section with some already defined settings which align with analyzers. If you find some inconsistency and you find appropriate settings in Rider, which will fix it - please, contribute! :)

## License

Copyright Allegro Group

Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the License at

```http://www.apache.org/licenses/LICENSE-2.0```

Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
