
namespace TheBoxSoftware.Reflection.Signitures
{
    using Core.COFF;

    internal class SignitureBuilder
    {
        private readonly BlobStream _stream;

        public SignitureBuilder(BlobStream underlyingStream)
        {
            _stream = underlyingStream;
        }

        public Signiture Read(int offset)
        {
            if(offset < 0 || offset >= _stream.GetLength())
                return null;

            // first byte is supposed to the length
            throw new System.NotImplementedException();
        }
    }
}
