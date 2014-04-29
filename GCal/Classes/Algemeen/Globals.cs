using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Resources;
using System.Windows.Forms;

namespace GCal.Classes.Algemeen
{
    public class Globals
    {
        private static Globals instance = null;

        public string AppPath;
        public string Taal;

        public string AppTitle = "GCal";
        public IntPtr Handle;

        public static readonly string CLIENT_ID = "283953418211.apps.googleusercontent.com";

        public static readonly string CLIENT_SECRET = "cFuG0PzPAsIaizFMlTDjkC_5";

        public static readonly string REDIRECT_URI = "urn:ietf:wg:oauth:2.0:oob";

        public static RichTextBox TextBox;

        public static string TokenFile
        {
            get
            {
                string myPath = Path.GetDirectoryName(Application.ExecutablePath);

                if (!Directory.Exists(myPath))
                {
                    Directory.CreateDirectory(myPath);
                }

                return Path.Combine(myPath, "GCal.Auth");
            }
        }

        private Globals()
        {

        }

        public static Globals Current
        {
            get
            {
                if (instance == null)
                {
                    instance = new Globals();
                }

                return instance;
            }
        }

    }
}
