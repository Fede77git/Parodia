using System.Collections;
using UnityEngine;
using TMPro;

public class DialogoJefe : MonoBehaviour
{
    public GameObject burbujaVisual;
    public TextMeshProUGUI textoUI;

    private void Start()
    {
        HablarInicioDeRonda();
    }

    public void HablarInicioDeRonda()
    {
        if (burbujaVisual != null && textoUI != null)
        {
            StartCoroutine(MostrarDialogo());
        }
    }

    private IEnumerator MostrarDialogo()
    {
        int ronda = DatosPartidaManager.Instance != null ? DatosPartidaManager.Instance.rondaActual : 1;

        if (ronda >= 5)
        {
            textoUI.text = "Earn your life. Zero points is a death sentence";
        }
        else
        {
            textoUI.text = "First one to score 5 wins";
        }

        burbujaVisual.SetActive(true);
        yield return new WaitForSeconds(5f);
        burbujaVisual.SetActive(false);
    }
}
