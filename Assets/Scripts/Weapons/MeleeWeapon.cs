using System.Collections;
using UnityEngine;

public class MeleeWeapon : BasicWeapon
{
    public GameObject attackArea;
    public LayerMask enemyLayers;
    private BoxCollider2D attackCollider;
    private Animator leftArmAnimator;
    private Animator rightArmAnimator;

    private Animator animator;


    private void Start()
    {
        attackCollider = attackArea.GetComponent<BoxCollider2D>();
        //attackCollider.enabled = false; // Отключаем коллайдер по умолчанию

        //leftArmAnimator = transform.root.Find("Arms/Left Arm").GetComponent<Animator>();
        //rightArmAnimator = transform.root.Find("Arms/Right Arm").GetComponent<Animator>();

        // if (leftArmAnimator == null || rightArmAnimator == null)
        // {
        //     Debug.LogError("Animator not found on arms.");
        // }

        animator = GetComponent<Animator>();
    }

    public override void UseWeapon()
    {
        if (isCooldown) return;
        Attack();
        StartCoroutine(Cooldown());
    }

    private void Attack()
    {
        // leftArmAnimator.Play("Right-hand-attack");
        // rightArmAnimator.Play("Right-hand-attack");
        if (animator != null)
        {
            animator.SetTrigger("Attack"); // Запуск анимации атаки
        }
        StartCoroutine(CooldownAttack());
    }

    //КОСТЫЛЬ ДЛЯ УДАРА КУЛАКАМИ (ЧТОБЫ АТАКА БЫЛА ВО ВРЕМЯ УДАРА ИМЕННО)
    protected IEnumerator CooldownAttack()
    {
        yield return new WaitForSeconds(0.3f);

        Debug.Log("KNIFE ATTACK");
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
