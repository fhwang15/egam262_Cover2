using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManage : MonoBehaviour
{

    public Button restartButton;

    void Start()
    {
        if (restartButton != null)
        {
            // 버튼 클릭 이벤트에 메서드 등록
            restartButton.onClick.AddListener(RestartScene);
        }
        else
        {
            Debug.LogError("Restart 버튼이 할당되지 않았습니다!");
        }
    }

    public void RestartScene()
    {
        // 현재 활성화된 씬을 다시 로드하여 초기화
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
        Debug.Log("씬 초기화 완료!");
    }
}
