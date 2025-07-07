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

        GridMaker(); // just create the grid
    }

    public void GridMaker() {
        int count = 1;
        boardArray = new GameObject[gridSize][];

        for (int i = 0; i < gridSize; i++) {
            boardArray[i] = new GameObject[gridSize];

            for (int j = 0; j < gridSize; j++) {
                var tile = Instantiate(tilePrefab, boardGrid.transform);
                boardArray[i][j] = tile;

// Get TileController and set position
                tile.GetComponent<TileController>().Initialize(new Vector2Int(i, j));

// Setup hole
                if (i == gridSize - 1 && j == gridSize - 1) {
                    Image tileImage = tile.GetComponent<Image>();
                    Color color = tileImage.color;
                    color.a = 0f; // transparent
                    tileImage.color = color;
                    tile.GetComponentInChildren<TextMeshProUGUI>().text = "";
                    tile.name = "Hole";
                    holeObject = tile;
                    holePosition = new Vector2Int(i, j);
                } else {
                    tile.GetComponentInChildren<TextMeshProUGUI>().text = $"{count++}";
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

            // Swap hole position and tile's position
            Vector2Int temp = holePosition;
            holePosition = pos;
            tile.gridPosition = temp;

            // 🔁 Check all tiles after move
            UpdateTileFadeStates();
        }
    }


    private bool IsAdjacent(Vector2Int a, Vector2Int b) {
        int dx = Mathf.Abs(a.x - b.x);
        int dy = Mathf.Abs(a.y - b.y);
        return (dx == 1 && dy == 0) || (dx == 0 && dy == 1);
    }

    private void UpdateTileFadeStates() {
        for (int i = 0; i < gridSize; i++) {
            for (int j = 0; j < gridSize; j++) {
                var tileObj = boardArray[i][j];
                if (tileObj == holeObject) continue;

                TileController controller = tileObj.GetComponent<TileController>();
                Vector2Int currentPos = new Vector2Int(i, j);
                bool isCorrect = currentPos == controller.originalPosition;

                controller.SetFaded(isCorrect);
            }
        }
    }

}
