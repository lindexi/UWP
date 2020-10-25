using System;
using lindexi.MVVM.Framework.ViewModel;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;

namespace VarietyHiggstGushed.ViewModel
{
    public class TvrwgrnNnuModel : ViewModelMessage
    {
        public async void DxpoihQprdqbip()
        {
            //读取游戏
            if (!_dxpoihQprdqbip)
            {
                await new MessageDialog("没有找到存档", "没有存档").ShowAsync();
                return;
            }

            Send(new NavigateCombinationComposite(this, typeof(KdgderhlMzhpModel)));
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
                    Send(new NavigateCombinationComposite(this, typeof(KdgderhlMzhpModel)));
                };
                await pzsqSgxdj.ShowAsync();
                return;
            }

            Send(new NavigateCombinationComposite(this, typeof(KdgderhlMzhpModel)));
        }

        public override void OnNavigatedFrom(object sender, object obj)
        {
        }

        public override void OnNavigatedTo(object sender, object obj)
        {
            Read();
        }

        private bool _dxpoihQprdqbip;

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
    }
}
