using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    public static PhotonManager Instance { get; private set; }

    [SerializeField] private string gameVersion = "1.0";
    [SerializeField] private byte maxPlayersPerRoom = 2;
    [SerializeField] private string roomSceneName = "3.RoomScene";

    bool isConnecting = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("PhotonManager 중복 인스턴스가 발견되어 제거됩니다.");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = gameVersion;
    }

    public void ConnectAndJoinRandomRoom()
    {
        if (isConnecting)
        {
            Debug.Log("[PhotonManager] 이미 접속 시도 중입니다. 요청을 무시합니다.");
            return;
        }

        isConnecting = true;
        Debug.Log("[PhotonManager] 포톤 서버 접속 및 랜덤 룸 입장을 시도합니다.");

        if (PhotonNetwork.IsConnected)
        {
            Debug.Log("[PhotonManager] 이미 포톤에 접속 중이므로 바로 랜덤 룸 입장을 시도합니다.");
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            Debug.Log("[PhotonManager] 포톤에 미접속 상태입니다. ConnectUsingSettings() 호출.");
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        if (isConnecting)
        {
            Debug.Log("[PhotonManager] 마스터 서버에 접속되었습니다. 로비 입장을 시도합니다.");
            PhotonNetwork.JoinLobby();
        }
    }

    public override void OnJoinedLobby()
    {
        if (isConnecting)
        {
            Debug.Log("[PhotonManager] 로비에 입장했습니다. 랜덤 룸 입장을 시도합니다.");
            PhotonNetwork.JoinRandomRoom();
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.LogWarning($"[PhotonManager] 랜덤 룸 입장 실패 (code: {returnCode}, message: {message}). 새 룸을 생성합니다.");
        RoomOptions roomOptions = new RoomOptions { MaxPlayers = maxPlayersPerRoom };
        PhotonNetwork.CreateRoom(null, roomOptions);
    }

    public override void OnJoinedRoom()
    {
        isConnecting = false;
        Debug.Log($"[PhotonManager] 룸에 입장했습니다. 씬 '{roomSceneName}'을(를) 로드합니다. 현재 플레이어 수: {PhotonNetwork.CurrentRoom.PlayerCount}");
        PhotonNetwork.LoadLevel(roomSceneName);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        isConnecting = false;
        Debug.LogWarning($"Photon disconnected: {cause}");
    }
}
