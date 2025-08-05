using UnityEngine;
using UnityEngine.EventSystems;

public class GridCursorEvents : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private CustomCursorManager cursorManager;

    void Start()
    {
        // Finds the CursorManager in the scene
        cursorManager = FindObjectOfType<CustomCursorManager>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (cursorManager != null)
            cursorManager.OnButtonPointerEnter();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (cursorManager != null)
            cursorManager.OnButtonPointerExit();
    }
}