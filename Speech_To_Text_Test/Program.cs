using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Media;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;

namespace Speech_To_Text_Test
{
    class Program
    {
        #region プロパティ
        /// <summary>
        /// True, if audio source is mic
        /// </summary>
        public bool UseMicrophone { get; set; }

        /// <summary>
        /// True, if audio source is audio file
        /// </summary>
        public bool UseFileInput { get; set; }

        /// <summary>
        /// Only baseline model used for recognition
        /// </summary>
        public bool UseBaseModel { get; set; }

        /// <summary>
        /// Only custom model used for recognition
        /// </summary>
        public bool UseCustomModel { get; set; }

        /// <summary>
        /// Both models used for recognition
        /// </summary>
        public bool UseBaseAndCustomModels { get; set; }

        /// <summary>
        /// IsDebug
        /// </summary>
        public bool IsrecognizingCheck { get; set; }

        /// <summary>
        /// Gets or sets Subscription Key
        /// </summary>
        public string SubscriptionKey
        {
            get
            {
                return this.subscriptionKey;
            }

            set
            {
                this.subscriptionKey = value?.Trim();
                this.OnPropertyChanged<string>();
            }
        }

        /// <summary>
        /// Gets or sets region name of the service
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// Gets or sets recognition language
        /// </summary>
        public string RecognitionLanguage { get; set; }

        /// <summary>
        /// Gets or sets endpoint ID of the custom model
        /// </summary>
        public string CustomModelEndpointId
        {
            get
            {
                return this.customModeEndpointId;
            }

            set
            {
                this.customModeEndpointId = value?.Trim();
                this.OnPropertyChanged<string>();
            }
        }
        /// <summary>
        /// Gets or sets Microphone Device ID
        /// </summary>
        public string MicrophoneID
        {
            get
            {
                return this.microphoneID;
            }
            set
            {
                this.microphoneID = value?.Trim();
                this.OnPropertyChanged<string>();
            }
        }

        /// <summary>
        /// Gets or sets Proxy host
        /// </summary>
        public string PROXY_HOST
        {
            get
            {
                return this.proxy_Host;
            }
            set
            {
                this.proxy_Host = value;
            }
        }

        /// <summary>
        /// Gets or sets Proxy port
        /// </summary>
        public string PROXY_Port
        {
            get
            {
                return this.proxy_Port;
            }
            set
            {
                this.proxy_Port = value;
            }
        }


        // Private properties
        private const string defaultLocale = "ja-JP";
        private string customModeEndpointId = "none";
        private string subscriptionKey = "none";
        private string microphoneID = "";
        private string proxy_Host = "";
        private string proxy_Port = "";
        private string wavFileName = "";
        #endregion
        // The TaskCompletionSource must be rooted.
        // See https://blogs.msdn.microsoft.com/pfxteam/2011/10/02/keeping-async-methods-alive/ for details.
        private TaskCompletionSource<int> stopBaseRecognitionTaskCompletionSource;
        private TaskCompletionSource<int> stopCustomRecognitionTaskCompletionSource;

        #region レコードタイプ
        /// <summary>
        /// For this app there are two recognizers, one with the baseline model (Base), one with CRIS model (Custom)
        /// </summary>
        enum RecoType
        {
            Base = 1,
            Custom = 2
        }
        #endregion
 

        #region Recognition Event Handlers
        /// <summary>
        /// Implement INotifyPropertyChanged interface
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Logs intermediate recognition results
        /// </summary>
        private void RecognizingEventHandler(SpeechRecognitionEventArgs e, RecoType rt)
        {
            //SpeechRecognizingEventArgs eventArgs = new SpeechRecognizingEventArgs();
            Console.WriteLine("Intermediate result:  " + e.Result.Text);
            //eventArgs.Offset = e.Offset;
            //eventArgs.SessionId = e.SessionId;
            //OnSpeechRecognizingEvent(eventArgs);
        }

        /// <summary>
        /// Logs the final recognition result
        /// </summary>
        private void RecognizedEventHandler(SpeechRecognitionEventArgs e, RecoType rt)
        {

            if (rt == RecoType.Base)
            {
                Console.WriteLine(e.Result.Text);
            }
            else
            {
                Console.WriteLine(e.Result.Text);
            }

            Console.WriteLine($" --- Final result received. Reason: {e.Result.Reason.ToString()}. --- ");
            

            if (!string.IsNullOrEmpty(e.Result.Text))
            {
                //this.WriteLine(log, e.Result.Text);
                Console.WriteLine(e.Result.Text);
                

            }

            // if access to the JSON is needed it can be obtained from Properties
            string json = e.Result.Properties.GetProperty(PropertyId.SpeechServiceResponse_JsonResult);

        }

        /// <summary>
        /// Logs Canceled events
        /// And sets the TaskCompletionSource to 0, in order to trigger Recognition Stop
        /// </summary>
        private void CanceledEventHandler(SpeechRecognitionCanceledEventArgs e, RecoType rt, TaskCompletionSource<int> source)
        {
            
            Console.WriteLine("--- recognition canceled ---");
         
            Console.WriteLine($"CancellationReason: {e.Reason.ToString()}. ErrorDetails: {e.ErrorDetails}.");
            
        }

        /// <summary>
        /// Session started event handler.
        /// </summary>
        private void SessionStartedEventHandler(SessionEventArgs e, RecoType rt)
        {
           
            Console.WriteLine("Speech recognition: Session started event: " + e.ToString() + ".");
            
        }

        /// <summary>
        /// Session stopped event handler. Set the TaskCompletionSource to 0, in order to trigger Recognition Stop
        /// </summary>
        private void SessionStoppedEventHandler(SessionEventArgs e, RecoType rt, TaskCompletionSource<int> source)
        {
            source.TrySetResult(0);
            //SpeechSessionStopedEventArgs eventArgs = new SpeechSessionStopedEventArgs();
            Console.WriteLine("Speech recognition: Session stopped event: " + e.ToString() + ".");
            //OnSpeechSessionStopedEvent(eventArgs);
        }

        private void DetectedEventHandler(RecognitionEventArgs e, RecoType rt, string eventType)
        {
            //SpeechDetectedEvendEventArgs eventArgs = new SpeechDetectedEvendEventArgs();
            Console.WriteLine("Speech recognition: Speech " + eventType + "detected event: " + e.ToString() + ".");
            //OnSpeechDetectedEvent(eventArgs);
        }

        #endregion

        static void Main(string[] args)
        {
            Program p = new Program();
            p.UseMicrophone = false;
            p.UseFileInput = true;
            p.UseBaseModel = true;
            p.UseCustomModel = false;
            p.UseBaseAndCustomModels = false;
            p.subscriptionKey = "your subscription key";
            p.Region = "japaneast";
            p.RecognitionLanguage = "ja-JP";
            p.customModeEndpointId = "none";
            p.MicrophoneID = "";
            p.PROXY_HOST = "";
            p.PROXY_Port = "";
            p.Start(@"wev file path");
            Console.ReadLine();
        }

        private static void test()
        {
           
        }

        private void OnPropertyChanged<T>([CallerMemberName]string caller = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
        }

        /// <summary>
        /// Checks if keys are valid
        /// </summary>
        private bool CheckKeysValid()
        {
            if (this.subscriptionKey == null || this.subscriptionKey.Length <= 0 ||
                ((this.UseCustomModel || this.UseBaseAndCustomModels) && (this.customModeEndpointId == null || this.customModeEndpointId.Length <= 0)))
            {
                return false;
            }
            return true;
        }

        #region 音声認識開始
        /// <summary>
        /// Checks if keys are valid
        /// Plays audio if input source is a valid audio file
        /// Triggers Creation of specified Recognizers
        /// </summary>
        public void Start()
        {
            this.LogRecognitionStart();

            if (!CheckKeysValid())
            {
                if (this.UseBaseModel)
                {
                    Console.WriteLine("Subscription Key is wrong or missing!");
                }
                else if (this.UseCustomModel)
                {
                    Console.WriteLine("Subscription Key or Custom Model Endpoint ID is missing or wrong! If you do not need the custom model");
                }
                else if (this.UseBaseAndCustomModels)
                {
                    Console.WriteLine("Subscription Key or Custom Model Endpoint ID is missing or wrong! If you do not need the custom model");
                }

                return;
            }

            if (!this.UseMicrophone)
            {
                wavFileName = GetFile(wavFileName);
                if (wavFileName.Length <= 0) return;
                Task.Run(() => this.PlayAudioFile());
            }

            if (this.UseCustomModel || this.UseBaseAndCustomModels)
            {
                stopCustomRecognitionTaskCompletionSource = new TaskCompletionSource<int>();
                Task.Run(async () => { await CreateCustomReco().ConfigureAwait(false); });
            }

            if (this.UseBaseModel || this.UseBaseAndCustomModels)
            {
                stopBaseRecognitionTaskCompletionSource = new TaskCompletionSource<int>();
                Task.Run(async () => { await CreateBaseReco().ConfigureAwait(false); });
            }
        }

        /// <summary>
        /// Checks if keys are valid
        /// Plays audio if input source is a valid audio file
        /// Triggers Creation of specified Recognizers
        /// </summary>
        /// <param name="_fileName">wav file name</param>
        public void Start(string _fileName)
        {
            this.LogRecognitionStart();

            if (!CheckKeysValid())
            {
                if (this.UseBaseModel)
                {
                    Console.WriteLine("Subscription Key is wrong or missing!");
                }
                else if (this.UseCustomModel)
                {
                    Console.WriteLine("Subscription Key or Custom Model Endpoint ID is missing or wrong! If you do not need the custom model");
                }
                else if (this.UseBaseAndCustomModels)
                {
                    Console.WriteLine("Subscription Key or Custom Model Endpoint ID is missing or wrong! If you do not need the custom model");
                }

                return;
            }

            if (!this.UseMicrophone)
            {
                wavFileName = GetFile(_fileName);
                if (wavFileName.Length <= 0) return;
                Task.Run(() => this.PlayAudioFile());
            }

            if (this.UseCustomModel || this.UseBaseAndCustomModels)
            {
                stopCustomRecognitionTaskCompletionSource = new TaskCompletionSource<int>();
                Task.Run(async () => { await CreateCustomReco().ConfigureAwait(false); });
            }

            if (this.UseBaseModel || this.UseBaseAndCustomModels)
            {
                stopBaseRecognitionTaskCompletionSource = new TaskCompletionSource<int>();
                Task.Run(async () => { await CreateBaseReco().ConfigureAwait(false); });
            }
        }
        #endregion 

        /// <summary>
        /// Stops Recognition and enables Settings Panel in UI
        /// </summary>
        public void Stop()
        {
            try
            {
                if (this.UseBaseModel || this.UseBaseAndCustomModels)
                {
                    stopBaseRecognitionTaskCompletionSource.TrySetResult(0);
                }
                if (this.UseCustomModel || this.UseBaseAndCustomModels)
                {
                    stopCustomRecognitionTaskCompletionSource.TrySetResult(0);
                }
            }
            catch { }
        }

        /// <summary>
        /// Creates Recognizer with baseline model and selected language:
        /// Creates a config with subscription key and selected region
        /// If input source is audio file, creates recognizer with audio file otherwise with default mic
        /// Waits on RunRecognition
        /// </summary>
        private async Task CreateBaseReco()
        {
            try
            {
                // Todo: suport users to specifiy a different region.
                var config = SpeechConfig.FromSubscription(this.SubscriptionKey, this.Region);
                config.SpeechRecognitionLanguage = this.RecognitionLanguage;
                //If proxy information has been entered, set the proxy
                if ((this.proxy_Host != null && this.proxy_Host.Length > 0) &&
                    (this.proxy_Port != null && this.proxy_Port.Length > 0))
                    config.SetProxy(proxy_Host, int.Parse(proxy_Port));

                SpeechRecognizer basicRecognizer;
                if (this.UseMicrophone)
                {
                    if (MicrophoneID == "Default")
                    {
                        using (basicRecognizer = new SpeechRecognizer(config))
                        {
                            await this.RunRecognizer(basicRecognizer, RecoType.Base, stopBaseRecognitionTaskCompletionSource).ConfigureAwait(false);
                        }
                    }
                    else
                    {
                        using (var audioInput = AudioConfig.FromMicrophoneInput(MicrophoneID))
                        {
                            //fromMicrophoneInput(string)
                            using (basicRecognizer = new SpeechRecognizer(config, audioInput))
                            {
                                await this.RunRecognizer(basicRecognizer, RecoType.Base, stopBaseRecognitionTaskCompletionSource).ConfigureAwait(false);
                            }
                        }
                    }
                }
                else
                {
                    using (var audioInput = AudioConfig.FromWavFileInput(wavFileName))
                    {
                        using (basicRecognizer = new SpeechRecognizer(config, audioInput))
                        {
                            await this.RunRecognizer(basicRecognizer, RecoType.Base, stopBaseRecognitionTaskCompletionSource).ConfigureAwait(false);
                        }
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Creates Recognizer with custom model endpointId and selected language:
        /// Creates a config with subscription key and selected region
        /// If input source is audio file, creates recognizer with audio file otherwise with default mic
        /// Waits on RunRecognition
        /// </summary>
        private async Task CreateCustomReco()
        {
            // Todo: suport users to specifiy a different region.
            var config = SpeechConfig.FromSubscription(this.SubscriptionKey, this.Region);
            config.SpeechRecognitionLanguage = this.RecognitionLanguage;
            config.EndpointId = this.CustomModelEndpointId;
            //If proxy information has been entered, set the proxy
            if ((this.proxy_Host != null || this.proxy_Host.Length >= 0) &&
                (this.proxy_Port != null || this.proxy_Port.Length >= 0))
                config.SetProxy(proxy_Host, int.Parse(proxy_Port));

            SpeechRecognizer customRecognizer;
            if (this.UseMicrophone)
            {
                if (MicrophoneID == "Default")
                {
                    using (customRecognizer = new SpeechRecognizer(config))
                    {
                        await this.RunRecognizer(customRecognizer, RecoType.Custom, stopCustomRecognitionTaskCompletionSource).ConfigureAwait(false);
                    }
                }
                else
                {
                    using (var audioConfig = AudioConfig.FromMicrophoneInput(MicrophoneID))
                    {
                        using (customRecognizer = new SpeechRecognizer(config, audioConfig))
                        {
                            await this.RunRecognizer(customRecognizer, RecoType.Custom, stopCustomRecognitionTaskCompletionSource).ConfigureAwait(false);
                        }
                    }
                }
            }
            else
            {
                using (var audioInput = AudioConfig.FromWavFileInput(wavFileName))
                {
                    using (customRecognizer = new SpeechRecognizer(config, audioInput))
                    {
                        await this.RunRecognizer(customRecognizer, RecoType.Custom, stopCustomRecognitionTaskCompletionSource).ConfigureAwait(false);
                    }
                }
            }
        }

        /// <summary>
        /// Subscribes to Recognition Events
        /// Starts the Recognition and waits until final result is received, then Stops recognition
        /// </summary>
        /// <param name="recognizer">Recognizer object</param>
        /// <param name="recoType">Type of Recognizer</param>
        ///  <value>
        ///   <c>Base</c> if Baseline model; otherwise, <c>Custom</c>.
        /// </value>
        private async Task RunRecognizer(SpeechRecognizer recognizer, RecoType recoType, TaskCompletionSource<int> source)
        {

            EventHandler<SpeechRecognitionEventArgs> recognizingHandler = (sender, e) => RecognizingEventHandler(e, recoType);

            if (IsrecognizingCheck)
            {
                recognizer.Recognizing += recognizingHandler;
            }

            EventHandler<SpeechRecognitionEventArgs> recognizedHandler = (sender, e) => RecognizedEventHandler(e, recoType);
            EventHandler<SpeechRecognitionCanceledEventArgs> canceledHandler = (sender, e) => CanceledEventHandler(e, recoType, source);
            EventHandler<SessionEventArgs> sessionStartedHandler = (sender, e) => SessionStartedEventHandler(e, recoType);
            EventHandler<SessionEventArgs> sessionStoppedHandler = (sender, e) => SessionStoppedEventHandler(e, recoType, source);
            EventHandler<RecognitionEventArgs> speechStartDetectedHandler = (sender, e) => DetectedEventHandler(e, recoType, "start");
            EventHandler<RecognitionEventArgs> speechEndDetectedHandler = (sender, e) => DetectedEventHandler(e, recoType, "end");

            recognizer.Recognized += recognizedHandler;
            recognizer.Canceled += canceledHandler;
            recognizer.SessionStarted += sessionStartedHandler;
            recognizer.SessionStopped += sessionStoppedHandler;
            recognizer.SpeechStartDetected -= speechStartDetectedHandler;
            recognizer.SpeechEndDetected -= speechEndDetectedHandler;

            //start,wait,stop recognition
            await recognizer.StartContinuousRecognitionAsync().ConfigureAwait(false);
            await source.Task.ConfigureAwait(false);
            await recognizer.StopContinuousRecognitionAsync().ConfigureAwait(false);

            //this.EnableButtons();

            // unsubscribe from events
            if (IsrecognizingCheck)
            {
                recognizer.Recognizing -= recognizingHandler;
            }
            recognizer.Recognized -= recognizedHandler;
            recognizer.Canceled -= canceledHandler;
            recognizer.SessionStarted -= sessionStartedHandler;
            recognizer.SessionStopped -= sessionStoppedHandler;
            recognizer.SpeechStartDetected -= speechStartDetectedHandler;
            recognizer.SpeechEndDetected -= speechEndDetectedHandler;
        }



        /// <summary>
        /// Logs the recognition start.
        /// </summary>
        private void LogRecognitionStart()
        {
            string recoSource;
            recoSource = this.UseMicrophone ? "microphone" : "wav file";

            //RecognitionStartEventArgs eventArgs = new RecognitionStartEventArgs();
            Console.WriteLine("--- Start speech recognition using " + recoSource + " in " + defaultLocale + " language ----");
            //OnRecognitionStartEvent(eventArgs);
        }

        private void PlayAudioFile()
        {
            SoundPlayer player = new SoundPlayer(wavFileName);
            player.Load();
            player.Play();
        }

        /// <summary>
        /// Checks if specified audio file exists and returns it
        /// </summary>
        public string GetFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return "";
            }
            return filePath;
        }
    }
}
