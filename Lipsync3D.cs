using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CrazyMinnow.SALSA;

public class Lipsync3D : MonoBehaviour {
    public Salsa3D salsa3D; // Reference to the Salsa3D class
    public AudioClip audioClips; // An array of example sound to play

    private int clipIndex = 0; // Track audioClips index

    // These private variables are used to position buttons in the OnGUI method
    private int yPos = 0; // The Y position of a GUI button
    private int yGap = 10; // The vertical spacing between GUI buttons
    private int xWidth = 150; // Button and label width
    private int yHeight = 35; // Button and label height
                              // Use this for initialization
    void Start () {
        if (!salsa3D) // salsa3D is null
        {
            salsa3D = (Salsa3D)FindObjectOfType(typeof(Salsa3D)); // Try to get a local reference to Salsa3D
        }

      //  AudioGettest();

      /*  if (audioClips.Length > 0)
        {
            salsa3D.SetAudioClip(audioClips[clipIndex]);
        }*/
    }

    void OnGUI()
    {
    /*    yPos = 0; // Reset the button Y position

        #region Salsa3D Play, Pause, and Stop controls
        yPos += yGap;
        if (GUI.Button(new Rect(20, yPos, xWidth, yHeight), "Play"))
        {
            salsa3D.Play(); // Salsa3D Play method
        }

        yPos += (yGap + yHeight);
        if (GUI.Button(new Rect(20, yPos, xWidth, yHeight), "Pause"))
        {
            salsa3D.Pause(); // Salsa3D Pause method
        }

        yPos += (yGap + yHeight);
        if (GUI.Button(new Rect(20, yPos, xWidth, yHeight), "Stop"))
        {
            salsa3D.Stop(); // Salsa3D Stop method
        }
        #endregion

        #region Toggle which audio clip is set on Salsa3D
        yPos += (yGap + yHeight);
      /*  if (GUI.Button(new Rect(20, yPos, xWidth, yHeight), "Set audio clip"))
        {
            if (clipIndex < audioClips.Length - 1)
            {
                clipIndex++;
                salsa3D.SetAudioClip(audioClips[clipIndex]);
            }
            else
            {
                clipIndex = 0;
                salsa3D.SetAudioClip(audioClips[clipIndex]);
            }
        }*/
        //#endregion
        #region Display the currently selected audio clip
      //  GUI.Label(new Rect(30 + xWidth, yPos, xWidth, yHeight), "Clip " + audioClips[clipIndex].name);
        #endregion
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void AudioGet(GameObject audiosource)
    {
        print("methoed used1!!!!");
        audioClips = audiosource.GetComponent<AudioSource>().clip;
        //   if (audiosource != null)
        //    Debug.Log("Get final audio!!!!!");
        salsa3D.SetAudioClip(audiosource.GetComponent<AudioSource>().clip);
      //  salsa3D.Play();
        
      //  Debug.Log("audio clip is name" + audiosource.GetComponent<AudioSource>().clip.name);
    }
    public void AudioGettest()
    {
        print("methoed used1111!!!!");
        GameObject Audioobject = GameObject.Find("TestAudio");
        if (Audioobject != null)
        {
            Debug.Log("Get final audio!!!!!");
            print(Audioobject.name);
        }
        salsa3D.SetAudioClip(Audioobject.GetComponent<AudioClip>());
        audioClips = Audioobject.GetComponent<AudioSource>().clip;
        Debug.Log("audio clip is name" + Audioobject.GetComponent<AudioSource>().clip.name);
    }
}
