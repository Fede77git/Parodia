using System.Collections;
using UnityEngine;
using TMPro;

public class EfectoMaquinaDeEscribir : MonoBehaviour
{
    public TextMeshProUGUI textoUI;
    public string mensajeFinal = "The few stopped fighting. The masses were left without a circus.";
    public float velocidadEscritura = 0.05f;

    public AudioSource audioSource;
    public AudioClip sonidoTecla;

    public void DispararMensaje()
    {
        if (textoUI != null)
        {
            textoUI.text = "";
            StartCoroutine(EscribirTexto());
        }
    }

    private IEnumerator EscribirTexto()
    {
        foreach (char letra in mensajeFinal)
        {
            textoUI.text += letra;

            if (audioSource != null && sonidoTecla != null && letra != ' ')
            {
                audioSource.PlayOneShot(sonidoTecla);
            }

            yield return new WaitForSeconds(velocidadEscritura);
        }
    }
}
