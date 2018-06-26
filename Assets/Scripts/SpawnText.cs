using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnText : MonoBehaviour
{
	public GameObject prefab;

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
		if (Physics.Raycast(Camera.main.ScreenPointToRay(center), out hit))
		{
			if (hit.transform != null)
			{
				Debug.Log(hit.transform.name + " ==>> " + hit.point);
				GameObject go = Instantiate(prefab);
				go.transform.position = hit.point;
			}
		}
	}
}
