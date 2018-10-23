using System;
using System.Collections.ObjectModel;
using System.Reflection;
using Windows.UI.Xaml;
using lindexi.MVVM.Framework.ViewModel;
using lindexi.uwp.Framework;

namespace VarietyHiggstGushed.ViewModel
{
    public class KdgderhlMzhpModel : NavigateViewModel
    {
        public KdgderhlMzhpModel()
        {
            for (var i = 0; i < 100; i++)
            {
                var whzmnTstbq = new YcftxgEcgs("按钮" + i);
                whzmnTstbq.IxfmHlsg += (s, e) => PngvnwIjpy = ((YcftxgEcgs) s).YwkLjuakc;
                VsibgyegZkyi.Add(whzmnTstbq);
            }
        }

        public ObservableCollection<YcftxgEcgs> VsibgyegZkyi { get; set; } = new ObservableCollection<YcftxgEcgs>();

        public string PngvnwIjpy
        {
            get => _pngvnwIjpy;
            set
            {
                _pngvnwIjpy = value;
                OnPropertyChanged();
            }
        }

        public override void OnNavigatedFrom(object sender, object obj)
        {
        }

        public override void OnNavigatedTo(object sender, object obj)
        {
            this.CombineViewModel(Application.Current.GetType().GetTypeInfo().Assembly);
            AllAssemblyComposite(Application.Current.GetType().GetTypeInfo().Assembly);
        }

        public void UmfqawovKaxkrdrg()
        {
            //进行跳转
            Navigate(typeof(StorageModel), null);
        }

        private string _pngvnwIjpy;
    }

    public class YcftxgEcgs
    {
        public YcftxgEcgs(string ywkLjuakc)
        {
            YwkLjuakc = ywkLjuakc;
        }

        public string YwkLjuakc { get; set; }
        public string KmulfmFshszg { get; set; }

        public event EventHandler IxfmHlsg;

        public void SloafemulWugxhrd()
        {
            IxfmHlsg?.Invoke(this, null);
        }
    }
}