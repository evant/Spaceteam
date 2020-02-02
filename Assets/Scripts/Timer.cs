using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour {

    public TextMeshProUGUI timerText;
    private float startTime = 0.0f;

    // Start is called before the first frame update
    void Start() {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update() {
        float t = Time.time - startTime;
        
        string minutes = ((int)t / 60).ToString("00");
        string seconds = (t % 60).ToString("00");
        string milliseconds = ((int)(t * 100f) % 100).ToString("00");

        timerText.text = minutes + ":" + seconds + ":" + milliseconds;
    }
}
