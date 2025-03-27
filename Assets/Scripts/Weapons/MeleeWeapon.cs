using System.Collections;
using State_Machine;
using UnityEngine;

public class MeleeWeapon : BasicWeapon
{
    // public GameObject attackArea;
    // public LayerMask enemyLayers;
    // public BoxCollider2D attackCollider;
    // public SpriteRenderer knifeSprite;
    // public State attackKnifeState;

    // private void Start()
    // {
    //     attackCollider = attackArea.GetComponent<BoxCollider2D>();
    //     // Убедитесь, что вы назначаете AttackKnifeState через инспектор или в другом месте кода
    // }

    // public override void UseWeapon()
    // {
    //     if (isCooldown || attackKnifeState == null) return;
    //     attackKnifeState.Enter(); // Вход в состояние атаки
    //     StartCoroutine(Cooldown());
    // }

    public GameObject attackArea;
    public LayerMask enemyLayers;
    private BoxCollider2D attackCollider;

    public SpriteRenderer knifeSprite;

    [Header("Animation")]
    public State anim;


    private void Start()
    {
        attackCollider = attackArea.GetComponent<BoxCollider2D>();

        if (anim != null)
        {
            anim.SetCore(transform.root.GetComponent<Core>());
        }
    }

    public override void UseWeapon()
    {
        if (isCooldown) return;
        Attack();
        StartCoroutine(Cooldown());
    }

    private void Attack()
    {

    }


    //КОСТЫЛЬ ДЛЯ УДАРА КУЛАКАМИ (ЧТОБЫ АТАКА БЫЛА ВО ВРЕМЯ УДАРА ИМЕННО)
    protected IEnumerator CooldownAttack()
    {
        yield return new WaitForSeconds(0.1f);

        attackCollider.enabled = true;

        // Получаем всех врагов в зоне действия
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackCollider.bounds.center, attackCollider.bounds.size, 0f, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<HealthController>()?.TakeDamage(damage, 0);
        }

        // Отключаем коллайдер после проверки
        attackCollider.enabled = false;
    }
}
