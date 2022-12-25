using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Threading;

namespace YZKGame.NET
{
    static class CommonHelper
    {

        [DllImport("winmm.dll")]
        public static extern int mciSendString(string m_strCmd, string m_strReceive, int m_v1, int m_v2);

        public static int mciSendString(string m_strCmd)
        {
            return mciSendString(m_strCmd, null, 0, 0);
        }

        [DllImport("Kernel32")]
        static extern int GetShortPathName(string path, StringBuilder shortPath, int shortPathLength);

        static string GetShortPathName(string path)
        {
            StringBuilder shortpath = new StringBuilder(255);
            GetShortPathName(path, shortpath, shortpath.Capacity);
            return shortpath.ToString();
        }

        public static void PlaySound(string soundFile, bool repeat)
        {
            soundFile = soundFile.ToLower();
            if (!soundFile.EndsWith(".wav") && !soundFile.EndsWith(".mp3"))
            {
                throw new ArgumentException("Only wav and mp3 files are supported.",nameof(soundFile));
            }
            if (!File.Exists(soundFile))
            {
                throw new ArgumentException("File doesn't exist." + soundFile,nameof(soundFile));
            }
            string shortpath = GetShortPathName(soundFile);
            mciSendString("close " + shortpath);//stop before playing

            string mciCmd = "play " + shortpath;
            if (repeat)
            {
                mciCmd +=" repeat";
            }
            mciSendString(mciCmd);
        }

        public static void CloseSound(string soundFile)
        {
            if (!soundFile.EndsWith(".wav") && !soundFile.EndsWith(".mp3"))
            {
                throw new ArgumentException("Only wav and mp3 files are supported.", nameof(soundFile));
            }

            string shortpath = GetShortPathName(soundFile);
            string mciCmd = "close " + shortpath;
            mciSendString(mciCmd);
        }

        public static string MapPath(string path)
        {
            string appDir = AppDomain.CurrentDomain.BaseDirectory;
            string absolutePath = Path.Combine(appDir,path);
            return absolutePath;
        }

        public static T Invoke<T>(DispatcherObject dispatcherObj,  Func<T> func)
        {
            return dispatcherObj.Dispatcher.Invoke(func);
        }

        public static void Invoke(DispatcherObject dispatcherObj, Action func)
        {
            dispatcherObj.Dispatcher.Invoke(func);
        }

        private static readonly object LogLock = new object();
        
        public static void LogError(string msg)
        {    
            lock(LogLock)
            {
                string fullMsg = "Game error:" + msg;
                Debug.WriteLine(fullMsg);

                ConsoleColor oldColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;               
                Console.WriteLine(fullMsg);

                Console.ForegroundColor = oldColor;
            }

        }    
    }
}