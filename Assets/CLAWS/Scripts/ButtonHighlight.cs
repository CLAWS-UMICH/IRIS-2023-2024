using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHighlight : MonoBehaviour
{
    GameObject regularQuad;
    GameObject highLightQuad;

    // Start is called before the first frame update
    void Start()
    {
        regularQuad = transform.Find("Button").transform.Find("BackPlate").transform.Find("Quad").gameObject;
        highLightQuad = transform.Find("Button").transform.Find("BackPlate").transform.Find("HighlightQuad").gameObject;
        highLightQuad.SetActive(false);
        regularQuad.SetActive(true);
    }

    public void OnClick()
    {
        EventBus.Publish(new HighlightButton(this.gameObject));
    }

    public void highLight()
    {
        regularQuad.SetActive(false);
        highLightQuad.SetActive(true);
    }

    public void unHighlight()
    {
        highLightQuad.SetActive(false);
        regularQuad.SetActive(true);
    }
}
