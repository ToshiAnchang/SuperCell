using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{

    [HideInInspector] public float currentDistance = 0f;
    [HideInInspector] public float targetDistance = 0f;

    public bool headEnemy = false;
    public bool hpTextEnemy = false;
    [SerializeField] EnemyGroup enemyGroup;
    [SerializeField] TMPro.TextMeshProUGUI hpText;
    

    public void UpdateUnit(Vector3 newPos, Vector3 lookAtPos, float moveStep)
    {
        // 등속 이동으로 목표 거리 추적
        currentDistance = Mathf.MoveTowards(currentDistance, targetDistance, moveStep);

        transform.position = newPos;
        if (Vector3.Distance(newPos, lookAtPos) > 0.01f)
        {
          //  transform.LookAt(lookAtPos);
          //  LookAt은 안해요. 애니메이션이 이상해져요.
        }
        if (hpTextEnemy)
        {
            hpText.text = enemyGroup.currentHp.ToString("F0");
        }
    }

    public void TakeDamage(float damage)
    {
        if (headEnemy) { Debug.Log("Enemy.cs : Head 에너미는 맞질 않아요."); return; }
        
        enemyGroup.TakeDamage(damage);
    }

    public EnemyGroup GetEnemyGroup()
    {
        return enemyGroup;
    }

    public void SetEnemyGroup(EnemyGroup eg)
    {
        enemyGroup = eg;
    }

    public void SetHpText(bool tf)
    {
        if (tf)
        {
            hpTextEnemy = true;
        }
        else
        {
            hpTextEnemy = false;
            hpText.gameObject.SetActive(false);
        }
    }

    public void Die()
    {
        EnemyController controller = FindFirstObjectByType<EnemyController>();
        if (controller != null) controller.OnEnemyDeath(this);
    }
}
