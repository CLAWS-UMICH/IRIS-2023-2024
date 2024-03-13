using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCameraController : MonoBehaviour
{
    [SerializeField] GameObject following;
    private Vector3 targetPosition;

    void Start()
    {
        EventBus.Subscribe<GeosampleModeStartedEvent>(e => SetZoom(4));
        EventBus.Subscribe<GeosampleModeEndedEvent>(e => SetZoom(20));
    }
    void Update()
    {
        targetPosition = new Vector3(following.transform.position.x, following.transform.position.y + 10f, following.transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.time);
    }

    void SetZoom(int size)
    {
        GetComponent<Camera>().orthographicSize = size;
    }
}
