using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

public class CSVToSO : Editor
{
    [MenuItem("Tools/CSV를 Resources에 저장")]
    public static void CreateSO()
    {
        string csvPath = Application.dataPath + "/StageData.csv";
        if (!File.Exists(csvPath)) return;

        string directoryPath = Application.dataPath + "/Resources/StageData";
        if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);

        // 엑셀이 열려있어도 읽을 수 있도록 설정
        List<string> lines = new List<string>();
        using (var stream = new FileStream(csvPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        using (var reader = new StreamReader(stream))
        {
            while (!reader.EndOfStream) lines.Add(reader.ReadLine());
        }

        // 첫 번째 줄(제목) 제외하고 실제 데이터가 있는 줄부터 반복
        // image_f8d2d6.png 기준: 1번 인덱스부터 끝까지
        for (int i = 1; i < lines.Count; i++)
        {
            string line = lines[i];
            if (string.IsNullOrWhiteSpace(line)) continue;

            // 쉼표로 나누고 각 칸의 특수문자/공백 제거
            string[] row = line.Split(',')
                               .Select(cell => Regex.Replace(cell, @"[^0-9.-]", "").Trim())
                               .ToArray();

            // 최소 4개의 숫자가 들어있는 행만 처리
            if (row.Length >= 4 && !string.IsNullOrEmpty(row[0]))
            {
                try
                {
                    StageDataSO so = ScriptableObject.CreateInstance<StageDataSO>();

                    so.stageIndex = int.Parse(row[0]);
                    so.enemyCount = int.Parse(row[1]);
                    so.firstEnemyHP = float.Parse(row[2]);
                    so.hpIncreasePerEnemy = float.Parse(row[3]);

                    string savePath = $"Assets/Resources/StageData/Stage_{so.stageIndex}.asset";
                    AssetDatabase.CreateAsset(so, savePath);
                    Debug.Log($"<color=white>[생성 성공]</color> {savePath}");
                }
                catch (System.Exception e)
                {
                    Debug.LogWarning($"{i + 1}번째 줄 데이터 파싱 실패: {e.Message}");
                }
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("<color=green><b>모든 스테이지 SO 생성 작업이 완료되었습니다!</b></color>");
    }
}
