
namespace TheBoxSoftware.Reflection.Syntax.CSharp
{
    /// <summary>
    /// Set of constant instances which represent the common tokens used for c sharp
    /// syntax formatting.
    /// </summary>
    /// <seealso cref="TheBoxSoftware.Relection.Syntax.VisualBasic.Constants"/>
    public static class Constants
    {
        public readonly static SyntaxToken Space = new SyntaxToken(" ", SyntaxTokens.Text);

        public readonly static SyntaxToken GenericStart = new SyntaxToken("<", SyntaxTokens.Text);
        public readonly static SyntaxToken GenericEnd = new SyntaxToken(">", SyntaxTokens.Text);

        public readonly static SyntaxToken ArrayStart = new SyntaxToken("[", SyntaxTokens.Text);
        public readonly static SyntaxToken ArrayEnd = new SyntaxToken("]", SyntaxTokens.Text);
        public readonly static SyntaxToken ArrayEmpty = new SyntaxToken("[]", SyntaxTokens.Text);


        public readonly static SyntaxToken KeywordThis = new SyntaxToken("this", SyntaxTokens.Keyword);
        public readonly static SyntaxToken KeywordClass = new SyntaxToken("class", SyntaxTokens.Keyword);
        public readonly static SyntaxToken KeywordPointer = new SyntaxToken("*", SyntaxTokens.Text);
        public readonly static SyntaxToken KeywordConstant = new SyntaxToken("const", SyntaxTokens.Keyword);
        public readonly static SyntaxToken KeywordDelegate = new SyntaxToken("delegate", SyntaxTokens.Keyword);
        public readonly static SyntaxToken KeywordEnumeration = new SyntaxToken("enum", SyntaxTokens.Keyword);
        public readonly static SyntaxToken KeywordEvent = new SyntaxToken("event", SyntaxTokens.Keyword);
        public readonly static SyntaxToken KeywordEventAdd = new SyntaxToken("add", SyntaxTokens.Keyword);
        public readonly static SyntaxToken KeywordEventRemove = new SyntaxToken("remove", SyntaxTokens.Keyword);
        public readonly static SyntaxToken KeywordGet = new SyntaxToken("get", SyntaxTokens.Keyword);
        public readonly static SyntaxToken KeywordSet = new SyntaxToken("set", SyntaxTokens.Keyword);
        public readonly static SyntaxToken KeywordInterface = new SyntaxToken("interface", SyntaxTokens.Keyword);
        public readonly static SyntaxToken KeywordImplicit = new SyntaxToken("implicit", SyntaxTokens.Keyword);
        public readonly static SyntaxToken KeywordExplicit = new SyntaxToken("explicit", SyntaxTokens.Keyword);
        public readonly static SyntaxToken KeywordOperator = new SyntaxToken("operator", SyntaxTokens.Keyword);
        public readonly static SyntaxToken KeywordStruct = new SyntaxToken("struct", SyntaxTokens.Keyword);


        public readonly static SyntaxToken KeywordRef = new SyntaxToken("ref", SyntaxTokens.Keyword);
        public readonly static SyntaxToken KeywordOut = new SyntaxToken("out", SyntaxTokens.Keyword);


        // access modifiers
        public readonly static SyntaxToken KeywordPrivate = new SyntaxToken("private", SyntaxTokens.Keyword);
        public readonly static SyntaxToken KeywordProtected = new SyntaxToken("protected", SyntaxTokens.Keyword);
        public readonly static SyntaxToken KeywordInternal = new SyntaxToken("internal", SyntaxTokens.Keyword);
        public readonly static SyntaxToken KeywordPublic = new SyntaxToken("public", SyntaxTokens.Keyword);


        // inheritance modifiers
        public readonly static SyntaxToken KeywordAbstract = new SyntaxToken("abstract", SyntaxTokens.Keyword);
        public readonly static SyntaxToken KeywordSealed = new SyntaxToken("sealed", SyntaxTokens.Keyword);
        public readonly static SyntaxToken KeywordStatic = new SyntaxToken("static", SyntaxTokens.Keyword);


        // Well known Types
        public readonly static SyntaxToken TypeObject = new SyntaxToken("object", SyntaxTokens.Keyword);
        public readonly static SyntaxToken TypeBoolean = new SyntaxToken("bool", SyntaxTokens.Keyword);
        public readonly static SyntaxToken TypeSByte = new SyntaxToken("sbyte", SyntaxTokens.Keyword);
        public readonly static SyntaxToken TypeByte = new SyntaxToken("byte", SyntaxTokens.Keyword);
        public readonly static SyntaxToken TypeChar = new SyntaxToken("char", SyntaxTokens.Keyword);
        public readonly static SyntaxToken TypeDouble = new SyntaxToken("double", SyntaxTokens.Keyword);
        public readonly static SyntaxToken TypeShort = new SyntaxToken("short", SyntaxTokens.Keyword);
        public readonly static SyntaxToken TypeInt = new SyntaxToken("int", SyntaxTokens.Keyword);
        public readonly static SyntaxToken TypeLong = new SyntaxToken("long", SyntaxTokens.Keyword);
        public readonly static SyntaxToken TypeFloat = new SyntaxToken("float", SyntaxTokens.Keyword);
        public readonly static SyntaxToken TypeString = new SyntaxToken("string", SyntaxTokens.Keyword);
        public readonly static SyntaxToken TypeUShort = new SyntaxToken("ushort", SyntaxTokens.Keyword);
        public readonly static SyntaxToken TypeUInt = new SyntaxToken("uint", SyntaxTokens.Keyword);
        public readonly static SyntaxToken TypeULong = new SyntaxToken("ulong", SyntaxTokens.Keyword);
        public readonly static SyntaxToken TypeVoid = new SyntaxToken("void", SyntaxTokens.Keyword);
    }
}
