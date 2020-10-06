using System;
using System.Windows;
using System.Windows.Controls;
using NAudio.CoreAudioApi;
using SourceChord.Lighty;

namespace Recode_to_text
{
    /// <summary>
    /// MicrophoneDeviceSetingWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MicrophoneDeviceSetingWindow : UserControl
    {
        internal bool DialogResult { get; set; }
        internal string Device { get; set; }

        public MicrophoneDeviceSetingWindow()
        {
            InitializeComponent();
            comboBox_Device.Items.Add("Default:Default");
            var enumerator = new MMDeviceEnumerator();
            foreach (var endpoint in
                     enumerator.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active))
            {
                Console.WriteLine("{0} ({1})", endpoint.FriendlyName, endpoint.ID);
                comboBox_Device.Items.Add(endpoint.FriendlyName + ":" + endpoint.ID);
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            if (Device != null)
            {
                comboBox_Device.SelectedItem = Device;
            }
        }

        private void Button_OK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            if (comboBox_Device.Text != null)
            {
                Device = comboBox_Device.Text;
            }
            this.Close();
        }
    }
}
