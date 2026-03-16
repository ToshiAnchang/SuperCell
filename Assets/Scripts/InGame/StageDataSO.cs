using UnityEngine;

[CreateAssetMenu(fileName = "StageData", menuName = "ScriptableObjects/StageData")]
public class StageDataSO : ScriptableObject
{
    public int stageIndex;
    public int enemyCount;
    public float firstEnemyHP;
    public float hpIncreasePerEnemy;
    public GameObject enemyPrefab; // 이 스테이지에서 스폰할 에너미 프리팹
}
