using MahApps.Metro.Controls.Dialogs;
using SimpleWeb.Properties;
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

namespace SimpleWeb
{
    /// <summary>
    /// Логика взаимодействия для EditPassword.xaml
    /// </summary>
    public partial class EditPassword
    {
        string password = EncDecHelper.DecryptString(Settings.Default.Password).ToInsecureString();

        public EditPassword()
        {
            InitializeComponent();

            SaveBt.MouseDown += GlobalVars.mMouseDown;
            CancelBt.MouseDown += GlobalVars.mMouseDown;

            SaveBt.MouseUp += GlobalVars.mMouseUp;
            CancelBt.MouseUp += GlobalVars.mMouseUp;

            SaveBt.MouseLeave += GlobalVars.mMouseLeave;
            CancelBt.MouseLeave += GlobalVars.mMouseLeave;

            //BadName.Visibility = Visibility.Hidden;
            //BadSurname.Visibility = Visibility.Hidden;
            //BadSecondName.Visibility = Visibility.Hidden;
            BadLabel.Visibility = Visibility.Hidden;

            NewPassword.PreviewKeyDown += (sender, e) => { if (e.Key == Key.Enter) SaveNewPassword(); };
            ConfirmPassword.PreviewKeyDown += (sender, e) => { if (e.Key == Key.Enter) SaveNewPassword(); };

            NewPassword.PasswordChanged += (sender, e) => {
                if (!BadNewPassword.BorderBrush.Equals(Brushes.LightGray)) {
                        BadNewPassword.BorderBrush = Brushes.LightGray;

                    if (BadConfirmPassword.BorderBrush.Equals(Brushes.LightGray))
                            BadLabel.Visibility = Visibility.Hidden;
                }

                if (BadLabel.Visibility.Equals(Visibility.Visible) &&
                    BadConfirmPassword.BorderBrush.Equals(Brushes.LightGray))
                    BadLabel.Visibility = Visibility.Hidden;
            };

            ConfirmPassword.PasswordChanged += (sender, e) => {
                if (!BadConfirmPassword.BorderBrush.Equals(Brushes.LightGray)) {
                        BadConfirmPassword.BorderBrush = Brushes.LightGray;

                    if (BadNewPassword.BorderBrush.Equals(Brushes.LightGray))
                            BadLabel.Visibility = Visibility.Hidden;
                }

                if (BadLabel.Visibility.Equals(Visibility.Visible) &&
                    BadNewPassword.BorderBrush.Equals(Brushes.LightGray))
                    BadLabel.Visibility = Visibility.Hidden;
            };

            NewPassword.IsEnabled = false;
            ConfirmPassword.IsEnabled = false;

            Password.PasswordChanged += (sender, e) => {
                if (Password.Password == password) {
                    Password.IsEnabled = false;
                    NewPassword.IsEnabled = true;
                    ConfirmPassword.IsEnabled = true;

                    NewPassword.Focus();
                }
            };

            CancelBt.MouseUp += (sender, e) => {
                if (!GlobalVars.IsPressedFlag) return;

                GlobalVars.IsPressedFlag = false;

                this.Close();
            };

            SaveBt.MouseUp += (sender, e) => {
                if (!GlobalVars.IsPressedFlag) return;

                GlobalVars.IsPressedFlag = false;

                SaveNewPassword();
            };

            Password.Focus();
        }

        private async void SaveNewPassword() {
            SaveLabel.Focusable = true;
            SaveLabel.Focus();
            SaveLabel.Focusable = false;

            if (Password.IsEnabled) this.Close();

            var flag = true;

            if (NewPassword.Password.Equals(string.Empty)) {
                flag = false;
                BadNewPassword.BorderBrush = Brushes.Red;
            }

            if (ConfirmPassword.Password.Equals(string.Empty)) {
                flag = false;
                BadConfirmPassword.BorderBrush = Brushes.Red;
            }

            if (!flag) {
                BadLabel.Content = "Заполните обязательные поля";
                BadLabel.Visibility = Visibility.Visible;
                return;
            }

            if (NewPassword.Password != ConfirmPassword.Password) {               
                NewPassword.Clear();
                ConfirmPassword.Clear();
                NewPassword.Focus();
                BadLabel.Content = "Пароли не совпадают";
                BadLabel.Visibility = Visibility.Visible;
                return;
            }

            if (NewPassword.Password == Password.Password) {
                NewPassword.Clear();
                ConfirmPassword.Clear();
                NewPassword.Focus();
                BadLabel.Content = "Пароль уже используется";
                BadLabel.Visibility = Visibility.Visible;
                return;
            }

            EncDecHelper.SetPassword(NewPassword.Password);

            await this.ShowMessageAsync(string.Empty, "Password has changed.");
            this.Close();
        }
    }
}
