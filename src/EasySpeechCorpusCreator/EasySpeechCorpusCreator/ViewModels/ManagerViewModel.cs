using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using EasySpeechCorpusCreator.Business;
using EasySpeechCorpusCreator.Consts;
using EasySpeechCorpusCreator.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using UtfUnknown;

namespace EasySpeechCorpusCreator.ViewModels
{
    public class ManagerViewModel : ViewModelBase
    {
        // ラベル
        public ReactiveProperty<string> LabelProject { get; set; }
        public ReactiveProperty<string> LabelNo { get; set; }
        public ReactiveProperty<string> LabelName { get; set; }
        public ReactiveProperty<string> LabelSentence { get; set; }
        public ReactiveProperty<string> LabelKana { get; set; }
        public ReactiveProperty<string> LabelTags { get; set; }

        // 中身
        public ReactiveProperty<string> Project { get; }
        public ReactiveProperty<CorpusItem?> SelectItem { get; }
        public ReactiveCollection<CorpusItem> CorpusItems { get; }
        public ReactiveProperty<string> No { get; set; }
        public ReactiveProperty<string> Name { get; set; }
        public ReactiveProperty<string> Sentence { get; set; }
        public ReactiveProperty<string> Kana { get; set; }
        public ReactiveProperty<string> Tags { get; set; }

        // 要素設定値
        public ReactiveCollection<string> ProjectList { get; set; }
        public ReactiveProperty<string> SaveCorpusItemText { get; set; }

        // Header名
        public ReactiveProperty<CorpusHeader> CorpusItemHeader { get; }

        // ボタン 処理
        public DelegateCommand SaveCorpusItemCommand { get; set; }

        public ManagerViewModel()
        {
            this.LabelProject = new ReactiveProperty<string>(ProjectConst.LABEL_PROJECT).AddTo(this.Disposable);
            this.LabelNo = new ReactiveProperty<string>(CorpusConst.NO + "：").AddTo(this.Disposable);
            this.LabelName = new ReactiveProperty<string>(CorpusConst.NAME + "：").AddTo(this.Disposable);
            this.LabelSentence = new ReactiveProperty<string>(CorpusConst.SENTENCE + "：").AddTo(this.Disposable);
            this.LabelKana = new ReactiveProperty<string>(CorpusConst.KANA + "：").AddTo(this.Disposable);
            this.LabelTags = new ReactiveProperty<string>(CorpusConst.TAGS + "：").AddTo(this.Disposable);

            this.Project = new ReactiveProperty<string>(this.CurrentData.Project).AddTo(this.Disposable);
            this.SelectItem = new ReactiveProperty<CorpusItem?>().AddTo(this.Disposable);
            this.CorpusItemHeader = new ReactiveProperty<CorpusHeader>(new CorpusHeader()).AddTo(this.Disposable);
            this.CorpusItems = new ReactiveCollection<CorpusItem>().AddTo(this.Disposable);
            this.No = new ReactiveProperty<string>().AddTo(this.Disposable);
            this.Name = new ReactiveProperty<string>().AddTo(this.Disposable);
            this.Sentence = new ReactiveProperty<string>().AddTo(this.Disposable);
            this.Kana = new ReactiveProperty<string>().AddTo(this.Disposable);
            this.Tags = new ReactiveProperty<string>().AddTo(this.Disposable);

            this.ProjectList = new ReactiveCollection<string>().AddTo(this.Disposable);
            this.Projects.ForEach(project => this.ProjectList.Add(project));
            this.SaveCorpusItemText = new ReactiveProperty<string>(SystemConst.SAVE).AddTo(this.Disposable);

            this.SaveCorpusItemCommand = new DelegateCommand(this.SaveCorpusItem);

            this.Project.Subscribe(this.SetProject);
            this.SelectItem.Subscribe(this.ChangeCorpus);
        }

        private void SaveCorpusItem()
        {
            var selectItem = this.SelectItem.Value;

            if (selectItem != null)
            {
                var selectIdx = CorpusItems.IndexOf(selectItem);

                var setItem = new CorpusItem(selectItem.No);
                if (int.TryParse(this.No.Value, out var noInt))
                {
                    setItem.No = noInt;
                }
                setItem.SentenceData.Name = this.Name.Value;
                setItem.SentenceData.Sentence = this.Sentence.Value;
                setItem.SentenceData.Kana = this.Kana.Value;
                setItem.TagsStr = this.Tags.Value;

                CorpusItems[selectIdx] = setItem;
                this.SelectItem.Value = setItem;
            }

            this.UpdateCorpusItem(this.CurrentData.Project, this.CorpusItems);
        }

        private void SetProject(string project)
        {
            this.CorpusItems.Clear();

            var currentProjectDirectoryParh = System.IO.Path.Combine(this.Settings.ProjectPass, project);
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

            this.CurrentData.Project = project;

            this.SelectItem.Value = this.CorpusItems.FirstOrDefault();
        }

        private void ChangeCorpus(CorpusItem? corpus)
        {
            if (corpus != null)
            {
                this.No.Value = corpus.No.ToString();
                this.Name.Value = corpus.SentenceData.Name;
                this.Sentence.Value = corpus.SentenceData.Sentence;
                this.Kana.Value = corpus.SentenceData.Kana;
                this.Tags.Value = corpus.TagsStr;
            }
        }


        private void UpdateCorpusItem(string project, ReactiveCollection<CorpusItem> corpusItems)
        {
            var currentProjectDirectoryParh = System.IO.Path.Combine(this.Settings.ProjectPass, project);
            var currentProjectFileParh = System.IO.Path.Combine(currentProjectDirectoryParh, CorpusConst.CORPUS_FILE_NAME_JSON);
            var enc = Encoding.GetEncoding(CorpusConst.CORPUS_FORMAT);

            File.Delete(currentProjectFileParh);

            using (var writer = new StreamWriter(currentProjectFileParh, true, enc))
            {
                writer.Write('[');

                foreach (var corpusItem in corpusItems)
                {
                    var setStr = JsonUtil.ToJson(corpusItem);

                    writer.Write(setStr);
                    if (corpusItems.LastOrDefault() != corpusItem)
                    {
                        writer.WriteLine(',');
                    }
                }

                writer.Write(']');
            }
        }
    }
}
