using System.Windows;
using System.Windows.Controls;

namespace Recode_to_text
{
    public partial class RecodingButton : Button
    {
        public RecodingButton()
        {
            InitializeComponent();
        }

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(RecodingButton), new FrameworkPropertyMetadata("Title", FrameworkPropertyMetadataOptions.AffectsRender));

        //public string SubTitle
        //{
        //    get { return (string)GetValue(SubTitleProperty); }
        //    set { SetValue(SubTitleProperty, value); }
        //}

        //public static readonly DependencyProperty SubTitleProperty =
        //    DependencyProperty.Register("SubTitle", typeof(string), typeof(RecodingButton), new FrameworkPropertyMetadata("SubTitle", FrameworkPropertyMetadataOptions.AffectsRender));

        public FrameworkElement Image
        {
            get { return (FrameworkElement)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register("Image", typeof(FrameworkElement), typeof(RecodingButton), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));
    }
}
