using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Zenith.Injector
{
    public class MapInject
    {
        private static IMAGE_OPTIONAL_HEADER toImageOptionalHeader(byte[] byteArray, int offset)
        {
            IMAGE_OPTIONAL_HEADER image_OPTIONAL_HEADER;
            image_OPTIONAL_HEADER.Magic = BitConverter.ToUInt16(byteArray, offset);
            image_OPTIONAL_HEADER.MajorLinkerVersion = (byte)BitConverter.ToChar(byteArray, offset + 2);
            image_OPTIONAL_HEADER.MinorLinkerVersion = (byte)BitConverter.ToChar(byteArray, offset + 3);
            image_OPTIONAL_HEADER.SizeOfCode = BitConverter.ToUInt32(byteArray, offset + 4);
            image_OPTIONAL_HEADER.SizeOfInitializedData = BitConverter.ToUInt32(byteArray, offset + 8);
            image_OPTIONAL_HEADER.SizeOfUninitializedData = BitConverter.ToUInt32(byteArray, offset + 12);
            image_OPTIONAL_HEADER.AddressOfEntryPoint = BitConverter.ToUInt32(byteArray, offset + 16);
            image_OPTIONAL_HEADER.BaseOfCode = BitConverter.ToUInt32(byteArray, offset + 20);
            image_OPTIONAL_HEADER.BaseOfData = BitConverter.ToUInt32(byteArray, offset + 24);
            image_OPTIONAL_HEADER.ImageBase = BitConverter.ToUInt32(byteArray, offset + 28);
            image_OPTIONAL_HEADER.SectionAlignment = BitConverter.ToUInt32(byteArray, offset + 32);
            image_OPTIONAL_HEADER.FileAlignment = BitConverter.ToUInt32(byteArray, offset + 36);
            image_OPTIONAL_HEADER.MajorOperatingSystemVersion = BitConverter.ToUInt16(byteArray, offset + 40);
            image_OPTIONAL_HEADER.MinorOperatingSystemVersion = BitConverter.ToUInt16(byteArray, offset + 42);
            image_OPTIONAL_HEADER.MajorImageVersion = BitConverter.ToUInt16(byteArray, offset + 44);
            image_OPTIONAL_HEADER.MinorImageVersion = BitConverter.ToUInt16(byteArray, offset + 46);
            image_OPTIONAL_HEADER.MajorSubsystemVersion = BitConverter.ToUInt16(byteArray, offset + 48);
            image_OPTIONAL_HEADER.MinorSubsystemVersion = BitConverter.ToUInt16(byteArray, offset + 50);
            image_OPTIONAL_HEADER.Win32VersionValue = BitConverter.ToUInt32(byteArray, offset + 52);
            image_OPTIONAL_HEADER.SizeOfImage = BitConverter.ToUInt32(byteArray, offset + 56);
            image_OPTIONAL_HEADER.SizeOfHeaders = BitConverter.ToUInt32(byteArray, offset + 60);
            image_OPTIONAL_HEADER.CheckSum = BitConverter.ToUInt32(byteArray, offset + 64);
            image_OPTIONAL_HEADER.Subsystem = BitConverter.ToUInt16(byteArray, offset + 68);
            image_OPTIONAL_HEADER.DllCharacteristics = BitConverter.ToUInt16(byteArray, offset + 70);
            image_OPTIONAL_HEADER.SizeOfStackReserve = BitConverter.ToUInt32(byteArray, offset + 72);
            image_OPTIONAL_HEADER.SizeOfStackCommit = BitConverter.ToUInt32(byteArray, offset + 76);
            image_OPTIONAL_HEADER.SizeOfHeapReserve = BitConverter.ToUInt32(byteArray, offset + 80);
            image_OPTIONAL_HEADER.SizeOfHeapCommit = BitConverter.ToUInt32(byteArray, offset + 84);
            image_OPTIONAL_HEADER.LoaderFlags = BitConverter.ToUInt32(byteArray, offset + 88);
            image_OPTIONAL_HEADER.NumberOfRvaAndSizes = BitConverter.ToUInt32(byteArray, offset + 92);
            image_OPTIONAL_HEADER.DataDirectory = new IMAGE_DATA_DIRECTORY[16];
            for (int i = 0; i < 10; i++)
            {
                IMAGE_DATA_DIRECTORY image_DATA_DIRECTORY;
                image_DATA_DIRECTORY.VirtualAddress = BitConverter.ToUInt32(byteArray, offset + (96 + i * 8));
                image_DATA_DIRECTORY.Size = BitConverter.ToUInt32(byteArray, offset + (96 + i * 8) + 4);
                image_OPTIONAL_HEADER.DataDirectory[i] = image_DATA_DIRECTORY;
            }
            return image_OPTIONAL_HEADER;
        }

        private static IMAGE_FILE_HEADER toImageFileHeader(byte[] byteArray, int offset)
        {
            IMAGE_FILE_HEADER result;
            result.Machine = BitConverter.ToUInt16(byteArray, offset);
            result.NumberOfSections = BitConverter.ToUInt16(byteArray, offset + 2);
            result.TimeDateStamp = BitConverter.ToUInt32(byteArray, offset + 4);
            result.PointerToSymbolTable = BitConverter.ToUInt32(byteArray, offset + 8);
            result.NumberOfSymbols = BitConverter.ToUInt32(byteArray, offset + 12);
            result.SizeOfOptionalHeader = BitConverter.ToUInt16(byteArray, offset + 16);
            result.Characteristics = BitConverter.ToUInt16(byteArray, offset + 18);
            return result;
        }

        private static IMAGE_DOS_HEADER toImageDosHeader(byte[] byteArray, int offset)
        {
            IMAGE_DOS_HEADER image_DOS_HEADER;
            image_DOS_HEADER.e_magic = BitConverter.ToUInt16(byteArray, offset);
            image_DOS_HEADER.e_cblp = BitConverter.ToUInt16(byteArray, offset + 2);
            image_DOS_HEADER.e_cp = BitConverter.ToUInt16(byteArray, offset + 4);
            image_DOS_HEADER.e_crlc = BitConverter.ToUInt16(byteArray, offset + 6);
            image_DOS_HEADER.e_cparhdr = BitConverter.ToUInt16(byteArray, offset + 8);
            image_DOS_HEADER.e_minalloc = BitConverter.ToUInt16(byteArray, offset + 10);
            image_DOS_HEADER.e_maxalloc = BitConverter.ToUInt16(byteArray, offset + 12);
            image_DOS_HEADER.e_ss = BitConverter.ToUInt16(byteArray, offset + 14);
            image_DOS_HEADER.e_sp = BitConverter.ToUInt16(byteArray, offset + 16);
            image_DOS_HEADER.e_csum = BitConverter.ToUInt16(byteArray, offset + 18);
            image_DOS_HEADER.e_ip = BitConverter.ToUInt16(byteArray, offset + 20);
            image_DOS_HEADER.e_cs = BitConverter.ToUInt16(byteArray, offset + 22);
            image_DOS_HEADER.e_lfarlc = BitConverter.ToUInt16(byteArray, offset + 24);
            image_DOS_HEADER.e_ovno = BitConverter.ToUInt16(byteArray, offset + 26);
            image_DOS_HEADER.e_res = new ushort[4];
            image_DOS_HEADER.e_res[0] = BitConverter.ToUInt16(byteArray, offset + 28);
            image_DOS_HEADER.e_res[1] = BitConverter.ToUInt16(byteArray, offset + 30);
            image_DOS_HEADER.e_res[2] = BitConverter.ToUInt16(byteArray, offset + 32);
            image_DOS_HEADER.e_res[3] = BitConverter.ToUInt16(byteArray, offset + 34);
            image_DOS_HEADER.e_oemid = BitConverter.ToUInt16(byteArray, offset + 36);
            image_DOS_HEADER.e_oeminfo = BitConverter.ToUInt16(byteArray, offset + 38);
            image_DOS_HEADER.e_res2 = new ushort[10];
            image_DOS_HEADER.e_res2[0] = BitConverter.ToUInt16(byteArray, offset + 40);
            image_DOS_HEADER.e_res2[1] = BitConverter.ToUInt16(byteArray, offset + 42);
            image_DOS_HEADER.e_res2[2] = BitConverter.ToUInt16(byteArray, offset + 44);
            image_DOS_HEADER.e_res2[3] = BitConverter.ToUInt16(byteArray, offset + 46);
            image_DOS_HEADER.e_res2[4] = BitConverter.ToUInt16(byteArray, offset + 48);
            image_DOS_HEADER.e_res2[5] = BitConverter.ToUInt16(byteArray, offset + 50);
            image_DOS_HEADER.e_res2[6] = BitConverter.ToUInt16(byteArray, offset + 52);
            image_DOS_HEADER.e_res2[7] = BitConverter.ToUInt16(byteArray, offset + 54);
            image_DOS_HEADER.e_res2[8] = BitConverter.ToUInt16(byteArray, offset + 56);
            image_DOS_HEADER.e_res2[9] = BitConverter.ToUInt16(byteArray, offset + 58);
            image_DOS_HEADER.e_lfanew = BitConverter.ToUInt32(byteArray, offset + 60);
            return image_DOS_HEADER;
        }

        private static IMAGE_NT_HEADERS toImageNtHeaders(byte[] byteArray, int offset)
        {
            IMAGE_NT_HEADERS result;
            result.Signature = BitConverter.ToUInt32(byteArray, offset);
            result.FileHeader = toImageFileHeader(byteArray, offset + 4);
            result.OptionalHeader = toImageOptionalHeader(byteArray, offset + 24);
            return result;
        }

        private static IMAGE_SECTION_HEADER toImageSectionHeader(byte[] byteArray, int offset)
        {
            IMAGE_SECTION_HEADER image_SECTION_HEADER;
            image_SECTION_HEADER.Name = new byte[8];
            for (int i = 0; i < 8; i++)
            {
                image_SECTION_HEADER.Name[i] = (byte)BitConverter.ToChar(byteArray, offset + i);
            }
            image_SECTION_HEADER.PhysicalAddressOrVirtualSize = BitConverter.ToUInt32(byteArray, offset + 8);
            image_SECTION_HEADER.VirtualAddress = BitConverter.ToUInt32(byteArray, offset + 12);
            image_SECTION_HEADER.SizeOfRawData = BitConverter.ToUInt32(byteArray, offset + 16);
            image_SECTION_HEADER.PointerToRawData = BitConverter.ToUInt32(byteArray, offset + 20);
            image_SECTION_HEADER.PointerToRelocations = BitConverter.ToUInt32(byteArray, offset + 24);
            image_SECTION_HEADER.PointerToLinenumbers = BitConverter.ToUInt32(byteArray, offset + 28);
            image_SECTION_HEADER.NumberOfRelocations = BitConverter.ToUInt16(byteArray, offset + 32);
            image_SECTION_HEADER.NumberOfLinenumbers = BitConverter.ToUInt16(byteArray, offset + 36);
            image_SECTION_HEADER.Characteristics = BitConverter.ToUInt32(byteArray, offset + 40);
            return image_SECTION_HEADER;
        }

        private static byte[] makePayload()
        {
            return new byte[]
            {
                85,
                139,
                236,
                131,
                236,
                96,
                131,
                125,
                8,
                0,
                117,
                15,
                139,
                69,
                8,
                199,
                64,
                12,
                64,
                64,
                64,
                0,
                233,
                117,
                2,
                0,
                0,
                139,
                77,
                8,
                139,
                81,
                8,
                137,
                85,
                252,
                139,
                69,
                252,
                139,
                72,
                60,
                139,
                85,
                252,
                141,
                68,
                10,
                24,
                137,
                69,
                244,
                139,
                77,
                8,
                139,
                17,
                137,
                85,
                196,
                139,
                69,
                8,
                139,
                72,
                4,
                137,
                77,
                208,
                139,
                85,
                244,
                139,
                69,
                252,
                3,
                66,
                16,
                137,
                69,
                164,
                139,
                77,
                244,
                139,
                85,
                252,
                43,
                81,
                28,
                137,
                85,
                216,
                15,
                132,
                195,
                0,
                0,
                0,
                184,
                8,
                0,
                0,
                0,
                107,
                200,
                5,
                139,
                85,
                244,
                131,
                124,
                10,
                100,
                0,
                117,
                15,
                139,
                69,
                8,
                199,
                64,
                12,
                96,
                96,
                96,
                0,
                233,
                12,
                2,
                0,
                0,
                185,
                8,
                0,
                0,
                0,
                107,
                209,
                5,
                139,
                69,
                244,
                139,
                77,
                252,
                3,
                76,
                16,
                96,
                137,
                77,
                240,
                139,
                85,
                240,
                131,
                58,
                0,
                15,
                132,
                129,
                0,
                0,
                0,
                139,
                69,
                240,
                139,
                72,
                4,
                131,
                233,
                8,
                209,
                233,
                137,
                77,
                200,
                139,
                85,
                240,
                131,
                194,
                8,
                137,
                85,
                224,
                199,
                69,
                220,
                0,
                0,
                0,
                0,
                235,
                18,
                139,
                69,
                220,
                131,
                192,
                1,
                137,
                69,
                220,
                139,
                77,
                224,
                131,
                193,
                2,
                137,
                77,
                224,
                139,
                85,
                220,
                59,
                85,
                200,
                116,
                54,
                139,
                69,
                224,
                15,
                183,
                8,
                193,
                249,
                12,
                131,
                249,
                3,
                117,
                38,
                139,
                85,
                240,
                139,
                69,
                252,
                3,
                2,
                139,
                77,
                224,
                15,
                183,
                17,
                129,
                226,
                byte.MaxValue,
                15,
                0,
                0,
                3,
                194,
                137,
                69,
                212,
                139,
                69,
                212,
                139,
                8,
                3,
                77,
                216,
                139,
                85,
                212,
                137,
                10,
                235,
                176,
                139,
                69,
                240,
                139,
                77,
                240,
                3,
                72,
                4,
                137,
                77,
                240,
                233,
                115,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                186,
                8,
                0,
                0,
                0,
                193,
                226,
                0,
                139,
                69,
                244,
                131,
                124,
                16,
                100,
                0,
                15,
                132,
                220,
                0,
                0,
                0,
                185,
                8,
                0,
                0,
                0,
                193,
                225,
                0,
                139,
                85,
                244,
                139,
                69,
                252,
                3,
                68,
                10,
                96,
                137,
                69,
                236,
                139,
                77,
                236,
                131,
                121,
                12,
                0,
                15,
                132,
                186,
                0,
                0,
                0,
                139,
                85,
                236,
                139,
                69,
                252,
                3,
                66,
                12,
                137,
                69,
                192,
                139,
                77,
                196,
                137,
                77,
                188,
                139,
                85,
                192,
                82,
                byte.MaxValue,
                85,
                188,
                137,
                69,
                204,
                139,
                69,
                236,
                139,
                77,
                252,
                3,
                8,
                137,
                77,
                248,
                139,
                85,
                236,
                139,
                69,
                252,
                3,
                66,
                16,
                137,
                69,
                232,
                131,
                125,
                248,
                0,
                117,
                6,
                139,
                77,
                232,
                137,
                77,
                248,
                235,
                18,
                139,
                85,
                248,
                131,
                194,
                4,
                137,
                85,
                248,
                139,
                69,
                232,
                131,
                192,
                4,
                137,
                69,
                232,
                139,
                77,
                248,
                131,
                57,
                0,
                116,
                81,
                139,
                85,
                248,
                139,
                2,
                37,
                0,
                0,
                0,
                128,
                116,
                31,
                139,
                77,
                208,
                137,
                77,
                184,
                139,
                85,
                248,
                139,
                2,
                37,
                byte.MaxValue,
                byte.MaxValue,
                0,
                0,
                80,
                139,
                77,
                204,
                81,
                byte.MaxValue,
                85,
                184,
                139,
                85,
                232,
                137,
                2,
                235,
                36,
                139,
                69,
                248,
                139,
                77,
                252,
                3,
                8,
                137,
                77,
                180,
                139,
                85,
                208,
                137,
                85,
                176,
                139,
                69,
                180,
                131,
                192,
                2,
                80,
                139,
                77,
                204,
                81,
                byte.MaxValue,
                85,
                176,
                139,
                85,
                232,
                137,
                2,
                235,
                149,
                139,
                69,
                236,
                131,
                192,
                20,
                137,
                69,
                236,
                233,
                57,
                byte.MaxValue,
                byte.MaxValue,
                byte.MaxValue,
                185,
                8,
                0,
                0,
                0,
                107,
                209,
                9,
                139,
                69,
                244,
                131,
                124,
                16,
                100,
                0,
                116,
                76,
                185,
                8,
                0,
                0,
                0,
                107,
                209,
                9,
                139,
                69,
                244,
                139,
                77,
                252,
                3,
                76,
                16,
                96,
                137,
                77,
                172,
                139,
                85,
                172,
                139,
                66,
                12,
                137,
                69,
                228,
                235,
                9,
                139,
                77,
                228,
                131,
                193,
                4,
                137,
                77,
                228,
                131,
                125,
                228,
                0,
                116,
                29,
                139,
                85,
                228,
                131,
                58,
                0,
                116,
                21,
                139,
                69,
                228,
                139,
                8,
                137,
                77,
                168,
                106,
                0,
                106,
                1,
                139,
                85,
                252,
                82,
                byte.MaxValue,
                85,
                168,
                235,
                212,
                139,
                69,
                164,
                137,
                69,
                160,
                106,
                0,
                106,
                1,
                139,
                77,
                252,
                81,
                byte.MaxValue,
                85,
                160,
                139,
                85,
                8,
                139,
                69,
                252,
                137,
                66,
                12,
                139,
                229,
                93,
                194,
                4,
                0
            };
        }

        public static bool ManualMap(Process proc, string filepath)
        {
            int num = Imports.OpenProcess(2035711U, false, proc.Id);
            if (num == 0)
            {
                throw new Exception("Could not open process");
            }
            int num2 = 0;
            if (previousHandle == num)
            {
                byte[] array = new byte[previousModuleSize];
                Array.Clear(array, 0, previousModuleSize);
                Imports.WriteProcessMemory(num, previousModuleBase, array, previousModuleSize, ref num2);
                Imports.VirtualFreeEx(num, previousModuleBase, 0, 32768U);
                previousModuleBase = 0;
                previousModuleSize = 0;
            }
            previousHandle = num;
            byte[] array2 = File.ReadAllBytes(filepath);
            IMAGE_DOS_HEADER image_DOS_HEADER = toImageDosHeader(array2, 0);
            if (image_DOS_HEADER.e_magic != 23117)
            {
                throw new Exception("Invalid file type");
            }
            IMAGE_NT_HEADERS image_NT_HEADERS = toImageNtHeaders(array2, (int)image_DOS_HEADER.e_lfanew);
            IMAGE_OPTIONAL_HEADER optionalHeader = image_NT_HEADERS.OptionalHeader;
            IMAGE_FILE_HEADER fileHeader = image_NT_HEADERS.FileHeader;
            if (fileHeader.Machine != 332)
            {
                throw new Exception("Invalid platform");
            }
            int num3 = Imports.VirtualAllocEx(num, 0, 4096, 12288U, 64U);
            int num4 = Imports.VirtualAllocEx(num, 0, (int)optionalHeader.SizeOfImage, 12288U, 64U);
            if (num4 == 0 || num3 == 0)
            {
                throw new Exception("Target process memory allocation failed (ex) [Error Code: " + Imports.GetLastError().ToString() + "]");
            }
            previousModuleBase = num4;
            previousModuleSize = (int)optionalHeader.SizeOfImage;
            byte[] array3 = new byte[4096];
            Array.Clear(array3, 0, array3.Length);
            Imports.WriteProcessMemory(num, num3, array3, array3.Length, ref num2);
            byte[] array4 = new byte[optionalHeader.SizeOfImage];
            Array.Clear(array4, 0, array4.Length);
            Imports.WriteProcessMemory(num, num4, array4, array4.Length, ref num2);
            MANUAL_MAPPING_DATA manual_MAPPING_DATA;
            manual_MAPPING_DATA.pLoadLibraryA = Imports.GetProcAddress(Imports.GetModuleHandle("KERNEL32.dll"), "LoadLibraryA");
            manual_MAPPING_DATA.pGetProcAddress = Imports.GetProcAddress(Imports.GetModuleHandle("KERNEL32.dll"), "GetProcAddress");
            manual_MAPPING_DATA.pbase = num4;
            manual_MAPPING_DATA.hMod = 0;
            if (Imports.WriteProcessMemory(num, num4, array2, 4096, ref num2) == 0)
            {
                throw new Exception("Can't write file header [Error Code: " + Imports.GetLastError().ToString() + "]");
            }
            int e_lfanew = (int)image_DOS_HEADER.e_lfanew;
            int num5 = e_lfanew + 24 + (int)image_NT_HEADERS.FileHeader.SizeOfOptionalHeader;
            uint num6 = 0U;
            while (num6 != (uint)fileHeader.NumberOfSections)
            {
                IMAGE_SECTION_HEADER image_SECTION_HEADER = toImageSectionHeader(array2, num5);
                if (image_SECTION_HEADER.SizeOfRawData != 0U)
                {
                    byte[] array5 = new byte[image_SECTION_HEADER.SizeOfRawData];
                    for (int i = 0; i < (int)image_SECTION_HEADER.SizeOfRawData; i++)
                    {
                        array5[i] = array2[(int)(checked((IntPtr)(unchecked((ulong)image_SECTION_HEADER.PointerToRawData + (ulong)((long)i)))))];
                    }
                    if (Imports.WriteProcessMemory(num, num4 + (int)image_SECTION_HEADER.VirtualAddress, array5, array5.Length, ref num2) == 0)
                    {
                        throw new Exception("Can't map sections [Error Code: " + Imports.GetLastError().ToString() + "]");
                    }
                }
                num6 += 1U;
                num5 += 40;
            }
            int num7 = Imports.VirtualAllocEx(num, 0, 16, 4096U, 4U);
            if (num7 == 0)
            {
                throw new Exception("Target process mapping allocation failed (ex) [Error Code: " + Imports.GetLastError().ToString() + "]");
            }
            Imports.WriteProcessMemory(num, num7, BitConverter.GetBytes(manual_MAPPING_DATA.pLoadLibraryA), 4, ref num2);
            Imports.WriteProcessMemory(num, num7 + 4, BitConverter.GetBytes(manual_MAPPING_DATA.pGetProcAddress), 4, ref num2);
            Imports.WriteProcessMemory(num, num7 + 8, BitConverter.GetBytes(manual_MAPPING_DATA.pbase), 4, ref num2);
            Imports.WriteProcessMemory(num, num7 + 12, BitConverter.GetBytes(manual_MAPPING_DATA.hMod), 4, ref num2);
            if (num3 == 0)
            {
                throw new Exception("Memory shellcode allocation failed (ex) [Error Code: " + Imports.GetLastError().ToString() + "]");
            }
            byte[] array6 = makePayload();
            if (Imports.WriteProcessMemory(num, num3, array6, array6.Length, ref num2) == 0)
            {
                throw new Exception("Can't write shellcode [Error Code: " + Imports.GetLastError().ToString() + "]");
            }
            int num8 = 0;
            int num9 = Imports.CreateRemoteThread(num, 0, 0U, num3, num7, 0U, out num8);
            if (num9 == 0 || num8 == 0)
            {
                throw new Exception("Thread creation failed [Error Code: " + Imports.GetLastError().ToString() + "]");
            }
            Imports.WaitForSingleObjectEx(num9, 1000, true);
            Imports.CloseHandle(num9);
            int num10 = 0;
            while (num10 == 0)
            {
                uint num11 = 0U;
                Imports.GetExitCodeProcess(num, out num11);
                if (num11 != 259U)
                {
                    throw new Exception("Process crashed, exit code: " + num11.ToString());
                }
                byte[] array7 = new byte[16];
                if (Imports.ReadProcessMemory(num, num7, array7, 16, ref num2) == 0)
                {
                    throw new Exception("Failed to read process memory");
                }
                MANUAL_MAPPING_DATA manual_MAPPING_DATA2;
                manual_MAPPING_DATA2.pLoadLibraryA = BitConverter.ToInt32(array7, 0);
                manual_MAPPING_DATA2.pGetProcAddress = BitConverter.ToInt32(array7, 4);
                manual_MAPPING_DATA2.pbase = BitConverter.ToInt32(array7, 8);
                manual_MAPPING_DATA2.hMod = BitConverter.ToInt32(array7, 12);
                num10 = manual_MAPPING_DATA2.hMod;
                if (num10 == 4210752)
                {
                    throw new Exception("Wrong mapping ptr");
                }
                if (num10 == 6316128)
                {
                    throw new Exception("Wrong directory base relocation");
                }
                Thread.Sleep(10);
            }
            byte[] array8 = new byte[4096];
            Array.Clear(array8, 0, array8.Length);
            if (Imports.WriteProcessMemory(num, num4, array8, 4096, ref num2) == 0)
            {
                throw new Exception("Failed to erase file header(s)");
            }
            byte[] array9 = new byte[1048576];
            Array.Clear(array9, 0, array9.Length);
            num5 = e_lfanew + 24 + (int)image_NT_HEADERS.FileHeader.SizeOfOptionalHeader;
            uint num12 = 0U;
            while (num12 != (uint)fileHeader.NumberOfSections)
            {
                IMAGE_SECTION_HEADER image_SECTION_HEADER2 = toImageSectionHeader(array2, num5);
                if (image_SECTION_HEADER2.SizeOfRawData != 0U)
                {
                    string text = "";
                    byte[] array10 = new byte[16];
                    Imports.ReadProcessMemory(num, num5, array10, 16, ref num2);
                    int num13 = 0;
                    while (num13 < 16 && array10[num13] >= 32 && array10[num13] < 127)
                    {
                        string str = text;
                        char c = (char)array10[num13];
                        text = str + c.ToString();
                        num13++;
                    }
                    if ((text == ".pdata" || text == ".rsrc" || text == ".reloc") && Imports.WriteProcessMemory(num, num4 + (int)image_SECTION_HEADER2.VirtualAddress, array9, (int)image_SECTION_HEADER2.SizeOfRawData, ref num2) == 0)
                    {
                        throw new Exception(string.Concat(new string[]
                        {
                            "Can't clear section ",
                            text,
                            " [Error code: ",
                            Imports.GetLastError().ToString(),
                            "]"
                        }));
                    }
                }
                num12 += 1U;
                num5 += 40;
            }
            return true;
        }

        private const int IMAGE_NUMBEROF_DIRECTORY_ENTRIES = 16;

        private const ushort IMAGE_FILE_MACHINE_I386 = 332;

        private const ushort CURRENT_ARCH = 332;

        private const uint STATUS_PENDING = 259U;

        private const uint STILL_ACTIVE = 259U;

        private static int previousHandle;

        private static int previousModuleBase;

        private static int previousModuleSize;

        private class Imports
        {
            [DllImport("kernel32.dll")]
            public static extern int OpenProcess(uint dwDesiredAccess, bool bInheritHandle, int dwProcessId);

            [DllImport("kernel32.dll")]
            public static extern int ReadProcessMemory(int hProcess, int lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);

            [DllImport("kernel32.dll")]
            public static extern int WriteProcessMemory(int hProcess, int lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesWritten);

            [DllImport("kernel32.dll")]
            public static extern int VirtualProtectEx(int hProcess, int lpBaseAddress, int dwSize, uint new_protect, ref uint lpOldProtect);

            [DllImport("kernel32.dll")]
            public static extern int VirtualQueryEx(int hProcess, int lpAddress, out Imports.MEMORY_BASIC_INFORMATION lpBuffer, uint dwLength);

            [DllImport("kernel32.dll")]
            public static extern int VirtualAllocEx(int hProcess, int lpAddress, int size, uint allocation_type, uint protect);

            [DllImport("kernel32.dll")]
            public static extern int VirtualFreeEx(int hProcess, int lpAddress, int size, uint allocation_type);

            [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
            public static extern int GetModuleHandle(string lpModuleName);

            [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
            public static extern int GetProcAddress(int hModule, string procName);

            [DllImport("kernel32.dll")]
            public static extern uint GetLastError();

            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern bool CloseHandle(int hObject);

            [DllImport("kernel32.dll", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.U4)]
            public static extern int WaitForSingleObjectEx(int hHandle, [MarshalAs(UnmanagedType.U4)] int dwMilliseconds, bool bAlertable);

            [DllImport("kernel32.dll", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool GetExitCodeProcess(int hProcess, out uint lpExitCode);

            [DllImport("kernel32.dll")]
            public static extern int CreateRemoteThread(int hProcess, int lpThreadAttributes, uint dwStackSize, int lpStartAddress, int lpParameter, uint dwCreationFlags, out int lpThreadId);

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

            public const int EXCEPTION_CONTINUE_EXECUTION = -1;

            public const int EXCEPTION_CONTINUE_SEARCH = 0;

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
        }

        private struct IMAGE_DATA_DIRECTORY
        {
            public uint VirtualAddress;

            public uint Size;
        }

        private struct IMAGE_OPTIONAL_HEADER
        {
            public ushort Magic;

            public byte MajorLinkerVersion;

            public byte MinorLinkerVersion;

            public uint SizeOfCode;

            public uint SizeOfInitializedData;

            public uint SizeOfUninitializedData;

            public uint AddressOfEntryPoint;

            public uint BaseOfCode;

            public uint BaseOfData;

            public uint ImageBase;

            public uint SectionAlignment;

            public uint FileAlignment;

            public ushort MajorOperatingSystemVersion;

            public ushort MinorOperatingSystemVersion;

            public ushort MajorImageVersion;

            public ushort MinorImageVersion;

            public ushort MajorSubsystemVersion;

            public ushort MinorSubsystemVersion;

            public uint Win32VersionValue;

            public uint SizeOfImage;

            public uint SizeOfHeaders;

            public uint CheckSum;

            public ushort Subsystem;

            public ushort DllCharacteristics;

            public uint SizeOfStackReserve;

            public uint SizeOfStackCommit;

            public uint SizeOfHeapReserve;

            public uint SizeOfHeapCommit;

            public uint LoaderFlags;

            public uint NumberOfRvaAndSizes;

            public IMAGE_DATA_DIRECTORY[] DataDirectory;
        }

        private struct IMAGE_FILE_HEADER
        {
            public ushort Machine;

            public ushort NumberOfSections;

            public uint TimeDateStamp;

            public uint PointerToSymbolTable;

            public uint NumberOfSymbols;

            public ushort SizeOfOptionalHeader;

            public ushort Characteristics;
        }

        private struct IMAGE_DOS_HEADER
        {
            public ushort e_magic;

            public ushort e_cblp;

            public ushort e_cp;

            public ushort e_crlc;

            public ushort e_cparhdr;

            public ushort e_minalloc;

            public ushort e_maxalloc;

            public ushort e_ss;

            public ushort e_sp;

            public ushort e_csum;

            public ushort e_ip;

            public ushort e_cs;

            public ushort e_lfarlc;

            public ushort e_ovno;

            public ushort[] e_res;

            public ushort e_oemid;

            public ushort e_oeminfo;

            public ushort[] e_res2;

            public uint e_lfanew;
        }

        private struct IMAGE_NT_HEADERS
        {
            public uint Signature;

            public IMAGE_FILE_HEADER FileHeader;

            public IMAGE_OPTIONAL_HEADER OptionalHeader;
        }

        private struct MANUAL_MAPPING_DATA
        {
            public int pLoadLibraryA;

            public int pGetProcAddress;

            public int pbase;

            public int hMod;
        }

        private struct IMAGE_SECTION_HEADER
        {
            public byte[] Name;

            public uint PhysicalAddressOrVirtualSize;

            public uint VirtualAddress;

            public uint SizeOfRawData;

            public uint PointerToRawData;

            public uint PointerToRelocations;

            public uint PointerToLinenumbers;

            public ushort NumberOfRelocations;

            public ushort NumberOfLinenumbers;

            public uint Characteristics;
        }
    }
}
