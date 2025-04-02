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

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Basic Movement of the PlayerCharacter (Not in physics)
        Vector3 movement = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);
        transform.Translate(movement.normalized * (originSpeed + changeInSpeed) * Time.deltaTime);

        //Attack
        if (Input.GetKeyDown(KeyCode.X))
        {
            Attack();
        }


        //get key space
        //use the power drop if they exist.
    }
    

    private void Attack()
    {
        StartCoroutine(shootingDelay());
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
