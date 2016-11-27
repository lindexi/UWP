using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Button.Control;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace Button
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void ProgressButton_OnClick(object sender, ProgressButton.ProgressButtonEventArgs e)
        {
            await Task.Delay(1000);
            e.OnFinished.Invoke();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {

            Data data = new Data()
            {
                Name = "2"
            };

            var serializer = new DataContractJsonSerializer(typeof(Data));

            string str = null;
            //StringBuilder str=new StringBuilder();
            Stream stream = new MemoryStream();
            serializer.WriteObject(stream, data);
            str = new StreamReader(stream).ReadToEnd();
            //StreamWriter stream=new StreamWriter();
        }

        private class Data
        {
            
            public string Name
            {
                set;
                get;
            }

        }
    }
}
