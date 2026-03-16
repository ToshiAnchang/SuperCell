using UnityEngine;

public class EnemyBody : MonoBehaviour
{
    float maxHp;
    [SerializeField] float currentHp;
    [SerializeField] float formatHp;
    [SerializeField] BodyType bodyType;
    Transform tailPos;


    public void TakeDamage(float damage)
    {
        if (bodyType == BodyType.head) return;
        currentHp -= damage;
        if(currentHp <= 0)
        {
            KillBody();
        }
    }

    public virtual void KillBody()
    {     
        Destroy(gameObject);
    }

    public void SetBody(int c, BodyType bType)
    {
        currentHp = (c+1) * formatHp;
        bodyType = bType;
    }

}

public enum BodyType
{
    head, body, normalBox, rareBox, epicBox
}
