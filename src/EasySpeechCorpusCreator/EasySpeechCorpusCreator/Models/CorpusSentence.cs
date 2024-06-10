using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySpeechCorpusCreator.Models
{
    public class CorpusSentence
    {
        public string Name { get; set; }
        public string Sentence { get; set; }
        public string Kana { get; set; }

        public CorpusSentence(string name = "", string sentence = "", string kana = "")
        {
            this.Name = name;
            this.Sentence = sentence;
            this.Kana = kana;
        }

        public void SetProperty(string propertyName, string value)
        {
            switch (propertyName)
            {
                case nameof(this.Name):
                    this.Name = value;
                    break;

                case nameof(this.Sentence):
                    this.Sentence = value;
                    break;

                case nameof(this.Kana):
                    this.Kana = value;
                    break;
            }
        }
    }
}
