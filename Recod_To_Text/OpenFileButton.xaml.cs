using System.Windows;
using System.Windows.Controls;

namespace Recod_To_Text
{
    public partial class FileOpenButton : Button
    {
        public FileOpenButton()
        {
            InitializeComponent();
        }

        public string FileOpenButtonTitle
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("FileOpenButtonTitle", typeof(string), typeof(FileOpenButton), new FrameworkPropertyMetadata("FileOpenButtonTitle", FrameworkPropertyMetadataOptions.AffectsRender));

        //public string SubTitle
        //{
        //    get { return (string)GetValue(SubTitleProperty); }
        //    set { SetValue(SubTitleProperty, value); }
        //}

        //public static readonly DependencyProperty SubTitleProperty =
        //    DependencyProperty.Register("SubTitle", typeof(string), typeof(FancyButton), new FrameworkPropertyMetadata("SubTitle", FrameworkPropertyMetadataOptions.AffectsRender));

        public FrameworkElement FileOpenButtonImage
        {
            get { return (FrameworkElement)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register("FileOpenButtonImage", typeof(FrameworkElement), typeof(FileOpenButton), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));
    }
}