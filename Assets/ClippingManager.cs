using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Utilities;

public class ClippingManager : MonoBehaviour
{
    [SerializeField] private ClippingBox clipping;
    [SerializeField] private List<GameObject> objectsToClip;

    void Start()
    {
        SetRenderers();
    }

    private void SetRenderers()
    {
        foreach (GameObject g in objectsToClip)
        {
            Renderer[] renderers = g.GetComponentsInChildren<Renderer>();
            clipping.ClearRenderers();

            foreach (Renderer r in renderers)
            {
                clipping.AddRenderer(r);
            }
        }
    }


}
