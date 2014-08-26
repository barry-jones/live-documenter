using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheBoxSoftware.Reflection;
using TheBoxSoftware.Documentation;

namespace TheBoxSoftware.API.LiveDocumentor
{
    public sealed class ContentsEntry
    {
        private TheBoxSoftware.Documentation.Entry entry;                   // the internal representation of the entry data

        internal ContentsEntry(TheBoxSoftware.Documentation.Entry entry)
        {
            this.entry = entry;
        }

        public string CRefPath
        {
            get 
            {
                if (entry.Item is ReflectedMember && !(entry.Item is AssemblyDef))  // assemblies cant be cref'd
                { 
                    ReflectedMember member = entry.Item as ReflectedMember;
                    return TheBoxSoftware.Reflection.Comments.CRefPath.Create(member).ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public long Key
        {
            get { return this.entry.Key; }
        }

        public string SubKey
        {
            get { return this.entry.SubKey; }
        }

        public string DisplayName
        {
            get { return this.entry.Name; }
        }
    }
}
