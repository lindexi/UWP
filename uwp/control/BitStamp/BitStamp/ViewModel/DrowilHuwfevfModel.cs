using lindexi.MVVM.Framework.ViewModel;
using lindexi.uwp.Framework.ViewModel;

namespace BitStamp.ViewModel
{
    public class DrowilHuwfevfModel : NavigateViewModel
    {
        private bool _senKrobe;

        public Account Account { get; set; }

        private void TadSvc()
        {
            if (!_senKrobe)
            {
                _senKrobe = true;
                NavigateHrbHtlad();
            }
        }

        public void NavigateHrbHtlad()
        {
            Navigate(typeof(HrbHtladModel), Account);
        }

        public void NavigateSaeHqeupq()
        {
            Navigate(typeof(SaeHqeupqModel), Account);
        }

        public override void OnNavigatedFrom(object sender, object obj)
        {

        }

        public override void OnNavigatedTo(object sender, object obj)
        {
            Account = AccoutGoverment.AccountModel.Account;
            TadSvc();
        }
    }
}
