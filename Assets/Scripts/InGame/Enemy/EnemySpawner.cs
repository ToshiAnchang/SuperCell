using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public EnemyController controller;
    public GameObject defaultEnemyPrefab; // SO에 프리팹이 없을 경우 대비

    public void StartStage(StageDataSO stageData)
    {
        if (controller == null) return;

        // 기존 대열 청소
        controller.enemyList.Clear();

        GameObject prefab = stageData.enemyPrefab != null ? stageData.enemyPrefab : defaultEnemyPrefab;

        for (int i = 0; i < stageData.enemyCount; i++)
        {
            // 리더 위치(웨이포인트 0번)에서 생성
            GameObject go = Instantiate(prefab, controller.wayPoints[0].position, Quaternion.identity);
            Enemy enemy = go.GetComponent<Enemy>();
            enemy.transform.SetParent(controller.transform); // 계층 구조 정리

            if (enemy != null)
            {
                // 기존 Enemy 변수에 HP 직접 할당
                float calculatedHP = stageData.firstEnemyHP + (i * stageData.hpIncreasePerEnemy);
                enemy.maxHealth = calculatedHP;
                enemy.currentHealth = calculatedHP;

                // 생성 시점에 대열 간격 미리 벌려주기 (겹침 방지)
                enemy.targetDistance = -(i * controller.spacing);
                enemy.currentDistance = enemy.targetDistance;

                // Controller의 리스트에 추가 (이 순간부터 Controller.Update가 작동함)
                controller.enemyList.Add(enemy);
            }
        }
    }
}
