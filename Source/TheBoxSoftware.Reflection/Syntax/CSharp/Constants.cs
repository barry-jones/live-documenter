
namespace TheBoxSoftware.Reflection.Syntax.CSharp
{
    /// <summary>
    /// Set of constant instances which represent the common tokens used for c sharp
    /// syntax formatting.
    /// </summary>
    /// <seealso cref="TheBoxSoftware.Relection.Syntax.VisualBasic.Constants"/>
    public static class Constants
    {
        public static SyntaxToken Space = new SyntaxToken(" ", SyntaxTokens.Text);

        public static SyntaxToken GenericStart = new SyntaxToken("<", SyntaxTokens.Text);
        public static SyntaxToken GenericEnd = new SyntaxToken(">", SyntaxTokens.Text);

        public static SyntaxToken ArrayStart = new SyntaxToken("[", SyntaxTokens.Text);
        public static SyntaxToken ArrayEnd = new SyntaxToken("]", SyntaxTokens.Text);
        public static SyntaxToken ArrayEmpty = new SyntaxToken("[]", SyntaxTokens.Text);


        public static SyntaxToken KeywordThis = new SyntaxToken("this", SyntaxTokens.Keyword);
        public static SyntaxToken KeywordClass = new SyntaxToken("class", SyntaxTokens.Keyword);
        public static SyntaxToken KeywordPointer = new SyntaxToken("*", SyntaxTokens.Text);
        public static SyntaxToken KeywordConstant = new SyntaxToken("const", SyntaxTokens.Keyword);
        public static SyntaxToken KeywordDelegate = new SyntaxToken("delegate", SyntaxTokens.Keyword);
        public static SyntaxToken KeywordEnumeration = new SyntaxToken("enum", SyntaxTokens.Keyword);
        public static SyntaxToken KeywordEvent = new SyntaxToken("event", SyntaxTokens.Keyword);
        public static SyntaxToken KeywordEventAdd = new SyntaxToken("add", SyntaxTokens.Keyword);
        public static SyntaxToken KeywordEventRemove = new SyntaxToken("remove", SyntaxTokens.Keyword);
        public static SyntaxToken KeywordGet = new SyntaxToken("get", SyntaxTokens.Keyword);
        public static SyntaxToken KeywordSet = new SyntaxToken("set", SyntaxTokens.Keyword);
        public static SyntaxToken KeywordInterface = new SyntaxToken("interface", SyntaxTokens.Keyword);
        public static SyntaxToken KeywordImplicit = new SyntaxToken("implicit", SyntaxTokens.Keyword);
        public static SyntaxToken KeywordExplicit = new SyntaxToken("explicit", SyntaxTokens.Keyword);
        public static SyntaxToken KeywordOperator = new SyntaxToken("operator", SyntaxTokens.Keyword);
        public static SyntaxToken KeywordStruct = new SyntaxToken("struct", SyntaxTokens.Keyword);


        public static SyntaxToken KeywordRef = new SyntaxToken("ref", SyntaxTokens.Keyword);
        public static SyntaxToken KeywordOut = new SyntaxToken("out", SyntaxTokens.Keyword);

        
        // access modifiers
        public static SyntaxToken KeywordPrivate = new SyntaxToken("private", SyntaxTokens.Keyword);
        public static SyntaxToken KeywordProtected = new SyntaxToken("protected", SyntaxTokens.Keyword);
        public static SyntaxToken KeywordInternal = new SyntaxToken("internal", SyntaxTokens.Keyword);
        public static SyntaxToken KeywordPublic = new SyntaxToken("public", SyntaxTokens.Keyword);


        // inheritance modifiers
        public static SyntaxToken KeywordAbstract = new SyntaxToken("abstract", SyntaxTokens.Keyword);
        public static SyntaxToken KeywordSealed = new SyntaxToken("sealed", SyntaxTokens.Keyword);
        public static SyntaxToken KeywordStatic = new SyntaxToken("static", SyntaxTokens.Keyword);


        // Well known Types
        public static SyntaxToken TypeObject = new SyntaxToken("object", SyntaxTokens.Keyword);
        public static SyntaxToken TypeBoolean = new SyntaxToken("bool", SyntaxTokens.Keyword);
        public static SyntaxToken TypeSByte = new SyntaxToken("sbyte", SyntaxTokens.Keyword);
        public static SyntaxToken TypeByte = new SyntaxToken("byte", SyntaxTokens.Keyword);
        public static SyntaxToken TypeChar = new SyntaxToken("char", SyntaxTokens.Keyword);
        public static SyntaxToken TypeDouble = new SyntaxToken("double", SyntaxTokens.Keyword);
        public static SyntaxToken TypeShort = new SyntaxToken("short", SyntaxTokens.Keyword);
        public static SyntaxToken TypeInt = new SyntaxToken("int", SyntaxTokens.Keyword);
        public static SyntaxToken TypeLong = new SyntaxToken("long", SyntaxTokens.Keyword);
        public static SyntaxToken TypeFloat = new SyntaxToken("float", SyntaxTokens.Keyword);
        public static SyntaxToken TypeString = new SyntaxToken("string", SyntaxTokens.Keyword);
        public static SyntaxToken TypeUShort = new SyntaxToken("ushort", SyntaxTokens.Keyword);
        public static SyntaxToken TypeUInt = new SyntaxToken("uint", SyntaxTokens.Keyword);
        public static SyntaxToken TypeULong = new SyntaxToken("ulong", SyntaxTokens.Keyword);
        public static SyntaxToken TypeVoid = new SyntaxToken("void", SyntaxTokens.Keyword);
    }
}
