using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;

namespace TheBoxSoftware.Licencing
{
    /// <summary>
    /// Represents the details of a License which determines what functionliaty (if any) is available
    /// at run time.
    /// </summary>
    [Serializable]
    public sealed class Licence
    {
        private const string phrase = "49b76s9954";
        private DateTime start;
        private DateTime end;
        private string name;
        private string email;
        private Dictionary<string,int> components = new Dictionary<string,int>();

        // This constant string is used as a "salt" value for the PasswordDeriveBytes function calls.
        // This size of the IV (in bytes) must = (keysize / 8).  Default keysize is 256, so the IV must be
        // 32 bytes long.  Using a 16 character string here gives us 32 bytes when converted to a byte array.
        private static readonly byte[] initVectorBytes = Encoding.ASCII.GetBytes("tu89geji340t89u2");

        // This constant is used to determine the keysize of the encryption algorithm.
        private const int keysize = 256;

        /// <summary>
        /// Performs a validation test on the license to check if the provided components,
        /// versions and time makes this a valid or invalid license for those criteria.
        /// </summary>
        /// <param name="component">The string representing the component to check.</param>
        /// <param name="version">The version to check the component for.</param>
        /// <returns>A ValidationInfo file that describes the results of the validation test.</returns>
        public ValidationInfo Validate(string component, string version)
        {
            ValidationInfo info = new ValidationInfo();

            if (this.End != null && this.End != DateTime.MinValue) // trial version test
            {
                if (DateTime.Now > this.End)
                {
                    info.HasExpired = true;
                }
            }

            foreach (KeyValuePair<string, int> current in this.Components)
            {
                if (string.Compare(current.Key, component) == 0)
                {
                    info.IsComponentValid = true;

                    // check the version (this test will mean we will need to distribute new licenses for version upgrades)
                    Version v = new Version(version);
                    if (v.Major > current.Value)
                    {
                        info.IsVersionInvalid = true;
                    }
                }
            }

            return info;
        }

        /// <summary>
        /// Encrypts the current instance to a byte array.
        /// </summary>
        /// <returns>A byte array of encrypted data.</returns>
        public byte[] Encrypt()
        {
            byte[] plainTextBytes = null;

            // serialize this class
            BinaryFormatter formatter = new BinaryFormatter();
            using(MemoryStream s = new MemoryStream())
            {
                formatter.Serialize(s, this);
                s.Seek(0, SeekOrigin.Begin);
                plainTextBytes = new byte[s.Length];
                s.Read(plainTextBytes, 0, (int)s.Length);
            }

            
            PasswordDeriveBytes password = new PasswordDeriveBytes(phrase, null);

            byte[] keyBytes = password.GetBytes(keysize / 8);
            using (RijndaelManaged symmetricKey = new RijndaelManaged())
            {
                symmetricKey.Mode = CipherMode.CBC;
                using (ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes))
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                        {
                            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                            cryptoStream.FlushFinalBlock();
                            byte[] cipherTextBytes = memoryStream.ToArray();
                            return cipherTextBytes;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Decrypts the specified file to an instance of License.
        /// </summary>
        /// <param name="file">The file to decrypt/</param>
        /// <returns>A license containing licensing information or null if license was not found or is invalid.</returns>
        public static Licence Decrypt(string file)
        {
            if (!File.Exists(file))
            {
                return null;
            }

            byte[] plainTextBytes;
            byte[] cipherTextBytes = File.ReadAllBytes(file);
            PasswordDeriveBytes password = new PasswordDeriveBytes(phrase, null);

            byte[] keyBytes = password.GetBytes(keysize / 8);
            using(RijndaelManaged symmetricKey = new RijndaelManaged())
            {
                symmetricKey.Mode = CipherMode.CBC;
                using(ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes))
                {
                    using(MemoryStream memoryStream = new MemoryStream(cipherTextBytes))
                    {
                        using(CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                        {
                            plainTextBytes = new byte[cipherTextBytes.Length];
                            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                        }
                    }
                }
            }

            // deserialize
            Licence lic = null;
            BinaryFormatter formatter = new BinaryFormatter();
            using(MemoryStream s = new MemoryStream(plainTextBytes))
            {
                lic = (Licence)formatter.Deserialize(s);
            }

            return lic;
        }

        /// <summary>
        /// Gets or sets the display name of the account holder.
        /// </summary>
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        /// <summary>
        /// Gest or sets the account id for the account holder.
        /// </summary>
        public string Email
        {
            get { return this.email; }
            set { this.email = value; }
        }

        /// <summary>
        /// Gets or sets the components that are licensed.
        /// </summary>
        public Dictionary<string,int> Components
        {
            get { return this.components; }
            set { this.components = value; }
        }

        /// <summary>
        /// Gets or sets a date that indicates when the license will expire - can be null.
        /// </summary>
        public DateTime End
        {
            get { return this.end; }
            set { this.end = value; }
        }

        /// <summary>
        /// Gets or sets the start date for the license.
        /// </summary>
        public DateTime Start
        {
            get { return this.start; }
            set { this.start = value; }
        }

        /// <summary>
        /// Provides information on the validatity of a license based on a test.
        /// </summary>
        /// <seealso cref="Licence.Validate"/>
        public class ValidationInfo
        {
            private bool isVersionInvalid = false;
            private bool hasExpired = false;
            private bool isComponentValid = false;

            /// <summary>
            /// Indicates if the version is licensed.
            /// </summary>
            public bool IsVersionInvalid
            {
                get { return this.isVersionInvalid; }
                set { this.isVersionInvalid = value; }
            }

            /// <summary>
            /// Indicates if the component is licensed.
            /// </summary>
            public bool IsComponentValid
            {
                get { return this.isComponentValid; }
                set { this.isComponentValid = value; }
            }

            /// <summary>
            /// Indicates if the license has expired.
            /// </summary>
            public bool HasExpired
            {
                get { return this.hasExpired; }
                set { this.hasExpired = value; }
            }
        }
    }
}
