using UnityEngine;
using TMPro;

public class TiendaJugadorUI : MonoBehaviour
{
    public int playerID;
    public TiendaManager tiendaManager;
    public TextMeshProUGUI textoMonedas;
    public TextMeshProUGUI textoVelocidad;
    public TextMeshProUGUI textoTamano;
    public TextMeshProUGUI textoDash;
    public TextMeshProUGUI textoTiro;

    public Transform modeloPollo;
    public float multiplicadorEscalaVisual = 0.2f;
    private Vector3 escalaBaseVisual;

    private void Start()
    {
        if (DatosPartidaManager.Instance != null && DatosPartidaManager.Instance.jugadores[playerID].estaEliminado)
        {
            if (modeloPollo != null) modeloPollo.gameObject.SetActive(false);
            gameObject.SetActive(false);
            return;
        }

        if (modeloPollo != null)
        {
            escalaBaseVisual = modeloPollo.localScale;
        }
    }

    private void Update()
    {
        if (DatosPartidaManager.Instance == null) return;

        DatosJugador datos = DatosPartidaManager.Instance.jugadores[playerID];
        
        if (textoMonedas != null) textoMonedas.text = "Coins: " + datos.puntosMoneda;
        
        ActualizarTexto(textoVelocidad, "Speed", datos.nivelVelocidad);
        ActualizarTexto(textoTamano, "Size", datos.nivelTamano);
        ActualizarTexto(textoDash, "Dash", datos.nivelCooldownDash);
        ActualizarTexto(textoTiro, "Shoot", datos.nivelCooldownTiro);

        if (modeloPollo != null && escalaBaseVisual != Vector3.zero)
        {
            float aumentoProporcional = 1f + ((datos.nivelTamano - 1) * multiplicadorEscalaVisual);
            modeloPollo.localScale = escalaBaseVisual * aumentoProporcional;
        }
    }

    private void ActualizarTexto(TextMeshProUGUI texto, string nombre, int nivel)
    {
        if (texto == null || tiendaManager == null) return;
        int costo = Mathf.FloorToInt(tiendaManager.costoBase * Mathf.Pow(tiendaManager.multiplicadorCosto, nivel - 1));
        texto.text = nombre + " (Lv" + nivel + ") - $" + costo;
    }
}
