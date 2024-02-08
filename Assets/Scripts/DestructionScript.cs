using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

namespace AlexzanderCowell
{
    public class DestructionScript : MonoBehaviour
    {
        [Range(-100, 100)] [SerializeField] private int gridSizeX;
        [Range(-100, 100)] [SerializeField] private int gridSizeY;
        [Range(-1, 3)] [SerializeField] private float cellSize;
        [Range(-100, 100)] [SerializeField] private float adjustableXLocation;
        [Range(-100, 100)] [SerializeField] private float adjustableYLocation;
        [Range(-100, 100)] [SerializeField] private int cellSizingX;
        [Range(-100, 100)] [SerializeField] private int cellSizingY;
        private int[,] grid; // Represents the state of each grid cell
        [Header("Tile Prefab")]
        [SerializeField] private GameObject dirtLayer;
        [SerializeField] private GameObject grassDirtLayer;
        //[SerializeField] private GameObject rockLayer;
        private Vector3 gridCenter;
        private GameObject spawnTile;
        private Vector3 cellCenter;

        
        
        private void Awake()
        {
            InitializeGrid();
        }

        private void InitializeGrid()
        {
            grid = new int[gridSizeX, gridSizeY];
            gridCenter = transform.position + new Vector3((gridSizeX - adjustableXLocation) * cellSize / 1, (gridSizeY - adjustableYLocation) * cellSize / 1, 0f);
            
            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    cellCenter = gridCenter + new Vector3(x * cellSize, y * cellSize, 0f);
            
                    // Determine which layer to use
                    if (y == gridSizeY - 1) // Top layer
                    {
                        spawnTile = Instantiate(grassDirtLayer, cellCenter, Quaternion.identity);
                    }
                    else // Rest of the layers
                    {
                        spawnTile = Instantiate(dirtLayer, cellCenter, Quaternion.identity);
                    }
            
                    // Future enhancement: Add logic for random layer generation here
            
                    spawnTile.name = $"Tile {x} {y}";
                    spawnTile.transform.parent = this.transform;
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
