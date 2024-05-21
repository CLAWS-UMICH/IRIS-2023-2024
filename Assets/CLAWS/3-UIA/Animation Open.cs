using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AnimationOpen: MonoBehaviour
{
    public GameObject top;
    public GameObject middle;
    public GameObject bottom;

    private void OnEnable()
    {
        top.SetActive(true);
        middle.SetActive(false);
        bottom.SetActive(false);

        StartCoroutine(Animate());
    }

    IEnumerator Animate()
    {
        yield return new WaitForSeconds(.5f);

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
