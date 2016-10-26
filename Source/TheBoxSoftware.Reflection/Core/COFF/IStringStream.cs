using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Core.COFF
{
    public interface IStringStream
    {
        string GetString(int index);

        Dictionary<int, string> GetAllStrings();
    }
}
