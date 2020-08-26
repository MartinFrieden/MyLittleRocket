using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Video;
//using GoogleMobileAds.Api;

public class GameManager : Singleton<GameManager>
{
    //массив шаблонов облаков
    public GameObject[] prefabClouds;

    //массив шаблонов препятствий
    public GameObject[] prefabBarrier;

    //массив с бонусами
    public GameObject[] prefabBonus;

    //барьеров в секунду
    public float barrierSpawnPerSecond = 1f;

    //облака в секунду
    public float cloudsSpawnPerSecond = 2f;

    //предок препятствий
    public Transform parent;

    // Местоположение, где должен появиться ракета.
    public GameObject startingPoint;

    // Сценарий, управляющий камерой, которая должна следовать за ракетой
    public CameraFollow cameraFollow;

    //фон за ракетой
    public CameraFollow bgFollow;

    // 'текущая' hfrtnf (в противоположность всем погибшим)
    public Rocket currentRocket;

    // Объект-шаблон для создания новой ракеты
    public GameObject rocketPrefab;

    //стартовое меню
    public RectTransform startMenu;

    // Компонент пользовательского интерфейса с кнопками
    // 'перезапустить и 'продолжить'
    public RectTransform mainMenu;

    // Компонент пользовательского интерфейса с кнопками
    // 'вверх', 'вниз' и 'меню
    public RectTransform gameplayMenu;

    // Компонент пользовательского интерфейса с экраном
    // 'GameOver!'
    public RectTransform gameOverMenu;

    // Задержка перед созданием новой ракеты после гибели
    public float delayAfterDeath = 1.0f;

    // Звук, проигрываемый в случае гибели ракеты
    public AudioClip rocketDiedSound;

    // Звук, проигрываемый в случае победы в игре
    public AudioClip gameOverSound;

    //private const string banner = "ca-app-pub-1020975280593406/6134711749";
    //private const string gameOverAD = "ca-app-pub-3940256099942544/1033173712";
    //private InterstitialAd ad;


    void Start()
    {
        gameOverMenu.gameObject.SetActive(false);
        mainMenu.gameObject.SetActive(false);
        gameplayMenu.gameObject.SetActive(false);
        //GoSpawn();
    }

    //спаун бонусов в апдэйте
    private void Update()
    {
        if(Random.Range(0,1000) == 1)
        {
            SpawnBonus();
        }     
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void SpawnClouds()
    {
            int ndx = Random.Range(0, prefabClouds.Length);
            GameObject go = Instantiate<GameObject>(prefabClouds[ndx], parent);
            Vector3 pos = Vector3.zero;
            Vector2 scale;
            float xMin = currentRocket.transform.position.x - 15f;
            float xMax = currentRocket.transform.position.x + 15f;
            float yMin = currentRocket.transform.position.y + 10f;
            float yMax = currentRocket.transform.position.y + 50f;
            float zMin = currentRocket.transform.position.z + 1f;
            float zMax = currentRocket.transform.position.z - 1f;
            float xScaleMin = 1f; float xScaleMax = 1.5f;
            float yScaleMin = 0.5f; float yScaleMax = 1.5f;
            scale.x = Random.Range(xScaleMin, xScaleMax);
            scale.y = Random.Range(yScaleMin, yScaleMax);
            pos.x = Random.Range(xMin, xMax);
            pos.y = Random.Range(yMin, yMax);
            pos.z = Random.Range(zMin, zMax);
            go.transform.position = pos;
            go.transform.localScale = scale;
            Invoke("SpawnClouds", 1f / cloudsSpawnPerSecond);
    }

    public void SpawnBonus()
    {
        if (currentRocket)
        {
            int ndx = Random.Range(0, prefabBonus.Length);
            GameObject go = Instantiate<GameObject>(prefabBonus[ndx], parent);
            // Установить начальные координаты созданного барьера
            Vector2 pos = Vector2.zero;
            float xMin = currentRocket.transform.position.x - 1f;
            float xMax = currentRocket.transform.position.x + 1f;
            pos.x = Random.Range(xMin, xMax);
            pos.y = currentRocket.transform.position.y + 10f;
            go.transform.position = pos;
        }
    }

    public void SpawnBarrier()
    {
         int ndx = Random.Range(0, prefabBarrier.Length);
         GameObject go = Instantiate<GameObject>(prefabBarrier[ndx], parent);
         // Установить начальные координаты созданного барьера
         Vector2 pos = Vector2.zero;
        float xMin = currentRocket.transform.position.x - 8f;      
        float xMax = currentRocket.transform.position.x + 8f;
        float yMin = currentRocket.transform.position.y + 7f;
        float yMax = currentRocket.transform.position.y + 25f;
        pos.x = Random.Range(xMin, xMax);
        pos.y = Random.Range(yMin, yMax);
        go.transform.position = pos;
        Invoke("SpawnBarrier", 1f / barrierSpawnPerSecond);
    }

    // Сбрасывает игру в исходное состояние.
    public void Reset()
    {
        // Выключает меню, включает интерфейс игры
        if (startMenu)
            startMenu.gameObject.SetActive(false);
        if (gameOverMenu)
            gameOverMenu.gameObject.SetActive(false);
        if (mainMenu)
            mainMenu.gameObject.SetActive(false);
        if (gameplayMenu)
            gameplayMenu.gameObject.SetActive(true);
        // Найти все компоненты Resettable и сбросить их в исходное состояние
        var resetObjects = FindObjectsOfType<Resettable>();
        foreach (Resettable r in resetObjects)
        {
            r.Reset();
        }
        // Создать новую ракету
        CreateNewRocket();
        // обновить бак
        FullFuel();
        //удалить барьеры
        DeleteBarriers();
        //удалить бонусы
        DeleteBonus();
        // Прервать паузу в игре
        Time.timeScale = 1.0f;
        //BannerView bannerV = new BannerView(banner, AdSize.Banner,AdPosition.Bottom);
        //AdRequest request = new AdRequest.Builder().AddTestDevice(AdRequest.TestDeviceSimulator).AddTestDevice(SystemInfo.deviceUniqueIdentifier.ToUpper()).Build();
        //AdRequest request = new AdRequest.Builder().AddTestDevice(AdRequest.TestDeviceSimulator).AddTestDevice("38ECA721B53A94A2").Build();
        //AdRequest request = new AdRequest.Builder().Build();
       // bannerV.LoadAd(request);
    }

    //удалить барьеры
    public void DeleteBarriers()
    {
        GameObject[] barriers;
        barriers = GameObject.FindGameObjectsWithTag("Barrier");
        foreach (GameObject b in barriers)
            Destroy(b);        
    }

    public void DeleteBonus()
    {
        GameObject[] bonuses;
        bonuses = GameObject.FindGameObjectsWithTag("Fuel");
        foreach (GameObject f in bonuses)
            Destroy(f);
    }

    //обновить бак после перезапуска
    public void FullFuel()
    {
        GameObject rocketEngine;
        rocketEngine = GameObject.Find("RocketEngine");
        rocketEngine.GetComponent<RocketEngine>().fuel = 100f;
        rocketEngine.GetComponent<RocketEngine>().IsUp = false;
    }

    void CreateNewRocket()
    {
        // Удалить текущего гномика, если имеется
        RemoveRocket();

        // Создать новый объект Gnome и назначить его текущим
        GameObject newRocket =
        (GameObject)Instantiate(rocketPrefab,
        startingPoint.transform.position,
        Quaternion.identity);

        currentRocket = newRocket.GetComponent<Rocket>();

        // Сообщить объекту cameraFollow, что он должен
        // начать следить за новым объектом Gnome
        cameraFollow.target = currentRocket.cameraFollowTarget;
        bgFollow.target = currentRocket.cameraFollowTarget;
    }

    void RemoveRocket()
    {
        // Запретить камере следовать за ракетой
        cameraFollow.target = null;
        // Если текущий гномик существует, исключить его из игры
        if (currentRocket != null)
        {
            // Пометить объект как исключенный из игры
            // (чтобы коллайдеры перестали сообщать о столкновениях с ним)
            currentRocket.gameObject.tag = "Untagged";
            // Найти все объекты с тегом "Player" и удалить этот тег
            foreach (Transform child in
            currentRocket.transform)
            {
                child.gameObject.tag = "Untagged";
            }
            // Установить признак отсутствия текущего гномика
            currentRocket = null;
        }
    }

    // Вызывается, когда ракета падает вниз.
    public void TheEnd()
    {

        // Приостановить игру
        Time.timeScale = 0.0f;
        // Выключить меню завершения игры и включить экран
        // "игра завершена"!
        if (gameOverMenu)
        {
            gameOverMenu.gameObject.SetActive(true);
        }
        if (gameplayMenu)
        {
            gameplayMenu.gameObject.SetActive(false);
        }
        var audio = GetComponent<AudioSource>();
        if (audio)
        {
            audio.PlayOneShot(this.gameOverSound);
        }

        //ad = new InterstitialAd(gameOverAD);
        //AdRequest request = new AdRequest.Builder().AddTestDevice(AdRequest.TestDeviceSimulator).AddTestDevice(SystemInfo.deviceUniqueIdentifier.ToUpper()).Build();
        //AdRequest request = new AdRequest.Builder().AddTestDevice(AdRequest.TestDeviceSimulator).AddTestDevice("38ECA721B53A94A2").Build();
        
        //AdRequest request = new AdRequest.Builder().Build();
        //ad.LoadAd(request);
        //ad.OnAdLoaded += OnAdLoaded;
    }

    public void OnAdLoaded(object sender, System.EventArgs args)
    {
        //ad.Show();
    }

    // Вызывается в ответ на касание кнопок Menu и Resume Game.
    public void SetPaused(bool paused)
    {
        // Если игра на паузе, остановить время и включить меню
        // (и выключить интерфейс игры)
        if (paused)
        {
            Time.timeScale = 0.0f;
            mainMenu.gameObject.SetActive(true);
            gameplayMenu.gameObject.SetActive(false);
        }
        else
        {
            // Если игра не на паузе, возобновить ход времени и
            // выключить меню (и включить интерфейс игры)
            Time.timeScale = 1.0f;
            mainMenu.gameObject.SetActive(false);
            gameplayMenu.gameObject.SetActive(true);
        }
    }

    // Вызывается в ответ на касание кнопки Restart.
    public void RestartGame()
    {
        // Немедленно удалить гномика (минуя этап гибели)
        Destroy(currentRocket.gameObject);
        currentRocket = null;
        // Сбросить игру в исходное состояние, чтобы создать нового гномика.
        Reset();
    }

    public void GoSpawn()
    {
        Invoke("SpawnBarrier", 1f / barrierSpawnPerSecond);
        Invoke("SpawnClouds", 1f / cloudsSpawnPerSecond);
    }


}
