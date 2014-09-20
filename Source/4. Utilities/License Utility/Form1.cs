using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace License_Utility
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Clears all of the form controls in preperation for a load or create.
        /// </summary>
        private void ClearControls()
        {
            this.txtApiVersion.Text = string.Empty;
            this.txtDesktopVersion.Text = string.Empty;
            this.txtServerVersion.Text = string.Empty;
            this.chkAPI.Checked = false;
            this.chkDesktop.Checked = false;
            this.chkServer.Checked = false;
            this.dtEnd.Value = this.dtEnd.MinDate;
        }

        /// <summary>
        /// Populates the form fields with the details of the provided <paramref name="license"/>.
        /// </summary>
        /// <param name="license">The license to populate the fields from.</param>
        private void PopulateFields(TheBoxSoftware.Licensing.License license)
        {
            foreach (string component in license.Components.Keys)
            {
                switch (component)
                {
                    case "ld-desktop":
                        this.txtDesktopVersion.Text = license.Components[component].ToString();
                        this.chkDesktop.Checked = true;
                        break;
                    case "ld-server":
                        this.txtServerVersion.Text = license.Components[component].ToString();
                        this.chkServer.Checked = true;
                        break;
                    case "ld-api":
                        this.txtApiVersion.Text = license.Components[component].ToString();
                        this.chkAPI.Checked = true;
                        break;
                }
            }
            this.txtName.Text = license.Name;
            this.txtEmail.Text = license.Email;
            if (license.End != DateTime.MinValue)
            {
                this.dtEnd.Value = license.End;
            }
        }

        /// <summary>
        /// Reads the details of a license from the fields and populates a new <see cref="TheBoxSoftware.Licensing.License"/>
        /// object.
        /// </summary>
        /// <returns>The populated License.</returns>
        private TheBoxSoftware.Licensing.License GetLicenseFromFields()
        {
            TheBoxSoftware.Licensing.License license = new TheBoxSoftware.Licensing.License();

            if (this.chkDesktop.Checked)
            {
                license.Components.Add("ld-desktop", int.Parse(this.txtDesktopVersion.Text));
            }
            if (this.chkServer.Checked)
            {
                license.Components.Add("ld-server", int.Parse(this.txtServerVersion.Text));
            }
            if (this.chkAPI.Checked)
            {
                license.Components.Add("ld-api", int.Parse(this.txtApiVersion.Text));
            }
            license.Name = this.txtName.Text;
            license.Email = this.txtEmail.Text;
            if(this.dtEnd.Value != this.dtEnd.MinDate)
            {
                license.End = this.dtEnd.Value;
            }

            return license;
        }

        /// <summary>
        /// Opens the file dialogue box so the user can read an existing license file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbOpen_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
            string[] filters = new string[] {
				"License Files (.lic)|*.lic"
				};
            ofd.Filter = string.Join("|", filters);
            ofd.AutoUpgradeEnabled = true;
            ofd.Multiselect = false;
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.Cursor = Cursors.WaitCursor;
                try
                {
                    if (!string.IsNullOrEmpty(ofd.FileName))
                    {
                        this.ClearControls();
                        this.PopulateFields(TheBoxSoftware.Licensing.License.Decrypt(ofd.FileName));
                    }
                }
                finally
                {
                    this.Cursor = null;
                    GC.Collect();
                }
            }
        }

        /// <summary>
        /// Saves the details of the license to a new license file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGenerate_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                System.Windows.Forms.SaveFileDialog sfd = new System.Windows.Forms.SaveFileDialog();
                sfd.AddExtension = true;
                sfd.AutoUpgradeEnabled = true;
                sfd.CreatePrompt = false;
                sfd.DefaultExt = "lic";
                sfd.Filter = "License (.lic)|*.lic";
                sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (!string.IsNullOrEmpty(sfd.FileName))
                    {
                        TheBoxSoftware.Licensing.License license = this.GetLicenseFromFields();
                        System.IO.File.WriteAllBytes(sfd.FileName, license.Encrypt());
                    }
                }
            }
            finally
            {
                this.Cursor = null;
            }
        }

        /// <summary>
        /// Clears all the fields in preperation for creating a new license.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbNew_Click(object sender, EventArgs e)
        {
            this.ClearControls();
        }
    }
}
