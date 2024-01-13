using System.Collections.Generic;
using UnityEngine;

using System.Collections;

public class AstronautInstance : MonoBehaviour
{
    public static AstronautInstance instance { get; private set; }
    [SerializeField] private Astronaut _user;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
            
    }

    // STATIC INTERFACE
    public static Astronaut User
    {
        get
        {
            return instance._user;
        }
        set
        {
            instance._user = value;
        }
    }

    void Start()
    {
        _user.currently_navigating = false;
        _user.inDanger = false;
        StartCoroutine(UpdateLocation());
    }

    IEnumerator UpdateLocation()
    {
        AstronautInstance.User.location = GPSUtils.AppPositionToGPSCoords(GameObject.Find("Main Camera").transform.position);
        yield return new WaitForSeconds(0.5f);
    }
}
