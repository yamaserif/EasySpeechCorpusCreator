using EasySpeechCorpusCreator.Consts;
using Prism.Mvvm;
using Reactive.Bindings;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Linq;

namespace EasySpeechCorpusCreator.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        public ReactiveProperty<string> Title { get; } = new ReactiveProperty<string>(SystemConst.TITLE);

        // ページ名
        public ReactiveProperty<string> ManagerName { get; } = new ReactiveProperty<string>(PageNameConst.MANAGER);
        public ReactiveProperty<string> ImportName { get; } = new ReactiveProperty<string>(PageNameConst.IMPORT);
    }
}
