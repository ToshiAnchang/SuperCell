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

        EnemyGroup enemyGroup = null;

        for (int i = 0; i < stageData.enemyCount * 3; i++)
        {
            if (i == 0)
            {
                enemyGroup = new EnemyGroup();
                if (enemyGroup != null)
                {
                    enemyGroup.SetBodyType(BodyType.head);
                }

                // 리더 위치(웨이포인트 0번)에서 생성
                GameObject go = Instantiate(stageData.enemyHeadPrefab, controller.wayPoints[0].position, Quaternion.identity);
                Enemy enemy = go.GetComponent<Enemy>();
                enemy.transform.SetParent(controller.transform); // 계층 구조 정리
                enemyGroup.AddEnemy(enemy); // 그룹에 적 추가
                enemy.SetEnemyGroup(enemyGroup); // 적에게 그룹 정보 전달

                if (enemy != null)
                {
                    // 생성 시점에 대열 간격 미리 벌려주기 (겹침 방지)
                    enemy.targetDistance = -(i * controller.spacing);
                    enemy.currentDistance = enemy.targetDistance;

                    // Controller의 리스트에 추가 (이 순간부터 Controller.Update가 작동함)
                    controller.enemyList.Add(enemy);
                }
            }

            else
            {
                int groupIndex = i - 1 / 3;
                if ((i - 1) % 3 == 0)
                {
                    enemyGroup = new EnemyGroup();
                    if (enemyGroup != null)
                    {
                        enemyGroup.maxHp = stageData.firstEnemyHP + (groupIndex * stageData.hpIncreasePerEnemy);
                        enemyGroup.currentHp = enemyGroup.maxHp;
                        enemyGroup.SetBodyType(BodyType.body);
                    }
                }

                // 리더 위치(웨이포인트 0번)에서 생성
                GameObject go = Instantiate(prefab, controller.wayPoints[0].position, Quaternion.identity);
                Enemy enemy = go.GetComponent<Enemy>();
                enemy.transform.SetParent(controller.transform); // 계층 구조 정리
                if ((i - 1) % 3 == 1)
                {
                    enemy.SetHpText(true);
                }
                else
                {
                    enemy.SetHpText(false);
                }
                enemyGroup.AddEnemy(enemy); // 그룹에 적 추가
                enemy.SetEnemyGroup(enemyGroup); // 적에게 그룹 정보 전달

                if (enemy != null)
                {
                    // 생성 시점에 대열 간격 미리 벌려주기 (겹침 방지)
                    enemy.targetDistance = -(i * controller.spacing);
                    enemy.currentDistance = enemy.targetDistance;

                    // Controller의 리스트에 추가 (이 순간부터 Controller.Update가 작동함)
                    controller.enemyList.Add(enemy);
                }
            }
        }
    }
}
