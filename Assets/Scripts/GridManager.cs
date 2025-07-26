using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GridManager : MonoBehaviour {
    public GameObject boardGrid;
    public GameObject tilePrefab;
    public int gridSize = 3;

    public GameObject[][] boardArray;
    public static GridManager Instance;

    public GameObject holeObject;
    public Vector2Int holePosition;

    private GridLayoutGroup gridLayout;

    private void Awake() {
        Instance = this;
    }

    void Start() {
        gridLayout = boardGrid.GetComponent<GridLayoutGroup>();
        float cellSize = 300f / gridSize;
        gridLayout.cellSize = new Vector2(cellSize, cellSize);

        GridMaker();
        StartShuffle(100);
    }

    public void GridMaker() {
        int count = 1;
        boardArray = new GameObject[gridSize][];

        for (int i = 0; i < gridSize; i++) {
            boardArray[i] = new GameObject[gridSize];

            for (int j = 0; j < gridSize; j++) {
                GameObject tile = Instantiate(tilePrefab, boardGrid.transform);
                boardArray[i][j] = tile;

                TileController controller = tile.GetComponent<TileController>();
                controller.Initialize(new Vector2Int(i, j));

                if (i == gridSize - 1 && j == gridSize - 1) {
                    tile.GetComponent<Image>().color = new Color(1, 1, 1, 0);
                    tile.GetComponentInChildren<TextMeshProUGUI>().text = "";
                    tile.name = "Hole";
                    holeObject = tile;
                    holePosition = new Vector2Int(i, j);
                } else {
                    tile.GetComponentInChildren<TextMeshProUGUI>().text = count.ToString();
                    count++;
                }
            }
        }

        UpdateTileFadeStates();
    }

    public void TryMoveTile(TileController tile) {
        Vector2Int pos = tile.gridPosition;

        if (IsAdjacent(pos, holePosition)) {
            GameObject tileObj = boardArray[pos.x][pos.y];

            boardArray[holePosition.x][holePosition.y] = tileObj;
            boardArray[pos.x][pos.y] = holeObject;

            Transform tileTransform = tileObj.transform;
            Transform holeTransform = holeObject.transform;

            Vector3 tempPos = tileTransform.position;
            tileTransform.position = holeTransform.position;
            holeTransform.position = tempPos;

            Vector2Int temp = holePosition;
            holePosition = pos;
            tile.gridPosition = temp;

            UpdateTileFadeStates();
        } else {
            Debug.LogWarning($"⚠️ Blocked invalid move: Tile at {pos} is not adjacent to hole at {holePosition}");
        }
    }

    private bool IsAdjacent(Vector2Int a, Vector2Int b) {
        int dx = Mathf.Abs(a.x - b.x);
        int dy = Mathf.Abs(a.y - b.y);
        return (dx == 1 && dy == 0) || (dx == 0 && dy == 1);
    }

    private bool IsWithinBounds(Vector2Int pos) {
        return pos.x >= 0 && pos.x < gridSize && pos.y >= 0 && pos.y < gridSize;
    }

    private List<TileController> GetAdjacentTiles() {
        List<TileController> adjacent = new List<TileController>();
        Vector2Int[] directions = {
            Vector2Int.up,
            Vector2Int.down,
            Vector2Int.left,
            Vector2Int.right
        };

        foreach (var dir in directions) {
            Vector2Int neighbor = holePosition + dir;
            if (IsWithinBounds(neighbor)) {
                GameObject tileObj = boardArray[neighbor.x][neighbor.y];
                if (tileObj != null && tileObj != holeObject) {
                    adjacent.Add(tileObj.GetComponent<TileController>());
                }
            }
        }

        return adjacent;
    }

    private IEnumerator ShuffleRoutine(int moves) {
        for (int i = 0; i < moves; i++) {
            yield return new WaitForSeconds(0.01f);
            List<TileController> adjacent = GetAdjacentTiles();
            if (adjacent.Count > 0) {
                TileController randomTile = adjacent[Random.Range(0, adjacent.Count)];
                TryMoveTile(randomTile);
            }
        }

        Debug.Log("✅ Shuffling complete");
    }

    public void StartShuffle(int moveCount = 50) {
        StartCoroutine(ShuffleRoutine(moveCount));
    }

    private void UpdateTileFadeStates() {
        for (int i = 0; i < gridSize; i++) {
            for (int j = 0; j < gridSize; j++) {
                GameObject tile = boardArray[i][j];
                if (tile == holeObject) continue;

                TileController controller = tile.GetComponent<TileController>();
                Vector2Int currentPos = new Vector2Int(i, j);
                bool isCorrect = currentPos == controller.originalPosition;

                controller.SetFaded(isCorrect);
            }
        }
    }
}
