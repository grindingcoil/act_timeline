using System.IO;
using System.Text.RegularExpressions;

namespace ACTTimeline
{
    public class Globals
    {
        static private readonly Regex stripEndSlashesRegex = new Regex(@"/*$");
        static string StripEndSlashes(string pathstr)
        {
            return stripEndSlashesRegex.Replace(pathstr, "");
        }

        static private string resourceRoot;
        static public string ResourceRoot
        {
            get { return resourceRoot; }
            set
            {
                resourceRoot = StripEndSlashes(value);
            }
        }

        static public string SoundFilesRoot
        {
            get { return ResourceRoot + "/wav"; }
        }

        static public int NumberOfSoundFilesInResourcesDir()
        {
            try
            {
                return Directory.GetFileSystemEntries(SoundFilesRoot, "*.wav").Length;
            }
            catch (DirectoryNotFoundException)
            {
                return 0;
            }
        }

        static public string TimelineTxtsRoot
        {
            get { return ResourceRoot + "/timeline"; }
        }

        static public string[] TimelineTxtsInResourcesDir
        {
            get
            {
                try
                {
                    return Directory.GetFileSystemEntries(TimelineTxtsRoot, "*.txt");
                }
                catch (DirectoryNotFoundException)
                {
                    return new string[] { };
                }
            }
        }
    }
}
