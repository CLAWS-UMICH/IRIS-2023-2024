using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    Coroutine lunaCoroutine;
    [SerializeField] private float timeDelay = 1f;
    [SerializeField] GameObject playerCam;
    public float offset = 0f;

    void Start()
    {
        EnterLunaMode();
    }

    IEnumerator LunaMove()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeDelay);
            gameObject.transform.rotation = Quaternion.Euler(0, playerCam.transform.rotation.eulerAngles.y + offset, 0);
        }
    }
    [ContextMenu("Luna Follow")]
    public void EnterLunaMode()
    {
        lunaCoroutine = StartCoroutine(LunaMove());
    }
    [ContextMenu("Luna Fix")]
    public void StopLunaMode()
    {
        StopCoroutine(lunaCoroutine);
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = playerCam.transform.position;
    }
}
