using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;

namespace Assignment4Part2.Model
{
    public class State : ObservableObject
    {
        private string statename;
        private string stateCode;
        public String Name { get; set; }
        public String Code { get; set; }

        public string StateName
        {
            get
            {
                return statename;
            }
            set
            {
                Set<string>(() => this.StateName, ref statename, value);
            }
        }

        public string StateCode
        {
            get
            {
                return stateCode;
            }
            set
            {
                Set<string>(() => this.StateCode, ref stateCode, value);
            }
        }
    }
}
