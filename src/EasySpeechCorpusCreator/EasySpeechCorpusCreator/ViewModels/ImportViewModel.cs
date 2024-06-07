﻿using EasySpeechCorpusCreator.Consts;
using Prism.Mvvm;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace EasySpeechCorpusCreator.ViewModels
{
    public class ImportViewModel : ViewModelBase
    {
        public ReactiveProperty<string> Text { get; }

        public ImportViewModel()
        {
            this.Text = new ReactiveProperty<string>("ImportViewModel").AddTo(this.Disposable);
        }
    }
}
