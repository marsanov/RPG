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
        if(gameObject.CompareTag("Enemy")) return;

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
        Health character = GetComponent<Health>();

        GameManager.RegisterCharacter(netID, character);
    }

    void OnDisble()
    {
        if (sceneCamera != null)
            sceneCamera.gameObject.SetActive(true);

        GameManager.UnregisterPlayer(transform.name);
    }
}
