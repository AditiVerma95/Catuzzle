using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager Instance;

    public Tile holeTile;

    void Awake()
    {
        Instance = this;
    }

    public void TrySwap(Tile clickedTile)
    {
        Vector2Int a = clickedTile.gridPosition;
        Vector2Int b = holeTile.gridPosition;

        int dx = Mathf.Abs(a.x - b.x);
        int dy = Mathf.Abs(a.y - b.y);

        if ((dx == 1 && dy == 0) || (dx == 0 && dy == 1))
        {
            Swap(clickedTile, holeTile);
        }
    }

    void Swap(Tile a, Tile b)
    {
        // Swap grid positions
        (a.gridPosition, b.gridPosition) = (b.gridPosition, a.gridPosition);

        // Swap world positions
        Vector3 tempPos = a.transform.localPosition;
        a.transform.localPosition = b.transform.localPosition;
        b.transform.localPosition = tempPos;

        // Update hole reference
        holeTile = a == holeTile ? b : a;
    }
}