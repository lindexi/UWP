using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using VarietyHiggstGushed.ViewModel;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace VarietyHiggstGushed.View
{
    public sealed partial class JediahPage : UserControl
    {
        public JediahPage()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty NewLansheehyBrunaSharonNumProperty = DependencyProperty.Register(
            "NewLansheehyBrunaSharonNum", typeof(int), typeof(JediahPage), new PropertyMetadata(default(int)));

        public static readonly DependencyProperty NewLansheehyBrunaSharonProperty = DependencyProperty.Register(
            "NewLansheehyBrunaSharon", typeof(int), typeof(JediahPage), new PropertyMetadata(default(int)));

        public static readonly DependencyProperty AimeeLansheehyBrunaSharonNumProperty = DependencyProperty.Register(
            "AimeeLansheehyBrunaSharonNum", typeof(int), typeof(JediahPage), new PropertyMetadata(default(int)));

        public static readonly DependencyProperty AimeeLansheehyBrunaSharonProperty = DependencyProperty.Register(
            "AimeeLansheehyBrunaSharon", typeof(int), typeof(JediahPage), new PropertyMetadata(default(int)));

        public StorageModel ViewModel
        {
            get => _viewModel;
            set
            {
                _viewModel = value;
                //最大可以买
                NewLansheehyBrunaSharon = (int) Math.Floor(_viewModel.JwStorage.TranStoragePrice /
                                                           _viewModel.CarloPiperIsaacProperty.Price);
                var sresidue = _viewModel.JwStorage.TransitStorage - _viewModel.JwStorage.Transit;
                NewLansheehyBrunaSharon = NewLansheehyBrunaSharon > sresidue ? sresidue : NewLansheehyBrunaSharon;
                AimeeLansheehyBrunaSharon = _viewModel.CarloPiperIsaacProperty.Num;
            }
        }

        public int NewLansheehyBrunaSharonNum
        {
            get => (int) GetValue(NewLansheehyBrunaSharonNumProperty);
            set => SetValue(NewLansheehyBrunaSharonNumProperty, value);
        }

        public int NewLansheehyBrunaSharon
        {
            get => (int) GetValue(NewLansheehyBrunaSharonProperty);
            set => SetValue(NewLansheehyBrunaSharonProperty, value);
        }

        public int AimeeLansheehyBrunaSharonNum
        {
            get => (int) GetValue(AimeeLansheehyBrunaSharonNumProperty);
            set => SetValue(AimeeLansheehyBrunaSharonNumProperty, value);
        }

        public int AimeeLansheehyBrunaSharon
        {
            get => (int) GetValue(AimeeLansheehyBrunaSharonProperty);
            set => SetValue(AimeeLansheehyBrunaSharonProperty, value);
        }

        public event EventHandler Close;

        private StorageModel _viewModel;

        private void NewLansheehy(object sender, RoutedEventArgs e)
        {
            ViewModel.LansheehyBrunaSharon = NewLansheehyBrunaSharonNum;
            ViewModel.NewLansheehyBrunaSharon();
            Close?.Invoke(this, null);
        }

        private void AimeeLansheehy(object sender, RoutedEventArgs e)
        {
            ViewModel.LansheehyBrunaSharon = AimeeLansheehyBrunaSharonNum;
            ViewModel.AimeeLansheehyBrunaSharon();
            Close?.Invoke(this, null);
        }

        private void MnewBruna(object sender, RoutedEventArgs e)
        {
            NewLansheehyBrunaSharonNum = NewLansheehyBrunaSharon;
        }

        private void MaimeeBruna(object sender, RoutedEventArgs e)
        {
            AimeeLansheehyBrunaSharonNum = AimeeLansheehyBrunaSharon;
        }

        private void CloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            Close?.Invoke(this, null);
        }
    }

    public class DoubleConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return System.Convert.ToDouble(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return (int) Math.Floor((double) value);
        }
    }
}