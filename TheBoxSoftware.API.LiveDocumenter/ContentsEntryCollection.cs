using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.API.LiveDocumentor
{
    class ContentsEntryCollection : IEnumerable<ContentsEntry>
    {
        private List<ContentsEntry> contents;



        #region IEnumerable<ContentsEntry> Members

        public IEnumerator<ContentsEntry> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
