using System;
using System.Collections;
using System.Collections.Generic;
using RPG.SceneManagement;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    enum DestinationIdentifier
    {
        A, B, C, D, E
    }

    [SerializeField] int sceneToLoad = -1;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private DestinationIdentifier destination;
    [SerializeField] private float fadeOutTime = 1f;
    [SerializeField] private float fadeInTime = 2f;
    [SerializeField] private float fadeWaitTime = 0.5f;


    void Start()
    {
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            StartCoroutine(Transition());
        }
    }

    private IEnumerator Transition()
    {
        if (sceneToLoad < 0)
        {
            Debug.LogError("Scene to load not set.");
            yield return null;
        }

        DontDestroyOnLoad(gameObject);

        Fader fader = FindObjectOfType<Fader>();

        yield return fader.FadeOut(fadeOutTime);

        //Save current level
        SavingWrapper wrapper = FindObjectOfType<SavingWrapper>();
        wrapper.Save();

        yield return SceneManager.LoadSceneAsync(sceneToLoad);

        //Load current level
        wrapper.Load();

        Portal otherPortal = GetOtherPortal();
        UpdapePlayer(otherPortal);

        //Save last scene
        wrapper.Save();

        yield return new WaitForSeconds(fadeWaitTime);
        yield return fader.FadeIn(fadeInTime);

        Destroy(gameObject);
        print("destroyed");
    }

    private void UpdapePlayer(Portal otherPortal)
    {
        GameObject player = GameObject.FindWithTag("Player");
        player.GetComponent<NavMeshAgent>().enabled = false;
        player.GetComponent<NavMeshAgent>().Warp(otherPortal.spawnPoint.position);
        player.transform.rotation = otherPortal.spawnPoint.rotation;
        player.GetComponent<NavMeshAgent>().enabled = true;
    }

    private Portal GetOtherPortal()
    {
        foreach (Portal portal in FindObjectsOfType<Portal>())
        {
            if (portal == this) continue;
            if(portal.destination != destination) continue;

            return portal;
        }

        return null;
    }
}
