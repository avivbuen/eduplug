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
using MahApps.Metro.Controls;

namespace EduPlugWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            lblLink.MouseDown += LblLink_MouseDown;
        }

        private void LblLink_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ConnectPhone form = new ConnectPhone();
            form.Show();
            this.Hide();
        }

        private void PasswordBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                PerformClick(ButtonBox);
        }
        private void PerformClick(Button btn)
        {
            btn.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Primitives.ButtonBase.ClickEvent));
        }

        private void idBox_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            IdBox.Text = "";
        }
    }
}
