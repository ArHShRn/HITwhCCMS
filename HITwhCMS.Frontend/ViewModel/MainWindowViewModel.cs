using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using MahApps.Metro;
using System.Windows.Input;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace HITwhCMS.Frontend.ViewModel
{
    /// <summary>
    /// View model for MainWindow.xaml
    /// </summary>
    public class MainWindowViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        private List<char> alphabets = new List<char>();
        public event PropertyChangedEventHandler PropertyChanged;

        public bool bValidUsernameInput
        {
            get;
            private set;
        }

        public MainWindowViewModel()
        {
            for(int i=0; i<10; ++i)
            {
                alphabets.Add((char)('0' + i));
            }
        }

        public string this[string columnName]
        {
            get
            {
                if (columnName == "InputStudentNumber" && this.InputStudentNumber != "")
                {
                    bValidUsernameInput = true;

                    if (this.InputStudentNumber.Count() !=9)
                    {
                        bValidUsernameInput = false;
                        return "学号长度应该是9位";
                    }
                    else
                    {
                        try
                        {
                            int.Parse(this.InputStudentNumber);
                        }
                        catch
                        {
                            bValidUsernameInput = false;
                            return "学号只能包含数字";
                        }
                    }
                }
                return null;
            }
        }

        public string Error { get { return string.Empty; } }

        /// <summary>
        /// Raises the PropertyChanged event if needed.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        protected virtual void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private bool _quitConfirmationEnabled = true;
        public bool QuitConfirmationEnabled
        {
            get { return _quitConfirmationEnabled; }
            set
            {
                if (value.Equals(_quitConfirmationEnabled)) return;
                _quitConfirmationEnabled = value;
                RaisePropertyChanged("QuitConfirmationEnabled");
            }
        }

        private string _inputStudentNumber = "123123123";
        public string InputStudentNumber
        {
            get
            {
                return _inputStudentNumber;
            }
            set
            {
                _inputStudentNumber = value;
                RaisePropertyChanged("InputStudentNumber");
            }
        }
    }
}
