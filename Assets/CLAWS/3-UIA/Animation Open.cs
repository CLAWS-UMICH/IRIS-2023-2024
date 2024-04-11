using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationOpen: MonoBehaviour
{
    public GameObject top;
    public GameObject middle;
    public GameObject bottom;

    //private void onEnable()
    //{
    //    top.SetActive(true);
    //    middle.SetActive(false);
    //    bottom.SetActive(false);
    //    StartCoroutine(Animate());
    //}

    //For testing purposes
    private void Start()
    {
        top.SetActive(true);
        middle.SetActive(false);
        bottom.SetActive(false);
        StartCoroutine(Animate());
    }

    IEnumerator Animate()
    {
        while (gameObject.activeSelf)
        {
            if (bottom.activeInHierarchy)
            {
                bottom.SetActive(false);
                middle.SetActive(true);
            }
            else if(middle.activeInHierarchy){
                middle.SetActive(false);
                top.SetActive(true);
            }
            else
            {
                top.SetActive(false);
                bottom.SetActive(true);
            }
            yield return new WaitForSeconds(.5f);
        }
    }
}
