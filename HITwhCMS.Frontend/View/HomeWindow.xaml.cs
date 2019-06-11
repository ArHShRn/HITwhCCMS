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
using Microsoft.Xaml.Behaviors.Core;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HITwhCMS.Frontend.View
{
    /// <summary>
    /// Interaction logic for HomeWindow.xaml
    /// </summary>
    public partial class HomeWindow
    {
        public HomeWindow()
        {
            InitializeComponent();
        }

        public ICommand QuickLaunchBarFocusCommand => new ActionCommand(FocusQuickLaunchBar);

        private void FocusQuickLaunchBar()
        {
            QuickLaunchBar.Focus();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
