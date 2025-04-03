using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    //float variable for speed of shooting, moving around.
    //float original speed

    public float changeInSpeed;
    public float originShootingSpeed;
    public float originSpeed;

    public Bullet bulletPrefab;

    public Transform firePoint;
    private List<IPowerUps> activePowerUps;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        activePowerUps = new List<IPowerUps>();
    }

    // Update is called once per frame
    void Update()
    {
        //Basic Movement of the PlayerCharacter (Not in physics)
        Move();

        //Attack
        if (Input.GetKeyDown(KeyCode.X))
        {
            StartCoroutine(Attack());
        }

        //get key space
        //use the power drop if they exist.
    }

    private void Move()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(moveX, moveY, 0);
        transform.Translate(move * originSpeed * changeInSpeed * Time.deltaTime);

    }

    public void AdjustSpeed(float multiplier)
    {
        changeInSpeed = multiplier;
    }

    public void AddBulletType(IPowerUps powerUps)
    {
        if (!activePowerUps.Contains(powerUps))
        {
            activePowerUps.Add(powerUps);
        }
    }

    public void RemoveBulletType(IPowerUps type)
    {
        if (activePowerUps.Contains(type))
        {
            activePowerUps.Remove(type);
        }
    }

    private IEnumerator Attack()
    {
        bool hasLaser =false;

        foreach(IPowerUps powerUp in activePowerUps)
        {
            if (powerUp.Name == "Laser")
            {
                hasLaser = true;
                powerUp.Shoot(firePoint);
                float delay = powerUp.GetShootingDelay();
                yield return new WaitForSeconds(delay);
            }
            yield break;
        }

        if (!hasLaser)
        {
            if (activePowerUps.Count == 0)
            {
                yield return StartCoroutine(shootingDelay());
                yield break;
            }

            foreach (IPowerUps powerUp in activePowerUps)
            {
                powerUp.Shoot(firePoint);
                float delay = powerUp.GetShootingDelay();
                yield return new WaitForSeconds(delay);
            }
        }
    }

    IEnumerator shootingDelay()
    {
        Bullet shooooot = Instantiate<Bullet>(bulletPrefab, transform.position, Quaternion.identity); 
        Rigidbody2D rb = shooooot.GetComponent<Rigidbody2D>();
        rb.AddForce(Vector2.right * originShootingSpeed, ForceMode2D.Impulse);

        yield return new WaitForSeconds(originShootingSpeed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Enemy enemyCollided = collision.gameObject.GetComponent<Enemy>();
        if (enemyCollided != null)
        {
            Destroy(this.gameObject);
        }
    }
}
