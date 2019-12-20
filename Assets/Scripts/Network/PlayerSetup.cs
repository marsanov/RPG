using UnityEngine;
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
            }
        else
        {
            sceneCamera = GameObject.FindWithTag("SceneCamera");
            if (sceneCamera != null)
                sceneCamera.gameObject.SetActive(false);
        }

    }

    void OnDisble()
    {
        if (sceneCamera != null)
            sceneCamera.gameObject.SetActive(true);
        print(sceneCamera);
    }
}
