# Options

The following options can be configured with the Live Documenter desktop application.

## Document Settings

![Settings dialogue](/images/ld-settings.png)

### Build Configuration

The build configuration allows you to select between Release and Debug. This is only relevant when reading documentation from a Visual Studio project or solution. This will direct the application to look in the Release or Debug folders respectively for libraries and XML comment files.

### Syntax Language

This allows you to configure which syntax is displayed in the documentation pane. Currently you can choose between C# and VB.NET. After changing this option after navigating around the project you will see that the syntax blocks now display in the selected language.

### Visibility filter

The visibility filter controls which members and types are displayed in the documentation based on their access modifiers. You can choose a minimum of just public (public is always displayed and can't be changed) and one or more of the following.

+ Internal
+ Private
+ Protected
+ Protected internal

After accepting the documentation will be updated to display only those types and members that match your criteria.