using System;
using System.Windows;
using System.Windows.Input;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
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
        private static NotifyIcon Notify = new NotifyIcon();
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

        private void Minimize(object sender, RoutedEventArgs e) { WindowState = WindowState.Minimized; }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
            {
                ShowInTaskbar = false;
                var iconHandle = Properties.Resources.Backup.GetHicon();
                Notify.Icon = System.Drawing.Icon.FromHandle(iconHandle);
                Notify.DoubleClick += new EventHandler(WindowsStateToNormal);
                Notify.Visible = true;
                Notify.BalloonTipText = "Automatic Backup";
                Notify.BalloonTipTitle = "Your application is in the tray bar.";
                Notify.BalloonTipIcon = ToolTipIcon.Info;
                Notify.ShowBalloonTip(2000);
            }
        }

        private void WindowsStateToNormal(object sender, EventArgs e)
        {
            WindowState = WindowState.Normal;
            ShowInTaskbar = true;
        }


    }
}
