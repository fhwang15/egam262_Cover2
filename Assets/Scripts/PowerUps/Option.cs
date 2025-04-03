using UnityEngine;
using System.Collections.Generic;

public class Option : MonoBehaviour, IPowerUps
{
    public string Name => "Option";
    public bool canStack => true;
    public int maxStack => 3;
    public int currentStackCount { get; set; }
    public int GroupID => 3;

    public PowerUpType type => PowerUpType.Add;

    private GameObject optionPrefab;
    private List<GameObject> activeOptions = new List<GameObject>();
    private PlayerMovement player;


    void Start()
    {
        if (player == null)
        {
            player = FindObjectOfType<PlayerMovement>();
            if (player != null)
                Debug.Log("Option���� PlayerMovement ã��!");
            else
                Debug.LogError("Option���� PlayerMovement�� ã�� ����!");
        }
    }

    public void Initialize(PlayerMovement player)
    {
        this.player = player;
        if (player == null)
        {
            Debug.LogError("PlayerMovement�� null�Դϴ�!");
        }
        else
        {
            Debug.Log("PlayerMovement�� ���������� �Ҵ�Ǿ����ϴ�!");
        }
    }

    public void Activate(PlayerMovement player)
    {
        this.player = player;
        if (player == null)
        {
            Debug.LogError("Option Ȱ��ȭ �� PlayerMovement�� �����ϴ�!");
            return;
        }

        // PowerUps�� �ٽ� ������ (����ȭ ���� �ذ�)
        List<IPowerUps> playerPowerUps = player.GetActivePowerUps();
        if (playerPowerUps == null || playerPowerUps.Count == 0)
        {
            Debug.LogWarning("Option Ȱ��ȭ �� Player�� PowerUps ����Ʈ�� ��� �ֽ��ϴ�!");
        }
        else
        {
            Debug.Log("Option Ȱ��ȭ: Player�� PowerUps ����Ʈ ������");
        }

        CreateOption();
    }

    public void Deactivate(PlayerMovement player)
    {
        foreach (GameObject option in activeOptions)
        {
            Destroy(option);
        }
        activeOptions.Clear();
        currentStackCount = 0;
    }

    public void StackUp(PlayerMovement player)
    {
        if (currentStackCount < maxStack)
        {
            CreateOption();
            Debug.Log($"Option �߰���! ���� ��: {currentStackCount}");
        }
    }

    private void CreateOption()
    {
        if (optionPrefab == null)
        {
            Debug.LogError("Option �������� �������� �ʾҽ��ϴ�!");
            return;
        }

        Vector3 offset = new Vector3(0, -0.5f * currentStackCount, 0);
        GameObject option = Instantiate(optionPrefab, player.transform.position + offset, Quaternion.identity);

        if (option == null)
        {
            Debug.LogError("Option ������Ʈ ������ �����߽��ϴ�!");
            return;
        }

        OptionMotion motion = option.AddComponent<OptionMotion>();
        if (motion == null)
        {
            Debug.LogError("OptionMotion ������Ʈ �߰��� �����߽��ϴ�!");
            return;
        }

        motion.Initialize(player.transform);
        option.tag = "Option";
        activeOptions.Add(option);
        currentStackCount++;
        Debug.Log("Option ������!");
    }

    public void Shoot(Transform ShootTo)
    {
        // Option �߻� ȣ�� �α�
        Debug.Log("Option �߻� ȣ��");

        // PlayerMovement�� ���������� �Ҵ�Ǿ����� Ȯ��
        if (player == null)
        {
            Debug.LogError("Option���� PlayerMovement�� ã�� �� �����ϴ�!");
            return;
        }

        // Player�� PowerUps ��������
        List<IPowerUps> playerPowerUps = player.GetActivePowerUps();
        if (playerPowerUps == null || playerPowerUps.Count == 0)
        {
            Debug.LogError("Player�� PowerUps ����Ʈ�� ��� �ֽ��ϴ�!");
            return;
        }

        foreach (GameObject option in activeOptions)
        {
            foreach (IPowerUps powerUp in playerPowerUps)
            {
                if (powerUp.Name != "Option")  // Option ��ü�� �߻�� ����
                {
                    Debug.Log($"Option���� Bullet �߻�! �Ŀ���: {powerUp.Name}");

                    // Bullet �߻� ����
                    powerUp.Shoot(option.transform);
                    Debug.Log($"Bullet �߻� �Ϸ�! ��ġ: {option.transform.position}");
                }
            }
        }
    }

    public float GetShootingSpeed()
    {
        return 0;
    }

    public float GetShootingDelay()
    {
        return 0;
    }

    public BulletType GetBulletType()
    {
        return player.currentBulletType;
    }
}