using EasySpeechCorpusCreator.Models;
using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace EasySpeechCorpusCreator.Business
{
    public static class JsonUtil
    {
        public static string ToJson(Settings obj)
        {
            var json = JsonSerializer.Serialize(obj, JsonUtil.GetOption());
            return json;
        }

        public static Settings? ToSettings(string json)
        {
            var obj = JsonSerializer.Deserialize<Settings>(json, JsonUtil.GetOption());
            return obj;
        }

        private static JsonSerializerOptions GetOption()
        {
            var options = new JsonSerializerOptions()
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                WriteIndented = true,
            };
            return options;
        }
    }
}
