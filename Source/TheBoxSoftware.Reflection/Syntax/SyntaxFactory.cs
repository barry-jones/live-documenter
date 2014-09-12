using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Syntax {
	using TheBoxSoftware.Reflection.Syntax.CSharp;
	using TheBoxSoftware.Reflection.Syntax.VisualBasic;

	/// <summary>
	/// Static Factory class for instantiating syntax IFormatter implementations.
	/// </summary>
	/// <remarks>
	/// <para>The syntax system works from here. The Create method is called and the
	/// Factory will then instantiate the correct classes to format the member.</para>
	/// <para>There are a number of classes that interact with each other to provide
	/// the formatting. First the Syntax class, provides a wrapper to the member to
	/// make it easy to obtain the important information for the member. This class
	/// is language agnostic. The second portion provides language dependant formatters
	/// to provide the langauge syntax for the member.
	/// </para>
	/// <para>First a Interface is created to provide the basic mechanisms that all 
	/// formatters should adhere to. Then each langauge dependant formatter implements
	/// that interface and provides the language specific Syntax blocks which can be
	/// used outside of the library</para>
	/// <para>When creating new Formatter Interfaces the interface should derive from
	/// the IFormatter interface to make sure implementing classes implement the 
	/// Format method.</para>
	/// <para>Finally, it is important to note that formatters should implement formatting
	/// based on the language specification for the language they are formatting for.</para>
	/// </remarks>
	public static class SyntaxFactory {
		/// <summary>
		/// Instantiates the correct IFormatter implementation based on the provided
		/// <paramref name="member"/> and <paramref name="langauge"/>.
		/// </summary>
		/// <param name="member">The member to create the formatter for.</param>
		/// <param name="language">The language the formatter should be for.</param>
		/// <returns>A IFormatter implementation.</returns>
		public static IFormatter Create(ReflectedMember member, Languages language) {
			return SyntaxFactory.CreateFormatter(
				SyntaxFactory.CreateSyntax(member),
				language);
		}

		/// <summary>
		/// Returns a correctly typed and instantiated <see cref="Syntax"/> class
		/// for the specified <paramref name="member"/>.
		/// </summary>
		/// <param name="member">The member to create a Syntax instance for.</param>
		/// <returns>The instantiated Syntax class.</returns>
		private static Syntax CreateSyntax(ReflectedMember member) {
			Syntax syntax = null;

			if (member is TypeDef) {
				TypeDef type = member as TypeDef;
				if (type.IsInterface) {
					syntax = new InterfaceSyntax(type);
				}
				else if (type.IsEnumeration) {
					syntax = new EnumSyntax(type);
				}
				else if (type.IsStructure) {
					syntax = new StructSyntax(type);
				}
				else if (type.IsDelegate) {
					syntax = new DelegateSyntax(type);
				}
				else {
					syntax = new ClassSyntax(type);
				}
			}
			else if (member is FieldDef) {
				FieldDef field = member as FieldDef;
				if (field.IsConstant) {
					syntax = new ConstantSyntax(field);
				}
				else {
					syntax = new FieldSyntax(field);
				}
			}
			else if (member is MethodDef) {
				MethodDef method = member as MethodDef;
				if (method.IsConstructor) {
					syntax = new ConstructorSyntax(method);
				}
				else if (method.IsOperator) {
					syntax = new OperatorSyntax(method);
				}
				else {
					syntax = new MethodSyntax(method);
				}
			}
			else if (member is EventDef) {
				syntax = new EventSyntax(member as EventDef);
			}
			else if (member is PropertyDef) {
				// A property can be a noram property or an indexor
				PropertyDef property = member as PropertyDef;
				if (property.IsIndexer) {
					syntax = new IndexorSyntax(member as PropertyDef);
				}
				else {
					syntax = new PropertySyntax(member as PropertyDef);
				}
			}

			return syntax;
		}

		/// <summary>
		/// Instantiates a IFormatter implementing class for the provided
		/// <paramref name="syntax"/> and <paramref name="language"/>.
		/// </summary>
		/// <param name="syntax">The syntax to create a formatter for.</param>
		/// <param name="language">The language the formatter should be.</param>
		/// <returns>The IFormatter implementation.</returns>
		private static IFormatter CreateFormatter(Syntax syntax, Languages language) {
			IFormatter formatter = null;

			if (syntax is ClassSyntax) {
				switch (language) {
					case Languages.CSharp: formatter = new CSharpClassFormatter(syntax as ClassSyntax); break;
					case Languages.VisualBasic: formatter = new VBClassFormatter(syntax as ClassSyntax); break;
				}
			}
			else if (syntax is InterfaceSyntax) {
				switch (language) {
					case Languages.CSharp: formatter = new CSharpInterfaceFormatter(syntax as InterfaceSyntax); break;
					case Languages.VisualBasic: formatter = new VBInterfaceFormatter(syntax as InterfaceSyntax); break;
				}
			}
			else if (syntax is EnumSyntax) {
				switch (language) {
					case Languages.CSharp: formatter = new CSharpEnumerationFormatter(syntax as EnumSyntax); break;
					case Languages.VisualBasic: formatter = new VBEnumerationFormatter(syntax as EnumSyntax); break;
				}
			}
			else if (syntax is ConstantSyntax) {
				switch (language) {
					case Languages.CSharp: formatter = new CSharpConstantFormatter(syntax as ConstantSyntax); break;
					case Languages.VisualBasic: formatter = new VBConstantFormatter(syntax as ConstantSyntax); break;
				}
			}
			else if (syntax is FieldSyntax) {
				switch (language) {
					case Languages.CSharp: formatter = new CSharpFieldFormatter(syntax as FieldSyntax); break;
					case Languages.VisualBasic: formatter = new VBFieldFormatter(syntax as FieldSyntax); break;
				}
			}
			else if (syntax is StructSyntax) {
				switch (language) {
					case Languages.CSharp: formatter = new CSharpStructFormatter(syntax as StructSyntax); break;
					case Languages.VisualBasic: formatter = new VBStructFormatter(syntax as StructSyntax); break;
				}
			}
			else if (syntax is MethodSyntax) {
				switch (language) {
					case Languages.CSharp: formatter = new CSharpMethodFormatter(syntax as MethodSyntax); break;
					case Languages.VisualBasic: formatter = new VBMethodFormatter(syntax as MethodSyntax); break;
				}
			}
			else if (syntax is OperatorSyntax) {
				switch (language) {
					case Languages.CSharp: formatter = new CSharpOperatorFormatter(syntax as OperatorSyntax); break;
					case Languages.VisualBasic: formatter = new VBOperatorFormatter(syntax as OperatorSyntax); break;
				}
			}
			else if (syntax is ConstructorSyntax) {
				switch (language) {
					case Languages.CSharp: formatter = new CSharpConstructorFormatter(syntax as ConstructorSyntax); break;
					case Languages.VisualBasic: formatter = new VBConstructorFormatter(syntax as ConstructorSyntax); break;
				}
			}
			else if (syntax is EventSyntax) {
				switch (language) {
					case Languages.CSharp: formatter = new CSharpEventFormatter(syntax as EventSyntax); break;
					case Languages.VisualBasic: formatter = new VBEventFormatter(syntax as EventSyntax); break;
				}
			}
			else if (syntax is PropertySyntax) {
				switch (language) {
					case Languages.CSharp: formatter = new CSharpPropertyFormatter(syntax as PropertySyntax); break;
					case Languages.VisualBasic: formatter = new VBPropertyFormatter(syntax as PropertySyntax); break;
				}
			}
			else if (syntax is IndexorSyntax) {
				switch (language) {
					case Languages.CSharp: formatter = new CSharpIndexerFormatter(syntax as IndexorSyntax); break;
					case Languages.VisualBasic: formatter = new VBIndexorFormatter(syntax as IndexorSyntax); break;
				}
			}
			else if (syntax is DelegateSyntax) {
				switch (language) {
					case Languages.CSharp: formatter = new CSharpDelegateFormatter(syntax as DelegateSyntax); break;
					case Languages.VisualBasic: formatter = new VBDelegateFormatter(syntax as DelegateSyntax); break;
				}
			}

			return formatter;
		}
	}
}
