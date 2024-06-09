using EasySpeechCorpusCreator.Consts;
using EasySpeechCorpusCreator.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace EasySpeechCorpusCreator.Business
{
    public static class CorpusImportUtil
    {
        public static CorpusItem ToCorpusItem(int no, string str, string importVariableChar, string importFormat)
        {
            var returnCorpus = new CorpusItem(no);

            var properties = new List<string>();
            properties.Add(importVariableChar + nameof(CorpusItem.Name) + importVariableChar);
            properties.Add(importVariableChar + nameof(CorpusItem.Sentence) + importVariableChar);
            properties.Add(importVariableChar + nameof(CorpusItem.Kana) + importVariableChar);

            properties.RemoveAll(property => importFormat.IndexOf(property) < 0);

            properties.Sort((property1, property2) => {
                return importFormat.IndexOf(property1) - importFormat.IndexOf(property2);
            });

            var currentStr = str;

            properties.ForEach(property =>
            {
                var splitFormat = importFormat.Split(property);
                var strSplit = string.Empty;

                // プロパティの前側切り出し
                splitFormat[0] = splitFormat[0].Substring(splitFormat[0].LastIndexOf(importVariableChar) + 1);
                strSplit = currentStr.Split(splitFormat[0]).Last();

                // プロパティの後ろ側切り出し
                var indexOfResult = splitFormat[1].IndexOf(importVariableChar);
                if (indexOfResult < 0)
                {
                    indexOfResult = splitFormat[1].Length;
                }
                splitFormat[1] = splitFormat[1].Substring(0, indexOfResult);
                var strSplitList = new LinkedList<string>(strSplit.Split(splitFormat[1]));
                strSplit = strSplitList.First();
                strSplitList.RemoveFirst();

                currentStr = string.Join(splitFormat[1], strSplitList);

                var propertyName = property.Substring(importVariableChar.Length, property.Length - (importVariableChar.Length * 2));
                returnCorpus.SetProperty(propertyName, strSplit);
            });

            return returnCorpus;
        }
    }
}
