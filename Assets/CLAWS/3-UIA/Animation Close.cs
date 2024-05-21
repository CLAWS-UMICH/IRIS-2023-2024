using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationClose: MonoBehaviour
{
    public GameObject top;
    public GameObject middle;
    public GameObject bottom;

    private void OnEnable()
    {
        top.SetActive(false);
        middle.SetActive(false);
        bottom.SetActive(true);

        StartCoroutine(Animate());
    }

    IEnumerator Animate()
    {
        while (gameObject.activeSelf)
        {
            if (bottom.activeInHierarchy)
            {
                bottom.SetActive(false);
                top.SetActive(true);
            }
            else if(middle.activeInHierarchy){
                middle.SetActive(false);
                bottom.SetActive(true);
            }
            else
            {
                top.SetActive(false);
                middle.SetActive(true);
            }
            yield return new WaitForSeconds(.5f);
        }
    }
}
