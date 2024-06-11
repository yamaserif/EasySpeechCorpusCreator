using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Documents;
using EasySpeechCorpusCreator.Consts;
using EasySpeechCorpusCreator.Models;
using Prism.Mvvm;
using Prism.Navigation;
using Reactive.Bindings;

namespace EasySpeechCorpusCreator.ViewModels
{
    public class ViewModelBase : BindableBase, IDestructible, IDisposable
    {
        protected Settings Settings { get; set; }
        protected CurrentData CurrentData { get; set; }
        protected List<string> Projects { get; set; }

        protected CompositeDisposable Disposable { get; } = new CompositeDisposable();

        public ViewModelBase()
        {
            this.Settings = Application.Current.Properties[AppPropertiesConst.SETTINGS] as Settings ?? new Settings();
            this.CurrentData = Application.Current.Properties[AppPropertiesConst.CURRENT_DATA] as CurrentData ?? new CurrentData();
            this.Projects = Application.Current.Properties[AppPropertiesConst.PROJECTS] as List<string> ?? new List<string>();
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

