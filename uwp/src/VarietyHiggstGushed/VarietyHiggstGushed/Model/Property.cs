using System.ComponentModel;
using System.Runtime.CompilerServices;
using VarietyHiggstGushed.Annotations;

namespace VarietyHiggstGushed.Model
{
    /// <summary>
    ///     用于给商品显示运行包括价格
    /// </summary>
    public class WqmnygDcxwptivk : Property
    {
        public WqmnygDcxwptivk()
        {
        }

        public WqmnygDcxwptivk(Property property)
        {
            Name = property.Name;
            Num = property.Num;
            Value = property.Value; //价值
        }

        public double Price
        {
            set
            {
                _price = value;
                OnPropertyChanged();
            }
            get => _price;
        }

        public double AshliLyverGeraldo
        {
            get => _ashliLyverGeraldo;
            set
            {
                _ashliLyverGeraldo = value;
                OnPropertyChanged();
            }
        }

        private double _ashliLyverGeraldo; //买入
        private double _price;
    }


    public class Property : INotifyPropertyChanged
    {
        public Property()
        {
        }

        public Property(string name, double value)
        {
            Name = name;
            Value = value;
        }

        public Property(string name, string value)
        {
            Name = name;
            Value = double.Parse(value);
        }

        public string Name
        {
            set
            {
                _name = value;
                OnPropertyChanged();
            }
            get => _name;
        }

        public double Value { set; get; }

        public int Num
        {
            set
            {
                _num = value;
                OnPropertyChanged();
            }
            get => _num;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private string _name;
        private int _num;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
