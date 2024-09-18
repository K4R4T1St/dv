using UnityEngine;

public class TrapScript : Sounds
{
    public LayerMask wallLayer;

    private SpriteRenderer spriteRenderer;
    private bool isLit = false;

    void Start()
    {
        bool soundOn = PlayerPrefs.GetInt("SoundOn", 1) == 1;
        float volume = PlayerPrefs.GetFloat("Volume", 1f);
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.mute = !soundOn;
        audioSource.volume = volume;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        isLit = Flashlight.Instance.IsLitTraps(transform.position);
        if (isLit)
        {
            if(IsVisibleToPlayer())
            {
                spriteRenderer.enabled = true;
            }
        }
        else
        {
            spriteRenderer.enabled = false;
        }
    }

    bool IsVisibleToPlayer()
    {
        Vector2 playerPosition = Flashlight.Instance.transform.position;
        Vector2 directionToPlayer = (playerPosition - (Vector2)transform.position).normalized;
        float angleToPlayer = Vector2.Angle(Flashlight.Instance.transform.right, directionToPlayer);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, Vector2.Distance(playerPosition, transform.position), wallLayer);
        return hit.collider == null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlaySound(sounds[0]);
            collision.gameObject.GetComponent<PlayerControls>().Die();
        }
    }
}
