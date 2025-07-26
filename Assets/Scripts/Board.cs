using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour {
    private GridLayoutGroup gridLayoutGroup;
    
    // Prefab of Tile
    [SerializeField] private GameObject tileButton;
    
    // tiles array holding all the tiles of the user
    public Tile[][] tiles;
    public (int, int) emptyTileIndex;
    
    // Flag to stop user from playing move in animation
    private bool playingMove = false;

    [SerializeField] private List<Sprite> tileSprites;
    
    [SerializeField] private float tileAnimationSpeed = 0.5f;

    private void Start() {
        gridLayoutGroup = GetComponent<GridLayoutGroup>();
    }

    // This method initializes the grid
    public void InitializeNewGame(int size, string patternType) {
        // Setting Grid layout properties
        gridLayoutGroup.cellSize = new Vector2(1000 / size, 1000 / size);
        gridLayoutGroup.constraintCount = size;

        tiles = new Tile[GameInfoStaticData.gridSize][];
        InstantiateVisual();
        
        if (patternType == "Default") {
            int counter = 1;
            for (int i = 0; i < tiles.Length; i++) {
                for (int j = 0; j < tiles[i].Length; j++) {
                    Tile tile = tiles[i][j];
                    tile.boardIndexX = i;
                    tile.boardIndexY = j;
                    tile.GetComponent<Button>().onClick.AddListener(() => GameManager.Instance.MoveTile(tile));
                    tile.SetValue(counter++);
                }
            }
            emptyTileIndex = (size - 1, size - 1);
            tiles[size - 1][size - 1].GetComponent<Tile>().image.color = Color.clear;
            tiles[size - 1][size - 1].GetComponent<Tile>().valueTMPro.text = "";
        }
        else if (patternType == "Upside Down") {
            int counter = 1;
            for (int i = tiles.Length - 1; i >= 0; i--) {
                for (int j = tiles[i].Length - 1; j >= 0; j--) {
                    Tile tile = tiles[i][j];
                    tile.boardIndexX = i;
                    tile.boardIndexY = j;
                    tile.GetComponent<Button>().onClick.AddListener(() => GameManager.Instance.MoveTile(tile));
                    tile.SetValue(counter++);
                }
            }
            emptyTileIndex = (0, 0);
            tiles[0][0].GetComponent<Tile>().image.color = Color.clear;
            tiles[0][0].GetComponent<Tile>().valueTMPro.text = "";
        }
        else if (patternType == "Columns") {
            int counter = 1;
            for (int i = 0; i < tiles.Length; i++) {
                for (int j = 0; j < tiles[i].Length; j++) {
                    Tile tile = tiles[j][i];
                    tile.boardIndexX = j;
                    tile.boardIndexY = i;
                    tile.GetComponent<Button>().onClick.AddListener(() => GameManager.Instance.MoveTile(tile));
                    tile.SetValue(counter++);
                }
            }
            emptyTileIndex = (size - 1, size - 1);
            tiles[size - 1][size - 1].GetComponent<Tile>().image.color = Color.clear;
            tiles[size - 1][size - 1].GetComponent<Tile>().valueTMPro.text = "";
        }
        else if (patternType == "Snake") {
            int counter = 1;
            int direction = 1;
            for (int i = 0; i < tiles.Length; i++) {
                if (direction == 1) {
                    for (int j = 0; j < tiles[i].Length; j++) {
                        Tile tile = tiles[i][j];
                        tile.boardIndexX = i;
                        tile.boardIndexY = j;
                        tile.GetComponent<Button>().onClick.AddListener(() => GameManager.Instance.MoveTile(tile));
                        tile.SetValue(counter++);
                    }
                }
                else {
                    for (int j = tiles[i].Length - 1; j >= 0; j--) {
                        Tile tile = tiles[i][j];
                        tile.boardIndexX = i;
                        tile.boardIndexY = j;
                        tile.GetComponent<Button>().onClick.AddListener(() => GameManager.Instance.MoveTile(tile));
                        tile.SetValue(counter++);
                    }
                }

                direction = -direction;
            }

            emptyTileIndex = (size - 1, size - 1);
            tiles[size - 1][size - 1].GetComponent<Tile>().image.color = Color.clear;
            tiles[size - 1][size - 1].GetComponent<Tile>().valueTMPro.text = "";
        }
        else if (patternType == "Spiral") {
            int counter = 1;
            int top = 0, bottom = size - 1;
            int left = 0, right = size - 1;

            int lastRow = 0, lastCol = 0;

            while (top <= bottom && left <= right) {
                // → Right
                for (int i = left; i <= right; i++) {
                    Tile tile = tiles[top][i];
                    tile.boardIndexX = top;
                    tile.boardIndexY = i;
                    tile.GetComponent<Button>().onClick.AddListener(() => GameManager.Instance.MoveTile(tile));
                    tile.SetValue(counter++);
                    lastRow = top;
                    lastCol = i;
                }
                top++;

                // ↓ Down
                for (int i = top; i <= bottom; i++) {
                    Tile tile = tiles[i][right];
                    tile.boardIndexX = i;
                    tile.boardIndexY = right;
                    tile.GetComponent<Button>().onClick.AddListener(() => GameManager.Instance.MoveTile(tile));
                    tile.SetValue(counter++);
                    lastRow = i;
                    lastCol = right;
                }
                right--;

                // ← Left
                for (int i = right; i >= left; i--) {
                    Tile tile = tiles[bottom][i];
                    tile.boardIndexX = bottom;
                    tile.boardIndexY = i;
                    tile.GetComponent<Button>().onClick.AddListener(() => GameManager.Instance.MoveTile(tile));
                    tile.SetValue(counter++);
                    lastRow = bottom;
                    lastCol = i;
                }
                bottom--;

                // ↑ Up
                for (int i = bottom; i >= top; i--) {
                    Tile tile = tiles[i][left];
                    tile.boardIndexX = i;
                    tile.boardIndexY = left;
                    tile.GetComponent<Button>().onClick.AddListener(() => GameManager.Instance.MoveTile(tile));
                    tile.SetValue(counter++);
                    lastRow = i;
                    lastCol = left;
                }
                left++;
            }

            // Clear the last-filled tile (e.g., tile 36 in 6x6)
            emptyTileIndex = (lastRow, lastCol);
            Tile lastTile = tiles[lastRow][lastCol].GetComponent<Tile>();
            lastTile.image.color = Color.clear;
            lastTile.valueTMPro.text = "";
        }
        if (GameInfoStaticData.userImage != null) {
            AddImagePartsToList(GameInfoStaticData.userImage, size, 1000/ size);
            AssignImagePartsToTiles();
        }
    }
    
    private void InstantiateVisual() {
        for (int i = 0; i < tiles.Length; i++) {
            tiles[i] = new Tile[GameInfoStaticData.gridSize];
            for (int j = 0; j < tiles[i].Length; j++) {
                GameObject tileGO = Instantiate(tileButton, gameObject.transform);
                Tile tile = tileGO.GetComponent<Tile>();
                tile.SetValue(-1);
                tiles[i][j] = tile;
            }
        }
    }
    
    private void AddImagePartsToList(Texture2D image, int gridSize, int tileDimention) {
        int tileCount = gridSize * gridSize; // e.g., 4x4 = 16
        int tileWidth = tileDimention;  // example tile width in pixels, must be square
        int tileHeight = tileDimention; // same as tileWidth to keep tiles square

        int finalSize = tileWidth * gridSize; // total square image size

        // Step 1: Rescale original image into a square texture of finalSize x finalSize
        Texture2D scaledTexture = new Texture2D(finalSize, finalSize);

        // Resize with non-uniform scaling to stretch/compress image into square
        // Use GetPixels with bilinear sampling for smooth scaling
        for (int y = 0; y < finalSize; y++)
        {
            for (int x = 0; x < finalSize; x++)
            {
                // Map x,y in scaledTexture to u,v in original image
                float u = (float)x / finalSize;
                float v = (float)y / finalSize;

                // Sample the original image pixel using bilinear interpolation
                Color color = image.GetPixelBilinear(u, v);
                scaledTexture.SetPixel(x, y, color);
            }
        }
        scaledTexture.Apply();
        
        // Now slice and assign to tiles[x, y]
        for (int i = 0; i < gridSize; i++)       // rows (top to bottom in tile array)
        {
            for (int j = 0; j < gridSize; j++)   // columns (left to right)
            {
                // Flip the y coordinate to account for texture origin
                int flippedY = (gridSize - 1 - i) * tileWidth;

                Texture2D tileTexture = new Texture2D(tileWidth, tileWidth);
                tileTexture.filterMode = FilterMode.Point;
                tileTexture.wrapMode = TextureWrapMode.Clamp;
                tileTexture.SetPixels(scaledTexture.GetPixels(j * tileWidth, flippedY, tileWidth, tileWidth));
                tileTexture.Apply();
                tileSprites.Add(Sprite.Create(
                    tileTexture,
                    new Rect(0, 0, tileWidth, tileWidth),
                    new Vector2(0.5f, 0.5f)
                ));
            }
        }
    }

    private void AssignImagePartsToTiles() {
        for (int i = 0; i < tiles.Length; i++) {
            for (int j = 0; j < tiles[i].Length; j++) {
                tiles[i][j].image.sprite = tileSprites[tiles[i][j].value - 1];
                tiles[i][j].valueTMPro.text = "";
            }
        }
    }
    
    public IEnumerator ShuffleBoard() {
        yield return new WaitForSeconds(0.1f);
        float oldTileAnimationSpeed = tileAnimationSpeed;
        tileAnimationSpeed = 0.05f;
        int size = GameInfoStaticData.gridSize;
        bool moveHorizontally = true;
        for (int i = 0; i < 50; i++) {
            int randomIndexX = 0;
            int randomIndexY = 0;
            while (true) {
                if (moveHorizontally) {
                    randomIndexY = Random.Range(0, size);
                    randomIndexX = emptyTileIndex.Item1;
                    if (randomIndexY != emptyTileIndex.Item2) {
                        moveHorizontally = !moveHorizontally;
                        break;
                    }
                }
                else {
                    randomIndexX = Random.Range(0, size);
                    randomIndexY = emptyTileIndex.Item2;
                    if (randomIndexX != emptyTileIndex.Item1) {
                        moveHorizontally = !moveHorizontally;
                        break;
                    }
                }
            }
            // Apply the move using randomIndexX and randomIndexY

            Tile randomTile = tiles[randomIndexX][randomIndexY];
            yield return StartCoroutine(MoveTile(randomTile));
        }
        tileAnimationSpeed = oldTileAnimationSpeed;
    }
    
    private IEnumerator SwapTwoTiles(GameObject buttonA, GameObject buttonB) {
        RectTransform rectA = buttonA.GetComponent<RectTransform>();
        RectTransform rectB = buttonB.GetComponent<RectTransform>();

        
        Vector3 startPosA = rectA.localPosition;
        Vector3 startPosB = rectB.localPosition;
        AudioManager.Instance.sfxAudioSource.Play();
        float time = 0f;
        while (time < tileAnimationSpeed) {
            time += Time.deltaTime;
            float t = time / tileAnimationSpeed;
            rectA.localPosition = Vector3.Lerp(startPosA, startPosB, t);
            rectB.localPosition = Vector3.Lerp(startPosB, startPosA, t);
            yield return null;
        }

        // Snap to final position
        rectA.localPosition = startPosB;
        rectB.localPosition = startPosA;

        // Swap sibling index to make the grid layout reflect the swap
        int indexA = rectA.GetSiblingIndex();
        int indexB = rectB.GetSiblingIndex();

        rectA.SetSiblingIndex(indexB);
        rectB.SetSiblingIndex(indexA);
    }

    public IEnumerator MoveTile(Tile tile) {
        if (playingMove) yield break;
        playingMove = true;
        
        // x and y coordinate of current tile
        int x = tile.boardIndexX;
        int y = tile.boardIndexY;

        // x and y coordinate of empty tile
        int emptyTileX = emptyTileIndex.Item1;
        int emptyTileY = emptyTileIndex.Item2;

        // Empty tile and current tile in same row
        if (x == emptyTileX) {
            // Empty tile is on the right
            if (y < emptyTileY) {
                for (int i = emptyTileY; i > y; i--) {
                    yield return StartCoroutine(SwapTwoTiles(tiles[x][i].gameObject, tiles[x][i - 1].gameObject));
                    tiles[x][i].boardIndexY = i - 1;
                    tiles[x][i - 1].boardIndexY = i;
                    (tiles[x][i], tiles[x][i - 1]) = (tiles[x][i - 1], tiles[x][i]);
                }
            }
            // Empty tile is on the left
            else {
                for (int i = emptyTileY; i < y; i++) {
                    yield return StartCoroutine(SwapTwoTiles(tiles[x][i].gameObject, tiles[x][i + 1].gameObject));
                    tiles[x][i].boardIndexY = i + 1;
                    tiles[x][i + 1].boardIndexY = i;
                    (tiles[x][i], tiles[x][i + 1]) = (tiles[x][i + 1], tiles[x][i]);
                }
            }
            emptyTileIndex = (x, y);
            GameManager.Instance.timerAndSteps.UpdateSteps();
        }
        // Empty tile and current tile in same column
        else if (y == emptyTileY) {
            // Empty tile is on down
            if (x < emptyTileX) {
                for (int i = emptyTileX; i > x; i--) {
                    yield return StartCoroutine(SwapTwoTiles(tiles[i][y].gameObject, tiles[i - 1][y].gameObject));
                    tiles[i][y].boardIndexX = i - 1;
                    tiles[i - 1][y].boardIndexX = i;
                    (tiles[i][y], tiles[i - 1][y]) = (tiles[i - 1][y], tiles[i][y]);
                }
            }
            // Empty tile is on up
            else {
                for (int i = emptyTileX; i < x; i++) {
                    yield return StartCoroutine(SwapTwoTiles(tiles[i][y].gameObject, tiles[i + 1][y].gameObject));
                    tiles[i][y].boardIndexX = i + 1;
                    tiles[i + 1][y].boardIndexX = i;
                    (tiles[i][y], tiles[i + 1][y]) = (tiles[i + 1][y], tiles[i][y]);
                }
            }
            emptyTileIndex = (x, y);
            GameManager.Instance.timerAndSteps.UpdateSteps();
        }
        playingMove = false;
    }
    
}
