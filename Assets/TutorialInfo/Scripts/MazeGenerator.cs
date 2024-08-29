using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MazeGenerator : MonoBehaviour
{
    public int width = 20;
    public int height = 20;
    public int currentLevel = 1;
    public GameObject wallPrefab;
    public GameObject exitPrefab;
    public GameObject enemyPrefab; // Префаб ворога
    public Camera mainCamera;
    public TextMeshProUGUI levelText;



    private bool[,] maze;
    private List<GameObject> spawnedObjects = new List<GameObject>();
    private List<GameObject> enemies = new List<GameObject>(); // Список для зберігання ворогів

    private Vector2Int[] directions = {
        Vector2Int.up,
        Vector2Int.down,
        Vector2Int.left,
        Vector2Int.right
    };

    void Start()
    {
        GenerateAndDrawMaze();
        UpdateLevelText();

    }

    void Update()
    {
        UpdateTextPosition(); // Оновлюємо позицію тексту кожного кадру
    }
  
    public void GenerateAndDrawMaze()
    {
        GenerateMaze();
        DrawMaze();
        AdjustCamera();
        SpawnEnemies(); // Спавнити ворогів після генерації лабіринту
    }

    void GenerateMaze()
    {
        maze = new bool[width, height];
        Stack<Vector2Int> stack = new Stack<Vector2Int>();
        Vector2Int currentCell = new Vector2Int(1, 1);

        maze[currentCell.x, currentCell.y] = true;
        stack.Push(currentCell);

        Vector2Int previousCell = currentCell;

        while (stack.Count > 0)
        {
            currentCell = stack.Peek();
            List<Vector2Int> unvisitedNeighbors = new List<Vector2Int>();

            foreach (var dir in directions)
            {
                Vector2Int neighbor = currentCell + dir * 2;
                if (IsInMaze(neighbor) && !maze[neighbor.x, neighbor.y])
                {
                    unvisitedNeighbors.Add(neighbor);
                }
            }

            if (unvisitedNeighbors.Count > 0)
            {
                Vector2Int chosenNeighbor;
                if (stack.Count > 1)
                {
                    List<Vector2Int> differentDirectionNeighbors = unvisitedNeighbors.FindAll(neighbor => (neighbor - currentCell) != (currentCell - previousCell));

                    if (differentDirectionNeighbors.Count > 0 && Random.value > 0.3f) 
                    {
                        chosenNeighbor = differentDirectionNeighbors[Random.Range(0, differentDirectionNeighbors.Count)];
                    }
                    else
                    {
                        chosenNeighbor = unvisitedNeighbors[Random.Range(0, unvisitedNeighbors.Count)];
                    }
                }
                else
                {
                    chosenNeighbor = unvisitedNeighbors[Random.Range(0, unvisitedNeighbors.Count)];
                }

                Vector2Int wall = currentCell + (chosenNeighbor - currentCell) / 2;
                maze[wall.x, wall.y] = true;
                maze[chosenNeighbor.x, chosenNeighbor.y] = true;
                stack.Push(chosenNeighbor);

                previousCell = currentCell;

                if (Random.value > 0.5f && unvisitedNeighbors.Count > 1)
                {
                    Vector2Int randomNeighbor = unvisitedNeighbors[Random.Range(0, unvisitedNeighbors.Count)];
                    if (randomNeighbor != chosenNeighbor)
                    {
                        Vector2Int randomWall = currentCell + (randomNeighbor - currentCell) / 2;
                        maze[randomWall.x, randomWall.y] = true;
                        maze[randomNeighbor.x, randomNeighbor.y] = true;
                    }
                }
            }
            else
            {
                stack.Pop();
                if (stack.Count > 0)
                {
                    previousCell = currentCell;
                }
            }
        }
    }

    bool IsInMaze(Vector2Int point)
    {
        return point.x > 0 && point.x < width - 1 && point.y > 0 && point.y < height - 1;
    }

    void DrawMaze()
{
    ClearMaze();
    ClearEnemies(); // Clear enemies when generating a new maze

    Vector2Int farthestCell = Vector2Int.zero;
    float maxDistance = 0;
    Vector2Int start = new Vector2Int(1, 1);
    float scaleMultiplier = 2.0f; // Increase this value to make passages wider

    for (int x = 0; x < width; x++)
    {
        for (int y = 0; y < height; y++)
        {
            if (!maze[x, y])
            {
                GameObject wall = Instantiate(wallPrefab, new Vector2(x * scaleMultiplier, y * scaleMultiplier), Quaternion.identity);
                wall.transform.localScale *= scaleMultiplier; // Increase the size of each wall
                spawnedObjects.Add(wall);
            }
            else
            {
                float distance = Vector2Int.Distance(start, new Vector2Int(x, y));
                if (distance > maxDistance)
                {
                    maxDistance = distance;
                    farthestCell = new Vector2Int(x, y);
                }
            }
        }
    }

    Debug.Log($"Farthest Cell Position: {farthestCell.x}, {farthestCell.y}");

    GameObject exit = Instantiate(exitPrefab, new Vector2(farthestCell.x * scaleMultiplier, farthestCell.y * scaleMultiplier), Quaternion.identity);
    spawnedObjects.Add(exit);

    Debug.Log("Exit generated at: " + exit.transform.position);

    GameObject player = GameObject.FindWithTag("Player");
    if (player != null)
    {
        player.transform.position = new Vector2(start.x * scaleMultiplier, start.y * scaleMultiplier);
    }
}


    void AdjustCamera()
    {
        if (mainCamera != null)
        {
            mainCamera.transform.position = new Vector3(width / 2f, height / 2f, -10);
            float aspectRatio = (float)Screen.width / Screen.height;
            mainCamera.orthographicSize = Mathf.Max(width / 2f / aspectRatio, height / 2f);
        }
    }

    void ClearMaze()
    {
        foreach (var obj in spawnedObjects)
        {
            Destroy(obj);
        }
        spawnedObjects.Clear();
    }

    void ClearEnemies()
    {
        foreach (var enemy in enemies)
        {
            Destroy(enemy);
        }
        enemies.Clear();
    }

    void SpawnEnemies()
    {
        int enemyCount = currentLevel * 5; // Кількість ворогів залежить від рівня
        float enemySpeed = 2f + (currentLevel - 1) * 0.2f; // Швидкість ворогів збільшується з кожним рівнем

        for (int i = 0; i < enemyCount; i++)
        {
            Vector2 spawnPosition = new Vector2(Random.Range(1, width - 1), Random.Range(1, height - 1));
            GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            enemy.GetComponent<EnemyController>().speed = enemySpeed; // Встановлюємо швидкість ворога
            enemies.Add(enemy);
        }
    }

    public void IncreaseMazeSizeAndRegenerate()
    {
        width += 5;
        height += 5;
        currentLevel++;
         GameObject map = GameObject.Find("Map");
    if (map != null)
    {
        map.transform.localScale += new Vector3(2f, 2f, 0);
    }
        GenerateAndDrawMaze();
        UpdateLevelText();
    }

    void UpdateLevelText()
    {
        if (levelText != null)
        {
            levelText.text = "Level: " + currentLevel;
           // Збільшення розміру шрифту на 0.5 при кожному новому рівні
        }
    }

    void UpdateTextPosition()
    {
        if (mainCamera != null && levelText != null)
        {
            Vector3 newPosition = mainCamera.ViewportToWorldPoint(new Vector3(0, 1, mainCamera.nearClipPlane));
            newPosition.z = 0;

            float xOffset = 3f;
            float yOffset = -2f;

            newPosition.x += xOffset;
            newPosition.y += yOffset;

            levelText.transform.position = newPosition;
        }
    }
}
