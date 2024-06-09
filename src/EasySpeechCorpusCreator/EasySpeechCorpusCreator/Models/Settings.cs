using EasySpeechCorpusCreator.Consts;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EasySpeechCorpusCreator.Consts.SettingsConst;

namespace EasySpeechCorpusCreator.Models
{
    public class Settings
    {
        public string ProjectPass { get; set; }

        public Format Format { get; set; }

        public Settings(
            string projectPass = SettingsConst.DEFAULT_PROJECT_PASS,
            Format format = SettingsConst.DEFAULT_FORMAT)
        {
            this.ProjectPass = System.IO.Path.GetFullPath(projectPass);
            this.Format = format;
        }
    }
}
