using System.Windows;
using System.Windows.Controls;

namespace Recod_To_Text
{
    public partial class StopButton : Button
    {
        public StopButton()
        {
            InitializeComponent();
        }

        public string StopButtonTitle
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("StopButtonTitle", typeof(string), typeof(StopButton), new FrameworkPropertyMetadata("StopButtonTitle", FrameworkPropertyMetadataOptions.AffectsRender));

        //public string SubTitle
        //{
        //    get { return (string)GetValue(SubTitleProperty); }
        //    set { SetValue(SubTitleProperty, value); }
        //}

        //public static readonly DependencyProperty SubTitleProperty =
        //    DependencyProperty.Register("SubTitle", typeof(string), typeof(RecodingButton), new FrameworkPropertyMetadata("SubTitle", FrameworkPropertyMetadataOptions.AffectsRender));

        public FrameworkElement StopButtonImage
        {
            get { return (FrameworkElement)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register("StopButtonImage", typeof(FrameworkElement), typeof(StopButton), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));
    }
}
