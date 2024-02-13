using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AlexzanderCowell
{
    public class DestructionScript : MonoBehaviour
    {
        [Range(-100, 1000)] [SerializeField] private int gridSizeX;
        [Range(-100, 1000)] [SerializeField] private int gridSizeY;
        [Range(-1, 3)] [SerializeField] private float cellSize;
        [Range(-100, 1000)] [SerializeField] private float adjustableXLocation;
        [Range(-100, 1000)] [SerializeField] private float adjustableYLocation;
        private int cellSizingX;
        private int cellSizingY;
        private int[,] grid; // Represents the state of each grid cell
        [Header("Tile Prefab")]
        [SerializeField] private GameObject[] mostSeenLayer;
        [SerializeField] private GameObject grassDirtLayer;
        [SerializeField] private GameObject[] oreLayer;
        private Vector3 gridCenter;
        private GameObject spawnTile;
        private Vector3 cellCenter;
        float noiseScale = 0.1f;
        private float offsetX;
        private float offsetY;

        private void Start()
        {
            offsetX = Random.Range(0f, 99999f);
            offsetY = Random.Range(0f, 99999f);
            StartCoroutine(InitializeGrid());
        }

        IEnumerator InitializeGrid()
        {
            grid = new int[gridSizeX, gridSizeY];
            gridCenter = transform.position + new Vector3((gridSizeX - adjustableXLocation) * cellSize, (gridSizeY - adjustableYLocation) * cellSize, 0f);
    
            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    cellCenter = gridCenter + new Vector3(x * cellSize, y * cellSize, 0f);
                    float xCoord = (x + offsetX) * noiseScale;
                    float yCoord = (y + offsetY) * noiseScale;
                    float noiseValue = Mathf.PerlinNoise(xCoord, yCoord);
            
                    GameObject selectedPrefab;
                    if (y == gridSizeY - 1)
                    {
                        selectedPrefab = grassDirtLayer;
                        spawnTile = Instantiate(selectedPrefab, cellCenter, Quaternion.identity);
                    }
                    else if (noiseValue < 0.6f)
                    {
                        selectedPrefab = mostSeenLayer[Random.Range(0, mostSeenLayer.Length)];
                        spawnTile = Instantiate(selectedPrefab, cellCenter, Quaternion.identity);
                    }
                    else if (noiseValue > 0.6f)
                    {
                        selectedPrefab = oreLayer[Random.Range(0, oreLayer.Length)];
                        spawnTile = Instantiate(selectedPrefab, cellCenter, Quaternion.identity);
                    }
                    else
                    {
                        selectedPrefab = mostSeenLayer[Random.Range(0, mostSeenLayer.Length)];
                        spawnTile = Instantiate(selectedPrefab, cellCenter, Quaternion.identity);
                    }
                    spawnTile.name = $"Tile {x} {y}";
                    spawnTile.transform.parent = this.transform;

                    if ((x * gridSizeY + y) % 800 == 0) // Adjust the modulo value based on your performance needs
                    {
                        yield return null; // Pause the coroutine, wait for the next frame
                    }
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            grid = new int[gridSizeX, gridSizeY];
            gridCenter = transform.position + new Vector3((gridSizeX - adjustableXLocation) * cellSize / 1, (gridSizeY - adjustableYLocation) * cellSize / 1, 0f);
            
            for (int x = cellSizingX; x < gridSizeX; x++)
            {
                for (int y = cellSizingY; y < gridSizeY; y++)
                {
                    cellCenter = gridCenter + new Vector3(x * cellSize, y * cellSize, 0f);
                    Gizmos.color = Color.green;
                    Gizmos.DrawWireCube(cellCenter, new Vector3(cellSize, cellSize, 0f));
                }
            }
        }
    }
}
