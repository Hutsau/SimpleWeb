using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SimpleWeb.Models;
using MahApps.Metro.Controls.Dialogs;

namespace SimpleWeb
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private const double topOffset = 20;
        private const double leftOffset = 380;
        public MainWindow()
        {
            
            InitializeComponent();

            GlobalVars.MWindow = this;

            new AuthWindow().ShowDialog();
            if (!GlobalVars.AuthFlag) this.Close();

            GlobalVars.growlNotifications.Top = SystemParameters.WorkArea.Top + topOffset;
            GlobalVars.growlNotifications.Left = SystemParameters.WorkArea.Left + SystemParameters.WorkArea.Width - leftOffset;

            EditPassword.Click += (sender, e) => new EditPassword { Owner = GlobalVars.MWindow }.ShowDialog();

            Menu.SelectedIndex = 0;
        }

        protected override void OnClosed(System.EventArgs e)
        {
            GlobalVars.growlNotifications.Close();
            base.OnClosed(e);
        }
    
        private void Menu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (((ComboBox)sender).SelectedIndex) {
                case 0:
                    mFrame.Source = new Uri("StatPage.xaml", UriKind.Relative);
                    break;
                case 1:
                    mFrame.Source = new Uri("TestsPage.xaml", UriKind.Relative);
                    break;
                case 2:
                    mFrame.Source = new Uri("LogsPage.xaml", UriKind.Relative);
                    break;
            }
        }
    }
}
