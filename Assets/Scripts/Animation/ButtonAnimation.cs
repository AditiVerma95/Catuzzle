using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 originalScale;
    private Vector3 targetScale;
    private Coroutine scaleCoroutine;

    [SerializeField] private float scaleFactor = 1.1f;
    [SerializeField] private float duration = 0.2f;

    private void Start()
    {
        originalScale = transform.localScale;
        targetScale = originalScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        AnimateScale(originalScale * scaleFactor);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        AnimateScale(originalScale);
    }

    private void AnimateScale(Vector3 newScale)
    {
        if (scaleCoroutine != null)
            StopCoroutine(scaleCoroutine);

        scaleCoroutine = StartCoroutine(ScaleTo(newScale, duration));
    }

    private System.Collections.IEnumerator ScaleTo(Vector3 target, float time)
    {
        Vector3 startScale = transform.localScale;
        float timer = 0f;

        while (timer < time)
        {
            timer += Time.deltaTime;
            float t = timer / time;
            t = Mathf.SmoothStep(0, 1, t); // For smoother ease-in-out effect
            transform.localScale = Vector3.Lerp(startScale, target, t);
            yield return null;
        }

        transform.localScale = target;
    }
}