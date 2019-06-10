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
    class RegisterWindowViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        private List<char> idComponents;

        public bool bValidUsernameInput
        {
            get;
            private set;
        }

        private string _inputIDNumber = "";
        public string InputIDNumber
        {
            get
            {
                return _inputIDNumber;
            }
            set
            {
                _inputIDNumber = value;
                RaisePropertyChanged("InputIDNumber");
            }
        }

        public string this[string columnName]
        {
            get
            {
                if (columnName == "InputIDNumber" && this.InputIDNumber != "")
                {
                    bValidUsernameInput = true;

                    if (this.InputIDNumber.Count() != 18)
                    {
                        bValidUsernameInput = false;
                        return "身份证长度应该是18位";
                    }
                    else
                    {
                        foreach(var c in InputIDNumber)
                        {
                            if (!idComponents.Contains(c)) return "身份证号码只能包含数字以及X字母";
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

        public RegisterWindowViewModel()
        {
            idComponents = new List<char>();
            for(int i = 0; i<=9; ++i) idComponents.Add((char)('0' + i));
            idComponents.Add('X');
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}