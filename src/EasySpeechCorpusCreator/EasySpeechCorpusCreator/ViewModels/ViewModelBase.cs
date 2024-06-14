using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Documents;
using EasySpeechCorpusCreator.Consts;
using EasySpeechCorpusCreator.Models;
using EasySpeechCorpusCreator.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Reactive.Bindings;

namespace EasySpeechCorpusCreator.ViewModels
{
    public class ViewModelBase : BindableBase, IDestructible, IDisposable
    {
        protected Window? MainWindow { get; set; }
        protected Settings Settings { get; set; }
        protected CurrentData CurrentData { get; set; }
        protected List<string> Projects { get; set; }

        // キーの入力
        public Action KeySpaceDownAction {
            get
            {
                return Application.Current.Properties[AppPropertiesConst.KEY_SPACE_DOWN_ACTION] as Action ?? (() => { });
            }
            set {
                Application.Current.Properties[AppPropertiesConst.KEY_SPACE_DOWN_ACTION] = value;
            }
        }
        public Action KeySpaceUpAction
        {
            get
            {
                return Application.Current.Properties[AppPropertiesConst.KEY_SPACE_UP_ACTION] as Action ?? (() => { });
            }
            set
            {
                Application.Current.Properties[AppPropertiesConst.KEY_SPACE_UP_ACTION] = value;
            }
        }
        public Action KeyZDownAction
        {
            get
            {
                return Application.Current.Properties[AppPropertiesConst.KEY_Z_DOWN_ACTION] as Action ?? (() => { });
            }
            set
            {
                Application.Current.Properties[AppPropertiesConst.KEY_Z_DOWN_ACTION] = value;
            }
        }
        public Action KeyZUpAction
        {
            get
            {
                return Application.Current.Properties[AppPropertiesConst.KEY_Z_UP_ACTION] as Action ?? (() => { });
            }
            set
            {
                Application.Current.Properties[AppPropertiesConst.KEY_Z_UP_ACTION] = value;
            }
        }

        protected CompositeDisposable Disposable { get; } = new CompositeDisposable();

        public ViewModelBase()
        {
            this.MainWindow = Application.Current.Properties[AppPropertiesConst.MAIN_WINDOW] as Window;
            this.Settings = Application.Current.Properties[AppPropertiesConst.SETTINGS] as Settings ?? new Settings();
            this.CurrentData = Application.Current.Properties[AppPropertiesConst.CURRENT_DATA] as CurrentData ?? new CurrentData();
            this.Projects = Application.Current.Properties[AppPropertiesConst.PROJECTS] as List<string> ?? new List<string>();
        }

        public void CloseEvents()
        {
            // 念の為、継承時に使えるように作成
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

