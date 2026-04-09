using System;
using System.IO;
using System.Collections.Generic;

public class CPHInline
{
    public bool Execute()
    {
        CPH.TryGetArg("rawInput", out string input);
        CPH.TryGetArg("command", out string commandName);

        input = input?.Trim().TrimStart('@');

        if (string.IsNullOrEmpty(input))
        {
            CPH.SendMessage($"Usage: {commandName} <username>");
            return false;
        }

        string logDir = CPH.GetGlobalVar<string>("creditsDir", true);
        if (string.IsNullOrEmpty(logDir))
        {
            CPH.SendMessage("Credits directory is not set.");
            return false;
        }

        string blocklistPath = Path.Combine(logDir, "credits_blocklist.txt");

        List<string> blocklist = new List<string>();
        if (File.Exists(blocklistPath))
            blocklist = new List<string>(File.ReadAllLines(blocklistPath));

        if (blocklist.Exists(u => u.Equals(input, StringComparison.OrdinalIgnoreCase)))
        {
            CPH.SendMessage($"{input} is already on the credits blocklist.");
            return false;
        }

        try
        {
            blocklist.Add(input);
            File.WriteAllLines(blocklistPath, blocklist);
            CPH.SendMessage($"{input} added to the credits blocklist.");
            return true;
        }
        catch (Exception ex)
        {
            CPH.SendMessage($"Failed to update blocklist: {ex.Message}");
            return false;
        }
    }
}