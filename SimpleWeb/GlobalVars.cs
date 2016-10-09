using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace SimpleWeb
{
    public static class GlobalVars
    {
        public static int studentID { get; set; } = 0;

        public static int groupID { get; set; } = 0;

        public static int testID { get; set; } = 0;

        public static bool flag { get; set; } = true; //убрать из релиза

        public static bool editFlag { get; set; }

        public static bool saveFlag { get; set; }

        public static bool deleteFlag { get; set; }

        public static bool authFlag { get; set; } = false;

        public static readonly char HTML_TAG = '0';
        public static readonly char CSS_TAG = '1';    
        public static readonly char JS_TAG = '2';
        public static readonly char XML_TAG = '3';

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
    }
}
