using lindexi.uwp.Framework.ViewModel;

namespace VarietyHiggstGushed.ViewModel
{
    public class TvrwgrnNnuModel : ViewModelMessage
    {
        private async void Read()
        {
            if (await AccountGoverment.JwAccountGoverment.ReadJwStorage())
            {

            }
            else
            {
                await AccountGoverment.JwAccountGoverment.Read();
            }
        }



        public override void OnNavigatedFrom(object sender, object obj)
        {

        }

        public override void OnNavigatedTo(object sender, object obj)
        {
            Read();
        }
    }
}