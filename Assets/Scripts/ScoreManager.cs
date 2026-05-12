using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public GameObject effectObj;
    
    public float respawnHeight = 15f;
    public float respawnAreaSize = 8f;
    public float respawnDelay = 1.0f;

    public TMP_Text scoreTextP1;
    public TMP_Text scoreTextP2;
    public TMP_Text scoreTextP3;
    public TMP_Text scoreTextP4;

    
    public GameObject[] estrellasP1;
    public GameObject[] estrellasP2;
    public GameObject[] estrellasP3;
    public GameObject[] estrellasP4;

    public TMP_Text winText;

    private int scoreP1 = 0;
    private int scoreP2 = 0;
    private int scoreP3 = 0;
    private int scoreP4 = 0;

    public static ScoreManager instance;
    public static bool isGameOver = false;
    public static bool isGameComplete = false;
    private float transitionTimer = 10f;
    private string baseWinMessage = "";

    public float winnerTax = 0.5f;

    AudioManager audioManager;

    private void Awake()
    {
        instance = this;
        isGameOver = false;
        isGameComplete = false;
        GameObject audioObj = GameObject.FindGameObjectWithTag("Audio");
        if(audioObj != null) audioManager = audioObj.GetComponent<AudioManager>();
    }

    private void Start()
    {
        if (effectObj != null) effectObj.SetActive(false);
        winText.text = "";
        UpdateUI();
        UpdateStarsUI();
    }

    private void UpdateStarsUI()
    {
        if (DatosPartidaManager.Instance == null) return;
        ActivarEstrellas(estrellasP1, DatosPartidaManager.Instance.jugadores[0].estrellas);
        ActivarEstrellas(estrellasP2, DatosPartidaManager.Instance.jugadores[1].estrellas);
        ActivarEstrellas(estrellasP3, DatosPartidaManager.Instance.jugadores[2].estrellas);
        ActivarEstrellas(estrellasP4, DatosPartidaManager.Instance.jugadores[3].estrellas);
    }

    private void ActivarEstrellas(GameObject[] arrayEstrellas, int cantidad)
    {
        if (arrayEstrellas == null) return;
        for (int i = 0; i < arrayEstrellas.Length; i++)
        {
            if (arrayEstrellas[i] != null) arrayEstrellas[i].SetActive(i < cantidad);
        }
    }

    private void Update()
    {
        if (isGameComplete)
        {
            if (winText != null)
            {
                winText.text = baseWinMessage + "\nCHAMPION OF CHICKENS!\nPress ESC to return to Main Menu";
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Time.timeScale = 1f;
                if (DatosPartidaManager.Instance != null) Destroy(DatosPartidaManager.Instance.gameObject);
                SceneManager.LoadScene("MainMenu"); 
            }
        }
        else if (isGameOver)
        {
            transitionTimer -= Time.unscaledDeltaTime;

            if (winText != null)
            {
                winText.text = baseWinMessage + "\nNext Round in: " + Mathf.CeilToInt(Mathf.Max(0, transitionTimer)).ToString();
            }

            if (transitionTimer <= 0)
            {
                Time.timeScale = 1f;
                SceneManager.LoadScene("Shop");
            }
        }
    }

    private void OnTriggerStay(Collider otherCollider)
    {
        if (otherCollider.gameObject.CompareTag("Ball"))
        {
            BallInfo ballInfo = otherCollider.GetComponent<BallInfo>();
            if (ballInfo == null || ballInfo.hasScored) return;


            if (ballInfo.realVelocity.y > 0)
            {
                ballInfo.passedFromBelow = true;
                return; 
            }

            if (ballInfo.passedFromBelow)
            {
                return;
            }

            ballInfo.hasScored = true; 

            if (effectObj != null) effectObj.SetActive(true);
            if (audioManager != null) audioManager.PlaySFX(audioManager.ballNet);

            if (ballInfo.lastPlayerID == 1) scoreP1++;
            else if (ballInfo.lastPlayerID == 2) scoreP2++;
            else if (ballInfo.lastPlayerID == 3) scoreP3++;
            else if (ballInfo.lastPlayerID == 4) scoreP4++;

            UpdateUI();
            StartCoroutine(DisableEffect());
            StartCoroutine(RespawnBallCoroutine(otherCollider.gameObject));
        }
    }

    private void UpdateUI()
    {
        if (scoreTextP1 != null) scoreTextP1.text = scoreP1.ToString();
        if (scoreTextP2 != null) scoreTextP2.text = scoreP2.ToString();
        if (scoreTextP3 != null) scoreTextP3.text = scoreP3.ToString();
        if (scoreTextP4 != null) scoreTextP4.text = scoreP4.ToString();

        if (!isGameOver)
        {
            if (scoreP1 >= 5) DeclararGanador(0, "Green chicken wins!", Color.green);
            else if (scoreP2 >= 5) DeclararGanador(1, "Purple chicken wins!", new Color(0.5f, 0f, 0.5f));
            else if (scoreP3 >= 5) DeclararGanador(2, "Orange chicken wins!", new Color(1f, 0.5f, 0f));
            else if (scoreP4 >= 5) DeclararGanador(3, "Red chicken wins!", Color.red);
        }
    }

    private void DeclararGanador(int winnerID, string text, Color color)
    {
        isGameOver = true;
        winText.color = Color.white;
        
        int coinsP1 = CalcularMonedas(0, scoreP1, winnerID);
        int coinsP2 = CalcularMonedas(1, scoreP2, winnerID);
        int coinsP3 = CalcularMonedas(2, scoreP3, winnerID);
        int coinsP4 = CalcularMonedas(3, scoreP4, winnerID);

        RepartirRecompensas(winnerID, coinsP1, coinsP2, coinsP3, coinsP4);

        string rewards = "\n";
        rewards += "P1: " + (winnerID == 0 ? "+1 Star | " : "") + "+" + coinsP1 + " Coins\n";
        rewards += "P2: " + (winnerID == 1 ? "+1 Star | " : "") + "+" + coinsP2 + " Coins\n";
        rewards += "P3: " + (winnerID == 2 ? "+1 Star | " : "") + "+" + coinsP3 + " Coins\n";
        rewards += "P4: " + (winnerID == 3 ? "+1 Star | " : "") + "+" + coinsP4 + " Coins\n";

        baseWinMessage = text + rewards;
        winText.text = baseWinMessage;

        Time.timeScale = 0f;
    }

    private int CalcularMonedas(int pID, int score, int winnerID)
    {
        float monedasBase = score * 10f;
        if (pID == winnerID) monedasBase *= (1f - winnerTax);
        return Mathf.FloorToInt(monedasBase);
    }

    private void RepartirRecompensas(int winnerID, int coinsP1, int coinsP2, int coinsP3, int coinsP4)
    {
        if (DatosPartidaManager.Instance != null)
        {
            if (winnerID >= 0)
            {
                DatosPartidaManager.Instance.SumarEstrellas(winnerID, 1);
                if (DatosPartidaManager.Instance.jugadores[winnerID].estrellas >= 3)
                {
                    isGameComplete = true;
                }
            }
            DatosPartidaManager.Instance.SumarMonedas(0, coinsP1);
            DatosPartidaManager.Instance.SumarMonedas(1, coinsP2);
            DatosPartidaManager.Instance.SumarMonedas(2, coinsP3);
            DatosPartidaManager.Instance.SumarMonedas(3, coinsP4);
        }
    }

    public void CheckTimeOutWinner()
    {
        if (isGameOver) return;
        isGameOver = true;

        if (scoreP1 > scoreP2 && scoreP1 > scoreP3 && scoreP1 > scoreP4)
        {
            DeclararGanador(0, "Green chicken wins!", Color.green);
        }
        else if (scoreP2 > scoreP1 && scoreP2 > scoreP3 && scoreP2 > scoreP4)
        {
            DeclararGanador(1, "Purple chicken wins!", new Color(0.5f, 0f, 0.5f));
        }
        else if (scoreP3 > scoreP1 && scoreP3 > scoreP2 && scoreP3 > scoreP4)
        {
            DeclararGanador(2, "Orange chicken wins!", new Color(1f, 0.5f, 0f));
        }
        else if (scoreP4 > scoreP1 && scoreP4 > scoreP2 && scoreP4 > scoreP3)
        {
            DeclararGanador(3, "Red chicken wins!", Color.red);
        }
        else
        {
            DeclararGanador(-1, "It's a Draw!", Color.white);
        }
    }

    private IEnumerator DisableEffect()
    {
        yield return new WaitForSeconds(2.0f);
        if (effectObj != null) effectObj.SetActive(false);
    }

    private IEnumerator RespawnBallCoroutine(GameObject ball)
    {
        yield return new WaitForSeconds(respawnDelay);

        Rigidbody ballRb = ball.GetComponent<Rigidbody>();
        if (ballRb != null)
        {
            ballRb.velocity = Vector3.zero;
            ballRb.angularVelocity = Vector3.zero;
        }

        float randomX = Random.Range(-respawnAreaSize, respawnAreaSize);
        float randomZ = Random.Range(-respawnAreaSize, respawnAreaSize);
        ball.transform.position = new Vector3(randomX, respawnHeight, randomZ);
        
        BallInfo bInfo = ball.GetComponent<BallInfo>();
        if (bInfo != null)
        {
            bInfo.hasScored = false;
            bInfo.passedFromBelow = false;
            bInfo.lastPlayerID = 0;
        }
    }
}
