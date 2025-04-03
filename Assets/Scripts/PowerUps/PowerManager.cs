using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum BulletType
{
    Basic,
    Double,
    Missile,
    Laser
}

public enum PowerUpType
{
    Replace,
    Add,
    ChangeState
}

public class PowerManager : MonoBehaviour
{
    private PlayerMovement player;

    public GameObject missilePrefab;
    public GameObject doublePrefab;
    public GameObject laserPrefab;
    public GameObject optionPrefab;
    public GameObject shieldPrefab;

    private Dictionary<string, GameObject> powerUpPrefabs;
    private List<IPowerUps> activePowerUps;

    void Start()
    {
        player = GetComponent<PlayerMovement>();
        activePowerUps = new List<IPowerUps>();

        powerUpPrefabs = new Dictionary<string, GameObject>
        {
            { "Missile", missilePrefab },
            { "Double", doublePrefab },
            { "Laser", laserPrefab },
            { "Option", optionPrefab },
            { "Shield", shieldPrefab }
        };
        if (optionPrefab == null)
        {
            Debug.LogError("Option �������� PowerManager�� �������� �ʾҽ��ϴ�!");
        }
    }

    public void AddPowerUp(IPowerUps powerUp)
    {


        foreach (var activePowerUp in activePowerUps)
        {
            if (activePowerUp.GroupID == powerUp.GroupID)
            {
                // ���� �Ŀ��� ��Ȱ��ȭ
                activePowerUp.Deactivate(player);
                activePowerUps.Remove(activePowerUp);
                Debug.Log($"���� {activePowerUp.Name} �Ŀ��� ��Ȱ��ȭ (GroupID: {activePowerUp.GroupID})");
                break;
            }
        }

        if (!powerUp.canStack && HasPowerUp(powerUp.Name))
        {
            Debug.Log($"{powerUp.Name} �Ŀ����� �ߺ� �Ұ�!");
            return;
        }

        // �Ŀ��� Ȱ��ȭ
        powerUp.Activate(player);

        // �ߺ� ����: �̹� ����Ʈ�� ������ �߰����� ����
        if (!activePowerUps.Contains(powerUp))
        {
            activePowerUps.Add(powerUp);
            player.AddBulletType(powerUp);  
            Debug.Log($"{powerUp.Name} �Ŀ��� �߰���!");
        }
        else
        {
            Debug.Log($"{powerUp.Name} �Ŀ����� �̹� Ȱ��ȭ�Ǿ� �ֽ��ϴ�!");
        }
    }

    public void RemovePowerUp(IPowerUps powerUp)
    {
        if (activePowerUps.Contains(powerUp))
        {
            activePowerUps.Remove(powerUp);
            powerUp.Deactivate(player);
            Debug.Log($"{powerUp.Name} �Ŀ��� ��Ȱ��ȭ!");
        }
    }

    public void StackPowerUp(IPowerUps powerUp)
    {
        if (powerUp.canStack)
        {
            powerUp.StackUp(player);
        }
    }

    public void ClearAllPowerUps()
    {
        foreach (var powerUp in activePowerUps)
        {
            powerUp.Deactivate(player);
        }
        activePowerUps.Clear();
        Debug.Log("��� �Ŀ��� �ʱ�ȭ!");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public GameObject GetPowerUpPrefab(string powerUpName)
    {
        if (powerUpPrefabs.TryGetValue(powerUpName, out GameObject prefab))
        {
            return prefab;
        }
        return null;
    }

    
    public bool HasPowerUp(string powerUpName)
    {
        foreach (var powerUp in activePowerUps)
        {
            if (powerUp.Name == powerUpName)
                return true;
        }
        return false;
    }

    public int GetOptionCount()
    {
        int count = 0;
        foreach (var powerUp in activePowerUps)
        {
            if (powerUp.Name == "Option")
            {
                count++;
            }
        }
        return count;
    }


    ///////////////////////////////// PowerUps //////////////////////////////////////
    public class SpeedUp : IPowerUps
    {
        public string Name => "SpeedUp";
        public bool canStack => true;
        public int maxStack => 5;
        public int currentStackCount { get; set; }
        public int GroupID => 0;

        public PowerUpType type => PowerUpType.ChangeState;

        private float speedIncrease = 0.5f;

        public SpeedUp()
        {
            currentStackCount = 1;
        }

        public void Activate(PlayerMovement player)
        {
            ApplySpeed(player);
        }

        public void Deactivate(PlayerMovement player)
        {
            player.AdjustSpeed(1f);
        }

        public void StackUp(PlayerMovement player)
        {
            if (currentStackCount < maxStack)
            {
                currentStackCount++;
                ApplySpeed(player);
            }
            else
            {
                //Cannot apply speed! so cannot press space lol
            }
        }
        private void ApplySpeed(PlayerMovement player)
        {
            float newSpeed = 1f + (currentStackCount * speedIncrease);
            player.AdjustSpeed(newSpeed);
            Debug.Log($"Speed Up ����! ���� �ӵ� ����: {newSpeed}");
        }

        public void Shoot(Transform ShootTo)
        {
            //Has Nothing to do with shooting haha
        }

        public float GetShootingSpeed()
        {
            return 0f;
        }

        public float GetShootingDelay()
        {
            return 0f;
        }

        public BulletType GetBulletType()
        {
            return BulletType.Basic;
        }


    }

    public class Missile : IPowerUps
    {
        public string Name => "Missile";
        public bool canStack => false;
        public int maxStack => 1;
        public int currentStackCount { get; set; }
        public int GroupID => 1;

        public PowerUpType type => PowerUpType.Add;

        //Same as normal bullet tbh
        private GameObject missilePrefab;

        public Missile(GameObject prefab)
        {
            missilePrefab = prefab;
        }
        public void Activate(PlayerMovement player)
        {
            currentStackCount = 1;
        }

        public void Deactivate(PlayerMovement player)
        {

        }

        public void StackUp(PlayerMovement player)
        {
            //Cannot Stack Up
        }

        public void Shoot(Transform ShootTo)
        {
            GameObject shooooot = Object.Instantiate(missilePrefab, ShootTo.position, Quaternion.Euler(0, 0, -80));
            Rigidbody2D rb = shooooot.GetComponent<Rigidbody2D>();

            // �߻� ���� ����
            Vector2 direction = Quaternion.Euler(0, 0, -80) * Vector2.right;
            rb.AddForce(direction * GetShootingSpeed(), ForceMode2D.Impulse);

            Debug.Log($"Missile �߻�! ����: {-80}");
        }

        public float GetShootingSpeed()
        {
            return 7f;
        }

        public float GetShootingDelay()
        {
            return 0.1f;
        }

        public BulletType GetBulletType()
        {
            return BulletType.Basic;
        }

    }


    public class Double : IPowerUps
    {
        public string Name => "Double";
        public bool canStack => false;
        public int maxStack => 1;
        public int currentStackCount { get; set; }
        public int GroupID => 2;
    
        public PowerUpType type => PowerUpType.Add;

        //Same as normal bullet tbh
        private GameObject doublePrefab;

        public Double(GameObject prefab)
        {
            doublePrefab = prefab;
        }
        public void Activate(PlayerMovement player)
        {
            currentStackCount = 1;
        }

        public void Deactivate(PlayerMovement player)
        {
            currentStackCount = 0;
        }

        public void StackUp(PlayerMovement player)
        {
            //Cannot Stack Up ==> Have more than one double
        }

        public void Shoot(Transform ShootTo)
        {
            GameObject shooooot = Object.Instantiate(doublePrefab, ShootTo.position, Quaternion.Euler(0, 0, 45));
            Rigidbody2D rb = shooooot.GetComponent<Rigidbody2D>();

            Vector2 direction = Quaternion.Euler(0, 0, 45) * Vector2.right;
            rb.AddForce(direction * GetShootingSpeed(), ForceMode2D.Impulse);
        }

        public float GetShootingSpeed()
        {
            return 7f;
        }

        public float GetShootingDelay()
        {
            return 0.3f;
        }

        public BulletType GetBulletType()
        {
            return BulletType.Basic;
        }

    }

    public class Laser : IPowerUps
    {
        public string Name => "Laser";
        public bool canStack => false;
        public int maxStack => 1;
        public int currentStackCount { get; set; }
        public int GroupID => 2;

        public PowerUpType type => PowerUpType.Replace;

        //won't destruct when hit the enemy
        private GameObject LaserPrefab;

        public Laser(GameObject prefab)
        {
            LaserPrefab = prefab;
        }
        public void Activate(PlayerMovement player)
        {
            player.AddBulletType(this);
            player.SetBulletType(BulletType.Laser);
        }

        public void Deactivate(PlayerMovement player)
        {
            player.SetBulletType(BulletType.Basic);
            player.RemoveBulletType(this);
        }

        public void StackUp(PlayerMovement player)
        {
            //Cannot Stack Up!
        }

        public void Shoot(Transform ShootTo)
        {
            GameObject shooooot = Instantiate<GameObject>(LaserPrefab, ShootTo.position, Quaternion.identity);
            Rigidbody2D rb = shooooot.GetComponent<Rigidbody2D>();
            rb.AddForce(Vector2.right * GetShootingSpeed(), ForceMode2D.Impulse);
        }

        public float GetShootingSpeed()
        {
            return 15f;
        }

        public float GetShootingDelay()
        {
            return 0.1f;
        }

        public BulletType GetBulletType()
        {
            return BulletType.Laser;
        }

    }

    public class Shield : IPowerUps
    {
        public string Name => "Shield";
        public bool canStack => false;
        public int maxStack => 1;
        public int currentStackCount { get; set; }
        public int GroupID => 4;

        public PowerUpType type => PowerUpType.Add;

        public Shield(GameObject prefab)
        {
            
        }
        public void Activate(PlayerMovement player)
        {

        }

        public void Deactivate(PlayerMovement player)
        {

        }

        public void StackUp(PlayerMovement player)
        {

        }

        public void Shoot(Transform ShootTo)
        {

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
            return BulletType.Basic;
        }

    }


}
