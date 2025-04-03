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
    private Transform playerTransform;
    void Start()
    {
        if (player == null)
        {
            player = FindObjectOfType<PlayerMovement>();
            if (player != null)
                Debug.Log("Option에서 PlayerMovement 찾음!");
            else
                Debug.LogError("Option에서 PlayerMovement를 찾지 못함!");
        }
    }

    public void SetPlayerTransform(Transform player)
    {
        if (player != null)
        {
            playerTransform = player;
            Debug.Log("Option이 Player Transform을 성공적으로 받았습니다!");
        }
        else
        {
            Debug.LogError("SetPlayerTransform: 전달된 Player Transform이 null입니다!");
        }
    }

    void Update()
    {
        if (playerTransform != null)
        {
            Vector3 offset = new Vector3(0, -0.5f, 0);  // 플레이어 위치 아래쪽으로
            transform.position = playerTransform.position + offset;
        }
        else
        {
            Debug.LogError("Option이 Player Transform을 찾지 못했습니다!");
        }
    }


    public void Initialize(PlayerMovement player)
    {
        this.player = player;
        if (player == null)
        {
            Debug.LogError("PlayerMovement가 null입니다!");
        }
        else
        {
            Debug.Log("PlayerMovement가 정상적으로 할당되었습니다!");
        }
    }

    public void Activate(PlayerMovement player)
    {
        this.player = player;
        if (player == null)
        {
            Debug.LogError("Option 활성화 중 PlayerMovement가 없습니다!");
            return;
        }

        // PowerUps를 다시 가져옴 (동기화 문제 해결)
        List<IPowerUps> playerPowerUps = player.GetActivePowerUps();
        if (playerPowerUps == null || playerPowerUps.Count == 0)
        {
            Debug.LogWarning("Option 활성화 중 Player의 PowerUps 리스트가 비어 있습니다!");
        }
        else
        {
            Debug.Log("Option 활성화: Player의 PowerUps 리스트 가져옴");
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
            Debug.Log($"Option 추가됨! 현재 수: {currentStackCount}");
        }
    }

    private void CreateOption()
    {
        Vector3 offset = new Vector3(0, -0.5f * currentStackCount, 0);
        GameObject option = Instantiate(gameObject, player.transform.position + offset, Quaternion.identity);

        if (option == null)
        {
            Debug.LogError("Option 오브젝트 생성에 실패했습니다!");
            return;
        }

        OptionMotion motion = option.AddComponent<OptionMotion>();
        if (motion == null)
        {
            Debug.LogError("OptionMotion 컴포넌트 추가에 실패했습니다!");
            return;
        }

        motion.Initialize(player.transform);
        option.tag = "Option";
        activeOptions.Add(option);
        currentStackCount++;
        Debug.Log("Option 생성됨!");
    }

    public void Shoot(Transform ShootTo)
    {
        // Option 발사 호출 로그
        Debug.Log("Option 발사 호출");

        // PlayerMovement가 정상적으로 할당되었는지 확인
        if (player == null)
        {
            Debug.LogError("Option에서 PlayerMovement를 찾을 수 없습니다!");
            return;
        }

        // Player의 PowerUps 가져오기
        List<IPowerUps> playerPowerUps = player.GetActivePowerUps();
        if (playerPowerUps == null || playerPowerUps.Count == 0)
        {
            Debug.LogError("Player의 PowerUps 리스트가 비어 있습니다!");
            return;
        }

        foreach (GameObject option in activeOptions)
        {
            foreach (IPowerUps powerUp in playerPowerUps)
            {
                if (powerUp.Name != "Option")  // Option 자체의 발사는 방지
                {
                    Debug.Log($"Option에서 Bullet 발사! 파워업: {powerUp.Name}");

                    // Bullet 발사 로직
                    powerUp.Shoot(option.transform);
                    Debug.Log($"Bullet 발사 완료! 위치: {option.transform.position}");
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