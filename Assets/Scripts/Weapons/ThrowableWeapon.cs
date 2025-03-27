using UnityEngine;

public class ThrowableWeapon : BasicWeapon
{
    public GameObject projectilePrefab;
    public Transform throwPoint;
    public float throwForce;

    public override void UseWeapon()
    {
        if (isCooldown) return;

        Throw();
        StartCoroutine(Cooldown());
    }

    private void Throw()
    {
        Instantiate(projectilePrefab, throwPoint.position, throwPoint.rotation);
        
        //Rigidbody2D rb = throwable.GetComponent<Rigidbody2D>();
        //rb.AddForce(throwPoint.up * throwForce, ForceMode2D.Impulse);
    }
}
