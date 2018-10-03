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

    public class MainViewModel : ViewModelBase
    {
        private string idTextBox;
        private string nameText;
        private string addressText;
        private string cityText;
        private string stateText;
        private string zipText;
        private string statedata;
        private int customerID { get; set; }
        public ICommand GetCustomerCommand { get; private set; }
        public ICommand AddUsersCommand { get; private set; }
        public ICommand DeleteUsersCommand { get; private set; }
        public ICommand UpdateUsersCommand { get; private set; }
        private Customer selectedCustomer;
        public RelayCommand<Window> CloseWindowCommand { get; private set; }


        public MainViewModel()
        {
            selectedCustomer = new Customer();
            this.CloseWindowCommand = new RelayCommand<Window>(this.CloseWindow);
            AddUsersCommand = new RelayCommand(addUsers);
            UpdateUsersCommand = new RelayCommand(updateUsers);
            GetCustomerCommand = new RelayCommand(() =>
            {              
                try
                {
                    customerID = Convert.ToInt32(IdTextBox);
                    var data = (from customer in MMABooksEntity.mmaBooks.Customers
                                where customer.CustomerID == customerID
                                select customer).FirstOrDefault<Customer>();
                    statedata = (from customer in MMABooksEntity.mmaBooks.Customers
                                       join state in MMABooksEntity.mmaBooks.States
                                       on customer.CustomerID equals customerID
                                       select state.StateName).First().ToString();
                    selectedCustomer = data;
                    this.DisplayCustomer(statedata);
                    var viewModelMessage = new ViewModelMessage()
                    {
                        SName = statedata,
                        Text = "StateName"
                    };
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    ex.Entries.Single().Reload();
                    if (MMABooksEntity.mmaBooks.Entry(selectedCustomer).State == EntityState.Detached)
                    {
                        MessageBox.Show("Another user has deleted " + "that customer.", "Concurrency Error");
                        //customerID = " ";
                    }
                    else
                    {
                        MessageBox.Show("Another user has updated " + "that customer.", "Concurrency Error");
                        DisplayCustomer(null);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, ex.GetType().ToString());
                    this.ClearControls();
                }
            });

            DeleteUsersCommand = new RelayCommand(() =>
            {
                try
                {
                    customerID = Convert.ToInt32(IdTextBox);
                    var data = (from customer in MMABooksEntity.mmaBooks.Customers
                                    //join state in MMABooksEntity.mmaBooks.States
                                    //on customer.State equals state.StateCode
                                where customer.CustomerID == customerID
                                select customer).FirstOrDefault<Customer>();
                    selectedCustomer = data;
                    MMABooksEntity.mmaBooks.Customers.Remove(selectedCustomer);
                    MMABooksEntity.mmaBooks.SaveChanges();
                    MessageBox.Show("Customer Removed!");
                    this.ClearControls();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    ex.Entries.Single().Reload();
                    if (MMABooksEntity.mmaBooks.Entry(selectedCustomer).State == EntityState.Detached)
                    {
                        MessageBox.Show("Another user has deleted " + "that customer.", "Concurrency Error");
                        //customerID = " ";
                    }
                    else
                    {
                        MessageBox.Show("Another user has updated " + "that customer.", "Concurrency Error");
                        DisplayCustomer(null);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, ex.GetType().ToString());
                }
            });

            Messenger.Default.Register<Customer>(this, "add", (customer) =>
            {               
                try
                {
                    selectedCustomer = customer;
                    this.DisplayCustomer(null);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, ex.GetType().ToString());
                }
            });

            Messenger.Default.Register<Customer>(this, "edit", (customer) =>
            {
                try
                {
                    selectedCustomer = customer;
                    this.DisplayCustomer(null);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, ex.GetType().ToString());
                }
            });
        }

        public void addUsers()
        {
            AddView addview = new AddView();
            addview.ShowDialog();
        }

        public void updateUsers()
        {
            UpdateView updateview = new UpdateView();
            if (selectedCustomer == null)
            {
                MessageBox.Show("No customer selected");
            }
            else
            {
                var viewModelMessage = new ViewModelMessage()
                {
                    cust = selectedCustomer,
                    Text = "Update",
                    SName = statedata
                };
                Messenger.Default.Send(viewModelMessage);
                updateview.ShowDialog();
            }
        }

        private void DisplayCustomer(string statename)
        {
            NameText = selectedCustomer.Name;
            AddressText = selectedCustomer.Address;
            CityText = selectedCustomer.City;
            StateText = statename;
            ZipText = selectedCustomer.ZipCode;
        }

        private void ClearControls()
        {
            IdTextBox = "";
            NameText = "";
            AddressText = "";
            CityText = "";
            StateText = "";
            ZipText = "";
        }


        public string IdTextBox
        {
            get
            {
                return idTextBox;
            }
            set
            {
                idTextBox = value;
                RaisePropertyChanged("IdTextBox");
            }
        }

        public string NameText
        {
            get
            {
                return nameText;
            }
            set
            {
                nameText = value;
                RaisePropertyChanged("NameText");
            }
        }

        public string AddressText
        {
            get
            {
                return addressText;
            }
            set
            {
                addressText = value;
                RaisePropertyChanged("AddressText");
            }
        }

        public string CityText
        {
            get
            {
                return cityText;
            }
            set
            {
                cityText = value;
                RaisePropertyChanged("CityText");
            }
        }

        public string StateText
        {
            get
            {
                return stateText;
            }
            set
            {
                stateText = value;
                RaisePropertyChanged("StateText");
            }
        }

        public string ZipText
        {
            get
            {
                return zipText;
            }
            set
            {
                zipText = value;
                RaisePropertyChanged("ZipText");
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