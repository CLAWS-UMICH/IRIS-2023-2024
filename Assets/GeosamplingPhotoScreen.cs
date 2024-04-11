using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeosamplingPhotoScreen : MonoBehaviour
{
    [SerializeField] GameObject TakePhotoButton;
    [SerializeField] GameObject RetryButton;
    [SerializeField] GameObject UsePhotoButton;
    [SerializeField] GameObject BlankPictureQuad;
    [SerializeField] GameObject PictureQuad;
    [SerializeField] GameObject outputQuad;

    [SerializeField] SingleGeosampleScreen Geosample;


    private void OnEnable()
    {
        StartPictureMode();
    }

    public void StartPictureMode()
    {
        TakePhotoButton.SetActive(true);
        RetryButton.SetActive(false);
        UsePhotoButton.SetActive(false);

        BlankPictureQuad.SetActive(true);
        PictureQuad.SetActive(false);

        Geosample.gameObject.SetActive(false);
    }
    
    public void TakePhoto()
    {
        GetComponent<Screenshot>().TakePhoto();

        PictureQuad.SetActive(true);
        BlankPictureQuad.SetActive(false);

        TakePhotoButton.SetActive(false);
        RetryButton.SetActive(true);
        UsePhotoButton.SetActive(true);
    }

    public void UseThisPhoto()
    {
        string s = GetComponent<Screenshot>().UseThisPhoto(outputQuad, Geosample.Sample.geosample_id);
        outputQuad.SetActive(true);

        if (s != null)
        {
            Geosample.Sample.photo_jpg = s;
            GeosamplingManager.SendData();
            // Debug.Log(Geosample.Sample.photo_jpg);
        }
        else
        {
            Debug.LogError("[Geo] Invalid photo jpg string?");
        }
        Geosample.gameObject.SetActive(true);
        Geosample.OnPhotoButtonPressed();

    }
}
