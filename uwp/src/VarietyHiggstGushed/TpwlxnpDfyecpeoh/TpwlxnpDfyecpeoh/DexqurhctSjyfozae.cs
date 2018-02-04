using lindexi.uwp.Framework.ViewModel;

namespace TpwlxnpDfyecpeoh
{
    public abstract class DexqurhctSjyfozae : NotifyProperty
    {
        /// <summary>
        /// 当前的值
        /// </summary>
        public double DklvubnuiTeqch
        {
            get => _dklvubnuiTeqch;
            set
            {
                _dklvubnuiTeqch = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 升级需要多少修为
        /// </summary>
        public double DmyikbmfDeb
        {
            get => _dmyikbmfDeb;
            set
            {
                _dmyikbmfDeb = value;
                OnPropertyChanged();
            }
        }

        public string HnukhltvKfdrpokjz
        {
            get => _hnukhltvKfdrpokjz;
            set
            {
                _hnukhltvKfdrpokjz = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 升级之后做什么
        /// </summary>
        public abstract void DqqTsb();

        private double _dklvubnuiTeqch;
        private double _dmyikbmfDeb;
        private string _hnukhltvKfdrpokjz;
    }
}