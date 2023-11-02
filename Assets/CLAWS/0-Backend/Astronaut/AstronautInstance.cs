using System.Collections.Generic;
using UnityEngine;

public class AstronautInstance : MonoBehaviour
{

    public Astronaut astronautInstance;
    // Start is called before the first frame update
    void Start()
    {
        astronautInstance = new Astronaut();
        astronautInstance.currently_navigating = false;
        astronautInstance.inDanger = false;
        astronautInstance.color = "Blue";
    }

}
