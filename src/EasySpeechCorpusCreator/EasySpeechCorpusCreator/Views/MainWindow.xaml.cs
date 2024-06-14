using EasySpeechCorpusCreator.Consts;
using EasySpeechCorpusCreator.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml;

namespace EasySpeechCorpusCreator.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Dictionary<Key, bool> IsDowning { get; set; } = new Dictionary<Key, bool>()
        {
            {Key.Space, false},
            {Key.Z, false}
        };

        public MainWindow()
        {
            InitializeComponent();
        }

        public void Window_Closing(object sender, CancelEventArgs e)
        {
            // ソフト終了 確認ダイアログ
            var result = MessageBox.Show(DialogConst.CLOSE_MESSAGE, DialogConst.CLOSE_CAPTION, MessageBoxButton.YesNo, MessageBoxImage.Information);
            if (MessageBoxResult.Yes != result)
            {
                e.Cancel = true;
                return;
            }
        }

        public void Window_Closed(object sender, EventArgs e)
        {
            // 各ViewModelを取得
            var viewModels = new List<ViewModelBase?>() {
                this.DataContext as ViewModelBase
            };

            var mainGrid = this.Content as Grid;
            var tabControl = mainGrid?.Children?[0] as TabControl;
            var tabItems = tabControl?.Items;

            if (tabItems != null)
            {
                foreach (TabItem tabItem in tabItems)
                {
                    var view = tabItem.ContentTemplate.LoadContent() as UserControl;
                    viewModels.Add(view?.DataContext as ViewModelBase);
                }
            }

            foreach (var viewModel in viewModels)
            {
                // Closeイベント
                viewModel?.CloseEvents();

                // Destroyを叩く
                viewModel?.Destroy();
            }
        }

        public void Window_KeyDown(object sender, KeyEventArgs e)
        {
            var viewModel = this.DataContext as ViewModelBase;
            switch (e.Key)
            {
                case Key.Space:
                    if (!this.IsDowning[Key.Space])
                    {
                        this.IsDowning[Key.Space] = true;
                        viewModel?.KeySpaceDownAction();
                    }
                    break;
                case Key.Z:
                    if (!this.IsDowning[Key.Z])
                    {
                        this.IsDowning[Key.Z] = true;
                        viewModel?.KeyZDownAction();
                    }
                    break;
            }
        }

        public void Window_KeyUp(object sender, KeyEventArgs e)
        {
            var viewModel = this.DataContext as ViewModelBase;
            switch (e.Key)
            {
                case Key.Space:
                    this.IsDowning[Key.Space] = false;
                    viewModel?.KeySpaceUpAction();
                    break;
                case Key.Z:
                    this.IsDowning[Key.Z] = false;
                    viewModel?.KeyZUpAction();
                    break;
            }
        }
    }
}
