using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadNewLevel : MonoBehaviour
{

    public Animator anim;

    private void OnTriggerEnter(Collider other)
    {
        anim.SetTrigger("FadeOut");
    }

    private void Update()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("FadeOut") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f) {

            OnFadeComplete();
        }
    }


    public void OnFadeComplete() {

        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
