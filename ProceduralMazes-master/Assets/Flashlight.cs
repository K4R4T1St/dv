using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public LayerMask wallLayer; // ���� ����
    public LayerMask enemyLayer;
    public LayerMask trapLayer;
    public float raycastDistance = 4f; // ���������� ��������
    public float raycastInterval = 0.05f; // �������� ����� ����������

    private float nextRaycastTime = 0f;
    private LineRenderer lineRenderer;
    public static Flashlight Instance { get; private set; } // ����������� ����������, �������� ������ �� ��������� ��������

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
        // ���������� ������� �� ������� ������ � ����������� ������� ������
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - (Vector2)transform.position).normalized;

        // ������� ����� ��������
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.startColor = new Color(1, 1, 1, 0.05f);
        lineRenderer.endColor = new Color(1, 1, 1, 0.05f);

        // ������� ��������� ����� � ������� ������
        int numRays = 100; // ���������� �����
        float angleStep = 60f / (numRays - 1); // ��� ���� ����� ������
        lineRenderer.positionCount = numRays * 2; // ���������� ����� ��� ���� �����

        for (int i = 0; i < numRays; i++)
        {
            float angle = -30f + i * angleStep; // ���� �������� ����
            Vector2 rayDirection = Quaternion.Euler(0, 0, angle) * direction; // ����������� �������� ����
            RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection, raycastDistance, wallLayer);

            if (hit)
            {
                // ������� ����������� �� �����
                lineRenderer.SetPosition(i * 2, (transform.position + new Vector3(0f, 0f, 0.1f)));
                lineRenderer.SetPosition(i * 2 + 1, hit.point);
            }
            else
            {
                // ������� �� ����������� �� �����
                lineRenderer.SetPosition(i * 2, (transform.position + new Vector3(0f, 0f, 0.1f)));
                lineRenderer.SetPosition(i * 2 + 1, (Vector2)transform.position + rayDirection * raycastDistance);
            }
        }
    }
    public bool IsLit(Vector2 position)
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - (Vector2)transform.position).normalized;

        int numRays = 100; // ���������� �����
        float angleStep = 60f / (numRays - 1); // ��� ���� ����� ������

        for (int i = 0; i < numRays; i++)
        {
            float angle = -30f + i * angleStep; // ���� �������� ����
            Vector2 rayDirection = Quaternion.Euler(0, 0, angle) * direction; // ����������� �������� ����
            RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection, raycastDistance, enemyLayer);

            if (hit)
            {
                // ������� ����������� �� ������� �����
                return true;
            }
        }

        return false;
    }
    public bool IsLitItems(Vector2 position)
    {
        // �������� ������� ��������
        Vector2 flashlightPosition = transform.position;

        // ���������, ���� �� ����� ��������� � ��������� �����
        Vector2 direction = (position - flashlightPosition).normalized;
        float distance = Vector2.Distance(position, flashlightPosition);
        RaycastHit2D hit = Physics2D.Raycast(flashlightPosition, direction, distance, wallLayer);

        // ���� ��� �����, �� ������� ��������� � ���� �������� ��������
        return hit.collider == null;
    }

    public bool IsLitTraps(Vector2 position)
    {
        // �������� ������� ��������
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - (Vector2)transform.position).normalized;

        int numRays = 100; // ���������� �����
        float angleStep = 60f / (numRays - 1); // ��� ���� ����� ������

        for (int i = 0; i < numRays; i++)
        {
            float angle = -30f + i * angleStep; // ���� �������� ����
            Vector2 rayDirection = Quaternion.Euler(0, 0, angle) * direction; // ����������� �������� ����
            RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection, raycastDistance, trapLayer);

            if (hit)
            {
                // ������� ����������� �� ������� �����
                return true;
            }
        }

        return false;
    }
}