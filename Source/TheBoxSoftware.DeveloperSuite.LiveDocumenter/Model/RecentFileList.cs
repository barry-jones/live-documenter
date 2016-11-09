
namespace TheBoxSoftware.DeveloperSuite.LiveDocumenter.Model
{
    using System;
    using System.Collections.ObjectModel;

    public class RecentFileList : ObservableCollection<RecentFile>
    {
        public void AddFile(RecentFile file)
        {
            RecentFile alreadyExisting = null;
            // Check for and do not re-add an already stored file, just move its position
            foreach(RecentFile current in this)
            {
                if(current.Filename == file.Filename)
                {
                    alreadyExisting = current;
                    break;
                }
            }

            if(alreadyExisting != null)
            {
                this.Remove(alreadyExisting);
                this.Insert(0, alreadyExisting);
            }
            else
            {
                this.Insert(0, file);
                if(this.Count > 10)
                {
                    this.RemoveAt(this.Count - 1);
                }
            }
        }
    }

    [Serializable]
    public class RecentFile
    {
        public RecentFile() { }
        public RecentFile(string filename, string displayname)
        {
            this.Filename = filename;
            this.DisplayName = displayname;
        }
        public string Filename { get; set; }
        public string DisplayName { get; set; }
    }
}