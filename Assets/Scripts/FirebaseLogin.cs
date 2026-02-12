using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using TMPro;

public class FirebaseLogin : MonoBehaviour
{
    [SerializeField] TMP_InputField emailInput;
    [SerializeField] TMP_InputField passwordInput;
    FirebaseAuth auth;

    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
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