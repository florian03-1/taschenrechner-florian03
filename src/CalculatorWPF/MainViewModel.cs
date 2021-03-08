using CalculatorWPF.Infrastructure;
using ExpressionParser;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CalculatorWPF
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            Calculations = new ObservableCollection<CalculationVm>();
            IsError = false;
        }

        private ExpressionParser.ExpressionParser ep;

        private ObservableCollection<CalculationVm> calculations;
        public ObservableCollection<CalculationVm> Calculations
        {
            get { return calculations;  }
            set { calculations = value; RaisePropertyChanged(); }
        }

        private string textInput;
        public string TextInput
        {
            get { return textInput; }
            set { textInput = value; RaisePropertyChanged(); }
        }


        private bool isError;
        public bool IsError
        {
            get { return isError; }
            set { isError = value; RaisePropertyChanged(); }
        }

        private bool isFocus;
        public bool IsFocus
        {
            get { return isFocus; }
            set { isFocus = value; RaisePropertyChanged(); }
        }

        //Commands

        private ICommand _inputValue;
        public ICommand InpurValueCommand
        {
            get
            {
                if (_inputValue == null)
                {
                    _inputValue = new RelayCommand((x) => {
                        TextInput += x.ToString();
                        IsFocus = true;
                    });
                }
                return _inputValue;
            }
        }

        private ICommand _clearCommand;
        public ICommand ClearCommand
        {
            get
            {
                if (_clearCommand == null)
                {
                    _clearCommand = new RelayCommand((x) => {
                        TextInput = "";
                        IsFocus = true;
                    });
                }
                return _clearCommand;
            }
        }


        private ICommand _helpCommand;
        public ICommand HelpCommand
        {
            get
            {
                if (_helpCommand == null)
                {
                    _helpCommand = new RelayCommand((x) => {
                        MessageBox.Show("Sie können in den Rechner einen Term eingeben, der gemäß der mathematischen Gesetze berechnet wird. D.h. es gilt Punkt vor Strich, Klammern werden berücksichtigt. Zusätzlich stehen folgende Operatoren zur Verfügung:\nsin()\ncos()\nsqrt() - Wurzel\n^ - Potenzzeichen");
                        IsFocus = true;
                    });
                }
                return _helpCommand;
            }
        }

        private ICommand _calculate;
        public ICommand CalculateCommand
        {
            get
            {
                if (_calculate == null)
                {
                    _calculate = new RelayCommand((x) => {
                        
                        try {
                            IsError = false;
                            ep = new(TextInput, AngleMode.Deg);
                            TextInput = ep.Result.ToString();
                            Calculations.Add(new CalculationVm() { Expression = ep.Expression, Result = ep.Result.ToString() });
                        }
                        catch { IsError = true; }
                        IsFocus = true;
                    });
                }
                return _calculate;
            }
        }

    }

    public class CalculationVm : ViewModelBase
    {
        private string expression;
        public string Expression
        {
            get { return expression; }
            set { expression = value; }
        }

        private string result;
        public string Result
        {
            get { return result; }
            set { result = value; }
        }

        public override string ToString()
        {
            return Expression + " = " + Result;
        }
    }
}
