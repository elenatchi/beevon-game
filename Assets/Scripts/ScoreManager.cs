using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public int honey;
    public int pollen;
    public float targetTime = 60.0f;

    public TMP_Text honeyDisplay;
    public TMP_Text pollenDisplay;
    public TMP_Text Timer;

    public ScoreVariable score;

    int previousSecond;

    // Start is called before the first frame update
    void Start()
    {
        honey = 0;
        pollen = 0;
        previousSecond = 60;
    }

    // Update is called once per frame
    void Update()
    {
        targetTime -= Time.deltaTime;
        Timer.text = Mathf.Floor(targetTime).ToString();

        if (targetTime< previousSecond)
        {
            Timer.rectTransform.LeanScale(new Vector3(1.2f, 1.2f, 1.2f), 0.5f).setEaseOutCubic().setOnComplete(ShrinkDownTimer);
            Timer.text = Mathf.Floor(targetTime).ToString();
            previousSecond -= 1;
        }

        if (targetTime <= 0.0f)
        {
            Timer.text = "";
            EndRound();
        }
    }

    void ShrinkDownTimer()
    {
        Timer.rectTransform.LeanScale(new Vector3(1f, 1f, 1f), 0.5f).setEaseInCubic();
    }


    public void EndRound()
    {
        score.runtimeValue = honey;
        this.GetComponent<StartMenuManager>().GoToScore();
    }

    public void AddPollen(int number)
    {
        pollen += number;
        pollenDisplay.text = pollen.ToString();
    }

    public void RemovePollen()
    {
        pollen = 0;
        pollenDisplay.text = pollen.ToString();
    }

    public void AddHoney()
    {
        honey += pollen;
        pollen = 0;

        honeyDisplay.text = honey.ToString();
        pollenDisplay.text = pollen.ToString();

        this.GetComponent<SpawnFlowers>().SpawnFlowerField();
    }
}
