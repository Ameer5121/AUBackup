using System;
using System.Windows;
using System.Windows.Input;
using System.IO;
using MVVMAUbackup.Serialization;
using MVVMAUbackup.ViewModels;

namespace MVVMAUbackup
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private FolderViewModel FM = new FolderViewModel();
        public MainWindow()
        {
            InitializeComponent();
            if (File.Exists(@"./History.dat"))
            {
                DataContext = FM.Deserialize();
            }
            else
            {
                DataContext = FM;
            }
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) { DragMove();}

        
    }
}
