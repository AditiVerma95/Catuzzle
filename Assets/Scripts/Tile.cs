using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public Vector2Int gridPosition;
    private Button button;
    private Image image;
    private Text label;

    void Start()
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();
        label = GetComponentInChildren<Text>();

        button.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        PuzzleManager.Instance.TrySwap(this);
    }

    public void SetText(string text)
    {
        if (label != null)
            label.text = text;
    }

    public void SetAsHole()
    {
        image = GetComponent<Image>();
        if (image != null) image.color = Color.black;
        SetText("");
    }
}