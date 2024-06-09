using EasySpeechCorpusCreator.Consts;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySpeechCorpusCreator.Models
{
    public class CorpusHeader
    {
        public string No { get; set; }

        public string Name { get; set; }

        public string Sentence { get; set; }

        public string Kana { get; set; }

        public CorpusHeader(
            string no = CorpusConst.NO,
            string name = CorpusConst.NAME,
            string sentence = CorpusConst.SENTENCE,
            string kana = CorpusConst.KANA)
        {
            this.No = no;
            this.Name = name;
            this.Sentence = sentence;
            this.Kana = kana;
        }
    }
}
