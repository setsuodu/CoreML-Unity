using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookRota : MonoBehaviour
{
	public Transform target;

	void Start()
	{
        target = Camera.main.transform;      
	}

	void Update()
	{
		if (target != null)
		{
			Vector3 lookPos = transform.position - target.transform.position;
			lookPos.y = 0;
			Quaternion rotation = Quaternion.LookRotation(lookPos);
			transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.1f);
		}
	}
}
