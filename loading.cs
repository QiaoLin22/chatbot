using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class loading : MonoBehaviour
{

    /// <summary>
    /// 单件
    /// </summary>
    /// 


    public static loading Instance;

    ///// <summary>
    ///// 下一个要跳转的场景名字
    ///// </summary>
    //public string nextSceneName = "Game";
    /// <summary>
    /// 加载界面
    /// </summary>
    private GameObject loadingPanel;

    /// <summary>
    /// 加载进度条
    /// </summary>
    public Image loadingBar;

    AsyncOperation async;

    private float progress = 0;


    void Awake()
    {

        Instance = this;

      //  loadingBar = GameObject.Find("UIRoot").transform.Find("LoadingBar").gameObject;

        //loadingBar = loadingPanel.transform.FindChild("LoadingBar/ChildSprite").gameObject;

        //loadingBar.transform.GetChild(0).GetComponent<Image>().fillAmount = 1f;
        loadingBar.fillAmount = progress;

    }

    // Use this for initialization
    void Start()
    {
        StartCoroutine(LoadScene());
    }


    private void SetProgress(int progress)
    {
        loadingBar.fillAmount = progress * 0.1f;
    }

    IEnumerator LoadScene(float startPercent = 0)
    {

        int startProgress = (int)(startPercent * 100);

        int displayProgress = startProgress;

        int toProgress = startProgress;

        yield return new WaitForEndOfFrame();


        async = SceneManager.LoadSceneAsync("Mark_half");

        async.allowSceneActivation = false;

        while (async.progress < 0.9f)
        {
            toProgress = startProgress + (int)(async.progress * (1.0f - startProgress));

            while (displayProgress < toProgress)
            {
                ++displayProgress;
                SetProgress(displayProgress);
                yield return null;
            }
            yield return null;
        }

        toProgress = 100;
        while (displayProgress < toProgress)
        {
            ++displayProgress;
            SetProgress(displayProgress);
            yield return null;
        }
        async.allowSceneActivation = true;

        //yield return async;
    }

    // Update is called once per frame
    void Update()
    {

        //if (async != null)
        //{
        //    progress = (int)(async.progress * 100);

        //    loadingBar.GetComponent<UISlider>().value = progress;
        //}
    }
}



