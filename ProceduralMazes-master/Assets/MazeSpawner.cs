using System.Collections.Generic;
using UnityEngine;

public class MazeSpawner : MonoBehaviour
{
    public Cell CellPrefab;
    public GameObject trapPrefab;
    public GameObject enemyShieldPrefab;
    public GameObject enemyPrefab;
    public GameObject item1Prefab;
    public GameObject item2Prefab;
    public GameObject item3Prefab;
    public GameObject item4Prefab;
    public GameObject item5Prefab;
    public GameObject item6Prefab;
    public GameObject item7Prefab;
    public Vector3 CellSize = new Vector3(1,1,0);
    public HintRenderer HintRenderer;
    public Maze maze;

    private PlayerControls playerControls;
    private MazeGenerator generator;

    private void Start()
    {
        playerControls = FindObjectOfType<PlayerControls>();
        generator = new MazeGenerator();
        maze = generator.GenerateMaze();

        for (int x = 0; x < maze.cells.GetLength(0); x++)
        {
            for (int y = 0; y < maze.cells.GetLength(1); y++)
            {
                Cell c = Instantiate(CellPrefab, new Vector3(x * CellSize.x, y * CellSize.y, y * CellSize.z), Quaternion.identity);

                c.WallLeft.SetActive(maze.cells[x, y].WallLeft);
                c.WallBottom.SetActive(maze.cells[x, y].WallBottom);
            }
        }

        SpawnEnemies(20);
        SpawnShieldEnemies(10);
        SpawnItems();
        SpawnTraps();
    }

    private void SpawnEnemies(int enemyCount)
    {
        List<Vector2Int> enemyPositions = new List<Vector2Int>();

        for (int i = 0; i < enemyCount; i++)
        {
            int x, y;

            do
            {
                x = UnityEngine.Random.Range(1, maze.cells.GetLength(0) - 1);
                y = UnityEngine.Random.Range(1, maze.cells.GetLength(1) - 1);
            } while (maze.cells[x, y].WallLeft || maze.cells[x, y].WallBottom);

            Vector2 centerPosition = new Vector2(x + 0.5f, y + 0.5f);

            GameObject enemy = Instantiate(enemyPrefab, centerPosition, Quaternion.identity);

            enemyPositions.Add(new Vector2Int((int)centerPosition.x, (int)centerPosition.y));
        }
    }

    private void SpawnShieldEnemies(int enemyCount)
    {
        List<Vector2Int> enemyPositions = new List<Vector2Int>();

        for (int i = 0; i < enemyCount; i++)
        {
            int x, y;

            do
            {
                x = UnityEngine.Random.Range(1, maze.cells.GetLength(0) - 1);
                y = UnityEngine.Random.Range(1, maze.cells.GetLength(1) - 1);
            } while (maze.cells[x, y].WallLeft || maze.cells[x, y].WallBottom);

            Vector2 centerPosition = new Vector2(x + 0.5f, y + 0.5f);

            GameObject enemy = Instantiate(enemyShieldPrefab, centerPosition, Quaternion.identity);

            enemyPositions.Add(new Vector2Int((int)centerPosition.x, (int)centerPosition.y));
        }
    }
    private void SpawnItems()
    {
        // Список предметов
        GameObject[] items = new GameObject[] { item1Prefab, item2Prefab, item3Prefab, item4Prefab, item5Prefab, item6Prefab, item7Prefab };

        // Выберите 3 случайных предмета
        GameObject[] randomItems = GetRandomItems(items, 3);

        // Спавните предметы в случайных точках лабиринта
        foreach (GameObject item in randomItems)
        {
            int x, y;

            do
            {
                x = UnityEngine.Random.Range(1, maze.cells.GetLength(0) - 1);
                y = UnityEngine.Random.Range(1, maze.cells.GetLength(1) - 1);
            } while (maze.cells[x, y].WallLeft || maze.cells[x, y].WallBottom);

            Vector2 centerPosition = new Vector2(x + 0.5f, y + 0.5f);

            Instantiate(item, centerPosition, Quaternion.identity);
        }
    }

    // Метод для получения случайных предметов
    private GameObject[] GetRandomItems(GameObject[] items, int count)
    {
        List<GameObject> randomItems = new List<GameObject>();

        for (int i = 0; i < count; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, items.Length);
            randomItems.Add(items[randomIndex]);
            items = RemoveItem(items, randomIndex);
        }

        return randomItems.ToArray();
    }

    // Метод для удаления элемента из массива
    private GameObject[] RemoveItem(GameObject[] items, int index)
    {
        GameObject[] newItems = new GameObject[items.Length - 1];

        for (int i = 0; i < index; i++)
        {
            newItems[i] = items[i];
        }

        for (int i = index + 1; i < items.Length; i++)
        {
            newItems[i - 1] = items[i];
        }

        return newItems;
    }

    private void SpawnTraps()
    {
        // List of trap positions
        List<Vector2Int> trapPositions = new List<Vector2Int>();

        // Spawn traps in random positions
        for (int i = 0; i < 5; i++) // You can adjust the number of traps to spawn
        {
            int x, y;

            do
            {
                x = UnityEngine.Random.Range(1, maze.cells.GetLength(0) - 1);
                y = UnityEngine.Random.Range(1, maze.cells.GetLength(1) - 1);
            } while (maze.cells[x, y].WallLeft || maze.cells[x, y].WallBottom);

            Vector2 centerPosition = new Vector2(x + 0.5f, y + 0.5f);

            GameObject trap = Instantiate(trapPrefab, centerPosition, Quaternion.identity);

            trapPositions.Add(new Vector2Int((int)centerPosition.x, (int)centerPosition.y));
        }
    }
}