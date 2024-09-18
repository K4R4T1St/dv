using System.Xml;
using UnityEngine;

public class itemScript : Sounds
{
    public LayerMask wallLayer;
    public bool isLit = false;

    private SpriteRenderer spriteRenderer;

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
        isLit = Flashlight.Instance.IsLitItems(transform.position) && IsVisibleToPlayer();
        spriteRenderer.enabled = isLit;
    }

    bool IsVisibleToPlayer()
    {
        // Получаем позицию фонарика
        Vector2 flashlightPosition = Flashlight.Instance.transform.position;

        // Проверяем, есть ли между фонариком и предметом стена
        Vector2 direction = ((Vector2)transform.position - flashlightPosition).normalized;
        float distance = Vector2.Distance(transform.position, flashlightPosition);
        RaycastHit2D hit = Physics2D.Raycast(flashlightPosition, direction, distance, wallLayer);

        // Если нет стены, то предмет виден игроку
        return hit.collider == null;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the player has collided with the item
        if (collision.gameObject.CompareTag("Player"))
        {
            PlaySound(sounds[0], volume: 1f, destroed: true);   
            PlayerControls playerControls = FindObjectOfType<PlayerControls>();
            playerControls.items++;
            Destroy(gameObject); // Disable the sprite renderer to make the item disappear
            // You can also add code here to add the item to the player's inventory, etc.
        }
    }
}