using UnityEngine;

public class EnemyBox : EnemyBody
{
    public override void KillBody()
    {
        base.KillBody();
        SkillUpgradeCanvas.Instance.gameObject.SetActive(true);        
    }
}