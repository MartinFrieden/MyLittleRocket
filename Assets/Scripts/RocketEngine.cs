using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class RocketEngine : Singleton<RocketEngine>
{
    public bool         IsUp { get; set; }
    public float        fuel = 100f;
    public float        rocketSpeed = 100f;
    public float        currentHeight;
    public float        maxHeight;
    public Text         scoreGT;
    public Text         scoreRec;
    public Text         soreGameOverMenu;
    public Text         recordScoreGameOverMenu;
    GameObject          rocket;
    //звук двигателя
    public AudioClip engineSound;

    private void Start()
    {
        // Получить ссылку на игровой объект ScoreCounter
        GameObject scoreGO = GameObject.Find("Height");
        GameObject scoreRecord = GameObject.Find("RecordHeight");
        GameObject scoreGOMenu = GameObject.Find("Current Height GO");
        GameObject recordScoreGOMenu = GameObject.Find("Record Height GO");

        // Получить компонент Text этого игрового объекта
        scoreGT = scoreGO.GetComponent<Text>();
        scoreRec = scoreRecord.GetComponent<Text>();
        soreGameOverMenu = scoreGOMenu.GetComponent<Text>();
        recordScoreGameOverMenu = recordScoreGOMenu.GetComponent<Text>();

        maxHeight = PlayerPrefs.GetInt("recordInfo");

        // Установить начальное число очков равным 0
        scoreGT.text = "Height: 0";
        rocket = GameObject.FindGameObjectWithTag("Player");
        ResetRocket();
    }

    public void ResetRocket()
    {
        IsUp = false;
    }

    public void OnSoundEngine()
    {
        var audio = GetComponent<AudioSource>();
        if (RocketEngine.instance.IsUp == true)
        {
            audio.PlayOneShot(this.engineSound);
        }
        else
        {
            audio.Stop();
        }
    }

    private void FixedUpdate()
    {

        rocket = GameObject.FindGameObjectWithTag("Player");
        if (IsUp && fuel > 0)
        {
            
            // Получить величину наклона из InputManager
            float turn = InputManager.instance.sidewaysMotion;

            // Вычислить прилагаемую силу
            Vector2 force = new Vector2(0, Mathf.Cos(turn)*rocketSpeed*Time.deltaTime);
            Vector2 force2 = new Vector2(Mathf.Sin(turn)*rocketSpeed*Time.deltaTime, 0);
            Vector2 force3 = force + force2;

            // Приложить силу
            rocket.GetComponent<Rigidbody2D>().AddForce(force3);
            fuel -= 0.05f;
        }
        else if(fuel <= 0 )
        {
            GameManager.instance.TheEnd();
        }

        currentHeight = rocket.transform.position.y;
        int height = (Int32)currentHeight;
        scoreGT.text ="Height: " + height.ToString();
        if((int)currentHeight >= maxHeight)
        {
            //maxHeight = (float)height;
            PlayerPrefs.SetInt("recordInfo", (int)currentHeight);
            PlayerPrefs.Save();  
        }
        maxHeight = PlayerPrefs.GetInt("recordInfo");
        scoreRec.text = "Record height: " + maxHeight.ToString();

        soreGameOverMenu.text = "Your Height: " +"\n" +((int)currentHeight).ToString()+" m";
        recordScoreGameOverMenu.text = "Record Height: " + "\n" + maxHeight.ToString()+" m";
    }
}
