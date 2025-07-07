using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class TileController : MonoBehaviour, IPointerClickHandler {
    public Vector2Int gridPosition;
    public Vector2Int originalPosition;

    private Image tileImage;
    private TextMeshProUGUI tileText;

    private void Awake() {
        tileImage = GetComponent<Image>();
        tileText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void Initialize(Vector2Int pos) {
        gridPosition = pos;
        originalPosition = pos;
    }

    public void OnPointerClick(PointerEventData eventData) {
        GridManager.Instance.TryMoveTile(this);
    }

    public void SetFaded(bool faded) {
        float alpha = faded ? 0.5f : 1f;

        if (tileImage != null) {
            Color imgColor = tileImage.color;
            imgColor.a = alpha;
            tileImage.color = imgColor;
        }

        if (tileText != null) {
            Color textColor = tileText.color;
            textColor.a = alpha;
            tileText.color = textColor;
        }
    }
}