using System;
using System.Linq;
using System.Windows;
using System.Xml.Serialization;
using SourceChord.Lighty;
using System.IO;
using Microsoft.WindowsAPICodePack.Dialogs;
using Nayuki749.Speech_to_Text;
using log4net;

namespace Recod_To_Text
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        internal Settings setting;
        private Speech_To_Text stt;
        internal string filepath;
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public MainWindow()
        {
            InitializeComponent();
            logger.Info("------------- Application Start -------------");
#if DEBUG
            logger.Debug("******************** Debug mode ********************");
#endif
            grid_progress.Visibility = Visibility.Hidden;
            setting = new Settings();
            stt = new Speech_To_Text();
            logger.Info("loading configuration.");
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
                logger.Info("Save Azure settings.");
                setting2xml();              
            }
            else
            {
                logger.Debug("------------- Cancel save Azure settings -------------");
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
            logger.Debug("------------- Microphone device setting window show dialog -------------");
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
                logger.Info("Save Microphone Device settings.");
            }
            else
            {
                logger.Debug("------------- Cancel Save Microphone Device settings -------------");
            }
        }

        /// <summary>
        /// 設定ファイルに書き出し
        /// </summary>
        private void setting2xml()
        {
            // XmlSerializerを使ってファイルに保存（TwitSettingオブジェクトの内容を書き込む）
            XmlSerializer serializer = new XmlSerializer(typeof(Settings));
            try
            {
                // カレントディレクトリに"settings.xml"というファイルで書き出す
                FileStream fs = new FileStream(Directory.GetCurrentDirectory() + @"\settings.xml", FileMode.Create);

                // オブジェクトをシリアル化してXMLファイルに書き込む
                serializer.Serialize(fs, setting);
                fs.Close();
                logger.Info("Completed save the setting file.");
            }
            catch(Exception e)
            {
                logger.Error(e.Message + e.Source + e.StackTrace);
            }
        }

        /// <summary>
        /// 設定ファイル読み込み
        /// </summary>
        private void xml2setting()
        {
            if (File.Exists(Directory.GetCurrentDirectory() + @"\settings.xml"))
            {
                try
                {
                    // XmlSerializerを使ってファイルに保存（TwitSettingオブジェクトの内容を書き込む）
                    XmlSerializer serializer = new XmlSerializer(typeof(Settings));

                    FileStream fs = new FileStream(Directory.GetCurrentDirectory() + @"\settings.xml", FileMode.Open);

                    // XMLファイルを読み込み、逆シリアル化（復元）する
                    setting = (Settings)serializer.Deserialize(fs);
                    fs.Close();
                    logger.Info("Completed loading the setting file.");
                }
                catch (Exception e)
                {
                    logger.Error(e.Message + e.Source + e.StackTrace);
                }
            }
            else
            {
                logger.Info("Dose not exsist setting file.");
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
            stt.IsPlayWav = toggleSwitch_play.IsOn;
            stt.IsrecognizingCheck = false;
#if DEBUG
            stt.IsrecognizingCheck = true;
#endif
            #endregion

            try
            {
                logger.Info("Start voice recognition processing with microphone.");
                //音声認識開始
                stt.Start();
                logger.Debug("------------- Start ProgressRing -------------");
                grid_progress.Visibility = Visibility.Visible;
            }
            catch(Exception exception)
            {
                logger.Error(exception.Message);
                MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                grid_progress.Visibility = Visibility.Hidden;
                logger.Debug("------------- Stop ProgressRing -------------");
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
                logger.Info("Stop voice recognition processing with microphone.");
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
                if (System.IO.Path.GetExtension(cofd.FileName) == ".wav"|| System.IO.Path.GetExtension(cofd.FileName) == ".WAV") {
                    logger.Info("The file for voice recognition has been opened.");
                    logger.Info("Open File Name:" + cofd.FileName);
                    filepath = cofd.FileName;
                }
                else
                {
                    logger.Error("File with extension:*" + System.IO.Path.GetExtension(cofd.FileName) + " selected.");
                    logger.Error("Please specify a wav file.");
                    MessageBox.Show("File with extension:*" + System.IO.Path.GetExtension(cofd.FileName) + " selected.\nPlease specify a wav file.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
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
            #region 音声認識機能プロパティ設定
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
            stt.IsPlayWav = toggleSwitch_play.IsOn;
            stt.IsrecognizingCheck = false;
#if DEBUG
            stt.IsrecognizingCheck = true;
#endif
            #endregion

            try
            {
                logger.Info("Starts voice recognition processing using WAV files.");
                stt.Start(filepath);
                logger.Debug("------------- Start ProgressRing -------------");
                grid_progress.Visibility = Visibility.Visible;
            }
            catch (Exception exception)
            {
                logger.Error(exception.Message);
                MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                logger.Debug("------------- Stop ProgressRing -------------");
                grid_progress.Visibility = Visibility.Hidden;
            }
        }

        private void Button_versionInformation_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.FileVersionInfo ver = System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location);
            VersionInfomationWindow VersionWindow = new VersionInfomationWindow("Recode_To_Text_Ver:" + ver.FileVersion, "Speech_To_Text_DLL_Ver:" + stt.Version);
            logger.Debug("------------- VersionWindow show dialog -------------");
            LightBox.ShowDialog(this, VersionWindow);
        }

        #region 音声認識イベント

        /// <summary>
        /// ストップイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private  void SpeechSessionStoped(object sender, SpeechSessionStopedEventArgs e)
        {
            using (var write = new StreamWriter(filepath + ".txt", true, System.Text.Encoding.GetEncoding("shift_jis")))
            {
                logger.Info("Azure session stop");
                logger.Debug("SpeechSessionStoped Event:" + e.Message);
                write.WriteLine(e.Message);
            }
            logger.Debug("------------- Stop ProgressRing -------------");
            Dispatcher.Invoke(new Action(() =>
            {
                grid_progress.Visibility = Visibility.Hidden;
            }));
        }

        /// <summary>
        /// スタートイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SpeechSessionStarted(object sender, SpeechSessionStartedEventArgs e)
        {
            using (var write = new StreamWriter(filepath + ".txt", true, System.Text.Encoding.GetEncoding("shift_jis")))
            {
                logger.Info("Azure session start");
                logger.Debug("SpeechSessionStarted Event:" + e.Message);
                write.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// キャンセルイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SpechCanceled(object sender, SpeechCanceledEventArgs e)
        {
            using (var write = new StreamWriter(filepath + ".txt", true, System.Text.Encoding.GetEncoding("shift_jis")))
            {
                logger.Debug("SpechCanceled Event:" + e.Message);
                write.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// 音声認識完了イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            using (var write = new StreamWriter(filepath + ".txt", true, System.Text.Encoding.GetEncoding("shift_jis")))
            {
                logger.Info("Azure recognitioned Response");
                logger.Debug("SpeechRecognized Event:" + e.Message);
                write.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// 音声認識中間イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SpeechRecognizing(object sender, SpeechRecognizingEventArgs e)
        {
            using (var write = new StreamWriter(filepath + ".txt", true, System.Text.Encoding.GetEncoding("shift_jis")))
            {
                logger.Debug("SpeechRecognizing Event:" + e.Message);
                write.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// 音声認識スタートイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RecognitionStart(object sender, RecognitionStartEventArgs e)
        {
            using (var write = new StreamWriter(filepath + ".txt", true, System.Text.Encoding.GetEncoding("shift_jis")))
            {
                logger.Info("Start Azure recognition process");
                logger.Debug("RecognitionStart Event:" + e.Message);
                write.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// 音声認識検出イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SpeechDetected(object sender, SpeechDetectedEvendEventArgs e)
        {
            using (var write = new StreamWriter(filepath + ".txt", true, System.Text.Encoding.GetEncoding("shift_jis")))
            {                
                logger.Debug("SpeechDetected Event:" + e.Message);
                write.WriteLine(e.Message);
            }
        }
        #endregion

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            logger.Info("------------- Application Stop -------------");
        }
    }
}
