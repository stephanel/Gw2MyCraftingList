using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANet.WebAPI
{
    class Config
    {
        private static string _language = "fr";

        public static string Language
        {
            get { return _language; }
            set { _language = value; }
        }

        public static string FileSignature
        {
            get { return Properties.Settings.Default.fileSignature; }
            set { Properties.Settings.Default.fileSignature = value; }
        }
        public static string FileID
        {
            get { return Properties.Settings.Default.fileID; }
            set { Properties.Settings.Default.fileID = value; }
        }
        public static string FileFormat
        {
            get { return Properties.Settings.Default.fileFormat; }
            set { Properties.Settings.Default.fileFormat = value; }
        }

        public static void Save()
        {
            Properties.Settings.Default.Save();
        }

    }
}
