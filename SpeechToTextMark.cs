using FullSerializer;
using IBM.Watson.DeveloperCloud.Connection;
using IBM.Watson.DeveloperCloud.Logging;
using IBM.Watson.DeveloperCloud.Services.SpeechToText.v1;
using IBM.Watson.DeveloperCloud.Utilities;
using IBM.Watson.DeveloperCloud.DataTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;


public class SpeechToTextMark : MonoBehaviour {
    [SerializeField]
    private int m_RecordingRoutine = 0;

    private string m_MicrophoneID = null;
    private AudioClip m_Recording = null;
    private int m_RecordingBufferSize = 1;
    private int m_RecordingHZ = 22050;
    public Text function;
    public Image company;
    public GameObject model;
    public GameObject videodemo;
    public bool videoon = false;
    public RobotDance danceRob;
    public Button StartRec;

    public Text OutputText;
    public RobotDance dancerobot;
    public Test Marktest;
    public Text test;
    public InputField test11;
    public AudioSource videosound;
    public GameObject clothred;
    public GameObject clothwhitea;

    public BaiduApi baiduloc;
    //    public GameObject webview;

    private bool weatheron;

    public GameObject map;
    public GameObject background;
    public Text outputtest;
    //   public Text texttoread;

    public ConversationMark convermark1;
    private SpeechToText m_SpeechToText;
    //youhuihuodong
    public string youhui = "";
    public string Hotel = "";
    public GameObject dialogbox;
    public Text dialog;
    public bool _realstart = false;
    public TestBaidu baidapi;
    public Image hotelmap;
    public bool _whitecloth;

    public string videourl = "";
    public string imageurl = "";
    //songplay
    public AudioSource songsource;



    //    public GameObject BowlMove;
    //  public GameObject PlaneMove;
    //   public CubePresentationScript planescript;


    //  public Image Cursor;
    // Use this for initialization
    void Start() {
        GlobPara glob = GameObject.Find("GlobalGame").GetComponent<GlobPara>();
        videourl = glob.videourl1;
        imageurl = glob.imageurl1;

        

    }

    // Update is called once per frame
    void Update() {
     //     Debug.Log("active status !!!!!!!!!!!!!!!!" + Active);
      //     Debug.Log("active status !!!!!!!!!!!!!!!!" + m_SpeechToText.IsListening);
      //    Active = true;
     //      if (convermark1.videoon)
     //          Active = true;
    }

    public bool Active
    {
        get { return m_SpeechToText.IsListening; }
        set
        {
            if (value && !m_SpeechToText.IsListening)
            {
                m_SpeechToText.DetectSilence = true;
                m_SpeechToText.EnableWordConfidence = false;
                m_SpeechToText.EnableTimestamps = false;
                m_SpeechToText.SilenceThreshold = 0.03f;
                m_SpeechToText.MaxAlternatives = 1;
                //m_SpeechToText.EnableContinousRecognition = true;
                m_SpeechToText.EnableInterimResults = true;
                // m_SpeechToText.OnError = OnError;
                m_SpeechToText.StartListening(OnRecognize);
            }
            else if (!value && m_SpeechToText.IsListening)
            {
                m_SpeechToText.StopListening();
            }
        }
    }
    public void activeon()
    {
        //Active = true;
        // Active = false;
        //   dancerobot.StartCoroutine(dancerobot.Losetime());
        // dancerobot.StartCoroutine(dancerobot.DanceTime());

    }
    public void StartRecording()
    {
        if (m_RecordingRoutine == 0)
        {
            UnityObjectUtil.StartDestroyQueue();
            m_RecordingRoutine = Runnable.Run(RecordingHandler());

        }
    }

    private void StopRecording()
    {
        if (m_RecordingRoutine != 0)
        {
            Microphone.End(m_MicrophoneID);
            Runnable.Stop(m_RecordingRoutine);
            m_RecordingRoutine = 0;
        }
    }

    private void OnError(string error)
    {
        //Active = false;

        Log.Debug("ExampleStreaming", "Error! {0}", error);
    }

    private IEnumerator RecordingHandler()
    {

        m_Recording = Microphone.Start(m_MicrophoneID, true, m_RecordingBufferSize, m_RecordingHZ);
        yield return null;      // let m_RecordingRoutine get set..

        if (m_Recording == null)
        {
            StopRecording();
            yield break;
        }

        bool bFirstBlock = true;
        int midPoint = m_Recording.samples / 2;
        float[] samples = null;

        while (m_RecordingRoutine != 0 && m_Recording != null)
        {

            int writePos = Microphone.GetPosition(m_MicrophoneID);
            if (writePos > m_Recording.samples || !Microphone.IsRecording(m_MicrophoneID))
            {
                Log.Error("MicrophoneWidget", "Microphone disconnected.");

                StopRecording();
                yield break;
            }

            if ((bFirstBlock && writePos >= midPoint)
                || (!bFirstBlock && writePos < midPoint))
            {
                // front block is recorded, make a RecordClip and pass it onto our callback.
                samples = new float[midPoint];
                m_Recording.GetData(samples, bFirstBlock ? 0 : midPoint);

                AudioData record = new AudioData();
                record.MaxLevel = Mathf.Max(samples);
                record.Clip = AudioClip.Create("Recording", midPoint, m_Recording.channels, m_RecordingHZ, false);
                record.Clip.SetData(samples, 0);

                m_SpeechToText.OnListen(record);

                bFirstBlock = !bFirstBlock;
            }
            else
            {
                // calculate the number of samples remaining until we ready for a block of audio, 
                // and wait that amount of time it will take to record.
                int remaining = bFirstBlock ? (midPoint - writePos) : (m_Recording.samples - writePos);
                float timeRemaining = (float)remaining / (float)m_RecordingHZ;

                yield return new WaitForSeconds(timeRemaining);
            }

        }

        yield break;
    }

    private void OnRecognize(SpeechRecognitionEvent result)
    {
        ConversationMark convermark = gameObject.GetComponent<ConversationMark>();
        if (result != null && result.results.Length > 0)
        {
            foreach (var res in result.results)
            {
                foreach (var alt in res.alternatives)
                {
                    string text = alt.transcript;
                    Log.Debug("ExampleStreaming", string.Format("{0} ({1}, {2:0.00})\n", text, res.final ? "Final" : "Interim", alt.confidence));
                    if (res.final)
                    {
                        //Active = false;
                        OutputText.text = alt.transcript;
                        //  alt.transcript = alt.transcript.Replace(" ", "");
                        OutputText.text = OutputText.text.Replace(" ", "");
                        outputtest.text = alt.transcript;

                        /*     if (weatheron)
                             {
                                 StartCoroutine(Marktest.GetWeather(alt.transcript));
                                 weatheron = false;
                             }*/
                        /*mark    if (convermark.videoon)
                            {
                                if (alt.transcript.Contains("stop") || alt.transcript.Contains("quit") || alt.transcript.Contains("back"))
                                {     
                                    convermark.stopvideo();
                                }
                                else
                                {
                                    Active = true;
                                }
                            }*/
                        //     else
                        //   {

                        // StartConversation();
                        //  startTuling(OutputText.text);
                        //  startchatbot();
                        //   }

                        if (OutputText.text.Contains("你叫什么") || OutputText.text.Contains("名字") || OutputText.text.Contains("叫什么") || OutputText.text.Contains("你是谁"))
                        {
                            string greeting = "你好，我是光子机器人，有什么可以为您服务？";
                            Marktest.speaktuling(greeting);


                            map.gameObject.SetActive(false);
                            //          webview.transform.localPosition = new Vector3(5f, 1.414f, -0.461f);
                        }
                        else if (OutputText.text.Contains("哪儿") || OutputText.text.Contains("哪") || OutputText.text.Contains("位置") || OutputText.text.Contains("在哪") || OutputText.text.Contains("在那") || OutputText.text.Contains("那"))
                        {
                            string greeting = "好的，麻烦您稍等";
                            Marktest.speaktuling(greeting);
                            StartCoroutine(baiduloc.GetToken(OutputText.text));
                            //     webview.transform.localPosition = new Vector3(5f, 1.414f, -0.461f);
                        }
                        else if (OutputText.text.Contains("公司信息") || OutputText.text.Contains("信息") || OutputText.text.Contains("公司") || OutputText.text.Contains("技术") || OutputText.text.Contains("介绍"))
                        {
                            string greeting = "深圳光子晶体是一家致力于全透明显示的科技公司。在过去，由于物理法则的限制，人们无法在透明的玻璃上实现成像。通过我们公司的纳米专利技术，可以将投影仪的光呈现在玻璃上并选择性的透过其他光源。这一技术可以广泛的应用在多个场合，比如商场橱窗，汽车抬头显示等。";
                            Marktest.speaktuling(greeting);

                            map.gameObject.SetActive(false);
                            //      webview.transform.localPosition = new Vector3(5f, 1.414f, -0.461f);
                        }
                        else if (OutputText.text.Contains("跳舞") || OutputText.text.Contains("跳支舞") || OutputText.text.Contains("跳个舞") || OutputText.text.Contains("舞蹈") || OutputText.text.Contains("舞"))
                        {
                            string greeting = "好的，请您稍等";
                            Marktest.speaktuling(greeting);
                            danceRob.StartCoroutine(danceRob.Losetime());
                            danceRob.StartCoroutine(danceRob.DanceTime());
                            //  company.gameObject.SetActive(false);
                            //    function.gameObject.SetActive(false);
                            videodemo.gameObject.SetActive(false);

                            map.gameObject.SetActive(false);
                            //     webview.transform.localPosition = new Vector3(5f, 1.414f, -0.461f);
                        }
                        else if (OutputText.text.Contains("视频") || OutputText.text.Contains("视频演示") || OutputText.text.Contains("演示") || OutputText.text.Contains("播放"))
                        {
                            string greeting = "好的，请您稍等";
                            Marktest.speaktuling(greeting);
                            videodemo.gameObject.SetActive(true);
                            //  company.gameObject.SetActive(false);
                            //   function.gameObject.SetActive(false);
                            StartCoroutine(videodemoshow());
                            map.gameObject.SetActive(false);
                            //        webview.transform.localPosition = new Vector3(5f, 1.414f, -0.461f);
                        }
                        /*     else if (OutputText.text.Contains("古董"))
                             {
                                 string greeting = "好的，请您稍等";
                                 Marktest.speaktuling(greeting);
                                 LoadNew();
                                 map.gameObject.SetActive(false);
                                 webview.transform.localPosition = new Vector3(5f, 1.414f, -0.461f);
                             }*/
                        /*     else if (OutputText.text.Contains("飞机"))
                             {
                                 string greeting = "好的，请您稍等";
                                 Marktest.speaktuling(greeting);
                                 LoadNew1();
                                 map.gameObject.SetActive(false);
                                 webview.transform.localPosition = new Vector3(5f, 1.414f, -0.461f);
                             }*/
                        else if (OutputText.text.Contains("停止") || OutputText.text.Contains("退出") || OutputText.text.Contains("取消") || OutputText.text.Contains("停") || OutputText.text.Contains("返回"))
                        {
                            string greeting = "好的";
                            Marktest.speaktuling(greeting);
                            stopvideo();
                            map.gameObject.SetActive(false);
                            //    webview.transform.localPosition = new Vector3(5f, 1.414f, -0.461f);
                            model.gameObject.SetActive(true);
                            //       PlaneMove.transform.localPosition = new Vector3(14.3f, 0.43f, 30.15f);
                            //      BowlMove.transform.localPosition = new Vector3(14.3f, 0.997f, 0.363f);
                            model.transform.localPosition = new Vector3(0, 0, 0);
                            //       Cursor.gameObject.SetActive(false);
                        }

                        else
                        {
                            map.gameObject.SetActive(false);
                            //    webview.transform.localPosition = new Vector3(5f, 1.414f, -0.461f);
                            startTuling(OutputText.text);
                            //     Cursor.gameObject.SetActive(false);

                        }



                        /*  if (alt.transcript.Contains("weather") || alt.transcript.Contains("temperature")|| alt.transcript.Contains("degree")|| alt.transcript.Contains("climate"))
                          {
                              Startunderstand();
                          }
                          else
                          {
                              //
                           //   Startunderstand();
                              StartConversation();
                          }*/
                    }
                    OutputText.text = "";
                }
            }
        }
    }
    //message test

    public void startTuling(string key)
    {
#if UNITY_ANDROID
        StartCoroutine(Marktest.Tulingbot(key));
#endif
    }

    public void startTuling11()
    {
#if UNITY_ANDROID
        StartCoroutine(Marktest.Tulingbot(test11.text));
#endif
    }
    private void startchatbot()
    {
        ConversationMark convermark = gameObject.GetComponent<ConversationMark>();
        convermark.chatbotanswer();
    }
    private void Startunderstand()
    {
        NatureUnderMark UnderMark = gameObject.GetComponent<NatureUnderMark>();
        UnderMark.StartUnderstand();
    }
    public void stopvideo()
    {
        //function.gameObject.SetActive(true);

        // model.transform.localPosition = new Vector3(0, 0, 0);
        videodemo.gameObject.SetActive(false);
        videosound.Stop();
        videoon = false;

    }
    public void StopImage()
    {

        map.gameObject.SetActive(false);
        videoon = false;

    }
    public void Testvideo(){
        videoon = true;
        var videop = videodemo.GetComponent<UnityEngine.Video.VideoPlayer>();
        videop.url = videourl;

        videodemo.gameObject.SetActive(true);
      

    }

    public void TestPic()
    {
        //     string filePath = imageurl;
        videoon = true;
        string filePath = imageurl;
        Texture2D tex = null;
        byte[] fileData;

        if (File.Exists(filePath))
        {
            fileData = File.ReadAllBytes(filePath);
            tex = new Texture2D(2, 2);
            tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
            Debug.Log(tex.name);
            map.GetComponent<Renderer>().material.mainTexture = tex;
        }
      //  return tex;
    }
    public IEnumerator videodemoshow()
    {
       // model.transform.localPosition = new Vector3(0, 1, 0);
        videosound.Play();
        //Active = true;
        videoon = true;
        yield return new WaitForSeconds(160);
       // function.gameObject.SetActive(true);
        //  video1.Pause();
        videosound.Stop();
        //Active = true;
       // model.transform.localPosition = new Vector3(0, 0, 0);
        videodemo.gameObject.SetActive(false);
        videoon = false;
    }
    public void StartConversation1()
    {
        //mark
        // Active = false;
        test.text = test.text.Replace(" ", "");
        if (test.text != "")
        {

            if (_realstart)
            {

                if (!videoon)
                {


                    if (test.text.Contains("你叫什么") || test.text.Contains("名字") || test.text.Contains("叫什么") || test.text.Contains("你是谁"))
                    {
                        function.gameObject.SetActive(true);
                        background.gameObject.SetActive(true);
                        map.gameObject.SetActive(false);
                        hotelmap.gameObject.SetActive(false);
                        string greeting = "你好，我是光子机器人，有什么可以为您服务？";
                        dialog.text = greeting;
                        Marktest.speaktuling(greeting);

                    }
                    else if (test.text.Contains("酒店地图") || test.text.Contains("地图"))
                    {
                        function.gameObject.SetActive(true);
                        background.gameObject.SetActive(true);
                        hotelmap.gameObject.SetActive(true);
                        string greeting = "西会议室在您左前方,东会议室在您右前方,收银区在您左方,品茶区在您右方";
                        dialog.text = greeting;
                        Marktest.speaktuling(greeting);

                    }
                /*    else if (test.text.Contains("衣服"))
                    {
                        if (_whitecloth)
                        {
                            clothwhitea.gameObject.SetActive(false);
                            clothred.gameObject.SetActive(true);
                            _whitecloth = false;
                        }
                        else
                        {
                            clothwhitea.gameObject.SetActive(true);
                            clothred.gameObject.SetActive(false);
                            _whitecloth = true;
                        }

                        function.gameObject.SetActive(true);
                        background.gameObject.SetActive(true);
                        //   hotelmap.gameObject.SetActive(true);

                        baidapi._isListen = true;


                    }*/

                    else if (test.text.Contains("优惠活动") || test.text.Contains("优惠") || test.text.Contains("活动"))
                    {
                        function.gameObject.SetActive(true);
                        background.gameObject.SetActive(true);
                        map.gameObject.SetActive(false);
                        hotelmap.gameObject.SetActive(false);
                        string greeting = youhui;
                        dialog.text = greeting;
                        Marktest.speaktuling(greeting);
                    }
                    else if (test.text.Contains("酒店信息") || test.text.Contains("信息") || test.text.Contains("技术") || test.text.Contains("民宿") || test.text.Contains("酒店介绍") || test.text.Contains("民宿介绍") || test.text.Contains("介绍下酒店") || test.text.Contains("介绍下民宿"))
                    {
                        function.gameObject.SetActive(true);
                        background.gameObject.SetActive(true);
                        map.gameObject.SetActive(false);
                        hotelmap.gameObject.SetActive(false);
                        string greeting = Hotel;
                        //       string greeting = "您好，这里是AI民宿，为人工智能高科技民宿，内涵21世界最巅峰科技，是上海首家集人工智能机器人及物联网智能家居于一体的高科技民宿，" +
                        //         "AI民宿拼音为爱民宿，让民宿充满爱，给您家一样的温暖，给您不一样的爱！AI民宿位于美丽富饶的国际金融大都市上海，旁于世界闻名乐趣无穷的迪士尼，落于民风淳朴热情好客的新场仁义村。外聚天地亘古之造化，山水相依，风景秀丽；内涵当代科技之典范，人工智能，智能家居。内外镶嵌，巧夺天工！古今融合，前所未有！实为居家、旅游、出差之佳境，休闲、娱乐、散心之圣地。AI民宿，爱民宿，在此，竭诚恭候各位五湖四海的贵宾前来下榻，让我们带您穿越时空，在全身心完全融入古韵风情的同时，带您尽情体验当代人工智能高科技的诸多震撼。住民宿，我们就在爱民宿！给您不一样的感觉，给您不一样的爱！";

                        dialog.text = greeting;
                        Marktest.speaktuling(greeting);
                    }
                    else if (test.text.Contains("仁义") || test.text.Contains("村") || test.text.Contains("仁义村") || test.text.Contains("我们村") || test.text.Contains("介绍下这里"))
                    {
                        function.gameObject.SetActive(true);
                        background.gameObject.SetActive(true);
                        map.gameObject.SetActive(false);
                        hotelmap.gameObject.SetActive(false);
                        // string greeting = Hotel;
                        string greeting = "您好，我们这是仁义村,隶属于上海市浦东新场镇，于2002年11月由原仁义村，范桥村合并组成。东与坦南村，蒋桥村相接，西与航头镇福善村，鹤东村相连。南与蒋桥村和新卫村接壤，北同坦西村，坦南村相交，“仁义礼智信”为孔孟之道，儒家“五常”。与五行学说“金木水火土”，“梅花篆字”梅报五福（平安、健康、幸福、快乐、长寿）共同成为" +
                            "中国千年文化价值体系中的最核心因素。三字经之中的“曰仁义，礼智信。此五常，不容紊。”值得当代的我们时刻深思学习，故此，仁义村起名仁义，寓意我们不忘传统，不忘根本，不忘仁义，方能历久弥新，更上一层楼！！";
                        dialog.text = greeting;
                        Marktest.speaktuling(greeting);
                    }
                    else if (test.text.Contains("迪士尼怎么走") || test.text.Contains("迪士尼路线")  )
                    {
                        function.gameObject.SetActive(true);
                        background.gameObject.SetActive(true);
                        map.gameObject.SetActive(false);
                        hotelmap.gameObject.SetActive(false);
                        // string greeting = Hotel;
                        string greeting = "从此前往迪士尼，出门往北直走，然后到头右转，进入申江南路，右转直走，到达与航三路交叉口，坐乘16号地铁4站到罗山路，然后转乘11号地铁3站路底站迪士尼，全城约50分钟。" +
                            "或者在申江南路打的，全程半小时，约55元。";
                        dialog.text = greeting;
                        Marktest.speaktuling(greeting);
                    }

                    else if (test.text.Contains("介绍下周边") || test.text.Contains("附近")  || test.text.Contains("景点"))
                    {
                        function.gameObject.SetActive(true);
                        background.gameObject.SetActive(true);
                        map.gameObject.SetActive(false);
                        hotelmap.gameObject.SetActive(false);
                        // string greeting = Hotel;
                        string greeting = "这里是浦东新区新场仁义村，附近有迪士尼，新场古镇，天主堂，东岳观，杜敬之宅，千秋桥，上海中优城市广场，渔乐湾生态园，草莓园，傅雷故居，" +
                            "疯狂多肉大棚，上海野生动物园，薰衣草园、玫瑰园等等，绝对让您玩的愉快，不虚此行";
                        dialog.text = greeting;
                        Marktest.speaktuling(greeting);
                    }

                    else if (test.text.Contains("迪士尼介绍") || test.text.Contains("介绍迪士尼") || test.text.Contains("迪士尼里有什么好玩的") || test.text.Contains("迪士尼有什么好玩的"))
                    {
                        function.gameObject.SetActive(true);
                        background.gameObject.SetActive(true);
                        map.gameObject.SetActive(false);
                        hotelmap.gameObject.SetActive(false);
                        // string greeting = Hotel;
                        string greeting = "迪士尼是上午九点半开园，去玩前，建议做好功课，比如喜欢哪些项目。创极速光轮、雷鸣山漂流，七个小矮人过矿山车，翱翔天空这些都超级火爆而且好玩，这几个项目也有快速通行证的，就是可以不用排队直接玩。建议进园后挑自己喜欢的，直接奔向快速通行证处。另外，快速通行证是两个小时之后才能取第二次。加勒比海盗特别好玩，" +
                            "船很大，一般排除时间比较短。晚上的城堡的焰火很漂亮，八点半开始，半个小时。中午一点钟有迪士尼巡游队伍。还有，每个项目附近都迪士尼的人偶，可以拍照。十点钟出园还有地铁，16号前最晚10点半始发，请注意地铁换乘时间。";
                        dialog.text = greeting;
                        Marktest.speaktuling(greeting);
                    }

                    else if (test.text.Contains("动物园介绍") || test.text.Contains("介绍野生动物园") || test.text.Contains("介绍动物园") || test.text.Contains("介绍下上海野生动物园") || test.text.Contains("介绍一下上海野生动物园"))
                    {
                        function.gameObject.SetActive(true);
                        background.gameObject.SetActive(true);
                        map.gameObject.SetActive(false);
                        hotelmap.gameObject.SetActive(false);
                        // string greeting = Hotel;
                        string greeting = "上海野生动物园居住着大熊猫,金丝猴,金毛羚牛,朱鹮,长颈鹿,斑马,羚羊,白犀牛,猎豹等来自国内外的珍稀野生动物200余种，上万余只。园区分为车入区和步行区两大参观区域。" +
                            "步行区，不仅可以观赏到大熊猫,非洲象,长颈鹿,黑猩猩,长臂猿,朱鹮等众多珍稀野生动物，更有诸多特色的动物行为展示和互动体验呈现。 车入区为动物散放养展示形式，保持着人在笼中，动物自由的展览模式，给动物更多的自由空间。使您身临其境的感受一群群斑马,羚羊,角马,犀牛等食草动物簇拥在一起悠闲觅食；又能领略猎豹,东北虎," +
                            "非洲狮,熊,狼等大型猛兽部落展现野性雄姿。另外，园内还设有5座功能各异的表演场馆。身怀绝技的俄罗斯专业团队携各路动物明星演艺魔幻之旅；猎豹,格力犬,蒙古马,鸵鸟等竞技速力赛，让您一饱眼福；海洋精灵加州海狮和美洲海狮讲述着来自大海的传说。";
                        greeting.Trim();
                        dialog.text = greeting;
                        Marktest.speaktuling(greeting);
                    }
                    else if (test.text.Contains("动物园路线") || test.text.Contains("动物园怎么走") || test.text.Contains("怎么去动物园") || test.text.Contains("动物园怎么去"))
                    {
                        function.gameObject.SetActive(true);
                        background.gameObject.SetActive(true);
                        map.gameObject.SetActive(false);
                        hotelmap.gameObject.SetActive(false);
                        // string greeting = Hotel;
                        string greeting = "从此前往动物园，出门往北直走，然后到头右转，进入康新公路，右转直走，到达与航三路交叉口左转，坐乘16号地铁1站到达野生动物园。或者直接在康新公路打地10分钟约25元。祝您旅途愉快。";
                        dialog.text = greeting;
                        Marktest.speaktuling(greeting);
                    }

                    else if (test.text.Contains("视频") || test.text.Contains("视频演示") || test.text.Contains("演示"))
                    {

                        string greeting = "好的，请您稍等";
                        dialog.text = greeting;
                        dialogbox.gameObject.SetActive(false);
                        Marktest.speaktuling(greeting);
                        videodemo.gameObject.SetActive(true);
                        company.gameObject.SetActive(false);
                        function.gameObject.SetActive(false);
                        background.gameObject.SetActive(false);
                        map.gameObject.SetActive(false);
                        hotelmap.gameObject.SetActive(false);
                        Testvideo();
                    }

                    else if (test.text.Contains("图片") || test.text.Contains("图片展示"))
                    {

                        string greeting = "好的，请您稍等";
                        dialog.text = greeting;
                        dialogbox.gameObject.SetActive(false);
                        Marktest.speaktuling(greeting);
                        company.gameObject.SetActive(false);
                        function.gameObject.SetActive(false);
                        background.gameObject.SetActive(false);
                        map.gameObject.SetActive(true);
                        videodemo.gameObject.SetActive(false);
                        hotelmap.gameObject.SetActive(false);
                        TestPic();
                    }
                    else if (test.text.Contains("停止") || test.text.Contains("退出") || test.text.Contains("取消") || test.text.Contains("停"))
                    {
                        function.gameObject.SetActive(true);
                        background.gameObject.SetActive(true);
                        map.gameObject.SetActive(false);
                        dialogbox.gameObject.SetActive(true);
                        //  clothred.gameObject.SetActive(true);
                        hotelmap.gameObject.SetActive(false);
                        string greeting = "好的";
                        dialog.text = greeting;
                        Marktest.speaktuling(greeting);
                        stopvideo();
                    }

                    else
                    {
                        hotelmap.gameObject.SetActive(false);
                        function.gameObject.SetActive(true);
                        background.gameObject.SetActive(true);
                        map.gameObject.SetActive(false);
                        startTuling(test.text);
                        OutputText.text = "hecheng:" + test.text;
                        // test.text = "";
                    }
                }



                else
                {
                    if (test.text.Contains("停止") || test.text.Contains("退出") || test.text.Contains("取消") || test.text.Contains("停"))
                    {
                        hotelmap.gameObject.SetActive(false);
                        function.gameObject.SetActive(true);
                        background.gameObject.SetActive(true);
                        map.gameObject.SetActive(false);
                        dialogbox.gameObject.SetActive(true);
                        songsource.Stop();
                        hotelmap.gameObject.SetActive(false);
                        string greeting = "好的";
                        dialog.text = greeting;
                        Marktest.speaktuling(greeting);
                        videoon = false;
                        stopvideo();
                        StopImage();
                    }

                    else
                    {

                        baidapi._isListen = true;

                    }
                }
            }
                
           
            else
            {
                if (test.text.Contains("你好小娜") || test.text.Contains("小娜") || test.text.Contains("小那") || test.text.Contains("你好小那") || test.text.Contains("你好小男") || test.text.Contains("小男")
                || test.text.Contains("你好小南") || test.text.Contains("小南") || test.text.Contains("晓") || test.text.Contains("香奈儿") || test.text.Contains("小呢") || test.text.Contains("想呢") || test.text.Contains("晓呢"))
                {
                    string greeting = "你好,有什么可以为您服务";
                    dialog.text = greeting;
                    _realstart = true;
                    Marktest.speaktuling(greeting);
                  
                }
                else
                {
                    baidapi._isListen = true;
                }
            }
        }



        //OutputText.text = "";
    }


    /*public void LoadNew()
    {
        BowlMove.transform.localEulerAngles = new Vector3(0, 0, 0);
        BowlMove.transform.localPosition = new Vector3(0, 0.997f, 0.363f);
        PlaneMove.transform.localPosition = new Vector3(14.3f, 0.43f, 30.15f);
        model.transform.localPosition = new Vector3(5, 0, 0);
        Cursor.gameObject.SetActive(true);
    }*/
 /*   public void LoadNew1()
    {
        planescript.leftcount = 0;
        planescript.rightcount = 0;
        planescript.upcount = 0;
        PlaneMove.transform.localEulerAngles = new Vector3(0, 180f, 0);
        PlaneMove.transform.localPosition = new Vector3(0, 0.43f, 30.15f);
        BowlMove.transform.localPosition = new Vector3(14.3f, 0.997f, 0.363f);
        model.transform.localPosition = new Vector3(5, 0, 0);
        Cursor.gameObject.SetActive(false);
    }*/
    private void StartConversation()
    {
        //mark
        //Active = false;
          ConversationMark convermark = gameObject.GetComponent<ConversationMark>();

       
        print("startconversation");
        if (OutputText.text != "")
        {
            
            convermark.AskQuestion();
    
        }
        //OutputText.text = "";
    }
    private void GetCustomizations()
    {
        if (!m_SpeechToText.GetCustomizations(HandleGetCustomizations, OnFail))
            Log.Debug("ExampleSpeechToText.GetCustomizations()", "Failed to get customizations");
    }

    private void HandleGetCustomizations(Customizations customizations, Dictionary<string, object> customData)
    {
        Log.Debug("ExampleSpeechToText.HandleGetCustomizations()", "Speech to Text - Get customizations response: {0}", customData["json"].ToString());
    }
    private void OnFail(RESTConnector.Error error, Dictionary<string, object> customData)
    {
        Log.Error("ExampleAlchemyLanguage.OnFail()", "Error received: {0}", error.ToString());
    }
}

