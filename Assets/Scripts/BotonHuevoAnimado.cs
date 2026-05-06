using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(AudioSource))]
public class BotonHuevoAnimado : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    private Vector3 originalScale;
    private Vector3 targetScale;
    public float scaleSpeed = 10f;
    public float hoverScaleMultiplier = 1.15f;
    public float clickScaleMultiplier = 0.9f;
    public float wiggleAngle = 5f;
    public float wiggleSpeed = 10f;

    public AudioClip clickSound;
    private AudioSource audioSource;
    private Quaternion originalRotation;

    void Start()
    {
        originalScale = transform.localScale;
        targetScale = originalScale;
        originalRotation = transform.localRotation;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * scaleSpeed);
        float angle = Mathf.Sin(Time.time * wiggleSpeed) * wiggleAngle;
        transform.localRotation = originalRotation * Quaternion.Euler(0, 0, angle);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetScale = originalScale * hoverScaleMultiplier;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetScale = originalScale;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.localScale = originalScale * clickScaleMultiplier;
        if (clickSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }
}
