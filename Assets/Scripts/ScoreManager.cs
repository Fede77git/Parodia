using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public GameObject effectObj;
    

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

    private void OnTriggerEnter(Collider otherCollider)
    {
        if (otherCollider.gameObject.CompareTag("Ball"))
        {
            if (effectObj != null) effectObj.SetActive(true);
            if (audioManager != null) audioManager.PlaySFX(audioManager.ballNet);

            BallInfo ballInfo = otherCollider.GetComponent<BallInfo>();
            if (ballInfo != null)
            {
                if (ballInfo.lastPlayerID == 1) scoreP1++;
                else if (ballInfo.lastPlayerID == 2) scoreP2++;
    
            }
            else 
            {
                Debug.LogWarning("missing script ball");
            }

            UpdateUI();
            StartCoroutine(DisableEffect());
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
}
