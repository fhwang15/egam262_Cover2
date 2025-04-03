using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using static PowerManager;
using System.Net;

public class PlayerMovement : MonoBehaviour
{
    //float variable for speed of shooting, moving around.
    //float original speed
    public float changeInSpeed;
    public float originShootingSpeed;
    public float originSpeed;

    //Prefabs in general
    public Bullet bulletPrefab;

    public Transform firePoint;

    public PowerUpBar powerUpBar;
    private PowerManager powerUpManager;

    private List<IPowerUps> activePowerUps;
    public BulletType currentBulletType = BulletType.Basic;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        activePowerUps = new List<IPowerUps>();
        powerUpManager = GetComponent<PowerManager>();
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
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            UsePowerUp();
        }

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
        bool hasLaser = false;
        bool hasDouble = false;

        // Laser와 Double을 구분하여 처리
        foreach (IPowerUps powerUp in activePowerUps)
        {
            if (powerUp.Name == "Laser")
            {
                hasLaser = true;
                powerUp.Shoot(firePoint);
                float delay = powerUp.GetShootingDelay();
                yield return new WaitForSeconds(delay);
            }
            else if (powerUp.Name == "Double")
            {
                hasDouble = true;
            }
        }

        // 기본 Bullet 발사 (Laser가 없을 때만)
        if (!hasLaser)
        {
            if (hasDouble)
            {
                foreach (IPowerUps powerUp in activePowerUps)
                {
                    if (powerUp.Name == "Double")
                    {
                        powerUp.Shoot(firePoint);
                        float delay = powerUp.GetShootingDelay();
                        yield return new WaitForSeconds(delay);
                    }
                }
            }
            StartCoroutine(shootingDelay());
        }


        // Option도 같이 발사
        foreach (GameObject option in GameObject.FindGameObjectsWithTag("Option"))
        {
            Option optionComponent = option.GetComponent<Option>();
            if (optionComponent != null)
            {
                Debug.Log("Option에서 발사 시도!");
                optionComponent.Shoot(firePoint);  // firePoint를 인자로 넘김
            }
            else
            {
                Debug.LogError("Option 컴포넌트를 찾을 수 없음!");
            }
        }
    }
    IEnumerator shootingDelay()
    {

        GameObject shooooot;

        if (currentBulletType == BulletType.Laser)
        {
            shooooot = Instantiate(powerUpManager.GetPowerUpPrefab("Laser"), firePoint.position, Quaternion.identity);
        }
        else
        {
            shooooot = Instantiate(bulletPrefab.gameObject, firePoint.position, Quaternion.identity);
        }

        Rigidbody2D rb = shooooot.GetComponent<Rigidbody2D>();
        rb.AddForce(Vector2.right * originShootingSpeed, ForceMode2D.Impulse);

        yield return new WaitForSeconds(originShootingSpeed);
    }


    public void UsePowerUp()
    {
        int count = powerUpBar.powerUpItemCount;

        IPowerUps selectedPowerUp = null;

        switch (count)
        {
            case 1:
                selectedPowerUp = new SpeedUp();
                break;
            case 2:
                selectedPowerUp = new Missile(powerUpManager.GetPowerUpPrefab("Missile"));
                break;
            case 3:
                selectedPowerUp = new Double(powerUpManager.GetPowerUpPrefab("Double"));
                break;
            case 4:
                selectedPowerUp = new Laser(powerUpManager.GetPowerUpPrefab("Laser"));
                break;
            case 5:
                GameObject optionPrefab = powerUpManager.GetPowerUpPrefab("Option");

                // 프리팹이 null인지 확인
                if (optionPrefab == null)
                {
                    Debug.LogError("Option 프리팹을 찾을 수 없습니다!");
                    return;
                }

                GameObject optionObject = Instantiate(optionPrefab, transform.position, Quaternion.identity);
                Option optionComponent = optionObject.GetComponent<Option>();

                if (optionComponent != null)
                {
                    optionComponent.Initialize(this);
                    Debug.Log("Option 컴포넌트가 정상적으로 추가되었습니다!");
                    selectedPowerUp = optionComponent;
                }
                else
                {
                    Debug.LogError("생성된 Option 오브젝트에 Option 컴포넌트가 없습니다!");
                }
                break;
            case 6:
                selectedPowerUp = new Shield(powerUpManager.GetPowerUpPrefab("Shield"));
                break;
            default:
                Debug.Log("파워업 없음!");
                return;

        }

        if (selectedPowerUp != null)
        {
            // 먼저 PowerUp을 추가하고 나서 Option을 활성화
            powerUpManager.AddPowerUp(selectedPowerUp);

            // Option인 경우에는 활성화하면서 Player의 PowerUps를 다시 가져옴
            if (selectedPowerUp is Option optionPowerUp)
            {
                optionPowerUp.Activate(this);
                Debug.Log("Option 활성화 완료!");
            }

            powerUpBar.ResetPowerUpItem();
            Debug.Log($"{selectedPowerUp.Name} 파워업 사용!");
        }
    }

    public void SetBulletType(BulletType newType)
    {
        currentBulletType = newType;
    }

    public List<IPowerUps> GetActivePowerUps()
    {
        return activePowerUps;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Enemy enemyCollided = collision.gameObject.GetComponent<Enemy>();
        Bullet bullet = collision.gameObject.GetComponent<Bullet>();

        if (enemyCollided != null)
        {
            Destroy(this.gameObject);
        }

        if (bullet != null)
        {
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
        }

    }
}
