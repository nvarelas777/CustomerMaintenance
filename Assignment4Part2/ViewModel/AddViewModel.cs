using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.ObjectModel;
using Assignment4Part2.Model;
using Assignment4Part2.Messages;


namespace Assignment4Part2.ViewModel
{
    public class AddViewModel : ViewModelBase
    {
        private string firstTextBox;
        private string secondTextBox;
        private string thirdTextBox;
        private string fourthTextBox;
        private ObservableCollection<State> states;
        private State selectedMember;
        private string stateCode;
        public RelayCommand AcceptCommand { get; set; }
        public RelayCommand<Window> CloseWindowCommand { get; private set; }
        

        public AddViewModel()
        {
            this.CloseWindowCommand = new RelayCommand<Window>(this.CloseWindow);
            {
                var states = (from state in MMABooksEntity.mmaBooks.States orderby state.StateName select state).ToList();    
                States = new ObservableCollection<State>(states);     
            }

            AcceptCommand = new RelayCommand(() =>
            {
                if (IsValidData())
                {
                    Customer customer = new Customer();                   
                    this.PutCustomerData(customer);
                    try
                    {
                        customer = MMABooksEntity.mmaBooks.Customers.Add(customer);
                        MMABooksEntity.mmaBooks.SaveChanges();
                        MessageBox.Show("Customer Saved!");
                        Messenger.Default.Send(customer, "add");
                        
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, ex.GetType().ToString());
                    }
                }
                });
            }

        public ObservableCollection<State> States
        {
            get
            {
                return states;
            }
            set
            {
                states = value;
                RaisePropertyChanged("States");
            }

        }

        public State SelectedMember
        {
            get
            {
                return selectedMember;
            }
            set
            {
                selectedMember = value;
                RaisePropertyChanged("SelectedMember");
            }
        }

        private void PutCustomerData(Customer customer)
        {
            customer.Name = FirstTextBox;
            customer.Address = SecondTextBox;
            customer.City = ThirdTextBox;
            customer.ZipCode = FourthTextBox;
            customer.State = SelectedMember.StateCode;
        }

        private bool IsValidData()
        {
            return Validator.isPresent(FirstTextBox) &&
            Validator.isPresent(SecondTextBox) &&
            Validator.isPresent(ThirdTextBox) &&
            Validator.isPresent(FourthTextBox) &&
            Validator.isPresent(SelectedMember.StateCode) &&
            Validator.IsInt32(FourthTextBox);
        }

        public string StateCode
        {
            get
            {
                return stateCode;
            }
            set
            {
                stateCode = value;
                RaisePropertyChanged("StateCode");
            }
        }

        public string FirstTextBox
        {
            get
            {
                return firstTextBox;
            }
            set
            {
                firstTextBox = value;
                RaisePropertyChanged("FirstTextBox");
            }
        }

        public string SecondTextBox
        {
            get
            {
                return secondTextBox;
            }
            set
            {
                secondTextBox = value;
                RaisePropertyChanged("SecondTextBox");
            }
        }

        public string ThirdTextBox
        {
            get
            {
                return thirdTextBox;
            }
            set
            {
                thirdTextBox = value;
                RaisePropertyChanged("ThirdTextBox");
            }
        }

        public string FourthTextBox
        {
            get
            {
                return fourthTextBox;
            }
            set
            {
                fourthTextBox = value;
                RaisePropertyChanged("FourthTextBox");
            }
        }

        private void CloseWindow(Window win)
        {
            if (win != null)
            {
                win.Close();
            }
        }
    }
}
