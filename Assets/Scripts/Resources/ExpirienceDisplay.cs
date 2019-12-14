using System.Collections;
using System.Collections.Generic;
using RPG.Resources;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

public class ExpirienceDisplay : MonoBehaviour
{
    [SerializeField] private float expirienceToNextLevel = 100f;

    private GameObject player = null;
    private float currentPlayerExpirience = 0;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
         gameObject.GetComponent<Image>().fillAmount = player.GetComponent<Expirience>().GetPoints() / expirienceToNextLevel;
    }
}
