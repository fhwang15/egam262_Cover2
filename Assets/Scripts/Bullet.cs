using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Bullet : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        //If it collided to something, and if it has Enemy Script,
        Enemy enemyCollided = collision.gameObject.GetComponent<Enemy>();

        if (enemyCollided != null)
        {
            //If collides with Enemy and obstacle, destroySelf
            //if collides with Enemy, enemy also 
            Destroy(enemyCollided.gameObject);
            Destroy(this.gameObject);
        }
    }

}
