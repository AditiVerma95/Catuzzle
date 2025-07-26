using System;
using UnityEngine;
using UnityEngine.UI;

public class PatternBoard : MonoBehaviour {
    private GridLayoutGroup gridLayoutGroup;
    [SerializeField] private GameObject tilePrefab;
    
    public PatternTile[][] pattern;

    private void Start() {
        gridLayoutGroup = GetComponent<GridLayoutGroup>();
    }
    
    public void RemoveChildren() {
        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }
    }

    public void GeneratePattern(int size, string patternType) {
        // Setting Grid layout properties
        RemoveChildren();
        gridLayoutGroup = GetComponent<GridLayoutGroup>();
        gridLayoutGroup.cellSize = new Vector2(500 / size, 500 / size);
        gridLayoutGroup.constraintCount = size;
        
        pattern = new PatternTile[GameInfoStaticData.gridSize][];
        
        InstantiateVisual();
        
        if (patternType == "Default") {
            int counter = 1;
            for (int i = 0; i < pattern.Length; i++) {
                for (int j = 0; j < pattern[i].Length; j++) {
                    pattern[i][j].SetValue(counter++);
                }
            }
            pattern[size - 1][size - 1].GetComponent<PatternTile>().image.color = Color.clear;
            pattern[size - 1][size - 1].GetComponent<PatternTile>().valueTMPro.text = "";
        }
        else if (patternType == "Upside Down") {
            int counter = 1;
            for (int i = pattern.Length - 1; i >= 0; i--) {
                for (int j = pattern[i].Length - 1; j >= 0; j--) {
                    pattern[i][j].SetValue(counter++);
                }
            }
            pattern[0][0].GetComponent<PatternTile>().image.color = Color.clear;
            pattern[0][0].GetComponent<PatternTile>().valueTMPro.text = "";
        }
        else if (patternType == "Columns") {
            int counter = 1;
            for (int i = 0; i < pattern.Length; i++) {
                for (int j = 0; j < pattern[i].Length; j++) {
                    pattern[j][i].SetValue(counter++);
                }
            }
            pattern[size - 1][size - 1].GetComponent<PatternTile>().image.color = Color.clear;
            pattern[size - 1][size - 1].GetComponent<PatternTile>().valueTMPro.text = "";
        }
        else if (patternType == "Snake") {
            int counter = 1;
            int direction = 1;
            for (int i = 0; i < pattern.Length; i++) {
                if (direction == 1) {
                    for (int j = 0; j < pattern[i].Length; j++) {
                        pattern[i][j].SetValue(counter++);
                    }
                }
                else {
                    for (int j = pattern[i].Length - 1; j >= 0; j--) {
                        pattern[i][j].SetValue(counter++);
                    }
                }

                direction = -direction;
            }
            pattern[size - 1][size - 1].GetComponent<PatternTile>().image.color = Color.clear;
            pattern[size - 1][size - 1].GetComponent<PatternTile>().valueTMPro.text = "";
        }
        else if (patternType == "Spiral") {
            int counter = 1;
            int top = 0, bottom = size - 1;
            int left = 0, right = size - 1;

            int lastRow = 0, lastCol = 0;

            while (top <= bottom && left <= right) {
                // → Right
                for (int i = left; i <= right; i++) {
                    pattern[top][i].SetValue(counter++);
                    lastRow = top;
                    lastCol = i;
                }
                top++;

                // ↓ Down
                for (int i = top; i <= bottom; i++) {
                    pattern[i][right].SetValue(counter++);
                    lastRow = i;
                    lastCol = right;
                }
                right--;

                // ← Left
                for (int i = right; i >= left; i--) {
                    pattern[bottom][i].SetValue(counter++);
                    lastRow = bottom;
                    lastCol = i;
                }
                bottom--;

                // ↑ Up
                for (int i = bottom; i >= top; i--) {
                    pattern[i][left].SetValue(counter++);
                    lastRow = i;
                    lastCol = left;
                }
                left++;
            }

            // Clear the last-filled tile (e.g., tile 36 in 6x6)
            PatternTile lastTile = pattern[lastRow][lastCol].GetComponent<PatternTile>();
            lastTile.image.color = Color.clear;
            lastTile.valueTMPro.text = "";
        }

        if (GameInfoStaticData.userImage != null) {
            AssignImageParts(GameInfoStaticData.userImage, size, (int) 500f/ size);
        }
    }

    private void InstantiateVisual() {
        for (int i = 0; i < pattern.Length; i++) {
            pattern[i] = new PatternTile[GameInfoStaticData.gridSize];
            for (int j = 0; j < pattern[i].Length; j++) {
                GameObject tileGO = Instantiate(tilePrefab, gameObject.transform);
                PatternTile tile = tileGO.GetComponent<PatternTile>();
                tile.SetValue(-1);
                pattern[i][j] = tile;
            }
        }
    }

    private void AssignImageParts(Texture2D image, int gridSize, int tileDimention) {
        int tileCount = gridSize * gridSize; // e.g., 4x4 = 16
        int tileWidth = tileDimention; // example tile width in pixels, must be square
        int tileHeight = tileDimention; // same as tileWidth to keep tiles square

        int finalSize = tileWidth * gridSize; // total square image size

        // Step 1: Rescale original image into a square texture of finalSize x finalSize
        Texture2D scaledTexture = new Texture2D(finalSize, finalSize);

        // Resize with non-uniform scaling to stretch/compress image into square
        // Use GetPixels with bilinear sampling for smooth scaling
        for (int y = 0; y < finalSize; y++) {
            for (int x = 0; x < finalSize; x++) {
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
        for (int i = 0; i < gridSize; i++) // rows (top to bottom in tile array)
        {
            for (int j = 0; j < gridSize; j++) // columns (left to right)
            {
                // Flip the y coordinate to account for texture origin
                int flippedY = (gridSize - 1 - i) * tileWidth;

                Texture2D tileTexture = new Texture2D(tileWidth, tileWidth);
                tileTexture.filterMode = FilterMode.Point;
                tileTexture.wrapMode = TextureWrapMode.Clamp;
                tileTexture.SetPixels(scaledTexture.GetPixels(j * tileWidth, flippedY, tileWidth, tileWidth));
                tileTexture.Apply();

                pattern[i][j].image.sprite = Sprite.Create(
                    tileTexture,
                    new Rect(0, 0, tileWidth, tileWidth),
                    new Vector2(0.5f, 0.5f)
                );
                pattern[i][j].valueTMPro.text = "";
            }
        }
    }
}
