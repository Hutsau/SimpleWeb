using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SimpleWeb.Models;
using MahApps.Metro.Controls.Dialogs;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.ComponentModel;

namespace SimpleWeb
{
    /// <summary>
    /// Логика взаимодействия для EditStudent.xaml
    /// </summary>
    public partial class DeleteWindow
    {
        Repository repository = new Repository();

        int _group = GlobalVars.GroupID;
        int _student = GlobalVars.StudentID;
        int _test = GlobalVars.TestID;

        string key = GlobalVars.GroupID != 0 ? "group" : GlobalVars.StudentID != 0 ? "student" : "test";

        bool canUserCloseForm = true;

        public DeleteWindow()
        {
            InitializeComponent();

            this.Closing += mClosing;

            OkBt.MouseDown += GlobalVars.mMouseDown;
            CancelBt.MouseDown += GlobalVars.mMouseDown;

            OkBt.MouseUp += GlobalVars.mMouseUp;
            CancelBt.MouseUp += GlobalVars.mMouseUp;

            OkBt.MouseLeave += GlobalVars.mMouseLeave;
            CancelBt.MouseLeave += GlobalVars.mMouseLeave;

            OkBt.MouseUp += mMouseUp;
            CancelBt.MouseUp += mMouseUp;

            this.Title = $"Delete {key}";

            Header.Content = _group != 0 ? repository.GetGroupNumber(_group) :
                _student != 0 ? repository.GetStudent(_student).Surname : repository.GetTest(_test).Name.Substring(1);

            InfoLabel.Content = $"This {key} will be deleted.";

            GlobalVars.DeleteFlag = false;
        }

        private void Cancel()
        {
            this.Close();
        }

        private async void Delete()
        {
            InfoLabel.Content = "Delete...";

            ButtonsGrid.Visibility = Visibility.Hidden;
            proggressBar.Visibility = Visibility.Visible;

            canUserCloseForm = false;

            await Task.Run(() => {
                Thread.Sleep(1000);

                switch (key) {
                    case "group": repository.DeleteGroup(repository.GetGroup(_group)); break;
                    case "student": repository.DeleteStudent(repository.GetStudent(_student)); break;
                    case "test": repository.DeleteTest(repository.GetTest(_test)); break;
                }

                repository.Save();
            });

            InfoLabel.Content = $"{key.ToUpper().ElementAt(0)}{key.Substring(1)} deleted.";
            proggressBar.Visibility = Visibility.Collapsed;
            PreCloseLabel.Visibility = Visibility.Visible;

            canUserCloseForm = true;
            GlobalVars.DeleteFlag = true;

            await Task.Run(() => Thread.Sleep(2000));

            this.Close();
        }

        private void mMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!GlobalVars.IsPressedFlag) return;

            GlobalVars.IsPressedFlag = false;

            switch (((Grid)sender).Name)
            {
                case "CancelBt": Cancel(); break;
                case "OkBt": Delete(); break;
            }
        }

        private void mClosing(object sender, CancelEventArgs e)
        {
            if (!canUserCloseForm) e.Cancel = true;
        }
    }
}
