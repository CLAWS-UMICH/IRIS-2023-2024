using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCloseButtonPressed()
    {
        IEnumerator _close()
        {
            yield return new WaitForSeconds(0.01f);
            gameObject.SetActive(false);
        }

        StartCoroutine(_close());
    }
}
