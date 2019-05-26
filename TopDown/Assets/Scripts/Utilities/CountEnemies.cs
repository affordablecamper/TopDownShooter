using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CountEnemies : MonoBehaviour
{
    public GameObject[] enemies;
    public int count;
    public Text countText;
    public GameObject winText;
    public GameObject begText;
    public Transform player;
    public bool enemiesAllDead;
    // Start is called before the first frame update
    void Start()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }

   
  


    void Update()
    {

        if (enemiesAllDead)
        {
            winText.SetActive(true);
            //begText.SetActive(false);

        }
        else {
            winText.SetActive(false);
            //begText.SetActive(true);
        }
            

        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        count = enemies.Length;
        countText.text = count.ToString();

        if (enemies.Length <= 0)
            enemiesAllDead = true;

        else
            enemiesAllDead = false;
            
    }
}
