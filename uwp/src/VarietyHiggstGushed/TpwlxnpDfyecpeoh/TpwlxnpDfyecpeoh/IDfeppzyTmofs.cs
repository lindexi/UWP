using lindexi.uwp.Framework.ViewModel;

namespace TpwlxnpDfyecpeoh
{
    public interface IDfeppzyTmofs
    {
        //修为
        long KtrKvmvvnj { set; get; }

        /// <summary>
        /// 力量
        /// </summary>
        int KhbfhHtuxwwrn { set; get; }

        /// <summary>
        /// 防御
        /// </summary>
        int KahdxouTrifmznz { set; get; }

        /// <summary>
        /// 精神力
        /// </summary>
        int SnmTiet { get; set; }

        /// <summary>
        /// 魔力
        /// </summary>
        int DyjgSjdbgm { set; get; }
    }

    class TdsumTzwok : NotifyProperty, IDfeppzyTmofs
    {
        private long _ktrKvmvvnj = 0;
        private int _khbfhHtuxwwrn = 1;
        private int _kahdxouTrifmznz = 1;
        private int _snmTiet = 1;
        private int _dyjgSjdbgm = 1;

        public TdsumTzwok()
        {
        }

        public long KtrKvmvvnj
        {
            get { return _ktrKvmvvnj; }
            set
            {
                _ktrKvmvvnj = value;
                OnPropertyChanged();
            }
        }

        public int KhbfhHtuxwwrn
        {
            get { return _khbfhHtuxwwrn; }
            set
            {
                _khbfhHtuxwwrn = value;
                OnPropertyChanged();
            }
        }

        public int KahdxouTrifmznz
        {
            get { return _kahdxouTrifmznz; }
            set
            {
                _kahdxouTrifmznz = value;
                OnPropertyChanged();
            }
        }

        public int SnmTiet
        {
            get { return _snmTiet; }
            set
            {
                _snmTiet = value;
                OnPropertyChanged();
            }
        }

        public int DyjgSjdbgm
        {
            get { return _dyjgSjdbgm; }
            set
            {
                _dyjgSjdbgm = value;
                OnPropertyChanged();
            }
        }
    }
}
