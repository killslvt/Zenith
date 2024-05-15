using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

public class Util
{
    public static List<ProcInfo> openProcessesByName(string processName)
    {
        List<ProcInfo> list = new List<ProcInfo>();
        foreach (Process process in Process.GetProcessesByName(processName.Replace(".exe", "")))
        {
            try
            {
                if (process.Id != 0 && !process.HasExited)
                {
                    list.Add(new ProcInfo
                    {
                        processRef = process,
                        baseModule = 0UL,
                        handle = 0UL,
                        processId = (ulong)((long)process.Id),
                        processName = processName,
                        windowName = ""
                    });
                }
            }
            catch (NullReferenceException)
            {
            }
            catch (Exception)
            {
            }
        }
        return list;
    }

    public class ProcInfo
    {
        public ProcInfo()
        {
            processRef = null;
            processId = 0UL;
            handle = 0UL;
        }

        public bool isOpen()
        {
            try
            {
                if (processRef == null)
                {
                    return false;
                }
                if (processRef.HasExited)
                {
                    return false;
                }
                if (processRef.Id == 0)
                {
                    return false;
                }
                if (processRef.Handle == IntPtr.Zero)
                {
                    return false;
                }
            }
            catch (InvalidOperationException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
            return processId != 0UL && handle > 0UL;
        }

        public uint setPageProtect(ulong address, int size, uint protect)
        {
            uint result = 0U;
            Imports.VirtualProtectEx(handle, address, size, protect, ref result);
            return result;
        }

        public bool writeBytes(ulong address, byte[] bytes, int count = -1)
        {
            return Imports.WriteProcessMemory(handle, address, bytes, (count == -1) ? bytes.Length : count, ref nothing);
        }

        public bool writeInt32(ulong address, int value)
        {
            return Imports.WriteProcessMemory(handle, address, BitConverter.GetBytes(value), 4, ref nothing);
        }

        public bool writeUInt64(ulong address, ulong value)
        {
            return Imports.WriteProcessMemory(handle, address, BitConverter.GetBytes(value), 8, ref nothing);
        }

        public byte readByte(ulong address)
        {
            byte[] array = new byte[1];
            Imports.ReadProcessMemory(handle, address, array, 1, ref nothing);
            return array[0];
        }

        public byte[] readBytes(ulong address, int count)
        {
            byte[] array = new byte[count];
            Imports.ReadProcessMemory(handle, address, array, count, ref nothing);
            return array;
        }
      
        public uint readUInt32(ulong address)
        {
            byte[] array = new byte[4];
            Imports.ReadProcessMemory(handle, address, array, 4, ref nothing);
            return BitConverter.ToUInt32(array, 0);
        }

        public Process processRef;

        public ulong processId;

        public string processName;

        public string windowName;

        public ulong handle;

        public ulong baseModule;

        private int nothing;
    }
}
