using System;
using System.Diagnostics;
using System.Reactive.Disposables;
using System.Windows;
using EasySpeechCorpusCreator.Models;
using Prism.Mvvm;
using Prism.Navigation;
using Reactive.Bindings;

namespace EasySpeechCorpusCreator.ViewModels
{
    public class ViewModelBase : BindableBase, IDestructible, IDisposable
    {
        protected Settings Settings { get; }

        protected CompositeDisposable Disposable { get; } = new CompositeDisposable();

        public ViewModelBase()
        {
            this.Settings = Application.Current.Properties["Settings"] as Settings ?? new Settings();
        }

        public void Dispose()
        {
            this.Disposable.Dispose();
            Debug.WriteLine("Dispose ViewModel: {0}", this.GetType());
        }

        public void Destroy()
        {
            this.Dispose();
        }
    }
}

