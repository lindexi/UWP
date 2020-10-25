using System;
using System.Collections.ObjectModel;
using System.Linq;
using lindexi.MVVM.Framework.ViewModel;
using lindexi.uwp.Framework;
using VarietyHiggstGushed.Model;

namespace VarietyHiggstGushed.ViewModel
{
    public class StorageModel : ViewModelMessage
    {
        //public AmeriStorage AmeriStorage
        //{
        //    set;
        //    get;
        //}

        public JwStorage JwStorage { get; set; }

        //private async void Read()
        //{
        //    await AccountGoverment.JwAccountGoverment.Read();
        //}

        public ObservableCollection<WqmnygDcxwptivk> PropertyStorage { set; get; } =
            new ObservableCollection<WqmnygDcxwptivk>();

        public int LansheehyBrunaSharon
        {
            get => _lansheehyBrunaSharon;
            set
            {
                _lansheehyBrunaSharon = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     天数
        /// </summary>
        public int PinkieDuchesneGeraldo
        {
            get => AccountGoverment.JwAccountGoverment.JwStorage.PinkieDuchesneGeraldo;
            set
            {
                AccountGoverment.JwAccountGoverment.JwStorage.PinkieDuchesneGeraldo = value;
                OnPropertyChanged();
            }
        }

        public WqmnygDcxwptivk CarloPiperIsaacProperty
        {
            get => _carloPiperIsaacProperty;
            set
            {
                _carloPiperIsaacProperty = value;
                //LansheehyBrunaSharon = _carloPiperIsaacProperty.Num;
                OnPropertyChanged();
            }
        }

        public void NewLansheehyBrunaSharon()
        {
            //买入
            if (CarloPiperIsaacProperty == null)
            {
                LansheehyBrunaSharon = 0;
                return;
            }

            //判断最大可以买入
            //价钱
            var s = (int) Math.Floor(JwStorage.TranStoragePrice / CarloPiperIsaacProperty.Price);
            if (LansheehyBrunaSharon > s)
            {
                LansheehyBrunaSharon = s;
            }

            JwStorage.NewProperty(CarloPiperIsaacProperty, LansheehyBrunaSharon);
            LansheehyBrunaSharon = 0;
        }

        public void AimeeLansheehyBrunaSharon()
        {
            JwStorage.TisProperty(CarloPiperIsaacProperty, LansheehyBrunaSharon);
            LansheehyBrunaSharon = 0;
        }

        public void NewTransit()
        {
            if (JwStorage.TranStoragePrice > 50)
            {
                JwStorage.TranStoragePrice -= 50;
                JwStorage.TransitStorage++;
            }
        }

        public void MerilynPinkieDuchesneGeraldo()
        {
            PropertyStorage.Clear();
            foreach (var temp in JwStorage.PropertyStorage)
            {
                //创建临时价格
                temp.Price = _random.Next(80, 120) * temp.Value / 100;
                if (_random.Next(JwStorage.PropertyStorage.Count) <
                    JwStorage.PropertyStorage.Count / 2 - PropertyStorage.Count)
                {
                    PropertyStorage.Add(temp);
                }
            }
            //下天


            PinkieDuchesneGeraldo++;
        }

        public override void OnNavigatedFrom(object sender, object obj)
        {
            FjyhtrOcbhzjwi.Fhnazmoul.RemoveSuccessor(_ajuvqrDqsoljna);
        }

        public override void OnNavigatedTo(object sender, object obj)
        {
            JwStorage = AccountGoverment.JwAccountGoverment.JwStorage;
            MerilynPinkieDuchesneGeraldo();
            PinkieDuchesneGeraldo--;

            _ajuvqrDqsoljna = _ajuvqrDqsoljna ?? new AjuvqrDqsoljna(async fjyhtrOcbhzjwi =>
            {
                if (fjyhtrOcbhzjwi.Handle)
                {
                    return;
                }

                fjyhtrOcbhzjwi.Handle = true;
                await AccountGoverment.JwAccountGoverment.Storage();
                //返回上一层
                Send(new BackTvvxwlwIlibbcpMessage(this));
            });
            FjyhtrOcbhzjwi.Fhnazmoul.AddSuccessor(_ajuvqrDqsoljna);
        }

        private AjuvqrDqsoljna _ajuvqrDqsoljna;
        private WqmnygDcxwptivk _carloPiperIsaacProperty;
        private int _lansheehyBrunaSharon;
        private int _pinkieDuchesneGeraldo = 1;

        private Random _random = new Random();
    }

    public class BackTvvxwlwIlibbcpMessage : Message
    {
        public BackTvvxwlwIlibbcpMessage(ViewModelBase source) : base(source)
        {
            Goal = new PredicateInheritViewModel(typeof(NavigateViewModel));
        }
    }

    public class BackTvvxwlwIlibbcpComposite : Composite
    {
        public BackTvvxwlwIlibbcpComposite()
        {
            PredicateMessage = new TypePredicateMessage(typeof(BackTvvxwlwIlibbcpMessage));
        }

        public override void Run(ViewModelBase source, IMessage message)
        {
            var viewModel = (NavigateViewModel) source;
            var type = ((NavigateFrame) viewModel.Content).Frame.BackStack.LastOrDefault()?.SourcePageType;
            var id = viewModel.ViewModelPage.FirstOrDefault(temp =>
                Equals(temp.Page.PlatformPage, type))?.Key;
            if (!string.IsNullOrEmpty(id))
            {
                viewModel.Navigate(id, null);
            }
        }
    }
}
