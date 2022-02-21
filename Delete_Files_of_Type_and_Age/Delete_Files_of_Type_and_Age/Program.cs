using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DeleteFilesInDir
{
    public class Program
    {
        public static string RootDirectory { get; set; }
        public static int DeleteAgeDays { get; set; }
        public static string FileExtension { get; set; }

        public static void Main(string[] args)
        {
            Console.Write("Root Directory to Parse: ");
            RootDirectory = Console.ReadLine();

            if (!Directory.Exists(RootDirectory))
            {
                Console.WriteLine("The Root Directory provided cannot be found.  App will close.");
                Console.Write("Press any key to continue..");
                Console.Read();
                return;
            }

            Console.Write("Enter File Extension to Delete (or '*' for all files): ");
            FileExtension = Console.ReadLine().Replace(".", "").Trim().ToLower();

            Console.Write("Delete Files older than (days): ");
            int.TryParse(Console.ReadLine(), out int iDeleteAgeDays);

            DeleteAgeDays = Math.Abs(iDeleteAgeDays) * -1;

            if (DeleteAgeDays == 0)
            {
                Console.Write("Delete Files older than 0 days has been selected.  This will delete all files.  Are you sure you wish to continue (Y / N)?");
                if (Console.ReadLine().Contains('N'))
                { 
                    return; 
                }
            }

            ParseDirectory(RootDirectory);

            Console.WriteLine("Operation Complete!");
            Console.WriteLine("Press Any Key to Exit..");
            Console.Read();
        }

        public static void ParseDirectory(string Directory)
        {
            string[] ChildDirs = (new DirectoryInfo(Directory)).GetDirectories().Select(x => x.FullName).ToArray();
            Console.WriteLine($"{ChildDirs.Length} Directories found in {Directory}");
            Console.WriteLine();

            foreach(string ChildDir in ChildDirs)
                ParseDirectory(ChildDir);

            FileInfo[] files = (new DirectoryInfo(Directory)).GetFiles();
            if (FileExtension != "*")
                files = files.Where(x => x.Extension.Replace(".", "").Trim().ToLower() == FileExtension).ToArray();

            Console.WriteLine($"{files.Length} Files found in Directory {Directory} with Extension \"{FileExtension}\"");
            Console.WriteLine();

            foreach (FileInfo file in files)
            {
                if ((file.Extension.Replace(".", "").Trim().ToLower() == FileExtension || FileExtension == "*") && (file.LastAccessTime < (DateTime.Today.AddDays(DeleteAgeDays))))
                {
                    //Delete File
                    Console.WriteLine($"File {file.Name} Deleted in Directory {Directory}");
                    file.Delete();
                }
            }
        }
    } 
}
