using System.Collections;
using System.Collections.Generic;
using Firebase.Auth;
using Google;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance 
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
                if (instance == null)
                {
                    GameObject singletonObject = new GameObject();
                    instance = singletonObject.AddComponent<GameManager>();
                    singletonObject.name = typeof(GameManager).ToString() + " (Singleton)";
                }
            }
            return instance;
        }
    }

    [SerializeField]bool isLogin = false;
    [SerializeField]LogInType logInType = LogInType.none;
    [SerializeField] string userId = "";
    [SerializeField] string userEmail = "";

    FirebaseAuth auth;

    private void Awake()
    {
        // 싱글톤 중복 방지 로직 (필수)
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);

            // [중요] 여기서 auth를 초기화해야 합니다!
            auth = FirebaseAuth.DefaultInstance;

            // 인증 상태 변경 이벤트 연결
            auth.StateChanged += OnAuthStateChanged;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Update()
    {
        if(isLogin)
        {
            SceneManager.LoadScene("StartScene");
            Debug.Log("로그인 성공! 씬 전환 처리");
            isLogin = false; // 로그인 플래그 초기화
        }
    }

    public void SetUserLoginStatus(bool status, LogInType type)
    {
        Debug.Log($"로그인 상태 업데이트: {status}, 로그인 타입: {type}");
        isLogin = status;
        logInType = type;
    }

    public bool IsUserLoggedIn()
    {
        return isLogin;
    }

    public LogInType GetLoginType()
    {
        return logInType;
    }

    public void LogOut()
    {
        // 한 번 더 방어적으로 체크
        if (auth == null) auth = FirebaseAuth.DefaultInstance;

        if (auth != null)
        {
            auth.SignOut();
            if (GoogleSignIn.DefaultInstance != null)
            {
                GoogleSignIn.DefaultInstance.SignOut();
            }

            Debug.Log("SignOut 메서드 호출됨");
        }
    }

    // Firebase 인증 상태가 변경될 때 호출되는 콜백 함수
    void OnAuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser == null)
        {
            // 유저가 null이라는 것은 로그아웃이 완벽히 되었다는 뜻입니다.
            isLogin = false;
            logInType = LogInType.none;
            Debug.Log("Firebase로부터 확인됨: 로그아웃 완료");
            SceneManager.LoadScene("LoginScene"); // 로그아웃 후 로그인 씬으로 이동
        }
        else
        {
            string providerId = "";
            foreach (var profile in auth.CurrentUser.ProviderData)
            {
                providerId = profile.ProviderId;
                // 하나라도 일치하는 게 있으면 루프를 나갑니다.
                if (providerId == "google.com" || providerId == "apple.com" || providerId == "password")
                    break;
            }
            if (auth.CurrentUser == null)
            {
                return; // 안전하게 null 체크
            }
            if (auth.CurrentUser.IsAnonymous)
            {
                logInType = LogInType.annonymous;
            }
            else if (auth.CurrentUser.Email != null && providerId == "password")
            //pasord는 이메일 로그인에서 사용되는 providerId입니다. 그래서 메일 주소가 google.com 이나 apple.com 이더라도
            //providerId가 password로 나오는 경우에는 이메일로 가입한 사용자입니다. 순수하게 구글로그인 api나 애플로그인 api를
            //사용해서 가입한 사용자는 providerId가 google.com이나 apple.com만 있고 password는 없습니다.
            //그래서 providerId가 password인 경우에는 이메일 로그인 사용자로 생각하면 됩니다.
            {
                logInType = LogInType.email;
            }            
            else if (auth.CurrentUser.Email != null && providerId == "google.com")
            {
                logInType = LogInType.google;
            }
            else if (auth.CurrentUser.Email != null && providerId == "apple.com")
            {
                logInType = LogInType.apple;
            }
            Debug.Log($"Firebase로부터 확인됨: 로그인 완료, 로그인 타입: {logInType} | {providerId}");
            userId = auth.CurrentUser.UserId;
            userEmail = auth.CurrentUser.Email;
            SetUserLoginStatus(true, logInType);         
        }
    }

    // 스크립트가 파괴될 때 이벤트 연결을 해제해주는 것이 메모리 관리에 좋습니다.
    void OnDestroy()
    {
        if (auth != null)
        {
            auth.StateChanged -= OnAuthStateChanged;
        }
    }
}

public enum LogInType
{
    none, annonymous, email, google, apple
}
