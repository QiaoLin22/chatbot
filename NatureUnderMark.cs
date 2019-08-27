using UnityEngine;
using System.Collections;
using IBM.Watson.DeveloperCloud.Services.NaturalLanguageUnderstanding.v1;
using IBM.Watson.DeveloperCloud.Logging;
using IBM.Watson.DeveloperCloud.Utilities;
using FullSerializer;
using System;
using System.IO;
using System.Collections.Generic;
using IBM.Watson.DeveloperCloud.Connection;
using UnityEngine.UI;

public class NatureUnderMark : MonoBehaviour {
    private NaturalLanguageUnderstanding _understand;

    private bool _getModelsTested = false;
    private bool _analyzeTested = false;
    public Text _questiontext;
    public Test marktest;
    // Use this for initialization
    void Start () {
        LogSystem.InstallDefaultReactors();
        Credentials credentials = new Credentials("dedf1de0-34f0-4de5-a8c3-e0006f89d577", "2G364oc4pTYU", "https://gateway.watsonplatform.net/natural-language-understanding/api");
        _understand = new NaturalLanguageUnderstanding(credentials);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void StartUnderstand()
    {
        Parameters parameters = new Parameters()
        {
            text = _questiontext.text,
           // text = "what is the weather in san jose",
            return_analyzed_text = true,
            language = "en",
            features = new Features()
            {
                entities = new EntitiesOptions()
                {
                    limit = 50,
                    sentiment = false,
                    emotion = false,
                },
                keywords = new KeywordsOptions()
                {
                    limit = 50,
                    sentiment = false,
                    emotion = false
                }
            }
        };

        if (!_understand.Analyze(OnAnalyze, OnFail, parameters))
            Log.Debug("ExampleNaturalLanguageUnderstanding.Analyze()", "Failed to get models.");
    }

    private void OnGetModels(ListModelsResults resp, Dictionary<string, object> customData)
    {
        Log.Debug("ExampleNaturalLanguageUnderstanding.OnGetModels()", "ListModelsResult: {0}", customData["json"].ToString());
        _getModelsTested = true;
    }

    private void OnAnalyze(AnalysisResults resp, Dictionary<string, object> customData)
    {
        Log.Debug("ExampleNaturalLanguageUnderstanding.OnAnalyze()", "AnalysisResults: {0}", customData["json"].ToString());

        string[] splitweather1 = customData["json"].ToString().Split(new string[] { "entities" }, StringSplitOptions.None);
        string[] splitweather2 = splitweather1[1].Split(new string[] { "text", "relevance" }, StringSplitOptions.None);
        Debug.Log("aaaaaaaaaaaaaCITY"+splitweather2[1]);
        string city = GetText(splitweather2[1], 3);
        Debug.Log("aaaaaaaaaaaaa" + city);
        StartCoroutine(marktest.GetWeather(city));
        _analyzeTested = true;
        
    }

    private void OnFail(RESTConnector.Error error, Dictionary<string, object> customData)
    {
        Log.Error("ExampleNaturalLanguageUnderstanding.OnFail()", "Error received: {0}", error.ToString());
    }

    string GetText(string text, int i)
    {
        var newstring = text;
        var index = newstring.Length;
        newstring = newstring.Remove(0, i);
        newstring = newstring.Remove(index - (i + i), i);
        return newstring;
    }
}
