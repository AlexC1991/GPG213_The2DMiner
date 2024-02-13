using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AlexzanderCowell
{
    public class DestructionScript : MonoBehaviour
    {
        [Header("Tile Grid Placement")]
        [Range(-100, 1000)] [SerializeField] private int gridSizeX;
        [Range(-100, 1000)] [SerializeField] private int gridSizeY;
        [Range(-1, 3)] [SerializeField] private float cellSize;
        [Range(-100, 1000)] [SerializeField] private float adjustableXLocation;
        [Range(-100, 1000)] [SerializeField] private float adjustableYLocation;
        private int cellSizingX;
        private int cellSizingY;
        private int[,] grid; // Represents the state of each grid cell
        [Header("Most Common Blocks")]
        [SerializeField] private GameObject[] mostSeenLayer;
        [SerializeField] private GameObject grassDirtLayer;
        [Header("None Common Ore's")]
        [SerializeField] private GameObject[] oreLayer;
        [Header("Copper Ore & Commons")]
        [SerializeField] private GameObject[] copperOreOptions;
        [Header("Steel Ore & Commons")]
        [SerializeField] private GameObject[] steelOreOptions;
        [Header("Diamond Ore & Commons")]
        [SerializeField] private GameObject[] diamondOreOptions;
        [Header("BlocksOnScene")]
        [SerializeField] private GameObject[] blockChildren;
        [SerializeField] private GameObject parentOfBlockChildren;
        [Header("Sprites Of Each Ore")]
        [SerializeField] private Sprite copperExample;
        [SerializeField] private Sprite steelExample;
        [SerializeField] private Sprite diamondExample;
        [Header("Lists Of Each Ore In Grid")]
        [SerializeField] private List<GameObject> copperBlocks;
        [SerializeField] private List<GameObject> steelBlocks;
        [SerializeField] private List<GameObject> diamondBlocks;
        private Vector3 gridCenter;
        private GameObject spawnTile;
        private Vector3 cellCenter;
        float noiseScale = 0.1f;
        private float offsetX;
        private float offsetY;
        public bool checkCopperStage;

        private void Start()
        {
            offsetX = Random.Range(0f, 99999f);
            offsetY = Random.Range(0f, 99999f);
            StartCoroutine(InitializeGrid());
            StartCoroutine(CheckCopperOres());
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
            
            blockChildren = GetAllChildren(parentOfBlockChildren.transform);
            checkCopperStage = true;
        }

        IEnumerator CheckCopperOres()
        {
            while (true)
            {
                if (checkCopperStage)
                {
                    foreach (GameObject g in blockChildren)
                    {
                        if (g.GetComponent<SpriteRenderer>().sprite == copperExample)
                        {
                            copperBlocks.Add(g);
                        }
                    }

                    float spacing = 1.0f; // Replace with your actual spacing between blocks

                    foreach (GameObject copperBlock in copperBlocks)
                    {
                        Vector3 position = copperBlock.transform.position;
                        // Define adjacent positions
                        Vector3[] adjacentPositions = new Vector3[]
                        {
                            position + new Vector3(spacing, 0, 0), // Right
                            position + new Vector3(-spacing, 0, 0), // Left
                            position + new Vector3(0, spacing, 0), // Up
                            position + new Vector3(0, -spacing, 0) // Down
                        };

                        foreach (Vector3 adjacentPosition in adjacentPositions)
                        {
                            GameObject adjacentBlock = FindBlockAtPosition(adjacentPosition);
                            if (adjacentBlock != null &&
                                adjacentBlock.GetComponent<SpriteRenderer>().sprite != copperExample)
                            {
                                // This is where you apply your logic for replacing the block or whatever you intend to do
                                // For example, to replace it with a random copper block:
                                GameObject newBlockPrefab = copperOreOptions[Random.Range(0, copperOreOptions.Length)];
                                Instantiate(newBlockPrefab, adjacentPosition, Quaternion.identity);
                                Destroy(adjacentBlock); // Remove the old block
                            }
                        }
                    }
                    checkCopperStage = false;
                }
                
                yield return null;
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
        
            private GameObject[] GetAllChildren(Transform parent)
            {
                int childCount = parent.childCount;
                GameObject[] children = new GameObject[childCount];

                for (int i = 0; i < childCount; i++)
                {
                    Transform child = parent.GetChild(i);
                    children[i] = child.gameObject;
                }

                return children;
            }
        
            private GameObject FindBlockAtPosition(Vector3 position)
            {
                foreach (GameObject block in blockChildren)
                {
                    if (block.transform.position == position) // Consider using approximation due to floating-point precision
                    {
                        return block;
                    }
                }
                return null;
            }
    }
}
