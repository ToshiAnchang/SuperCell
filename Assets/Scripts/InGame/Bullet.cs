using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public float damage = 10f;

    void Update()
    {
        // 생성될 때 정해진 forward 방향으로 계속 이동
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            Debug.Log($"Bullet hit enemy with {enemy.currentHealth} HP");
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
