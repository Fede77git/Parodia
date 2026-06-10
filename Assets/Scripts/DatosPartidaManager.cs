using UnityEngine;

[System.Serializable]
public class DatosJugador
{
    public int estrellas;
    public int puntosMoneda;
    public int nivelVelocidad = 1;
    public int nivelTamano = 1;
    public int nivelCooldownTiro = 1;
    public int nivelCooldownDash = 1;
    public bool estaEliminado = false;
}

public enum TipoEstadistica
{
    Velocidad,
    Tamano,
    CooldownTiro,
    CooldownDash
}

public class DatosPartidaManager : MonoBehaviour
{
    public static DatosPartidaManager Instance { get; private set; }

    public DatosJugador[] jugadores = new DatosJugador[4];
    public int rondaActual = 1;
    public string mensajeFinalAlternativo = "";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        for (int i = 0; i < 4; i++)
        {
            if (jugadores[i] == null)
            {
                jugadores[i] = new DatosJugador();
            }
        }
    }

    public void SumarEstrellas(int playerID, int cantidad)
    {
        if (playerID >= 0 && playerID < 4)
        {
            jugadores[playerID].estrellas += cantidad;
        }
    }

    public void SumarMonedas(int playerID, int cantidad)
    {
        if (playerID >= 0 && playerID < 4)
        {
            jugadores[playerID].puntosMoneda += cantidad;
        }
    }

    public void AumentarEstadistica(int playerID, TipoEstadistica estadistica)
    {
        if (playerID >= 0 && playerID < 4)
        {
            switch (estadistica)
            {
                case TipoEstadistica.Velocidad:
                    jugadores[playerID].nivelVelocidad++;
                    break;
                case TipoEstadistica.Tamano:
                    jugadores[playerID].nivelTamano++;
                    break;
                case TipoEstadistica.CooldownTiro:
                    jugadores[playerID].nivelCooldownTiro++;
                    break;
                case TipoEstadistica.CooldownDash:
                    jugadores[playerID].nivelCooldownDash++;
                    break;
            }
        }
    }

    public void AvanzarRonda()
    {
        rondaActual++;
    }
}
