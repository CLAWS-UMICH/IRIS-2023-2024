using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum AlertEnum
{
    LLMC,
    Vital,
    Task,
    NewStation,
    NewSample,
    NewInterest,
    NewDanger
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

        //StartCoroutine(_AddAlert());
    }

    /*IEnumerator _AddAlert()
    {

        yield return new WaitForSeconds(2);
        EventBus.Publish(new CreateAlert(AlertEnum.Task, "New Task", "Equipment Repair"));
        yield return new WaitForSeconds(4);
        EventBus.Publish(new CreateAlert(AlertEnum.NewDanger, "New Danger Point C", "Crater", "C"));
        yield return new WaitForSeconds(3);
        EventBus.Publish(new CreateAlert(AlertEnum.Vital, "Vital Warning", "Heart Rate: 120 BPM"));
    }*/

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
            case AlertEnum.LLMC:
                icon.transform.Find("LMCC").gameObject.SetActive(true);
                break;

            case AlertEnum.Vital:
                icon.transform.Find("Vital").gameObject.SetActive(true);
                pre.transform.Find("Warning").gameObject.SetActive(true);
                pre.transform.Find("Quad").gameObject.SetActive(false);
                break;

            case AlertEnum.Task:
                icon.transform.Find("Task").gameObject.SetActive(true);
                break;

            case AlertEnum.NewStation:
                icon.transform.Find("Station").gameObject.SetActive(true);
                icon.transform.Find("Letter").GetComponent<TextMeshPro>().text = e.letter;
                icon.transform.Find("Letter").gameObject.SetActive(true);
                break;

            case AlertEnum.NewSample:
                icon.transform.Find("Sample").gameObject.SetActive(true);
                icon.transform.Find("Letter").GetComponent<TextMeshPro>().text = e.letter;
                icon.transform.Find("Letter").gameObject.SetActive(true);
                break;

            case AlertEnum.NewInterest:
                icon.transform.Find("Interest").gameObject.SetActive(true);
                icon.transform.Find("Letter").GetComponent<TextMeshPro>().text = e.letter;
                icon.transform.Find("Letter").gameObject.SetActive(true);
                break;

            case AlertEnum.NewDanger:
                icon.transform.Find("Danger").gameObject.SetActive(true);
                icon.transform.Find("Letter").GetComponent<TextMeshPro>().text = e.letter;
                icon.transform.Find("Letter").gameObject.SetActive(true);
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
        icon.transform.Find("Letter").GetComponent<TextMeshPro>().text = "";
        icon.transform.Find("Letter").gameObject.SetActive(false);
        icon.transform.Find("LMCC").gameObject.SetActive(false);
        icon.transform.Find("Vital").gameObject.SetActive(false);
        icon.transform.Find("Task").gameObject.SetActive(false);
        icon.transform.Find("Station").gameObject.SetActive(false);
        icon.transform.Find("Sample").gameObject.SetActive(false);
        icon.transform.Find("Interest").gameObject.SetActive(false);
        icon.transform.Find("Danger").gameObject.SetActive(false);
    }

}
