using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using lindexi.uwp.Framework.ViewModel;
using lindexi.uwp.Framework.ViewModel.MasterDetail;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace Framework.ViewModel
{
    [ViewModel]
    public class CodeStorageModel : NavigateViewModel, IReceiveMessage
    {
        public CodeStorageModel()
        {
            DetailMaster = new DetailMasterModel();
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                AppViewBackButtonVisibility.Visible;

            var applacationAssembly = Application.Current.GetType().GetTypeInfo().Assembly;
            foreach (var temp in applacationAssembly.DefinedTypes
                .Where(temp => temp.CustomAttributes.Any(t => t.AttributeType == typeof(CodeStorageAttribute))))
            {
                var viewmodel = temp.AsType().GetConstructor(Type.EmptyTypes).Invoke(null);
                Type page = null;
                try
                {
                    page =
                        applacationAssembly.DefinedTypes.First(
                            t => t.Name.Replace("Page", "") == temp.Name.Replace("Model", "")).AsType();
                }
                catch
                {
                    //InvalidOperationException
                    //提醒没有page
                    //throw new Exception("没有"+temp.Name.Replace("Model","")+"Page");
                }

                ViewModel.Add(new ViewModelPage(viewmodel as ViewModelBase, page));
            }

            foreach (var temp in ViewModel.Where(temp => temp.ViewModel is ISendMessage))
            {
                ((ISendMessage) temp.ViewModel).SendMessageHandler += (s, e) =>
                {
                    ReceiveMessage(this, e);
                };
            }
        }

        public DetailMasterModel DetailMaster
        {
            set;
            get;
        }

        //public ViewModelBase this[string str]
        //{
        //    get
        //    {
        //        foreach (var temp in ViewModel)
        //        {
        //            if (temp.Key == str)
        //            {
        //                return temp.ViewModel;
        //            }
        //        }
        //        return null;
        //    }
        //}


        public override void OnNavigatedFrom(object sender, object obj)
        {
        }

        public override void OnNavigatedTo(object sender, object obj)
        {
            DetailMaster.Narrow();
            foreach (var temp in ViewModel)
            {
                temp.ViewModel.OnNavigatedTo(this, null);
            }
        }

        //public void ReceiveMessage(Message message)
        //{
        //    if (message.Key == "点击列表")
        //    {
        //        DetailMaster.MasterClick();
        //    }
        //    foreach (var temp in Framework.ViewModel)
        //    {
        //        if (temp.Key == message.Goal)
        //        {
        //            var receive = temp.ViewModel as IReceiveMessage;
        //            receive?.ReceiveMessage(this,message);
        //        }
        //    }
        //}
    }
}
