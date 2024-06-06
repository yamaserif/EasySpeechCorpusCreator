using EasySpeechCorpusCreator.Consts;
using Prism.Mvvm;
using Reactive.Bindings;

namespace EasySpeechCorpusCreator.ViewModels
{
    public class ImportViewModel : BindableBase
    {
        public ReactiveProperty<string> Text { get; } = new ReactiveProperty<string>("TestImportView");
    }
}
