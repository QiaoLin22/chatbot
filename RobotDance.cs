using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RobotDance : MonoBehaviour {
    public int timeleft = 7;
    public Text countdown;
    public AudioSource[] counterdownnumber;
    public AudioSource dancesong;
    public GameObject dancemodel;
    public GameObject standmode;
    private SpeechToTextMark sttmark;
//    public Text answertext;
    public bool danceon = false;
    public TestBaidu baidyspeech;
    // Use this for initialization
    void Start () {
        sttmark = GameObject.Find("Main Camera").GetComponent<SpeechToTextMark>();
        danceon = false;
        //     StartCoroutine(Losetime());
        //   StartCoroutine(DanceTime());
    }
	
	// Update is called once per frame
	void Update () {
        if (timeleft <= 3 && timeleft >0)
        {
            countdown.text = timeleft + "";
        }
        if (timeleft <= 0)
        {

            timeleft = 8;
        }

    }
    public IEnumerator Losetime()
    {
        countdown.gameObject.SetActive(true);
       // sttmark.Active = false;
        danceon = true;
      // counterdownnumber[2].Play();
        while (timeleft > 0)
        {
            yield return new WaitForSeconds(1);

            timeleft--;
            if (timeleft == 3)
            {
                counterdownnumber[2].Play();
                danceon = true;
            }
            if (timeleft >= 1 && timeleft < 3)
                counterdownnumber[timeleft - 1].Play();


        }
        if (timeleft == 0)
        {
            dancesong.Play();
            countdown.gameObject.SetActive(false);
            standmode.SetActive(false);
            dancemodel.SetActive(true);

        }


    }

    public IEnumerator DanceTime()
    {

      
        yield return new WaitForSeconds(37);
        Debug.Log("STTTTTTTTTTTTTTTTOOOOOPPPPPPPPPPPP");
          standmode.SetActive(true);
            dancemodel.SetActive(false);
        //  sttmark.Active = true;
        danceon = false;
        baidyspeech._isListen = true;
  

    }
}
