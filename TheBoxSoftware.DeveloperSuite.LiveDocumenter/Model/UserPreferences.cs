using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Model {
	using TheBoxSoftware.Reflection.Syntax;

	[Serializable]
	public class UserPreferences {
		private BuildConfigurations buildConfiguration = BuildConfigurations.Debug;
		private Languages language = Languages.CSharp;

		/// <summary>
		/// Gets or sets the user selected build configuration, used when
		/// loading projects and solutions
		/// </summary>
		public BuildConfigurations BuildConfiguration {
			get { return this.buildConfiguration; }
			set { this.buildConfiguration = value; } 
		}

		public Languages Language {
			get { return this.language; }
			set { this.language = value; }
		}
	}
}
