using System.Diagnostics;
using System.IO;
using System.Windows.Threading;

namespace YZKGame.NET;
public static class CommonHelper
{

    public static string MapPath(string path)
    {
        string appDir = AppDomain.CurrentDomain.BaseDirectory;
        string absolutePath = Path.Combine(appDir, path);
        return absolutePath;
    }

    public static T Invoke<T>(DispatcherObject dispatcherObj, Func<T> func)
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
        lock (LogLock)
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