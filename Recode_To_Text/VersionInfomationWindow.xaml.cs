using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SourceChord.Lighty;

namespace Recod_To_Text
{
    /// <summary>
    /// VersionInfomationWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class VersionInfomationWindow : UserControl
    {
        

        public VersionInfomationWindow(string _recod_To_Text_Ver, string _speech_To_Text_Ver)
        {
            InitializeComponent();

            Recod_To_Text_Ver_Data RTTV = new Recod_To_Text_Ver_Data();
            Speech_To_Text_DLL_Ver_Data STTDV = new Speech_To_Text_DLL_Ver_Data();
            RTTV.Recod_To_Text_Ver = _recod_To_Text_Ver;
            STTDV.Speech_To_Text_DLL_Ver = _speech_To_Text_Ver;

            Binding RTTVBinding = new Binding("Recod_To_Text_Ver");
            Binding STTDVBinding = new Binding("Speech_To_Text_DLL_Ver");

            RTTVBinding.Source = RTTV;
            STTDVBinding.Source = STTDV;

            textBlock_Recod_To_Text_Ver.SetBinding(TextBlock.TextProperty, RTTVBinding);
            textBlock_Speech_To_Text_DLL_Ver.SetBinding(TextBlock.TextProperty, STTDVBinding);

            //Bird bird = new Bird();
            //bird.Name = "Penguin";

            //Binding binding = new Binding("Name");
            //binding.Source = bird;

            //textBlock1.SetBinding(TextBlock.TextProperty, binding);
        }

        private void Button_OK_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        //public string StopButtonTitle
        //{
        //    get { return (string)GetValue(TitleProperty); }
        //    set { SetValue(TitleProperty, value); }
        //}

        //public static readonly DependencyProperty TitleProperty =
        //    DependencyProperty.Register("StopButtonTitle", typeof(string), typeof(StopButton), new FrameworkPropertyMetadata("StopButtonTitle", FrameworkPropertyMetadataOptions.AffectsRender));
    }
}
