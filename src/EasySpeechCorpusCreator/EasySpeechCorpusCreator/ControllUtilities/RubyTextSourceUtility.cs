using EasySpeechCorpusCreator.Business;
using EasySpeechCorpusCreator.Consts;
using EasySpeechCorpusCreator.Models;
using System;
using System.IO;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using static System.Net.Mime.MediaTypeNames;

namespace EasySpeechCorpusCreator.ControllUtilities
{
    public static class RubyTextSourceUtility
    {
        public static readonly DependencyProperty BindableSourceProperty =
            DependencyProperty.RegisterAttached("BindableSource", typeof(string), typeof(RubyTextSourceUtility), new UIPropertyMetadata(null, BindableSourcePropertyChanged));

        public static string GetBindableSource(DependencyObject obj)
        {
            return (string)obj.GetValue(BindableSourceProperty);
        }

        public static void SetBindableSource(DependencyObject obj, string value)
        {
            obj.SetValue(BindableSourceProperty, value);
        }

        public static void BindableSourcePropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var browser = o as WebBrowser;
            var rubyText = e.NewValue as string ?? string.Empty;
            if (browser != null)
            {
                var rubyPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "rubyText.html");

                RubyTextSourceUtility.CreateRubyFile(rubyPath, rubyText);
                browser.Source = new Uri("file:\\\\\\" + rubyPath);
            }
        }

        private static void CreateRubyFile(string rubyPath, string rubyText)
        {
            if (File.Exists(rubyPath))
            {
                File.Delete(rubyPath);
            }

            using (var writer = new StreamWriter(rubyPath, true, Encoding.UTF8))
            {
                writer.Write("<html><head>");
                writer.Write("<style>html *{margin: 0px; font-weight: bold; color: #0000A0; font-family: 'Helvetica Neue', Arial, 'Hiragino Kaku Gothic ProN', 'Hiragino Sans', Meiryo, sans-serif;}</style>");
                writer.Write("<style>ruby *{font-weight: lighter; color: #0000A0;}</style>");
                writer.Write("</head><body><p>");

                while (true)
                {
                    var hiraganaMatch = Regex.Match(rubyText, "(\\p{IsHiragana}|ゝ|ゞ)+\\(.*?\\)").Value;
                    var katakanaMatch = Regex.Match(rubyText, "(\\p{IsKatakana}|ヽ|ヾ)+\\(.*?\\)").Value;
                    var kanjiMatch = Regex.Match(rubyText, "(\\p{IsCJKUnifiedIdeographs}|々|ヶ|ヵ)+\\(.*?\\)").Value;
                    var hankakuMatch = Regex.Match(rubyText, "(\\p{Ll}|\\p{Lu})+\\(.*?\\)").Value;

                    if (hiraganaMatch == string.Empty &&
                        katakanaMatch == string.Empty &&
                        kanjiMatch == string.Empty &&
                        hankakuMatch == string.Empty)
                    {
                        break;
                    }
                    else
                    {
                        if (hiraganaMatch != string.Empty)
                        {
                            rubyText = RubyTextSourceUtility.ReplaceRubyText(rubyText, hiraganaMatch);
                        }
                        if (katakanaMatch != string.Empty)
                        {
                            rubyText = RubyTextSourceUtility.ReplaceRubyText(rubyText, katakanaMatch);
                        }
                        if (kanjiMatch != string.Empty)
                        {
                            rubyText = RubyTextSourceUtility.ReplaceRubyText(rubyText, kanjiMatch);
                        }
                        if (hankakuMatch != string.Empty)
                        {
                            rubyText = RubyTextSourceUtility.ReplaceRubyText(rubyText, hankakuMatch);
                        }
                    }
                }

                writer.Write(rubyText);

                writer.Write("</p></body></html>");
            }
        }

        private static string ReplaceRubyText(string rubyText, string matchStr)
        {
            var replaceStr = "<ruby>";

            replaceStr += matchStr.Replace("(", "<rp>[</rp><rt>")
                                 .Replace(")", "</rt><rp>]</rp>");

            replaceStr += "</ruby>";
            return rubyText.Replace(matchStr, replaceStr);
        }
    }
}
