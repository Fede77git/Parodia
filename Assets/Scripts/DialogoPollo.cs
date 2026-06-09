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

    private Coroutine rutinaActual;

    private void Start()
    {
        if (burbujaVisual != null) burbujaVisual.SetActive(false);
        StartCoroutine(BuclePensamientos());
    }

    private IEnumerator BuclePensamientos()
    {
        while (true)
        {
            float esperaAleatoria = Random.Range(10f, 20f);
            yield return new WaitForSeconds(esperaAleatoria);

            if (dialogosDesbloqueados && dialogosPensando != null && dialogosPensando.Length > 0 && burbujaVisual != null && !burbujaVisual.activeSelf)
            {
                string textoElegido = dialogosPensando[Random.Range(0, dialogosPensando.Length)];
                rutinaActual = StartCoroutine(MostrarDialogo(textoElegido, 3f));
            }
        }
    }

    public void RecibirGolpe()
    {
        if (dialogosDesbloqueados && dialogosGolpe != null && dialogosGolpe.Length > 0 && burbujaVisual != null && textoUI != null)
        {
            if (rutinaActual != null)
            {
                StopCoroutine(rutinaActual);
            }

            string textoElegido = dialogosGolpe[Random.Range(0, dialogosGolpe.Length)];
            rutinaActual = StartCoroutine(MostrarDialogo(textoElegido, 2f));
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
