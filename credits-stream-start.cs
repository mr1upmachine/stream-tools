using System;
using System.IO;

/*
 * Trigger for this should be Twitch > Channel > Stream Online
 */

public class CPHInline
{
    public bool Execute()
    {
        // Get the directory that the file should be in
        string logDir = CPH.GetGlobalVar<string>("creditsLogDir", true);

        // Create the file using the stream start timestamp
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        string path = Path.Combine(logDir, $"credits_{timestamp}.txt");
        Directory.CreateDirectory(logDir);
        File.WriteAllText(path, string.Empty);

        // Write the credits file path to a variable to use during this stream session
        CPH.SetGlobalVar("creditsLogFile", path, true);
        CPH.LogInfo($"Credits log initialized: {path}");

        return true;
    }
}