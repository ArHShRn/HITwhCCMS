using MahApps.Metro.Controls;
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

namespace HITwhCMS.Frontend.View
{
    /// <summary>
    /// Interaction logic for HomeWindow.xaml
    /// </summary>
    public partial class HomeWindow : MetroWindow
    {
        public HomeWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Common method to launch a new window
        /// </summary>
        public void Launch()
        {
            Owner = Application.Current.MainWindow;
            // only for this window, because we allow minimizing
            if (WindowState == WindowState.Minimized)
            {
                WindowState = WindowState.Normal;
            }
            Show();
        }
    }
}
