# Live Documenter Export Configuration Files
An export configuration file is a description to the exporter of how to export the current documentation set. There are various pre-packaged Live Documenter Export Configuration (LDEC) files, for exporting to HTML Help 1, Help Viewer 1, Help Viewer 2, Web and XML.

## Using LDEC files
LDEC files are easy to use, the following describes the use of LDEC files to configure exports in each of the export applications.

### Live Documenter Desktop
The Live Documenter desktop application stores the LDEC files in the ApplicationData folder in it's installation directory.

To add new or remove LDEC files from the Live Documenter export dialogue simply add and remove the items from this folder. Live Documenter searches this directory for LDEC files.

### Live Documenter Exporter
This works in the same way as the desktop application, in that it has an ApplicationData folder where the LDEC files are installed. However when exports are run the exporter uses a configuration file where you can detail which LDEC files to use when exporting.


	<?xml version="1.0" encoding="UTF-8"?>
	<configuration>
	  <document>c:\path-to-project\mysolution.sln</document>-->
		
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

### API
The API does not use LDEC files so there is no need to provide details of them. The API outputs what is the same output from the xml.ldec file (our intermediate XML) to you application to do with as you please.

## What is an LDEC file?
An LDEC file is basically a ZIP file containing all the files and information necessary to perform that export. For example the web-msdn.ldec file has the following structure:

	styles/
	styles/default.css
	styles/images/...
	export.config
	screenshot.png
	webexport.xslt

The important file is the export.config file which is read by Live Documenter so it knows where important files such as the xslt are. The web-msdn.ldec export.config file has these contents:

	<?xml version="1.0" encoding="UTF-8"?>
	<export xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
	    <name>Web Export: MSDN Lo Band Style</name>
		<description>A wesbite export of the documentation in the MSDN Lo Band web style.</description>
		<author>The Box Software</author>
		<version>1.0</version>
		<exporter>web</exporter>
	    <xslt>webexport.xslt</xslt>
		<screenshot>screenshot.png</screenshot>
		
	    <properties>
			<property name="extension" value="htm" />
	    </properties>
	    
	    <outputfiles>
	        <file internal="styles/default.css" output="\styles\default.css" />
	        <file internal="styles/images/" output="\styles\images\" />
	    </outputfiles>
	</export>

The `name`, `description`, `author`, and `version` elements contain details that are displayed in the Live Documenter export dialogue and can be used to describe the LDEC file.

`exporter` describes the internal processor for the export. This can be one of the following; `html1`, `html2`, `helpviewer1`, `xml` and `web`. This can be left blank but will default to web. These represent internal processors in the application, the HTML Help and Help Viewer exporters for example have a final processing step which compiles the resulting help files.

The `sxlt` element points to the XSLT file which will be used to generate the content, in the web exporter this is the end content, the HTML files necessary to produce a website. Alternatively the html1 xslt produces web content with the required information to produce a compiled HTML Help chm file at the end.

The `extension` property determine the file extension of all the outputted files.

And finally the `outputfiles` section allow you to describe a number of individual files and folders that should also be published during the export. In the above example this is used to output the style sheets and image the website uses.

## Find out more
[How to create your own LDEC files](create-your-own-ldec-files.md "Create your own LDEC files")

[LDEC exporters](exporter)

