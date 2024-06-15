using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Threading;
using EasySpeechCorpusCreator.Business;
using EasySpeechCorpusCreator.Consts;
using EasySpeechCorpusCreator.Models;
using EasySpeechCorpusCreator.ViewModels;
using EasySpeechCorpusCreator.Views;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Unity;

namespace EasySpeechCorpusCreator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            this.DispatcherUnhandledException += this.CatchException;
        }

        protected override Window CreateShell()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var path = SettingsConst.SETTINGS_PATH;
            var enc = Encoding.GetEncoding(SettingsConst.SETTINGS_FORMAT);
            var currentDir = AppDomain.CurrentDomain.BaseDirectory;
            Directory.SetCurrentDirectory(currentDir);

            // 設定ファイルがない場合は作成する
            if (!File.Exists(path))
            {
                using (var writer = new StreamWriter(path, false, enc))
                {
                    writer.WriteLine(JsonUtil.ToJson(new Settings()));
                }
            }

            // 設定値 読み込み
            var settingsStr = File.ReadAllText(path, enc);
            var setting = JsonUtil.ToSettings(settingsStr);
            Application.Current.Properties[AppPropertiesConst.SETTINGS] = setting;

            // プロジェクトファイルがない場合は作成する
            if (setting != null && !File.Exists(setting.ProjectPass))
            {
                Directory.CreateDirectory(setting.ProjectPass);
            }

            // 作業プロジェクト 読み込み
            Application.Current.Properties[AppPropertiesConst.PROJECTS] = new List<string>();
            Application.Current.Properties[AppPropertiesConst.CURRENT_DATA] = new CurrentData();
            if (Directory.Exists(setting?.ProjectPass))
            {
                var directory = new DirectoryInfo(setting.ProjectPass);
                var files = directory.GetDirectories("*", SearchOption.TopDirectoryOnly);
                if (0 < files.Length)
                {
                    var fileNames = files.Select(file => { return file.Name; }).ToList();
                    Application.Current.Properties[AppPropertiesConst.PROJECTS] = fileNames;
                    Application.Current.Properties[AppPropertiesConst.CURRENT_DATA] = new CurrentData(fileNames.First());
                }
            }

            var mainWindow = Container.Resolve<MainWindow>();
            Application.Current.Properties[AppPropertiesConst.MAIN_WINDOW] = mainWindow;

            return mainWindow;
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }

        protected override void ConfigureViewModelLocator()
        {
            base.ConfigureViewModelLocator();

            ViewModelLocationProvider.Register<MainWindow, MainWindowViewModel>();
        }
        private void CatchException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            var exMessage = e.Exception.ToString();

            // 保存完了 ダイアログ
            MessageBox.Show(
                DialogConst.EXCEPTION_MESSAGE + exMessage,
                DialogConst.EXCEPTION_CAPTION,
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }
}
