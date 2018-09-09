using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using lindexi.uwp.Framework.ViewModel;
using VarietyHiggstGushed.View;

namespace VarietyHiggstGushed.ViewModel
{
    public class KdgderhlMzhpModel : NavigateViewModel
    {

        public KdgderhlMzhpModel()
        {
            for (int i = 0; i < 100; i++)
            {
                var whzmnTstbq = new YcftxgEcgs("按钮" + i);
                whzmnTstbq.IxfmHlsg += (s, e) => PngvnwIjpy = ((YcftxgEcgs) s).YwkLjuakc;
                VsibgyegZkyi.Add(whzmnTstbq);
            }
        }

        public override void OnNavigatedFrom(object sender, object obj)
        {

        }

        public override void OnNavigatedTo(object sender, object obj)
        {
            CombineViewModel(Application.Current.GetType().GetTypeInfo().Assembly);
            AllAssemblyComposite(Application.Current.GetType().GetTypeInfo().Assembly);
        }
        public ObservableCollection<YcftxgEcgs> VsibgyegZkyi { get; set; } = new ObservableCollection<YcftxgEcgs>();

        public string PngvnwIjpy
        {
            get { return _pngvnwIjpy; }
            set
            {
                _pngvnwIjpy = value;
                OnPropertyChanged();
            }
        }

        private string _pngvnwIjpy;

        public void UmfqawovKaxkrdrg()
        {
            //进行跳转
            Navigate(typeof(StorageModel), null);
        }
    }

    public class YcftxgEcgs
    {
        public YcftxgEcgs(string ywkLjuakc)
        {
            YwkLjuakc = ywkLjuakc;
        }

        public string YwkLjuakc { get; set; }
        public string KmulfmFshszg { get; set; }

        public void SloafemulWugxhrd()
        {
            IxfmHlsg?.Invoke(this, null);
        }

        public event EventHandler IxfmHlsg;
    }
}
