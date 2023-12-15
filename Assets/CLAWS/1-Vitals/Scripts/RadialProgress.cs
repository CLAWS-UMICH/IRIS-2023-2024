using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RadialProgress : MonoBehaviour
{
	//public GameObject ProgressIndicator;
	public TextMeshPro ProgressIndicator;
	public Image LoadingBar;
	float currentValue;
	public float speed;

	// Use this for initialization
	void Start()
	{
		speed = 1;
		currentValue = 0;
		//ProgressIndicatorText = ProgressIndicator.transform.Find("ProgressIndicator").gameObject.GetComponent<TextMeshPro>();
	}

	// Update is called once per frame
	void Update()
	{
		if (currentValue < 100)
		{
			currentValue += speed * Time.deltaTime;
			ProgressIndicator.text = ((int)currentValue).ToString() + "%";
		}
		else
		{
			ProgressIndicator.text = "Done";
		}

		LoadingBar.fillAmount = currentValue / 100;
	}
}