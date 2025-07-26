using UnityEngine;
using UnityEngine.UI;

public class UIBackgroundScroll : MonoBehaviour {
    public RawImage rawImage;
    public Vector2 scrollSpeed = new Vector2(0.1f, 0); // Scroll left

    private Vector2 uvOffset = Vector2.zero;

    void Update() {
        uvOffset += scrollSpeed * Time.deltaTime;
        rawImage.uvRect = new Rect(uvOffset, rawImage.uvRect.size);
    }
}