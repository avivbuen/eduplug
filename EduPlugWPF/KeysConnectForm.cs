using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace EduPlugWPF
{
    public partial class KeysConnectForm : Form
    {
        public KeysConnectForm()
        {
            InitializeComponent();
        }
        private void KeysConnectForm_Load(object sender, EventArgs e)
        {
            Visible = false;
        }
        public static void SendKey(string key, string currentProcess, bool isFullScreen)
        {
            string str;
            var str1 = key;
            if (str1 != null)
            {
                switch (str1)
                {
                    case "1":
                    {
                        SendKeys.SendWait(KeySet.PlayPause(currentProcess, isFullScreen));
                        break;
                    }
                    case "2":
                    {
                        SendKeys.SendWait(KeySet.Backward(currentProcess));
                        break;
                    }
                    case "3":
                    {
                        SendKeys.SendWait(KeySet.Forward(currentProcess));
                        break;
                    }
                    case "4":
                    {
                        SendKeys.SendWait(KeySet.FullScreen(currentProcess, isFullScreen));
                        break;
                    }
                    case "5":
                    {
                        SendKeys.SendWait(KeySet.VolumeUp(currentProcess));
                        break;
                    }
                    case "6":
                    {
                        SendKeys.SendWait(KeySet.Mute(currentProcess));
                        break;
                    }
                    case "7":
                    {
                        SendKeys.SendWait(KeySet.VolumeDown(currentProcess));
                        break;
                    }
                    default:
                    {
                        if (key.Contains("play"))
                        {
                            SendKeys.SendWait("f");
                            SendKeys.SendWait("k");
                            str = key.Substring(key.IndexOf(":", StringComparison.Ordinal) + 1);
                            Process.Start(string.Concat("https://www.youtube.com/watch?v=", str));
                        }
                        return;
                    }
                }
            }
            else
            {
                if (key.Contains("play"))
                {
                    SendKeys.SendWait("f");
                    SendKeys.SendWait("k");
                    str = key.Substring(key.IndexOf(":", StringComparison.Ordinal) + 1);
                    Process.Start(string.Concat("https://www.youtube.com/watch?v=", str));
                }
                return;
            }
        }
    }
    public class KeySet
    {
        public KeySet()
        {
        }

        public static string Backward(string CurrentProcess)
        {
            string str;
            if (CurrentProcess == "YouTube")
            {
                KeySet.MessageForm("<<");
                str = "j";
            }
            else if (CurrentProcess != "PowerPoint")
            {
                str = (CurrentProcess != "WindowsMediaPlayer" ? " " : "^B");
            }
            else
            {
                str = "{LEFT}";
            }
            return str;
        }

        public static string Forward(string CurrentProcess)
        {
            string str;
            if (CurrentProcess == "YouTube")
            {
                KeySet.MessageForm(">>");
                str = "l";
            }
            else if (CurrentProcess != "PowerPoint")
            {
                str = (CurrentProcess != "WindowsMediaPlayer" ? " " : "^F");
            }
            else
            {
                str = "{RIGHT}";
            }
            return str;
        }

        public static string FullScreen(string CurrentProcess, bool FullScreen)
        {
            string str;
            if (CurrentProcess == "YouTube")
            {
                KeySet.MessageForm("Full Screen");
                str = "f";
            }
            else if (CurrentProcess != "PowerPoint")
            {
                str = (CurrentProcess != "WindowsMediaPlayer" ? " " : "%{ENTER}");
            }
            else
            {
                str = (!FullScreen ? "{F5}" : "{ESC}");
            }
            return str;
        }

        public static void MessageForm(string message)
        {
            //(new Notfic(message)).Show();
        }

        public static string Mute(string CurrentProcess)
        {
            string str;
            if (CurrentProcess != "YouTube")
            {
                str = " ";
            }
            else
            {
                KeySet.MessageForm("Mute");
                str = "m";
            }
            return str;
        }

        public static string PlayPause(string currentProcess, bool isFullScreen)
        {
            string str;
            if (currentProcess == "YouTube")
            {
                KeySet.MessageForm("Play/Pause");
                str = "k";
            }
            else if (currentProcess != "PowerPoint")
            {
                str = (currentProcess != "WindowsMediaPlayer" ? " " : "^P");
            }
            else
            {
                str = (!isFullScreen ? "{F5}" : "{RIGHT}");
            }
            return str;
        }

        public static string VolumeDown(string currentProcess)
        {
            string str;
            if (currentProcess != "YouTube")
            {
                str = (currentProcess != "WindowsMediaPlayer" ? " " : "{F8}");
            }
            else
            {
                MessageForm("Volume-");
                str = "{DOWN}";
            }
            return str;
        }

        public static string VolumeUp(string currentProcess)
        {
            string str;
            if (currentProcess != "YouTube")
            {
                str = (currentProcess != "WindowsMediaPlayer" ? " " : "{F9}");
            }
            else
            {
                MessageForm("Volume+");
                str = "{UP}";
            }
            return str;
        }
    }
}
