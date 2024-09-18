using UnityEngine;

public class EnemyController : Sounds
{
    public LayerMask wallLayer;
    public GameObject bulletPrefab;
    public float speed = 1f;
    public float bulletSpeed = 4f;
    public float raycastLength = 4f;
    public float raycastDirection = 0.5f;
    public float shotCooldown = 0.0001f;

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private LineRenderer lineRenderer;
    private Rigidbody2D rb;
    private Material lineMaterial;
    private bool isLit = false;
    private float lastShotTime = 0f;

    void Start()
    {
        bool soundOn = PlayerPrefs.GetInt("SoundOn", 1) == 1;
        float volume = PlayerPrefs.GetFloat("Volume", 1f);
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.mute = !soundOn;
        audioSource.volume = volume;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        lineRenderer = GetComponent<LineRenderer>();
        lineMaterial = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.material = lineMaterial;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.startColor = new Color(1, 1, 1, 0.05f);
        lineRenderer.endColor = new Color(1, 1, 1, 0.05f);
    }

    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, raycastDirection, wallLayer);
        if (hit.collider != null)
        {
            TurnRight();
        }
        else
        {
            MoveForward();
        }
        isLit = Flashlight.Instance.IsLit(transform.position);
        if (isLit)
        {
            bool isVisible = IsVisibleToPlayer();
            if (isVisible)
            {
                spriteRenderer.enabled = true;
                lineRenderer.enabled = true; // Enable the line renderer when the enemy is lit
                int numRays = 100;
                float angleStep = 45f / (numRays - 1);
                lineRenderer.positionCount = numRays * 2;
                for (int i = 0; i < numRays; i++)
                {
                    float angle = -30f + i * angleStep; // угол текущего луча
                    Vector2 rayDirection = Quaternion.Euler(0, 0, angle) * transform.right; // направление текущего луча
                    RaycastHit2D hit1 = Physics2D.Raycast(transform.position, rayDirection, raycastLength, wallLayer);
                    if (hit1)
                    {
                        // рейкаст остановилс€ на стене
                        lineRenderer.SetPosition(i * 2, (transform.position + new Vector3(0f, 0f, 0.1f)));
                        lineRenderer.SetPosition(i * 2 + 1, hit1.point);
                    }
                    else
                    {
                        // рейкаст не остановилс€ на стене
                        lineRenderer.SetPosition(i * 2, (transform.position + new Vector3(0f, 0f, 0.1f)));
                        lineRenderer.SetPosition(i * 2 + 1, (Vector2)transform.position + rayDirection * raycastLength);
                    }
                }
            }
            else
            {
                spriteRenderer.enabled = false;
                lineRenderer.enabled = false;
            }
        }
        else
        {
            spriteRenderer.enabled = false;
            lineRenderer.enabled = false; // Disable the line renderer when the enemy is not lit
        }

        if (IsPlayerUnderFlashlight())
        {
            transform.right = Vector2.Lerp(transform.right, (Flashlight.Instance.transform.position - transform.position).normalized, 0.1f);
            if (CanShoot())
            {
                PlaySound(sounds[0]);
                ShootBullet();
            }
        }
    }

    // ‘ункци€ дл€ поворота направо
    void TurnRight()
    {
        transform.Rotate(0, 0, -90);
    }

    // ‘ункци€ дл€ движени€ вперед
    void MoveForward()
    {
        // ƒвигаем врага вперед с заданной скоростью
        rb.velocity = transform.right * speed;
    }

    bool CanShoot()
    {
        return Time.time - lastShotTime >= shotCooldown;
    }

    bool IsVisibleToPlayer()
    {
        // ѕолучаем позицию игрока
        Vector2 playerPosition = Flashlight.Instance.transform.position;

        // ѕровер€ем, есть ли между игроком и врагом стена
        RaycastHit2D hit = Physics2D.Raycast(playerPosition, (Vector2)transform.position - playerPosition, Vector2.Distance(playerPosition, transform.position), wallLayer);

        // ≈сли нет стены, то враг виден игроку
        return hit.collider == null;
    }

    bool IsPlayerUnderFlashlight()
    {
        // ѕолучаем позицию игрока
        Vector2 playerPosition = Flashlight.Instance.transform.position;

        // ѕровер€ем, находитс€ ли игрок в зоне действи€ фонарика врага
        float distanceToPlayer = Vector2.Distance(transform.position, playerPosition);
        float angleToPlayer = Vector2.Angle(transform.right, playerPosition - (Vector2)transform.position);

        // ѕровер€ем, есть ли между игроком и врагом стена
        RaycastHit2D hit = Physics2D.Raycast(transform.position, playerPosition - (Vector2)transform.position, distanceToPlayer, wallLayer);

        // ≈сли игрок находитс€ в зоне действи€ фонарика, угол между игроком и фонариком меньше 45 градусов и нет стены между ними, то игрок находитс€ под фонариком
        return distanceToPlayer < raycastLength && angleToPlayer < 45f && hit.collider == null;
    }

    public void Die()
    {
        spriteRenderer.enabled = true;
        lineRenderer.enabled = false;
        animator.SetBool("Hit", true);
        PlaySound(sounds[1]);
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
        GetComponent<CircleCollider2D>().enabled = false;
        enabled = false;
    }

    void ShootBullet()
    {
        Vector3 bulletPosition = transform.TransformPoint(new Vector3(3f, -1f, 0f));
        Vector2 bulletDirection = (Flashlight.Instance.transform.position - transform.position).normalized;
        GameObject bullet = Instantiate(bulletPrefab, bulletPosition, Quaternion.identity);
        Rigidbody2D bulletRigidbody = bullet.GetComponent<Rigidbody2D>();
        bulletRigidbody.velocity = bulletDirection * bulletSpeed;
        float angle = Mathf.Atan2(bulletDirection.y, bulletDirection.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(0f, 0f, angle + 90f);
        lastShotTime = Time.time;
    }
}