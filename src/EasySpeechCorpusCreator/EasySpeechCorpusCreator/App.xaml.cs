using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Shapes;
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
            if (Directory.Exists(setting?.ProjectPass))
            {
                var directory = new DirectoryInfo(setting.ProjectPass);
                var files = directory.GetDirectories("*", SearchOption.TopDirectoryOnly);
                if (0 < files.Length)
                {
                    Application.Current.Properties[AppPropertiesConst.CURRENT_PROJECT] = files.First().Name;
                }
            }

            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }

        protected override void ConfigureViewModelLocator()
        {
            base.ConfigureViewModelLocator();

            ViewModelLocationProvider.Register<MainWindow, MainWindowViewModel>();
        }
    }
}
