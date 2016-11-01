
namespace TheBoxSoftware.Reflection.Syntax.VisualBasic
{
    public static class Constants
    {
        public static SyntaxToken Space = new SyntaxToken(" ", SyntaxTokens.Text);

        public static SyntaxToken GenericStart = new SyntaxToken("(Of", SyntaxTokens.Text);
        public static SyntaxToken GenericEnd = new SyntaxToken(")", SyntaxTokens.Text);

        public static SyntaxToken ArrayStart = new SyntaxToken("(", SyntaxTokens.Text);
        public static SyntaxToken ArrayEnd = new SyntaxToken(")", SyntaxTokens.Text);
        public static SyntaxToken ArrayEmpty = new SyntaxToken("()", SyntaxTokens.Text);

        public static SyntaxToken KeywordPointer = new SyntaxToken("*", SyntaxTokens.Text);


        public static SyntaxToken KeywordClass = new SyntaxToken("Class", SyntaxTokens.Keyword);
        public static SyntaxToken KeywordFunction = new SyntaxToken("Function", SyntaxTokens.Keyword);
        public static SyntaxToken KeywordSub = new SyntaxToken("Sub", SyntaxTokens.Keyword);
        public static SyntaxToken KeywordAs = new SyntaxToken("As", SyntaxTokens.Keyword);

        // access modifiers
        public static SyntaxToken KeywordPrivate = new SyntaxToken("Private", SyntaxTokens.Keyword);
        public static SyntaxToken KeywordProtected = new SyntaxToken("Protected", SyntaxTokens.Keyword);
        public static SyntaxToken KeywordInternal = new SyntaxToken("Friend", SyntaxTokens.Keyword);
        public static SyntaxToken KeywordPublic = new SyntaxToken("Public", SyntaxTokens.Keyword);


        // modifiers
        public static SyntaxToken KeywordAbstract = new SyntaxToken("MustInherit", SyntaxTokens.Keyword);
        public static SyntaxToken KeywordSealed = new SyntaxToken("NotInheritable", SyntaxTokens.Keyword);
        public static SyntaxToken KeywordStatic = new SyntaxToken("Shared", SyntaxTokens.Keyword);


        // Well known Types
        public static SyntaxToken TypeObject = new SyntaxToken("Object", SyntaxTokens.Keyword);
        public static SyntaxToken TypeBoolean = new SyntaxToken("Boolean", SyntaxTokens.Keyword);
        public static SyntaxToken TypeSByte = new SyntaxToken("SByte", SyntaxTokens.Keyword);
        public static SyntaxToken TypeByte = new SyntaxToken("Byte", SyntaxTokens.Keyword);
        public static SyntaxToken TypeChar = new SyntaxToken("Char", SyntaxTokens.Keyword);
        public static SyntaxToken TypeDouble = new SyntaxToken("Double", SyntaxTokens.Keyword);
        public static SyntaxToken TypeShort = new SyntaxToken("Short", SyntaxTokens.Keyword);
        public static SyntaxToken TypeInt = new SyntaxToken("Int", SyntaxTokens.Keyword);
        public static SyntaxToken TypeLong = new SyntaxToken("Long", SyntaxTokens.Keyword);
        public static SyntaxToken TypeFloat = new SyntaxToken("Float", SyntaxTokens.Keyword);
        public static SyntaxToken TypeString = new SyntaxToken("String", SyntaxTokens.Keyword);
        public static SyntaxToken TypeUShort = new SyntaxToken("UShort", SyntaxTokens.Keyword);
        public static SyntaxToken TypeUInt = new SyntaxToken("UInt", SyntaxTokens.Keyword);
        public static SyntaxToken TypeULong = new SyntaxToken("ULong", SyntaxTokens.Keyword);
        public static SyntaxToken TypeVoid = new SyntaxToken("Void", SyntaxTokens.Keyword);
    }
}
