using EasySpeechCorpusCreator.Consts;
using Prism.Mvvm;
using Reactive.Bindings;

namespace EasySpeechCorpusCreator.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        public ReactiveProperty<string> Title { get; } = new ReactiveProperty<string>(SystemConst.TITLE);
        public ReactiveProperty<string> Text { get; } = new ReactiveProperty<string>("Test");
    }
}
