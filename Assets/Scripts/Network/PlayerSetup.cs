using RPG.Resources;
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
                gameObject.layer = LayerMask.NameToLayer("RemotePlayer");
            }
        else
        {
            sceneCamera = GameObject.FindWithTag("SceneCamera");
            if (sceneCamera != null)
                sceneCamera.gameObject.SetActive(false);
        }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        string netID = GetComponent<NetworkIdentity>().netId.ToString();
        Health player = GetComponent<Health>();

        GameManager.RegisterPlayer(netID, player);
    }

    void OnDisble()
    {
        if (sceneCamera != null)
            sceneCamera.gameObject.SetActive(true);

        GameManager.UnregisterPlayer(transform.name);
    }
}
