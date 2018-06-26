using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnText : MonoBehaviour
{
	public GameObject prefab;
	string result;

	void Start()
	{
	}

	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Spawn();
		}
	}

	void Spawn()
	{
		RaycastHit hit;
		Vector2 center = new Vector2(Screen.width / 2, Screen.height / 2); //屏幕中心点
		Ray ray = Camera.main.ScreenPointToRay(center);
		if (Physics.Raycast(ray, out hit))
		{
			if (hit.transform != null)
			{
				Debug.Log(hit.transform.name + " ==>> " + hit.point);
				GameObject go = Instantiate(prefab);
				go.transform.position = hit.point;
				go.GetComponent<TextMesh>().text = result;
			}
		}
	}

	// coreml识别到的结果回调，oc层异步每帧调用
	public void RecogniseCallback(string log)
	{
		Debug.Log("识别结果 ==>> " + log);
		result = log;
	}
}
