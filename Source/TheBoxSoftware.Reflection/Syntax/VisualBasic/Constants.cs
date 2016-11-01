
namespace TheBoxSoftware.Reflection.Syntax.VisualBasic
{
    public static class Constants
    {
        public readonly static SyntaxToken Space = new SyntaxToken(" ", SyntaxTokens.Text);

        public readonly static SyntaxToken GenericStart = new SyntaxToken("(Of", SyntaxTokens.Text);
        public readonly static SyntaxToken GenericEnd = new SyntaxToken(")", SyntaxTokens.Text);

        public readonly static SyntaxToken ArrayStart = new SyntaxToken("(", SyntaxTokens.Text);
        public readonly static SyntaxToken ArrayEnd = new SyntaxToken(")", SyntaxTokens.Text);
        public readonly static SyntaxToken ArrayEmpty = new SyntaxToken("()", SyntaxTokens.Text);

        public readonly static SyntaxToken KeywordPointer = new SyntaxToken("*", SyntaxTokens.Text);


        public readonly static SyntaxToken KeywordClass = new SyntaxToken("Class", SyntaxTokens.Keyword);
        public readonly static SyntaxToken KeywordFunction = new SyntaxToken("Function", SyntaxTokens.Keyword);
        public readonly static SyntaxToken KeywordSub = new SyntaxToken("Sub", SyntaxTokens.Keyword);
        public readonly static SyntaxToken KeywordAs = new SyntaxToken("As", SyntaxTokens.Keyword);

        // access modifiers
        public readonly static SyntaxToken KeywordPrivate = new SyntaxToken("Private", SyntaxTokens.Keyword);
        public readonly static SyntaxToken KeywordProtected = new SyntaxToken("Protected", SyntaxTokens.Keyword);
        public readonly static SyntaxToken KeywordInternal = new SyntaxToken("Friend", SyntaxTokens.Keyword);
        public readonly static SyntaxToken KeywordPublic = new SyntaxToken("Public", SyntaxTokens.Keyword);


        // modifiers
        public readonly static SyntaxToken KeywordAbstract = new SyntaxToken("MustInherit", SyntaxTokens.Keyword);
        public readonly static SyntaxToken KeywordSealed = new SyntaxToken("NotInheritable", SyntaxTokens.Keyword);
        public readonly static SyntaxToken KeywordStatic = new SyntaxToken("Shared", SyntaxTokens.Keyword);


        // Well known Types
        public readonly static SyntaxToken TypeObject = new SyntaxToken("Object", SyntaxTokens.Keyword);
        public readonly static SyntaxToken TypeBoolean = new SyntaxToken("Boolean", SyntaxTokens.Keyword);
        public readonly static SyntaxToken TypeSByte = new SyntaxToken("SByte", SyntaxTokens.Keyword);
        public readonly static SyntaxToken TypeByte = new SyntaxToken("Byte", SyntaxTokens.Keyword);
        public readonly static SyntaxToken TypeChar = new SyntaxToken("Char", SyntaxTokens.Keyword);
        public readonly static SyntaxToken TypeDouble = new SyntaxToken("Double", SyntaxTokens.Keyword);
        public readonly static SyntaxToken TypeShort = new SyntaxToken("Short", SyntaxTokens.Keyword);
        public readonly static SyntaxToken TypeInt = new SyntaxToken("Int", SyntaxTokens.Keyword);
        public readonly static SyntaxToken TypeLong = new SyntaxToken("Long", SyntaxTokens.Keyword);
        public readonly static SyntaxToken TypeFloat = new SyntaxToken("Float", SyntaxTokens.Keyword);
        public readonly static SyntaxToken TypeString = new SyntaxToken("String", SyntaxTokens.Keyword);
        public readonly static SyntaxToken TypeUShort = new SyntaxToken("UShort", SyntaxTokens.Keyword);
        public readonly static SyntaxToken TypeUInt = new SyntaxToken("UInt", SyntaxTokens.Keyword);
        public readonly static SyntaxToken TypeULong = new SyntaxToken("ULong", SyntaxTokens.Keyword);
        public readonly static SyntaxToken TypeVoid = new SyntaxToken("Void", SyntaxTokens.Keyword);
    }
}
