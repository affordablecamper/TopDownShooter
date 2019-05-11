using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public int totalScore;
    public Text scoreText;
    
    public float desiredNumber;
    public float initialNumber;
    public float currentNumber;
    public float animationTime;
    public Killstreak streak;   
    public GameObject renderCanvas;
    public Transform pos;
    public float multipliedScore;
    public Vector3 posAddText;
    // Update is called once per frame
    void Update()
    {
        if (currentNumber != desiredNumber)
        {
            if (initialNumber < desiredNumber)
            {

                currentNumber += (animationTime * Time.deltaTime) * (desiredNumber - initialNumber);
                if (currentNumber >= desiredNumber)
                    currentNumber = desiredNumber;

            }
            else
            {
                currentNumber -= (animationTime * Time.deltaTime) * (initialNumber - desiredNumber);
                if (currentNumber <= desiredNumber)
                    currentNumber = desiredNumber;

            }
            scoreText.text = currentNumber.ToString("0");
        }
    }


    public void SetScore(float value) {

        initialNumber = currentNumber;
        desiredNumber = value;
    }

    public void AddPoints(float pointsAdd)
    {
        if (streak.kills < 2)
        {
            desiredNumber += pointsAdd;
            multipliedScore = pointsAdd;
        }
        else {
            desiredNumber += pointsAdd * streak.kills;
            multipliedScore = pointsAdd * streak.kills;
        }


        initialNumber = currentNumber;
        

    }
}
