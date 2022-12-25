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
        static extern Int32 GetShortPathName(String path, StringBuilder shortPath, Int32 shortPathLength);

        static string GetShortPathName(String path)
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
                throw new Exception("音乐只支持wav和mp3");
            }
            if (!File.Exists(soundFile))
            {
                throw new Exception("音乐文件不存在" + soundFile);
            }
            string shortpath = GetShortPathName(soundFile);
            mciSendString("close " + shortpath);//先停止之前播放的

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
                throw new Exception("音乐只支持wav和mp3");
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

        /// <summary>
        /// 带返回值的在UI线程执行委托，并且等待获得返回值
        /// 一般都是用Invoke，而不使用BeginInvoke，因为BeginInvoke无法保证执行顺序
        /// 简化Invoke的调用，调用者只要写lambda表达式就可以，不用再new Func()
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public static object Invoke(DispatcherObject dispatcherObj,  Func<object> func)
        {
            return dispatcherObj.Dispatcher.Invoke(func);
        }

        /// <summary>
        /// 不带返回值的在UI线程执行委托，并且等待委托返回
        /// 简化Invoke的调用，调用者只要写lambda表达式就可以，不用再new Func()
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public static void Invoke(DispatcherObject dispatcherObj, Action func)
        {
            dispatcherObj.Dispatcher.Invoke(func);
        }

        private static readonly object LogLock = new object();
        
        public static void LogError(String msg)
        {    
            lock(LogLock)
            {
                String fullMsg = "游戏引擎内核错误:" + msg;
                Debug.WriteLine(fullMsg);

                ConsoleColor oldColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;               
                Console.WriteLine(fullMsg);

                Console.ForegroundColor = oldColor;
            }

        }    
    }
}
