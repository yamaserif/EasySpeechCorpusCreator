using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
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
        public ReactiveProperty<string> LabelEditProject { get; set; }
        public ReactiveProperty<string> LabelNo { get; set; }
        public ReactiveProperty<string> LabelName { get; set; }
        public ReactiveProperty<string> LabelSentence { get; set; }
        public ReactiveProperty<string> LabelKana { get; set; }
        public ReactiveProperty<string> LabelTags { get; set; }

        // 中身
        public ReactiveProperty<string> Project { get; }
        public ReactiveProperty<string> EditProjectName { get; }
        public ReactiveProperty<CorpusItem?> SelectItem { get; }
        public ReactiveCollection<CorpusItem> CorpusItems { get; }
        public ReactiveProperty<string> No { get; set; }
        public ReactiveProperty<string> Name { get; set; }
        public ReactiveProperty<string> Sentence { get; set; }
        public ReactiveProperty<string> Kana { get; set; }
        public ReactiveProperty<string> Tags { get; set; }
        public ReactiveProperty<string> RubyText { get; set; }
        public ReactiveProperty<string> RecordingText { get; set; }
        public ReactiveProperty<string?> VoiceImagePath { get; set; }
        public ReactiveProperty<ImageSource?> VoiceImage { get; set; }

        // 要素設定値
        public ReactiveCollection<string> ProjectList { get; set; }
        public ReactiveProperty<string> SaveCorpusItemText { get; set; }
        public ReactiveProperty<string> AddCorpusItemText { get; set; }
        public ReactiveProperty<string> DeleteCorpusItemText { get; set; }
        public ReactiveProperty<string> PlayVoiceText { get; set; }
        public ReactiveProperty<string> ExternalExeText { get; set; }

        // Header名
        public ReactiveProperty<CorpusHeader> CorpusItemHeader { get; }

        // ボタン 処理
        public DelegateCommand EditProjectCommand { get; set; }
        public DelegateCommand SaveCorpusItemCommand { get; set; }
        public DelegateCommand AddCorpusItemCommand { get; set; }
        public DelegateCommand DeleteCorpusItemCommand { get; set; }
        public DelegateCommand PlayVoiceCommand { get; set; }
        public DelegateCommand ExternalExeCommand { get; set; }

        public ManagerViewModel()
        {
            this.LabelProject = new ReactiveProperty<string>(ProjectConst.LABEL_PROJECT).AddTo(this.Disposable);
            this.LabelEditProject = new ReactiveProperty<string>(ProjectConst.LABEL_EDIT_PROJECT).AddTo(this.Disposable);
            this.LabelNo = new ReactiveProperty<string>(CorpusConst.NO + "：").AddTo(this.Disposable);
            this.LabelName = new ReactiveProperty<string>(CorpusConst.NAME + "：").AddTo(this.Disposable);
            this.LabelSentence = new ReactiveProperty<string>(CorpusConst.SENTENCE + "：").AddTo(this.Disposable);
            this.LabelKana = new ReactiveProperty<string>(CorpusConst.KANA + "：").AddTo(this.Disposable);
            this.LabelTags = new ReactiveProperty<string>(CorpusConst.TAGS + "：").AddTo(this.Disposable);

            this.Project = new ReactiveProperty<string>(this.CurrentData.Project == string.Empty ? ProjectConst.NEW_PROJECT : this.CurrentData.Project).AddTo(this.Disposable);
            this.EditProjectName = new ReactiveProperty<string>(this.Project.Value).AddTo(this.Disposable);
            this.SelectItem = new ReactiveProperty<CorpusItem?>().AddTo(this.Disposable);
            this.CorpusItemHeader = new ReactiveProperty<CorpusHeader>(new CorpusHeader()).AddTo(this.Disposable);
            this.CorpusItems = new ReactiveCollection<CorpusItem>().AddTo(this.Disposable);
            this.No = new ReactiveProperty<string>().AddTo(this.Disposable);
            this.Name = new ReactiveProperty<string>().AddTo(this.Disposable);
            this.Sentence = new ReactiveProperty<string>().AddTo(this.Disposable);
            this.Kana = new ReactiveProperty<string>().AddTo(this.Disposable);
            this.Tags = new ReactiveProperty<string>().AddTo(this.Disposable);
            this.RubyText = new ReactiveProperty<string>().AddTo(this.Disposable);
            this.RecordingText = new ReactiveProperty<string>(RecordingConst.READY).AddTo(this.Disposable);
            this.VoiceImagePath = new ReactiveProperty<string?>().AddTo(this.Disposable);
            this.VoiceImage = new ReactiveProperty<ImageSource?>().AddTo(this.Disposable);

            this.ProjectList = new ReactiveCollection<string>().AddTo(this.Disposable);
            this.ProjectList.Add(ProjectConst.NEW_PROJECT);
            this.Projects.ForEach(project => this.ProjectList.Add(project));
            this.SaveCorpusItemText = new ReactiveProperty<string>(SystemConst.SAVE).AddTo(this.Disposable);
            this.AddCorpusItemText = new ReactiveProperty<string>(SystemConst.ADD).AddTo(this.Disposable);
            this.DeleteCorpusItemText = new ReactiveProperty<string>(SystemConst.DELETE).AddTo(this.Disposable);
            this.PlayVoiceText = new ReactiveProperty<string>(SystemConst.PLAY_AUDIO).AddTo(this.Disposable);
            this.ExternalExeText = new ReactiveProperty<string>(SystemConst.EXTERNAL_EXE).AddTo(this.Disposable);

            this.EditProjectCommand = new DelegateCommand(this.EditProject);
            this.SaveCorpusItemCommand = new DelegateCommand(this.SaveCorpusItem);
            this.AddCorpusItemCommand = new DelegateCommand(this.AddCorpusItem);
            this.DeleteCorpusItemCommand = new DelegateCommand(this.DeleteCorpusItem);
            this.PlayVoiceCommand = new DelegateCommand(this.PlayAudio);
            this.ExternalExeCommand = new DelegateCommand(this.ExternalExe);

            // キー入力
            this.KeySpaceDownAction = this.StartRecording;
            this.KeySpaceUpAction = this.StopRecording;
            this.KeyZDownAction = this.PlayAudio;

            this.Project.Subscribe(this.SetProject);
            this.SelectItem.Subscribe(this.ChangeCorpus);
            this.VoiceImagePath.Subscribe(this.SetVoiceImage);
        }

        private void EditProject()
        {
            var newProjectName = this.EditProjectName.Value;

            if (this.CurrentData.Project != newProjectName || ProjectConst.NEW_PROJECT == newProjectName)
            {
                var newIndex = this.ProjectList.IndexOf(newProjectName);
                while (0 <= newIndex)
                {
                    newProjectName += ProjectConst.PROJECT_NAME_SUFFIX;
                    newIndex = this.ProjectList.IndexOf(newProjectName);
                }

                var oldDirectoryParh = System.IO.Path.Combine(this.Settings.ProjectPass, this.CurrentData.Project);
                var newDirectoryParh = System.IO.Path.Combine(this.Settings.ProjectPass, newProjectName);
                if (Directory.Exists(oldDirectoryParh))
                {
                    AudioUtil.StopAudio();
                    Directory.Move(oldDirectoryParh, newDirectoryParh);

                    var oldIndex = this.Projects.IndexOf(this.CurrentData.Project);
                    this.Projects[oldIndex] = newProjectName;

                    oldIndex = this.ProjectList.IndexOf(this.CurrentData.Project);
                    this.ProjectList[oldIndex] = newProjectName;
                }
                else
                {
                    Directory.CreateDirectory(newDirectoryParh);

                    this.Projects.Add(newProjectName);
                    this.ProjectList.Add(newProjectName);
                }

                this.Project.Value = newProjectName;

                this.EditProjectName.Value = newProjectName;
            }
        }

        private void SaveCorpusItem()
        {
            var selectItem = this.SelectItem.Value;

            if (selectItem != null)
            {
                var oldVoicePath = selectItem.VoiceFileName;
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
                setItem.VoiceFileName = selectItem.VoiceFileName;

                var newVoicePath = this.GetVoicePath(setItem);
                if (File.Exists(oldVoicePath))
                {
                    AudioUtil.StopAudio();
                    File.Move(oldVoicePath, newVoicePath);
                    this.VoiceImagePath.Value = newVoicePath;
                }

                CorpusItems[selectIdx] = setItem;
                this.SelectItem.Value = setItem;
            }

            this.UpdateCorpusItem(this.CurrentData.Project, this.CorpusItems);
        }

        private void AddCorpusItem()
        {
            if (this.CurrentData.Project == ProjectConst.NEW_PROJECT)
            {
                this.EditProject();
            }

            var no = 0;
            var name = this.Name.Value;
            var sentence = this.Sentence.Value;
            var kana = this.Kana.Value;
            var tags = this.Tags.Value;
            if (int.TryParse(this.No.Value, out var parseNo))
            {
                no = parseNo;
            }

            var addCorpusItem = new CorpusItem(
                no,
                name,
                sentence,
                kana,
                string.Empty,
                tags
            );
            this.CorpusItems.Add(addCorpusItem);
            this.SelectItem.Value = addCorpusItem;

            this.UpdateCorpusItem(this.CurrentData.Project, this.CorpusItems);
        }

        private void DeleteCorpusItem()
        {
            var selectItem = this.SelectItem.Value;
            if (selectItem != null)
            {
                var index = this.CorpusItems.IndexOf(selectItem);
                this.CorpusItems.Remove(selectItem);
                var deleteVoicePath = this.GetVoicePath(selectItem);

                if (index < this.CorpusItems.Count)
                {
                    this.SelectItem.Value = this.CorpusItems[index];
                    this.VoiceImagePath.Value = this.GetVoicePath(this.CorpusItems[index].VoiceFileName);
                }
                else
                {
                    this.SelectItem.Value = null;
                    this.VoiceImagePath.Value = null;
                }

                if (File.Exists(deleteVoicePath))
                {
                    AudioUtil.StopAudio();
                    File.Delete(deleteVoicePath);
                }

                this.UpdateCorpusItem(this.CurrentData.Project, this.CorpusItems);
            }
        }

        public void PlayAudio()
        {
            if (!(FocusManager.GetFocusedElement(this.MainWindow) is TextBox))
            {
                var selectItem = this.SelectItem.Value;
                if (selectItem != null)
                {
                    var voicePath = this.GetVoicePath(selectItem.VoiceFileName);
                    if (File.Exists(voicePath))
                    {
                        AudioUtil.PlayAudio(System.IO.Path.Combine(voicePath));
                    }
                }
            }
        }

        private void ExternalExe()
        {
        }

        public void StartRecording()
        {
            if (!(FocusManager.GetFocusedElement(this.MainWindow) is TextBox))
            {
                this.RecordingText.Value = RecordingConst.RECORDING_NOW;
                this.VoiceImagePath.Value = null;

                var selectItem = this.SelectItem.Value;
                if (selectItem != null)
                {
                    var voicePath = this.GetVoicePath(selectItem);
                    AudioUtil.StartRecording(voicePath, 0, 44100);
                }
            }
        }

        public void StopRecording()
        {
            this.RecordingText.Value = RecordingConst.READY;

            AudioUtil.StopRecording();

            var voiceFileName = this.GetVoiceFileName(this.SelectItem.Value);
            var voicePath = this.GetVoicePath(voiceFileName);
            this.VoiceImagePath.Value = voicePath;
            if (this.SelectItem.Value != null)
            {
                this.SelectItem.Value.VoiceFileName = voiceFileName;
            }

            this.UpdateCorpusItem(this.CurrentData.Project, this.CorpusItems);
        }

        private void SetProject(string? project)
        {
            this.EditProjectName.Value = project ?? string.Empty;

            this.CorpusItems.Clear();

            var currentProjectDirectoryParh = System.IO.Path.Combine(this.Settings.ProjectPass, project ?? string.Empty);
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

            this.CurrentData.Project = project ?? string.Empty;

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
                this.RubyText.Value = corpus.SentenceData.Sentence;
                this.VoiceImagePath.Value = this.GetVoicePath(corpus.VoiceFileName);
            }
            else
            {
                this.No.Value = string.Empty;
                this.Name.Value = string.Empty;
                this.Sentence.Value = string.Empty;
                this.Kana.Value = string.Empty;
                this.Tags.Value = string.Empty;
                this.RubyText.Value = string.Empty;
                this.VoiceImagePath.Value = null;
            }
        }

        private void SetVoiceImage(string? path)
        {
            if (path != null)
            {
                this.VoiceImage.Value = AudioUtil.ExWaveImage(path);
            }
            else
            {
                this.VoiceImage.Value = null;
            }
        }


        private void UpdateCorpusItem(string project, ReactiveCollection<CorpusItem> corpusItems)
        {
            var currentProjectDirectoryParh = System.IO.Path.Combine(this.Settings.ProjectPass, project);
            var currentProjectFileParh = System.IO.Path.Combine(currentProjectDirectoryParh, CorpusConst.CORPUS_FILE_NAME_JSON);
            var enc = Encoding.GetEncoding(CorpusConst.CORPUS_FORMAT);

            AudioUtil.StopAudio();
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

        private string GetVoicePath(CorpusItem? corpusItem)
        {
            return this.GetVoicePath(this.GetVoiceFileName(corpusItem));
        }
        private string GetVoicePath(string voiceFileName)
        {
            return System.IO.Path.Combine(
                        this.Settings.ProjectPass,
                        this.CurrentData.Project,
                        RecordingConst.VOICE_DIRECTORY,
                        voiceFileName
                    );
        }
        private string GetVoiceFileName(CorpusItem? corpusItem)
        {
            return corpusItem?.No + "_" + corpusItem?.SentenceData.Name + "." + RecordingConst.VOICE_EXTENSION;
        }
    }
}
