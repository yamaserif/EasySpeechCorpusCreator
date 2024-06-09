using EasySpeechCorpusCreator.Business;
using EasySpeechCorpusCreator.Consts;
using EasySpeechCorpusCreator.Models;
using Microsoft.WindowsAPICodePack.Dialogs;
using Prism.Commands;
using Prism.Mvvm;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Media;
using static EasySpeechCorpusCreator.Consts.SettingsConst;

namespace EasySpeechCorpusCreator.ViewModels
{
    public class SettingViewModel : ViewModelBase
    {
        public ReactiveProperty<Settings> Settings { get; }

        // 説明
        public ReactiveProperty<string> ExplainProjectPass { get; set; }
        public ReactiveProperty<string> ExplainFormat { get; set; }

        // 中身
        public ReactiveProperty<string> ProjectPass { get; set; }
        public ReactiveProperty<Format> Format { get; set; }

        // 要素設定値
        public ReactiveProperty<string> SettingProjectPassText { get; set; }
        public ReactiveCollection<Format> FormatList { get; set; }
        public ReactiveProperty<string> SaveSettingText { get; set; }
        public ReactiveProperty<string> ResetSettingText { get; set; }

        // ボタン 処理
        public DelegateCommand SettingProjectPassCommand { get; set; }
        public DelegateCommand SaveSettingCommand { get; set; }
        public DelegateCommand ResetSettingCommand { get; set; }

        public SettingViewModel()
        {
            var settings = Application.Current.Properties["Settings"] as Settings ?? new Settings();
            this.Settings = new ReactiveProperty<Settings>(settings).AddTo(this.Disposable);

            this.ExplainProjectPass = new ReactiveProperty<string>(SettingsConst.EXPLAIN_PROJECT_PASS).AddTo(this.Disposable);
            this.ExplainFormat = new ReactiveProperty<string>(SettingsConst.EXPLAIN_FORMAT).AddTo(this.Disposable);

            this.ProjectPass = new ReactiveProperty<string>(settings.ProjectPass).AddTo(this.Disposable);
            this.Format = new ReactiveProperty<Format>(settings.Format).AddTo(this.Disposable);

            this.SettingProjectPassText = new ReactiveProperty<string>(SystemConst.SETTING_PATH).AddTo(this.Disposable);
            this.FormatList = new ReactiveCollection<Format>()
            {
                SettingsConst.Format.CSV,
                SettingsConst.Format.JSON
            }.AddTo(this.Disposable);
            this.SaveSettingText = new ReactiveProperty<string>(SystemConst.SAVE_SETTING).AddTo(this.Disposable);
            this.ResetSettingText = new ReactiveProperty<string>(SystemConst.RESET_SETTING).AddTo(this.Disposable);

            this.SettingProjectPassCommand = new DelegateCommand(this.SettingProjectPass);
            this.SaveSettingCommand = new DelegateCommand(this.SaveSetting);
            this.ResetSettingCommand = new DelegateCommand(this.ResetSetting);
        }

        private void SettingProjectPass()
        {
            using (var cofd = new CommonOpenFileDialog()
            {
                Title = DialogConst.SELECT_DIRECTORY,
                InitialDirectory = this.ProjectPass.Value,
                IsFolderPicker = true,
            })
            {
                if (cofd.ShowDialog() != CommonFileDialogResult.Ok)
                {
                    return;
                }

                this.ProjectPass.Value = cofd?.FileName ?? string.Empty;
            }
        }

        private void SaveSetting()
        {
            this.Settings.Value.ProjectPass = this.ProjectPass.Value;
            this.Settings.Value.Format = this.Format.Value;

            var path = SettingsConst.SETTINGS_PATH;
            var enc = Encoding.GetEncoding(SettingsConst.SETTINGS_FORMAT);
            using (var writer = new StreamWriter(path, false, enc))
            {
                writer.WriteLine(JsonUtil.ToJson(this.Settings.Value));
            }

            // 保存完了 ダイアログ
            MessageBox.Show(SystemConst.SAVE_MESSAGE, SystemConst.SAVE_CAPTION, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ResetSetting()
        {
            var defaultSetting = new Settings();

            this.ProjectPass.Value = defaultSetting.ProjectPass;
            this.Format.Value = defaultSetting.Format;

            // リセット完了 ダイアログ
            MessageBox.Show(SystemConst.RESET_MESSAGE, SystemConst.RESET_CAPTION, MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
