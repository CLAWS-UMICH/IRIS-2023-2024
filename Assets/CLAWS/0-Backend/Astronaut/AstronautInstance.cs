using System.Collections.Generic;
using UnityEngine;

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
    }
}
