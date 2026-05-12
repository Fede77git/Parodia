using UnityEngine;

public class AplicadorDeStats : MonoBehaviour
{
    public int playerID;

    public float velocidadBase = 5f;
    public float multiplicadorVelocidad = 1f;

    public Vector3 escalaBase = Vector3.one;
    public float multiplicadorEscala = 0.2f;
    public float masaBase = 1f;
    public float multiplicadorMasa = 0.5f;

    public float cooldownDashBase = 3f;
    public float reduccionCooldownDash = 0.25f;
    public float cooldownTiroBase = 2f;
    public float reduccionCooldownTiro = 0.2f;

    private void Start()
    {
        if (DatosPartidaManager.Instance == null) return;

        DatosJugador datos = DatosPartidaManager.Instance.jugadores[playerID];

        AplicarTamanoYPeso(datos.nivelTamano);
        AplicarStatsAScriptsExternos(datos.nivelVelocidad, datos.nivelCooldownDash, datos.nivelCooldownTiro);
    }

    private void AplicarTamanoYPeso(int nivel)
    {
        float aumento = (nivel - 1) * multiplicadorEscala;
        transform.localScale = escalaBase + new Vector3(aumento, aumento, aumento);

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.mass = masaBase + ((nivel - 1) * multiplicadorMasa);
        }
    }

    private void AplicarStatsAScriptsExternos(int nivelVelocidad, int nivelDash, int nivelTiro)
    {
        int nivelTamano = DatosPartidaManager.Instance.jugadores[playerID].nivelTamano;
        float bonoEscalaVisual = (nivelTamano - 1) * multiplicadorEscala * 3f;

        BasketController controlador = GetComponent<BasketController>();
        
        if (controlador != null)
        {
            controlador.MoveSpeed = velocidadBase + ((nivelVelocidad - 1) * multiplicadorVelocidad) + bonoEscalaVisual;
            controlador.dashCooldown = Mathf.Max(0.1f, cooldownDashBase - ((nivelDash - 1) * reduccionCooldownDash));
            controlador.chargeRate = cooldownTiroBase + ((nivelTiro - 1) * reduccionCooldownTiro);
        }
    }
}
