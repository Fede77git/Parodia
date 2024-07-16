using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Score : MonoBehaviour
{
    public GameObject effectObj;
    private int scoreCount;
    public TMP_Text scoreText;
    public TMP_Text winText;


    private void Start()
    {
        effectObj.SetActive(false);
        scoreCount = 0;
        SetText();
        winText.text = "";
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
            effectObj.SetActive (true);

            
            scoreCount = scoreCount + 1;
            SetText();
           

            StartCoroutine(DisableEffect());
        }
    }


    private void SetText()
    {
        scoreText.text = "" + scoreCount.ToString();
        if (scoreCount >= 5) 
        {

            winText.text = "Green player wins!";
            Debug.Log("WIN");
            Time.timeScale = 0f;


        }
    }


    private IEnumerator DisableEffect()
    {
        yield return new WaitForSeconds(1.0f);
        effectObj.SetActive(false);
    }
}
