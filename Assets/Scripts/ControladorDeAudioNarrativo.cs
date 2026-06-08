using UnityEngine;
using UnityEngine.Audio;

public class ControladorDeAudioNarrativo : MonoBehaviour
{
    public AudioMixerSnapshot fase1_Normal;
    public AudioMixerSnapshot fase2_Ahogado;
    public AudioMixerSnapshot fase3_Pesadilla;
    public AudioMixerSnapshot faseFinal_Silencio;

    public float tiempoDeTransicion = 3.0f;

    public void ActualizarAudioPorRonda(int numeroDeRonda)
    {
        if (numeroDeRonda == 1 || numeroDeRonda == 2)
        {
            if (fase1_Normal != null) fase1_Normal.TransitionTo(tiempoDeTransicion);
        }
        else if (numeroDeRonda == 3 || numeroDeRonda == 4)
        {
            if (fase2_Ahogado != null) fase2_Ahogado.TransitionTo(tiempoDeTransicion);
        }
        else if (numeroDeRonda >= 5)
        {
            if (fase3_Pesadilla != null) fase3_Pesadilla.TransitionTo(tiempoDeTransicion);
        }
    }

    public void ActivarSilencioDeHuelga()
    {
        if (faseFinal_Silencio != null)
        {
            faseFinal_Silencio.TransitionTo(tiempoDeTransicion + 4.0f);
        }
    }
}
