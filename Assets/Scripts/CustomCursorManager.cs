using UnityEngine;

public class CustomCursorManager : MonoBehaviour
{
    public Texture2D normalCupCursor;
    public Texture2D catInCupCursor;
    public Vector2 hotSpot = Vector2.zero;

    void Start()
    {
        // Set default cursor to the cup
        Cursor.SetCursor(normalCupCursor, hotSpot, CursorMode.Auto);
    }

    public void OnButtonPointerEnter()
    {
        Cursor.SetCursor(catInCupCursor, hotSpot, CursorMode.Auto);
    }

    public void OnButtonPointerExit()
    {
        Cursor.SetCursor(normalCupCursor, hotSpot, CursorMode.Auto);
    }
}