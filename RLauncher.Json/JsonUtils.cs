using RLauncher.Json.Internal;

using System;
using System.Text.Json;

namespace RLauncher.Json
{
    public static class JsonUtils
    {
        public static ILauncherData? ToLauncherData(string jsonText)
        {
            if (jsonText is null)
                throw new ArgumentNullException(nameof(jsonText));
            return JsonSerializer.Deserialize<LauncherJson>(jsonText);
        }
    }
}
