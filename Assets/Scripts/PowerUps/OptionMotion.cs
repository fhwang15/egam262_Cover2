using UnityEngine;

public class OptionMotion : MonoBehaviour
{
    private Transform playerTransform;

    public void Initialize(Transform playerTransform)
    {
        this.playerTransform = playerTransform;
    }

    void Update()
    {
        if (playerTransform != null)
        {
            Vector3 offset = new Vector3(0, -0.5f, 0);  // �÷��̾� ��ġ �Ʒ�������
            transform.position = playerTransform.position + offset;
        }
    }
}
