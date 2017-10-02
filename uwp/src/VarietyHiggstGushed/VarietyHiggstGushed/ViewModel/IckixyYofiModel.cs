using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using lindexi.uwp.Framework.ViewModel;
using VarietyHiggstGushed.Model;

namespace VarietyHiggstGushed.ViewModel
{
    public class IckixyYofiModel : NavigateViewModel
    {
        public IckixyYofiModel()
        {
        }

        public event EventHandler<string> LyfxkdxmSzjd;

        public override void OnNavigatedFrom(object sender, object obj)
        {

        }

        public override void OnNavigatedTo(object sender, object obj)
        {
            CombineViewModel(Application.Current.GetType().GetTypeInfo().Assembly);
            AllAssemblyComposite(Application.Current.GetType().GetTypeInfo().Assembly);
            //先到登陆
            Navigate(typeof(TvrwgrnNnuModel), null);
        }
    }

    public interface ILyfxkdxmSzjd
    {
        /// <summary>
        /// 通知
        /// </summary>
        event EventHandler<string> LyfxkdxmSzjd;

    }
}