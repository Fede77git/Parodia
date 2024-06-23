using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    public GameObject effectObj;
    private int scoreCount = 0;
    private void Start()
    {
        effectObj.SetActive(false);
    }
    private void OnTriggerEnter(Collider otherCollider)
    {
        if (otherCollider.gameObject.CompareTag("Ball"))
        {
            effectObj.SetActive (true);
            Debug.Log("Score!");

            scoreCount++;
            Debug.Log("Score:" + scoreCount);

            StartCoroutine(DisableEffect());
        }
    }

    private IEnumerator DisableEffect()
    {
        yield return new WaitForSeconds(1.0f);
        effectObj.SetActive(false);
    }
}
