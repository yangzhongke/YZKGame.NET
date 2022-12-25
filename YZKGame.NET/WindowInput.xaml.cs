using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace YZKGame.NET
{
    /// <summary>
    /// WindowInput.xaml 的交互逻辑
    /// </summary>
    partial class WindowInput : Window
    {
        private TextBlock txtMsg1;
        private TextBox txtValue1;

        public WindowInput()
        {
            InitializeComponent();

            txtMsg1 = (TextBlock)FindName("txtMsg");
            txtValue1 = (TextBox)FindName("txtValue");

            this.Loaded += WindowInput_Loaded;
        }

        void WindowInput_Loaded(object sender, RoutedEventArgs e)
        {
            txtValue1.Focus();
        }

        public string Value
        {
            get
            {
                return txtValue1.Text;
            }
            set
            {
                txtValue1.Text = value;
            }
        }

        public string Message
        {
            get
            {
                return txtMsg1.Text;
            }
            set
            {
                txtMsg1.Text = value;
            }
        }

        public static bool ShowInputBox(string msg,out string value)
        {
            WindowInput window = new WindowInput();
            window.Value = "";
            window.Message = msg;
            bool? result = window.ShowDialog();
            value = window.Value;
            return result== true;
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void txtValue_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                DialogResult = true;
            }
        }
    }
}
