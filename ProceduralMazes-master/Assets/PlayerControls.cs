using TMPro;
using UnityEngine;
using System.Collections;

public class PlayerControls : Sounds
{ 
    public AudioSource audioSource1;
    public GameObject menuWin;
    public GameObject menuLose;
    public GameObject bulletPrefab;
    public TextMeshProUGUI textAmmo;
    public TextMeshProUGUI textItems;
    public float Speed = 4f;
    public float LaserLength = 20f;
    public float bulletSpeed = 200f;
    public int ammo = 20;
    public int items;

    private Rigidbody2D componentRigidbody;
    private Animator animator;
    private bool soundOn;
    private bool isDead = false;

    private void Start()
    {
        soundOn = PlayerPrefs.GetInt("SoundOn", 1) == 1;
        float volume = PlayerPrefs.GetFloat("Volume", 0.25f);
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.mute = !soundOn;
        audioSource.volume = volume;
        audioSource1.mute = !soundOn;
        audioSource1.volume = volume + 0.1f;
        menuWin.SetActive(false);
        menuLose.SetActive(false);
        animator = GetComponent<Animator>();
        componentRigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        animator.SetBool("1", false);
        textItems.text = items.ToString();
        textAmmo.text = ammo.ToString();
        audioSource1.mute = true;
        componentRigidbody.velocity = Vector2.zero;
        if (Input.GetKey(KeyCode.S))
        {
            if (soundOn)
            {
                audioSource1.mute = false;
            }
            animator.SetBool("1", true);
            componentRigidbody.velocity += Vector2.left * Speed;
        }
        if (Input.GetKey(KeyCode.F))
        {
            if (soundOn)
            {
                audioSource1.mute = false;
            }
            animator.SetBool("1", true);
            componentRigidbody.velocity += Vector2.right * Speed;
        }
        if (Input.GetKey(KeyCode.E))
        {
            if (soundOn)
            {
                audioSource1.mute = false;
            }
            animator.SetBool("1", true);
            componentRigidbody.velocity += Vector2.up * Speed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            if (soundOn)
            {
                audioSource1.mute = false;
            }
            animator.SetBool("1", true);
            componentRigidbody.velocity += Vector2.down * Speed;
        }

        if (Input.GetMouseButtonDown(0) && ammo!= 0)
        {
            ammo--;
            PlaySound(sounds[1]);
            ShootLaser();
        }
    }
    private void ShootLaser()
    {
        Vector3 bulletPosition = transform.TransformPoint(new Vector3(0.9f, -3f, 0f));
        Vector2 laserDirection = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        GameObject bullet = Instantiate(bulletPrefab, bulletPosition, Quaternion.identity);
        Rigidbody2D bulletRigidbody = bullet.GetComponent<Rigidbody2D>();
        bulletRigidbody.velocity = laserDirection * bulletSpeed;
        float angle = Mathf.Atan2(laserDirection.y, laserDirection.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    public void Die()
    {
        if (!isDead)
        {
            isDead = true;
            animator.Play("Die");
            PlaySound(sounds[2]);
            componentRigidbody.velocity = Vector2.zero;
            StartCoroutine(ShowMenu(menuLose, 0.5f));
            componentRigidbody.isKinematic = true;
            componentRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
            GetComponent<FaceFollowCursor2D>().enabled = false;
            GetComponent<Flashlight>().enabled = false;
            enabled = false;
        }
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Exit"))
        {
            if (items == 3)
            {
                StartCoroutine(ShowMenu(menuWin, 0.5f)); // Add a 2-second delay
                PlaySound(sounds[3]);
            }
            else
            {
                StartCoroutine(ShowMenu(menuLose, 0.5f)); // Add a 2-second delay
                PlaySound(sounds[2]);
            }
        }
    }

    private IEnumerator ShowMenu(GameObject menu, float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.GetComponent<CircleCollider2D>().enabled = false;
        menu.SetActive(true);
    }
}