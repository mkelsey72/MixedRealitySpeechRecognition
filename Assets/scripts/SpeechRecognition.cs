using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.CognitiveServices.Speech.Translation;
using System.Threading.Tasks;
using System.Globalization;
using System;
//using System.Diagnostics;
using TMPro;
using System.Collections.Generic;
using System.Text;
#if PLATFORM_ANDROID
using UnityEngine.Android;
#endif
using Debug = UnityEngine.Debug;


/// <summary>
/// SpeechRecognition class lets the user use Speech-to-Text to convert spoken words
/// into text strings. There is an optional mode that can be enabled to also translate
/// the text after the recognition returns results. Both modes also support interim
/// results (i.e. recognition hypotheses) that are returned in near real-time as the 
/// speaks in the microphone.
/// </summary>
public class SpeechRecognition : MonoBehaviour
{
    // Public fields in the Unity inspector
    [Tooltip("Unity UI Text component used to report potential errors on screen.")]
    public Text RecognizedText;
    [Tooltip("Unity UI Text component used to post recognition results on screen.")]
    public Text ErrorText;

    [Tooltip("Text Mesh Pro - where subtitles appears")]
    public TextMeshProUGUI ResultText;

    [Tooltip("Indicates if session should be documented or not.")]
    public static bool recordSession;

    [Tooltip("Streamwriter object used below to document sessions.")]
    private string path;

    // Dropdown lists used to select translation languages, if enabled
    public Toggle TranslationEnabled;

    [Tooltip("Target language #1 for translation (if enabled).")]
    public TMP_Dropdown LanguageDropdown;

    //public GameObject TranslationMenu;

    // Used to show live messages on screen, must be locked to avoid threading deadlocks since
    // the recognition events are raised in a separate thread
    private string recognizedString = "";
    private string errorString = "";
    private System.Object threadLocker = new System.Object();

    // Speech recognition key, required
    [Tooltip("Connection string to Cognitive Services Speech.")]
    public string SpeechServiceAPIKey = string.Empty;
    [Tooltip("Region for your Cognitive Services Speech instance (must match the key).")]
    public string SpeechServiceRegion = "westus";

    // Cognitive Services Speech objects used for Speech Recognition
    private SpeechRecognizer recognizer;
    private TranslationRecognizer translator;

    // The current language of origin is locked to English-US in this sample. Change this
    // to another region & language code to use a different origin language.
    // e.g. fr-fr, es-es, etc.
    string inputLanguage = "en-US";
    string outputLanguage = "en";

    string[] langCodes = { "ar-LB", "zh-Hans", "nl-NL", "fr-FR", "de-DE", "hi-IN", "it-IT", "ja-JP", "ko-KR", "ru-RU", "es-US", "uk-UA" };

    private bool micPermissionGranted = false;
#if PLATFORM_ANDROID
    // Required to manifest microphone permission, cf.
    // https://docs.unity3d.com/Manual/android-manifest.html
    private Microphone mic;
#endif

    private void Awake()
    {
        //creating file initially
        path = Path.Combine(Application.persistentDataPath, "MRS_Documents.txt");
    }

    private void Start()
    {
#if PLATFORM_ANDROID
        // Request to use the microphone, cf.
        // https://docs.unity3d.com/Manual/android-RequestingPermissions.html
        recognizedString = "Waiting for microphone permission...";
        if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            Permission.RequestUserPermission(Permission.Microphone);
        }
#else
        micPermissionGranted = true;
#endif
    }

    public void documentSession()
    {
        recordSession = true;
    }

    /// <summary>
    /// Attach to button component used to launch continuous recognition (with or without translation)
    /// </summary>
    public void StartContinuous()
    {
        errorString = "";
        if (micPermissionGranted)
        {
            if (recordSession)
            {
                //File.Append opens the file if it exists, creates if it does not.
                using (FileStream writer = File.Open(path, FileMode.Append))
                {
                    Byte[] content = new UTF8Encoding(true).GetBytes("-------------- Beginning of Session --------------");
                    writer.Write(content, 0, content.Length);
                }                         
            }

            if (TranslationEnabled.isOn)
            {
                StartContinuousTranslation();
            }
            else
            {
                StartContinuousRecognition();
            }
        }
        else
        {
            recognizedString = "This app cannot function without access to the microphone.";
            errorString = "ERROR: Microphone access denied.";
            Debug.LogError(errorString);
        }
    }

    /// <summary>
    /// Creates a class-level Speech Recognizer for a specific language using Azure credentials
    /// and hooks-up lifecycle & recognition events
    /// </summary>
    void CreateSpeechRecognizer()
    {
        if (SpeechServiceAPIKey.Length == 0 || SpeechServiceAPIKey == "YourSubscriptionKey")
        {
            recognizedString = "You forgot to obtain Cognitive Services Speech credentials and inserting them in this app." + Environment.NewLine +
                               "See the README file and/or the instructions in the Awake() function for more info before proceeding.";
            errorString = "ERROR: Missing service credentials";
            Debug.LogError(errorString);
            return;
        }
        Debug.Log("Creating Speech Recognizer.");
        recognizedString = "Initializing speech recognition, please wait...";

        if (recognizer == null)
        {
            SpeechConfig config = SpeechConfig.FromSubscription(SpeechServiceAPIKey, SpeechServiceRegion);
            config.SpeechRecognitionLanguage = inputLanguage;
            recognizer = new SpeechRecognizer(config);

            if (recognizer != null)
            {               
                // Subscribes to speech events.
                recognizer.Recognizing += RecognizingHandler;
                recognizer.Recognized += RecognizedHandler;
                recognizer.SpeechStartDetected += SpeechStartDetectedHandler;
                recognizer.SpeechEndDetected += SpeechEndDetectedHandler;
                recognizer.Canceled += CanceledHandler;
                recognizer.SessionStarted += SessionStartedHandler;
                recognizer.SessionStopped += SessionStoppedHandler;
            }
        }
        Debug.Log("CreateSpeechRecognizer exit");
    }

    /// <summary>
    /// Initiate continuous speech recognition from the default microphone.
    /// </summary>
    private async void StartContinuousRecognition()
    {
        Debug.Log("Starting Continuous Speech Recognition.");
        CreateSpeechRecognizer();

        if (recognizer != null)
        {
            Debug.Log("Starting Speech Recognizer.");
            await recognizer.StartContinuousRecognitionAsync().ConfigureAwait(false);

            recognizedString = "Speech Recognizer is now running.";
            Debug.Log("Speech Recognizer is now running.");
        }
        Debug.Log("Start Continuous Speech Recognition exit");
    }

    #region Speech Recognition event handlers
    private void SessionStartedHandler(object sender, SessionEventArgs e)
    {
        Debug.Log($"\n    Session started event. Event: {e.ToString()}.");
    }

    private void SessionStoppedHandler(object sender, SessionEventArgs e)
    {
        Debug.Log($"\n    Session event. Event: {e.ToString()}.");
        Debug.Log($"Session Stop detected. Stop the recognition.");
    }

    private void SpeechStartDetectedHandler(object sender, RecognitionEventArgs e)
    {
        Debug.Log($"SpeechStartDetected received: offset: {e.Offset}.");
    }

    private void SpeechEndDetectedHandler(object sender, RecognitionEventArgs e)
    {
        Debug.Log($"SpeechEndDetected received: offset: {e.Offset}.");
        Debug.Log($"Speech end detected.");
    }

    // "Recognizing" events are fired every time we receive interim results during recognition (i.e. hypotheses)
    private void RecognizingHandler(object sender, SpeechRecognitionEventArgs e)
    {
        if (e.Result.Reason == ResultReason.RecognizingSpeech)
        {
            Debug.Log($"HYPOTHESIS: Text={e.Result.Text}");
            lock (threadLocker)
            {
                recognizedString = $"HYPOTHESIS: {Environment.NewLine}{e.Result.Text}";
            }
        }
    }

    // "Recognized" events are fired when the utterance end was detected by the server
    private void RecognizedHandler(object sender, SpeechRecognitionEventArgs e)
    {
        if (e.Result.Reason == ResultReason.RecognizedSpeech)
        {
            Debug.Log($"RECOGNIZED: Text={e.Result.Text}");
            lock (threadLocker)
            {
                recognizedString = $"RESULT: {Environment.NewLine}{e.Result.Text}";

                /*
                if (recordSession)
                {
                    using (FileStream writer = File.Open(path, FileMode.Append))
                    {
                        Byte[] content = new UTF8Encoding(true).GetBytes($"{Environment.NewLine}{e.Result.Text}");
                        writer.Write(content, 0, content.Length);
                    }
                }
                */
            }
        }
        else if (e.Result.Reason == ResultReason.NoMatch)
        {
            Debug.Log($"NOMATCH: Speech could not be recognized.");
        }
    }

    // "Canceled" events are fired if the server encounters some kind of error.
    // This is often caused by invalid subscription credentials.
    private void CanceledHandler(object sender, SpeechRecognitionCanceledEventArgs e)
    {
        Debug.Log($"CANCELED: Reason={e.Reason}");

        errorString = e.ToString();
        if (e.Reason == CancellationReason.Error)
        {
            Debug.LogError($"CANCELED: ErrorDetails={e.ErrorDetails}");
            Debug.LogError("CANCELED: Did you update the subscription info?");
        }
    }
    #endregion
    /// <summary>
    /// Initiate continuous speech recognition from the default microphone, including live translation.
    /// </summary>
    private async void StartContinuousTranslation()
    {
        Debug.Log("Starting Continuous Translation Recognition.");
        CreateTranslationRecognizer();

        if (translator != null)
        {
            Debug.Log("Starting Speech Translator.");
            recognizedString = "Starting Speech Translator.";
            await translator.StartContinuousRecognitionAsync().ConfigureAwait(false);
            recognizedString = "Speech Translator is now running.";

            Debug.Log("Speech Translator is now running.");
        }
        Debug.Log("Start Continuous Speech Translation exit");
    }

    /// <summary>
    /// Creates a class-level Translation Recognizer for a specific language using Azure credentials
    /// and hooks-up lifecycle & recognition events. Translation can be enabled with one or more target
    /// languages translated simultaneously
    /// </summary>
    void CreateTranslationRecognizer()
    {
        Debug.Log("Creating Translation Recognizer.");
        recognizedString = "Initializing speech recognition with translation, please wait...";

        if (translator == null)
        {
            SpeechTranslationConfig config = SpeechTranslationConfig.FromSubscription(SpeechServiceAPIKey, SpeechServiceRegion);
            
            int menuIndex = LanguageDropdown.value;
            inputLanguage = langCodes[menuIndex];

            //The language we hear
            config.SpeechRecognitionLanguage = inputLanguage;

            //The language we want to see
            config.AddTargetLanguage(outputLanguage);
            
            translator = new TranslationRecognizer(config);
           
            if (translator != null)
            {
                translator.Recognizing += RecognizingTranslationHandler;
                translator.Recognized += RecognizedTranslationHandler;
                translator.SpeechStartDetected += SpeechStartDetectedHandler;
                translator.SpeechEndDetected += SpeechEndDetectedHandler;
                translator.Canceled += CanceledTranslationHandler;
                translator.SessionStarted += SessionStartedHandler;
                translator.SessionStopped += SessionStoppedHandler;
            }
        }
        Debug.Log("CreateTranslationRecognizer exit");
    }

    #region Speech Translation event handlers
    // "Recognizing" events are fired every time we receive interim results during recognition (i.e. hypotheses)
    private void RecognizingTranslationHandler(object sender, TranslationRecognitionEventArgs e)
    {
        if (e.Result.Reason == ResultReason.TranslatingSpeech)
        {
            Debug.Log($"RECOGNIZED HYPOTHESIS: Text={e.Result.Text}");
            lock (threadLocker)
            {
               // recognizedString = $"RECOGNIZED HYPOTHESIS ({inputLanguage}): {Environment.NewLine}{e.Result.Text}";
                // recognizedString += $"{Environment.NewLine}TRANSLATED HYPOTHESESE:";
                foreach (var element in e.Result.Translations)
                {
                    // recognizedString += $"{Environment.NewLine} {element.Value}";
                }
            }
        }
    }
    // "Recognized" events are fired when the utterance end was detected by the server
    private void RecognizedTranslationHandler(object sender, TranslationRecognitionEventArgs e)
    {
        if (e.Result.Reason == ResultReason.TranslatedSpeech)
        {
            Debug.Log($"RECOGNIZED: Text={e.Result.Text}");
            lock (threadLocker)
            {
               // recognizedString = $"RECOGNIZED RESULT ({inputLanguage}): {Environment.NewLine}{e.Result.Text}";
                //recognizedString += $"{Environment.NewLine}TRANSLATED RESULTS:";
                foreach (var element in e.Result.Translations)
                {
                    recognizedString = $"{Environment.NewLine} {element.Value}";
                    /*
                    if (recordSession)
                    {
                        using (FileStream writer = File.Open(path, FileMode.Append))
                        {
                            Byte[] content = new UTF8Encoding(true).GetBytes($"{Environment.NewLine} {element.Value}");
                            writer.Write(content, 0, content.Length);
                        }
                    }
                    */
                }
            }
        }
        else if (e.Result.Reason == ResultReason.RecognizedSpeech)
        {
            Debug.Log($"RECOGNIZED: Text={e.Result.Text}");
            lock (threadLocker)
            {
                recognizedString = $"NON-TRANSLATED RESULT: {Environment.NewLine}{e.Result.Text}";
            }
        }
        else if (e.Result.Reason == ResultReason.NoMatch)
        {
            Debug.Log($"NOMATCH: Speech could not be recognized or translated.");
        }
    }

    // "Canceled" events are fired if the server encounters some kind of error.
    // This is often caused by invalid subscription credentials.
    private void CanceledTranslationHandler(object sender, TranslationRecognitionCanceledEventArgs e)
    {
        Debug.Log($"CANCELED: Reason={e.Reason}");

        errorString = e.ToString();
        if (e.Reason == CancellationReason.Error)
        {
            Debug.LogError($"CANCELED: ErrorDetails={e.ErrorDetails}");
            Debug.LogError($"CANCELED: Did you update the subscription info?");
        }
    }
    #endregion

    /// <summary>
    /// Main update loop: Runs every frame
    /// </summary>
    void Update()
    {
#if PLATFORM_ANDROID
        if (!micPermissionGranted && Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            micPermissionGranted = true;
        }
#endif
        // Used to update results on screen during updates
        lock (threadLocker)
        {
            RecognizedText.text = recognizedString;
            ErrorText.text = errorString;
            if (ResultText != null)
                ResultText.text = recognizedString;
        }
    }

    void OnDisable()
    {
        StopRecognition();
    }

    /// <summary>
    /// Stops the recognition on the speech recognizer or translator as applicable.
    /// Important: Unhook all events & clean-up resources.
    /// </summary>
    public async void StopRecognition()
    {
        if (recognizer != null)
        {
            await recognizer.StopContinuousRecognitionAsync().ConfigureAwait(false);
            recognizer.Recognizing -= RecognizingHandler;
            recognizer.Recognized -= RecognizedHandler;
            recognizer.SpeechStartDetected -= SpeechStartDetectedHandler;
            recognizer.SpeechEndDetected -= SpeechEndDetectedHandler;
            recognizer.Canceled -= CanceledHandler;
            recognizer.SessionStarted -= SessionStartedHandler;
            recognizer.SessionStopped -= SessionStoppedHandler;
            recognizer.Dispose();
            recognizer = null;
            recognizedString = "Speech Recognizer is now stopped.";
            Debug.Log("Speech Recognizer is now stopped.");
        }
        if (translator != null)
        {
            await translator.StopContinuousRecognitionAsync().ConfigureAwait(false);
            translator.Recognizing -= RecognizingTranslationHandler;
            translator.Recognized -= RecognizedTranslationHandler;
            translator.SpeechStartDetected -= SpeechStartDetectedHandler;
            translator.SpeechEndDetected -= SpeechEndDetectedHandler;
            translator.Canceled -= CanceledTranslationHandler;
            translator.SessionStarted -= SessionStartedHandler;
            translator.SessionStopped -= SessionStoppedHandler;
            translator.Dispose();
            translator = null;
            recognizedString = "Speech Translator is now stopped.";
            Debug.Log("Speech Translator is now stopped.");
        }
    }
}
