using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bug14
{
    public class Inherited6 : GenericParent<GenericParent<int>>
    {
    }
}
