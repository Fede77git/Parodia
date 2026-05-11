using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class TiendaManager : MonoBehaviour
{
    public int playerID;
    public int costoBase = 10;
    public float multiplicadorCosto = 1.5f;

    public InputActionReference comprarVelocidadAction;
    public InputActionReference comprarTamanoAction;
    public InputActionReference comprarDashAction;
    public InputActionReference comprarTiroAction;

    public UnityEvent OnCompraExitosa;
    public UnityEvent OnCompraFallida;

    private void OnEnable()
    {
        if (comprarVelocidadAction != null)
        {
            comprarVelocidadAction.action.performed += OnComprarVelocidad;
            comprarVelocidadAction.action.Enable();
        }
        if (comprarTamanoAction != null)
        {
            comprarTamanoAction.action.performed += OnComprarTamano;
            comprarTamanoAction.action.Enable();
        }
        if (comprarDashAction != null)
        {
            comprarDashAction.action.performed += OnComprarDash;
            comprarDashAction.action.Enable();
        }
        if (comprarTiroAction != null)
        {
            comprarTiroAction.action.performed += OnComprarTiro;
            comprarTiroAction.action.Enable();
        }
    }

    private void OnDisable()
    {
        if (comprarVelocidadAction != null)
        {
            comprarVelocidadAction.action.performed -= OnComprarVelocidad;
        }
        if (comprarTamanoAction != null)
        {
            comprarTamanoAction.action.performed -= OnComprarTamano;
        }
        if (comprarDashAction != null)
        {
            comprarDashAction.action.performed -= OnComprarDash;
        }
        if (comprarTiroAction != null)
        {
            comprarTiroAction.action.performed -= OnComprarTiro;
        }
    }

    private void OnComprarVelocidad(InputAction.CallbackContext context) => IntentarComprar(playerID, "Velocidad");
    private void OnComprarTamano(InputAction.CallbackContext context) => IntentarComprar(playerID, "Tamano");
    private void OnComprarDash(InputAction.CallbackContext context) => IntentarComprar(playerID, "CooldownDash");
    private void OnComprarTiro(InputAction.CallbackContext context) => IntentarComprar(playerID, "CooldownTiro");

    public void IntentarComprar(int pID, string tipoMejora)
    {
        if (DatosPartidaManager.Instance == null) return;

        DatosJugador datos = DatosPartidaManager.Instance.jugadores[pID];
        int nivelActual = ObtenerNivelActual(datos, tipoMejora);
        int costoActual = Mathf.FloorToInt(costoBase * Mathf.Pow(multiplicadorCosto, nivelActual - 1));

        if (datos.puntosMoneda >= costoActual)
        {
            DatosPartidaManager.Instance.SumarMonedas(pID, -costoActual);
            AplicarMejora(pID, tipoMejora);
            OnCompraExitosa?.Invoke();
        }
        else
        {
            OnCompraFallida?.Invoke();
        }
    }

    private int ObtenerNivelActual(DatosJugador datos, string tipoMejora)
    {
        switch (tipoMejora)
        {
            case "Velocidad": return datos.nivelVelocidad;
            case "Tamano": return datos.nivelTamano;
            case "CooldownDash": return datos.nivelCooldownDash;
            case "CooldownTiro": return datos.nivelCooldownTiro;
            default: return 1;
        }
    }

    private void AplicarMejora(int pID, string tipoMejora)
    {
        switch (tipoMejora)
        {
            case "Velocidad": DatosPartidaManager.Instance.AumentarEstadistica(pID, TipoEstadistica.Velocidad); break;
            case "Tamano": DatosPartidaManager.Instance.AumentarEstadistica(pID, TipoEstadistica.Tamano); break;
            case "CooldownDash": DatosPartidaManager.Instance.AumentarEstadistica(pID, TipoEstadistica.CooldownDash); break;
            case "CooldownTiro": DatosPartidaManager.Instance.AumentarEstadistica(pID, TipoEstadistica.CooldownTiro); break;
        }
    }
}
