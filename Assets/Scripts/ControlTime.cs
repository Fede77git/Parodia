using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ControlTime : MonoBehaviour
{

    public Image uiFill;
    public TMP_Text uiText;

    public int duration;
    private int remainingDuration;

    // Start is called before the first frame update
    void Start()
    {
        Being(duration);
    }

    private void Being(int Second)
    {
        remainingDuration = Second;
        StartCoroutine(UpdateTimer());
    }

    private IEnumerator UpdateTimer()
    {
        while (remainingDuration >= 0)
        {
            uiText.text = $"{remainingDuration/ 60:00}: {remainingDuration % 60:00}";
            uiFill.fillAmount =Mathf.InverseLerp(0, duration, remainingDuration);
            remainingDuration--;
            yield return new WaitForSeconds(1f);

        }
        OnEnd();
    }


    private void OnEnd()
    {
        print("End");
        Time.timeScale = 0f;

    }

}
