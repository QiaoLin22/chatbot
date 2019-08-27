using System.Collections;
using System.Collections.Generic;
using IBM.Watson.DeveloperCloud.UnitTests;
using IBM.Watson.DeveloperCloud.Services.Conversation.v1;
using IBM.Watson.DeveloperCloud.Utilities;
using IBM.Watson.DeveloperCloud.Logging;
using FullSerializer;
using System.IO;
using System;
using UnityEngine.UI;
using IBM.Watson.DeveloperCloud.Connection;
using UnityEngine;
using UnityEngine.Video;


[AddComponentMenu("Chatbot/Example Scripts/AIML Test Chat")]
public class ConversationMark : MonoBehaviour
{
    private string _username = null;
    private string _password = null;
    private string _workspaceId = "b7925dd8-391c-463a-81bf-d756cafdfe19";
        //private string _token = "<authentication-token>";

    private Conversation _conversation;
    private string _conversationVersionDate = "2017-05-26";

    private string[] _questionArray = { "what is your name?" };
    public Text _questiontext;
    private fsSerializer _serializer = new fsSerializer();
    private Dictionary<string, object> _context = null;
    private int _questionCount = -1;
    private bool _waitingForResponse = true;

    //mark
    public Image company;
    public Camera movecam;
    public GameObject model;

    public Text function;
    public InputField testquestion;
    public Text _answer;

    public AIMLTestChat chatbot;
    //chatbotmark
    private AIMLbot.Bot bot;
    private AIMLbot.User user;
    private AIMLbot.Request request;
    private AIMLbot.Result result;

    public RobotDance danceRob;
    public GameObject videodemo;
    public VideoPlayer video1;
    public bool videoon;
    public AudioSource videosound;

    public Test Marktest;

    void Start()

  {
      //  Debug.Log("aaaaa");
        LogSystem.InstallDefaultReactors();

            //  Create credential and instantiate service
        Credentials credentials = new Credentials("c20a3f60-9df3-4d5b-86e5-027ccde5c42c", "mnCUwtXFpNSl", "https://gateway.watsonplatform.net/conversation/api");

        _conversation = new Conversation(credentials);
        _conversation.VersionDate = _conversationVersionDate;

        bot = new AIMLbot.Bot();
        user = new AIMLbot.User("User", bot);
        request = new AIMLbot.Request("", user, bot);
        result = new AIMLbot.Result(user, bot, request);
        bot.loadSettings(Application.dataPath + "/Chatbot/Program #/config/Settings.xml");
        // Load AIML files from AIML path defined in Settings.xml
        bot.loadAIMLFromFiles();
        if (bot != null)
            bot.UseJavaScript = true;


        //   TextToSpeechMark ttsm = gameObject.GetComponent<TextToSpeechMark>();
        //  AskQuestion();
    }

    private void Update()
    {
   //     if (_questiontext.text != "")
    //       // Debug.Log(_questiontext.text);
      //      AskQuestion();
    }
    public void AskQuestion()
    {
        MessageRequest messageRequest = new MessageRequest()
            {
            input = new Dictionary<string, object>()
            {
            { "text", /*_questiontext.text*/ testquestion.text }
            },
            context = _context
            
            };

            if (!_conversation.Message(OnSuccess, OnFail, _workspaceId, messageRequest))
                Log.Debug("TestConversation.AskQuestion()", "Failed to message!");

        }


    private void OnSuccess(object resp, Dictionary<string, object> customData)
    {

        SpeechToTextMark sttm = gameObject.GetComponent<SpeechToTextMark>();
        sttm.Active = false;
        Log.Debug("TestConversation.OnMessage()", "Conversation: Message Response: {0}", customData["json"].ToString());


        string[] splitanswer = customData["json"].ToString().Split(new string[] { "[", "]" },StringSplitOptions.None);
     

        var newstring = splitanswer[5];
        Debug.Log("new string " + newstring);
  
       // var index = newstring.Length;
       // newstring = newstring.Remove(0, 2);
       // newstring = newstring.Remove(index-2, 1);
 
        //intent
        string[] intents = customData["json"].ToString().Split(new string[] { "intent", "confidence" }, StringSplitOptions.None);
        var index1 = intents[2].Length;
        var intent = intents[2].Remove(0, 3);
        intent = intent.Remove(index1 - 6, 3);
        Debug.Log("intent!!!!" + intent);
        //greeting
        if (intent != "fal" /*&& !newstring.StartsWith("I didn't understand") && !newstring.StartsWith("Can you reword") */ )
        {
            _questiontext.text = "";
            _answer.text = newstring;
            if(intent == "打招呼")
            {
                function.gameObject.SetActive(true);
                company.gameObject.SetActive(false);
          
            }
            else if (intent == "公司信息" )
            {
             //   movecam.transform.localPosition = new Vector3(-0.45f, 1.87f, -5.98f);
             //   model.transform.localEulerAngles = new Vector3(0, 220f, 0);
                company.gameObject.SetActive(true);
              //  function.gameObject.SetActive(false);
                videodemo.gameObject.SetActive(false);
            }
            else if (intent == "视频" )
            {
                videodemo.gameObject.SetActive(true);
                company.gameObject.SetActive(false);
                function.gameObject.SetActive(false);
                StartCoroutine(videodemoshow());
            }
          
            else if (intent == "Cityname" || intent == "No" || intent == "Weather" || intent == "goodbyes")
            {
             //   movecam.transform.localPosition = new Vector3(0f, 1.87f, -5.98f);
             //   model.transform.localEulerAngles = new Vector3(0, 174f, 0);
                company.gameObject.SetActive(false);
             //   function.gameObject.SetActive(false);
                videodemo.gameObject.SetActive(false);
            }
            else if (intent == "跳舞")
            {
                  
                danceRob.StartCoroutine(danceRob.Losetime());
                danceRob.StartCoroutine(danceRob.DanceTime());
                company.gameObject.SetActive(false);
            //    function.gameObject.SetActive(false);
                videodemo.gameObject.SetActive(false);
            }
            Debug.Log("intentttttttt!!!!!!!!!!!!!!!!" + intent);
   
                TextToSpeechMark ttsm = gameObject.GetComponent<TextToSpeechMark>();
            // ttsm.Synthesize(_answer.text);
            Marktest.speaktuling(_answer.text);

            fsData fsdata = null;
            fsResult r = _serializer.TrySerialize(resp.GetType(), resp, out fsdata);
            if (!r.Succeeded)
                throw new WatsonException(r.FormattedMessages);

            //  Convert fsdata to MessageResponse
            MessageResponse messageResponse = new MessageResponse();
            //mark
            // string intentss = messageResponse.intents[0].intent;

            // Debug.Log("finaly!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!" + intentss);


            object obj = messageResponse;

            r = _serializer.TryDeserialize(fsdata, obj.GetType(), ref obj);
            if (!r.Succeeded)
                throw new WatsonException(r.FormattedMessages);

            object _tempContext = null;
            //  Set context for next round of messaging
            (resp as Dictionary<string, object>).TryGetValue("context", out _tempContext);


            if (_tempContext != null)
            {
                _context = _tempContext as Dictionary<string, object>;


            }
            else
                Log.Debug("TestConversation.OnMessage()", "Failed to get context");

            // Test(messageResponse != null);
            _waitingForResponse = false;


            //  TextToSpeechMark ttsm = gameObject.GetComponent<TextToSpeechMark>();
            //  ttsm.Synthesize("");

            //  Convert resp to fsdata
        }


        else
        {
            startTuling(_questiontext.text);
            //  company.gameObject.SetActive(false);
            /* request.rawInput = _questiontext.text;
             _questiontext.text = "";
             request.StartedOn = DateTime.Now;
             result = bot.Chat(request);
             _answer.text = result.Output;
               TextToSpeechMark ttsm = gameObject.GetComponent<TextToSpeechMark>();
              ttsm.Synthesize(_answer.text);*/
        }
      //  _questiontext.text = "";
    }
    private void startTuling(string key)
    {

        StartCoroutine(Marktest.Tulingbot(key));
    }
    public void stopvideo()
    {
        function.gameObject.SetActive(true);
        TextToSpeechMark ttsm = gameObject.GetComponent<TextToSpeechMark>();
        ttsm.Synthesize("What can I do for you?");
        model.transform.localPosition = new Vector3(0, 0, 0);
        videodemo.gameObject.SetActive(false);
        videosound.Stop();
        videoon = false;
        
    }
    public IEnumerator videodemoshow()
    {
        model.transform.localPosition = new Vector3(0, 1, 0);
        videosound.Play();
        SpeechToTextMark sttm = gameObject.GetComponent<SpeechToTextMark>();
        sttm.Active = true;
        videoon = true;
        yield return new WaitForSeconds(160);
        function.gameObject.SetActive(true);
        //  video1.Pause();
        videosound.Stop();
        sttm.Active = true;
        model.transform.localPosition = new Vector3(0, 0, 0);
        videodemo.gameObject.SetActive(false);
        videoon = false;
    }
    public void chatbotanswer()
    {
        request.rawInput = _questiontext.text;
        request.StartedOn = DateTime.Now;
        result = bot.Chat(request);
        _answer.text = result.Output;
      //  TextToSpeechMark ttsm = gameObject.GetComponent<TextToSpeechMark>();
       // ttsm.Synthesize(_answer.text);
    }

        private void OnFail(RESTConnector.Error error, Dictionary<string, object> customData)
        {
            Log.Error("TestConversation.OnFail()", "Error received: {0}", error.ToString());
        }
    }

