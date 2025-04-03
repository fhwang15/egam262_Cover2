using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PowerUpBar : MonoBehaviour
{
    public int powerUpItemCount = 0;
    public TextMeshProUGUI powerUpText;

    void Update()
    {
        powerUpText.text = $"Power Up: {powerUpItemCount}";
    }

    public void AddPowerUpItem()
    {
        powerUpItemCount++;
        if (powerUpItemCount > 6)
            powerUpItemCount = 6;  // �ִ� 6���� ����
    }

    public void ResetPowerUpItem()
    {
        powerUpItemCount = 0;
    }
}
