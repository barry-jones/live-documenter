using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Documentation
{
	class DocumentMapper
	{
		public static DocumentMap Generate(List<DocumentedAssembly> assemblies, Mappers typeOfMapper)
		{
			switch (typeOfMapper)
			{
				case Mappers.AssemblyFirst:
					return new DocumentMap();

				case Mappers.NamespaceFirst:
				default:
					return new DocumentMap();
			}
		}
	}
}
