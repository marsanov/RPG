using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CharCanvas : NetworkBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Camera.main == null) return;
        transform.LookAt(Camera.main.transform);
        transform.Rotate(Vector3.up, 180f, Space.Self);
    }
}
