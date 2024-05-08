using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointFollow3D : MonoBehaviour
{
    GameObject player;
    GameObject body;
    GameObject title;
    GameObject icon;
    float updateDistance;

    private void Awake()
    {
        player = GameObject.Find("Main Camera");
        body = gameObject.transform.Find("Body").gameObject;
        title = body.transform.Find("Title").gameObject;
        icon = body.transform.Find("Quad").gameObject;
    }

    private void Start()
    {
        title.SetActive(true);
        icon.SetActive(true);


    }


    // Update is called once per frame
    void Update()
    {
        updateDistance = Vector3.Distance(body.transform.position, player.transform.position);
        body.transform.rotation = Quaternion.Euler(0, player.transform.eulerAngles.y, 0);
    }
}
