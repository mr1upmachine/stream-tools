using System;
using System.IO;
using System.Collections.Generic;

/*
 * Trigger for this should be Twitch > Chat > First Words
 */

public class CPHInline
{
    public bool Execute()
    {
        // Get reference to the credits log file that should have been created when the stream began
        string path = CPH.GetGlobalVar<string>("creditsLogFile", true);
        if (string.IsNullOrEmpty(path) || !File.Exists(path))
        {
            CPH.LogWarn("Credits log file not found.");
            return false;
        }

        string username = args["userName"].ToString();

        // Skip broadcaster
        var broadcaster = CPH.TwitchGetBroadcaster();
        if (username.Equals(broadcaster.UserName, StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        // Skip anyone in the blocklist file
        string blocklistPath = CPH.GetGlobalVar<string>("creditsBlocklistPath", true);
        List<string> blocklist = new List<string>();
        if (File.Exists(blocklistPath))
        {
            blocklist = new List<string>(File.ReadAllLines(blocklistPath));
        }
        if (blocklist.Exists(u => u.Equals(username, StringComparison.OrdinalIgnoreCase)))
        {
            return true;
        }

        // If the chatter is not in the file already, then add them
        List<string> chatters = new List<string>(File.ReadAllLines(path));
        if (!chatters.Exists(u => u.Equals(username, StringComparison.OrdinalIgnoreCase)))
        {
            chatters.Add(username);
            File.WriteAllLines(path, chatters);
        }

        return true;
    }
}