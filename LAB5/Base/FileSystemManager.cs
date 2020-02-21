using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using LAB5.Exception_Classes;

namespace LAB5.Base
{
    internal enum CreationMode
    {
        Create,
        Append,
        Exists
    }

    internal static class FileSystemManager
    {
        public static bool CheckPathValidity(params string[] arr)
        {
            var invalidpaths = new List<string>();
            foreach (var path in arr)
                if (!File.Exists(path))
                {
                    if (Directory.Exists(path))
                        continue;

                    invalidpaths.Add(path);
                }

            if (invalidpaths.Count > 0)
                throw new EgyptDirectoryNotFoundException("File directory not found", invalidpaths);
            return true;
        }

        public static void FileInfo(string path)
        {
            CheckPathValidity(path);
            var file = new FileInfo(path);
            PrintBegin("FILE INFO");
            Console.WriteLine($"Path: {file.DirectoryName}");
            Console.WriteLine($"File Name: {file.Name}");
            Console.WriteLine($"Creation Time: {file.CreationTime}");
            Console.WriteLine($"Last Write Time: {file.LastWriteTime}");
            PrintEnd();
        }

        public static void FindFile(string directory, string filename)
        {
            var foundFiles = Directory.GetFiles(directory, filename, SearchOption.AllDirectories);
            PrintBegin("FILE SEARCH");
            if (foundFiles.Length == 0)
            {
                Console.WriteLine($"\'{filename}\' not found in \'{directory}\' folder and its subfolders");
            }
            else
            {
                Console.WriteLine(Regex.IsMatch(filename, "[*/?\\\\]")
                    ? $"There are {foundFiles.Length} files with \'{filename}\' pattern in \'{directory}\' :\n"
                    : $"There are {foundFiles.Length} files with name \'{filename}\' in \'{directory}\' :\n");

                foreach (var file in foundFiles)
                    Console.WriteLine($"- {file}");
            }

            PrintEnd();
        }

        public static void CreateFolder(string directory, string foldername)
        {
            CheckPathValidity(directory);
            var path = Path.Combine(directory, foldername);
            if (Directory.Exists(path))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n-- Folder \'{path}\' already exists");
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Directory.CreateDirectory(Path.Combine(directory, foldername));
                Console.WriteLine($"\n-- Folder \'{foldername}\' created in \'{directory}\'");
            }
        }

        public static void CreateFile(string directory, string filename, string content, CreationMode mode)
        {
            CheckPathValidity(directory);
            var path = Path.Combine(directory, filename);
            switch (mode)
            {
                case CreationMode.Create:
                {
                    File.Create(path);
                    Console.WriteLine($"\n-- File \'{filename}\' created in \'{directory}\'");
                }
                    break;
                case CreationMode.Append:
                {
                    File.AppendAllText(path, content);
                    Console.WriteLine($"\n-- Added text to \'{filename}\' in \'{directory}\'");
                }
                    break;
                case CreationMode.Exists:
                {
                    if (File.Exists(path))
                        throw new DirectoryNotFoundException($"CreateFile(mode Exists): \'{Path.Combine(directory, filename)}\' already exists");
                    
                    goto case CreationMode.Create;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }
        }

        public static string ReadFile(string path)
        {
            CheckPathValidity(path);
            return File.ReadAllText(path);
        }

        public static void FileSystemInfo()
        {
            PrintBegin("FILE SYSTEM INFO");
            foreach (var drive in DriveInfo.GetDrives())
            {
                Console.WriteLine($"Drive: {drive.Name}");
                Console.WriteLine($"   Size:\t{drive.TotalSize / 1000000} MB");
                Console.WriteLine($"   Free space:\t{drive.TotalFreeSpace / 1000000} MB");
            }

            PrintEnd();
        }

        private static void PrintEnd()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("------------------------------------------------------------------");
            Console.ForegroundColor = ConsoleColor.White;
        }

        private static void PrintBegin(string str)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n---------------------------{str}-----------------------------");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}