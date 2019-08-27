using UnityEngine;
using UnityEngine.UI;
using Wit.BaiduAip.Speech;

public class BaiduSpeech : MonoBehaviour
{
    public string APIKey = "EKuUoSzPXg1yCF7ljlDi0vyk";
    public string SecretKey = "dyAWP1eyaLiIv2P0SbHSkS7AT68ydmMA";
    public Button SynthesisButton;
    public InputField Input;
    //  public Text DescriptionText;
    public Test makrtest;
    private Tts _asr;
    public AudioSource _audioSource;
    public bool _startPlaying;

    public TestBaidu _BaiduSpeech;
    public RobotDance robotdance;
    public Text test11;

    void Start()
    {
        _asr = new Tts(APIKey, SecretKey);
        StartCoroutine(_asr.GetAccessToken());

       // _audioSource = gameObject.AddComponent<AudioSource>();

    //    DescriptionText.text = "";

        SynthesisButton.onClick.AddListener(OnClickSynthesisButton);
    }

    public void SysncBaiduAudio(string text)
    {
        Debug.Log("audio sync");
        StartCoroutine(_asr.Synthesis(text, s =>
        {
            test11.text = s.err_msg;
            if (s.Success)
            {
                //      DescriptionText.text = "合成成功，正在播放";
                _audioSource.clip = s.clip;
                _audioSource.Play();

                _startPlaying = true;

                makrtest.PlayClipTu();
            }
            else
            {
                //   DescriptionText.text = s.err_msg;
                Debug.Log("合成失败");
            }
        }));
    }

    public void SysncBaiduAudioWave(string text)
    {
        StartCoroutine(_asr.Synthesis(text, s =>
        {
            if (s.Success)
            {
                //      DescriptionText.text = "合成成功，正在播放";
                _audioSource.clip = s.clip;
                _audioSource.Play();

                _startPlaying = true;

           
            }
            else
            {
                //   DescriptionText.text = s.err_msg;
                Debug.Log("合成失败");
            }
        }));
    }

    private void OnClickSynthesisButton()
    {
        SynthesisButton.gameObject.SetActive(false);
    //    DescriptionText.text = "合成中...";

        StartCoroutine(_asr.Synthesis(Input.text, s =>
        {
            if (s.Success)
            {
          //      DescriptionText.text = "合成成功，正在播放";
                _audioSource.clip = s.clip;
                _audioSource.Play();
             
                _startPlaying = true;
            }
            else
            {
             //   DescriptionText.text = s.err_msg;
                SynthesisButton.gameObject.SetActive(true);
            }
        }));
    }

    void Update()
    {
        Debug.Log(robotdance.danceon);
        if (_startPlaying)
        {
            if (!_audioSource.isPlaying)
            {
                _startPlaying = false;
                if (robotdance.danceon == false)
                {
                   
                    _BaiduSpeech._isListen = true;
             
                }
               // DescriptionText.text = "播放完毕，可以修改文本继续测试";
                SynthesisButton.gameObject.SetActive(true);
            }
        }
    }
}