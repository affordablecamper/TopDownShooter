using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CountEnemies : MonoBehaviour
{
    public GameObject[] enemies;
    public int count;
    public Text countText;
    // Start is called before the first frame update
    void Start()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }

    // Update is called once per frame
    void Update()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        count = enemies.Length;
        countText.text = count.ToString();


    }
}
