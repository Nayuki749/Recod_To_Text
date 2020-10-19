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
    /// test.xaml の相互作用ロジック
    /// </summary>
    public partial class AzureSettingWindow : UserControl
    {
        internal bool DialogResult { get; set; }
        internal string Region { get; set; }
        internal string SubscriptionKey { get; set; }
        internal string Location { get; set; }
        internal string PROXY_Host { get; set; }
        internal string PROXY_Port { get; set; }

        public AzureSettingWindow()
        {
            InitializeComponent();
            #region Region
            comboBox_region.Items.Add("centralus");
            comboBox_region.Items.Add("eastus");
            comboBox_region.Items.Add("eastus2");
            comboBox_region.Items.Add("northcentralus");
            comboBox_region.Items.Add("southcentralus");
            comboBox_region.Items.Add("westcentralus");
            comboBox_region.Items.Add("westus");
            comboBox_region.Items.Add("westus2");
            comboBox_region.Items.Add("canadacentral");
            comboBox_region.Items.Add("brazilsouth");
            comboBox_region.Items.Add("eastasia");
            comboBox_region.Items.Add("southeastasia");
            comboBox_region.Items.Add("australiaeast");
            comboBox_region.Items.Add("centralindia");
            comboBox_region.Items.Add("japaneast");
            comboBox_region.Items.Add("japanwest");
            comboBox_region.Items.Add("koreacentral");
            comboBox_region.Items.Add("northeurope");
            comboBox_region.Items.Add("westeurope");
            comboBox_region.Items.Add("francecentral");
            comboBox_region.Items.Add("uksouth");
            comboBox_region.SelectedItem = "japaneast";
            #endregion

            #region Location
            comboBox_Location.Items.Add("ar-AE");
            comboBox_Location.Items.Add("ar-BH");
            comboBox_Location.Items.Add("ar-EG");
            comboBox_Location.Items.Add("ar-IL");
            comboBox_Location.Items.Add("ar-JO");
            comboBox_Location.Items.Add("ar-KW");
            comboBox_Location.Items.Add("ar-LB");
            comboBox_Location.Items.Add("ar-PS");
            comboBox_Location.Items.Add("ar-QA");
            comboBox_Location.Items.Add("ar-SA");
            comboBox_Location.Items.Add("ar-SY");
            comboBox_Location.Items.Add("ca-ES");
            comboBox_Location.Items.Add("cs-CZ");
            comboBox_Location.Items.Add("da-DK");
            comboBox_Location.Items.Add("de-DE");
            comboBox_Location.Items.Add("en-AU");
            comboBox_Location.Items.Add("en-CA");
            comboBox_Location.Items.Add("en-GB");
            comboBox_Location.Items.Add("en-HK");
            comboBox_Location.Items.Add("en-IE");
            comboBox_Location.Items.Add("en-IN");
            comboBox_Location.Items.Add("en-NZ");
            comboBox_Location.Items.Add("en-PH");
            comboBox_Location.Items.Add("en-SG");
            comboBox_Location.Items.Add("en-US");
            comboBox_Location.Items.Add("en-ZA");
            comboBox_Location.Items.Add("es-AR");
            comboBox_Location.Items.Add("es-BO");
            comboBox_Location.Items.Add("es-CL");
            comboBox_Location.Items.Add("es-CO");
            comboBox_Location.Items.Add("es-CR");
            comboBox_Location.Items.Add("es-CU");
            comboBox_Location.Items.Add("es-DO");
            comboBox_Location.Items.Add("es-EC");
            comboBox_Location.Items.Add("es-ES");
            comboBox_Location.Items.Add("es-GT");
            comboBox_Location.Items.Add("es-HN");
            comboBox_Location.Items.Add("es-MX");
            comboBox_Location.Items.Add("es-NI");
            comboBox_Location.Items.Add("es-PA");
            comboBox_Location.Items.Add("es-PE");
            comboBox_Location.Items.Add("es-PR");
            comboBox_Location.Items.Add("es-PY");
            comboBox_Location.Items.Add("es-SV");
            comboBox_Location.Items.Add("es-US");
            comboBox_Location.Items.Add("es-UY");
            comboBox_Location.Items.Add("es-VE");
            comboBox_Location.Items.Add("fi-FI");
            comboBox_Location.Items.Add("fr-CA");
            comboBox_Location.Items.Add("fr-FR");
            comboBox_Location.Items.Add("gu-IN");
            comboBox_Location.Items.Add("hi-IN");
            comboBox_Location.Items.Add("hu-HU");
            comboBox_Location.Items.Add("it-IT");
            comboBox_Location.Items.Add("ja-JP");
            comboBox_Location.Items.Add("ko-KR");
            comboBox_Location.Items.Add("mr-IN");
            comboBox_Location.Items.Add("nb-NO");
            comboBox_Location.Items.Add("nl-NL");
            comboBox_Location.Items.Add("pl-PL");
            comboBox_Location.Items.Add("pt-BR");
            comboBox_Location.Items.Add("pt-PT");
            comboBox_Location.Items.Add("ru-RU");
            comboBox_Location.Items.Add("sv-SE");
            comboBox_Location.Items.Add("ta-IN");
            comboBox_Location.Items.Add("te-IN");
            comboBox_Location.Items.Add("th-TH");
            comboBox_Location.Items.Add("tr-TR");
            comboBox_Location.Items.Add("zh-CN");
            comboBox_Location.Items.Add("zh-HK");
            comboBox_Location.Items.Add("zh-TW");
            comboBox_Location.SelectedItem = "ja-JP";
            #endregion
        }

        public AzureSettingWindow(string _subscriptionKey, string _region, string _location)
        {

            InitializeComponent();
            #region Region
            comboBox_region.Items.Add("centralus");
            comboBox_region.Items.Add("eastus");
            comboBox_region.Items.Add("eastus2");
            comboBox_region.Items.Add("northcentralus");
            comboBox_region.Items.Add("southcentralus");
            comboBox_region.Items.Add("westcentralus");
            comboBox_region.Items.Add("westus");
            comboBox_region.Items.Add("westus2");
            comboBox_region.Items.Add("canadacentral");
            comboBox_region.Items.Add("brazilsouth");
            comboBox_region.Items.Add("eastasia");
            comboBox_region.Items.Add("southeastasia");
            comboBox_region.Items.Add("australiaeast");
            comboBox_region.Items.Add("centralindia");
            comboBox_region.Items.Add("japaneast");
            comboBox_region.Items.Add("japanwest");
            comboBox_region.Items.Add("koreacentral");
            comboBox_region.Items.Add("northeurope");
            comboBox_region.Items.Add("westeurope");
            comboBox_region.Items.Add("francecentral");
            comboBox_region.Items.Add("uksouth");
            comboBox_region.SelectedItem = _region;
            #endregion

            #region Location
            comboBox_Location.Items.Add("ar-AE");
            comboBox_Location.Items.Add("ar-BH");
            comboBox_Location.Items.Add("ar-EG");
            comboBox_Location.Items.Add("ar-IL");
            comboBox_Location.Items.Add("ar-JO");
            comboBox_Location.Items.Add("ar-KW");
            comboBox_Location.Items.Add("ar-LB");
            comboBox_Location.Items.Add("ar-PS");
            comboBox_Location.Items.Add("ar-QA");
            comboBox_Location.Items.Add("ar-SA");
            comboBox_Location.Items.Add("ar-SY");
            comboBox_Location.Items.Add("ca-ES");
            comboBox_Location.Items.Add("cs-CZ");
            comboBox_Location.Items.Add("da-DK");
            comboBox_Location.Items.Add("de-DE");
            comboBox_Location.Items.Add("en-AU");
            comboBox_Location.Items.Add("en-CA");
            comboBox_Location.Items.Add("en-GB");
            comboBox_Location.Items.Add("en-HK");
            comboBox_Location.Items.Add("en-IE");
            comboBox_Location.Items.Add("en-IN");
            comboBox_Location.Items.Add("en-NZ");
            comboBox_Location.Items.Add("en-PH");
            comboBox_Location.Items.Add("en-SG");
            comboBox_Location.Items.Add("en-US");
            comboBox_Location.Items.Add("en-ZA");
            comboBox_Location.Items.Add("es-AR");
            comboBox_Location.Items.Add("es-BO");
            comboBox_Location.Items.Add("es-CL");
            comboBox_Location.Items.Add("es-CO");
            comboBox_Location.Items.Add("es-CR");
            comboBox_Location.Items.Add("es-CU");
            comboBox_Location.Items.Add("es-DO");
            comboBox_Location.Items.Add("es-EC");
            comboBox_Location.Items.Add("es-ES");
            comboBox_Location.Items.Add("es-GT");
            comboBox_Location.Items.Add("es-HN");
            comboBox_Location.Items.Add("es-MX");
            comboBox_Location.Items.Add("es-NI");
            comboBox_Location.Items.Add("es-PA");
            comboBox_Location.Items.Add("es-PE");
            comboBox_Location.Items.Add("es-PR");
            comboBox_Location.Items.Add("es-PY");
            comboBox_Location.Items.Add("es-SV");
            comboBox_Location.Items.Add("es-US");
            comboBox_Location.Items.Add("es-UY");
            comboBox_Location.Items.Add("es-VE");
            comboBox_Location.Items.Add("fi-FI");
            comboBox_Location.Items.Add("fr-CA");
            comboBox_Location.Items.Add("fr-FR");
            comboBox_Location.Items.Add("gu-IN");
            comboBox_Location.Items.Add("hi-IN");
            comboBox_Location.Items.Add("hu-HU");
            comboBox_Location.Items.Add("it-IT");
            comboBox_Location.Items.Add("ja-JP");
            comboBox_Location.Items.Add("ko-KR");
            comboBox_Location.Items.Add("mr-IN");
            comboBox_Location.Items.Add("nb-NO");
            comboBox_Location.Items.Add("nl-NL");
            comboBox_Location.Items.Add("pl-PL");
            comboBox_Location.Items.Add("pt-BR");
            comboBox_Location.Items.Add("pt-PT");
            comboBox_Location.Items.Add("ru-RU");
            comboBox_Location.Items.Add("sv-SE");
            comboBox_Location.Items.Add("ta-IN");
            comboBox_Location.Items.Add("te-IN");
            comboBox_Location.Items.Add("th-TH");
            comboBox_Location.Items.Add("tr-TR");
            comboBox_Location.Items.Add("zh-CN");
            comboBox_Location.Items.Add("zh-HK");
            comboBox_Location.Items.Add("zh-TW");
            comboBox_Location.SelectedItem = _location;
            #endregion

            textBox_subscriptionKey.Text = _subscriptionKey;

        }

        private void Button_OK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            SubscriptionKey = textBox_subscriptionKey.Text;
            Region = comboBox_region.Text;
            Location = comboBox_Location.Text;
            if (PROXY_Host == null)
            {
                PROXY_Host = "";
            }
            else
            {
                PROXY_Host = textBox_proxyHost.Text;
            }
            if (PROXY_Port == null)
            {
                PROXY_Port = "";
            }
            else
            {
                PROXY_Port = textBox_proxyPort.Text;
            }
            this.Close();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            //Set unless the Region property is null when initialization is complete.
            if (Region != null)
            {
                comboBox_region.SelectedItem = Region;
            }
            //Set unless the Subscription property is null when initialization is complete.
            if (SubscriptionKey != null)
            {
                textBox_subscriptionKey.Text = SubscriptionKey;
            }
            //Set unless the Location property is null when initialization is complete.
            if (Location != null)
            {
                comboBox_Location.SelectedItem = Location;
            }
            //Set unless the PROXY Host property is null when initialization is complete.
            if (PROXY_Host != null)
            {
                textBox_proxyHost.Text = PROXY_Host;
            }
            //Set unless the PROXY Port property is null when initialization is complete.
            if (PROXY_Port != null)
            {
                textBox_proxyPort.Text = PROXY_Port;
            }
        }
    }
}
