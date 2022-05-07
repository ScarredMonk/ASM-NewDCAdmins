# ASM-NewDCAdmins
A C# tool to gather the administrators on the domain controller (including local accounts) and detects when it changes. 

Whenever a new domain admin is added, it is notified on the console of this tool
![image](https://user-images.githubusercontent.com/46210620/167267856-0060c450-0a31-4a59-a277-c9af7a5925f2.png)

The same thing can be monitored from Windows Event Logs with event ID 4728 and check for any attempts of addding users into a Administrators groups of a domain controller.

Blogpost Link - [https://rootdse.org/posts/monitoring-realtime-activedirectory-domain-scenarios](https://rootdse.org/posts/monitoring-realtime-activedirectory-domain-scenarios)

