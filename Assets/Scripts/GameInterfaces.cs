public interface IDamageable
{
    void TakeDamage(int damage, float knockbackForce);
}

public interface ICollectible
{
    void Collect();
}
