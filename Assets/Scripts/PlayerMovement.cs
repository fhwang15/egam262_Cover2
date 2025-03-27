using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    //float variable for speed of shooting, moving around.
    //float original speed


    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        //if getkeydown x, then it will piu piu shoot the bullet.
        //Attack();

        //get Key up and down
        //verticalMove();

        //get key left & right
        //horizontalMove();

        //get key space
        //use the power drop if they exist.
    }

    //Player movement
    // Mostly Up and down, 

    private void verticalMove()
    {

    }

    private void horizontalMove() 
    { 

    }

    

    private void Attack()
    {


        //Coroutine for shooting.
        //StartCoroutine(shootingDelay());
    }

    IEnumerator shootingDelay()
    {
        yield return null;
    }
    //yield return WaitforSeconds(the variable);
    //
    //Instantiate(bulletPrefab, this.transform.position.x, this.transform.position.y);
    //Instantiate(bulletPrefab, this.transform.position.x, this.transform.position.y); 
    //two at a time
    //yield return null;

    //Collider that will destroy + get rid of the life.

}
