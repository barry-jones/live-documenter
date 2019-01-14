![Live Documenter](/readme/images/logo.png)

# Live Documenter [![Tweet](https://img.shields.io/twitter/url/http/shields.io.svg?style=social)](https://twitter.com/intent/tweet?text=Check%20out%20Live%20Documenter%20on%20GitHub.%20.NET%20documentation%20generator&url=https://github.com/barry-jones/live-documenter&hashtags=.net,xml_comments,documentation,generator,developers,livedocumenter)

[![](https://img.shields.io/github/release/barry-jones/live-documenter.svg)](https://github.com/barry-jones/live-documenter/releases/tag/v2.0.5)
[![Build Status](https://dev.azure.com/barryjones78/livedocumenter/_apis/build/status/barry-jones.live-documenter?branchName=master)](https://dev.azure.com/barryjones78/livedocumenter/_build/latest?definitionId=1?branchName=master)

A collection of applications that enable the ridiculously fast generation of API documentation in Web, XML, HTML Help and Help Viewer from .NET libraries and XML code comments.

#### Desktop Application
Desktop application allows visual studio projects, solutions or libraries to be opened and documentation
generated for them from the XML comment files live. If changes are made to the codebase these are reflected
in the application when the project is re-built.

It also provides a nice interface for exporting documentation, reading and printing.

![Live Documenter](/readme/images/ld-open-docs.png)

#### Console Application
Command line application that can be used to generate documentation from a configuration xml file. This is 
useful for exporting configuration as part of the build process.

![Live Documenter](/readme/images/command-line-output.png)

#### API Library
A DLL that can be referenced by projects to automatically generate documentation at runtime. Details of the 
API and how to use it can be found in the [documentation][1].

## Installation
This will only work on Windows at the moment, work is ongoing to transfer to netstandard 
and to make everything except the desktop application cross platform on .NET Core.

The installers are provided in the release pages for the 
[latest version](https://github.com/barry-jones/live-documenter/releases) MSI files are provided seperately. 
To install, download the required MSI files from the release page and run the installer.

## Build Latest

Requirements
-  .NET Core SDK 2.2
-  .NET Framework 4.7.1

The latest code has been worked on in Visual Studio 2017 and several of the projects have been transferred to netstandard libraries and use the latest csproj file format.

__Please let me know if there are any issues building from source so I can resolve them.__

[1]: https://livedocumenter.barryjones.me.uk/docs/api/index.html
