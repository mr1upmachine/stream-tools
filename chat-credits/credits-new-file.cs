using System;
using System.IO;

public class CPHInline
{
    public bool Execute()
    {
        string logDir = CPH.GetGlobalVar<string>("creditsDir", true);
        if (string.IsNullOrEmpty(logDir))
        {
            CPH.LogWarn("Credits directory is not set.");
            return false;
        }

        string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        string path = Path.Combine(logDir, $"credits_{timestamp}.txt");

        try
        {
            Directory.CreateDirectory(logDir);
            File.WriteAllText(path, string.Empty);

            CPH.SetGlobalVar("creditsFile", path, true);
            CPH.LogInfo($"Chatter log initialized: {path}");
            return true;
        }
        catch (Exception ex)
        {
            CPH.LogWarn($"Failed to create chatter log: {ex.Message}");
            return false;
        }
    }
}