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
            // ��ư Ŭ�� �̺�Ʈ�� �޼��� ���
            restartButton.onClick.AddListener(RestartScene);
        }
        else
        {
            Debug.LogError("Restart ��ư�� �Ҵ���� �ʾҽ��ϴ�!");
        }
    }

    public void RestartScene()
    {
        // ���� Ȱ��ȭ�� ���� �ٽ� �ε��Ͽ� �ʱ�ȭ
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
        Debug.Log("�� �ʱ�ȭ �Ϸ�!");
    }
}
