using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookRotation : MonoBehaviour
{
    void Update()
    {
        Vector3 forward = transform.position - Camera.main.transform.position;
        forward.y = 0;
        transform.rotation = Quaternion.LookRotation(forward);
    }
}