using EasySpeechCorpusCreator.Consts;
using EasySpeechCorpusCreator.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Xml;

namespace EasySpeechCorpusCreator.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void Window_Closing(object sender, CancelEventArgs e)
        {
            // ソフト終了 確認ダイアログ
            var result = MessageBox.Show(SystemConst.CLOSE_MESSAGE, SystemConst.CLOSE_CAPTION, MessageBoxButton.YesNo, MessageBoxImage.Information);
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
                this.DataContext as ViewModelBase // MainWindow
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

            // Destroyを叩く
            foreach (var viewModel in viewModels)
            {
                viewModel?.Destroy();
            }
        }
    }
}
