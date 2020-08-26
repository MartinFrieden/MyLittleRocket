using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuInfo : MonoBehaviour
{
    public Text maxScore;

    // Start is called before the first frame update
    void Start()
    {
        GameObject maxScoreGO = GameObject.Find("Challenge");
        maxScore = maxScoreGO.GetComponent<Text>();
        maxScore.text = "MAXIMUM HEIGHT: " + "\n" + PlayerPrefs.GetInt("recordInfo").ToString() + " m.";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
