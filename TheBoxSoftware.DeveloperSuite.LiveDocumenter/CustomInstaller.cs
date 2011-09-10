using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;


namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter {
	[RunInstaller(true)]
	public partial class CustomInstaller : System.Configuration.Install.Installer {
		public CustomInstaller() {
			InitializeComponent();
		}
	}
}
