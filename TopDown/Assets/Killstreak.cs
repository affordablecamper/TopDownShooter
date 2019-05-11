using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Killstreak : MonoBehaviour
{
    public float killstreakTime;
    public float killstreakTimeStart;
    public Text streakMultiplier;
    public GameObject streakText;
    public float kills;
   
    // Start is called before the first frame update
    void Start()
    {
        killstreakTimeStart = killstreakTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (kills >= 2)
        {
            streakText.SetActive(true);
            streakMultiplier.text = "x " + kills.ToString();
            killstreakTime -= Time.deltaTime;
        }
        else {
            
            streakText.SetActive(false);
        }

        
        if (killstreakTime <= 0) {

            kills = 0;
            streakText.SetActive(false);
        }
    }

    public void addKill() {
        killstreakTime = killstreakTimeStart;
        kills++;
        
    }

}
