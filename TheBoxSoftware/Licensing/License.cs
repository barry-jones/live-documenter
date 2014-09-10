using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheBoxSoftware.Licencing
{
    [Serializable]
    public sealed class License
    {
        private DateTime purchaseDate;
        private DateTime licencedUntil;
        private List<string> components;

        public static License Decrypt()
        {
            string pubKey = "";


            return new License();
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public List<string> Components
        {
            get { return this.components; }
            set { this.components = value; }
        }

        public DateTime LicencedUntil
        {
            get { return this.licencedUntil; }
            set { this.licencedUntil = value; }
        }

        public DateTime PurchaseDate
        {
            get { return this.purchaseDate; }
            set { this.purchaseDate = value; }
        }
    }
}
