using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using LitJson;
using UnityEngine.Networking;

using System.Text;
using System;
using UnityEngine.UI;
using System.IO;

public class BaiduApi : MonoBehaviour
{

    private string token;                           //access_token

    private string text;

    private string grant_Type = "client_credentials";
    private string client_ID = "a8lZASnNTujBGMaCg8vgu6cr";                   //百度appkey
    private string client_Secret = "98roYTeZ9XWv2MXhGC7zGeCHsvHLsppL";                   //百度Secret Key

    private string baiduAPI = "aip.baidubce.com/rpc/2.0/nlp/v1/lexer";
    private string getTokenAPIPath = "aip.baidubce.com/oauth/2.0/token";

    public GameObject Map;
    Renderer Map_rend;

    public SpeechToTextMark sstm;

    public Test marktest;

    private void Start()
    {


        //StartCoroutine(GetText());
        Map_rend = Map.GetComponent<Renderer>();
        //  StartCoroutine(GetToken(getTokenAPIPath));
        //  StartCoroutine(GetMap());
        //   CheckLocation();


    }
    private void Update()
    {
        
    }

    IEnumerator GetText()
    {
        string url = "touch.qunar.com/h5/train/trainList?startStation=%E5%8C%97%E4%BA%AC&endStation=%E4%B8%8A%E6%B5%B7&searchType=stasta&date=2018-01-25&sort=3&filterTrainType=1&filterTrainType=2&filterTrainType=3&filterTrainType=4&filterTrainType=5&filterTrainType=6&filterTrainType=7&filterDeptTimeRange=1&filterDeptTimeRange=2&filterDeptTimeRange=3&filterDeptTimeRange=4";
        using (WWW www = new WWW("http://"+url))
        {
            yield return www;
            //Renderer renderer = GetComponent<Renderer>();
            Map_rend.material.mainTexture = www.texture;
        }
    }


public IEnumerator GetToken(string inputx)
    {
        WWWForm getTForm = new WWWForm();
        getTForm.AddField("grant_type", grant_Type);
        getTForm.AddField("client_id", client_ID);
        getTForm.AddField("client_secret", client_Secret);

        WWW getTW = new WWW("https://"+getTokenAPIPath, getTForm);
        yield return getTW;
        if (getTW.isDone)
        {
            if (getTW.error == null)
            {
                token = JsonMapper.ToObject(getTW.text)["access_token"].ToString();
                // StartCoroutine(Answergot());
                Answergot(inputx);
            }
            else
                Debug.LogError(getTW.error);
        }
        print(token + "aaaaaaaaaaaaaa");
        
    }

    private string json = @"{
		'text':'北京在哪里'
	}";
    public class JsonInput
    {
        public string text;
    }
    public void Answergot(string inputx)
    {
       // text = "北京天气";
       // string text1 = "{\"text\":\"" + text + "\"}";

        string url_param = "?charset=UTF-8&access_token=" + token;
        string url1 = baiduAPI + url_param;
        //   string json = @"{
        //'text':\"+inputx +" \"}";
        //json = json.Replace("'", "\"");
        JsonInput myobject = new JsonInput();
        myobject.text = inputx;
        string json1 = JsonUtility.ToJson(myobject);
        byte[] postData = System.Text.Encoding.UTF8.GetBytes(json1);
        WWW www = new WWW("https://"+url1, postData);
        StartCoroutine(WaitForRequest(www));

    
        
    }
    IEnumerator WaitForRequest(WWW www)
    {
        yield return www;
        if (www.error == null)
        {
            //Print server response
            Debug.Log(www.text);
        }
        else
        {
            //Something goes wrong, print the error response
            Debug.Log(www.error);
        }
        Processjson(www.data);
    }
    public class Bodycontent
    {
        public string text;
        public ArrayList item;
    }


    private void Processjson(string jsonString)
    {
        JsonData jsonvale = JsonMapper.ToObject(jsonString);
        Bodycontent parsejson;
        parsejson = new Bodycontent();
        parsejson.text = jsonvale["text"].ToString();

        parsejson.item = new ArrayList();
        string[] locations = new string[2];
        int j = 0;
        for (int i = 0; i < jsonvale["items"].Count; i++)
        {
            parsejson.item.Add(jsonvale["items"][i]["item"].ToString());
            if(jsonvale["items"][i]["ne"].ToString() == "LOC")
            {
                locations[j] = jsonvale["items"][i]["item"].ToString();
                j++;
            }
        }
        Debug.Log("answer check" + locations[0]+locations[1]);

        if (locations[0] != null)
        {
            CheckLocation(locations[0]);
        }
        else
        {
            Debug.Log("no cirty chosee!!!!");
            marktest.speaktuling("对不起，没有找到您要的信息");
            sstm.Active = true;
        }
    }


    public IEnumerator GetMap(string loc)
    {
        string url = "restapi.amap.com/v3/staticmap?location=" + loc + "&zoom=10&size=750*300&markers=mid,,A:" + loc + "&key=8da47259ed204311cfc6bd3d9ebff6e0";
        WWW www = new WWW("http://"+url);

        yield return www;
        if (www.isDone)
        {
            Map.gameObject.SetActive(true);
        //    sstm.Active = true;
        }

        Map_rend.material.mainTexture = www.texture;
        Debug.Log(www.text);



    }
    public class LocationList
    {
        public ArrayList location;
    }
    //location
    public void CheckLocation(string loc)
    {
        string url = "restapi.amap.com/v3/geocode/geo?address=" + loc + "&key=8da47259ed204311cfc6bd3d9ebff6e0";
        WWW www = new WWW("http://"+url);
        StartCoroutine(WaitForRequestLoc(www));


    }
    IEnumerator WaitForRequestLoc(WWW www)
    {
        yield return www;
        if (www.error == null)
        {
            //Print server response
            Debug.Log(www.text);
        }
        else
        {
            //Something goes wrong, print the error response
            Debug.Log(www.error);
        }
        LocationJason(www.data);
    }

    private void LocationJason(string jsonString)
    {
        JsonData jsonvale = JsonMapper.ToObject(jsonString);
        LocationList parsejson;
        parsejson = new LocationList();

        parsejson.location = new ArrayList();


        for (int i = 0; i < jsonvale["geocodes"].Count; i++)
        {
            parsejson.location.Add(jsonvale["geocodes"][i]["location"].ToString());

        }
        Debug.Log("answer check" + parsejson.location[0]);


        StartCoroutine(GetMap(parsejson.location[0].ToString()));

    }
}