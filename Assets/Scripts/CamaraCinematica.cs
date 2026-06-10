using System.Collections;
using UnityEngine;

public class CamaraCinematica : MonoBehaviour
{
    public Transform objetivoGato;
    public float fovZoom = 30f;
    public float velocidadZoom = 2.0f;
    public float offsetZ = 5f;
    public float offsetY = 2f;

    private Vector3 posicionOriginal;
    private Quaternion rotacionOriginal;
    private float fovOriginal;
    private Camera camaraPrincipal;
    private Coroutine corrutinaActual;

    private void Awake()
    {
        camaraPrincipal = Camera.main;
        if (camaraPrincipal != null)
        {
            posicionOriginal = camaraPrincipal.transform.position;
            rotacionOriginal = camaraPrincipal.transform.rotation;
            fovOriginal = camaraPrincipal.fieldOfView;
        }
    }

    public void HacerZoomAlJefe()
    {
        if (camaraPrincipal == null || objetivoGato == null) return;

        if (corrutinaActual != null)
        {
            StopCoroutine(corrutinaActual);
        }

        Vector3 puntoMira = objetivoGato.position + Vector3.up * offsetY;
        Vector3 posicionObjetivo = puntoMira + objetivoGato.forward * offsetZ;
        Quaternion rotacionObjetivo = Quaternion.LookRotation(puntoMira - posicionObjetivo);

        corrutinaActual = StartCoroutine(MoverCamara(posicionObjetivo, rotacionObjetivo, fovZoom));
    }

    public void ResetearCamara()
    {
        if (camaraPrincipal == null) return;

        if (corrutinaActual != null)
        {
            StopCoroutine(corrutinaActual);
        }

        corrutinaActual = StartCoroutine(MoverCamara(posicionOriginal, rotacionOriginal, fovOriginal));
    }

    private IEnumerator MoverCamara(Vector3 posicionDestino, Quaternion rotacionDestino, float fovDestino)
    {
        while (Vector3.Distance(camaraPrincipal.transform.position, posicionDestino) > 0.01f || 
               Quaternion.Angle(camaraPrincipal.transform.rotation, rotacionDestino) > 0.1f ||
               Mathf.Abs(camaraPrincipal.fieldOfView - fovDestino) > 0.01f)
        {
            camaraPrincipal.transform.position = Vector3.Lerp(camaraPrincipal.transform.position, posicionDestino, Time.unscaledDeltaTime * velocidadZoom);
            camaraPrincipal.transform.rotation = Quaternion.Slerp(camaraPrincipal.transform.rotation, rotacionDestino, Time.unscaledDeltaTime * velocidadZoom);
            camaraPrincipal.fieldOfView = Mathf.Lerp(camaraPrincipal.fieldOfView, fovDestino, Time.unscaledDeltaTime * velocidadZoom);
            yield return null;
        }

        camaraPrincipal.transform.position = posicionDestino;
        camaraPrincipal.transform.rotation = rotacionDestino;
        camaraPrincipal.fieldOfView = fovDestino;
    }
}
