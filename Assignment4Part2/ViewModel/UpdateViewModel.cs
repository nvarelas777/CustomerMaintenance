using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Windows.Input;
using System.Data.Entity;
using System.Collections.ObjectModel;
using Assignment4Part2.Views;
using Assignment4Part2.Model;
using Assignment4Part2.Messages;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using System.Data.Entity.Infrastructure;
using System.Data;

namespace Assignment4Part2.ViewModel
{
    public class UpdateViewModel : ViewModelBase
    {
        private string firstTextBox;
        private string secondTextBox;
        private string thirdTextBox;
        private string fourthTextBox;
        private ObservableCollection<State> states;
        private State selectedMember;
        private State selectedState;
        private string stateCode;
        private string stateName;
        public RelayCommand UpdateCommand { get; set; }
        public RelayCommand<Window> CloseWindowCommand { get; private set; }
        private Customer oldcust;

        public UpdateViewModel()
        {
            Messenger.Default.Register<ViewModelMessage>(this, OnReceiveOldCust);
            this.CloseWindowCommand = new RelayCommand<Window>(this.CloseWindow);
            {
                var states = (from state in MMABooksEntity.mmaBooks.States orderby state.StateName select state).ToList();   
                States = new ObservableCollection<State>(states);
            }
            UpdateCommand = new RelayCommand(() =>
            {
                Customer customer = new Customer();
                this.PutCustomerData(customer);
                var original = MMABooksEntity.mmaBooks.Customers.Find(oldcust.CustomerID);
                if (original != null)
                {
                    try
                    {
                        customer.CustomerID = oldcust.CustomerID;
                        MMABooksEntity.mmaBooks.Entry(original).CurrentValues.SetValues(customer);
                        MMABooksEntity.mmaBooks.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        ex.Entries.Single().Reload();
                        if (MMABooksEntity.mmaBooks.Entry(original).State == EntityState.Detached)
                        {
                            MessageBox.Show("Another user has deleted " + "that customer.", "Concurrency Error");
                        }
                        else
                        {
                            MessageBox.Show("Another user has updated " + "that customer.", "Concurrency Error");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, ex.GetType().ToString());
                    }
                }
                this.SendCustomerData(customer);
                Messenger.Default.Send(new NotificationMessage("Customer Added!"));
                Messenger.Default.Send(customer, "edit");
            });
        }

        private void OnReceiveOldCust(ViewModelMessage obj)
        {
            if (obj != null && obj.Text.Equals("Update"))
            {
                oldcust = obj.cust;
                StateName = obj.SName;
                DisplayCustomer(oldcust, StateName);
            }
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

        public State SelectedState
        {
            get
            {
                return selectedState;
            }
            set
            {
                selectedState = value;
                RaisePropertyChanged("SelectedState");
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

        private void SendCustomerData(Customer customer)
        {
            customer.Name = FirstTextBox;
            customer.Address = SecondTextBox;
            customer.City = ThirdTextBox;
            customer.ZipCode = FourthTextBox;
            customer.State = SelectedMember.StateName;
        }

        private void DisplayCustomer(Customer selectedCustomer, String State)
        {
            FirstTextBox = selectedCustomer.Name;
            SecondTextBox = selectedCustomer.Address;
            ThirdTextBox = selectedCustomer.City;
            SelectedState = States[5];
            FourthTextBox = selectedCustomer.ZipCode;
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

        public string StateName
        {
            get
            {
                return stateName;
            }
            set
            {
                stateName = value;
                RaisePropertyChanged("StateName");
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
