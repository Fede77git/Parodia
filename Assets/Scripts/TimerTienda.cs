using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TimerTienda : MonoBehaviour
{
    public float tiempoRestante = 30f;
    public string nombreSiguienteEscena;
    public TextMeshProUGUI textoTemporizador;

    private void Update()
    {
        tiempoRestante -= Time.deltaTime;

        if (textoTemporizador != null)
        {
            int segundosMostrados = Mathf.CeilToInt(Mathf.Max(0, tiempoRestante));
            textoTemporizador.text = segundosMostrados.ToString();
        }

        if (tiempoRestante <= 0)
        {
            SceneManager.LoadScene(nombreSiguienteEscena);
        }
    }
}
