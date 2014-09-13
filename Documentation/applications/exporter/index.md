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
|<filename>|The file path to the configuration file that details the export.|

## Export Configuration File

An export configuration file contains the following information.

	<?xml version="1.0" encoding="UTF-8"?>
	<configuration>
	  	<document>c:\your-path\mysolution.sln</document>
		
		<!-- can be Public|Internal|Protected|ProtectedInternal|Private only those detailed will be output,
			not specifiying a filter section will result in only the Public members being exported.
	    
	    If these are specified and the document is an ldproj file then these will override the 
	    ldproj files filters. In the event that the document is not an ldprof file these will 
	    need to specified.
		-->	
		<filters>
			<filter>Public</filter>
			<filter>Protected</filter>
	    	<fitler>Internal</fitler>
	    	<filter>InternalProtected</filter>
	    	<filter>Private</filter>
		</filters>
	  	
		<!-- the application will always check the ApplicationData folder for LDEC files -->
	  	<outputs>
			<!-- the locations being output to will be cleared without warning -->
		    <ldec location="c:\temp\web\">web-msdn.ldec</ldec>
		    <ldec location="c:\temp\htmlhelp-1\">htmlhelp1-msdn.ldec</ldec>
		    <ldec location="c:\temp\htmlhelp-2\">htmlhelp2-msdn.ldec</ldec>
		    <ldec location="c:\temp\helpviewer-1\">helpviewer1-msdn.ldec</ldec>
		    <ldec location="c:\temp\xml\">xml.ldec</ldec>
		</outputs>
	</configuration>

The `document`  element is a path to your solution, project, .NET dll or ldproj file. Only one document can be specified.

`filters` determine the visibility of the members that will be exported. If these are not specified it will default to Public and Protected.

Finally, `outputs` detail the location of published files and the LDEC file used to perform the export. One or more outputs can be defined here. Each one will be exported in turn.

Please **note** that, just like the desktop application, the output locations will be cleared before the new export will continue.