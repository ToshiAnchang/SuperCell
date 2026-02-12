using Firebase;
using Firebase.Auth;
using Google;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class FirebaseLogin : MonoBehaviour
{
    [SerializeField] TMP_InputField emailInput;
    [SerializeField] TMP_InputField passwordInput;
    FirebaseAuth auth;

    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
    }

    string webClientId = "888241259450-t3l4om4plrhibnkd31t7ovh81a4j0r6j.apps.googleusercontent.com";
    private GoogleSignInConfiguration configuration;
    private void Awake()
    {
        configuration = new GoogleSignInConfiguration { WebClientId = webClientId, RequestEmail = true, RequestIdToken = true };
    }

    //구글 로그인
    public void SignInWithGoogle() //로그인 버튼에 등록 
    {
        Debug.Log("구글 로그인 시도");

        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;

        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnAuthenticationFinished);
    }

    //파이어베이스 로그인
    void OnAuthenticationFinished(Task<GoogleSignInUser> task)
    {
        if (task.IsFaulted)
        {
            Debug.Log("구글 로그인 실패");
        }
        else if (task.IsCanceled)
        {
            Debug.Log("구글 로그인 취소");
        }
        else
        {
            Debug.Log("Welcome: " + task.Result.DisplayName);
            Debug.Log("Email : " + task.Result.Email);
            Debug.Log("Google ID Token : " + task.Result.IdToken);

            SignInWithGoogleOnFirebase(task.Result.IdToken);
        }
    }

    //파이어베이스 로그인
    private void SignInWithGoogleOnFirebase(string idToken)
    {
        Credential credential = GoogleAuthProvider.GetCredential(idToken, null);

        auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
        {
            AggregateException ex = task.Exception;
            if (ex != null)
            {
                if (ex.InnerExceptions[0] is FirebaseException inner && (inner.ErrorCode != 0))
                    Debug.Log("\nError code = " + inner.ErrorCode + " Message = " + inner.Message);
            }
            else
            {
                Debug.Log("Sign In Successful.");
                //구글 로그인 성공. 여기서 씬전환을 바로 시켜주면 안됨. 왜냐하면 통신 쓰레드에서 씬전환을 시도하기 때문
                //메인 쓰레드에서 씬전환을 시켜줘야함. 결국 여기서는 로그인 성공 여부만 알려주고, 메인 쓰레드에서 씬전환을 시켜줘야함
                //예: 로그인 성공 플래그를 세우고, Update()에서 씬전환 처리 <<< 추천방식
                //예시 isLoginSuccessful = true;
                //또는 이벤트 시스템을 이용해서 메인 쓰레드에서 처리하게 할 수도 있음
                //이벤트 system.TriggerEvent("OnLoginSuccess");
                //이벤트 리스너가 메인 쓰레드에서 씬전환 처리 
            }
        });
    }

    //가입 버튼에 등록 함수
    public void OnClickedJoin()
    {
        string email = emailInput.text;
        string password = passwordInput.text;

        auth.CreateUserWithEmailAndPasswordAsync(email, password)
                        .ContinueWith(task => {
                            if (task.IsCanceled)
                            {
                                Debug.LogError("가입 취소");
                                return;
                            }
                            if (task.IsFaulted)
                            {
                                Debug.LogError("가입 실패 : " + task.Exception);
                                return;
                            }

                            // Firebase user has been created.
                            AuthResult result = task.Result;
                            Debug.LogFormat("가입 성공: {0} ({1})",
                                result.User.DisplayName, result.User.UserId);
                        });
    }
    //로그인 버튼에 등록 함수
    public void OnClickedLogin()
    {
        string email = emailInput.text;
        string password = passwordInput.text;

        auth.SignInWithEmailAndPasswordAsync(email, password)
                        .ContinueWith(task => {
                            if (task.IsCanceled)
                            {
                                Debug.LogError("로그인 취소됌");
                                return;
                            }
                            if (task.IsFaulted)
                            {
                                Debug.LogError("로그인 실패 : " + task.Exception);
                                return;
                            }

                            AuthResult result = task.Result;
                            Debug.LogFormat("로그인 성공 : {0} ({1})",
                                result.User.DisplayName, result.User.UserId);
                        });
    }

}
