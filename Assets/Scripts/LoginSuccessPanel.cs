using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LoginSuccessPanel : MonoBehaviour
{
    private static LoginSuccessPanel instance;
    public static LoginSuccessPanel Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<LoginSuccessPanel>();
                if (instance == null)
                {
                    GameObject singletonObject = new GameObject();
                    instance = singletonObject.AddComponent<LoginSuccessPanel>();
                    singletonObject.name = typeof(LoginSuccessPanel).ToString() + " (Singleton)";
                }
            }
            return instance;
        }
    }

    TMP_Text successMessage;
    [SerializeField]Button logoutBtn;

    private void Start()
    {
        SetTextMSG();
        SetLogoutBtn();
    }

    void SetTextBOX()
    {
        successMessage = GetComponentInChildren<TMP_Text>();
    }

    void SetLogoutBtn()
    {
        logoutBtn = GetComponentInChildren<Button>();
        logoutBtn.onClick.AddListener(() =>
        {
            Logout();
        });
    }

    void Logout()
    {
        GameManager.Instance.LogOut();
    }

    public void SetTextMSG()
    {
        if(successMessage == null)
        {
            SetTextBOX();
        }
        if(LogInType.google == GameManager.Instance.GetLoginType())
        {
            successMessage.text = "Google LOGIN!";
        }
        else if(LogInType.email == GameManager.Instance.GetLoginType())
        {
            successMessage.text = "Email LOGIN!";
        }
        else if(LogInType.annonymous == GameManager.Instance.GetLoginType())
        {
            successMessage.text = "Annonymous LOGIN!";
        }
        else if(LogInType.apple == GameManager.Instance.GetLoginType())
        {
            successMessage.text = "Apple LOGIN!";
        }
        else
        {
            successMessage.text = "Unknown LOGIN!";
        }
    }
}
