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
            foreach (KeyValuePair<string, int> current in l.Components)
            {
                Console.WriteLine(string.Format("component: {0} version: {1}", current.Key, current.Value));
            }
        }
    }
}
