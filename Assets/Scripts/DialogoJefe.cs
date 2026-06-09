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
        textoUI.text = "First one to score 5 wins";
        burbujaVisual.SetActive(true);
        yield return new WaitForSeconds(5f);
        burbujaVisual.SetActive(false);
    }
}
