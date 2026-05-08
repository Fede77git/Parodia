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

    
    public TMP_Text winText;

    private int scoreP1 = 0;
    private int scoreP2 = 0;
    private int scoreP3 = 0;
    private int scoreP4 = 0;

    public static ScoreManager instance;
    public static bool isGameOver = false;

    AudioManager audioManager;

    private void Awake()
    {
        instance = this;
        isGameOver = false;
        GameObject audioObj = GameObject.FindGameObjectWithTag("Audio");
        if(audioObj != null) audioManager = audioObj.GetComponent<AudioManager>();
    }

    private void Start()
    {
        if (effectObj != null) effectObj.SetActive(false);
        winText.text = "";
        UpdateUI();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("NBA");
            Time.timeScale = 1f;
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

        if (scoreP1 >= 5)
        {
            winText.text = "Green chicken wins!\nPress R to rematch";
            winText.color = Color.green;
            isGameOver = true;
            Time.timeScale = 0f;
        }
        else if (scoreP2 >= 5)
        {
            winText.text = "Purple chicken wins!\nPress R to rematch";
            winText.color = new Color(0.5f, 0f, 0.5f);
            isGameOver = true;
            Time.timeScale = 0f;
        }
        else if (scoreP3 >= 5)
        {
            winText.text = "Orange chicken wins!\nPress R to rematch";
            winText.color = new Color(1f, 0.5f, 0f);
            isGameOver = true;
            Time.timeScale = 0f;
        }
        else if (scoreP4 >= 5)
        {
            winText.text = "Red chicken wins!\nPress R to rematch";
            winText.color = Color.red;
            isGameOver = true;
            Time.timeScale = 0f;
        }
    }

    public void CheckTimeOutWinner()
    {
        if (isGameOver) return;
        isGameOver = true;

        if (scoreP1 > scoreP2 && scoreP1 > scoreP3 && scoreP1 > scoreP4)
        {
            winText.text = "Green chicken wins!\nPress R to rematch";
            winText.color = Color.green;
        }
        else if (scoreP2 > scoreP1 && scoreP2 > scoreP3 && scoreP2 > scoreP4)
        {
            winText.text = "Purple chicken wins!\nPress R to rematch";
            winText.color = new Color(0.5f, 0f, 0.5f);
        }
        else if (scoreP3 > scoreP1 && scoreP3 > scoreP2 && scoreP3 > scoreP4)
        {
            winText.text = "Orange chicken wins!\nPress R to rematch";
            winText.color = new Color(1f, 0.5f, 0f);
        }
        else if (scoreP4 > scoreP1 && scoreP4 > scoreP2 && scoreP4 > scoreP3)
        {
            winText.text = "Red chicken wins!\nPress R to rematch";
            winText.color = Color.red;
        }
        else
        {
            winText.text = "It's a Draw!\nPress R to rematch";
            winText.color = Color.white;
        }
        Time.timeScale = 0f;
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
