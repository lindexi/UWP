using System;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using lindexi.uwp.Framework.ViewModel;

namespace VarietyHiggstGushed.ViewModel
{
    public class TvrwgrnNnuModel : ViewModelMessage
    {
        private async void Read()
        {
            if (await AccountGoverment.JwAccountGoverment.ReadJwStorage())
            {
                //不是第一次使用
                _dxpoihQprdqbip = true;
            }
            else
            {
                await AccountGoverment.JwAccountGoverment.Read();
            }
        }

        private bool _dxpoihQprdqbip = false;

        public async void DxpoihQprdqbip()
        {
            //读取游戏
            if (!_dxpoihQprdqbip)
            {
                await new MessageDialog("没有找到存档", "没有存档").ShowAsync();
                return;
            }
            Send(new NavigateCombinationComposite(this, typeof(StorageModel)));
        }


        public async void AdraqbqhUgtwg()
        {
            if (_dxpoihQprdqbip)
            {
                var pzsqSgxdj = new ContentDialog
                {
                    Title = "已经存在存档，是否覆盖",
                    PrimaryButtonText = "确定",
                    SecondaryButtonText = "取消"
                };

                pzsqSgxdj.PrimaryButtonClick += async (sender, args) =>
                {
                    await AccountGoverment.JwAccountGoverment.Read();
                    Send(new NavigateCombinationComposite(this, typeof(StorageModel)));
                };
                await pzsqSgxdj.ShowAsync();
                return;
            }
            Send(new NavigateCombinationComposite(this, typeof(StorageModel)));
        }

        public override void OnNavigatedFrom(object sender, object obj)
        {

        }

        public override void OnNavigatedTo(object sender, object obj)
        {
            Read();
        }
    }

    public class NavigateCombinationComposite : CombinationComposite<NavigateViewModel>
    {
        public NavigateCombinationComposite(ViewModelBase source, Type view, object paramter = null) : base(source)
        {
            _run = viewModel =>
            {
                viewModel.Navigate(view, paramter);
            };
        }
    }
}