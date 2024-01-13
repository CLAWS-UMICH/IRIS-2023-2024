using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavController : MonoBehaviour
{
    GameObject playspace;
    GameObject player;
    Pathfinding pf;

    [SerializeField] GameObject target;

    private Subscription<StartPathfinding> pathfinding;

    // Start is called before the first frame update
    void Start()
    {
        pf = GetComponent<Pathfinding>();
        playspace = GameObject.Find("MixedRealityPlayspace");
        player = playspace.transform.Find("Main Camera").gameObject;

        // Subscribe to the events
        pathfinding = EventBus.Subscribe<StartPathfinding>(OnPathfinding);
    }

    public void testNav()
    {
        pf.startPathFinding(player.transform, target.transform);
    }

    private void OnPathfinding(StartPathfinding e)
    {
        pf.startPathFinding(player.transform, e.location);

    }
}
