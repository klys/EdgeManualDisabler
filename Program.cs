using System;
using System.Diagnostics;
using System.IO;

namespace EdgeManualDisabler
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Edge Disabler.");

            killProcess("Microsoft Edge");

            // C:\Windows\SystemApps
            // %programFiles%\\Microsoft\\Edge
            // %programfiles(x86)%
            string programPath = Environment.ExpandEnvironmentVariables(@"%programFiles%\\Microsoft");
            string programPath0 = Environment.ExpandEnvironmentVariables(@"%programfiles(x86)%\\Microsoft");
            string appPath = @"C:\\Windows\\SystemApps"; // Microsoft.MicrosoftEdge_8wekyb3d8bbwe
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory);//Environment.ExpandEnvironmentVariables(@"%userprofile%\\Desktop");
            string pinnedPath = Environment.ExpandEnvironmentVariables(@"%AppData%\Microsoft\Internet Explorer\\Quick Launch\\User Pinned\\TaskBar");
            string newEdgeFolder = "EdgeDisabled";

            
            
            Console.WriteLine("Desktop path: {0}", desktopPath);
            Console.WriteLine("Pinned path {0}", pinnedPath);

            findNRenameFolder(appPath, "Microsoft.MicrosoftEdge_*", newEdgeFolder);

            findNDeleteFile("Microsoft Edge.lnk", desktopPath);

            findNDeleteFile("Microsoft Edge.lnk", pinnedPath);

            findNDeleteFolder(programPath, "Edge");
            findNDeleteFolder(programPath0, "Edge");


        }

        static void setAttributesNormal(DirectoryInfo dir)
        {
            foreach (var subDir in dir.GetDirectories())
            {
                setAttributesNormal(subDir);
                subDir.Attributes = FileAttributes.Normal;
            }
            foreach (var file in dir.GetFiles())
            {
                FileAttributes attr = File.GetAttributes(@file.FullName);
                if (!((attr & FileAttributes.Directory) == FileAttributes.Directory))
                {
                    Console.WriteLine("{0} is a file.", file.FullName);
                    file.Attributes = FileAttributes.Normal;
                }
                    
            }
        }

        static void killProcess(string ProcessName)
        {
            foreach (var process in Process.GetProcessesByName(ProcessName))
            {
                process.Kill();
            }
        }

        static void findNDeleteFile(string fileName, string path)
        {
            string[] Files = Directory.GetFiles(path, fileName);

            Console.WriteLine("files found: {0}", Files.Length);


            foreach (string file in Files)
            {
                if (File.Exists(file))
                {
                    File.Delete(file);
                }
            }
        }

        static void findNRenameFolder(string path, string oldFolderName, string newFolderName)
        {
            string[] Folders = Directory.GetDirectories(path, oldFolderName);

            Console.WriteLine("Folders found: {0}", Folders.Length);
            foreach (string folder in Folders)
            {
                Console.WriteLine("Renaming folder: {0}", folder);
                //setAttributesNormal(new DirectoryInfo(@folder));
                if (Directory.Exists(folder) && !folder.Contains("DevTool"))
                {
                    int existings = 0;
                    string ext = "";
                    while (Directory.Exists(path + newFolderName + ext))
                    {
                        existings++;
                        ext = existings.ToString();
                    }
                    Console.WriteLine("New name of folder: {0}", path + newFolderName + ext);
                    Directory.Move(folder, path + newFolderName + ext);

                }
            }
        }

        static void findNDeleteFolder(string path, string FolderName)
        {
            if (!Directory.Exists(path)) return;

            string[] Folders = Directory.GetDirectories(path, FolderName);

            Console.WriteLine("Folders found: {0}", Folders.Length);
            foreach (string folder in Folders)
            {
                
                //setAttributesNormal(new DirectoryInfo(@folder));
                if (Directory.Exists(folder))
                {
                    Console.WriteLine("Deleting folder: {0}", folder);    
                    Directory.Delete(folder, true);

                }
            }
        }
    }
}
