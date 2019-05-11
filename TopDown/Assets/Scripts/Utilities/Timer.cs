using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Timer : MonoBehaviour
{
    public Text timer;
    public float time;
    public CountEnemies enem;
    public bool pauseTime;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
            time = 0f;

            enem = GameObject.FindGameObjectWithTag("gm destroy").GetComponent<CountEnemies>();


        if (enem.enemies.Length <= 0)
        {

            pauseTime = true;
        }
        else pauseTime = false;
            
        if (!pauseTime) {

            time += Time.deltaTime;
            string minutes = Mathf.Floor((time % 3600) / 60).ToString("00");
            string seconds = (time % 60).ToString("00");

            timer.text = minutes + ":" + seconds;
        }
        
    }
}
