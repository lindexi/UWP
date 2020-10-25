using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace lindexi.uwp.control.Button
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            View = new global::lindexi.uwp.control.Button.ViewModel.ViewModel();
            this.InitializeComponent();

        }

        private global::lindexi.uwp.control.Button.ViewModel.ViewModel View
        {
            set;
            get;
        }

        //private async void ProgressButton_OnClick(object sender, ProgressButton.ProgressButtonEventArgs e)
        //{
        //    await Task.Delay(1000);
        //    e.OnFinished.Invoke();
        //}

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            new Task(async () =>
            {
                //ProgressButton temp=sender as ProgressButton;
                //if (temp == null)
                //{
                //    return;
                //}
                await Task.Delay(1000);
                //temp.Complete = true;
                View.Complete = true;
            }).Start();
            //Data data = new Data()
            //{
            //    Name = "2"
            //};

            //var serializer = new DataContractJsonSerializer(typeof(Data));

            //string str = null;
            ////StringBuilder str=new StringBuilder();
            //Stream stream = new MemoryStream();
            //serializer.WriteObject(stream, data);
            //str = new StreamReader(stream).ReadToEnd();
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
