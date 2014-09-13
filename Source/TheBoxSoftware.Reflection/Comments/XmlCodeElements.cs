using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Comments {
	/// <summary>
	/// Enumeration of supported XML Code Comment tags
	/// </summary>
	/// <remarks>
	/// If this enumeration is increased make sure that the relevant modifications in the
	/// parsing methods <see cref="XmlContainerCodeElement"/> and the definition list is
	/// <see cref="XmlCodeElement.DefinedElements"/> updated.
	/// </remarks>
	public enum XmlCodeElements {
		// Compiler supported XmlCodeElements
		/// <summary>
		/// The &lt;c&gt; tag gives you a way to indicate that text within a description should be 
		/// marked as code. Use &lt;code&gt; to indicate multiple lines as code. 
		/// </summary>
		C,
		/// <summary>
		/// The &lt;code&gt; tag gives you a way to indicate multiple lines as code. Use &lt;c&gt; 
		/// to indicate that text within a description should be marked as code. 
		/// </summary>
		Code,
		/// <summary>
		/// The &lt;example&gt; tag lets you specify an example of how to use a method or other 
		/// library member. This commonly involves using the &lt;code&gt; tag. 
		/// </summary>
		Example,
		/// <summary>
		/// The &lt;exception&lt; tag lets you specify which exceptions can be thrown. This tag 
		/// can be applied to definitions for methods, properties, events, and indexers.
		/// </summary>
		Exception,
		/// <summary>
		/// Used to point the compiler at an external comments file for the element being documented;
		/// this should never exist inside an xml comments file.
		/// </summary>
		Include,
		/// <summary>
		/// <para>
		/// The &lt;listheader&gt; block is used to define the heading row of either a table or definition 
		/// list. When defining a table, you only need to supply an entry for term in the heading.
		/// </para>
		/// <para>
		/// Each item in the list is specified with an &lt;item&gt; block. When creating a definition 
		/// list, you will need to specify both term and description. However, for a table, bulleted 
		/// list, or numbered list, you only need to supply an entry for description.
		/// </para>
		/// <para>A list or table can have as many &lt;item&gt; blocks as needed.</para>
		/// </summary>
		List,
		/// <summary>
		/// The &lt;listheader&gt; block is used to define the heading row of either a table or definition 
		/// list. When defining a table, you only need to supply an entry for term in the heading. Further,
		/// the listheader is not required and should not be provided when a simple list is required.
		/// </summary>
		ListHeader,
		/// <summary>
		/// A list or table can have as many &lt;item&gt; blocks as needed.
		/// </summary>
		ListItem,
		/// <summary>
		/// Only available in listheader and item elements of list blocks. This defines a term and is used
		/// when the list is to be presented as a definition list or a table.
		/// </summary>
		Term,
		/// <summary>
		/// Only available in listheader and item elements of list blocks. This defines a description and is
		/// used when the list is to be presented as a definition list, list or table. If only the description
		/// is present the list will be presented as a simple list.
		/// </summary>
		Description,
		/// <summary>
		/// The &lt;para&lt; tag is for use inside a tag, such as &lt;summary&gt;, &lt;remarks&gt;, or 
		/// &lt;returns&gt;, and lets you add structure to the text.
		/// </summary>
		Para,
		/// <summary>
		/// The &lt;param&lt; tag should be used in the comment for a method declaration to describe 
		/// one of the parameters for the method. 
		/// </summary>
		Param,
		/// <summary>
		/// The &lt;paramref&gt; tag gives you a way to indicate that a word in the code comments, for 
		/// example in a &lt;summary&gt; or &lt;remarks&gt; block refers to a parameter. The XML file 
		/// can be processed to format this word in some distinct way, such as with a bold or italic font.
		/// </summary>
		ParamRef,
		/// <summary>
		/// The &lt;permission&gt; tag lets you document the access of a member. The PermissionSet class 
		/// lets you specify access to a member. 
		/// </summary>
		Permission,
		/// <summary>
		/// The &lt;remarks&gt; tag is used to add information about a type, supplementing the information 
		/// specified with &lt;summary&gt;. This information is displayed in the Object Browser.
		/// </summary>
		Remarks,
		/// <summary>
		/// The &lt;returns&gt; tag should be used in the comment for a method declaration to describe the 
		/// return value. 
		/// </summary>
		Returns,
		/// <summary>
		/// The &lt;see&gt; tag lets you specify a link from within text. Use &lt;seealso&gt; to indicate 
		/// that text should be placed in a See Also section. Use the cref Attribute to create internal 
		/// hyperlinks to documentation pages for code elements. 
		/// </summary>
		See,
		/// <summary>
		/// The &lt;seealso&lt; tag lets you specify the text that you might want to appear in a See Also 
		/// section. Use &lt;see&gt; to specify a link from within text. 
		/// </summary>
		SeeAlso,
		/// <summary>
		/// <para>The &lt;summary&lt; tag should be used to describe a type or a type member. Use &lt;remarks
		/// &gt; to add supplemental information to a type description. Use the cref Attribute to enable 
		/// documentation tools such as Sandcastle to create internal hyperlinks to documentation pages for 
		/// code elements.
		/// </para>
		/// <para>
		/// The text for the &lt;summary&gt; tag is the only source of information about the type in 
		/// IntelliSense, and is also displayed in the Object Browser.
		/// </summary>
		Summary,
		/// <summary>
		/// The &lt;typeparam&gt; tag should be used in the comment for a generic type or method declaration 
		/// to describe a type parameter. Add a tag for each type parameter of the generic type or method.
		/// </summary>
		TypeParam,
		/// <summary>
		/// Use this tag to enable consumers of the documentation file to format the word in some distinct 
		/// way, for example in italics. 
		/// </summary>
		TypeParamRef,
		/// <summary>
		/// The &lt;value&gt; tag lets you describe the value that a property represents. Note that when 
		/// you add a property via code wizard in the Visual Studio .NET development environment, it will 
		/// add a &lt;summary&gt; tag for the new property. You should then manually add a &lt;value&gt; tag 
		/// to describe the value that the property represents. 
		/// </summary>
		Value,

		// Our application supported XmlCodeElements, we can support cref attributes
		/// <summary>
		/// The &lt;b&gt; tag lets you emphasise a portion of the comments.
		/// </summary>
		B,
		/// <summary>
		/// The &lt;i&gt; tag lets you emphasise a portion of the comments.
		/// </summary>
		I,
		/// <summary>
		/// Not exactly an xml code element, but this is used by the parser to contain the textual elements
		/// stored in other XmlCodeElement instances.
		/// </summary>
		Text
	}
}
