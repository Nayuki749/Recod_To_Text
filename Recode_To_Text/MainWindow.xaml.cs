using System;
using System.Linq;
using System.Windows;
using System.Xml.Serialization;
using SourceChord.Lighty;
using System.IO;
using Microsoft.WindowsAPICodePack.Dialogs;
using Nayuki749.Speech_to_Text;

namespace Recode_to_text
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        internal Settings setting;
        private Speech_To_Text stt;
        internal string filepath;

        public MainWindow()
        {
            InitializeComponent();
            setting = new Settings();
            stt = new Speech_To_Text();
            xml2setting();
            button_Start.IsEnabled = true;
            button_Stop.IsEnabled = false;
            

            #region Spheech to Text Events
            stt.SpeechRecognizingEvent += new Speech_To_Text.SpeechRecognizingEventHandler(this.SpeechRecognizing);
            stt.SpeechDetectedEvent += new Speech_To_Text.SpeechDetectedEventHandler(this.SpeechDetected);
            stt.SpeechRecognizedEvent += new Speech_To_Text.SpeechRecognizedEventHandler(this.SpeechRecognized);
            stt.SpeechCanceledEvent += new Speech_To_Text.SpeechCanceledEventHandler(this.SpechCanceled);
            stt.SpeechSessionStartedEvent += new Speech_To_Text.SpeechSessionStartedEventHandler(this.SpeechSessionStarted);
            stt.SpeechSessionStopedEvent += new Speech_To_Text.SpeechSessionStopedEventHandler(this.SpeechSessionStoped);
            stt.RecognitionStartEvent += new Speech_To_Text.RecognitionStartEventHandler(this.RecognitionStart);
            #endregion
        }

        /// <summary>
        /// Azure設定ダイアログ表示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_azureSetting_Click(object sender, RoutedEventArgs e)
        {
            AzureSettingWindow asw = new AzureSettingWindow();
            
            if (setting.SubscriptionKey != null)
            {
                asw.SubscriptionKey = setting.SubscriptionKey;
            }
            if (setting.Region != null)
            {
                asw.Region = setting.Region;
            }
            if (setting.Location != null)
            {
                asw.Location = setting.Location;
            }
            if (setting.PROXY_Host != null)
            {
                asw.PROXY_Host = setting.PROXY_Host;
            }
            if (setting.PROXY_Port != null)
            {
                asw.PROXY_Port = setting.PROXY_Port;
            }
            LightBox.ShowDialog(this, asw);
            if (asw.DialogResult)
            {
                setting.SubscriptionKey = asw.SubscriptionKey;
                setting.Region = asw.Region;
                setting.Location = asw.Location;
                setting.PROXY_Host = asw.PROXY_Host;
                setting.PROXY_Port = asw.PROXY_Port;
                setting.Device = setting.Device;
                setting2xml();
            }
        }

        /// <summary>
        /// マイク設定ダイアログ表示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_deviceSetting_Click(object sender, RoutedEventArgs e)
        {
            MicrophoneDeviceSetingWindow mdsw = new MicrophoneDeviceSetingWindow();
            
            if (setting.Device != null)
            {
                mdsw.Device = setting.Device;
            }
            LightBox.ShowDialog(this, mdsw);
            if (mdsw.DialogResult)
            {
                setting.SubscriptionKey = setting.SubscriptionKey;
                setting.Region = setting.Region;
                setting.Location = setting.Location;
                setting.PROXY_Host = setting.PROXY_Host;
                setting.PROXY_Port = setting.PROXY_Port;
                setting.Device = mdsw.Device;
                setting2xml();
            }
        }

        /// <summary>
        /// 設定ファイルに書き出し
        /// </summary>
        private void setting2xml()
        {
            // XmlSerializerを使ってファイルに保存（TwitSettingオブジェクトの内容を書き込む）
            XmlSerializer serializer = new XmlSerializer(typeof(Settings));

            // カレントディレクトリに"settings.xml"というファイルで書き出す
            FileStream fs = new FileStream(Directory.GetCurrentDirectory() + @"\settings.xml", FileMode.Create);

            // オブジェクトをシリアル化してXMLファイルに書き込む
            serializer.Serialize(fs, setting);
            fs.Close();
        }

        /// <summary>
        /// 設定ファイル読み込み
        /// </summary>
        private void xml2setting()
        {
            if (File.Exists(Directory.GetCurrentDirectory() + @"\settings.xml"))
            {
                // XmlSerializerを使ってファイルに保存（TwitSettingオブジェクトの内容を書き込む）
                XmlSerializer serializer = new XmlSerializer(typeof(Settings));

                FileStream fs = new FileStream(Directory.GetCurrentDirectory() + @"\settings.xml", FileMode.Open);

                // XMLファイルを読み込み、逆シリアル化（復元）する
                setting = (Settings)serializer.Deserialize(fs);
                fs.Close();

           }
        }

        /// <summary>
        /// マイク入力変換開始
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RecodingButton_Click(object sender, RoutedEventArgs e)
        {
            button_Start.IsEnabled = false;
            button_Stop.IsEnabled = true;

            //変換結果書き出し先指定
            filepath = Directory.GetCurrentDirectory() + @"\Microphone";

            #region 音声認識機能プロパティ設定
            stt.SubscriptionKey = setting.SubscriptionKey;
            stt.Region = setting.Region;
            stt.RecognitionLanguage = setting.Location;
            stt.PROXY_HOST = setting.PROXY_Host;
            stt.PROXY_Port = setting.PROXY_Port;
            stt.MicrophoneID = setting.Device.Split(':').Last<string>();
            stt.CustomModelEndpointId = "none";
            stt.UseMicrophone = true;
            stt.UseFileInput = false;
            stt.UseBaseModel = true;
            stt.UseBaseAndCustomModels = false;
            stt.UseCustomModel = false;
            #endregion

            try
            {
                //音声認識開始
                stt.Start();
            }
            catch(Exception exception)
            {
                MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// マイク入力変換終了
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            button_Start.IsEnabled = true;
            button_Stop.IsEnabled = false;

            try
            {
                stt.Stop();
            }
            catch { }
        }

        /// <summary>
        /// ファイルオープン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FileOpenButton_Click(object sender, RoutedEventArgs e)
        {
            using (var cofd = new CommonOpenFileDialog()
            {
                Title = "音声認識をするWAVファイルを選択してください",
                InitialDirectory = @"C:\",
                // フォルダ選択モードにする
                //IsFolderPicker = false,
                RestoreDirectory = true,
            })
            {
                if (cofd.ShowDialog() != CommonFileDialogResult.Ok)
                {
                    return;
                }

                filepath = cofd.FileName;

                // フォルダを取得する
                //System.Windows.MessageBox.Show($"{cofd.FileName}を選択しました");
            }

        }

        /// <summary>
        /// ファイルから音声認識
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_ProcessStart_Click(object sender, RoutedEventArgs e)
        {

            stt.SubscriptionKey = setting.SubscriptionKey;
            stt.Region = setting.Region;
            stt.RecognitionLanguage = setting.Location;
            stt.PROXY_HOST = setting.PROXY_Host;
            stt.PROXY_Port = setting.PROXY_Port;
            stt.CustomModelEndpointId = "none";
            stt.UseMicrophone = false;
            stt.UseFileInput = true;
            stt.UseBaseModel = true;
            stt.UseBaseAndCustomModels = false;
            stt.UseCustomModel = false;

            stt.Start(filepath);
        }

        #region 音声認識イベント
        private  void SpeechSessionStoped(object sender, SpeechSessionStopedEventArgs e)
        {
            using (var write = new StreamWriter(filepath + ".txt", true))
            {
               write.WriteLine(e.Message);
            }
        }

        private void SpeechSessionStarted(object sender, SpeechSessionStartedEventArgs e)
        {
            using (var write = new StreamWriter(filepath + ".txt", true))
            {
                write.WriteLine(e.Message);
            }
        }

        private void SpechCanceled(object sender, SpeechCanceledEventArgs e)
        {
            using (var write = new StreamWriter(filepath + ".txt", true))
            {
                write.WriteLine(e.Message);
            }
        }

        private void SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            using (var write = new StreamWriter(filepath + ".txt", true))
            {
                write.WriteLine(e.Message);
            }
        }

        private void SpeechRecognizing(object sender, SpeechRecognizingEventArgs e)
        {
            using (var write = new StreamWriter(filepath + ".txt", true))
            {
                write.WriteLine(e.Message);
            }
        }

        private void RecognitionStart(object sender, RecognitionStartEventArgs e)
        {
            using (var write = new StreamWriter(filepath + ".txt", true))
            {
                write.WriteLine(e.Message);
            }
        }

        private void SpeechDetected(object sender, SpeechDetectedEvendEventArgs e)
        {
            using (var write = new StreamWriter(filepath + ".txt", true))
            {
                write.WriteLine(e.Message);
            }
        }
        #endregion
    }
}
