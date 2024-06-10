using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive.Disposables;
using System.Windows;
using EasySpeechCorpusCreator.Business;
using EasySpeechCorpusCreator.Consts;
using EasySpeechCorpusCreator.Models;
using Prism.Mvvm;
using Prism.Navigation;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using UtfUnknown;

namespace EasySpeechCorpusCreator.ViewModels
{
    public class ManagerViewModel : ViewModelBase
    {
        public ReactiveProperty<string> Project { get; }
        public ReactiveProperty<CorpusItem?> SelectItem { get; }
        public ReactiveCollection<CorpusItem> CorpusItems { get; }

        // Header名
        public ReactiveProperty<CorpusHeader> CorpusItemHeader { get; }

        public ManagerViewModel()
        {
            this.Project = new ReactiveProperty<string>(this.CurrentData.Project).AddTo(this.Disposable);
            this.SelectItem = new ReactiveProperty<CorpusItem?>().AddTo(this.Disposable);
            this.CorpusItemHeader = new ReactiveProperty<CorpusHeader>(new CorpusHeader()).AddTo(this.Disposable);
            this.CorpusItems = new ReactiveCollection<CorpusItem>().AddTo(this.Disposable);

            var currentProjectDirectoryParh = System.IO.Path.Combine(this.Settings.ProjectPass, this.CurrentData.Project);
            var currentProjectFileParh = System.IO.Path.Combine(currentProjectDirectoryParh, CorpusConst.CORPUS_FILE_NAME_JSON);
            if (File.Exists(currentProjectFileParh))
            {
                var charset = CharsetDetector.DetectFromFile(currentProjectFileParh);
                var currentProjectStr = File.ReadAllText(currentProjectFileParh, charset.Detected.Encoding);
                var corpusItems = JsonUtil.ToCorpusItems(currentProjectStr);
                while (0 < (corpusItems?.Count ?? 0))
                {
                    this.CorpusItems.Add(corpusItems?.Dequeue() ?? new CorpusItem(0));
                }
            }

            //SelectItem.Subscribe(x => this.Project.Value = x?.No.ToString() ?? "");
        }
    }
}
