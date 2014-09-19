using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheBoxSoftware.Licensing;

namespace Test.Licensing
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Program p = new Program();
            p.TestGenerateLicenseFile();
            p.TestReadLicenseFile();

            p.GenerateExpiredLicense();
            p.GenerateNoComponentLicense();
            p.GenerateOldVersionLicense();
            p.GenerateValidLicense();
        }

        private void TestGenerateLicenseFile()
        {
            License l = new License();
            l.Name = "Barry Jones";
            l.Email = "licensed@barryjones.me.uk";
            l.Start = DateTime.Now;

            l.Components.Add("ld-desktop", 1);
            l.Components.Add("ld-server", 1);
            l.Components.Add("ld-api", 1);

            byte[] file = l.Encrypt();
            System.IO.File.WriteAllBytes(@"test.lic", file);
        }

        private void TestReadLicenseFile()
        {
            License l = License.Decrypt(@"test.lic");

            Console.WriteLine(l.Name);
            Console.WriteLine(l.Email);
            Console.WriteLine(l.Start.ToShortDateString());
            if (l.End != null && l.End != DateTime.MinValue)
            {
                Console.WriteLine(l.End.ToShortDateString());
            }
            foreach (KeyValuePair<string, int> current in l.Components)
            {
                Console.WriteLine(string.Format("component: {0} version: {1}", current.Key, current.Value));
            }
        }

        private void GenerateExpiredLicense()
        {
            License l = new License();
            l.Name = "Barry Jones";
            l.Email = "licensed@barryjones.me.uk";
            l.Start = DateTime.Now;
            l.End = DateTime.Parse("2014-09-09");

            l.Components.Add("ld-desktop", 1);
            l.Components.Add("ld-server", 1);
            l.Components.Add("ld-api", 1);

            byte[] file = l.Encrypt();
            System.IO.File.WriteAllBytes(@"test-expired.lic", file);
        }

        private void GenerateOldVersionLicense()
        {
            License l = new License();
            l.Name = "Barry Jones";
            l.Email = "licensed@barryjones.me.uk";
            l.Start = DateTime.Now;

            l.Components.Add("ld-desktop", 0);
            l.Components.Add("ld-server", 0);
            l.Components.Add("ld-api", 0);

            byte[] file = l.Encrypt();
            System.IO.File.WriteAllBytes(@"test-invalidversion.lic", file);
        }

        private void GenerateNoComponentLicense()
        {
            License l = new License();
            l.Name = "Barry Jones";
            l.Email = "licensed@barryjones.me.uk";
            l.Start = DateTime.Now;

            byte[] file = l.Encrypt();
            System.IO.File.WriteAllBytes(@"test-nocomponent.lic", file);
        }

        private void GenerateValidLicense()
        {
            License l = new License();
            l.Name = "Barry Jones";
            l.Email = "licensed@barryjones.me.uk";
            l.Start = DateTime.Now;

            l.Components.Add("ld-desktop", 10);
            l.Components.Add("ld-server", 10);
            l.Components.Add("ld-api", 10);

            byte[] file = l.Encrypt();
            System.IO.File.WriteAllBytes(@"test-valid.lic", file);
        }
    }
}
