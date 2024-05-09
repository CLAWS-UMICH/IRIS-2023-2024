using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum AlertEnum
{
    Vital_Heart,
    Vital_O2,
    Vital_Battery,
    Vital_Coolant,
    Vital_Scrubber,
    Vital_CO2,
    Vital_Temp,
    Vital_Pressure,
    Vital_Fan
}

public class NotificationController : MonoBehaviour
{
    public GameObject pre;
    private Subscription<CreateAlert> alertEvent;
    private List<GameObject> alerts = new List<GameObject>();
    private NewScroll scroll;

    // Start is called before the first frame update
    void Start()
    {
        scroll = transform.Find("Scroll").GetComponent<NewScroll>();
        alertEvent = EventBus.Subscribe<CreateAlert>(Alert);

        StartCoroutine(_Test());
    }

    IEnumerator _Test()
    {
        yield return new WaitForSeconds(2f);
        EventBus.Publish<CreateAlert>(new CreateAlert(AlertEnum.Vital_Battery, "Heart Rate High", "160 BPM, slow down"));
        yield return new WaitForSeconds(2f);
        EventBus.Publish<CreateAlert>(new CreateAlert(AlertEnum.Vital_Pressure, "O2 Sec Pressure High", "4.1 PSI"));
    }

    public void AddAlert(GameObject prefab)
    {
        GameObject newButton = scroll.HandleAddingButton(prefab);
        alerts.Add(newButton);
        StartCoroutine(_DeleteAlert(newButton));
    }

    IEnumerator _DeleteAlert(GameObject alert)
    {
        yield return new WaitForSeconds(5);
        scroll.HandleButtonDeletion(alert);
        alerts.Remove(alert);
    }

    private void Alert(CreateAlert e)
    {
        ResetPrefab();
        pre.transform.Find("Title").transform.Find("Text").GetComponent<TextMeshPro>().text = e.title;
        pre.transform.Find("Desc").transform.Find("Text").GetComponent<TextMeshPro>().text = e.desc;

        GameObject icon = pre.transform.Find("Icon").gameObject;
        switch (e.alertType) {
            case AlertEnum.Vital_Heart:
                icon.transform.Find("Vital_Heart").gameObject.SetActive(true);
                pre.transform.Find("Warning").gameObject.SetActive(true);
                pre.transform.Find("Quad").gameObject.SetActive(false);
                break;

            case AlertEnum.Vital_O2:
                icon.transform.Find("Vital_O2").gameObject.SetActive(true);
                pre.transform.Find("Warning").gameObject.SetActive(true);
                pre.transform.Find("Quad").gameObject.SetActive(false);
                break;

            case AlertEnum.Vital_Battery:
                icon.transform.Find("Vital_Battery").gameObject.SetActive(true);
                pre.transform.Find("Warning").gameObject.SetActive(true);
                pre.transform.Find("Quad").gameObject.SetActive(false);
                break;

            case AlertEnum.Vital_Coolant:
                icon.transform.Find("Vital_Coolant").gameObject.SetActive(true);
                pre.transform.Find("Warning").gameObject.SetActive(true);
                pre.transform.Find("Quad").gameObject.SetActive(false);
                break;

            case AlertEnum.Vital_Scrubber:
                icon.transform.Find("Vital_Scrubber").gameObject.SetActive(true);
                pre.transform.Find("Warning").gameObject.SetActive(true);
                pre.transform.Find("Quad").gameObject.SetActive(false);
                break;

            case AlertEnum.Vital_CO2:
                icon.transform.Find("Vital_CO2").gameObject.SetActive(true);
                pre.transform.Find("Warning").gameObject.SetActive(true);
                pre.transform.Find("Quad").gameObject.SetActive(false);
                break;

            case AlertEnum.Vital_Temp:
                icon.transform.Find("Vital_Temp").gameObject.SetActive(true);
                pre.transform.Find("Warning").gameObject.SetActive(true);
                pre.transform.Find("Quad").gameObject.SetActive(false);
                break;

            case AlertEnum.Vital_Pressure:
                icon.transform.Find("Vital_Pressure").gameObject.SetActive(true);
                pre.transform.Find("Warning").gameObject.SetActive(true);
                pre.transform.Find("Quad").gameObject.SetActive(false);
                break;

            case AlertEnum.Vital_Fan:
                icon.transform.Find("Vital_Fan").gameObject.SetActive(true);
                pre.transform.Find("Warning").gameObject.SetActive(true);
                pre.transform.Find("Quad").gameObject.SetActive(false);
                break;

            default:
                Debug.Log("Unknown alert type");
                break;
        }

        AddAlert(pre);
    }

    private void ResetPrefab()
    {
        pre.transform.Find("Title").transform.Find("Text").GetComponent<TextMeshPro>().text = "";
        pre.transform.Find("Desc").transform.Find("Text").GetComponent<TextMeshPro>().text = "";
        pre.transform.Find("Warning").gameObject.SetActive(false);
        pre.transform.Find("Quad").gameObject.SetActive(true);

        GameObject icon = pre.transform.Find("Icon").gameObject;

        // Loop through all icons of this GameObject
        foreach (Transform i in icon.transform)
        {
            // Deactivate each icon
            i.gameObject.SetActive(false);
        }
    }

}
