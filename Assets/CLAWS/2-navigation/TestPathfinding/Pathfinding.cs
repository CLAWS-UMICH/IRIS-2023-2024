using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour {

    Grid GridReference;//For referencing the grid class
    public Transform StartPosition;//Starting position to pathfind from
    public Transform TargetPosition;//Starting position to pathfind to
    public GameObject prefabToInstantiate;
    public GameObject empty;

    [SerializeField] int crumbsToShow = 3;
    [SerializeField] float crumbYChange = 0f;
    public Dictionary<int, GameObject> indexToBreadCrumb = new Dictionary<int, GameObject>();

    private Subscription<BreadCrumbCollisionEvent> collisionEvent;

    GameObject playspace;
    GameObject player;


    private void Awake()//When the program starts
    {
        GridReference = GetComponent<Grid>();//Get a reference to the game manager
    }

    private void Start()
    {
        collisionEvent = EventBus.Subscribe<BreadCrumbCollisionEvent>(OnBreadCollision);
        playspace = GameObject.Find("MixedRealityPlayspace");
        player = playspace.transform.Find("Main Camera").gameObject;
    }

    public void startPathFinding(Transform start, Transform end)
    {
        StartPosition = start;
        TargetPosition = end;

        FindNewPath();
    }

    public void startPathFinding(Transform start, Location end)
    {
        StartPosition = start;

        // Ensure TargetPosition is properly initialized
        if (TargetPosition == null)
        {
            TargetPosition = new GameObject("TargetPosition").transform;
        }

        TargetPosition.position = GPSUtils.GPSCoordsToAppPosition(end);

        FindNewPath();
    }

    // Button on screen will find the path everytime when pressed to not have it run every frame
    // Could maybe be changed to have it clicked once and always running, but for now, this will suffice
    public void FindNewPath()
    {
        FindPath(StartPosition.position, TargetPosition.position);//Find a path to the goal
    }

    /*
    private void Update()//Every frame
    {
        FindPath(StartPosition.position, TargetPosition.position);//Find a path to the goal
    }*/
    

    void FindPath(Vector3 a_StartPos, Vector3 a_TargetPos)
    {
        Node StartNode = GridReference.NodeFromWorldPoint(a_StartPos);//Gets the node closest to the starting position
        Node TargetNode = GridReference.NodeFromWorldPoint(a_TargetPos);//Gets the node closest to the target position

        Heap<Node> OpenList = new Heap<Node>(GridReference.MaxSize);//List of nodes for the open list
        HashSet<Node> ClosedList = new HashSet<Node>();//Hashset of nodes for the closed list

        OpenList.Add(StartNode);//Add the starting node to the open list to begin the program

        while(OpenList.Count > 0)//Whilst there is something in the open list
        {
            Node CurrentNode = OpenList.RemoveFirst();//Create a node and set it to the first item in the open list
            /*
            for(int i = 1; i < OpenList.Count; i++)//Loop through the open list starting from the second object
            {
                if (OpenList[i].FCost < CurrentNode.FCost || OpenList[i].FCost == CurrentNode.FCost && OpenList[i].ihCost < CurrentNode.ihCost)//If the f cost of that object is less than or equal to the f cost of the current node
                {
                    CurrentNode = OpenList[i];//Set the current node to that object
                }
            }
            OpenList.Remove(CurrentNode);//Remove that from the open list
            */
            ClosedList.Add(CurrentNode);//And add it to the closed list

            if (CurrentNode == TargetNode)//If the current node is the same as the target node
            {
                GetFinalPath(StartNode, TargetNode);//Calculate the final path
            }

            foreach (Node NeighborNode in GridReference.GetNeighboringNodes(CurrentNode))//Loop through each neighbor of the current node
            {
                if (!NeighborNode.bIsWall || ClosedList.Contains(NeighborNode))//If the neighbor is a wall or has already been checked
                {
                    continue;//Skip it
                }
                int MoveCost = CurrentNode.igCost + GetEuclideanDistance(CurrentNode, NeighborNode) + NeighborNode.movementPenalty;//Get the F cost of that neighbor

                if (MoveCost < NeighborNode.igCost || !OpenList.Contains(NeighborNode))//If the f cost is greater than the g cost or it is not in the open list
                {
                    NeighborNode.igCost = MoveCost;//Set the g cost to the f cost
                    NeighborNode.ihCost = GetEuclideanDistance(NeighborNode, TargetNode);//Set the h cost
                    NeighborNode.ParentNode = CurrentNode;//Set the parent of the node for retracing steps

                    if(!OpenList.Contains(NeighborNode))//If the neighbor is not in the openlist
                    {
                        OpenList.Add(NeighborNode);//Add it to the list
                    } else
                    {
                        OpenList.UpdateItem(NeighborNode);
                    }
                }
            }

        }
    }

    public void destroyCurrentBreadCrumbs()
    {
        // Iterate through each child of the parent
        for (int i = empty.transform.childCount - 1; i >= 0; i--)
        {
            // Destroy the child object
            Destroy(empty.transform.GetChild(i).gameObject);
        }
        AstronautInstance.User.BreadCrumbData.AllCrumbs.Clear();
        indexToBreadCrumb.Clear();
        // TODO: Should we send the breadcrumb data to web here?
    }

    void GetFinalPath(Node a_StartingNode, Node a_EndNode)
    {
        destroyCurrentBreadCrumbs(); // Destroys all current breadcrumbs
        List<Node> FinalPath = new List<Node>();//List to hold the path sequentially 
        Node CurrentNode = a_EndNode;//Node to store the current node being checked

        while(CurrentNode != a_StartingNode)//While loop to work through each node going through the parents to the beginning of the path
        {
            FinalPath.Add(CurrentNode);//Add that node to the final path
            CurrentNode = CurrentNode.ParentNode;//Move onto its parent node

        }

        FinalPath.Reverse();//Reverse the path to get the correct order

        GridReference.FinalPath = FinalPath;//Set the final path
        int index = 0;
        // Instantiate objects along the final path
        for (int i = 0; i < FinalPath.Count; i++)
        {
            if (i % 2 == 1)
            {
                continue;
            }
            GameObject instantiatedObject = Instantiate(prefabToInstantiate, FinalPath[i].vPosition, Quaternion.identity);
            if (i != FinalPath.Count - 1)
            {
                Vector3 direction = FinalPath[i + 1].vPosition - FinalPath[i].vPosition;
                Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up) * Quaternion.Euler(90f, 0f, 0f);
                instantiatedObject.transform.rotation = rotation;
            }
            else if (i == FinalPath.Count - 1)
            {
                Vector3 direction = TargetPosition.position - FinalPath[i].vPosition;
                Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up) * Quaternion.Euler(90f, 0f, 0f);
                instantiatedObject.transform.rotation = rotation;
            }
            else
            {
                instantiatedObject.transform.LookAt(FinalPath[i].vPosition);
            }
            instantiatedObject.SetActive(false);
            Breadcrumb b = new Breadcrumb(index, GPSUtils.AppPositionToGPSCoords(instantiatedObject.transform.position), 1);
            AstronautInstance.User.BreadCrumbData.AllCrumbs.Add(b);
            indexToBreadCrumb[index] = instantiatedObject;
            instantiatedObject.GetComponent<BreadCrumbController>().UpdateInfo(index);
            // TODO: Should we send the breadcrumb data to web here?
            instantiatedObject.transform.SetParent(empty.transform);
            index++;
        }

        showCrumbs();
    }

    private void showCrumbs()
    {
        int number = Mathf.Min(crumbsToShow, AstronautInstance.User.BreadCrumbData.AllCrumbs.Count);

        for (int i = 0; i < number; i++)
        {
            GameObject gm = indexToBreadCrumb[AstronautInstance.User.BreadCrumbData.AllCrumbs[i].crumb_id];
            Vector3 newPosition = gm.transform.position;
            newPosition.y = player.transform.position.y - crumbYChange;
            gm.transform.position = newPosition;
            gm.SetActive(true);
        }
    }

    private void OnBreadCollision(BreadCrumbCollisionEvent e)
    {
        // Remove objects from AllCrumbs list
        AstronautInstance.User.BreadCrumbData.AllCrumbs.RemoveAll(crumb => crumb.crumb_id <= e.index);

        // Remove objects from indexToBreadCrumb dictionary
        List<int> keysToRemove = new List<int>();
        foreach (var entry in indexToBreadCrumb)
        {
            if (entry.Key <= e.index)
            {
                keysToRemove.Add(entry.Key);
                Destroy(entry.Value);
            }
        }

        // Remove items from the dictionary based on the keysToRemove list
        foreach (int key in keysToRemove)
        {
            indexToBreadCrumb.Remove(key);
        }

        showCrumbs();
    }

    int GetEuclideanDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.iGridX - nodeB.iGridX);
        int dstY = Mathf.Abs(nodeA.iGridY - nodeB.iGridY);

        if (dstX > dstY)
        {
            return 14 * dstY + 10 * (dstX - dstY);
        }

        return 14 * dstX + 10 * (dstY - dstX);
        //return Mathf.RoundToInt(Mathf.Sqrt(dstX * dstX + dstY * dstY));
    }
}
