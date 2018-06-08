#if false


using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using Windows.Storage;
using Newtonsoft.Json;
#if WINDOWS_UWP
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
#elif wpf
using System.Windows.Controls;

#endif

namespace lindexi.uwp.Framework.ViewModel
{
    /// <summary>
    /// 用于存放配置
    /// </summary>
    public class Account
    {

    }

    public interface IAccount
    {
        Task Read(StorageFolder folder);
        Task Write();
    }

    public class Foo : Account, IAccount
    {
        public async Task Read(StorageFolder folder)
        {
            await Task.Delay(100);
        }

        public async Task Write()
        {
            await Task.Delay(100);
        }
    }


    public static class AccountGoverment
    {
        public static async Task Read()
        {
            //反射获得程序集内所有的 account
            List<Type> account = new List<Type>();

            //是否继承自己读写
            foreach (var temp in account)
            {
                _accounts[temp] = await Read(temp);
            }
        }

        private static async Task<Account> Read(Type type)
        {
            var account = (Account) type.GetConstructor(Type.EmptyTypes).Invoke(null);
            if (Folder == null)
            {
                var folder = ApplicationData.Current.RoamingFolder;
                string str = "account";
                try
                {
                    folder = await folder.GetFolderAsync(str);
                    Folder = folder;
                }
                catch (FileNotFoundException e)
                {
                    Console.WriteLine(e);
                }
            }

            if (Folder != null)
            {
                if (account is IAccount temp)
                {
                    await temp.Read(Folder);
                    return account;
                }
                else
                {
                    var str = type.Name;
                    try
                    {
                        var file = await Folder.GetFileAsync(str);
                        str = await FileIO.ReadTextAsync(file);
                        account = (Account) JsonConvert.DeserializeObject(str, type);
                        return account;
                    }
                    catch (FileNotFoundException e)
                    {
                        Console.WriteLine(e);
                    }
                }
            }

            return account;
        }

        /// <summary>
        /// 获取文件夹
        /// </summary>
        /// <returns></returns>
        private static async Task<StorageFolder> AccountFolder()
        {
            var folder = ApplicationData.Current.RoamingFolder;
            string str = "account";
            try
            {
                folder = await folder.GetFolderAsync(str);
                return folder;
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        private static StorageFolder Folder { get; set; }

        public static void Storage()
        {

        }



        private static Dictionary<Type, Account> _accounts = new Dictionary<Type, Account>();
    }


    [ViewModel]
    public class AModel : ViewModelBase
    {
        public string Name { get; set; } = "csdn";

        public override void OnNavigatedFrom(object sender, object obj)
        {
            return;
        }

        public override void OnNavigatedTo(object sender, object obj)
        {
        }
    }

    [ViewModel]
    public class LinModel : ViewModelBase
    {
        public LinModel()
        {
        }

        public override void OnNavigatedFrom(object sender, object obj)
        {
        }

        public override void OnNavigatedTo(object sender, object obj)
        {
        }
    }

    public class ViewModel : NavigateViewModel
    {
        public ViewModel()
        {
            View = this;
        }

        public AModel AModel { set; get; }

        public LinModel LinModel { set; get; }

        //public CodeStorageModel CodeStorageModel
        //{
        //    set;
        //    get;
        //}

        public Visibility FrameVisibility
        {
            set
            {
                _frameVisibility = value;
                OnPropertyChanged();
            }
            get { return _frameVisibility; }
        }

        public ViewModel View { set; get; }

        public override void OnNavigatedFrom(object sender, object obj)
        {
        }

        public override void OnNavigatedTo(object sender, object obj)
        {
            FrameVisibility = Visibility.Collapsed;
            Content = (Frame) obj;
#if NOGUI
#else
            //Content.Navigate(typeof(SplashPage));
#endif
            if (ViewModel == null)
            {
                ViewModel = new List<ViewModelPage>();
                //加载所有ViewModel
                var applacationAssembly = Application.Current.GetType().GetTypeInfo().Assembly;

                //CodeStorageModel = new CodeStorageModel();
                //ViewModel.Add(new ViewModelPage(CodeStorageModel, typeof(MasterDetailPage))
                //);
                foreach (
                    var temp in applacationAssembly.DefinedTypes.Where(temp => temp.IsSubclassOf(typeof(ViewModelBase)))
                )
                {
                    ViewModel.Add(new ViewModelPage(temp.AsType()));
                }

                foreach (var temp in applacationAssembly.DefinedTypes.Where(temp => temp.IsSubclassOf(typeof(Page))))
                {
                    //获取特性，特性有包含ViewModel
                    var p = temp.GetCustomAttribute<ViewModelAttribute>();

                    var viewmodel = ViewModel.FirstOrDefault(t => t.Equals(p?.ViewModel));
                    if (viewmodel != null)
                    {
                        viewmodel.Page = temp.AsType();
                    }
                }
            }

            FrameVisibility = Visibility.Visible;
            Navigate(typeof(AModel), null);
        }

        public void NavigateToList()
        {
            //Navigate(typeof(CodeStorageModel), null);
        }

        public void NavigateToInfo()
        {
        }

        public void NavigateToAccount()
        {
        }

        private Visibility _frameVisibility;
    }
}
#endif
