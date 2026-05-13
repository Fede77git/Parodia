using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class WinnerSceneManager : MonoBehaviour
{
    public GameObject[] pollitosEscena;
    public TMP_Text winnerText;

    private void Start()
    {
        if (pollitosEscena != null)
        {
            foreach (GameObject pollito in pollitosEscena)
            {
                if (pollito != null) pollito.SetActive(false);
            }
        }

        int winnerID = -1;
        if (DatosPartidaManager.Instance != null)
        {
            for (int i = 0; i < 4; i++)
            {
                if (DatosPartidaManager.Instance.jugadores[i].estrellas >= 3)
                {
                    winnerID = i;
                    break;
                }
            }
        }

        if (winnerID != -1 && pollitosEscena != null && pollitosEscena.Length > winnerID)
        {
            GameObject pollitoGanador = pollitosEscena[winnerID];
            if (pollitoGanador != null)
            {
                pollitoGanador.SetActive(true);
                Animator anim = pollitoGanador.GetComponentInChildren<Animator>();
                if (anim != null)
                {
                    anim.SetTrigger("Dance");
                }
            }

            string[] colors = { "Green", "Purple", "Orange", "Red" };
            if (winnerText != null)
            {
                winnerText.text = colors[winnerID] + " Chicken is the ultimate CHAMPION!";
            }
        }

        if (DatosPartidaManager.Instance != null)
        {
            Destroy(DatosPartidaManager.Instance.gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
