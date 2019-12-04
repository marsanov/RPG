using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentObbjectSpawner : MonoBehaviour
{
    [SerializeField] private GameObject persistentObjectPrefab;

    private static bool hasSpawned = false;

    private void Awake()
    {
        if (hasSpawned) return;

        SpawnPersistentObjects();

        hasSpawned = true;
    }

    private void SpawnPersistentObjects()
    {
        GameObject persistentObject = Instantiate(persistentObjectPrefab);
        DontDestroyOnLoad(persistentObject);
    }
}
