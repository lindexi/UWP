using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using VarietyHiggstGushed.Annotations;

namespace VarietyHiggstGushed.Model
{
    public class JwStorage : INotifyPropertyChanged
    {
        private double _tranStoragePrice;
        private int _transit;
        private int _transitStorage;

        public JwStorage()
        {

        }

        public ObservableCollection<Property> PropertyStorage
        {
            set;
            get;
        } = new ObservableCollection<Property>();

        /// <summary>
        /// 仓库
        /// </summary>
        public int TransitStorage
        {
            set { _transitStorage = value; OnPropertyChanged(); }
            get { return _transitStorage; }
        }

        /// <summary>
        /// 当前
        /// </summary>
        public int Transit
        {
            set { _transit = value; OnPropertyChanged(); }
            get { return _transit; }
        }

        public void NewTransit(int n)
        {
            TransitStorage += n;
        }

        public double TranStoragePrice
        {
            set { _tranStoragePrice = value; OnPropertyChanged(); }
            get { return _tranStoragePrice; }
        }

        /// <summary>
        /// 购买
        /// </summary>
        /// <param name="property"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public void NewProperty(Property property,int n)
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
        /// 卖出
        /// </summary>
        /// <param name="property"></param>
        /// <param name="n"></param>
        public void TisProperty([NotNull] Property property,int n)
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

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}