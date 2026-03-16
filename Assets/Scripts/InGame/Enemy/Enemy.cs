using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float currentHealth;
    public float maxHealth;

    [HideInInspector] public float currentDistance = 0f;
    [HideInInspector] public float targetDistance = 0f;

    public bool headEnemy = false;

    public void UpdateUnit(Vector3 newPos, Vector3 lookAtPos, float moveStep)
    {
        // 등속 이동으로 목표 거리 추적
        currentDistance = Mathf.MoveTowards(currentDistance, targetDistance, moveStep);

        transform.position = newPos;
        if (Vector3.Distance(newPos, lookAtPos) > 0.01f)
        {
          //  transform.LookAt(lookAtPos);
        }
    }

    public void TakeDamage(float damage)
    {
        if (headEnemy) return;
        currentHealth -= damage;
        if (currentHealth <= 0) Die();
    }

    void Die()
    {
        EnemyController controller = FindFirstObjectByType<EnemyController>();
        if (controller != null) controller.OnEnemyDeath(this);
    }
}
