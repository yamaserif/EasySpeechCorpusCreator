using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EasySpeechCorpusCreator.Models
{
    public class CorpusItem
    {
        public int No { get; set; }

        public CorpusSentence SentenceData { get; set; }

        public List<string> Tags { get; set; }

        [JsonIgnore]
        public string TagsStr
        {
            get
            {
                return string.Join(", ", this.Tags);
            }
            set
            {
                var setStrData = value.Trim();
                setStrData = Regex.Replace(setStrData, ", *", ",").Trim(',');
                if (setStrData == string.Empty)
                {
                    this.Tags = new List<string>();
                }
                else
                {
                    var setData = setStrData.Split(",");
                    this.Tags = setData.ToList();
                }
            }
        }

        public CorpusItem()
        {
            this.SentenceData = new CorpusSentence();
            this.Tags = new List<string>();
        }

        public CorpusItem(int no = 0, string name = "", string sentence = "", string kana = "", string tags = "")
        {
            this.No = no;
            this.SentenceData = new CorpusSentence(name, sentence, kana);
            this.Tags = new List<string>();
            this.TagsStr = tags;
        }

        public void SetProperty(string propertyName, string value)
        {
            switch (propertyName)
            {
                case nameof(this.No):
                    if (int.TryParse(value, out int setValue))
                    {
                        this.No = setValue;
                    }
                    break;

                case nameof(this.SentenceData.Name):
                    this.SentenceData.Name = value;
                    break;

                case nameof(this.SentenceData.Sentence):
                    this.SentenceData.Sentence = value;
                    break;

                case nameof(this.SentenceData.Kana):
                    this.SentenceData.Kana = value;
                    break;
            }
        }
    }
}
