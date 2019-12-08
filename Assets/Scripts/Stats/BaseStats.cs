using Rpg.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1f, 99f)]
        [SerializeField] private int level = 1;
        [SerializeField] private CharacterClass characterClass;
        [SerializeField] private Progression progression = null;
    }

}