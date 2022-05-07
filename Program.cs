using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.ActiveDirectory;


namespace Admin_AttackSurface
{
    internal class Program
    {
        static List<KeyValuePair<string, string>> origGroupMembersList = new List<KeyValuePair<string, string>>();
        static void Main()
        {


            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(" _   _ _______        __      _    ____  __  __ ___ _   _ ____  ");
            Console.WriteLine("| \\ | | ____\\ \\      / /     / \\  |  _ \\|  \\/  |_ _| \\ | / ___| ");
            Console.WriteLine("|  \\| |  _|  \\ \\ /\\ / _____ / _ \\ | | | | |\\/| || ||  \\| \\___ \\ ");
            Console.WriteLine("| |\\  | |___  \\ V  V |_____/ ___ \\| |_| | |  | || || |\\  |___) |");
            Console.WriteLine("|_| \\_|_____|  \\_/\\_/     /_/   \\_|____/|_|  |_|___|_| \\_|____/ ");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("                                                 by @ScarredMonk");
            Console.ForegroundColor = ConsoleColor.Gray;


            try
            {
                Domain.GetCurrentDomain().ToString();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message + "\n\nPlease run it inside the domain joined machine \n\n");
                Console.ForegroundColor = ConsoleColor.Gray;
                return;
            }

            //Saving all the members into the list
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n[+] Existing Local Administrators on Domain Controller:\n");
            Console.ForegroundColor = ConsoleColor.Gray;
            SaveGroupMembers("Administrators");

            //Checking for new machine account addition into the security groups
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n[+] Monitoring the change in local admins on Domain Controller \n");
            Console.ForegroundColor = ConsoleColor.Gray;
            while (true)
            {
                GetGroupMembers("Administrators");
                Thread.Sleep(5000);
            }
        }

        static void SaveGroupMembers(string groupname)
        {
            PrincipalContext context = new PrincipalContext(ContextType.Domain, Domain.GetCurrentDomain().ToString());
            GroupPrincipal group = GroupPrincipal.FindByIdentity(context, IdentityType.Name, groupname);
            if (group != null)
            {

                foreach (Principal p in group.GetMembers(true))
                {
                    origGroupMembersList.Add(new KeyValuePair<string, string>(group.Name, p.Name));
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[Old] " + p.Name);
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
                group.Dispose();
            }
        }
        static void GetGroupMembers(string groupname)
        {
            PrincipalContext ctx = new PrincipalContext(ContextType.Domain, Domain.GetCurrentDomain().ToString());
            GroupPrincipal group = GroupPrincipal.FindByIdentity(ctx, IdentityType.Name, groupname);

            if (group != null)
            {
                foreach (Principal p in group.GetMembers(true))
                {
                    var compareList = origGroupMembersList.Where(x => x.Key == group.Name && x.Value == p.Name);
                    if (!compareList.Any())
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("[New] - " + p.Name );
                        Console.ForegroundColor = ConsoleColor.Gray;

                        origGroupMembersList.Add(new KeyValuePair<string, string>(group.Name, p.Name));
                    }
                }
                group.Dispose();
            }
        }
    }
}
