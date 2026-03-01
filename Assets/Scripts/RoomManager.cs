using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.EventSystems;

public class RoomManager : MonoBehaviour
{
    [Header("Chat Input Field")]
    [SerializeField] private TMP_InputField chatInputField;

    [Header("Chat UI")]
    [SerializeField] private Transform chatContentRoot;          // ChatContent
    [SerializeField] private TextMeshProUGUI chatMessagePrefab;  // ChatMessageText 프리팹
    [SerializeField] private int maxMessages = 50;               // 화면에 유지할 최대 메시지 수

    // 현재 화면에 떠 있는 메시지 오브젝트 리스트
    private readonly List<TextMeshProUGUI> messageUIList = new List<TextMeshProUGUI>();

    /// <summary>
    /// RoomManager는 PhotonView가 필요함!
    /// 반드시 컴포넌트에 PhotonView가 붙어있어야 함.
    /// </summary>
    private PhotonView photonView;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
        if (photonView == null)
        {
            photonView = gameObject.AddComponent<PhotonView>();
        }
    }

    private void Start()
    {
        if (chatInputField == null)
        {
            chatInputField = FindObjectOfType<TMP_InputField>();
        }

        if (chatInputField != null)
        {
            // 엔터(Submit) 처리 → 여기서 전부 처리
            chatInputField.onSubmit.AddListener(OnChatInputSubmit);

            // 혹시 포커스 유지 원하면 시작할 때 한 번 포커스
            chatInputField.ActivateInputField();
            chatInputField.Select();
        }
    }

    private void OnDestroy()
    {
        if (chatInputField != null)
        {
            chatInputField.onSubmit.RemoveListener(OnChatInputSubmit);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && chatInputField != null && chatInputField.isFocused)
        {
            chatInputField.text = "";
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    private void OnChatInputSubmit(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            // 공백이면 버리기
            chatInputField.text = "";
            chatInputField.ActivateInputField();
            return;
        }
        string sender = string.IsNullOrEmpty(PhotonNetwork.NickName)
            ? "TestUser"
            : PhotonNetwork.NickName;

        photonView.RPC("ReceiveChatMessage", RpcTarget.All, message, sender);

        chatInputField.text = "";
        chatInputField.ActivateInputField();
    }

    [PunRPC]
    private void ReceiveChatMessage(string message, string sender, PhotonMessageInfo info)
    {
        bool isMine = info.Sender == PhotonNetwork.LocalPlayer;

        AddChatLine(message, sender, isMine);
    }

    /// <summary>
    /// 채팅 한 줄을 인풋 위에 쌓이게 만드는 함수
    /// </summary>
    private void AddChatLine(string message, string sender, bool isMine)
    {
        if (chatContentRoot == null || chatMessagePrefab == null)
        {
            Debug.LogWarning("ChatContentRoot 또는 chatMessagePrefab 이 설정되지 않았습니다.");
            return;
        }
        // 프리팹 생성
        TextMeshProUGUI msg = Instantiate(chatMessagePrefab, chatContentRoot);

        RectTransform rt = msg.GetComponent<RectTransform>();

        if (isMine)
        {
            // 내 채팅 → 오른쪽 정렬
            msg.alignment = TextAlignmentOptions.TopRight;
            msg.text = message;

            rt.anchorMin = new Vector2(1f, rt.anchorMin.y);
            rt.anchorMax = new Vector2(1f, rt.anchorMax.y);
            rt.pivot     = new Vector2(1f, 0.5f);

            rt.anchoredPosition = new Vector2(0, rt.anchoredPosition.y);
        }
        else
        {
            // 상대 채팅 → 왼쪽 정렬 + 아이디 표시
            msg.alignment = TextAlignmentOptions.TopLeft;
            msg.text = $"{sender}: {message}";

            rt.anchorMin = new Vector2(0f, rt.anchorMin.y);
            rt.anchorMax = new Vector2(0f, rt.anchorMax.y);
            rt.pivot     = new Vector2(0f, 0.5f);

            rt.anchoredPosition = new Vector2(0, rt.anchoredPosition.y);
        }

        messageUIList.Add(msg);

        // 메시지 수가 너무 많으면 위에서부터 삭제
        if (messageUIList.Count > maxMessages)
        {
            Destroy(messageUIList[0].gameObject);
            messageUIList.RemoveAt(0);
        }
    }
}
