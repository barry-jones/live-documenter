﻿
namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Model
{
    using System;
    using System.IO.IsolatedStorage;

    /// <summary>
    /// Class that stores all user specific application information and preferences;
    /// this is stored via IsolatedStorage.
    /// </summary>
    [Serializable]
    public class UserApplicationStore
    {
        [NonSerialized]
        private static UserApplicationStore store;

        /// <summary>
        /// Initialises a new instance of the UserApplicationStore class.
        /// </summary>
        public UserApplicationStore()
        {
            this.RecentFiles = new RecentFileList();
        }

        /// <summary>
        /// Loads and populates the static <see cref="Store"/> property.
        /// </summary>
        public static void Load()
        {
            IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForAssembly();
            using(IsolatedStorageFileStream fs = new IsolatedStorageFileStream("userpreferences.xml", System.IO.FileMode.OpenOrCreate, file))
            {
                if(fs != null)
                {
                    System.IO.StreamReader reader = new System.IO.StreamReader(fs);
                    string preferenceData = string.Empty;
                    preferenceData = reader.ReadToEnd();
                    reader.Close();

                    // We are opening or creating, we could have no file present.
                    if(!string.IsNullOrEmpty(preferenceData.Trim()))
                    {
                        UserApplicationStore.store = (Model.UserApplicationStore)UserApplicationStore.DeSerialize(
                            preferenceData, typeof(Model.UserApplicationStore)
                            );
                    }
                }
            }
        }

        /// <summary>
        /// Saves the current contents of the UserApplicationStore.
        /// </summary>
        public static void Save()
        {
            IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForAssembly();
            using(IsolatedStorageFileStream fs = new IsolatedStorageFileStream("userpreferences.xml", System.IO.FileMode.Truncate, file))
            {
                using(System.IO.StreamWriter writer = new System.IO.StreamWriter(fs))
                {
                    writer.Write(UserApplicationStore.Serialize(UserApplicationStore.store));
                    writer.Close();
                }
            }
        }

        /// <summary>
        /// Serialize the specified object to an xml string.
        /// </summary>
        /// <param name="objectToSerialize">The object to serialize</param>
        /// <returns>The serialized xml string</returns>
        private static string Serialize(object objectToSerialize)
        {
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(objectToSerialize.GetType());

            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
            System.IO.StringWriter writer = new System.IO.StringWriter(stringBuilder);

            serializer.Serialize(writer, objectToSerialize);
            writer.Close();

            return stringBuilder.ToString();
        }

        /// <summary>
        /// De-serialize the specified xml string to an object of the specified type.
        /// </summary>
        /// <param name="xmlToDeSerialize">The xml string to de-serialize</param>
        /// <param name="objectType">The type of object to de-serialize to</param>
        /// <returns>The de-serialized object</returns>
        private static object DeSerialize(string xmlToDeSerialize, Type objectType)
        {
            object deSerializedObject;
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(objectType);
            System.IO.StringReader reader = new System.IO.StringReader(xmlToDeSerialize);

            deSerializedObject = serializer.Deserialize(reader);
            reader.Close();

            return deSerializedObject;
        }

        #region Properties
        /// <summary>
        /// Obtains a reference to teh single instance of the UserApplicationStore.
        /// </summary>
        public static UserApplicationStore Store
        {
            get
            {
                if(UserApplicationStore.store == null)
                {
                    UserApplicationStore.store = new UserApplicationStore();
                }
                return UserApplicationStore.store;
            }
        }

        /// <summary>
        /// The users list of files they have recently accessed.
        /// </summary>
        public RecentFileList RecentFiles { get; set; }

        /// <summary>
        /// The last position the window was in when the user exited the application
        /// </summary>
        public System.Windows.Point LastWindowPosition { get; set; }

        /// <summary>
        /// The last size the user had the window when the application was closed.
        /// </summary>
        public System.Windows.Size LastWindowSize { get; set; }

        /// <summary>
        /// The users selected viewing mode
        /// </summary>
        public System.Windows.Controls.FlowDocumentReaderViewingMode ViewingMode { get; set; }

        /// <summary>
        /// Indicates if the user closed the application while the window was maximized
        /// </summary>
        public bool IsMaximized { get; set; }
        #endregion
    }
}