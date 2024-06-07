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

        public CorpusHeader(
            string no = CorpusItemsHeaderConst.NO,
            string name = CorpusItemsHeaderConst.NAME)
        {
            this.No = no;
            this.Name = name;
        }
    }
}
