# Live Documenter

A .NET application for automatically producing documentation from .NET libraries and XML comment files.

__The project contains__

### Desktop Application
Desktop application allows visual studio projects, solutions or libraries to be opened and documentation
generated for them from the XML comment files live. If changes are made to the codebase these are reflected
in the application when the project is re-built.

It also provides a nice interface for exporting documentation, reading and printing.
### Console Application
Command line application that can be used to generate documentation from a configuration xml file. This is 
useful for exporting configuration as part of the build process.
### API Library
A DLL that can be referenced by projects to automatically generate documentation at runtime.
### PE Viewer
An unfinished application that provides a simple view of metadata tables available in .NET libraries.
