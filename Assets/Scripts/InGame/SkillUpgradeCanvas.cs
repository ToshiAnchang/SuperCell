using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillUpgradeCanvas : MonoBehaviour
{

    [SerializeField] GameObject skillUpgradePanel;
    
    private static SkillUpgradeCanvas instance;
    public static SkillUpgradeCanvas Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SkillUpgradeCanvas>(true);
            }
            return instance;
        }
    }
}
