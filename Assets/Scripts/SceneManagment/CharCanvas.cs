using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharCanvas : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.LookAt(Camera.main.transform);
        transform.Rotate(Vector3.up, 180f, Space.Self);
    }
}
