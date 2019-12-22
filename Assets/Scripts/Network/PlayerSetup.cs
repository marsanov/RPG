﻿using UnityEngine;
using UnityEngine.Networking;

public class PlayerSetup : NetworkBehaviour
{
    [SerializeField] private Behaviour[] componentsToDisable;

    private GameObject sceneCamera;
    private Camera playerCamera;

    // Start is called before the first frame update
    void Start()
    {
        if (!isLocalPlayer)
            foreach (Behaviour behaviour in componentsToDisable)
            {
                behaviour.enabled = false;
                gameObject.layer = LayerMask.NameToLayer("RemotePlayer");
            }
        else
        {
            sceneCamera = GameObject.FindWithTag("SceneCamera");
            if (sceneCamera != null)
                sceneCamera.gameObject.SetActive(false);
        }

        transform.name = "Player " + GetComponent<NetworkIdentity>().netId;
    }

    void OnDisble()
    {
        if (sceneCamera != null)
            sceneCamera.gameObject.SetActive(true);
        print(sceneCamera);
    }
}
