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
using System.Windows.Shapes;
using SimpleWeb.Models;
using MahApps.Metro.Controls.Dialogs;

namespace SimpleWeb
{
    /// <summary>
    /// Логика взаимодействия для EditGroup.xaml
    /// </summary>
    public partial class EditGroup
    {
        Repository repository = new Repository();
        int _group = GlobalVars.GroupID;
        string group_number;

        public EditGroup()
        {
            InitializeComponent();

            SaveBt.MouseDown += GlobalVars.mMouseDown;
            CancelBt.MouseDown += GlobalVars.mMouseDown;

            SaveBt.MouseUp += GlobalVars.mMouseUp;
            CancelBt.MouseUp += GlobalVars.mMouseUp;

            SaveBt.MouseLeave += GlobalVars.mMouseLeave;
            CancelBt.MouseLeave += GlobalVars.mMouseLeave;

            SaveBt.MouseUp += mMouseUp;
            CancelBt.MouseUp += mMouseUp;

            GroupNumber.TextChanged += mTextChanged;
            GroupNumber.PreviewKeyDown += (sender, e) => { if (e.Key == Key.Enter) Save(); };

            BadLabel.Visibility = Visibility.Hidden;

            this.Title = _group == 0 ? "New Group" : "Edit Group";

            if (_group != 0) {
                group_number = repository.GetGroupNumber(_group);
                GroupNumber.Text = group_number;
            }

            GlobalVars.SaveFlag = false;
            GroupNumber.Focus();
        }

        private async void Cancel() {
            var group_number = GroupNumber.Text;

            var flag = true;
            if (_group != 0) {
                if (group_number != this.group_number) {
                    var result = await this.ShowMessageAsync(string.Empty, "Discard your changes?", MessageDialogStyle.AffirmativeAndNegative);
                    if (result == MessageDialogResult.Negative) flag = false;
                }
            } else if (!group_number.Equals(string.Empty)) {
                var result = await this.ShowMessageAsync(string.Empty, "Discard your changes?", MessageDialogStyle.AffirmativeAndNegative);
                if (result == MessageDialogResult.Negative) flag = false;
            }

            if (flag) this.Close();
        }

        private async void Save() {
            SaveLabel.Focus();

            if (GroupNumber.Text.Equals(string.Empty)) {
                GroupNumber.Focus();
                BadGroup.BorderBrush = Brushes.Red;
                BadLabel.Content = "Заполните обязательные поля";
                BadLabel.Visibility = Visibility.Visible;
                return;
            }

            var group_number = GroupNumber.Text;

            if (_group != 0)
                if (group_number == this.group_number) { this.Close(); return; }

            if (repository.GetGroup(GroupNumber.Text) != null) {
                GroupNumber.Focus();
                BadLabel.Content = "Группа с таким номером уже существует";
                BadLabel.Visibility = Visibility.Visible;
                return;
            }

            repository.SaveGroup(new Group {
                GroupID = _group,
                GroupNumber = group_number
            });

            await this.ShowMessageAsync(string.Empty, "Group saved.");
            GlobalVars.SaveFlag = true;
            this.Close();
        }

        private void mMouseUp(object sender, MouseButtonEventArgs e) {
            if (!GlobalVars.IsPressedFlag) return;

            GlobalVars.IsPressedFlag = false;

            switch (((Grid)sender).Name) {
                case "CancelBt": Cancel(); break;
                case "SaveBt": Save(); break;
            }
        }

        private void mTextChanged(object sender, TextChangedEventArgs e) {
            if (!BadGroup.BorderBrush.Equals(Brushes.LightGray)) {
                BadGroup.BorderBrush = Brushes.LightGray;
                BadLabel.Visibility = Visibility.Hidden;
            }
        }
    }
}
