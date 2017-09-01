using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using VarietyHiggstGushed.Annotations;

namespace VarietyHiggstGushed.Model
{
    public class Property : INotifyPropertyChanged
    {
        private int _num;
        private double _price;
        private string _name;
        private double _ashliLyverGeraldo;

        public Property(string name, double value)
        {
            Name = name;
            Value = value;
        }

        public Property(string name, string value)
        {
            Name = name;
            Value = int.Parse(value);
        }

        public string Name
        {
            set { _name = value; OnPropertyChanged(); }
            get { return _name; }
        }

        public double Price
        {
            set { _price = value; OnPropertyChanged(); }
            get { return _price; }
        }

        public double Value
        {
            set;
            get;
        }

        public int Num
        {
            set { _num = value; OnPropertyChanged(); }
            get { return _num; }
        }

        public double AshliLyverGeraldo
        {
            get { return _ashliLyverGeraldo; }
            set { _ashliLyverGeraldo = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
