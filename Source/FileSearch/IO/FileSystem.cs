using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;
using FileSearch.Model;
using Directory = System.IO.Directory;

namespace FileSearch.IO
{
    public class FileSystem : IFileSystem
    {
        #region Private Methods

        private const uint FILE_ATTRIBUTE_DIRECTORY = 0x00000010;
        private const uint FILE_ATTRIBUTE_NORMAL = 0x00000080; 

        [DllImport("Shell32.dll")]
        private static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, out SHFILEINFO psfi, uint cbfileInfo, SHGFI uFlags);

        private static Icon GetIcon(string filePath, uint attributes)
        {
            SHFILEINFO info = new SHFILEINFO(true);
            int cbFileInfo = Marshal.SizeOf(info);
            SHGetFileInfo(filePath,  attributes, out info, (uint)cbFileInfo, SHGFI.SHGFI_ICON | SHGFI.SHGFI_LARGEICON);
            if (info.hIcon != (IntPtr)0)
            {
                return Icon.FromHandle(info.hIcon);
            }
            return null;
        }

        private static bool HasReadPermission(string directoryPath)
        {
			var security = Directory.GetAccessControl(directoryPath);
            return !security.
                        GetAccessRules(true, true, typeof (SecurityIdentifier)).
                        OfType<FileSystemAccessRule>().
                        Any(
                            rule =>
                            rule.AccessControlType == AccessControlType.Deny &&
                            (rule.FileSystemRights & FileSystemRights.ReadData) == FileSystemRights.ReadData);
        }

        #endregion


        public bool IsDirectoryExists(string directoryPath)
        {
            return Directory.Exists(directoryPath);
        }

        public IEnumerable<string> LoadDirectoryFiles(string directoryPath, string filter)
        {
            if (HasReadPermission(directoryPath))
            {
                try
                {
                    return Directory.GetFiles(directoryPath, filter);
                }
                catch (Exception)
                {
                    return new string[0];
                }
            }
            return new string[0];
        }

        public IEnumerable<string> LoadChildDirectories(string directoryPath)
        {
            if (HasReadPermission(directoryPath))
            {
                try
                {
                    return Directory.GetDirectories(directoryPath);
                }
                catch(Exception)
                {
                    return new string[0];
                }
            }
            return new string[0];
        }

        public Icon GetAssociatedIcon(string filePath, bool isDirectory)
        {
            return GetIcon(filePath, isDirectory ? FILE_ATTRIBUTE_DIRECTORY : FILE_ATTRIBUTE_NORMAL);
        }

        public FileInfo GetFileInfo(string fullPath)
        {
            return new FileInfo(fullPath);
        }
    }
}