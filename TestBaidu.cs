using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using LitJson;

public class TestBaidu : MonoBehaviour
{
    public Animator anim;
    public Text WakeUpText;
    public GameObject songbuffer;
    public bool _buffer = false;
    public float counter = 0;
    public AudioSource audio1;
    public SpeechToTextMark stt;
    public Test marktest;
    public bool _isListen = false;
    public Text myText;
    public Text secondtext;
    public Text secondtext1;
    public Text test2;

    public Text _response;
    public SpeechToTextMark sst;

    public StringBuilder stringBuilder = new StringBuilder();

    public AndroidJavaClass ajc = null;
    

    public AndroidJavaObject ajo = null;
    public AndroidJavaClass wakejc = null;
    public AndroidJavaObject wakejo = null;

    public List<string> res = new List<string>();

    public List<string> nlures = new List<string>();

    public List<string> domains = new List<string>();

    public Text status;

  

    public float timer = 0;


    public void Update()
    {
        //audio
        if (_buffer)
        {
            counter += Time.deltaTime;
            if (audio1.isPlaying)
            {
        
                songbuffer.gameObject.SetActive(false);
                Debug.Log("song now play");
                _buffer = false;
                counter = 0;
            }
            if (counter >= 10f)
            {
                songbuffer.transform.GetChild(0).GetComponent<Text>().text = "【歌曲未找到】";

               
            }
            if (counter >= 11.5f)
            {
                songbuffer.transform.GetChild(0).GetComponent<Text>().text = "【歌曲未找到】";
                songbuffer.gameObject.SetActive(false);
                stt.videoon = false;
                _buffer = false;
                counter = 0;
            }

        }

        if (_isListen)
        {
            StartVoice("");
            _isListen = false;
          
            
        }
        if (myText.text == "start")
        {
            timer += Time.deltaTime;
            status.text = timer.ToString();
        }
        else
        {
            timer = 0;
            status.text = timer.ToString();
        }

        if (timer > 61f)
        {
            timer = 0;
            Cancel();
            StartVoice("");
            
            _isListen = false;
        }


    }

    public void StartListen_bool()
    {
        if (_isListen)
        {
           // _isListen = true;
            anim.CrossFade("Stand", 0.1f);
            anim.SetBool("iswave", false);
            anim.SetBool("isstand", true);
        }      
        else{
           // _isListen = false;
            anim.CrossFade("Wave", 0.1f);
            anim.SetBool("iswave", true);
            anim.SetBool("isstand", false);
        }
    }

    public void SetText(string str)
    {
        this.myText.text = "";
       
        //test
       // myText.text += str;
        //
        if (str == "1000")
        {
           
            print("hahahaha" + res[res.Count - 1]);
            test2.text =res[res.Count - 1].ToString();
         //   secondtext.text = addon;
           /* if (nlures != null && nlures.Count!=0)
            {
                secondtext.text = nlures[0];
            }*/
            Cancel();
            sst.StartConversation1();
     
            res.Clear();
       //     nlures.Clear();
         
        }
        this.stringBuilder.Length = 0;
        this.stringBuilder.Append(str);
        this.myText.text = stringBuilder.ToString();
        
    }

    public void SetText1(string str)
    {

        //  string ss = "{'results_recoginization':'xuhaiti','best_result':'34','father':['xuguozhu','55','run']}";
      


//        JsonData hh = JsonMapper.ToObject(str);

//        this.res.Add(hh["best_result"].ToString());
        //   this.res.Add(hh["merged_res"].ToString());
//        this.stringBuilder.Append(res[0].ToString());
      //  this.myText.text = stringBuilder.ToString();
    //nlu
       // myText.text += str;
        //nlu
        // myText.text += str;
       // myText.text += res[0].ToString();

    }
    public void SetText2(string str)
    {
       // int chooseline = 0;
        JsonData hh = JsonMapper.ToObject(str);
        string domain = "";
 //       this.nlures.Add(hh["merged_res"]["semantic_form"]["results"][0]["domain"].ToString());
        string answer = hh["merged_res"]["semantic_form"]["raw_text"].ToString();
        secondtext.text = answer;
        if (hh["merged_res"]["semantic_form"]["results"].Count != 0)
        {
             domain = hh["merged_res"]["semantic_form"]["results"][0]["domain"].ToString();
       /*     for(int i = 0; i< hh["merged_res"]["semantic_form"]["results"].Count; i++)
            {
                domains.Add(hh["merged_res"]["semantic_form"]["results"][i]["domain"].ToString());
            }
            for (int i = 0; i < domains.Count; i++)
            {
                if (domains[i] == "weather")
                {
                    domain = "weather";
                    break;
                }
                else if (domains[i] == "music")
                {
                    domain = "music";
                    chooseline = i;
                    break;
                }
            }*/
        }



        secondtext1.text = domain;
        if (domain == "weather")
        {
            if (hh["merged_res"]["semantic_form"]["results"][0]["object"].Count.ToString() == "0")
            {
                answer = "上海天气怎么样";
            }

            //  secondtext1.text = hh["merged_res"]["semantic_form"]["results"][0]["object"]["region"].ToString();
            secondtext1.text += "COUNT" + hh["merged_res"]["semantic_form"]["results"][0]["object"].Count.ToString();
            test2.text = answer;

            Cancel();
            sst.StartConversation1();
            res.Clear();

        }
        else if (domain!="" && answer.Contains("播放") && !_buffer)
        {
            if (hh["merged_res"]["semantic_form"]["results"][0]["object"].Count.ToString() != "0")
            {
                string songname = hh["merged_res"]["semantic_form"]["results"][0]["object"]["name"].ToString();
                secondtext1.text = " bo:" + songname;
                _buffer = true;
                stt.videoon = true;
                songbuffer.gameObject.SetActive(true);
                songbuffer.transform.GetChild(0).GetComponent<Text>().text = "【歌曲查询中........】";
                FinsSong(songname);
                test2.text = answer;
            }
            else
            {

                answer = "没有歌曲信息";
                test2.text = answer;

                Cancel();
                marktest.speaktuling(answer);
            
            }


        }
        else
        {
            test2.text = answer;

            Cancel();
            sst.StartConversation1();
            res.Clear();
        }
        // print("hahahaha" + res[res.Count - 1]);


    }

    public void FinsSong(string name)
    {

        secondtext1.text = " bo fang 2:"+name;
        StartCoroutine(getsong(name));
    }

    IEnumerator getsong(string songname)
    {
        string uuu = WWW.EscapeURL(songname);
        string uu = "169.45.105.14:3000/?name=" + uuu;
        Debug.Log(uu);
        WWW www = new WWW("http://" + uu);
     
        yield return www;


        string url = www.text;
        Debug.Log(url);
        char[] deli = new char[] { '<', '>' };
        string[] parts = url.Split(deli, System.StringSplitOptions.RemoveEmptyEntries);
        string finalurl = parts[1] + parts[3] + parts[5];
 
        StartCoroutine(playsong(finalurl));
    }
    IEnumerator playsong(string url)
    {
        Debug.Log("http://" + url);
        WWW www = new WWW("http://" + url);

        yield return www;

        audio1.clip = www.GetAudioClip(false,false,AudioType.MPEG);
        audio1.Play();
    }

/*    public void ClearWWW()
    {
        if (m_www != null)
        {
            m_www.Dispose();
            m_www = null;
           
        }
    }*/


    // Use this for initialization
    void Start()
    {
        audio1.clip = null;
         Reponsejason("1");
        ajc = new AndroidJavaClass("com.nano.baiduvoice.BaiduVoice");
        ajc.CallStatic("start");
        ajo = ajc.GetStatic<AndroidJavaObject>("instance");

        ajo.Call("Init");

    }

    public void StartWakeUp(string str)
    {
        str = "{\"accept-audio-volume\":false,\"wp-words-file\":\"assets://WakeUp.bin\"}";
        wakejo.Call("start", str);
    }
    /// <summary>
    /// 开始语音识别方法
    /// </summary>
    /// <param name="str">json参数</param>
    public void StartVoice(string str)
    {


        //pid 为识别语言种类
        str = "{\"accept-audio-data\":true,\"vad.endpoint-timeout\":0,\"vad\":dnn,\"pid\":15361,\"disable-punctuation\":false,\"accept-audio-volume\":true}";
      //  ajo.Call("start", str);
        anim.CrossFade("Stand", 0.1f);
        anim.SetBool("iswave", true);
        anim.SetBool("isstand", false);
        Debug.Log("wozaikongzhi");
        _isListen = true;
    }

    public void wakeText(string str)
    {
        this.WakeUpText.text = str;

        

    }

    /// <summary>
    /// 停止识别方法
    /// </summary>
    public void StopVoice()
    {
        ajo.Call("stop");
        anim.Play("Stand");
        _isListen = false;
    }

    public void Cancel()
    {
        
        ajo.Call("cancel");
    }

    public class Response
    {
        public string best_result;
    }
    private void Reponsejason(string json)
    {
        string ss = "{'results_recoginization':'xuhaiti','best_result':'34','father':['xuguozhu','55','run']}";
        JsonData hh = JsonMapper.ToObject(ss);
        

    }

    // Update is called once per frame

}