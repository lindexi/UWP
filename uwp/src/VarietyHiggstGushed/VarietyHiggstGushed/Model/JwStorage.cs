using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using VarietyHiggstGushed.Annotations;

namespace VarietyHiggstGushed.Model
{
    public class JwStorage : INotifyPropertyChanged
    {
        /// <summary>
        ///     天数
        /// </summary>
        public int PinkieDuchesneGeraldo { get; set; } = 1;

        public ObservableCollection<WqmnygDcxwptivk> PropertyStorage { set; get; } =
            new ObservableCollection<WqmnygDcxwptivk>();

        /// <summary>
        ///     仓库
        /// </summary>
        public int TransitStorage
        {
            set
            {
                _transitStorage = value;
                OnPropertyChanged();
            }
            get => _transitStorage;
        }

        /// <summary>
        ///     当前
        /// </summary>
        public int Transit
        {
            set
            {
                _transit = value;
                OnPropertyChanged();
            }
            get => _transit;
        }

        public double TranStoragePrice
        {
            set
            {
                _tranStoragePrice = value;
                OnPropertyChanged();
            }
            get => _tranStoragePrice;
        }

        public void NewTransit(int n)
        {
            TransitStorage += n;
        }

        /// <summary>
        ///     购买
        /// </summary>
        /// <param name="property"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public void NewProperty(WqmnygDcxwptivk property, int n)
        {
            if (property == null)
            {
                return;
            }

            var s = TransitStorage - Transit;
            if (n > s)
            {
                n = s;
            }

            if (n == 0)
            {
                return;
            }

            Transit += n;
            TranStoragePrice -= n * property.Price;

            var temp = property.AshliLyverGeraldo * property.Num;
            temp += n * property.Price;

            property.Num += n;

            property.AshliLyverGeraldo = temp / property.Num;
        }

        /// <summary>
        ///     卖出
        /// </summary>
        /// <param name="property"></param>
        /// <param name="n"></param>
        public void TisProperty([NotNull] Property property, int n)
        {
            if (property == null)
            {
                return;
            }

            if (n > property.Num)
            {
                n = property.Num;
            }

            property.Num -= n;
            Transit -= n;
            TranStoragePrice += n * property.Value;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private int _transit;
        private int _transitStorage;
        private double _tranStoragePrice;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
