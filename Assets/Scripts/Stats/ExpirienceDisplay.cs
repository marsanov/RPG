using System.Collections;
using System.Collections.Generic;
using RPG.Resources;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace RPG.Stats
{
    public class ExpirienceDisplay : NetworkBehaviour
    {
        [SerializeField] private float expirienceToNextLevel = 0f;
        [SerializeField] private Progression progresiion = null;

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
            gameObject.GetComponent<Image>().fillAmount = CurrentFill();
        }

        float CurrentFill()
        {
            float fill = 0;
            int currentLevel = player.GetComponent<BaseStats>().GetLevel();
            expirienceToNextLevel = player.GetComponent<BaseStats>().GetStat(Stat.ExpirienceToLevelUp);
            float currentXP = player.GetComponent<Expirience>().GetPoints();

            if (currentLevel == 1)
            {
                return currentXP / expirienceToNextLevel;
            }
            else
            {
                float prevLevelXP =
                    progresiion.GetStat(Stat.ExpirienceToLevelUp, CharacterClass.Player, currentLevel - 1);
                
                fill = (currentXP - prevLevelXP) / (expirienceToNextLevel - prevLevelXP);
                return fill;
            }
        }
    }

}
