using System;
using System.ComponentModel;
using System.Reflection;
using System.Configuration.Install;
using IWshRuntimeLibrary;

namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter {
	/// <summary>
	/// Custom installer actions for the Live Documenter setup project.
	/// </summary>
	partial class CustomInstaller {
		const string ALLUSERS = "ALLUSERS";
		const string DESKTOPSHORTCUT = "DESKTOP_SHORTCUT";
		const string STARTMENUSHORTCUT = "STARTMENU_SHORTCUT";
		const string QUICKLAUNCHSHORTCUT = "QUICKLAUNCH_SHORTCUT";
		private string location;
		private string name;
		private string description;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			components = new System.ComponentModel.Container();
		}

		#endregion

		#region Properties
		private string ShortcutTarget {
			get {
				if (string.IsNullOrEmpty(location))
					this.location = Assembly.GetExecutingAssembly().Location;
				return this.location;
			}
		}

		private string ShortcutName {
			get {
				if (string.IsNullOrEmpty(this.name)) {
					Assembly myAssembly = Assembly.GetExecutingAssembly();

					try {
						object titleAttribute = myAssembly.GetCustomAttributes(typeof(AssemblyProductAttribute), false)[0];
						this.name = ((AssemblyProductAttribute)titleAttribute).Product;
					}
					catch { }

					if (string.IsNullOrWhiteSpace(this.name.Trim()))
						this.name = myAssembly.GetName().Name;
				}
				return this.name;
			}
		}
		
		private string ShortcutDescription {
			get {
				if (string.IsNullOrEmpty(this.description)) {
					Assembly myAssembly = Assembly.GetExecutingAssembly();

					try {
						object descriptionAttribute = myAssembly.GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false)[0];
						this.description = ((AssemblyDescriptionAttribute)descriptionAttribute).Description;
					}
					catch { }

					if (string.IsNullOrWhiteSpace(this.description))
						this.description = "Launch " + ShortcutName;
				}
				return this.description;
			}
		}
		#endregion

		public override void Install(System.Collections.IDictionary stateSaver) {
			base.Install(stateSaver);

			bool allUsers = !string.IsNullOrEmpty(this.Context.Parameters[ALLUSERS]);
			bool desktopShortcut = !string.IsNullOrEmpty(this.Context.Parameters[DESKTOPSHORTCUT]);
			bool quickLaunchShortcut = !string.IsNullOrEmpty(this.Context.Parameters[QUICKLAUNCHSHORTCUT]);
			bool startMenuShortcut = !string.IsNullOrEmpty(this.Context.Parameters[STARTMENUSHORTCUT]);

			string desktopDir = System.Environment.GetFolderPath(allUsers
				? System.Environment.SpecialFolder.CommonDesktopDirectory
				: System.Environment.SpecialFolder.DesktopDirectory
				);
			string quicklaunchDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
					"\\Microsoft\\Internet Explorer\\Quick Launch";
			string startMenuDir = System.Environment.GetFolderPath(allUsers
				? System.Environment.SpecialFolder.CommonStartMenu
				: System.Environment.SpecialFolder.StartMenu
				);

			if (desktopShortcut) this.CreateShortcut(desktopDir);
			if (quickLaunchShortcut) this.CreateShortcut(quicklaunchDir);
			if (startMenuShortcut) this.CreateShortcut(startMenuDir);
		}

		public override void Rollback(System.Collections.IDictionary savedState) {
			base.Rollback(savedState);
			this.DeleteShortcuts();
		}

		public override void Uninstall(System.Collections.IDictionary savedState) {
			base.Uninstall(savedState);
			this.DeleteShortcuts();
		}

		#region Internal methods
		private void CreateShortcut(string folder) {
			string shortcutFullname = System.IO.Path.Combine(folder, this.ShortcutName + ".lnk");

			WshShell shell = new WshShellClass();
			IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutFullname);
			shortcut.TargetPath = this.ShortcutTarget;
			shortcut.Description = this.ShortcutDescription;
			shortcut.Save();
		}

		private void DeleteShortcuts() {
			string[] allShortcuts = new string[] {
				System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonDesktopDirectory),
				System.Environment.GetFolderPath(System.Environment.SpecialFolder.DesktopDirectory),
				Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Microsoft\\Internet Explorer\\Quick Launch",
				System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonStartMenu),
				System.Environment.GetFolderPath(System.Environment.SpecialFolder.StartMenu)
			};
			foreach (string shortcut in allShortcuts) {
				string fullname = System.IO.Path.Combine(shortcut, this.ShortcutName + ".lnk");
				if (System.IO.File.Exists(fullname)) {
					System.IO.File.Delete(fullname);
				}
			}
		}
		#endregion
	}
}