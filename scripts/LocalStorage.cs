using Godot;
using System;


public static class LocalStorage
{
    private string directoryPath = "C:\Users\adas\Documents\spits_progress"
    private static bool IsWebPlatform()
    {
        return OS.HasFeature("JavaScript");
    }

    public static void SaveProgress(string key, string value)
    {
        if (IsWebPlatform())
        {
            string jsCode = $"localStorage.setItem('{key}', '{value}');";
            JavaScript.Eval(jsCode);
        }
        else
        {
            // Save to file for testing in the editor
            var file = new File();
            file.Open($"{directoryPath}\{key}.txt", File.ModeFlags.Write);
            file.StoreString(value);
            file.Close();
        }
    }

    public static string LoadProgress(string key)
    {
        if (IsWebPlatform())
        {
            string jsCode = $"localStorage.getItem('{key}');";
            return JavaScript.Eval(jsCode).ToString();
        }
        else
        {
            // Load from file for testing in the editor
            var file = new File();
            if (file.FileExists($"{directoryPath}\{key}.txt")
            {
                file.Open($"{directoryPath}\{key}.txt", File.ModeFlags.Read);
                string value = file.GetAsText();
                file.Close();
                return value;
            }
            return "";
        }
    }
}
