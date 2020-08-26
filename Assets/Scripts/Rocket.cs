using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    // Объект, за которым должна следовать камера.
    public Transform cameraFollowTarget;
    public GameObject particles;
    public GameObject currRock;
    //звук проигрываемый при столкновении с ракетой
    public AudioClip touchBonus;
    public float delayBeforeRemoving = 3.0f;

    private void Awake()
    {
        particles = GameObject.Find("Fire");
        particles.SetActive(false);
    }
    public void DestroyRocket()
    {
        var remove = gameObject.AddComponent<RemoveAfterDelay>();
        remove.delay = delayBeforeRemoving;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //врезание в барьер
        if (other.tag == "Barrier")
            
            GameManager.instance.TheEnd();

        //подбор топлива
        if (other.tag == "Fuel")
        {
            RocketEngine.instance.fuel += 10;
            Destroy(other.gameObject);
            var audio = GetComponent<AudioSource>();
            if (audio)
            {
                audio.PlayOneShot(this.touchBonus);
            }
            
        }

        if(other.tag == "Resterter")
        {
            GameManager.instance.RestartGame();
        }
    }

    private void Update()
    {
        if(RocketEngine.instance.IsUp == true)
        {
            particles.SetActive(true);
        }
        else
        {
            if (particles == null)
            {
                particles = GameObject.Find("Fire");
            }
            particles.SetActive(false);
        }
    }


}
