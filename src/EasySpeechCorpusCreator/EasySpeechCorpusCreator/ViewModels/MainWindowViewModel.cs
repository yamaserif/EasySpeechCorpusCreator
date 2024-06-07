using EasySpeechCorpusCreator.Consts;
using Prism.Mvvm;
using Prism.Navigation;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace EasySpeechCorpusCreator.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ReactiveProperty<string> Title { get; } = new ReactiveProperty<string>(SystemConst.TITLE);

        // ページ名
        public ReactiveProperty<string> ManagerName { get; }
        public ReactiveProperty<string> ImportName { get; }
        public ReactiveProperty<string> SettingName { get; }

        public MainWindowViewModel()
        {
            this.Title = new ReactiveProperty<string>(SystemConst.TITLE).AddTo(this.Disposable);

            this.ManagerName = new ReactiveProperty<string>(PageNameConst.MANAGER).AddTo(this.Disposable);
            this.ImportName = new ReactiveProperty<string>(PageNameConst.IMPORT).AddTo(this.Disposable);
            this.SettingName = new ReactiveProperty<string>(PageNameConst.SETTING).AddTo(this.Disposable);
        }
    }
}
