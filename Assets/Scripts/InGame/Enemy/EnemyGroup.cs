using System.Collections.Generic;
using UnityEngine;

public class EnemyGroup
{
    [SerializeField] public float currentHp;
    [SerializeField] public float maxHp;
    [SerializeField] BodyType bodyType;
    List<Enemy> enemies;

    public void TakeDamage(float damage)
    {
        if (bodyType == BodyType.head)
        {
            Debug.Log("EnemyGroup.cs : Head 에너미는 맞질 않아요.");
            return;
        }
        currentHp -= damage;
        Debug.Log($"EnemyGroup took {damage} damage, current HP: {currentHp}");
        if (currentHp <= 0)
        {
            currentHp = 0;
             // 그룹 전체 사망 처리
             for(int i = 0; i < enemies.Count; i++)
            {
                if(enemies[i] != null)
                {
                    enemies[i].Die();
                }
            }
        }
    }

    public void AddEnemy(Enemy enemy)
    {
        if (enemies == null) enemies = new List<Enemy>();
        enemies.Add(enemy);
    }   

    public void SetBodyType(BodyType type)
    {
        bodyType = type;
    }
}

public enum BodyType
{
    head, body, normalBox, rareBox, epicBox
}
