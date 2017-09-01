using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Storage;
using Windows.UI.Core;
using VarietyHiggstGushed.Model;

namespace VarietyHiggstGushed.ViewModel
{
    public class AccountGoverment
    {
        public AccountGoverment()
        {
            JwAccountGoverment = this;
        }

        public Account Account
        {
            set;
            get;
        }

        public async Task Read()
        {
            JwStorage.TranStoragePrice = 100;
            JwStorage.TransitStorage = 100;

            var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(
                "ms-appx:///PropertyStorage.txt"));
            var str = (await FileIO.ReadTextAsync(file)).Split('\n');
            List<Property> propertyStorage = new List<Property>();
            for (int i = 0; i < str.Length; i++)
            {
                propertyStorage.Add(new Property(str[i], str[i + 1]));
                i++;
            }
            propertyStorage.Sort((a, b) => a.Value.CompareTo(b.Value));

            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                           () =>
                           {
                               JwStorage.PropertyStorage.Clear();
                               foreach (var temp in propertyStorage)
                               {
                                   JwStorage.PropertyStorage.Add(temp);
                               }
                           });
        }

        public JwStorage JwStorage { get; set; } = new JwStorage();

        private static AccountGoverment _accountGoverment;

        public static AccountGoverment JwAccountGoverment
        {
            set
            {
                _accountGoverment = value;
            }
            get
            {
                return _accountGoverment ?? (_accountGoverment = new AccountGoverment());
            }
        }
    }
}