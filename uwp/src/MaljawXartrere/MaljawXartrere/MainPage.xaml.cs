using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Color = Windows.UI.Color;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace MaljawXartrere
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

        private void PenerfePowForqe_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (PenerfePowForqe.Text.Length == 6 || PenerfePowForqe.Text.Length==3 || PenerfePowForqe.Text.Length==8)
            {
                try
                {
                    SecatrearDasNasumutir.Background = GetSolidColorBrush(PenerfePowForqe.Text);
                }
                catch (Exception exception)
                {
                    Debug.WriteLine(exception);
                    
                }
            }
        }

        private SolidColorBrush GetSolidColorBrush(string hex)
        {
            hex = hex.Replace("#", string.Empty);

            //#FFDFD991
            //#DFD991
            //#FD92
            //#DAC

            bool existAlpha = hex.Length == 8 || hex.Length == 4;
            bool isDoubleHex = hex.Length == 8 || hex.Length == 6;

            if (!existAlpha && hex.Length != 6 && hex.Length != 3)
            {
                throw new ArgumentException("输入的hex不是有效颜色");
            }

            int n = 0;
            byte a;
            int hexCount = isDoubleHex ? 2 : 1;
            if (existAlpha)
            {
                n = hexCount;
                a = (byte) ConvertHexToByte(hex, 0, hexCount);
                if (!isDoubleHex)
                {
                    a = (byte) (a * 16 + a);
                }
            }
            else
            {
                a = 0xFF;
            }

            var r = (byte) ConvertHexToByte(hex, n, hexCount);
            var g = (byte) ConvertHexToByte(hex, n + hexCount, hexCount);
            var b = (byte) ConvertHexToByte(hex, n + 2 * hexCount, hexCount);
            if (!isDoubleHex)
            {
                //#FD92 = #FFDD9922

                r = (byte) (r * 16 + r);
                g = (byte) (g * 16 + g);
                b = (byte) (b * 16 + b);
            }

            return new SolidColorBrush(Windows.UI.Color.FromArgb(a, r, g, b));
        }

        private static uint ConvertHexToByte(string hex, int n, int count = 2)
        {
            return Convert.ToUInt32(hex.Substring(n, count), 16);
        }

        private void GawpeargouciWhonacel_OnClick(object sender, RoutedEventArgs e)
        {
            PenerfePowForqe.Text += "7";
        }


        private void PasuBurceaVisowpoSawkoukay_OnClick(object sender, RoutedEventArgs e)
        {
            PenerfePowForqe.Text += "8";
        }

        private void WihisviwisKusur_OnClick(object sender, RoutedEventArgs e)
        {
            PenerfePowForqe.Text += "9";
        }

        private void SasmaysougowZawna_OnClick(object sender, RoutedEventArgs e)
        {
            PenerfePowForqe.Text += "A";
        }

        private void DisyeJaygetou_OnClick(object sender, RoutedEventArgs e)
        {
            PenerfePowForqe.Text += "D";
        }

        private void HasarcayKarsalllorpeeHairjo_OnClick(object sender, RoutedEventArgs e)
        {
            PenerfePowForqe.Text += "B";
        }

        private void FetotouseTassairkukaKaqirStaipooner_OnClick(object sender, RoutedEventArgs e)
        {
            PenerfePowForqe.Text += "E";
        }

        private void TaschuSallchi_OnClick(object sender, RoutedEventArgs e)
        {
            PenerfePowForqe.Text += "C";
        }

        private void MernitayTowhu_OnClick(object sender, RoutedEventArgs e)
        {
            PenerfePowForqe.Text += "F";
        }

        private void BemusiMemxea_OnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(PenerfePowForqe.Text))
            {
                return;
            }

            PenerfePowForqe.Text = PenerfePowForqe.Text.Substring(0, PenerfePowForqe.Text.Length - 1);
        }

        private void SirereNati_OnClick(object sender, RoutedEventArgs e)
        {
            PenerfePowForqe.Text += "0";
        }

        private void MudelKepeZaimaicha_OnClick(object sender, RoutedEventArgs e)
        {
            PenerfePowForqe.Text += "4";
        }

        private void NairzoBeaqar_OnClick(object sender, RoutedEventArgs e)
        {
            PenerfePowForqe.Text += "5";
        }

        private void PousirTor_OnClick(object sender, RoutedEventArgs e)
        {
            PenerfePowForqe.Text += "6";
        }

        private void LurwhuJelba_OnClick(object sender, RoutedEventArgs e)
        {
            PenerfePowForqe.Text += "1";
        }

        private void BeNowwulorCawirsalldee_OnClick(object sender, RoutedEventArgs e)
        {
            PenerfePowForqe.Text += "2";
        }

        private void TuriBowguDrersoo_OnClick(object sender, RoutedEventArgs e)
        {
            PenerfePowForqe.Text += "3";
        }
    }
}
