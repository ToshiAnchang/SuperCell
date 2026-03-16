using UnityEngine;

public class StageManager : MonoBehaviour
{
    public int currentStage = 1;
    public EnemySpawner spawner;

    void Start()
    {
        LoadAndStartStage();
    }

    public void LoadAndStartStage()
    {
        // Resources 폴더에서 해당 번호의 SO 로드
        StageDataSO data = Resources.Load<StageDataSO>($"StageData/Stage_{currentStage}");

        if (data != null)
        {
            spawner.StartStage(data);
        }
        else
        {
            Debug.Log("모든 스테이지를 클리어했거나 파일을 찾을 수 없습니다.");
        }
    }

    // EnemyController에서 모든 적이 사라지면 호출할 함수
    public void NextStage()
    {
        currentStage++;
        LoadAndStartStage();
    }
}
