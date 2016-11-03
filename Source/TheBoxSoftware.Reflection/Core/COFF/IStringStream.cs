
namespace TheBoxSoftware.Reflection.Core.COFF
{
    using System.Collections.Generic;

    public interface IStringStream
    {
        string GetString(uint index);

        Dictionary<int, string> GetAllStrings();
    }
}
