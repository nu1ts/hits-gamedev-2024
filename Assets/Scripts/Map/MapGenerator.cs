using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    public int width;
    public int height;

    public Tilemap tilemap;
    public RuleTile wallTile;
    public TileBase floorTile;

    [Range(0, 1)]
    public float wallClusterRatio; // Коэффициент, определяющий процент заполнения стенами

    private int[,] map;

    void Start()
    {
        StartCoroutine(GenerateMapCoroutine());
    }

    IEnumerator GenerateMapCoroutine()
    {
        GenerateBaseShape();
        yield return StartCoroutine(GenerateWallClustersCoroutine());
        DrawMap();
    }


    void GenerateBaseShape()
    {
        map = new int[width, height];

        // Заполнение всей карты стенами
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                map[x, y] = 1;
            }
        }

        // Создание основной формы карты из нескольких прямоугольников случайного размера и положения
        int numShapes = Random.Range(5, 8); // Количество фигур на карте
        for (int i = 0; i < numShapes; i++)
        {
            int shapeWidth = Random.Range(width / 3, width / 2);
            int shapeHeight = Random.Range(height / 3, height / 2);
            int startX = Random.Range(0, width - shapeWidth);
            int startY = Random.Range(0, height - shapeHeight);

            for (int x = startX; x < startX + shapeWidth; x++)
            {
                for (int y = startY; y < startY + shapeHeight; y++)
                {
                    map[x, y] = 0; // Пол
                }
            }
        }
    }

    List<Vector2Int> GetRandomTetrisShape()
    {
        List<List<Vector2Int>> tetrisShapes = new List<List<Vector2Int>>
        {
            new List<Vector2Int> { new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(2, 0), new Vector2Int(3, 0) }, // I
            new List<Vector2Int> { new Vector2Int(0, 0), new Vector2Int(0, 1), new Vector2Int(1, 0), new Vector2Int(1, 1) }, // O
            new List<Vector2Int> { new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(2, 0), new Vector2Int(1, 1) }, // T
            new List<Vector2Int> { new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(1, 1), new Vector2Int(2, 1) }, // S
            new List<Vector2Int> { new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(1, -1), new Vector2Int(2, -1) }, // Z
            new List<Vector2Int> { new Vector2Int(0, 0), new Vector2Int(0, 1), new Vector2Int(0, 2), new Vector2Int(1, 2) }, // L
            new List<Vector2Int> { new Vector2Int(0, 0), new Vector2Int(0, 1), new Vector2Int(0, 2), new Vector2Int(-1, 2) } // J
        };

        return tetrisShapes[Random.Range(0, tetrisShapes.Count)];
    }

    IEnumerator GenerateWallClustersCoroutine()
    {
        int totalCells = (width - 2) * (height - 2); // Количество внутренних клеток (без границ)
        int numberOfClusters = Mathf.FloorToInt(totalCells * wallClusterRatio);

        for (int i = 0; i < numberOfClusters; i++)
        {
            Vector2Int startPosition = new Vector2Int(Random.Range(1, width - 2), Random.Range(1, height - 2));
            List<Vector2Int> tetrisShape = GetRandomTetrisShape();

            foreach (var position in tetrisShape)
            {
                Vector2Int wallPosition = startPosition + position;
                if (IsInMapRange(wallPosition.x, wallPosition.y))
                {
                    map[wallPosition.x, wallPosition.y] = 1; // Установить стену
                    DrawTile(wallPosition.x, wallPosition.y, wallTile); // Обновление тайла в реальном времени
                    yield return new WaitForSeconds(0.05f); // Пауза для визуализации
                }
            }
        }
    }

    // IEnumerator GenerateWallClustersCoroutine()
    // {
    //     int totalCells = (width - 2) * (height - 2); // Количество внутренних клеток (без границ)
    //     int numberOfClusters = Mathf.FloorToInt(totalCells * wallClusterRatio);

    //     for (int i = 0; i < numberOfClusters; i++)
    //     {
    //         int clusterSize = Random.Range((width + height) / 7, (width + height) / 5); // Размер кластера стен
    //         int startX = Random.Range(1, width - 1);
    //         int startY = Random.Range(1, height - 1);

    //         for (int j = 0; j < clusterSize; j++)
    //         {
    //             int x = startX + Random.Range(-2, 3);
    //             int y = startY + Random.Range(-2, 3);

    //             if (IsInMapRange(x, y))
    //             {
    //                 map[x, y] = 1; // Установить стену
    //                 DrawTile(x, y, wallTile);
    //             }

    //             Debug.Log("A");
    //             yield return new WaitForSeconds(0.01f); // Пауза для визуализации
    //         }
    //     }
    // }

    List<Vector2Int> GetFloorNeighbors(Vector2Int current)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();

        Vector2Int[] directions = new Vector2Int[]
        {
            new Vector2Int(1, 0),
            new Vector2Int(-1, 0),
            new Vector2Int(0, 1),
            new Vector2Int(0, -1)
        };

        foreach (var direction in directions)
        {
            Vector2Int neighbor = current + direction;
            if (IsInMapRange(neighbor.x, neighbor.y) && map[neighbor.x, neighbor.y] == 0)
            {
                neighbors.Add(neighbor);
            }
        }

        return neighbors;
    }

    bool IsInMapRange(int x, int y)
    {
        return x >= 1 && x < width - 1 && y >= 1 && y < height - 1;
    }

    void DrawMap()
    {
        tilemap.ClearAllTiles(); // Очистка старых плиток

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                DrawTile(x, y, map[x, y] == 1 ? wallTile : floorTile);
            }
        }
    }

    void DrawTile(int x, int y, TileBase tile)
    {
        Vector3Int tilePosition = new Vector3Int(x, y, 0);
        tilemap.SetTile(tilePosition, tile);
    }

    // void DrawMap()
    // {
    //     tilemap.ClearAllTiles(); // Очистка старых плиток

    //     for (int x = 0; x < width; x++)
    //     {
    //         for (int y = 0; y < height; y++)
    //         {
    //             Vector3Int tilePosition = new Vector3Int(x, y, 0);

    //             if (map[x, y] == 1)
    //             {
    //                 tilemap.SetTile(tilePosition, wallTile);
    //             }
    //             else
    //             {
    //                 tilemap.SetTile(tilePosition, floorTile);
    //             }
    //         }
    //     }
    // }
}
