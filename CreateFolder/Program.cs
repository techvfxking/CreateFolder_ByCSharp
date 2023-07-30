using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;

namespace CreateFolder
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string basePath = @"E:\TVK Videos";
            DirectoryInfo dir = new DirectoryInfo(basePath);
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                CreateFolders(true,string.Format("{0}\\{1}",basePath, file.Name.Replace(".mp4","")));
            }
            Console.WriteLine("Done");
            Console.ReadLine();
        }
        private static void CreateFolders(bool allUsers, string ConfigurationPath)
        {
            try
            {
                DirectoryInfo directoryInfo;
                DirectorySecurity directorySecurity;
                AccessRule rule;
                //SecurityIdentifier securityIdentifier = new SecurityIdentifier
                //    (WellKnownSidType.BuiltinUsersSid, null);
                SecurityIdentifier securityIdentifier = new SecurityIdentifier
                (WellKnownSidType.WorldSid, null);
                if (!Directory.Exists(ConfigurationPath))
                {
                    if (!allUsers)
                    {
                        directoryInfo = Directory.CreateDirectory(ConfigurationPath);
                        bool modified;
                        directorySecurity = directoryInfo.GetAccessControl();
                        rule = new FileSystemAccessRule(
                                securityIdentifier,
                                FileSystemRights.Write |
                                FileSystemRights.ReadAndExecute |
                                FileSystemRights.FullControl,
                                AccessControlType.Allow);
                        directorySecurity.ModifyAccessRule(AccessControlModification.Add, rule, out modified);
                        directoryInfo.SetAccessControl(directorySecurity);
                    }
                    if (allUsers)
                    {
                        bool modified;
                        directoryInfo = Directory.CreateDirectory(ConfigurationPath);
                        directorySecurity = directoryInfo.GetAccessControl();
                        rule = new FileSystemAccessRule(
                            securityIdentifier,
                            FileSystemRights.Write |
                            FileSystemRights.ReadAndExecute |
                            FileSystemRights.FullControl,
                            InheritanceFlags.ContainerInherit |
                            InheritanceFlags.ObjectInherit,
                            PropagationFlags.None,
                            AccessControlType.Allow);
                        directorySecurity.ModifyAccessRule(AccessControlModification.Add, rule, out modified);
                        directoryInfo.SetAccessControl(directorySecurity);
                    }
                }              
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.ReadLine();
            }
        }
    }
}
