using System;
using System.Net.Mime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour {
    public GameObject boardGrid;
    public GameObject tilePrefab;
    public int gridSize;
    
    public GameObject[][] boardArray;
    public static GridManager Instance;

    private void Awake() {
        Instance = this;
    }

    void Start() {
        GetComponent<GridLayoutGroup>().cellSize = new Vector2(300f / gridSize, 300f / gridSize);
        GridMaker();
    }

    // Update is called once per frame
    void Update()
    { 
       
    }

    public void GridMaker() {
        int count = 1;
        boardArray = new GameObject[gridSize][];
        for (int i = 0; i < gridSize; i++) {
            boardArray[i] = new GameObject[gridSize];
            for (int j = 0; j < gridSize; j++) {
                boardArray[i][j] = Instantiate(tilePrefab, boardGrid.transform);
                // Make the last tile the "hole"
                if (i == gridSize - 1 && j == gridSize - 1) {
                    Image tileImage = boardArray[i][j].GetComponentInChildren<Image>();
                    Color color = tileImage.color;
                    color.a = 0f; // fully transparent
                    tileImage.color = color;
                    boardArray[i][j].GetComponentInChildren<TextMeshProUGUI>().text = "";
                    boardArray[i][j].name = "Hole";
                }
                else {
                    boardArray[i][j].GetComponentInChildren<TextMeshProUGUI>().text = $"{count++}";
                }
                
            }
        }
        
    }
   
    
}