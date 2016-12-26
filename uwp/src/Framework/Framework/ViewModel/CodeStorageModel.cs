using System.Threading.Tasks;
using Windows.UI.Core;

namespace Framework.ViewModel
{
    public class CodeStorageModel : ViewModelBase,IReceiveMessage
    {
        public CodeStorageModel()
        {
            DetailMaster = new DetailMasterModel();
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                AppViewBackButtonVisibility.Visible;
        }

        public DetailMasterModel DetailMaster
        {
            set;
            get;
        }

        public ContentModel ContentModel
        {
            set;
            get;
        }

        public ListModel ListModel
        {
            set;
            get;
        }

        public override void OnNavigatedFrom(object obj)
        {

        }

        public override void OnNavigatedTo(object obj)
        {
            DetailMaster.Narrow();
            MasterSendMessage temp=new MasterSendMessage(ReceiveMessage);
            ListModel = new ListModel()
            {
                SendMessage = temp
            };
            ListModel.OnNavigatedTo(null);
            ContentModel = new ContentModel();
            
        }

        public void ReceiveMessage(Message message)
        {
            if (message.Key == "点击列表")
            {
                DetailMaster.MasterClick();

            }
            if (message.Goal == nameof(ContentModel))
            {
                ContentModel.ReceiveMessage(message);
            }
        }

    }
}