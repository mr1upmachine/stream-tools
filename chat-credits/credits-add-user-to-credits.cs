using System;
using System.IO;
using System.Collections.Generic;

public class CPHInline
{
    public bool Execute()
    {
        string path = CPH.GetGlobalVar<string>("creditsFile", true);
        if (string.IsNullOrEmpty(path) || !File.Exists(path))
        {
            CPH.LogWarn("Chatter log file not found.");
            return false;
        }

        string logDir = CPH.GetGlobalVar<string>("creditsDir", true);
        if (string.IsNullOrEmpty(logDir))
        {
            CPH.LogWarn("Credits directory is not set.");
            return false;
        }

        CPH.TryGetArg("user", out string user);

        string blocklistPath = Path.Combine(logDir, "credits_blocklist.txt");

        List<string> blocklist = new List<string>();
        if (File.Exists(blocklistPath))
            blocklist = new List<string>(File.ReadAllLines(blocklistPath));

        if (blocklist.Exists(u => u.Equals(user, StringComparison.OrdinalIgnoreCase))) return true;

        try
        {
            File.AppendAllText(path, user + Environment.NewLine);
            return true;
        }
        catch (Exception ex)
        {
            CPH.LogWarn($"Failed to write chatter log: {ex.Message}");
            return false;
        }
    }
}