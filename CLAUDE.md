# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Build Commands

Build the entire solution:
```
dotnet build developersuite.sln
```

Build a specific project:
```
dotnet build Source/TheBoxSoftware.Reflection/TheBoxSoftware.Reflection.csproj
```

Run all tests:
```
dotnet test developersuite.sln
```

Run tests for a specific project:
```
dotnet test Source/TheBoxSoftware.Reflection.Tests/TheBoxSoftware.Reflection.Tests.csproj
```

Run a single test by name:
```
dotnet test Source/TheBoxSoftware.Reflection.Tests/TheBoxSoftware.Reflection.Tests.csproj --filter "FullyQualifiedName~TestName"
```

Run the console exporter (after build):
```
Source/TheBoxSoftware.DeveloperSuite.LiveDocumenter.Exporter/bin/Debug/netcoreapp3.1/win-x64/exporter.exe <filename> [modifiers]
```

Exporter usage:
```
exporter <filename> -to <output-dir> -format <ldec-file> -filters "public|protected"
exporter config.xml   # use a configuration XML file
```

## Architecture

Live Documenter is a .NET documentation suite with three modes: a WPF desktop application, a console exporter, and a library API. The solution (`developersuite.sln`) targets Windows, with core libraries on `netstandard2.0` and applications on `netcoreapp3.1`.

### Project dependency chain

```
TheBoxSoftware (netstandard2.0)         — shared utilities and diagnostics
    └── TheBoxSoftware.Reflection       — PE/COFF binary parser and reflection model
            └── TheBoxSoftware.Documentation  — document model, mappers, exporters
                    └── TheBoxSoftware.API.LiveDocumenter  — public API surface
                    └── TheBoxSoftware.DeveloperSuite.LiveDocumenter.Exporter  — console app
                    └── TheBoxSoftware.DeveloperSuite.LiveDocumenter  — WPF desktop app (Windows-only)
```

### TheBoxSoftware.Reflection

Reads .NET PE/COFF binaries directly without loading them into the runtime. Key types:

- `PeCoffFile` — parses the PE file format, locates the CLR metadata directory
- `AssemblyDef` — top-level reflection entry point, created via `AssemblyDef.Create(fileName)`
- `TypeDef`, `MethodDef`, `FieldDef`, `PropertyDef`, `EventDef`, `ParamDef` — reflected member types
- `Core/COFF/` — low-level metadata table row types (one file per ECMA-335 metadata table)
- `Comments/` — parses the XML documentation comment files (`.xml`) alongside assemblies. `CRefPath` represents the `cref` attribute format used to cross-reference members.
- `Signatures/` — decodes method and field binary signatures from the blob heap

### TheBoxSoftware.Documentation

Builds a navigable `DocumentMap` (tree of `Entry` objects) from a set of `DocumentedAssembly` instances. Key concepts:

- `DocumentMapper` (abstract) — factory-created via `DocumentMapper.Create(...)`. Three strategies: `AssemblyFirstDocumentMapper`, `NamespaceFirstDocumentMapper`, `GroupedNamespaceDocumentMapper`
- `Document` — wraps the DocumentMap and its assemblies; the central object passed through the export pipeline
- `InputFileReader` / `LibraryFileReader` / `ProjectFileReader` / `SolutionFileReader` — read different input types (.dll, .csproj, .sln) and produce `DocumentedAssembly` lists
- `Exporting/Exporter` (abstract) — factory-created via `Exporter.Create(document, settings, config)`. Concrete types: `WebsiteExporter`, `HtmlHelp1Exporter`, `HtmlHelp2Exporter`, `HelpViewer1Exporter`, `XmlExporter`
- `Exporting/ExportConfigFile` — wraps a `.ldec` file (zip containing an `export.config` XML and XSLT/assets). The `.ldec` files in `ApplicationData/` define the available export formats.
- Export pipeline: `Entry` → XML (via `Rendering/XmlRenderer`) → XSLT transform → output format

### TheBoxSoftware.DeveloperSuite.LiveDocumenter.Exporter (console app)

Entry point: `Program.HandleExport()`. Accepts either:
1. A single library/project/solution file with `-to`, `-format`, `-filters` flags
2. An XML configuration file (see `example-configuration.xml`) that specifies document, filters, and multiple outputs

The `AppContext.SetSwitch("Switch.System.Xml.AllowDefaultResolver", true)` call at startup is required for XSLT processing on .NET Core (workaround for dotnet/corefx#31390).

### LDEC files

Export configuration packages (`.ldec`) are ZIP archives containing:
- `export.config` — XML describing name, exporter type, XSLT path, output files, and properties
- An XSLT stylesheet
- Supporting assets (CSS, JS, images)

Built-in formats live in `Source/TheBoxSoftware.DeveloperSuite.LiveDocumenter.Exporter/ApplicationData/` and are copied to output on build.

### Test projects

- `TheBoxSoftware.Reflection.Tests` — NUnit 3, uses Moq; depends on `DocumentationTest` (a helper assembly of test fixtures)
- `TheBoxSoftware.Documentation.Tests` — NUnit 3, uses Moq
- `TheBoxSoftware.DeveloperSuite.LiveDocumenter.Exporter.Tests` — NUnit 3, uses Moq
- `TheBoxSoftware.API.LiveDocumenter.Tests` — NUnit 3

All test projects use `netcoreapp3.1`.
