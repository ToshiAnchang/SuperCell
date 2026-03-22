using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class EnemyController : MonoBehaviour
{
    [Header("경로 설정")]
    public Transform pathParent;
    public Transform[] wayPoints;

    [Header("대열 설정")]
    public List<Enemy> enemyList = new List<Enemy>();
    public float spacing = 1.2f;
    public float moveSpeed = 2f;
    public float fillGapSpeed = 5f;

    private bool isFillingGap = false;
    private float totalPathLength = 0f;

    private static EnemyController instance;
    public static EnemyController Instance { get {
        if (instance == null)
        {
            instance = FindObjectOfType<EnemyController>();
        }
        return instance;
    }
    }

    void Awake()
    {
        SortWayPoints();
        CalculatePathLength();
    }

    [ContextMenu("경로 자동 정렬")]
    void SortWayPoints()
    {
        if (pathParent == null) return;
        wayPoints = pathParent.GetComponentsInChildren<WayPoint>()
            .OrderBy(wp => int.Parse(wp.name))
            .Select(wp => wp.transform).ToArray();
    }

    void CalculatePathLength()
    {
        totalPathLength = 0;
        for (int i = 0; i < wayPoints.Length - 1; i++)
            totalPathLength += Vector3.Distance(wayPoints[i].position, wayPoints[i + 1].position);
    }

    void Update()
    {
        if (enemyList.Count == 0) return;

        if (isFillingGap)
        {
            CheckGapFilled();
        }
        else
        {
            // 리더 이동 및 거리 제한(Clamp)
            float nextDist = enemyList[0].targetDistance + (moveSpeed * Time.deltaTime);
            enemyList[0].targetDistance = Mathf.Min(nextDist, totalPathLength);

            // 나머지 대열 간격 유지
            for (int i = 1; i < enemyList.Count; i++)
                enemyList[i].targetDistance = enemyList[0].targetDistance - (i * spacing);
        }

        // 실제 이동 실행
        float step = (isFillingGap ? fillGapSpeed : moveSpeed * 5f) * Time.deltaTime;
        foreach (Enemy e in enemyList)
        {
            e.UpdateUnit(GetPositionOnPath(e.currentDistance),
                         GetPositionOnPath(e.currentDistance + 0.1f), step);
            if (e.currentDistance >= totalPathLength)
            {
                Debug.Log("EnemyController.cs : 적이 경로 끝에 도달했습니다.");             
            }
        }        
    }

    void CheckGapFilled()
    {
        bool allArrived = true;
        foreach (Enemy e in enemyList)
        {
            if (Mathf.Abs(e.currentDistance - e.targetDistance) > 0.01f) { allArrived = false; break; }
        }
        if (allArrived) isFillingGap = false;
    }

    public void OnEnemyDeath(Enemy deadEnemy)
    {
        int index = enemyList.IndexOf(deadEnemy);
        if (index == -1) return;

        // 리더를 뒤로 밀어 전체 대열 재정렬 유도
        enemyList[0].targetDistance -= spacing;
        enemyList.RemoveAt(index);
        Destroy(deadEnemy.gameObject);

        for (int i = 0; i < enemyList.Count; i++)
            enemyList[i].targetDistance = enemyList[0].targetDistance - (i * spacing);

        isFillingGap = true;
    }

    public Vector3 GetPositionOnPath(float dist)
    {
        if (wayPoints.Length < 2) return Vector3.zero;
        if (dist <= 0) return wayPoints[0].position;

        float acc = 0;
        for (int i = 0; i < wayPoints.Length - 1; i++)
        {
            float seg = Vector3.Distance(wayPoints[i].position, wayPoints[i + 1].position);
            if (acc + seg >= dist) return Vector3.Lerp(wayPoints[i].position, wayPoints[i + 1].position, (dist - acc) / seg);
            acc += seg;
        }
        return wayPoints[wayPoints.Length - 1].position;
    }

    public void SetHeadEnemyUnit()
    {
        if (enemyList.Count > 0)
        {
                enemyList[0].headEnemy = true;
        }
    }
}
