using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    public int width = 11;
    public int height = 11;
    public int giftCount = 1;
    public int minDistance = 5;
    public int spikesCount = 3;
    public float spikesCooldown = 2f;

    public GameObject wallPrefab;
    public GameObject floorPrefab;
    public GameObject[] giftPrefabs;
    public GameObject holePrefab;

    private bool[,] maze;
    private List<Hole> holes = new List<Hole>();
    private List<Vector2Int> giftPositions = new List<Vector2Int>();

    void Start()
    {
        GenerateMaze();
        SpawnMaze();
        SpawnGifts();
        SpawnHoles();
    }

    void GenerateMaze()
    {
        maze = new bool[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                maze[x, y] = false;
            }
        }

        List<Vector2Int> activeCells = new List<Vector2Int>();
        Vector2Int startCell = GetCenterCell();
        maze[startCell.x, startCell.y] = true;
        activeCells.Add(startCell);
        while (activeCells.Count > 0)
        {
            Vector2Int currentCell = activeCells[Random.Range(0, activeCells.Count)];
            List<Vector2Int> neighbors = new List<Vector2Int>();
            foreach (Vector2Int direction in new Vector2Int[] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right })
            {
                Vector2Int neighbor = currentCell + direction * 2;
                if (IsInBounds(neighbor) && !maze[neighbor.x, neighbor.y])
                {
                    neighbors.Add(neighbor);
                }
            }

            if (neighbors.Count > 0)
            {
                Vector2Int chosenNeighbor = neighbors[Random.Range(0, neighbors.Count)];
                Vector2Int wallBetween = (currentCell + chosenNeighbor) / 2;
                maze[wallBetween.x, wallBetween.y] = true;
                maze[chosenNeighbor.x, chosenNeighbor.y] = true;
                activeCells.Add(chosenNeighbor);
            }
            else
            {
                activeCells.Remove(currentCell);
            }
        }
    }


    void SpawnMaze()
    {
        Vector2Int center = GetCenterCell();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 position = new Vector3(x - center.x, -(y - center.y), 0);
                if (maze[x, y])
                {
                    Instantiate(floorPrefab, position, Quaternion.identity, transform);
                }
                else
                {
                    Instantiate(wallPrefab, position, Quaternion.identity, transform);
                }
            }
        }
    }

    void SpawnGifts()
    {
        Vector2Int playerPosition = GetCenterCell();
        for (int i = 0; i < giftCount; i++)
        {
            Vector2Int giftPosition;
            do
            {
                giftPosition = new Vector2Int(Random.Range(1, width - 1), Random.Range(1, height - 1));
            }
            while (!maze[giftPosition.x, giftPosition.y] || Vector2Int.Distance(giftPosition, playerPosition) < minDistance || giftPositions.Contains(giftPosition));

            giftPositions.Add(giftPosition);

            GameObject randomGift = giftPrefabs[Random.Range(0, giftPrefabs.Length)];
            Vector3 giftWorldPosition = new Vector3(giftPosition.x - playerPosition.x, -(giftPosition.y - playerPosition.y), 0);
            Instantiate(randomGift, giftWorldPosition, Quaternion.identity, transform);
        }
    }

    void SpawnHoles()
    {
        Vector2Int center = GetCenterCell();
        int spikesSpawned = 0;
        while (spikesSpawned < spikesCount)
        {
            Vector2Int spikePosition = new Vector2Int(Random.Range(1, width - 1), Random.Range(1, height - 1));
            if (maze[spikePosition.x, spikePosition.y] && !giftPositions.Contains(spikePosition) && spikePosition != center)
            {
                Vector3 worldPosition = new Vector3(spikePosition.x - center.x, -(spikePosition.y - center.y), 0);
                Hole hole = Instantiate(holePrefab, worldPosition, Quaternion.identity, transform).AddComponent<Hole>();
                holes.Add(hole);
                spikesSpawned++;
            }
        }
        CheckPlayerOnActiveSpikes();
        StartCoroutine(ToggleSpikes());
    }

    void CheckPlayerOnActiveSpikes()
    {
        Vector3 playerPosition = FindObjectOfType<PlayerController>().transform.position;
        Vector2Int playerCell = new Vector2Int(
            Mathf.RoundToInt(playerPosition.x + width / 2),
            Mathf.RoundToInt(-playerPosition.y + height / 2)
        );

        foreach (Hole hole in holes)
        {
            Vector3 spikeWorldPosition = hole.transform.position;
            Vector2Int spikeCell = new Vector2Int(
                Mathf.RoundToInt(spikeWorldPosition.x + width / 2),
                Mathf.RoundToInt(-spikeWorldPosition.y + height / 2)
            );

            if (spikeCell == playerCell && hole.IsActive())
            {
                hole.GetComponent<Hole>().TriggerPlayerDamage();
            }
        }
    }


    IEnumerator ToggleSpikes()
    {
        while (true)
        {
            yield return new WaitForSeconds(spikesCooldown);

            foreach (Hole hole in holes)
            {
                hole.GetComponent<Hole>().ToggleState();
            }
        }
    }

    Vector2Int GetCenterCell()
    {
        return new Vector2Int(width / 2, height / 2);
    }

    bool IsInBounds(Vector2Int position)
    {
        return position.x > 0 && position.x < width - 1 && position.y > 0 && position.y < height - 1;
    }
}
