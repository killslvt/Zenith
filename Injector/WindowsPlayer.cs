using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace Zenith.Injector
{
    public class WindowsPlayer : Util
    {
        public static bool isInjected()
        {
            if (injectorProc != null && lastProcInfo != null && lastProcInfo.processRef != null)
            {
                try
                {
                    return !injectorProc.HasExited && !lastProcInfo.processRef.HasExited;
                }
                catch (InvalidOperationException)
                {
                    return false;
                }
            }
            return false;
        }

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool SetProp(IntPtr hWnd, string lpString, IntPtr hData);

        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string sClass, string sWindow);

        public static void sendScript(string source)
        {
            File.WriteAllText(Path.GetTempPath() + "celery" + "\\myfile.txt", source);
        }

        public static Process ExecuteAsAdmin(string fileName)
        {
            Process process = new Process();
            process.StartInfo.FileName = fileName;
            process.StartInfo.UseShellExecute = true;
            process.StartInfo.Verb = "runas";
            process.Start();
            return process;
        }

        public static InjectionStatus injectPlayer(Util.ProcInfo pinfo)
        {
            if (isInjectingMainPlayer)
            {
                return InjectionStatus.ALREADY_INJECTING;
            }
            if (isInjected())
            {
                return InjectionStatus.ALREADY_INJECTED;
            }
            isInjectingMainPlayer = true;
            SetProp(FindWindow(null, "Roblox"), "CELERYHOOKED", (IntPtr)273);
            injectorProc = ExecuteAsAdmin(AppDomain.CurrentDomain.BaseDirectory + "CeleryInject.exe");
            lastProcInfo = pinfo;
            isInjectingMainPlayer = false;
            return InjectionStatus.SUCCESS;
        }

        public static List<Util.ProcInfo> getInjectedProcesses()
        {
            List<Util.ProcInfo> list = new List<Util.ProcInfo>();
            if (isInjected())
            {
                list.Add(lastProcInfo);
            }
            return list;
        }

        private static string injectFileName = "celerywindows.bin";

        public static Util.ProcInfo lastProcInfo;

        public static Process injectorProc;

        private static List<Util.ProcInfo> postInjectedMainPlayer = new List<Util.ProcInfo>();

        private static bool isInjectingMainPlayer = false;
    }
}
