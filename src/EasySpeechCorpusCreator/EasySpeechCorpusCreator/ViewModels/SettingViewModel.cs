using EasySpeechCorpusCreator.Consts;
using Prism.Mvvm;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace EasySpeechCorpusCreator.ViewModels
{
    public class SettingViewModel : ViewModelBase
    {
        public ReactiveProperty<string> Text { get; }

        public SettingViewModel()
        {
            this.Text = new ReactiveProperty<string>("SettingViewModel").AddTo(this.Disposable);
        }
    }
}
