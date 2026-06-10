using System.Collections;
using UnityEngine;
using TMPro;

public class DialogoPollo : MonoBehaviour
{
    public GameObject burbujaVisual;
    public TextMeshProUGUI textoUI;

    public string[] dialogosPensando;
    public string[] dialogosGolpe;

    public bool dialogosDesbloqueados = false;
    public Vector3 offsetRotacion;

    private Coroutine rutinaActual;

    private Vector3 escalaInicialBurbuja;

    private void Start()
    {
        if (burbujaVisual != null) 
        {
            escalaInicialBurbuja = burbujaVisual.transform.localScale;
            burbujaVisual.SetActive(false);
        }
    }

    private void OnEnable()
    {
        StartCoroutine(BuclePensamientos());
    }

    private void OnDisable()
    {
        if (burbujaVisual != null) burbujaVisual.SetActive(false);
        StopAllCoroutines();
    }

    private void LateUpdate()
    {
        if (burbujaVisual != null && burbujaVisual.activeSelf && Camera.main != null)
        {
            Vector3 rotacionCamara = Camera.main.transform.rotation.eulerAngles;
            burbujaVisual.transform.rotation = Quaternion.Euler(offsetRotacion.x, rotacionCamara.y + offsetRotacion.y, offsetRotacion.z);
            Vector3 pScale = transform.localScale;
            burbujaVisual.transform.localScale = new Vector3(escalaInicialBurbuja.x / pScale.x, escalaInicialBurbuja.y / pScale.y, escalaInicialBurbuja.z / pScale.z);
        }
    }

    private IEnumerator BuclePensamientos()
    {
        while (true)
        {
            float esperaAleatoria = Random.Range(8f, 15f);
            yield return new WaitForSeconds(esperaAleatoria);

            if (dialogosDesbloqueados && dialogosPensando != null && dialogosPensando.Length > 0 && burbujaVisual != null && !burbujaVisual.activeSelf)
            {
                string textoElegido = dialogosPensando[Random.Range(0, dialogosPensando.Length)];
                rutinaActual = StartCoroutine(MostrarDialogo(textoElegido, 5f));
            }
        }
    }

    public void RecibirGolpe()
    {
        if (Random.value > 0.6f) return;

        if (dialogosDesbloqueados && dialogosGolpe != null && dialogosGolpe.Length > 0 && burbujaVisual != null && textoUI != null)
        {
            if (rutinaActual != null)
            {
                StopCoroutine(rutinaActual);
            }

            string textoElegido = dialogosGolpe[Random.Range(0, dialogosGolpe.Length)];
            rutinaActual = StartCoroutine(MostrarDialogo(textoElegido, 4f));
        }
    }

    private IEnumerator MostrarDialogo(string mensaje, float tiempo)
    {
        textoUI.text = mensaje;
        burbujaVisual.SetActive(true);
        yield return new WaitForSeconds(tiempo);
        burbujaVisual.SetActive(false);
    }
}
