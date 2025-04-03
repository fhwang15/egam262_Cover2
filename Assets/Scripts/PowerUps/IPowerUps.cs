using UnityEngine;

public interface IPowerUps
{
    //Activate the powerUp
    void Activate(PlayerMovement player);
    //Deactivate PowerUp
    void Deactivate(PlayerMovement player);
    void StackUp(PlayerMovement player);
    void Shoot(Transform shootTo);

    float GetShootingSpeed();
    float GetShootingDelay();


    public string Name { get; }
    //See if this can overlap or not.
    bool canStack { get; }
    //Max amount of stacks it can get
    int maxStack { get; }
    //Current Stack
    int currentStackCount { get; set; }
    int GroupID { get; }
    PowerUpType type { get; }
    BulletType GetBulletType();



}
