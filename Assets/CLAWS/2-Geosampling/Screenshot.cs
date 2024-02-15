using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using UnityEngine.Windows.WebCam;
using System;

[System.Serializable]
public class Screenshot : MonoBehaviour
{

    private GameObject button;
    PhotoCapture photoCaptureObject = null;
    Texture2D targetTexture = null;

    public string sampleName = "GeoSample";

    [SerializeField] private Material defaultmaterial;
    [SerializeField] private Material defaultConfirmMaterial;
    [SerializeField] private GameObject confirmationQuad;


    // take photo button
    [ContextMenu("func TakePhoto")]
    public void TakePhoto()
    {
#if !UNITY_WEBGL
        Resolution cameraResolution = PhotoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).First();
        targetTexture = new Texture2D(cameraResolution.width, cameraResolution.height);

        Renderer r = confirmationQuad.GetComponent<Renderer>();
        r.material = defaultConfirmMaterial;

        // create photocapture object
        PhotoCapture.CreateAsync(false, delegate (PhotoCapture captureObject)
        {
            photoCaptureObject = captureObject;
            CameraParameters cameraParameters = new CameraParameters
            {
                hologramOpacity = 0.0f,
                cameraResolutionWidth = cameraResolution.width,
                cameraResolutionHeight = cameraResolution.height,
                pixelFormat = CapturePixelFormat.BGRA32
            };

            // activate camera
            photoCaptureObject.StartPhotoModeAsync(cameraParameters, delegate (PhotoCapture.PhotoCaptureResult result)
            {
                // Take a picture
                photoCaptureObject.TakePhotoAsync(PhotoToMemory);
            });
        });
#endif
    }

#if !UNITY_WEBGL

    void PhotoToMemory(PhotoCapture.PhotoCaptureResult result, PhotoCaptureFrame photoCaptureFrame)
    {
        if (result.success)
        {

            // Copy the raw image data into the target texture
            photoCaptureFrame.UploadImageDataToTexture(targetTexture);

            Renderer r = confirmationQuad.GetComponent<Renderer>();
            r.material = new Material(Shader.Find("Unlit/Texture"));
            r.material.SetTexture("_MainTex", targetTexture);
        }
        // Deactivate the camera
        photoCaptureObject.StopPhotoModeAsync(OnStoppedPhotoMode);
    }
#endif

    // use this photo button
    [ContextMenu("func UseThisPhoto")]
    public string UseThisPhoto(GameObject outputQuad)
    {
#if !UNITY_WEBGL
        Renderer r = outputQuad.GetComponent<Renderer>();
        r.material = new Material(Shader.Find("Unlit/Texture"));
        r.material.SetTexture("_MainTex", targetTexture);

        if (Directory.Exists(Application.persistentDataPath + "/" + sampleName)) 
            System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/" + sampleName);

        string fileName = System.DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss") + ".jpg";
        string filePath = Path.Combine(Application.persistentDataPath, fileName);

        var jpg = targetTexture.EncodeToJPG();
        File.WriteAllBytes(filePath, jpg);
        Debug.Log(Application.persistentDataPath + fileName);

        string s = Convert.ToBase64String(jpg); // TODO send to WEB TEAM!!!
        return s;
#else
        return ""
#endif
    }

#if !UNITY_WEBGL
    void OnStoppedPhotoMode(PhotoCapture.PhotoCaptureResult result)
    {
        photoCaptureObject.Dispose();
        photoCaptureObject = null;
    }
#endif


    public void TogglePanel(GameObject panel)
    {
        panel.SetActive(!button.activeSelf);
    }

}
