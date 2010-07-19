using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Reflection.Core {
	/// <summary>
	/// Represents a version number
	/// </summary>
	[System.Diagnostics.DebuggerDisplay("Version: {ToString()}")]
	public struct Version {
		/// <summary>The major version number</summary>
		public int Major;
		/// <summary>The minor version number</summary>
		public int Minor;
		/// <summary>The build number</summary>
		public int Build;
		/// <summary>The build revision number</summary>
		public int Revision;

		/// <summary>
		/// Creates a new Version with the specified details
		/// </summary>
		/// <param name="major">The major number</param>
		/// <param name="minor">The minor number</param>
		/// <param name="build">The build number</param>
		/// <param name="revision">The build revision</param>
		public Version(int major, int minor, int build, int revision) {
			this.Major = major;
			this.Minor = minor;
			this.Build = build;
			this.Revision = revision;
		}

		/// <summary>
		/// Creates a new Version with the specified details
		/// </summary>
		/// <param name="major">The major number</param>
		/// <param name="minor">The minor number</param>
		/// <param name="build">The build number</param>
		/// <param name="revision">The build revision</param>
		public Version(string major, string minor, string build, string revision) {
			int.TryParse(major, out this.Major);
			int.TryParse(minor, out this.Minor);
			int.TryParse(build, out this.Build);
			int.TryParse(revision, out this.Revision);
		}

		/// <summary>
		/// Returns a string representation of this version specified as
		/// "&gt;major&lt;.&gt;major&lt;.&gt;major&lt;.&gt;major&lt;"
		/// </summary>
		/// <returns>The string representation of this Version.</returns>
		public override string ToString() {
			return string.Format("{0}.{1}.{2}.{3}",
				this.Major,
				this.Minor,
				this.Build,
				this.Revision);
		}

		#region Operator Overloads
		public static bool operator < (Version version1, Version version2) {
			if(!(version1.Major < version2.Major)) {
				if(!(version1.Minor < version2.Minor)) {
					if(!(version1.Build < version2.Build)) {
						if(!(version1.Revision < version2.Revision)) {
							return false;
						}
					}
				}
			}
			return true;
		}

		public static bool operator > (Version version1, Version version2) {
			return !(version1 < version2);
		}

		public static bool operator == (Version version1, Version version2) {
			return (
				version1.Major == version2.Major &&
				version1.Minor == version2.Minor &&
				version1.Build == version2.Build &&
				version1.Revision == version2.Revision
				);
		}

		public static bool operator != (Version version1, Version version2) {
			return !(version1 == version2);
		}
		#endregion
	}
}
