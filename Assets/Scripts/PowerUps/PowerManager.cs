using UnityEngine;

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


    void Start()
    {
        player = GetComponent<PlayerMovement>();
    }

    public void AddPowerUp(IPowerUps powerUp)
    {
        powerUp.Activate(player);
    }

    public void RemovePowerUp(IPowerUps powerUp)
    {
        powerUp.Deactivate(player);
    }


    // Update is called once per frame
    void Update()
    {

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
        private Bullet missilePrefab;

        public Missile(Bullet prefab)
        {
            missilePrefab = prefab;
        }
        public void Activate(PlayerMovement player)
        {

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
            Bullet shooooot = Instantiate<Bullet>(missilePrefab, ShootTo.position, Quaternion.Euler(0, 0, -45));
            Rigidbody2D rb = missilePrefab.GetComponent<Rigidbody2D>();
            rb.AddForce(Vector2.right * GetShootingSpeed(), ForceMode2D.Impulse);
        }

        public float GetShootingSpeed()
        {
            return 7f;
        }

        public float GetShootingDelay()
        {
            return 0.1f;
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
        private Bullet doublePrefab;

        public Double(Bullet prefab)
        {
            doublePrefab = prefab;
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
            //Cannot Stack Up ==> Have more than one double
        }

        public void Shoot(Transform ShootTo)
        {
            Bullet shooooot = Instantiate<Bullet>(doublePrefab, ShootTo.position, Quaternion.Euler(0, 0, -45));
            Rigidbody2D rb = doublePrefab.GetComponent<Rigidbody2D>();
            rb.AddForce(Vector2.right * GetShootingSpeed(), ForceMode2D.Impulse);
        }

        public float GetShootingSpeed()
        {
            return 7f;
        }

        public float GetShootingDelay()
        {
            return 0.3f;
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
        private Bullet LaserPrefab;

        public Laser(Bullet prefab)
        {
            LaserPrefab = prefab;
        }
        public void Activate(PlayerMovement player)
        {
            player.AddBulletType(this);
        }

        public void Deactivate(PlayerMovement player)
        {
            player.RemoveBulletType(this);
        }

        public void StackUp(PlayerMovement player)
        {
            //Cannot Stack Up!
        }

        public void Shoot(Transform ShootTo)
        {
            Bullet shooooot = Instantiate<Bullet>(LaserPrefab, ShootTo.position, Quaternion.identity);
            Rigidbody2D rb = shooooot.GetComponent<Rigidbody2D>();
            rb.AddForce(Vector2.right * GetShootingSpeed(), ForceMode2D.Impulse);
            Debug.Log("Laser น฿ป็!");
        }

        public float GetShootingSpeed()
        {
            return 15f;
        }

        public float GetShootingDelay()
        {
            return 0.1f;
        }


    }

    public class Option : IPowerUps
    {
        public string Name => "Option";
        public bool canStack => true;
        public int maxStack => 3;
        public int currentStackCount { get; set; }
        public int GroupID => 3;

        public PowerUpType type => PowerUpType.Add;

        //Cute companion friend
        private GameObject optionPrefab;

        public Option(GameObject prefab)
        {
            optionPrefab = prefab;
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
            return 1;
        }

        public float GetShootingDelay()
        {
            return 1;
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
            return 1;
        }

        public float GetShootingDelay()
        {
            return 1;
        }

    }

}
