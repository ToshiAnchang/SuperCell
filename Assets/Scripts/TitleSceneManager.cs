using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class TitleSceneManager : MonoBehaviour
{
    [SerializeField] Image loadGageImage;
    [SerializeField] TextMeshProUGUI loadingText;

    private void Awake()
    {
        if (loadGageImage != null)
        {
            loadGageImage.fillAmount = 0f;
            StartCoroutine(FillLoadGageAndMoveScene());
            loadingText.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("loadGageImage가 에디터에서 할당되지 않았습니다.");
        }
    }

    private IEnumerator FillLoadGageAndMoveScene()
    {
        float duration = 3f;
        float elapsed = 0f; 

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            if (loadGageImage != null)
            {
                loadGageImage.fillAmount = Mathf.Clamp01(elapsed / duration);
            }
            yield return null;
        }
        if (loadGageImage != null)
        {
            loadGageImage.fillAmount = 1f;
        }
        if (loadingText != null)
        {
            loadingText.text = "Loading Complete!";
            loadingText.gameObject.SetActive(true);
        }
        yield return new WaitForSeconds(1f);
        UnityEngine.SceneManagement.SceneManager.LoadScene("1.LoginScene");
    }
}
