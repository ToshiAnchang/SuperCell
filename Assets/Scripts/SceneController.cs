using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void OnClickGameStart()
    {
        if (PhotonManager.Instance != null)
        {
            Debug.Log("PhotonManager 인스턴스를 찾았습니다. 방 접속 시도 #1/2");
            PhotonManager.Instance.ConnectAndJoinRandomRoom();
            Debug.Log("PhotonManager 인스턴스를 찾았습니다. 방 접속 시도 #2/2");
        }
        else
        {
            Debug.LogError("PhotonManager 인스턴스를 찾을 수 없습니다. 씬에 PhotonManager가 있는지 확인하세요.");
        }
    }
}