using EasySpeechCorpusCreator.Models;
using System.Collections.Generic;
using System;
using System.Text.Json;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace EasySpeechCorpusCreator.Business
{
    public static class CsvUtil
    {
        public static string ToCsvHeader(CorpusItem obj)
        {
            var strBuilder = new StringBuilder();

            // ヘッダー
            strBuilder.Append(nameof(obj.No)).Append(",")
                      .Append(nameof(obj.SentenceData.Name)).Append(",")
                      .Append(nameof(obj.SentenceData.Sentence)).Append(",")
                      .Append(nameof(obj.SentenceData.Kana)).Append(",")
                      .Append(nameof(obj.VoiceFileName)).Append(",")
                      .Append(nameof(obj.Tags)).Append("\r\n");

            return strBuilder.ToString();
        }

        public static string ToCsvItem(CorpusItem obj)
        {
            var no = obj.No;
            var name = obj.SentenceData.Name.Replace("\"","\"\"").Replace(",", "\\u002C");
            var sentence = obj.SentenceData.Sentence.Replace("\"", "\"\"").Replace(",", "\\u002C");
            var kana = obj.SentenceData.Kana.Replace("\"", "\"\"").Replace(",", "\\u002C");
            var voiceFileName = obj.VoiceFileName.Replace("\"", "\"\"").Replace(",", "\\u002C");
            var tags = obj.TagsStr.Replace("\"", "\"\"").Replace(",", "\\u002C");

            var strBuilder = new StringBuilder();

            // カラム
            strBuilder.Append("\"").Append(no).Append("\",")
                      .Append("\"").Append(name).Append("\",")
                      .Append("\"").Append(sentence).Append("\",")
                      .Append("\"").Append(kana).Append("\",")
                      .Append("\"").Append(voiceFileName).Append("\",")
                      .Append("\"").Append(tags).Append("\"\r\n");

            return strBuilder.ToString();
        }

        public static Queue<CorpusItem>? ToCorpusItems(string csv)
        {
            try
            {
                var headerIndex = new Dictionary<string, int>();
                var csvLines = csv.Replace("\r\n", "\n").Split(new[] {'\n', '\r'});

                // ヘッダー
                var csvheaders = csvLines[0].Split(',');
                for(var i = 0; i < csvheaders.Length; i++)
                {
                    headerIndex.Add(csvheaders[i], i);
                }

                var returnItems = new Queue<CorpusItem>();

                // カラム
                for (var i = 1; i < csvLines.Length; i++)
                {
                    var csvItems = csvLines[i].Split(',');
                    for (var j = 0; j < csvItems.Length; j++)
                    {
                        var item = csvItems[j];
                        if (1 < item.Length && item[0] == '"' && item[item.Length - 1] == '"')
                        {
                            csvItems[j] = item.Substring(1, item.Length - 2);
                        }
                    }

                    var setItem = new CorpusItem();
                    foreach (var header in headerIndex)
                    {
                        var key = header.Key;
                        if (key == nameof(setItem.Tags))
                        {
                            key = nameof(setItem.TagsStr);
                        }

                        if (csvItems.Length <= header.Value)
                        {
                            continue;
                        }

                        var itemProperty = typeof(CorpusItem).GetProperty(key);

                        if (itemProperty == null)
                        {
                            itemProperty = typeof(CorpusSentence).GetProperty(key);
                            itemProperty?.SetValue(setItem.SentenceData, csvItems[header.Value].Replace("\"\"", "\"").Replace("\\u002C", ","));
                        }
                        else
                        {
                            if (key == nameof(setItem.No))
                            {
                                var setNoStr = csvItems[header.Value];
                                if (int.TryParse(setNoStr, out var setNo))
                                {
                                    itemProperty?.SetValue(setItem, setNo);
                                }
                            }
                            else
                            {
                                itemProperty?.SetValue(setItem, csvItems[header.Value].Replace("\"\"", "\"").Replace("\\u002C", ","));
                            }
                        }
                    }

                    returnItems.Enqueue(setItem);
                }

                return returnItems;
            }
            catch (Exception)
            {
                // 読み込み失敗時は異常値としてnullを返す。
                return null;
            }
        }
    }
}
