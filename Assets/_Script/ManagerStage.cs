using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class ManagerStage : MonoBehaviour
{
    private int currentScreen;
    public static ManagerStage instance;
    UnityEvent m_MyEvento;
    [SerializeField]private Button textPlayGame;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this.gameObject);
        }

        SceneManager.sceneLoaded += Load;
        m_MyEvento = new UnityEvent();
    }
   


    void Update()
    {
        currentScreen = SceneManager.GetActiveScene().buildIndex;

    }

    public enum Screen
    {
        HomeScreen,
        Transition,
        Phase1,
        GameOver,
        Underground_Phase1,
        Congratulations
    }

    void Load(Scene cena, LoadSceneMode modo)
    {
        WhichStage();

        if (WhichStage() == (int)Screen.HomeScreen)
        {
            try
            {
                textPlayGame = GameObject.Find("TxtPlayerGame").GetComponent<Button>();
            }
            catch
            {
                StartGame();
            }
            textPlayGame.onClick.RemoveListener(() => StartGame());
            textPlayGame.onClick.AddListener(() => StartGame());
            UIManager.instance.highScore = GameObject.Find("ScoringRecord").GetComponent<Text>();
            UIManager.instance.highScore.text = UIManager.instance.RecoverSavedScore().ToString("D6");
            if (GameObject.Find("Mario"))
            {
                Destroy(GameObject.Find("Mario").gameObject);
            }
        }

        if (WhichStage() == (int)Screen.GameOver)
        {
           
            try
            {
                SoundManager.instance.TocarBG(1);
                Invoke("BackHomeScreen", 2.5f);
            }
            catch
            {
                instance = this ;
            }

        }
        if (WhichStage() == (int)Screen.Underground_Phase1)
        {
            Time.timeScale = 1;
            SoundManager.instance.TocarBG(2);
            GameObject mario = GameObject.Find("Mario");
            GameObject pointStart = GameObject.Find("pointStart");
            mario.GetComponent<Collider2D>().enabled = true;
            mario.GetComponent<SpriteRenderer>().sortingOrder = 5;
            mario.transform.position = new Vector3(pointStart.transform.position.x, pointStart.transform.position.y, mario.transform.position.z);
        }
    }
    public void StartGame()
    {
        LoadScreen(Screen.Transition);

        if (Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            LoadScreen(Screen.Transition);
            currentScreen = 1;
        }
    }

    void StopSound()
    {
        
        SoundManager.instance.StopSoundBG();
    }

    void BackHomeScreen()
    {
        StopSound();
        SceneManager.LoadScene(0);
        
    }

    public int WhichStage()
    {
        currentScreen = SceneManager.GetActiveScene().buildIndex;
        return currentScreen;
    }

    public void LoadScreen(Screen scene) 
    {
        SceneManager.LoadScene(scene.ToString());
    }
}
