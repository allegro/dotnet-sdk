# Allegro .NET SDK

## About

The Allegro .NET SDK provides common and importable project settings, such as build properties, coding styles, analyzers configuration etc.

The SDK is versioned and published on nuget It can be imported into dotnet projects.

## Using the SDK

The SDK is meant to be easily importable - only a few initial config lines are required to bring its benefits. The SDK also makes it possible to override any of its default settings.

## Importing

The sections below list the changes required in order to import the SDK.

Additional information can be found in the docs - [Reference a project SDK](https://docs.microsoft.com/en-us/visualstudio/msbuild/how-to-use-project-sdk?view=vs-2022#reference-a-project-sdk).

### global.json

It's necessary to include `Allegro.DotnetSdk` with the desired version in `global.json` in the repository root:

```json
{
    "sdk": {
      "version": "6.0.100",
      "rollForward": "latestFeature"
    },
    "msbuild-sdks": {
        "Allegro.DotnetSdk": "1.0.2"
    }
}

```
### Directory.Build.props

The `Directory.Build.props` file should be updated in order to actually import the SDK:

```xml
<Project Sdk="Allegro.DotnetSdk">
    <PropertyGroup>
        <TargetFramework>net6.0</TargetFramework>
    </PropertyGroup>
    
    <!-- other project-specific properties -->
</Project>
```

### Analyzers

The SDK imports and configures several external analyzers - StyleCop, AsyncFixer, Meziantou.

The existing analyzer package reference sections should be removed from `paket.dependencies` and `paket.references`. If they're not removed, the build may fail because of duplicated package references.

## Overriding

The imported properties can be overridden per repo or per project.

## Project settings

In order to override or disable some of the imported components, the behavior changing properties can be added into `Directory.Build.props` or `.csproj` files.

* `Directory.Build.props` - repo wide:
```xml
<Project Sdk="Allegro.DotnetSdk">
    <PropertyGroup>
        <TargetFramework>net6.0</TargetFramework>
    </PropertyGroup>

    <PropertyGroup>
        <!-- The line below disables nullable reference types -->
        <nullable>disable</nullable>
        <!-- The line below disables the StyleCop config -->
        <AllegroDotnetSdkEnableStyleCop>false</AllegroDotnetSdkEnableStyleCop>
    </PropertyGroup>
    
    <!-- other repo-specific properties -->
</Project>
```

* `.csproj` - project settings:
```xml
<Project Sdk="Microsoft.NET.Sdk">
    <PropertyGroup>
        <OutputType>Exe</OutputType>
        <TargetFramework>net6.0</TargetFramework>
        <GenerateDocumentationFile>true</GenerateDocumentationFile>
    </PropertyGroup>

    <!-- other project-specific properties -->
</Project>
```

## Editor config

The SDK contains a global analyzer config file containing the default configuration for various analyzers and formatters.

Its entries can be also overridden by entries from `.editorconfig`. Such editor config files are merged with the imported global config file.

More about the analyzer and editor config files can be found in [the docs](https://docs.microsoft.com/en-us/dotnet/fundamentals/code-analysis/configuration-files).

## License
Copyright 2022 Allegro Group

Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the License at

```http://www.apache.org/licenses/LICENSE-2.0```

Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
