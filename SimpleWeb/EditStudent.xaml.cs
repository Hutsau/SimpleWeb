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

namespace SimpleWeb
{
    /// <summary>
    /// Логика взаимодействия для EditStudent.xaml
    /// </summary>
    public partial class EditStudent
    {
        Repository repository = new Repository();
        int _group = GlobalVars.groupID;
        int _student = GlobalVars.studentID;
        Student student;

        public EditStudent()
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

            Surname.TextChanged += mTextChanged;
            Name.TextChanged += mTextChanged;
            SecondName.TextChanged += mTextChanged;

            BadName.Visibility = Visibility.Hidden;
            BadSurname.Visibility = Visibility.Hidden;
            BadLabel.Visibility = Visibility.Hidden;

            this.Title = _student == 0 ? "New Student" : "Edit Student";
            GroupNumber.Content = repository.GetGroupNumber(_group);

            if (_student != 0) {
                student = repository.GetStudent(_student);
                Name.Text = student.Name;
                Surname.Text = student.Surname;
                SecondName.Text = student.SecondName;
            }

            GlobalVars.saveFlag = false;
        }

        private async void Cancel() {
            var _surname = Surname.Text;
            var _name = Name.Text;
            var _second_name = SecondName.Text;

            var flag = true;
            if (_student != 0) {
                if (_surname != student.Surname || _name != student.Name || _second_name != student.SecondName) {
                    var result = await this.ShowMessageAsync(string.Empty, "Discard your changes?", MessageDialogStyle.AffirmativeAndNegative);
                    if (result == MessageDialogResult.Negative) flag = false;
                }
            } else if (_surname != "" || _name != "" || _second_name != "") {
                var result = await this.ShowMessageAsync(string.Empty, "Discard your changes?", MessageDialogStyle.AffirmativeAndNegative);
                if (result == MessageDialogResult.Negative) flag = false;
            }

            if (flag) this.Close();
        }

        private async void Save() {
            SaveLabel.Focus();

            var flag = true;

            if (Surname.Text.Equals("")) {
                BadSurname.Visibility = Visibility.Visible;
                flag = false;
            }

            if (Name.Text.Equals("")) {
                BadName.Visibility = Visibility.Visible;
                flag = false;
            }

            if (!flag) {
                BadLabel.Visibility = Visibility.Visible;
                return;
            }

            var _surname = Surname.Text;
            var _name = Name.Text;
            var _second_name = SecondName.Text;

            if (student != null)
                if (_surname == student.Surname &&
                    _name == student.Name &&
                    _second_name == student.SecondName) { this.Close(); return; }

            repository.SaveStudent(new Student {
                    StudentID = _student,
                    GroupID = _group,
                    Name = _name,
                    Surname = _surname,
                    SecondName = _second_name
            });

            await this.ShowMessageAsync(string.Empty, "Student saved.");
            GlobalVars.saveFlag = true;
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

        private void mTextChanged(object sender, TextChangedEventArgs e)
        {
            if (((TextBox)sender).Text.Length == 1)
                ((TextBox)sender).Text = ((TextBox)sender).Text.ToUpper();
            ((TextBox)sender).Select(((TextBox)sender).Text.Length, 0);

            switch (((TextBox)sender).Name) {
                case "Surname":
                    if (!BadSurname.Visibility.Equals(Visibility.Hidden)) {
                        BadSurname.Visibility = Visibility.Hidden;

                        if (BadName.Visibility.Equals(Visibility.Hidden))
                            BadLabel.Visibility = Visibility.Hidden;
                    } break;
                case "Name":
                    if (!BadName.Visibility.Equals(Visibility.Hidden)) {
                        BadName.Visibility = Visibility.Hidden;

                        if (BadSurname.Visibility.Equals(Visibility.Hidden))
                            BadLabel.Visibility = Visibility.Hidden;
                    } break;
            }
        }
    }
}
