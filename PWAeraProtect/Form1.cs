using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;


namespace PWAeraProtect
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            int hWnd = 0;
            MemoryAccess.Memory memory;
            MemoryAccess.WindowInfo win = new MemoryAccess.WindowInfo();
            MemoryAccess.WindowArray winenum = new MemoryAccess.WindowArray();
            foreach (int hwnd in winenum)
                if (win.IsVisible(hwnd) && win.GetWindowTitle(hwnd, 100).IndexOf("Element Client") == 0)
                {
                    hWnd = hwnd;
                    break;
                }

            if (hWnd == 0)
            {
                //Console.WriteLine("");
                Console.ReadLine();
                label1.Text = ("Perfect World window not found!");
            }
            else
            {
                memory = new MemoryAccess.Memory((IntPtr)hWnd);

                while (memory.IsOpen)
                {
                    MessageBox.Show("Animation: " + GetAnimation(memory));
                    System.Threading.Thread.Sleep(500);
                }
            }
        }
        static uint GetAnimation(MemoryAccess.Memory mem)
        {
            return mem.ReadUInt(mem.ReadUInt(mem.ReadUInt(0x0000093C) + 0x20) + 0x448);
        }
    }
}
