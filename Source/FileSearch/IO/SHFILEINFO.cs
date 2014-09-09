using System;
using System.Runtime.InteropServices;

namespace FileSearch.IO
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct SHFILEINFO
    {
        public SHFILEINFO(bool b)
        {
            hIcon = IntPtr.Zero; iIcon = 0; dwAttributes = 0; szDisplayName = ""; szTypeName = "";
        }
        public IntPtr hIcon;
        public int iIcon;
        public uint dwAttributes;
        [MarshalAs(UnmanagedType.LPStr, SizeConst = 260)]
        public string szDisplayName;
        [MarshalAs(UnmanagedType.LPStr, SizeConst = 80)]
        public string szTypeName;
    };
}