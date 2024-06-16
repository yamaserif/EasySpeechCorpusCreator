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
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using static EasySpeechCorpusCreator.Consts.SettingsConst;

namespace EasySpeechCorpusCreator.ViewModels
{
    public class SettingViewModel : ViewModelBase
    {
        // 説明
        public ReactiveProperty<string> ExplainProjectPass { get; set; }
        public ReactiveProperty<string> ExplainFormat { get; set; }
        public ReactiveProperty<string> ExplainExternalSoftware { get; set; }
        public ReactiveProperty<string> ExplainDevice { get; set; }
        public ReactiveProperty<string> ExplainSamplingRate { get; set; }
        public ReactiveProperty<string> ExplainSaveVoiceFormat { get; set; }

        // 中身
        public ReactiveProperty<string> ProjectPass { get; set; }
        public ReactiveProperty<Format> Format { get; set; }
        public ReactiveProperty<string> ExternalSoftware { get; set; }
        public ReactiveProperty<string> Device { get; set; }
        public ReactiveProperty<string> SamplingRate { get; set; }
        public ReactiveProperty<string> SaveVoiceFormat { get; set; }

        // 要素設定値
        public ReactiveProperty<string> SettingProjectPassText { get; set; }
        public ReactiveCollection<Format> FormatList { get; set; }
        public ReactiveCollection<string> DeviceList { get; set; }
        public ReactiveProperty<string> SaveSettingText { get; set; }
        public ReactiveProperty<string> ResetSettingText { get; set; }

        // ボタン 処理
        public DelegateCommand SettingProjectPassCommand { get; set; }
        public DelegateCommand SaveSettingCommand { get; set; }
        public DelegateCommand ResetSettingCommand { get; set; }

        public SettingViewModel()
        {
            this.ExplainProjectPass = new ReactiveProperty<string>(SettingsConst.EXPLAIN_PROJECT_PASS).AddTo(this.Disposable);
            this.ExplainFormat = new ReactiveProperty<string>(SettingsConst.EXPLAIN_FORMAT).AddTo(this.Disposable);
            this.ExplainExternalSoftware = new ReactiveProperty<string>(SettingsConst.EXPLAIN_EXTERNAL_SOFTWARE).AddTo(this.Disposable);
            this.ExplainDevice = new ReactiveProperty<string>(SettingsConst.EXPLAIN_DEVICE).AddTo(this.Disposable);
            this.ExplainSamplingRate = new ReactiveProperty<string>(SettingsConst.EXPLAIN_SAMPLING_RATE).AddTo(this.Disposable);
            this.ExplainSaveVoiceFormat = new ReactiveProperty<string>(SettingsConst.EXPLAIN_SAVE_VOICE_FORMAT).AddTo(this.Disposable);

            this.ProjectPass = new ReactiveProperty<string>(this.Settings.ProjectPass).AddTo(this.Disposable);
            this.Format = new ReactiveProperty<Format>(this.Settings.Format).AddTo(this.Disposable);
            this.ExternalSoftware = new ReactiveProperty<string>(this.Settings.ExternalSoftware).AddTo(this.Disposable);
            this.Device = new ReactiveProperty<string>().AddTo(this.Disposable);
            this.SamplingRate = new ReactiveProperty<string>(this.Settings.SamplingRate.ToString()).AddTo(this.Disposable);
            this.SaveVoiceFormat = new ReactiveProperty<string>(this.Settings.SaveVoiceFormat).AddTo(this.Disposable);

            this.SettingProjectPassText = new ReactiveProperty<string>(SystemConst.SETTING_PATH).AddTo(this.Disposable);
            this.FormatList = new ReactiveCollection<Format>()
            {
                SettingsConst.Format.JSON,
                SettingsConst.Format.CSV
            }.AddTo(this.Disposable);
            this.DeviceList = new ReactiveCollection<string>().AddTo(this.Disposable);
            this.SaveSettingText = new ReactiveProperty<string>(SystemConst.SAVE).AddTo(this.Disposable);
            this.ResetSettingText = new ReactiveProperty<string>(SystemConst.RESET).AddTo(this.Disposable);

            this.SettingProjectPassCommand = new DelegateCommand(this.SettingProjectPass);
            this.SaveSettingCommand = new DelegateCommand(this.SaveSetting);
            this.ResetSettingCommand = new DelegateCommand(this.ResetSetting);

            var devices = AudioUtil.GetRecordingDevices();
            devices.ForEach(device => {
                this.DeviceList.Add(device);
            });
            var deviceIndex = devices.IndexOf(this.Settings.Device);
            this.Device.Value = deviceIndex < 0 ? (devices.FirstOrDefault() ?? string.Empty) : devices[deviceIndex];
        }

        private void SettingProjectPass()
        {
            using (var fileDialog = new CommonOpenFileDialog()
            {
                Title = DialogConst.SELECT_DIRECTORY,
                InitialDirectory = this.ProjectPass.Value,
                IsFolderPicker = true
            })
            {
                if (fileDialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    this.ProjectPass.Value = fileDialog?.FileName ?? string.Empty;
                }
            }
        }

        private void SaveSetting()
        {
            this.Settings.ProjectPass = this.ProjectPass.Value;
            this.Settings.Format = this.Format.Value;
            this.Settings.ExternalSoftware = this.ExternalSoftware.Value;
            this.Settings.Device = this.Device.Value;
            if (int.TryParse(this.SamplingRate.Value, out var number))
            {
                this.Settings.SamplingRate = number;
            }
            this.Settings.SaveVoiceFormat = this.SaveVoiceFormat.Value;

            var path = SettingsConst.SETTINGS_PATH;
            var enc = Encoding.GetEncoding(SettingsConst.SETTINGS_FORMAT);
            using (var writer = new StreamWriter(path, false, enc))
            {
                writer.WriteLine(JsonUtil.ToJson(this.Settings));
            }

            // 保存完了 ダイアログ
            MessageBox.Show(DialogConst.SAVE_MESSAGE, DialogConst.SAVE_CAPTION, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ResetSetting()
        {
            var defaultSetting = new Settings();

            this.ProjectPass.Value = defaultSetting.ProjectPass;
            this.Format.Value = defaultSetting.Format;
            this.ExternalSoftware.Value = defaultSetting.ExternalSoftware;
            this.Device.Value = this.DeviceList.FirstOrDefault() ?? string.Empty;
            this.SamplingRate.Value = defaultSetting.SamplingRate.ToString();
            this.SaveVoiceFormat.Value = defaultSetting.SaveVoiceFormat;

            // リセット完了 ダイアログ
            MessageBox.Show(DialogConst.RESET_MESSAGE, DialogConst.RESET_CAPTION, MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
