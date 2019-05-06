using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Timer : MonoBehaviour
{
    public Text timer;
    public float time;
    

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        string minutes = Mathf.Floor((time % 3600) / 60).ToString("00");
        string seconds = (time % 60).ToString("00");

        timer.text = minutes + ":" + seconds;
    }
}
