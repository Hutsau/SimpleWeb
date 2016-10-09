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
        public EditPassword()
        {
            InitializeComponent();

            SaveBt.MouseDown += GlobalVars.mMouseDown;
            CancelBt.MouseDown += GlobalVars.mMouseDown;

            SaveBt.MouseUp += GlobalVars.mMouseUp;
            CancelBt.MouseUp += GlobalVars.mMouseUp;

            SaveBt.MouseLeave += GlobalVars.mMouseLeave;
            CancelBt.MouseLeave += GlobalVars.mMouseLeave;

            BadName.Visibility = Visibility.Hidden;
            BadSurname.Visibility = Visibility.Hidden;
            BadSecondName.Visibility = Visibility.Hidden;
            BadLabel.Visibility = Visibility.Hidden;

            CancelBt.MouseUp += (sender, e) => {
                if (!GlobalVars.IsPressedFlag) return;

                GlobalVars.IsPressedFlag = false;

                this.Close();
            };
        }
    }
}
