using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("공격 설정")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 0.5f;
    private float nextFireTime = 0f;

    private Vector3 lastLookDirection = Vector3.forward;

    void Update()
    {
        // 1. 마우스 클릭 시 방향 업데이트
        if (Input.GetMouseButtonDown(0))
        {
            UpdateRotationToMouse();
        }

        // 2. 발사 타이밍 체크 (클릭하고 있을 때 발사되게 하려면 GetMouseButton(0) 사용)
        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void UpdateRotationToMouse()
    {
        // 마우스 위치를 월드 좌표로 변환하기 위해 레이캐스트 사용
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastPlane(ray);
    }

    void RaycastPlane(Ray ray)
    {
        // 플레이어의 높이(Y축)를 기준으로 평면을 만들어 정확한 지점 계산
        Plane playerPlane = new Plane(Vector3.up, transform.position);
        float hitDist = 0.0f;

        if (playerPlane.Raycast(ray, out hitDist))
        {
            Vector3 targetPoint = ray.GetPoint(hitDist);
            lastLookDirection = (targetPoint - transform.position).normalized;

            // 플레이어 몸체를 클릭 방향으로 회전 (Y축만 회전)
            if (lastLookDirection != Vector3.zero)
            {
                transform.forward = new Vector3(lastLookDirection.x, 0, lastLookDirection.z);
            }
        }
    }

    void Shoot()
    {
        if (bulletPrefab == null || firePoint == null) return;

        // 마우스 방향을 업데이트한 후 발사
        UpdateRotationToMouse();

        // 총알 생성 및 방향 설정
        GameObject bulletGo = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        bulletGo.transform.forward = lastLookDirection;
    }
}
