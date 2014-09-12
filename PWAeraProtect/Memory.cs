using System;
using System.Collections;
using System.Text;
using System.Runtime.InteropServices;

namespace MemoryAccess
{
    /// <summary>
    /// Allows reading from and writing to the memory of another window.
    /// </summary>
    public class Memory
    {
        //From pinvoke.net
        [DllImport("kernel32.dll")]
        private static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle,
            uint dwProcessId);

        [DllImport("kernel32.dll")]
        private static extern bool CloseHandle(IntPtr hObject);

        //From pinvoke.net
        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        //From pinvoke.net
        [DllImport("kernel32.dll")]
        private static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress,
            [Out] byte[] lpBuffer, UIntPtr nSize, IntPtr lpNumberOfBytesRead);

        //From pinvoke.net
        [DllImport("kernel32.dll")]
        private static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress,
            byte[] lpBuffer, UIntPtr nSize, IntPtr lpNumberOfBytesWritten);


        public bool IsOpen
        {
            get { return isOpen; }
        }

        private IntPtr pHandle;
        private bool isOpen;

        /// <summary>
        /// Allows one to read and/or write from another process' memory
        /// </summary>
        /// <param name="hWnd">The handle of the window to be read/written</param>
        public Memory(IntPtr hWnd)
        {
            uint pid;
            GetWindowThreadProcessId(hWnd, out pid);
            if (pid == 0)
                throw new Exception("Could not get ProcessID");

            pHandle = OpenProcess(0x1F0FFF, false, pid);
            if (pHandle == (IntPtr)0)
                throw new Exception("Could not open process");

            isOpen = true;
        }

        /// <summary>
        /// Enables the user to read any amount of bytes from memory for future conversion.
        /// </summary>
        /// <param name="Address">Address from which to read bytes</param>
        /// <param name="buffer">The byte array to hold the bytes read.  The length of this byte array will define how many bytes to read.</param>
        /// <returns>Returns true if successful, false if not</returns>
        public bool ReadMemory(uint Address, ref byte[] buffer)
        {
            if (!isOpen)
                throw new Exception("Process is not opened for reading");

            if (ReadProcessMemory(pHandle, (IntPtr)Address, buffer, (UIntPtr)buffer.Length, (IntPtr)0))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Enables the user to read any amount of bytes from memory for future conversion.
        /// </summary>
        /// <param name="Address">Address from which to read bytes.</param>
        /// <param name="size">Number of bytes to be read (usually 1, 2, or 4)</param>
        /// <returns>Returns a byte array populated by bytes read from memory and with a length of that specified by parameter 2, "size"</returns>
        public byte[] ReadMemory(uint Address, int size)
        {
            if (!isOpen)
                throw new Exception("Process is not opened for reading");

            byte[] buffer = new byte[size];
            if (ReadProcessMemory(pHandle, (IntPtr)Address, buffer, (UIntPtr)buffer.Length, (IntPtr)0))
                return buffer;
            else
                throw new Exception("ReadProcessMemory error");
        }

        /// <summary>
        /// Reads a single byte from memory.
        /// </summary>
        /// <param name="Address">The address from which to read.</param>
        /// <returns>Returns the value read from memory</returns>
        public byte ReadByte(uint Address)
        {
            if (!isOpen)
                throw new Exception("Process is not opened for reading");

            byte[] buffer = new byte[1];

            if (ReadProcessMemory(pHandle, (IntPtr)Address, buffer, (UIntPtr)buffer.Length, (IntPtr)0))
                return buffer[0];
            else
                throw new Exception("ReadProcessMemory error");
        }

        /// <summary>
        /// Reads two bytes from memory.
        /// </summary>
        /// <param name="Address">The address from which to read.</param>
        /// <returns>Returns the value read from memory</returns>
        public short ReadShort(uint Address)
        {
            if (!isOpen)
                throw new Exception("Process is not opened for reading");

            byte[] buffer = new byte[2];

            if (ReadProcessMemory(pHandle, (IntPtr)Address, buffer, (UIntPtr)buffer.Length, (IntPtr)0))
                return BitConverter.ToInt16(buffer, 0);
            else
                throw new Exception("ReadProcessMemory error");
        }

        /// <summary>
        /// Reads two bytes from memory.
        /// </summary>
        /// <param name="Address">The address from which to read.</param>
        /// <returns>Returns the value read from memory</returns>
        public ushort ReadUShort(uint Address)
        {
            if (!isOpen)
                throw new Exception("Process is not opened for reading");

            byte[] buffer = new byte[2];

            if (ReadProcessMemory(pHandle, (IntPtr)Address, buffer, (UIntPtr)buffer.Length, (IntPtr)0))
                return BitConverter.ToUInt16(buffer, 0);
            else
                throw new Exception("ReadProcessMemory error");
        }

        /// <summary>
        /// Reads four bytes from memory.
        /// </summary>
        /// <param name="Address">The address from which to read.</param>
        /// <returns>Returns the value read from memory</returns>
        public int ReadInt(uint Address)
        {
            if (!isOpen)
                throw new Exception("Process is not opened for reading");

            byte[] buffer = new byte[4];

            if (ReadProcessMemory(pHandle, (IntPtr)Address, buffer, (UIntPtr)buffer.Length, (IntPtr)0))
                return BitConverter.ToInt32(buffer, 0);
            else
                throw new Exception("ReadProcessMemory error");
        }

        /// <summary>
        /// Reads four bytes from memory.
        /// </summary>
        /// <param name="Address">The address from which to read.</param>
        /// <returns>Returns the value read from memory</returns>
        public uint ReadUInt(uint Address)
        {
            if (!isOpen)
                throw new Exception("Process is not opened for reading");

            byte[] buffer = new byte[4];

            if (ReadProcessMemory(pHandle, (IntPtr)Address, buffer, (UIntPtr)buffer.Length, (IntPtr)0))
                return BitConverter.ToUInt32(buffer, 0);
            else
                throw new Exception("ReadProcessMemory error");
        }

        /// <summary>
        /// Reads four bytes from memory.
        /// </summary>
        /// <param name="Address">The address from which to read.</param>
        /// <returns>Returns the value read from memory</returns>
        public float ReadFloat(uint Address)
        {
            if (!isOpen)
                throw new Exception("Process is not opened for reading");

            byte[] buffer = new byte[4];

            if (ReadProcessMemory(pHandle, (IntPtr)Address, buffer, (UIntPtr)buffer.Length, (IntPtr)0))
                return BitConverter.ToSingle(buffer, 0);
            else
                throw new Exception("ReadProcessMemory error");
        }

        /// <summary>
        /// Reads a string from memory.
        /// </summary>
        /// <param name="Address">The address from which to read.</param>
        /// <param name="length">The maximum length of the string.  If the string is shorter than specified, a shorter string than the length specified here will be returned.</param>
        /// <returns>Returns the string that was read from memory.</returns>
        public string ReadString(uint Address, int length)
        {
            if (!isOpen)
                throw new Exception("Process is not opened for reading");

            byte[] buffer = new byte[length];

            if (!ReadProcessMemory(pHandle, (IntPtr)Address, buffer, (UIntPtr)buffer.Length, (IntPtr)0))
                throw new Exception("ReadProcessMemory error");
            else
            {
                string ret = System.Text.Encoding.UTF8.GetString(buffer);
                if (ret.IndexOf("\0") != -1)
                    return ret.Remove(ret.IndexOf("\0"), ret.Length - ret.IndexOf("\0"));
                else
                    return ret;
            }
        }

        /// <summary>
        /// Writes a byte array of any length to memory.
        /// </summary>
        /// <param name="Address">The address at which to write.</param>
        /// <param name="buffer">The byte array to write to memory.</param>
        /// <returns>Returns true if successful, false if not.</returns>
        public bool WriteMemory(uint Address, byte[] buffer)
        {
            if (!isOpen)
                throw new Exception("Process is not opened for reading");

            if (Address <= 0)
                throw new Exception("Address not correct format");

            if (buffer == null || buffer.Length <= 0)
                throw new Exception("Buffer not correct format");

            if (WriteProcessMemory(pHandle, (IntPtr)Address, buffer, (UIntPtr)buffer.Length, (IntPtr)0))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Writes a single byte to memory.
        /// </summary>
        /// <param name="Address">The address at which to write.</param>
        /// <param name="buffer">The value to write.</param>
        /// <returns>Returns true if successful, false if not.</returns>
        public bool WriteByte(uint Address, byte buffer)
        {
            return WriteMemory(Address, BitConverter.GetBytes(buffer));
            /*if (!isOpen)
                throw new Exception("Process is not opened for reading");

            if (WriteProcessMemory(pHandle, (IntPtr)Address, BitConverter.GetBytes(buffer), (UIntPtr)BitConverter.GetBytes(buffer).Length, (IntPtr)0))
                return true;
            else
                return false;
            */
        }

        /// <summary>
        /// Writes two byte to memory.
        /// </summary>
        /// <param name="Address">The address at which to write.</param>
        /// <param name="buffer">The value to write.</param>
        /// <returns>Returns true if successful, false if not.</returns>
        public bool WriteShort(uint Address, short buffer)
        {
            return WriteMemory(Address, BitConverter.GetBytes(buffer));

            /*if (!isOpen)
                throw new Exception("Process is not opened for reading");

            if (WriteProcessMemory(pHandle, (IntPtr)Address, BitConverter.GetBytes(buffer), (UIntPtr)BitConverter.GetBytes(buffer).Length, (IntPtr)0))
                return true;
            else
                return false;
            */
        }

        /// <summary>
        /// Writes two bytes to memory.
        /// </summary>
        /// <param name="Address">The address at which to write.</param>
        /// <param name="buffer">The value to write.</param>
        /// <returns>Returns true if successful, false if not.</returns>
        public bool WriteUShort(uint Address, ushort buffer)
        {
            return WriteMemory(Address, BitConverter.GetBytes(buffer));

            /*if (!isOpen)
                throw new Exception("Process is not opened for reading");

            if (WriteProcessMemory(pHandle, (IntPtr)Address, BitConverter.GetBytes(buffer), (UIntPtr)BitConverter.GetBytes(buffer).Length, (IntPtr)0))
                return true;
            else
                return false;
            */
        }

        /// <summary>
        /// Writes four bytes to memory.
        /// </summary>
        /// <param name="Address">The address at which to write.</param>
        /// <param name="buffer">The value to write.</param>
        /// <returns>Returns true if successful, false if not.</returns>
        public bool WriteInt(uint Address, int buffer)
        {
            return WriteMemory(Address, BitConverter.GetBytes(buffer));

            /*
            if (!isOpen)
                throw new Exception("Process is not opened for reading");

            if (WriteProcessMemory(pHandle, (IntPtr)Address, BitConverter.GetBytes(buffer), (UIntPtr)BitConverter.GetBytes(buffer).Length, (IntPtr)0))
                return true;
            else
                return false;
            */
        }

        /// <summary>
        /// Writes four bytes to memory.
        /// </summary>
        /// <param name="Address">The address at which to write.</param>
        /// <param name="buffer">The value to write.</param>
        /// <returns>Returns true if successful, false if not.</returns>
        public bool WriteUInt(uint Address, uint buffer)
        {
            return WriteMemory(Address, BitConverter.GetBytes(buffer));

            /*if (!isOpen)
                throw new Exception("Process is not opened for reading");

            if (WriteProcessMemory(pHandle, (IntPtr)Address, BitConverter.GetBytes(buffer), (UIntPtr)BitConverter.GetBytes(buffer).Length, (IntPtr)0))
                return true;
            else
                return false;
            */
        }

        /// <summary>
        /// Writes four bytes to memory.
        /// </summary>
        /// <param name="Address">The address at which to write.</param>
        /// <param name="buffer">The value to write.</param>
        /// <returns>Returns true if successful, false if not.</returns>
        public bool WriteFloat(uint Address, float buffer)
        {
            return WriteMemory(Address, BitConverter.GetBytes(buffer));

            /*if (!isOpen)
                throw new Exception("Process is not opened for reading");

            if (WriteProcessMemory(pHandle, (IntPtr)Address, BitConverter.GetBytes(buffer), (UIntPtr)BitConverter.GetBytes(buffer).Length, (IntPtr)0))
                return true;
            else
                return false;
            */
        }

        /// <summary>
        /// Writes a string to memory.
        /// </summary>
        /// <param name="Address">The address at which to write.</param>
        /// <param name="buffer">The string to write.</param>
        /// <returns>Returns true if successful, false if not.</returns>
        public bool WriteString(uint Address, string str)
        {
            if (str.Length <= 0)
                throw new Exception("String incorrect format");

            byte[] buffer = new byte[str.Length];
            for (int i = 0; i < str.Length; i++)
                buffer[i] = Convert.ToByte(str[i]);

            return WriteMemory(Address, buffer);
        }

        /// <summary>
        /// Closes the handle on the process, disabling reading and writing.
        /// </summary>
        public void Close()
        {
            CloseHandle(pHandle);
            isOpen = false;
        }
    }

    /// <summary>
    /// Gather information about windows using IsWindowVisible and GetWindowText API
    /// </summary>
    public class WindowInfo
    {
        [DllImport("user32.dll", EntryPoint = "IsWindowVisible")]
        private static extern bool _IsWindowVisible(int hWnd);
        [DllImport("user32.dll", EntryPoint = "GetWindowText")]
        private static extern int _GetWindowText(int hWnd, StringBuilder buf, int nMaxCount);

        /// <summary>
        /// Returns the window title of the specified window
        /// </summary>
        /// <param name="hWnd">A handle to the window</param>
        /// <param name="length">Length of the string to be returned</param>
        /// <returns></returns>
        public string GetWindowTitle(int hWnd, int length)
        {
            StringBuilder str = new StringBuilder(length);
            _GetWindowText(hWnd, str, length);
            return str.ToString();
        }

        /// <summary>
        /// Returns true if the window is visible, false if not
        /// </summary>
        /// <param name="hWnd">A handle to the window</param>
        /// <returns></returns>
        public bool IsVisible(int hWnd)
        {
            return _IsWindowVisible(hWnd);
        }
    }

    /// <summary>
    /// Enumerate open windows
    /// </summary>
    public class WindowArray : ArrayList
    {
        private delegate bool EnumWindowsCB(int handle, IntPtr param);

        [DllImport("user32")]
        private static extern int EnumWindows(EnumWindowsCB cb,
            IntPtr param);

        private static bool MyEnumWindowsCB(int hwnd, IntPtr param)
        {
            GCHandle gch = (GCHandle)param;
            WindowArray itw = (WindowArray)gch.Target;
            itw.Add(hwnd);
            return true;
        }

        /// <summary>
        /// Returns an array of all open windows and their hWnds
        /// </summary>
        public WindowArray()
        {
            GCHandle gch = GCHandle.Alloc(this);
            EnumWindowsCB ewcb = new EnumWindowsCB(MyEnumWindowsCB);
            EnumWindows(ewcb, (IntPtr)gch);
            gch.Free();
        }
    }
}