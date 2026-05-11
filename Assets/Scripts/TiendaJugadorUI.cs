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

    private void Update()
    {
        if (DatosPartidaManager.Instance == null) return;

        DatosJugador datos = DatosPartidaManager.Instance.jugadores[playerID];
        
        if (textoMonedas != null) textoMonedas.text = "Monedas: " + datos.puntosMoneda;
        
        ActualizarTexto(textoVelocidad, "Velocidad", datos.nivelVelocidad);
        ActualizarTexto(textoTamano, "Tamaño", datos.nivelTamano);
        ActualizarTexto(textoDash, "Dash", datos.nivelCooldownDash);
        ActualizarTexto(textoTiro, "Tiro", datos.nivelCooldownTiro);
    }

    private void ActualizarTexto(TextMeshProUGUI texto, string nombre, int nivel)
    {
        if (texto == null || tiendaManager == null) return;
        int costo = Mathf.FloorToInt(tiendaManager.costoBase * Mathf.Pow(tiendaManager.multiplicadorCosto, nivel - 1));
        texto.text = nombre + " (Lv" + nivel + ") - $" + costo;
    }
}
