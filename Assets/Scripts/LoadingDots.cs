using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // For UI.Text; if using TextMeshPro, add `using TMPro;`

public class LoadingDots : MonoBehaviour
{
    public TextMeshProUGUI loadingText; 
    public float dotSpeed = 0.5f; // Time between dot changes
    
    private bool loading = true;

    void Start()
    {
        StartCoroutine(AnimateLoading());
    }

    IEnumerator AnimateLoading()
    {
        string baseText = "Loading";
        int dotCount = 0;
        while (loading)
        {
            loadingText.text = baseText + new string('.', dotCount);
            dotCount = (dotCount + 1) % 4; // Cycles between 0–3 dots
            yield return new WaitForSeconds(dotSpeed);
        }
        loadingText.text = baseText; // Reset when done
    }

    // Call this when your loading is complete
    public void StopLoading()
    {
        loading = false;
    }
}
