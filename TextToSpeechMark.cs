using UnityEngine;
using System.Collections;
using IBM.Watson.DeveloperCloud.Services.TextToSpeech.v1;
using IBM.Watson.DeveloperCloud.Logging;
using IBM.Watson.DeveloperCloud.Utilities;
using FullSerializer;
using System;
using System.IO;
using System.Collections.Generic;
using IBM.Watson.DeveloperCloud.Connection;
using UnityEngine.UI;
//using FrostweepGames.Plugins.GoogleCloud.SpeechRecognition.Examples;


public class TextToSpeechMark : MonoBehaviour {
    public Lipsync3D lipsync;
    TextToSpeech _textToSpeech;
    public Text texttoread;
    private SpeechToTextMark sttmark;
    public Test makrtest;
    public RobotDance dancerot;
    
 //   public Example googletest;
 
 //   public GameObject FinalAudio;
    // Use this for initialization
    void Start () {
        LogSystem.InstallDefaultReactors();
        Credentials credentials = new Credentials("5d01f97c-da29-4217-b795-155b2f332eb6", "sV3wZJrgtXYF", "https://stream.watsonplatform.net/text-to-speech/api");
        _textToSpeech = new TextToSpeech(credentials);
        sttmark = gameObject.GetComponent<SpeechToTextMark>();
              

        GetVoice();
    //    Synthesize();
    }

    // Update is called once per frame
    void Update()
    {
        GetVoice();
    }
    private void GetVoice()
    {
        if (!_textToSpeech.GetVoice(OnGetVoice, OnFail, VoiceType.en_US_Lisa))
            Log.Debug("ExampleTextToSpeech.GetVoice()", "Failed to get voice!");
    }

    private void OnGetVoice(Voice voice, Dictionary<string, object> customData)
    {
       
        Log.Debug("ExampleTextToSpeech.OnGetVoice()", "Text to Speech - Get voice response: {0}", customData["json"].ToString());
    }


    public void Synthesize(string text)
    {
        if (text.Contains("cool"))
        {
            print("aaaooooooooooowwww++");
        }
      //  sttmark.Active = false;
       // googletest.StopRecordButtonOnClickHandler();
        _textToSpeech.Voice = VoiceType.en_US_Lisa;

        if (!_textToSpeech.ToSpeech(OnSynthesize, OnFail, text, true))
            Log.Debug("ExampleTextToSpeech.ToSpeech()", "Failed to synthesize!");
        
    }

    public void SynthesizeFormodel(string text)
    {

        _textToSpeech.Voice = VoiceType.en_US_Lisa;

        if (!_textToSpeech.ToSpeech(OnSynthesize, OnFail, text, false))
            Log.Debug("ExampleTextToSpeech.ToSpeech()", "Failed to synthesize!");

    }

    private void OnSynthesize(AudioClip clip, Dictionary<string, object> customData)
    {
        PlayClip(clip);
     //   sttmark.Active = false;
     //   googletest.StopRecordButtonOnClickHandler();
    }

    private void PlayClip(AudioClip clip)
    {
        
        if (Application.isPlaying && clip != null)
        {
            GameObject audioObject = new GameObject("FinalAudio");
            AudioSource source = audioObject.AddComponent<AudioSource>();

            source.spatialBlend = 0.0f;
            source.loop = false;
            source.clip = clip;
            source.Play();
            //mark
            //   float speed =29f*6f / clip.length /64f ;
            //   makrtest.anim.speed = speed;
            //   makrtest.randommove();

            //mark
            Debug.Log("Lenghttttttttttttttt+++++++++++" + clip.length);
            if (clip.length < 3f && clip.length > 2f)
            {
                
                makrtest.randommove();
            }
            else if(clip.length>=3f && clip.length < 5f)
            {
               float speed = 113f /(clip.length * 24f ) ;
                makrtest.anim.speed = speed;
            //    makrtest.MoveOn1 = true;
                makrtest.randommove2();
            }
            else if (clip.length >= 5f && clip.length < 7f)
            {
                //   makrtest.MoveOn11 = true;
                float length = clip.length;
           
                makrtest.randommove3( length);
            }
            else if (clip.length >= 7f )
            {
                makrtest.anim.speed = 1.2f;
               // makrtest.MoveOn = true;
               // makrtest.randommove4();
            }
            // lipsync.StartMicrophone(audioObject);
            lipsync.AudioGet(audioObject);
            StartCoroutine(listenbool(clip.length));
            StartCoroutine(movebool(clip.length));
            Destroy(audioObject, clip.length);
            //makrtest.MoveOn = true;
        }
    }
    public void stoptalking()
    {
        GameObject audioObject = new GameObject("FinalAudio");
        AudioSource source = audioObject.AddComponent<AudioSource>();
        Destroy(audioObject);
    }
    private IEnumerator listenbool(float length)
    {
        ConversationMark convermark = gameObject.GetComponent<ConversationMark>();
        yield return new WaitForSeconds(length+1f);
        if (!dancerot.danceon)
        {
         //   sttmark.Active = true;
        }

     //   googletest.StartRecordButtonOnClickHandler();

    //    makrtest.MoveOn1 = false;
    //   makrtest.MoveOn11 = false;
    //   makrtest.MoveOn2 = false;
     //   texttoread.text = "";
    }
    private IEnumerator movebool(float length)
    {
        yield return new WaitForSeconds(length-2f);
        makrtest.MoveOn = false;
        texttoread.text = "";
    }

    private void OnFail(RESTConnector.Error error, Dictionary<string, object> customData)
    {
        Log.Error("ExampleTextToSpeech.OnFail()", "Error received: {0}", error.ToString());
    }




}
