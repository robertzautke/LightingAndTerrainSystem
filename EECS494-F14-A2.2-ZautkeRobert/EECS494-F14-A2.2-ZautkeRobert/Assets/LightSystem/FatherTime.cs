using UnityEngine;
using System.Collections;

public class FatherTime : MonoBehaviour {


    // Current Gametime in seconds
    // 0 seconds = midnight
    [Range(0, 86400)]
    public float GameTime = 0f;

    // Multiplier for the time system
    // a multiplier of 1 means that the
    // lighting system is progressing in real
    // time.
    [Range(0, 8000)]
    public float multiplier = 1;

    // Number of seconds in a day
    [HideInInspector]
    public float secondsInDay = 86400f;


    private ClockText ct;

    void Start(){
        GameObject temp = GameObject.Find("ClockText");

        if(temp != null)
            ct = temp.GetComponent<ClockText>();

    }

    void Update() {
        GameTime += multiplier * Time.deltaTime;

        if (GameTime > secondsInDay) {
            GameTime = 0;
        }
               
        // If ClockText does not exist in game hierarchy,
        // do not display Gametime to the GUI
        if(ct != null)
            SetClockText();
    }


    // Display GameTime to the GUI in a "Viewer-Friendly" format
    void SetClockText() { 
        int hour = (int)GameTime / 3600;
        int displayhour = hour;

        string AMorPM = "am";

        if (hour == 0)
            displayhour = 12;

        if (hour >= 12) {
            displayhour = hour - 12;

            if (displayhour == 0)
                displayhour = 12;

            AMorPM = "pm";
        }

        int minute = (int)(GameTime - (hour * 3600)) / 60;

        int second = (int)GameTime % 60;

        ct.GetComponent<GUIText>().text = displayhour + ":" + minute.ToString("00") + ":" + second.ToString("00") + " " + AMorPM;
    }
}
