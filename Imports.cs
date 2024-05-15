using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32.SafeHandles;
public class Imports
{
    [DllImport("user32.dll")]
    public static extern int FindWindow(string sClass, string sWindow);

    [DllImport("user32.dll")]
    public static extern bool ShowWindow(int hWnd, int nCmdShow);

    [DllImport("user32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
    public static extern int MessageBoxA(int hWnd, string sMessage, string sCaption, uint mbType);

    [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern int MessageBoxW(int hWnd, string sMessage, string sCaption, uint mbType);

    [DllImport("kernel32.dll")]
    public static extern int GetConsoleWindow();

    [DllImport("kernel32.dll")]
    public static extern ulong OpenProcess(uint dwDesiredAccess, bool bInheritHandle, int dwProcessId);

    [DllImport("kernel32.dll")]
    public static extern bool ReadProcessMemory(ulong hProcess, ulong lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);

    [DllImport("kernel32.dll")]
    public static extern bool WriteProcessMemory(ulong hProcess, ulong lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesWritten);

    [DllImport("kernel32.dll")]
    public static extern bool VirtualProtectEx(ulong hProcess, ulong lpBaseAddress, int dwSize, uint new_protect, ref uint lpOldProtect);

    [DllImport("kernel32.dll")]
    public static extern ulong VirtualQueryEx(ulong hProcess, ulong lpAddress, out Imports.MEMORY_BASIC_INFORMATION lpBuffer, uint dwLength);

    [DllImport("kernel32.dll")]
    public static extern ulong VirtualAllocEx(ulong hProcess, ulong lpAddress, int size, uint allocation_type, uint protect);

    [DllImport("kernel32.dll")]
    public static extern ulong VirtualFreeEx(ulong hProcess, ulong lpAddress, int size, uint allocation_type);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
    public static extern ulong GetModuleHandle(string lpModuleName);

    [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
    public static extern ulong GetProcAddress(ulong hModule, string procName);

    [DllImport("kernel32.dll")]
    public static extern uint GetLastError();

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool CloseHandle(ulong hObject);

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool GetExitCodeProcess(ulong hProcess, out uint lpExitCode);

    [DllImport("kernel32.dll")]
    public static extern int CreateRemoteThread(ulong hProcess, int lpThreadAttributes, uint dwStackSize, int lpStartAddress, int lpParameter, uint dwCreationFlags, out int lpThreadId);

    [DllImport("kernel32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
    public static extern uint GetStdHandle(uint nStdHandle);

    [DllImport("kernel32.dll")]
    public static extern void SetStdHandle(uint nStdHandle, uint handle);

    [DllImport("kernel32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
    public static extern int AllocConsole();

    [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
    public static extern bool SetConsoleTitle(string lpConsoleTitle);

    [DllImport("kernel32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
    public static extern uint AttachConsole(uint dwProcessId);

    [DllImport("kernel32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
    public static extern uint CreateFileW(string lpFileName, uint dwDesiredAccess, uint dwShareMode, uint lpSecurityAttributes, uint dwCreationDisposition, uint dwFlagsAndAttributes, uint hTemplateFile);

    [DllImport("kernel32.dll")]
    public static extern uint GetCurrentProcessId();

    [DllImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool FreeConsole();

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern uint CreateFile(string lpFileName, uint dwDesiredAccess, uint dwShareMode, uint lpSecurityAttributes, uint dwCreationDisposition, uint dwFlagsAndAttributes, uint hTemplateFile);

    public const uint PAGE_NOACCESS = 1U;

    public const uint PAGE_READONLY = 2U;

    public const uint PAGE_READWRITE = 4U;

    public const uint PAGE_WRITECOPY = 8U;

    public const uint PAGE_EXECUTE = 16U;

    public const uint PAGE_EXECUTE_READ = 32U;

    public const uint PAGE_EXECUTE_READWRITE = 64U;

    public const uint PAGE_EXECUTE_WRITECOPY = 128U;

    public const uint PAGE_GUARD = 256U;

    public const uint PAGE_NOCACHE = 512U;

    public const uint PAGE_WRITECOMBINE = 1024U;

    public const uint MEM_COMMIT = 4096U;

    public const uint MEM_RESERVE = 8192U;

    public const uint MEM_DECOMMIT = 16384U;

    public const uint MEM_RELEASE = 32768U;

    public const uint PROCESS_WM_READ = 16U;

    public const uint PROCESS_ALL_ACCESS = 2035711U;

    private const uint GENERIC_WRITE = 1073741824U;

    private const uint GENERIC_READ = 2147483648U;

    private const uint FILE_SHARE_READ = 1U;

    private const uint FILE_SHARE_WRITE = 2U;

    private const uint OPEN_EXISTING = 3U;

    private const uint FILE_ATTRIBUTE_NORMAL = 128U;

    private const uint ERROR_ACCESS_DENIED = 5U;

    private const uint ATTACH_PARENT = 4294967295U;

    public const int EXCEPTION_CONTINUE_EXECUTION = -1;

    public const int EXCEPTION_CONTINUE_SEARCH = 0;

    public const uint STD_OUTPUT_HANDLE = 4294967285U;

    public const int MY_CODE_PAGE = 437;

    public const int SW_HIDE = 0;

    public const int SW_SHOW = 5;

    public struct MEMORY_BASIC_INFORMATION
    {
        public int BaseAddress;

        public int AllocationBase;

        public uint AllocationProtect;

        public int RegionSize;

        public uint State;

        public uint Protect;

        public uint Type;
    }

    public static class ConsoleHelper
    {
        public static void Initialize(bool alwaysCreateNewConsole = true)
        {
            bool flag = true;
            if (alwaysCreateNewConsole || (Imports.AttachConsole(4294967295U) == 0U && (long)Marshal.GetLastWin32Error() != 5L))
            {
                flag = (Imports.AllocConsole() != 0);
            }
            if (flag)
            {
                Imports.ConsoleHelper.InitializeOutStream();
                Imports.ConsoleHelper.InitializeInStream();
            }
            Console.OutputEncoding = Encoding.UTF8;
        }

        public static void Clear()
        {
            Console.Write("\n\n");
        }

        private static void InitializeOutStream()
        {
            Imports.ConsoleHelper.fwriter = Imports.ConsoleHelper.CreateFileStream("CONOUT$", 1073741824U, 2U, FileAccess.Write);
            if (Imports.ConsoleHelper.fwriter != null)
            {
                Imports.ConsoleHelper.writer = new StreamWriter(Imports.ConsoleHelper.fwriter)
                {
                    AutoFlush = true
                };
                Console.SetOut(Imports.ConsoleHelper.writer);
                Console.SetError(Imports.ConsoleHelper.writer);
            }
        }

        private static void InitializeInStream()
        {
            FileStream fileStream = Imports.ConsoleHelper.CreateFileStream("CONIN$", 2147483648U, 1U, FileAccess.Read);
            if (fileStream != null)
            {
                Console.SetIn(new StreamReader(fileStream));
            }
        }

        private static FileStream CreateFileStream(string name, uint win32DesiredAccess, uint win32ShareMode, FileAccess dotNetFileAccess)
        {
            SafeFileHandle safeFileHandle = new SafeFileHandle((IntPtr)((long)((ulong)Imports.CreateFileW(name, win32DesiredAccess, win32ShareMode, 0U, 3U, 128U, 0U))), true);
            if (!safeFileHandle.IsInvalid)
            {
                return new FileStream(safeFileHandle, dotNetFileAccess);
            }
            return null;
        }

        public static StreamWriter writer;

        public static FileStream fwriter;
    }
}
