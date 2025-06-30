using System.Net.Mime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleManager : MonoBehaviour {
    public GameObject boardGrid;
    public GameObject tilePrefab;
    public TextMeshPro insideText;
    public int gridSize;
    
    public GameObject[][] boardArray;
    
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
                boardArray[i][j].GetComponentInChildren<TextMeshProUGUI>().text = $"{count++}";
                
            }
        }
        
    }
}