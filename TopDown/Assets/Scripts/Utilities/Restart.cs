using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    public PlayerHealth health;
    public CountEnemies _enemies;
    public bool isTesting;
    private void Awake()
    {
        
    }
    void Update()
    {

        if (health.health <= 0 && !isTesting)
        {
            if (Input.GetKeyDown(KeyCode.R))
                SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        }
        else if (isTesting) {
            if (Input.GetKeyDown(KeyCode.R))
                SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        }
        


        if (_enemies.enemies.Length <= 0)
            if (Input.GetKeyDown(KeyCode.R))
                SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);

    }
}
