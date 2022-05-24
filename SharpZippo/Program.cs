using System;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;

/* References
https://stackoverflow.com/questions/1640223/how-to-validate-that-a-file-is-a-password-protected-zip-file-using-c-sharp
https://icsharpcode.github.io/SharpZipLib/help/api/ICSharpCode.SharpZipLib.Zip.ZipFile.html
*/

namespace SharpZippo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2 || args.Length > 4)
            {
                Console.WriteLine(@"
 _____ _                       _______                   
/  ___| |                     |___  (_)                  
\ `--.| |__   __ _ _ __ _ __     / / _ _ __  _ __   ___  
 `--. \ '_ \ / _` | '__| '_ \   / / | | '_ \| '_ \ / _ \ 
/\__/ / | | | (_| | |  | |_) |./ /__| | |_) | |_) | (_) |
\____/|_| |_|\__,_|_|  | .__/ \_____/_| .__/| .__/ \___/ 
                       | |            | |   | |          
                       |_|            |_|   |_|          
                                
" +
"" +
"Developed By: @sadpanda_sec \n\n" +
"Description: Check/List/Read Contents of Zip Archives.\n\n" +
"Args Description: \n" +
"Check Argument:    Checks encryption status of embedded files\n" +
"List Argument:     Lists contents of Zip file\n" +
"Read Argument:     Read the contents within the Zip file (with or without a password)\n\n" +
"Usage:\n" +
"SharpZippo.exe check C:\\Some\\Path\\To\\file.zip\n" +
"SharpZippo.exe list C:\\Some\\Path\\To\\file.zip\n" +
"SharpZippo.exe read C:\\Some\\Path\\To\\file.zip Full/EmbeddedPath/To/File.ext\n" +
"SharpZippo.exe read P@ssW0rd C:\\Some\\Path\\To\\file.zip Full/EmbeddedPath/To/File.ext");
                System.Environment.Exit(0);
            }
            else if (args.Length == 2 && args[0].ToLower() == "check")
            {
                if (File.Exists(args[1]) && Path.GetExtension(args[1]).ToString().ToLower() == ".zip")
                {
                    try
                    {
                        using (ZipFile zFile = new ZipFile(args[1]))
                        {
                            foreach (ZipEntry e in zFile)
                            {
                                if (e.IsFile)
                                {
                                    Console.WriteLine("Path: " + e);
                                    Console.WriteLine("Is Encrypted: " + e.IsCrypted);
                                }
                            }
                        }
                        System.Environment.Exit(0);
                    }
                    catch (ZipException ex)
                    {
                        Console.WriteLine(ex.Message);
                        System.Environment.Exit(0);
                    }
                }
                
            }
            else if (args.Length == 2 && args[0].ToLower() == "list")
            {
                if (File.Exists(args[1]) && Path.GetExtension(args[1]).ToString().ToLower() == ".zip")
                {
                    try
                    {
                        using (ZipFile zFile = new ZipFile(args[1]))
                        {
                            Console.WriteLine("Listing of : " + zFile.Name);
                            Console.WriteLine("");
                            Console.WriteLine("Raw Size    Size      Date     Time     Name");
                            Console.WriteLine("--------  --------  --------  ------  ---------");
                            foreach (ZipEntry e in zFile)
                            {
                                if (e.IsFile)
                                {
                                    DateTime d = e.DateTime;
                                    Console.WriteLine("{0, -10}{1, -10}{2}  {3}   {4}", e.Size, e.CompressedSize,
                                        d.ToString("dd-MM-yy"), d.ToString("HH:mm"),
                                        e.Name);
                                }
                            }
                        }
                        System.Environment.Exit(0);
                    }
                    catch (ZipException ex)
                    {
                        Console.WriteLine(ex.Message);
                        System.Environment.Exit(0);
                    }
                }

                else
                {
                    Console.WriteLine("File Does Not Exist: Exiting...");
                    System.Environment.Exit(0);
                }
            }
            //read files without password
            else if (args.Length == 3 && args[0].ToLower() == "read")
            {
                if (File.Exists(args[1]) && Path.GetExtension(args[1]).ToLower() == ".zip")
                {
                    try
                    {
                        using (ZipFile zipFile = new ZipFile(args[1]))
                        {
                            var fileEntry = zipFile.GetEntry(args[2]);
                            using (var filestream = zipFile.GetInputStream(fileEntry))
                            using (var reader = new StreamReader(filestream))
                            {
                                Console.WriteLine(reader.ReadToEnd());
                            }

                        }
                        System.Environment.Exit(0);
                    }
                    catch (ZipException ex)
                    {
                        Console.WriteLine(ex.Message);
                        System.Environment.Exit(0);
                    }
                }
                else
                {
                    Console.WriteLine("File Does Not Exist: Exiting...");
                    System.Environment.Exit(0);
                }
            }
            // Read files with password
            else if (args.Length == 4 && args[0].ToLower() == "read")
            {
                if (File.Exists(args[2]) && Path.GetExtension(args[2]).ToLower() == ".zip")
                {
                    try
                    {
                        using (ZipFile zipFile = new ZipFile(args[2]))
                        {
                            var pass = args[1].ToString();
                            zipFile.Password = pass;
                            var fileEntry = zipFile.GetEntry(args[3]);
                            using (var filestream = zipFile.GetInputStream(fileEntry))
                            using (var reader = new StreamReader(filestream))
                            {
                                Console.WriteLine(reader.ReadToEnd());
                            }

                        }
                        System.Environment.Exit(0);
                    }
                    catch (ZipException ex)
                    {
                        Console.WriteLine(ex.Message);
                        System.Environment.Exit(0);
                    }
                }
                else
                {
                    Console.WriteLine("File Does Not Exist: Exiting...");
                    System.Environment.Exit(0);
                }
            }
            else
            {
                Console.WriteLine("Wrong arguments passed: Exiting...");
                System.Environment.Exit(0);
            }
        }
    }
}
