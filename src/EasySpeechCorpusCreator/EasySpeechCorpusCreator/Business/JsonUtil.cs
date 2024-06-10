using EasySpeechCorpusCreator.Models;
using System;
using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace EasySpeechCorpusCreator.Business
{
    public static class JsonUtil
    {
        public static string ToJson(CorpusItem obj)
        {
            var json = JsonSerializer.Serialize(obj, JsonUtil.GetOption());
            return json;
        }

        public static string ToJson(Settings obj)
        {
            var json = JsonSerializer.Serialize(obj, JsonUtil.GetOption());
            return json;
        }

        public static Queue<CorpusItem>? ToCorpusItems(string json)
        {
            try
            {
                var obj = JsonSerializer.Deserialize<Queue<CorpusItem>>(json, JsonUtil.GetOption());
                return obj;
            }
            catch (Exception)
            {
                // 読み込み失敗時は異常値としてnullを返す。
                return null;
            }
        }

        public static Settings? ToSettings(string json)
        {
            try
            {
                var obj = JsonSerializer.Deserialize<Settings>(json, JsonUtil.GetOption());
                return obj;
            }
            catch (Exception)
            {
                // 読み込み失敗時は異常値としてnullを返す。
                return null;
            }
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
