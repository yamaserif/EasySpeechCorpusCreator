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
using System.Windows.Shapes;
using UtfUnknown;

namespace EasySpeechCorpusCreator.ViewModels
{
    public class ImportViewModel : ViewModelBase
    {

        // 説明
        public ReactiveProperty<string> ExplainImportPass { get; set; }
        public ReactiveProperty<string> ExplainImportPeek { get; set; }
        public ReactiveProperty<string> ExplainImportFormat { get; set; }

        // 中身
        public ReactiveProperty<string> ImportPass { get; set; }
        public ReactiveProperty<string> ImportPeek { get; set; }
        public ReactiveProperty<string> ImportVariableChar { get; set; }
        public ReactiveProperty<string> ImportFormat { get; set; }

        // 要素設定値
        public ReactiveProperty<string> SettingImportPassText { get; set; }
        public ReactiveProperty<string> ImportTestText { get; set; }
        public ReactiveProperty<string> ImportText { get; set; }
        public ReactiveProperty<string> ResetImportText { get; set; }

        // ボタン 処理
        public DelegateCommand SettingImportPassCommand { get; set; }
        public DelegateCommand FormatTestCommand { get; set; }
        public DelegateCommand ImportCommand { get; set; }
        public DelegateCommand ResetImportCommand { get; set; }

        public ImportViewModel()
        {
            this.ExplainImportPass = new ReactiveProperty<string>(ImportConst.EXPLAIN_IMPORT_PASS).AddTo(this.Disposable);
            this.ExplainImportPeek = new ReactiveProperty<string>(ImportConst.EXPLAIN_IMPORT_PEEK).AddTo(this.Disposable);
            this.ExplainImportFormat = new ReactiveProperty<string>(ImportConst.EXPLAIN_IMPORT_FORMAT).AddTo(this.Disposable);

            this.ImportPass = new ReactiveProperty<string>().AddTo(this.Disposable);
            this.ImportPeek = new ReactiveProperty<string>().AddTo(this.Disposable);
            this.ImportVariableChar = new ReactiveProperty<string>(ImportConst.DEFAULT_IMPORT_VARIABLE_CHAR).AddTo(this.Disposable);
            this.ImportFormat = new ReactiveProperty<string>(ImportConst.DEFAULT_IMPORT_FORMAT).AddTo(this.Disposable);

            this.SettingImportPassText = new ReactiveProperty<string>(SystemConst.SETTING_PATH).AddTo(this.Disposable);
            this.ImportTestText = new ReactiveProperty<string>(SystemConst.TEST).AddTo(this.Disposable);
            this.ImportText = new ReactiveProperty<string>(SystemConst.IMPORT).AddTo(this.Disposable);
            this.ResetImportText = new ReactiveProperty<string>(SystemConst.RESET).AddTo(this.Disposable);

            this.SettingImportPassCommand = new DelegateCommand(this.SettingImportPass);
            this.FormatTestCommand = new DelegateCommand(this.FormatTest);
            this.ImportCommand = new DelegateCommand(this.Import);
            this.ResetImportCommand = new DelegateCommand(this.ResetImport);
        }

        private void SettingImportPass()
        {
            this.ImportPeek.Value = string.Empty;

            using (var fileDialog = new CommonOpenFileDialog()
            {
                Title = DialogConst.SELECT_FILE,
                InitialDirectory = this.ImportPass.Value,
                IsFolderPicker = false
            })
            {
                if (fileDialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    this.ImportPass.Value = fileDialog?.FileName ?? string.Empty;

                    if (File.Exists(this.ImportPass.Value))
                    {
                        var charset = CharsetDetector.DetectFromFile(this.ImportPass.Value);
                        using (var reader = new StreamReader(this.ImportPass.Value, charset.Detected.Encoding))
                        {
                            string? line;
                            for (var i = 0; ((line = reader.ReadLine()) != null) && i < 3; i++)
                            {
                                this.ImportPeek.Value += line + '\n';
                            }
                        }
                    }
                }
            }
        }

        private void FormatTest()
        {
            if (File.Exists(this.ImportPass.Value))
            {
                var charset = CharsetDetector.DetectFromFile(this.ImportPass.Value);
                using (var reader = new StreamReader(this.ImportPass.Value, charset.Detected.Encoding))
                {
                    var testResult = DialogConst.FORMAT_TEST_MESSAGE;

                    string? line;
                    if ((line = reader.ReadLine()) != null)
                    {
                        var corpusItem = CorpusImportUtil.ToCorpusItem(0, line, this.ImportVariableChar.Value, this.ImportFormat.Value);

                        testResult += CorpusConst.NO + ": " + corpusItem.No + '\n';
                        testResult += CorpusConst.NAME + ": " + corpusItem.Name + '\n';
                        testResult += CorpusConst.SENTENCE + ": " + corpusItem.Sentence + '\n';
                        testResult += CorpusConst.KANA + ": " + corpusItem.Kana + '\n';
                    }

                    // テスト結果 ダイアログ
                    MessageBox.Show(testResult, DialogConst.FORMAT_TEST_CAPTION, MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                // ファイル読み込み不可 ダイアログ
                MessageBox.Show(DialogConst.CANT_READ_FILE_MESSAGE, DialogConst.CANT_READ_FILE_CAPTION, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Import()
        {
            // TODO:実装
            // 保存完了 ダイアログ
            MessageBox.Show(DialogConst.SAVE_MESSAGE, DialogConst.SAVE_CAPTION, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ResetImport()
        {
            this.ImportPass.Value = String.Empty;
            this.ImportPeek.Value = String.Empty;
            this.ImportVariableChar.Value = ImportConst.DEFAULT_IMPORT_VARIABLE_CHAR;
            this.ImportFormat.Value = ImportConst.DEFAULT_IMPORT_FORMAT;
        }
    }
}
