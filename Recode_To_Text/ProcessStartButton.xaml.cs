using System.Windows;
using System.Windows.Controls;

namespace Recod_To_Text
{
    public partial class ProcessStartButton : Button
    {
        public ProcessStartButton()
        {
            InitializeComponent();
        }

        public string ProcessStartButtonTitle
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("ProcessStartButtonTitle", typeof(string), typeof(ProcessStartButton), new FrameworkPropertyMetadata("ProcessStartButtonTitle", FrameworkPropertyMetadataOptions.AffectsRender));

        //public string SubTitle
        //{
        //    get { return (string)GetValue(SubTitleProperty); }
        //    set { SetValue(SubTitleProperty, value); }
        //}

        //public static readonly DependencyProperty SubTitleProperty =
        //    DependencyProperty.Register("SubTitle", typeof(string), typeof(RecodingButton), new FrameworkPropertyMetadata("SubTitle", FrameworkPropertyMetadataOptions.AffectsRender));

        public FrameworkElement ProcessStartButtonImage
        {
            get { return (FrameworkElement)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register("ProcessStartButtonImage", typeof(FrameworkElement), typeof(ProcessStartButton), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));
    }
}
