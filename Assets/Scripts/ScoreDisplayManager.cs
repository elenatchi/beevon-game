using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreDisplayManager : MonoBehaviour
{
    public TMP_Text scoreDisplay;
    public ScoreVariable score;

    // Start is called before the first frame update
    void Start()
    {
        scoreDisplay.text = score.runtimeValue.ToString();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
