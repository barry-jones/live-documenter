# API Overview

The API (application programming interface) provided by the Live Documenter enables you to have more flexibility over documentation that is generated. In essence it allows you to load a library or project and produce documentation on request a single page at a time.

This would allow you to, for example, create a web site that checks a directory, loads and displays documentation for types on request. An example of this is provided with the installers, the current community version can be found on GitHub.


	Documentation docs = new Documentation("myfile.dll");
	docs.Load();
	
	// get a page of documentation
	XmlDocument xmlDocument = new XmlDocument();
	xmlDocument.LoadXml(
		docs.Find("T:System.String"); // searching can be performed using cref paths
		);	