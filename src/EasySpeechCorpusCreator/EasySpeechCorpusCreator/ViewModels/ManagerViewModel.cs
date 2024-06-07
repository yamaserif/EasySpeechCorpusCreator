using System;
using System.Reactive.Disposables;
using EasySpeechCorpusCreator.Consts;
using EasySpeechCorpusCreator.Models;
using Prism.Mvvm;
using Prism.Navigation;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace EasySpeechCorpusCreator.ViewModels
{
    public class ManagerViewModel : ViewModelBase
    {
        public ReactiveProperty<string> Text { get; }
        public ReactiveProperty<CorpusItem?> SelectItem { get; }
        public ReactiveCollection<CorpusItem> CorpusItems { get; }

        // Header名
        public ReactiveProperty<CorpusHeader> CorpusItemHeader { get; }

        public ManagerViewModel()
        {
            this.Text = new ReactiveProperty<string>().AddTo(this.Disposable);
            this.SelectItem = new ReactiveProperty<CorpusItem?>().AddTo(this.Disposable);
            this.CorpusItemHeader = new ReactiveProperty<CorpusHeader>(new CorpusHeader()).AddTo(this.Disposable);
            this.CorpusItems = new ReactiveCollection<CorpusItem>
            {
                new CorpusItem(0, "CorpusItem1"),
                new CorpusItem(1, "CorpusItem2")
            }.AddTo(this.Disposable);

            SelectItem.Subscribe(x => this.Text.Value = x?.No.ToString() ?? "");
        }
    }
}
