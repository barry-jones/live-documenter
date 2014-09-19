# TheBoxSoftware.Reflection.Comments code

This code is designed to parse the XML comments files and store them as an object graph, so
they can be later processed for exporting or display.

Every class here will eventually inherit from XmlCodeElement. This class defines the list of
acceptable (i.e will be parsed) of elements. If this class is not defined here it will simply
be ignored along with all of it's child elements.

## Implementing new tokenisers

When adding support for new xml elements you should derive from one of the following classes:

+ XmlCodeElement
+ XmlContainerCodeElement

The first should be used if the element is at the end of the tree and will not include any
further child elements. The second when it is a container element which will/can include
child elements.

Code needs to be added to the XmlContainerCodeElement.Parse method to make sure it is handled,
also an enty needs to be added to teh XmlCodeElements enumeration.

See implemention in the LiveDocumenter desktop application to find out how it converts these
tokens in to screen displayable tokens in WPF. The other code which uses this is in the XML 
renderes which will decide how they are rendered in XML. **Either way** a change here will
generally require a change in both of those places.