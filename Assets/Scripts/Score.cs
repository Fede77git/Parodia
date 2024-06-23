using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
        scoreText.text = " " + scoreCount.ToString();
        if (scoreCount >= 5) 
        {

            winText.text = "You Win!";
            Debug.Log("WIN");

        }
    }


    private IEnumerator DisableEffect()
    {
        yield return new WaitForSeconds(1.0f);
        effectObj.SetActive(false);
    }
}
