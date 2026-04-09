using System;
using System.IO;

public class CPHInline
{
    public bool Execute()
    {
        CPH.TryGetArg("rawInput", out string input);
        CPH.TryGetArg("command", out string commandName);

        input = input?.Trim();

        if (string.IsNullOrEmpty(input))
        {
            CPH.SendMessage($"Usage: {commandName} <path>");
            return false;
        }

        try
        {
            Directory.CreateDirectory(input);

            string blocklistPath = Path.Combine(input, "credits_blocklist.txt");
            if (!File.Exists(blocklistPath))
                File.WriteAllText(blocklistPath, string.Empty);

            CPH.SetGlobalVar("creditsDir", input, true);
            CPH.SendMessage($"Credits directory set to: {input}");
            return true;
        }
        catch (Exception ex)
        {
            CPH.SendMessage($"Invalid path: {ex.Message}");
            return false;
        }
    }
}