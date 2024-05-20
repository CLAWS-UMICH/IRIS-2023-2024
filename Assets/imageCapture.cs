using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using UnityEngine.Windows.WebCam;
using System;
using WebSocketSharp;
using TMPro;

public class imageCapture : MonoBehaviour
{
    PhotoCapture photoCaptureObject = null;
    public Texture2D targetTexture = null;    

    WebsocketDataHandler wdh;

    public Camera tempCamera;

    // public TMP_Text ip_textbox;
    // public TMP_Text raycast_textbox;

    // Ballz
    public GameObject sphere1;
    public GameObject sphere2;
    public GameObject sphere3;
    public GameObject sphere4;

    // public GameObject viewportcube;
    // public GameObject UIA_backplate;

 /*   public GameObject ToolTipAnchor;
    public GameObject ToolTipPivot;
    public TMP_Text ToolTipLabel;

    public GameObject TestLineAnchor;
    public GameObject TestLinePivot;*/
    /*bool showline = false;*/

    Queue<RayCastAction> queued_actions = new Queue<RayCastAction>();

    public class RayCastAction
    {
        public string type; // UIA Egress or Geosample etc
        public Vector3[] keypoints;
        public string action; // What to do
        public Vector3 position;
        public Quaternion rotation;
        public string description;
    }

  /*  public void ToggleLine()
    {
        if (showline)
            showline = false;
        else
            showline = true;
    }
*/
    public void Start()
    {
        // wdh = GameObject.Find("Controller").transform.GetComponent<WebsocketDataHandler>();       

        // Create a temporary camera object
        tempCamera.fieldOfView = Camera.main.fieldOfView;
        tempCamera.aspect = Camera.main.aspect;

        Debug.Log("Starting camera...");
        StartPhotoCapture();

        // If the photo capture object cannot be loaded, a random image is loaded and sent instead for testing sake
        if (photoCaptureObject == null)
        {
            /*string filename = "uia_test.jpg";
            string filePath = Path.Combine(Application.persistentDataPath, filename);
            byte[] imageData = File.ReadAllBytes(filePath);
            Debug.Log("Unable to access camera, loading in example texture instead...");
            // Create a new texture to hold the loaded image
            targetTexture = new Texture2D(3904, 2196);
            // Load the image data into the texture
            targetTexture.LoadImage(imageData);
            Debug.Log("Loaded example texture");*/
        }
        else
        {
            Debug.Log("Started camera");
        }
    }

    void Update()
    {
        // There is a queue of raycast actions, on each update a task is loaded and completed
        if (queued_actions.Count > 0)
        {
            Debug.Log("Handling queued action");
            RayCastAction action = queued_actions.Dequeue();
            tempCamera.transform.position = action.position;
            tempCamera.transform.rotation = action.rotation;

            if (action.type == "UIA_place_panel")
            {
                PlaceUIAPanel(action);
            }
            else if (action.type == "geosample_place_label")
            {
                /*PlaceLabel(action);*/
            }
            else if (action.type == "calibration_place_sphere")
            {
                Debug.Log("Placing ballz");
                PlaceSphere(action);
            }
        }
    }

    Vector3 performRaycast(float x, float y)
    {
        // Define the hit object and the screen point (derived from viewport x,y coordinates) which we will use to perform a raycast
        RaycastHit hit;
        Vector3 point = tempCamera.ScreenToWorldPoint(new Vector3(x, y, tempCamera.nearClipPlane));

        if (Physics.Raycast(tempCamera.transform.position, point - tempCamera.transform.position, out hit, 1000f))
        {
            Debug.DrawLine(tempCamera.transform.position, hit.point, UnityEngine.Color.yellow, 30f);
            return hit.point;
        }
        else
        {
            Debug.Log("No hit");
        }
        // If nothing is found just return a zero vector (ideally there shouldn't be anything where the user was standing)
        return Vector3.zero;
    }

    void PlaceUIAPanel(RayCastAction action)
    {
        bool failed_cast = false;

        // Store all raycast hits and place the UIA panel in the COM (TODO: this could be fixed to not place the panel if the raycast is not aligned)
        Vector3[] hits = new Vector3[4];
        for (int i = 0; i < action.keypoints.Length; i++)
        {
            hits[i] = performRaycast(action.keypoints[i].x, action.keypoints[i].y);
            if (hits[i] == Vector3.zero) { failed_cast = true; break; }
        }


        if (!failed_cast)
        {
            // Vector3 COM = CalculateAverage(hits);
            // UIA_backplate.transform.position = COM;

            // Random ballz to show where the raycasts hit
            sphere1.transform.position = hits[0];
            sphere2.transform.position = hits[1];
            sphere3.transform.position = hits[2];
            sphere4.transform.position = hits[3];

            GetComponent<UIAManager>().SetUIAPanel();
        }
        else
        {
            // raycast_textbox.text = "Failed to place UIA panel";
        }
    }

/*    void PlaceLabel(RayCastAction action)
    {
        Vector3 hit = performRaycast(action.keypoints[0].x, action.keypoints[0].y);
        Debug.Log(hit);
        if (hit != Vector3.zero)
        {
            ToolTipAnchor.transform.position = hit;
            ToolTipLabel.text = action.description;
            ToolTipPivot.transform.position = hit + Vector3.up;
            // raycast_textbox.text = "Placed tooltip";

            if (showline)
            {
                TestLineAnchor.transform.position = hit;
                TestLinePivot.transform.position = tempCamera.transform.position;
            }
        }
        else
        {
            // raycast_textbox.text = "Failed to place tooltip";
        }
    }*/

    void PlaceSphere(RayCastAction action)
    {
        Vector3 hit = performRaycast(action.keypoints[0].x, action.keypoints[0].y);
        if (hit != Vector3.zero)
        {
            sphere1.transform.position = hit;
            // raycast_textbox.text = "Placed sphere1";
        }
        else
        {
            // raycast_textbox.text = "Failed to place ball";
        }
    }


/*    public void HandleJsonMessage(string jsonData)
    {
        if (jsonData.StartsWith("UIA"))
        {
            Debug.Log("Starting UIA Task");
            processUIAwebsocket(jsonData);
        }
        else if (jsonData.StartsWith("geosample"))
        {
            Debug.Log("Starting Geosample thing");
            processGeosampleWebsocket(jsonData);
        }
        else if (jsonData.StartsWith("calibrate"))
        {
            Debug.Log("Starting Calibration");
            processCalibration(jsonData);
        }
        else
        {
            Debug.Log("Unrecognized command");
        }
    }*/

    public void processUIAwebsocket(string position, string rotation, string points)
    {
        // UIA:x,y,z:a,b,c,d:x,y$x,y$x,y$x,y        
        Vector3 head_pos = parseVector(position);
        Quaternion head_rot = parseQuaternion(rotation);
        Vector3[] corners = parse_points(points);
        queued_actions.Enqueue(new RayCastAction
        {
            action = "rectangle",
            type = "UIA_place_panel",
            position = head_pos,
            rotation = head_rot,
            keypoints = corners,
            description = ""
        });
    }

    private void processGeosampleWebsocket(string message)
    {
        // geosample:x,y,z:a,b,c,d:x,y
        string[] components = message.Split(":");
        Vector3 head_pos = parseVector(components[1]);
        Quaternion head_rot = parseQuaternion(components[2]);
        Vector3[] point = parse_points(components[3]);
        queued_actions.Enqueue(new RayCastAction
        {
            action = "points_label",
            type = "geosample_place_label",
            position = head_pos,
            rotation = head_rot,
            keypoints = point,
            description = components[4]
        });
    }
    private void processCalibration(string message)
    {
        // calibration:x,y,z:a,b,c,d:x,y
        string[] components = message.Split(":");
        Vector3 head_pos = parseVector(components[1]);
        Quaternion head_rot = parseQuaternion(components[2]);
        Vector3[] point = parse_points(components[3]);
        queued_actions.Enqueue(new RayCastAction
        {
            action = "points_sphere",
            type = "calibration_place_sphere",
            position = head_pos,
            rotation = head_rot,
            keypoints = point,
            description = "ballz"
        });
    }

       
    void SendTextureWS(string type)
    { 
        Debug.Log("Sending Image");
        /*string head_pos = type + ":" + Camera.main.transform.position.ToString() + ":" + Camera.main.transform.forward.ToString() + ":" + Camera.main.transform.rotation.ToString();
        string resp = head_pos + "}$#EndHeadCoord" + Convert.ToBase64String(targetTexture.EncodeToJPG());*/
        if (type == "UIA")
        {
            UIAImage uiaImage = new UIAImage(Convert.ToBase64String(targetTexture.EncodeToJPG()), "", Camera.main.transform.position.ToString(), Camera.main.transform.rotation.ToString());
            wdh = GameObject.Find("Controller").transform.GetComponent<WebsocketDataHandler>();
            Debug.Log(wdh);
            wdh.SendUIA(uiaImage);
        }
    }
    
    public void SnapPhoto(string type)
    {
        if (photoCaptureObject == null)
        {
            Debug.Log("Not taking a picture cuz camera is not initialized, using default pic");
            SendTextureWS(type);
        }
        else
        {
            Debug.Log("Taking a picture");
            photoCaptureObject.TakePhotoAsync((result, frame) => OnCapturedPhotoToWebSocket(result, frame, type));
        }
    }

    [ContextMenu("func snapphoto")]
    public void UIATest()
    {
        SnapPhoto("UIA");
    }

    void StartPhotoCapture()
    {
        PhotoCapture.CreateAsync(false, OnPhotoCaptureCreated);
    }
    public void StopCamera()
    {
        Debug.Log("Stopping camera");
        photoCaptureObject.StopPhotoModeAsync(OnPhotoModeStopped);
    }
    void OnPhotoCaptureCreated(PhotoCapture captureObject)
    {
        try
        {
            photoCaptureObject = captureObject;

            CameraParameters cameraParameters = new CameraParameters(WebCamMode.PhotoMode);
            cameraParameters.hologramOpacity = 0.0f;
            cameraParameters.cameraResolutionWidth = 1024;
            cameraParameters.cameraResolutionHeight = 1024;
            Resolution cameraResolution = PhotoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).First();
            targetTexture = new Texture2D(cameraResolution.width, cameraResolution.height);

            // Start Camera
            captureObject.StartPhotoModeAsync(cameraParameters, OnPhotoModeStarted);
        }
        catch (Exception ex)
        {
            Debug.Log("Failed to start camera");
            Debug.Log(ex.ToString());
        }

    }

    public void SendJsonData(string jsonData)
    {
        Debug.Log("SendJsonData");
    }

    void OnPhotoModeStarted(PhotoCapture.PhotoCaptureResult result)
    {
        if (result.success)
        {
            Debug.Log("Successfully Initialized Camera");
        }
        else
        {
            Debug.LogError("Unable to start photo mode!");
        }
    }

    void OnCapturedPhotoToWebSocket(PhotoCapture.PhotoCaptureResult result, PhotoCaptureFrame photoCaptureFrame, string type)
    {
        if (result.success)
        {
            photoCaptureFrame.UploadImageDataToTexture(targetTexture);
            SendTextureWS(type);
        }
        else
        {
            Debug.LogError("Failed to capture photo to memory!");
        }
    }

    void OnPhotoModeStopped(PhotoCapture.PhotoCaptureResult result)
    {
        // Release the PhotoCapture object
        photoCaptureObject.Dispose();
        photoCaptureObject = null;
    }

    public static Vector3 CalculateAverage(Vector3[] vectorArray)
    {
        // Initialize variables to store the sum of each component
        float sumX = 0;
        float sumY = 0;
        float sumZ = 0;

        // Iterate through the array and accumulate the sum of each component
        foreach (Vector3 vector in vectorArray)
        {
            sumX += vector.x;
            sumY += vector.y;
            sumZ += vector.z;
        }

        // Calculate the average of each component by dividing the sum by the total number of elements
        float averageX = sumX / vectorArray.Length;
        float averageY = sumY / vectorArray.Length;
        float averageZ = sumZ / vectorArray.Length;

        // Create and return a new Vector3 instance with the average values
        return new Vector3(averageX, averageY, averageZ);
    }

    Vector3 parseVector(string input)
    {
        // Debug.Log("Parsing Vector");
        // Remove parentheses and split into components
        string[] components = input.Trim('(', ')').Split(',');

        // Parse components as floats
        float xt = float.Parse(components[0]);
        float yt = float.Parse(components[1]);
        float zt = float.Parse(components[2]);

        Vector3 vec = new Vector3(xt, yt, zt);
        return vec;
    }

    Quaternion parseQuaternion(string input)
    {
        // Debug.Log("Parsing Quaternion");
        string[] components = input.Trim('(', ')').Split(',');

        float x = float.Parse(components[0]);
        float y = float.Parse(components[1]);
        float z = float.Parse(components[2]);
        float w = float.Parse(components[3]);

        Quaternion qua = new Quaternion(x, y, z, w);
        return qua;
    }

    Vector3[] parse_points(string input)
    {
        // Debug.Log("Parsing Rectangle");
        string[] cornerString = input.Split("$");
        Vector3[] corners = new Vector3[4];
        for (int i = 0; i < cornerString.Length; i++)
        {
            if (cornerString[i].Length > 0)
            {
                string[] components = cornerString[i].Split(",");
                float x = float.Parse(components[0]);
                float y = float.Parse(components[1]);
                corners[i] = new Vector3(x, y, 0);
            }
        }
        return corners;
    }
}
