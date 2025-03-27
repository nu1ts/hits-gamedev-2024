using System.Collections;
using UnityEngine;

public class PlayerDashController : MonoBehaviour
{
    public DashBarUI dashBarUI; // Ссылка на DashBarUI
    public float dashCooldown = 1f; // Перезарядка рывка
    public float dashSpeed = 60f; // Скорость рывка
    public float dashDuration = 0.2f; // Длительность рывка
    public float timeScaleDuringDash = 0.5f; // Замедление времени во время рывка

    public float ghostInterval = 0.1f; // Интервал появления "призраков"
    public float ghostLifetime = 0.5f; // Время жизни "призрака"
    public float initialGhostAlpha = 0.1f;

    private float currentCooldown = 0f;
    public bool _isDashing = false;
    private bool _canDash = true;
    private Vector2 _dashDirection;
    private SpriteRenderer headSpriteRenderer;
    private Rigidbody2D rb;

    public GameObject head;

    private void Start()
    {
        // Находим компонент Rigidbody2D на родительском объекте
        rb = GetComponentInParent<Rigidbody2D>();

        // Находим голову
        headSpriteRenderer = head.GetComponent<SpriteRenderer>();

        if (dashBarUI != null)
        {
            dashBarUI.Initialize(dashCooldown);
        }
    }

    private void Update()
    {
        if (currentCooldown > 0)
        {
            currentCooldown -= Time.deltaTime;
            if (dashBarUI != null)
            {
                dashBarUI.UpdateCooldown(currentCooldown);
            }
        }
    }

    public bool CanDash()
    {
        return currentCooldown <= 0 && _canDash;
    }

    public void StartDash(Vector2 dashDirection)
    {
        if (CanDash())
        {
            _dashDirection = dashDirection;
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        _isDashing = true;
        _canDash = false;
        currentCooldown = dashCooldown;

        // Замедление времени
        // Time.timeScale = timeScaleDuringDash;
        // Time.fixedDeltaTime = Time.timeScale * 0.02f;

        StartCoroutine(CreateGhosts());

        rb.velocity = _dashDirection * dashSpeed;

        yield return new WaitForSeconds(dashDuration);

        // Восстановление времени
        // Time.timeScale = 1f;
        // Time.fixedDeltaTime = 0.02f;

        rb.velocity = Vector2.zero;
        _isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        _canDash = true;
    }

    private IEnumerator CreateGhosts()
    {
        float elapsed = 0f;
        float ghostAlpha = initialGhostAlpha;

        while (_isDashing)
        {
            CreateGhost(ghostAlpha);
            yield return new WaitForSeconds(ghostInterval);
            elapsed += ghostInterval;

            if (elapsed >= dashDuration)
            {
                break;
            }

            ghostAlpha += 0.2f;
        }
    }

    private void CreateGhost(float ghostAlpha)
    {
        GameObject ghost = new GameObject("Ghost");
        SpriteRenderer ghostRenderer = ghost.AddComponent<SpriteRenderer>();

        ghostRenderer.sprite = headSpriteRenderer.sprite;
        ghostRenderer.flipX = headSpriteRenderer.flipX;
        ghostRenderer.flipY = headSpriteRenderer.flipY;
        ghostRenderer.sortingLayerID = headSpriteRenderer.sortingLayerID;
        ghostRenderer.sortingOrder = headSpriteRenderer.sortingOrder;

        ghost.transform.position = headSpriteRenderer.transform.position;
        ghost.transform.rotation = headSpriteRenderer.transform.rotation;
        ghost.transform.localScale = headSpriteRenderer.transform.localScale;

        Color ghostColor = headSpriteRenderer.color;
        ghostColor.a = ghostAlpha; // Полупрозрачность
        ghostRenderer.color = ghostColor;

        Destroy(ghost, ghostLifetime);
    }
}
