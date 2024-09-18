using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public LayerMask wallLayer; // слой стен
    public LayerMask enemyLayer;
    public LayerMask trapLayer;
    public float raycastDistance = 4f; // расстояние рейкаста
    public float raycastInterval = 0.05f; // интервал между рейкастами

    private float nextRaycastTime = 0f;
    private LineRenderer lineRenderer;
    public static Flashlight Instance { get; private set; } // статическая переменная, хранящая ссылку на экземпляр фонарика

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        lineRenderer = gameObject.GetComponent<LineRenderer>();
    }

    void Update()
    {
        // отправляем рейкаст из позиции игрока в направлении взгляда игрока
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - (Vector2)transform.position).normalized;

        // создаем линию рейкаста
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.startColor = new Color(1, 1, 1, 0.05f);
        lineRenderer.endColor = new Color(1, 1, 1, 0.05f);

        // создаем несколько лучей с разными углами
        int numRays = 100; // количество лучей
        float angleStep = 60f / (numRays - 1); // шаг угла между лучами
        lineRenderer.positionCount = numRays * 2; // количество точек для всех лучей

        for (int i = 0; i < numRays; i++)
        {
            float angle = -30f + i * angleStep; // угол текущего луча
            Vector2 rayDirection = Quaternion.Euler(0, 0, angle) * direction; // направление текущего луча
            RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection, raycastDistance, wallLayer);

            if (hit)
            {
                // рейкаст остановился на стене
                lineRenderer.SetPosition(i * 2, (transform.position + new Vector3(0f, 0f, 0.1f)));
                lineRenderer.SetPosition(i * 2 + 1, hit.point);
            }
            else
            {
                // рейкаст не остановился на стене
                lineRenderer.SetPosition(i * 2, (transform.position + new Vector3(0f, 0f, 0.1f)));
                lineRenderer.SetPosition(i * 2 + 1, (Vector2)transform.position + rayDirection * raycastDistance);
            }
        }
    }
    public bool IsLit(Vector2 position)
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - (Vector2)transform.position).normalized;

        int numRays = 100; // количество лучей
        float angleStep = 60f / (numRays - 1); // шаг угла между лучами

        for (int i = 0; i < numRays; i++)
        {
            float angle = -30f + i * angleStep; // угол текущего луча
            Vector2 rayDirection = Quaternion.Euler(0, 0, angle) * direction; // направление текущего луча
            RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection, raycastDistance, enemyLayer);

            if (hit)
            {
                // рейкаст остановился на объекте врага
                return true;
            }
        }

        return false;
    }
    public bool IsLitItems(Vector2 position)
    {
        // Получаем позицию фонарика
        Vector2 flashlightPosition = transform.position;

        // Проверяем, есть ли между фонариком и предметом стена
        Vector2 direction = (position - flashlightPosition).normalized;
        float distance = Vector2.Distance(position, flashlightPosition);
        RaycastHit2D hit = Physics2D.Raycast(flashlightPosition, direction, distance, wallLayer);

        // Если нет стены, то предмет находится в зоне действия фонарика
        return hit.collider == null;
    }

    public bool IsLitTraps(Vector2 position)
    {
        // Получаем позицию фонарика
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - (Vector2)transform.position).normalized;

        int numRays = 100; // количество лучей
        float angleStep = 60f / (numRays - 1); // шаг угла между лучами

        for (int i = 0; i < numRays; i++)
        {
            float angle = -30f + i * angleStep; // угол текущего луча
            Vector2 rayDirection = Quaternion.Euler(0, 0, angle) * direction; // направление текущего луча
            RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection, raycastDistance, trapLayer);

            if (hit)
            {
                // рейкаст остановился на объекте врага
                return true;
            }
        }

        return false;
    }
}