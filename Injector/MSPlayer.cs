using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Forms;

namespace Zenith.Injector
{
    internal class MSPlayer
    {
        public static void showConsole()
        {
            if (!consoleLoaded)
            {
                consoleLoaded = true;
                consoleInUse = true;
                Imports.ConsoleHelper.Initialize(true);
                return;
            }
            consoleInUse = true;
            Imports.ShowWindow(Imports.GetConsoleWindow(), 5);
        }

        public static void hideConsole()
        {
            consoleInUse = false;
            Imports.ConsoleHelper.Clear();
            Imports.ShowWindow(Imports.GetConsoleWindow(), 0);
        }

        public bool findProcess(ref Util.ProcInfo outPInfo)
        {
            using (List<Util.ProcInfo>.Enumerator enumerator = Util.openProcessesByName(injectProcessName).GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    Util.ProcInfo procInfo = enumerator.Current;
                    outPInfo = procInfo;
                    return true;
                }
            }
            return false;
        }

        public static bool isInjected(Util.ProcInfo pinfo)
        {
            return pinfo.isOpen() && pinfo.readByte(Imports.GetProcAddress(Imports.GetModuleHandle("USER32.dll"), "DrawIcon") + 3UL) == 67;
        }

        public static void sendScript(Util.ProcInfo pinfo, string source)
        {
            ulong procAddress = Imports.GetProcAddress(Imports.GetModuleHandle("USER32.dll"), "DrawIcon");
            int num = 0;
            while (pinfo.readUInt32(procAddress + 8UL) > 0U)
            {
                Thread.Sleep(10);
                if (num++ > 100)
                {
                    return;
                }
            }
            if (!isInjected(pinfo))
            {
                return;
            }
            int num2 = 0;
            char[] chars = source.ToCharArray(0, source.Length);
            byte[] bytes = Encoding.UTF8.GetBytes(chars);
            ulong num3 = Imports.VirtualAllocEx(pinfo.handle, 0UL, bytes.Length, 12288U, 4U);
            Imports.WriteProcessMemory(pinfo.handle, num3, bytes, bytes.Length, ref num2);
            pinfo.writeUInt64(procAddress + 8UL, 1UL);
            pinfo.writeUInt64(procAddress + 12UL, num3);
            pinfo.writeInt32(procAddress + 16UL, bytes.Length);
        }

        public static InjectionStatus injectMSPlayer(Util.ProcInfo pinfo)
        {
            if (isInjectingMSPlayer)
            {
                return InjectionStatus.ALREADY_INJECTING;
            }
            if (isInjected(pinfo))
            {
                return InjectionStatus.ALREADY_INJECTED;
            }
            if (!skipAdministrative)
            {
                Thread.GetDomain().SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);
                if (!((WindowsPrincipal)Thread.CurrentPrincipal).IsInRole(WindowsBuiltInRole.Administrator))
                {
                    var result = MessageBox.Show("Celery is not running in administrative mode... There may be issues while injecting. Continue?", "Warning", MessageBoxButtons.OKCancel);
                    if (result == DialogResult.Cancel)
                    {
                        return InjectionStatus.FAILED_ADMINISTRATOR_ACCESS;
                    }
                    skipAdministrative = true;
                }
            }
            isInjectingMSPlayer = true;
            List<byte> list = new List<byte>();
            List<byte> list2 = new List<byte>();
            ulong procAddress = Imports.GetProcAddress(Imports.GetModuleHandle("USER32.dll"), "DrawIcon");
            byte[] array = pinfo.readBytes(procAddress + 8UL, 512);
            int num = 0;
            while (num < 510 && (array[num + 1] != 139 || array[num + 2] != 255))
            {
                list.Add(0);
                num++;
            }
            pinfo.setPageProtect(procAddress, list.Count + 8, 64U);
            pinfo.writeBytes(procAddress + 8UL, list.ToArray(), -1);
            procAddress = Imports.GetProcAddress(Imports.GetModuleHandle("USER32.dll"), "DrawIconEx");
            array = pinfo.readBytes(procAddress, 512);
            int num2 = 0;
            while (num2 < 510 && (array[num2 + 1] != 139 || array[num2 + 2] != 255))
            {
                list2.Add(0);
                num2++;
            }
            pinfo.setPageProtect(procAddress, list2.Count, 64U);
            pinfo.writeBytes(procAddress, list2.ToArray(), -1);
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "update.txt") && File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "update.txt") == "true")
            {
                return InjectionStatus.FAILED;
            }
            if (MapInject.ManualMap(pinfo.processRef, AppDomain.CurrentDomain.BaseDirectory + "dll/" + injectFileName))
            {
                while (pinfo.isOpen() && !isInjected(pinfo))
                {
                    Thread.Sleep(10);
                }
                postInjectedMSPlayer.Add(pinfo);
                isInjectingMSPlayer = false;
                showConsole();
                return InjectionStatus.SUCCESS;
            }
            isInjectingMSPlayer = false;
            return InjectionStatus.FAILED;
        }

        public static List<Util.ProcInfo> getInjectedProcesses()
        {
            List<Util.ProcInfo> list = new List<Util.ProcInfo>();
            foreach (Util.ProcInfo procInfo in Util.openProcessesByName(injectProcessName))
            {
                if (isInjected(procInfo))
                {
                    list.Add(procInfo);
                }
            }
            return list;
        }

        private static bool consoleLoaded = false;

        public static bool consoleInUse = false;

        private static string injectProcessName = "Windows10Universal.exe";

        private static string injectFileName = "celeryuwp.bin";

        private static List<Util.ProcInfo> postInjectedMSPlayer = new List<Util.ProcInfo>();

        private static bool isInjectingMSPlayer = false;

        private static bool skipAdministrative = true;
    }
}
