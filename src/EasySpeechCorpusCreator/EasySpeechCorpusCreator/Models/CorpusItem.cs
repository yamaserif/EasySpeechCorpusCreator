using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySpeechCorpusCreator.Models
{
    public class CorpusItem
    {
        public int No { get; set; }

        public string Name { get; set; }

        public CorpusItem(int no, string name)
        {
            this.No = no;
            this.Name = name;
        }
    }
}
