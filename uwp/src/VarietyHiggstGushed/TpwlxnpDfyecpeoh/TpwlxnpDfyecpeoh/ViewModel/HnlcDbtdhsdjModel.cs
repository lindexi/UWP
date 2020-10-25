using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lindexi.uwp.Framework.ViewModel;

namespace TpwlxnpDfyecpeoh.ViewModel
{
    public class HnlcDbtdhsdjModel : ViewModelMessage
    {
        /// <summary>
        /// 获取设置技能 
        /// </summary>
        public ObservableCollection<DexqurhctSjyfozae> DexqurhctSjyfozae
        {
            set
            {
                _dexqurhctSjyfozae = value;
                OnPropertyChanged();
            }
            get => _dexqurhctSjyfozae;
        }

        private ObservableCollection<DexqurhctSjyfozae> _dexqurhctSjyfozae;

        /// <summary>
        /// 获取设置 人物 
        /// </summary>
        public IDfeppzyTmofs KppnuhKxkpxdee
        {
            set
            {
                _kppnuhKxkpxdee = value;
                OnPropertyChanged();
            }
            get => _kppnuhKxkpxdee;
        }

        private IDfeppzyTmofs _kppnuhKxkpxdee;

        public HnlcDbtdhsdjModel()
        {
        }

        public override void OnNavigatedFrom(object sender, object obj)
        {

        }

        public override void OnNavigatedTo(object sender, object obj)
        {
            KppnuhKxkpxdee = new TdsumTzwok();
            var hisjfnnzSqsbtuuqq = new HisjfnnzSqsbtuuqq(KppnuhKxkpxdee);

            DexqurhctSjyfozae = new ObservableCollection<DexqurhctSjyfozae>()
            {
                hisjfnnzSqsbtuuqq,
            };
        }

        public void KdfoeDoct(DexqurhctSjyfozae dexqurhctSjyfozae)
        {
            if (KppnuhKxkpxdee.KtrKvmvvnj >= dexqurhctSjyfozae.DmyikbmfDeb)
            {
                KppnuhKxkpxdee.KtrKvmvvnj -= (long) Math.Ceiling(dexqurhctSjyfozae.DmyikbmfDeb);
                dexqurhctSjyfozae.DqqTsb();
            }
            else
            {

            }
        }
    }
}
