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
using System.Configuration;
using SimpleWeb.Properties;
using System.Security;
using SimpleWeb.Models;

namespace SimpleWeb
{
    /// <summary>
    /// Логика взаимодействия для AuthWindow.xaml
    /// </summary>
    public partial class AuthWindow
    {
        // string password = EncDecHelper.DecryptString(Settings.Default.Password).ToInsecureString();
        string password;
        Repository rep = new Repository();

        public AuthWindow()
        {
            InitializeComponent();

            password = rep.GetPass; // 12345678q -- delete comment when release

            Password.PasswordChanged += mPasswordChanged;

            Password.Focus();
        }

        private void mPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (GlobalVars.GetHashCode(Password.Password) == password)
            {
                GlobalVars.AuthFlag = true;
                this.Close();
            }
        }
    }
}

   /* public static class EncDecHelper
    {
        static byte[] entropy = Encoding.Unicode.GetBytes(GlobalVars.MWindow.Title);

        public static string EncryptString(SecureString input)
        {
            byte[] encryptedData = System.Security.Cryptography.ProtectedData.Protect(
                Encoding.Unicode.GetBytes(ToInsecureString(input)),
                entropy,
                System.Security.Cryptography.DataProtectionScope.CurrentUser);
            return Convert.ToBase64String(encryptedData);
        }

        public static SecureString DecryptString(string encryptedData)
        {
            try
            {
                byte[] decryptedData = System.Security.Cryptography.ProtectedData.Unprotect(
                    Convert.FromBase64String(encryptedData),
                    entropy,
                    System.Security.Cryptography.DataProtectionScope.CurrentUser);
                return ToSecureString(Encoding.Unicode.GetString(decryptedData));
            }
            catch
            {
                return new SecureString();
            }
        }

        public static SecureString ToSecureString(this string input)
        {
            SecureString secure = new SecureString();
            foreach (char c in input)
            {
                secure.AppendChar(c);
            }
            secure.MakeReadOnly();
            return secure;
        }

        public static string ToInsecureString(this SecureString input)
        {
            string returnValue = string.Empty;
            IntPtr ptr = System.Runtime.InteropServices.Marshal.SecureStringToBSTR(input);
            try
            {
                returnValue = System.Runtime.InteropServices.Marshal.PtrToStringBSTR(ptr);
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ZeroFreeBSTR(ptr);
            }
            return returnValue;
        }

        public static void SetPassword(string password) {
            Settings.Default.Password = EncryptString(ToSecureString(password));
        }
    }
}
*/
  