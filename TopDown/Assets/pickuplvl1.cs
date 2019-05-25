using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class pickuplvl1 : MonoBehaviour
{
    public GameObject shotgun;
    public TextMeshPro text;
    public GameObject Ttext;
    public GameObject Ttext1;
    // Update is called once per frame
    void Update()
    {
        if (shotgun.activeSelf == true) {
            text.text = "KILL HIM";
            
            Ttext1.SetActive(false);
            StartCoroutine("Wait", 1.5f);
            
        }
    }

    IEnumerator Wait(float delay)
    {


        yield return new WaitForSeconds(delay);
        text.enabled = false;
        Ttext.SetActive(true);
    }

}
