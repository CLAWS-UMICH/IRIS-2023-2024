using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreadCrumbController : MonoBehaviour
{
    private int index;
    public void UpdateInfo(int i)
    {
        index = i;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "MainCamera" || other.gameObject.tag == "crumb")
        {
            EventBus.Publish(new BreadCrumbCollisionEvent(index));
        }
    }

}
