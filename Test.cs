using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using System;
using Crosstales.RTVoice.Model;
using Crosstales.RTVoice.Demo;
//using SimpleWebBrowser;
using UnityEngine.SceneManagement;
using System.IO;
using LitJson;


public class Test : MonoBehaviour {
    public Animator anim;
    public TextToSpeechMark ttsm;
    public int Movecount = 1;
    public SpeechToTextMark sttmark;
    public bool MoveOn;
    public GameObject functions;
    public SpeakWrapper speaker;
    public Text answer;
    public Lipsync3D lipsync;
    public InputField xtest;
    public GameObject audioObject;
    public TestBaidu baiduapi;

    // public GameObject webview;
    public InputField weburl;

 //   public WebBrowser websearch;

    public AudioSource tulingaudio;
    public BaiduSpeech baidutalk;
    public Text dialog;

    public GameObject model;

    public Text text1;
    public Text text2;
    public Text text3;

    //test
    public Text test11;

    //Youhuihuodong


    // Use this for initialization
    void Start () {
    
        anim = gameObject.GetComponent<Animator>();
        baiduapi = gameObject.GetComponent<TestBaidu>();
        AudioSource audio1 = audioObject.GetComponent<AudioSource>();


}
    void Update()
    {
        baiduapi = gameObject.GetComponent<TestBaidu>();
        AudioSource audio1 = audioObject.GetComponent<AudioSource>();
        // baiduapi.StartListen_bool();
        if(baiduapi._isListen)
        {        baiduapi.StartVoice("{\"accept-audio-data\":true,\"vad.endpoint-timeout\":0,\"vad\":dnn,\"pid\":15361,\"disable-punctuation\":false,\"accept-audio-volume\":true}");
        }
        if (Input.GetKeyDown(KeyCode.Q))
         //if (baiduapi._isListen)
        // if (audio1.isPlaying)
        // if(audio1.clip)
        {
            anim.CrossFade("Stand", 0.1f);
            anim.SetBool("iswave", true);
            anim.SetBool("isstand", false);
            Debug.Log("wozaikongzhi");
        }
        else
        {
            anim.SetBool("iswave", false);
            anim.SetBool("isstand", true);
        }

// key control

        if (Input.GetKeyDown(KeyCode.Q)) {
            anim.CrossFade("Stand", 0.1f);
            anim.SetBool("iswave", true);
            anim.SetBool("isstand", false);
        }
        else
        {
            anim.SetBool("iswave", false);
            anim.SetBool("isstand", true);
        }
        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    anim.CrossFade("Combine1", 0.1f);

        //}
        if (Input.GetKeyDown(KeyCode.W))
        {
            anim.CrossFade("Stand", 0.1f);
            anim.SetBool("iscom1", true);
            anim.SetBool("isstand1", false);
        }
        else
        {
            anim.SetBool("iscom1", false);
            anim.SetBool("isstand1", true);
        }
       // if (Input.GetKeyDown(KeyCode.D))
       // {
       //     anim.CrossFade("Combine2", 0.1f);

      //  }
        if (Input.GetKeyDown(KeyCode.E))
        {
            anim.CrossFade("Stand", 0.1f);
            anim.SetBool("iscom2", true);
            anim.SetBool("isstand2", false);
        }
        else
        {
            anim.SetBool("iscom2", false);
            anim.SetBool("isstand2", true);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            anim.CrossFade("Stand", 0.1f);
            anim.SetBool("isran1", true);
            anim.SetBool("isstand3", false);
        }
        else
        {
            anim.SetBool("isran1", false);
            anim.SetBool("isstand3", true);
        }


    }


    public void HelloMovement()
    {

        anim.SetTrigger("Wave");
        string[] arr = new string[] { "" };
        anim.Play("Wave");
        baidutalk.SysncBaiduAudioWave(arr[UnityEngine.Random.Range(0, 2)]);
        sttmark.Active = true;
    }

    public void CloserHelloMovement()
    {
        functions.gameObject.SetActive(false);
    }

    public void ByeMovement()
    {
        sttmark.Active = false;
        
    }
    public void ChangeScene()
    {
        SceneManager.LoadScene("Setting");
    }

    public void randommove()
    {
       
        int i = UnityEngine.Random.Range(1, 4);
        anim.Play("Random3");
    }
    public void randommove2()
    {
        
        anim.Play("Random3");
    }

    public void randommove3(float length)
    {
        int i = UnityEngine.Random.Range(1, 3);
        if(i == 1)
        {
            float speed = 130f / (length * 24f);
            Debug.Log("Grush method is using!!!" + speed);
            anim.speed = speed;
        }
        else
        {
            float speed = 180f / (length * 24f);
            Debug.Log("Grush method is using!!!" + speed);
            anim.speed = speed;
        }

        anim.Play("combine"+i.ToString());
      

    }
    public void randommove4()
    {
        int i = UnityEngine.Random.Range(1, 3);
        anim.Play("Random3");
    }

    public IEnumerator GetWeather(string city)
    {
        WWW www = new WWW("api.openweathermap.org/data/2.5/find?q="+ city + "&units=metric&APPID=e2132d8c01b845076e67e323bfc04259");

        yield return www;
        
        print("testforweather" + www.size);
        if (www.size == 54)
        {
            ttsm.Synthesize("Sorry, please say the city name again");
        }
        else
        {
            string[] splitweather = www.text.Split(new string[] { ":", "," }, StringSplitOptions.None);
            string[] splitweather1 = www.text.Split(new string[] { "description", "icon" }, StringSplitOptions.None);
            string cityname = GetText(splitweather[10], 1);
            string temp = splitweather[18];
            string theday = DateTime.Now.DayOfWeek.ToString();
            string descrip = GetText(splitweather1[1], 3);
            //  string 
            string Weather = "The temperature of " + cityname + " on " + theday + " is " + temp + " and " + descrip;

            print(Weather);
            ttsm.Synthesize(Weather);
        }


    }
    public void starttulingtest(string text)
    {
        StartCoroutine(Tulingbot(xtest.text));
        
    }
    public void tullll(string key)
    {
       StartCoroutine( Tulingbot(key));
    }
    public IEnumerator Tulingbot(string key)
    {
#if UNITY_ANDROID
      //  test11.text =("start to check answer!!!!!!!!!!!!!!!!!"+ "tuling123.com/openapi/api?key=7837607eea3848e397a1864aaac10e78&info=" + key);
        string post_url = "www.tuling123.com/openapi/api?key=7837607eea3848e397a1864aaac10e78&info=" + key;
        WWW www = new WWW("http://"+post_url);

        yield return www;
        if (www.error != null)
        {
            Debug.Log(www.error);
            yield break;
        }

        print("tulingbot!!!" + www.text);
        OnGetResponse(www.text);
#endif
        yield return null;
     
    }
    public class TuLingResponseBase : IWebChatBotResponseBase
    {
        public int code;
        public string text;
        public string url;

        public int Code { get { return code; } }
        public string Text { get { return text; } }
        public string Url { get { return url; } }
    }
    public interface IWebChatBotResponseBase
    {
        string Text { get; }
        string Url { get; }
        int Code { get; }
    }

    public class NewList
    {
        public ArrayList news;
    }

    protected void OnGetResponse(string res)
    {
       
        var response = JsonUtility.FromJson<TuLingResponseBase>(res);
        print( response.text);
        print(response.code);
        print(response.url);
        //tianqi dao shang hai
        if (response.text.StartsWith("莱芜") || response.text.StartsWith("锦州"))
        {
            StartCoroutine(Tulingbot("上海天气"));
        }
        else {
            answer.text = response.text;

            print(response.text.Length);

            if (response.code == 200000)
            {

                speaktulingwithmap(answer.text, response.url);
            }
            else if (response.code == 302000)
            {
                JsonData _news = JsonMapper.ToObject(res);
                NewList _newslist;
                _newslist = new NewList();
                _newslist.news = new ArrayList();

                for (int i = 0; i < 4; i++)
                {
                    _newslist.news.Add(_news["list"][i]["article"].ToString());
                    answer.text += "。\n" + _news["list"][i]["article"].ToString();

                }

                Debug.Log("news" + _newslist.news[0]);
            }
            dialog.text = answer.text;
            test11.text = answer.text;
            answer.text = answer.text.Replace(" ", "");
            answer.text = answer.text.Replace("\n", "");
            speaktuling(answer.text);
        }
    }

    public void speaktuling(string key)
    {
        baidutalk.SysncBaiduAudio(key);
       // StartCoroutine(PlayClipTu());

       
        
    }
    public void speaktulingwithmap(string key, string url)
    {
        weburl.text = url;
        baidutalk.SysncBaiduAudio(key);
    }

    public void StopMove()
    {
        MoveOn = false;
        anim.Play("Stand");
    }
    
    public void PlayClipTu()
    {
        MoveOn = true;
        AudioSource source = audioObject.GetComponent<AudioSource>();
            print("+++++++++++++++++++++++++++" + source.name);
            
            source.spatialBlend = 0.0f;
            source.loop = false;
      
            if (source.clip != null)
                print("found clip!!!!!!!!!!!");
            else
                print("no clip!!!!!!!!!!!!");

            Debug.Log("Lenghttttttttttttttt+++++++++++" + source.clip.name);
            if (source.clip.length < 10f && source.clip.length > 0f)
            {

            anim.Play("Wave");
            }
            else if (source.clip.length >= 3f && source.clip.length < 5f)
            {
                float speed = 113f / (source.clip.length * 24f);
                anim.speed = speed;
              //  makrtest.MoveOn1 = true;
                randommove2();
            }
            else if (source.clip.length >= 5f && source.clip.length < 7f)
            {
                //   makrtest.MoveOn11 = true;
                float length = source.clip.length;

                randommove3(length);
            }
            else if (source.clip.length >= 7f)
            {
                anim.speed = 1.2f;
                MoveOn = true;
                // makrtest.randommove4();
            }
            
            // lipsync.StartMicrophone(audioObject);
            lipsync.AudioGet(audioObject);
          //  StartCoroutine(listenbool(source.clip.length));
            StartCoroutine(movebool(source.clip.length));
           // Destroy(audioObject, source.clip.length);
            //makrtest.MoveOn = true;
        }
    
    private IEnumerator listenbool(float length)
    {
       
        yield return new WaitForSeconds(length + 1f);

    }
    private IEnumerator movebool(float length)
    {
        yield return new WaitForSeconds(length - 2f);
        MoveOn = false;
        answer.text = "";
    }

    public string GetText(string text , int i)
    {
        var newstring = text;
        var index = newstring.Length;
        newstring = newstring.Remove(0, i);
        newstring = newstring.Remove(index - (i+i), i);
        return newstring;
    }

    public void changecolor()
    {
        answer.color = Color.black;
        text1.color = Color.black;
        text2.color = Color.black;
        text3.color = Color.black;
    }




}
