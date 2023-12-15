using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Utilities;

public class ClippingManager : MonoBehaviour
{
    [SerializeField] private ClippingBox clipping;
    [SerializeField] public List<GameObject> objectsToClip;

    void Start()
    {
        SetRenderers();
    }

    public void SetRenderers()
    {
        clipping.ClearRenderers();

        foreach (GameObject g in objectsToClip)
        {
            Renderer[] renderers = g.GetComponentsInChildren<Renderer>();

            foreach (Renderer r in renderers)
            {
                clipping.AddRenderer(r);
            }
        }
    }


}
