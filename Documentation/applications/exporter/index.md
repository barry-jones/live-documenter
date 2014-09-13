# Live Documenter Command Line Exporter
The exporter application is a useful for integrating documentation generation in to existing processes such as your build scripts. An application that works on the command line can be simply called.

The parameters accepted by the exporter are:
	
	exporter [modifiers] <filename>

	modifiers:
		-h		show help information
		-v		show verbose export details

	<filename>	The path to the configuration xml file

### Commands

|Action|Description|
|------|-----------|
|-h|Displays the help information above.|
|-v|Outputs the steps for the export instead of just the fact it has started an export. With or without this modifier it will always display the warnings and errors.|


