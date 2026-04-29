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

    
    public TMP_Text winText;

    private int scoreP1 = 0;
    private int scoreP2 = 0;

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

            UpdateUI();
            StartCoroutine(DisableEffect());
            StartCoroutine(RespawnBallCoroutine(otherCollider.gameObject));
        }
    }

    private void UpdateUI()
    {
        if (scoreTextP1 != null) scoreTextP1.text = scoreP1.ToString();
        if (scoreTextP2 != null) scoreTextP2.text = scoreP2.ToString();

        if (scoreP1 >= 5)
        {
            winText.text = "Green chicken wins!";
            isGameOver = true;
            Time.timeScale = 0f;
        }
        else if (scoreP2 >= 5)
        {
            winText.text = "Purple chicken wins!";
            isGameOver = true;
            Time.timeScale = 0f;
        }
    }

    public void CheckTimeOutWinner()
    {
        if (isGameOver) return;
        isGameOver = true;

        if (scoreP1 > scoreP2)
        {
            winText.text = "Green chicken wins!";
        }
        else if (scoreP2 > scoreP1)
        {
            winText.text = "Purple chicken wins!";
        }
        else
        {
            winText.text = "It's a Draw!";
        }
        Time.timeScale = 0f;
    }

    private IEnumerator DisableEffect()
    {
        yield return new WaitForSeconds(1.0f);
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
