using Microsoft.Win32.SafeHandles;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Collections.Generic;

using AsciiProgram;//for Vector2

namespace FastConsole
{
    public class FastWrite
    {
        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern SafeFileHandle CreateFile(
        string fileName,
        [MarshalAs(UnmanagedType.U4)] uint fileAccess,
        [MarshalAs(UnmanagedType.U4)] uint fileShare,
        IntPtr securityAttributes,
        [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
        [MarshalAs(UnmanagedType.U4)] int flags,
        IntPtr template);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool WriteConsoleOutputW(
            SafeFileHandle hConsoleOutput,
            CharInfo[] lpBuffer,
            Coord dwBufferSize,
            Coord dwBufferCoord,
            ref SmallRect lpWriteRegion);

        [StructLayout(LayoutKind.Sequential)]
        public struct Coord
        {
            public short X;
            public short Y;

            public Coord(short X, short Y)
            {
                this.X = X;
                this.Y = Y;
            }
        };

        [StructLayout(LayoutKind.Explicit)]
        public struct CharUnion
        {
            [FieldOffset(0)] public ushort UnicodeChar;
            [FieldOffset(0)] public byte AsciiChar;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct CharInfo
        {
            [FieldOffset(0)] public CharUnion Char;
            [FieldOffset(2)] public short Attributes;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SmallRect
        {
            public short Left;
            public short Top;
            public short Right;
            public short Bottom;
        }

        public struct CharSetInfo
        {
            public bool set;
            public CharInfo charInfo;
        }

        static FastWrite instance;

        SafeFileHandle handle;
        List<List<CharSetInfo>> bufList;
        SmallRect rect;
        short bufWidth;
        short bufHeight;

        //singlton
        private FastWrite()
        {

        }

        static public FastWrite GetInstance()
        {
            if (instance == null)
                instance = new FastWrite();
            
            return instance;
        }

        public bool InitializeBuffer(short bufferX, short bufferY)
        {
            handle = CreateFile("CONOUT$", 0x40000000, 2, IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);

            if (!handle.IsInvalid)
            {
                List<CharSetInfo> temp = new List<CharSetInfo>(new CharSetInfo[bufferX * bufferY]);
                bufList = new List<List<CharSetInfo>>();
                bufList.Add(temp);

                rect = new SmallRect() { Left = 0, Top = 0, Right = bufferX, Bottom = bufferY };
                bufWidth = bufferX;
                bufHeight = bufferY;

                return true;
            }
            else
                return false;

        }

        bool ValidLayer(int layer)
        {
            if(layer >= 0 && layer < bufList.Count)
                return true;
            else
                return false;
        }

        int GetBufferPos(int x, int y, int layer)
        {
            if(ValidLayer(layer))
                return x + ((bufList[layer].Count / bufHeight) * y);

            return -1;
        }

        public void AddToBuffer(int x, int y, int layer, char input, ConsoleColor foreground = ConsoleColor.White, ConsoleColor background = ConsoleColor.Black)
        {
            if (ValidLayer(layer) && (GetBufferPos(x, y, layer) >= bufHeight * bufWidth || GetBufferPos(x, y, layer) < 0))
            {
                //validate x and y
                return;
            }
            else if (!ValidLayer(layer) && (GetBufferPos(x, y, layer) < bufHeight * bufWidth || GetBufferPos(x, y, layer) >= 0))
            {
                //add missing layers
                int layersToAdd = layer - bufList.Count + 1;
                for(int i = 0; i < layersToAdd; ++i)
                {
                    bufList.Add(new List<CharSetInfo>(new CharSetInfo[bufWidth * bufHeight]));
                }
            }

            CharSetInfo temp = bufList[layer][GetBufferPos(x, y, layer)];

            temp.charInfo.Attributes = (short)(((int)background << 4) | ((int)foreground & 15));
            temp.charInfo.Char.UnicodeChar = input;
            temp.set = true;

            bufList[layer][GetBufferPos(x, y, layer)] = temp;
        }
        public void AddToBuffer(int x, int y, int layer, string input, ConsoleColor foreground = ConsoleColor.White, ConsoleColor background = ConsoleColor.Black)
        {
            for (int i = 0; i < input.Length; ++i)
            {
                if (GetBufferPos(x + i, y, layer) <= GetBufferPos(bufWidth - 1, y, layer))
                    AddToBuffer(x + i, y, layer, input[i], foreground, background);
            }
        }
        public void AddToBuffer(Vector2 position, int layer, string input, ConsoleColor foreground = ConsoleColor.White, ConsoleColor background = ConsoleColor.Black)
        {
            AddToBuffer(position.x, position.y, layer, input, foreground, background);
        }
        public void AddToBuffer(Vector2 position, int layer, char input, ConsoleColor foreground = ConsoleColor.White, ConsoleColor background = ConsoleColor.Black)
        {
            AddToBuffer(position.x, position.y, layer, input, foreground, background);
        }

        public void DisplayBuffer()
        {
            CharInfo[] buffer = new CharInfo[bufWidth * bufHeight];

            for(int i = 0; i < bufList.Count; ++i)
            {
                for(int x = 0; x < bufList[i].Count; ++x)
                {
                    if (bufList[i][x].set)
                        buffer[x] = bufList[i][x].charInfo;
                }
            }

            WriteConsoleOutputW(handle, buffer, new Coord() { X = bufWidth, Y = bufHeight }, new Coord() { X = 0, Y = 0 }, ref rect);
        }

        public void ClearBuffer()
        {
            for (int i = 0; i < bufList.Count; ++i)
            {
                bufList[i].Clear();
            }

            Console.Clear();
        }

        //simple way of getting rid of somthing drawn but for more control
        //have fastwrite loop through all char infos on layer and get rid of ones that only match with what needs to be erased, then redraw all on that layer
        public void ClearLayer(int layer)
        {
            if (ValidLayer(layer))
            {
                bufList[layer].Clear();
                bufList[layer].AddRange(new CharSetInfo[bufWidth * bufHeight]);
            }
        }
    }
    
}
