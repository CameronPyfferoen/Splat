using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {
    //text variable
    public Text counter;
    //does this really need explanation?
    public float playTime;
    private float diffTime, CPtime, minutes, seconds;

    private bool reset;
	// Use this for initialization
	void Start () {
        //allows counter to be used in a component as text
		counter = GetComponent<Text>() as Text;
        CPtime = 0;
        diffTime = 0;
        reset = false;
	}
    /*
    public void StoreTime(float t)
    {
        CPtime = t;
        //Debug.Log("CPtime: " + CPminute.ToString("00") + ":" + CPsecond.ToString("00"));
    }

    public void SetTime()
    {
        reset = true;
        diffTime = Time.timeSinceLevelLoad - CPtime;
        /*
        diffSec = (Time.timeSinceLevelLoad % 60f) - CPsecond;
        diffMin = (Time.timeSinceLevelLoad / 60f) - CPminute;
        Debug.Log("Time since load: " + (Time.timeSinceLevelLoad / 60f).ToString("00") + ":" + (Time.timeSinceLevelLoad % 60f).ToString("00"));
        Debug.Log("Time difference: " + diffMin.ToString("00") + ":" + diffSec.ToString("00"));
        
    }
    */
	
	// Update is called once per frame
	void Update () {
        //sets and updates the timer text
        /*
        if(!reset)
        {
            playTime = Time.timeSinceLevelLoad;
            //Debug.Log("time: " + playTime);
            minutes = (int)(playTime / 60f);
            seconds = (int)(playTime % 60f);
        }
        else if(reset)
        {
            playTime = Time.timeSinceLevelLoad - diffTime;
            minutes = (int)(playTime / 60f);
            seconds = (int)(playTime % 60f);
            /*
            minutes = ((Time.timeSinceLevelLoad / 60f) - diffMin);
            seconds = ((Time.timeSinceLevelLoad % 60f) - diffSec);
            
        }*/
        playTime = Time.timeSinceLevelLoad;
        minutes = (int)(playTime / 60f);
        seconds = (int)(playTime % 60f);
        counter.text = minutes.ToString("00") + ":" + seconds.ToString("00");
	}
}
