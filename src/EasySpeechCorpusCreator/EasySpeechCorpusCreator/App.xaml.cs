using System.Windows;
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
