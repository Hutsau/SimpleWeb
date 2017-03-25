using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace SimpleWeb
{
    public static class GlobalVars
    {
        public static int StudentID { get; set; } = 0;

        public static int GroupID { get; set; } = 0;

        public static int TestID { get; set; } = 0;

        //public static bool DebugFlag { get; set; } = true; // delete when release

        public static bool SaveFlag { get; set; }

        public static bool DeleteFlag { get; set; }

        public static bool AuthFlag { get; set; } = false;

        public static readonly char HTML_TAG = '0';
        public static readonly char CSS_TAG = '1';    
        public static readonly char JS_TAG = '2';
        public static readonly char XML_TAG = '3';

        public static readonly GrowlNotifiactions growlNotifications = new GrowlNotifiactions();

        public static MainWindow MWindow;
        public static bool IsPressedFlag { get; set; } = false;
        public static void mMouseDown(object sender, MouseButtonEventArgs e) {
            try { (((Grid)sender).Children[0] as Label).FontSize -= 2; }
            catch (Exception) { }
            IsPressedFlag = true;
        }
        public static void mMouseUp(object sender, MouseButtonEventArgs e) {
            if (IsPressedFlag)
                try { (((Grid)sender).Children[0] as Label).FontSize += 2; }
                catch (Exception) { }
        }
        public static void mMouseLeave(object sender, MouseEventArgs e) {
            if (IsPressedFlag) {
                try { (((Grid)sender).Children[0] as Label).FontSize += 2; }
                catch (Exception) { }
                IsPressedFlag = false;
            }
        }

        public static string GetHashCode(string s) {
            if (s == null) s = string.Empty;

            return string.Join("", new SHA256CryptoServiceProvider()
                                            .ComputeHash(Encoding.UTF8.GetBytes(s))
                                            .Select(x => x.ToString("x2")));
        }
    }
}
