using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using MarioUnity.Entidades;

public class UIManager : MonoBehaviour
{
    [SerializeField]private GameObject prefabAchievedPointText;
    [SerializeField]private Canvas canvas;
    [SerializeField]private Text scoreGame;
    [SerializeField]private Text totalCoinPurchased;
    [SerializeField]private Text chronometer;
    [SerializeField] public Text highScore;
    [SerializeField]private RawImage coinIcon;
    public static UIManager instance;
    private float time;
    private float currentTime;
    private int touch;
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
        time = 400;
        currentTime = time;
        SceneManager.sceneLoaded += Load;
        

    }

    
    void Start()
    {
        touch = 0;
        SaveGeneralData();
       
    }

   
    void Update()
    {
        StartCoroutine(ChangeCoinColor());
        StopCoroutine(ChangeCoinColor());
        if(ManagerStage.instance.WhichStage() != 0 && ManagerStage.instance.WhichStage() != 3  && ManagerStage.instance.WhichStage() != 1 && ManagerStage.instance.WhichStage() != 5)
        {
            chronometer.enabled = true;
            currentTime -= Time.deltaTime;
        }
        else
        {
            chronometer.enabled = false;
        } 

        if(currentTime <= 100 && touch==0)
        {   touch += 1;

            StartCoroutine(Expect());
            StopCoroutine(Expect());
          
        }

        chronometer.text = currentTime.ToString("f0");
        if (currentTime <= 0)
        {
            currentTime = 0;
            
        }
    }

   
    void Load(Scene cena, LoadSceneMode modo)
    {
        LocateUIComponent();
        if (ManagerStage.instance.WhichStage() == (int)ManagerStage.Screen.Underground_Phase1)
        {
            totalCoinPurchased.text = GetCoinSave().ToString("D2");
            scoreGame.text = GetScoreSave().ToString("D6");
        }
    }
    private void LocateUIComponent()
    {
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        scoreGame = GameObject.Find("ScoreGame").GetComponent<Text>();
        totalCoinPurchased = GameObject.Find("TxtNumberCoins").GetComponent<Text>();
        chronometer = GameObject.Find("Chronometer").GetComponent<Text>();
        coinIcon = GameObject.Find("CoinIcon").GetComponent<RawImage>();

    }
    public void ShowValue(string valor = "0",float xLocal =0,float yLocal =0)
    {
        GameObject showValue = Instantiate(prefabAchievedPointText,canvas.transform,false) as GameObject;
        showValue.GetComponent<Text>().text = valor;
        showValue.transform.position = new Vector2(xLocal,yLocal);
        Destroy(showValue, 1f);
    }

    public void AddValueTotalScore(int value )
    {
        int recordCalculation = int.Parse(scoreGame.text) + value;
        scoreGame.text = recordCalculation.ToString("D6");
        int record = RecoverSavedScore();
        if(recordCalculation>=record)
            SaveScore(recordCalculation);
    }

    public void AddValueTotalCoin()
    {
        int calculoTotalMoeda = int.Parse(totalCoinPurchased.text) + 1;
        totalCoinPurchased.text = calculoTotalMoeda.ToString("D2");
  
    }

    IEnumerator ChangeCoinColor()
    {
        coinIcon.color = Color.red;
        yield return new WaitForSeconds(1f);
        coinIcon.color = Color.white;
    }

    private void SaveScore(int pontos)
    {
        PlayerPrefs.SetInt("HighScore", pontos);
    }

    public void SaveGeneralData()
    {
        PlayerPrefs.SetInt("Score", int.Parse(scoreGame.text));
        PlayerPrefs.SetInt("Coin", int.Parse(totalCoinPurchased.text));
        
    }
    private int GetScoreSave()
    {
        return PlayerPrefs.GetInt("Score");
    }
    private int GetCoinSave()
    {
        return PlayerPrefs.GetInt("Coin");
    }
    public int RecoverSavedScore()
    {
        if (PlayerPrefs.HasKey("HighScore"))
          return  PlayerPrefs.GetInt("HighScore");
        else
        {
           int pontuacao;
           pontuacao = int.Parse(scoreGame.text);
           SaveScore(pontuacao);
           return pontuacao;
        }
    }

    IEnumerator Expect()
    {
        SoundManager.instance.PauseSoundBG();
        SoundManager.instance.TocarFx(1);
        yield return new WaitForSeconds(0.1f);
        SoundManager.instance.TocarBG(0);
        SoundManager.instance.ChangeMusicSpeed(1.2f);
    }

    public float CurrentTime()
    {
        return currentTime;
    }

    public void ResetTime()
    {
        currentTime = time;
    }
}
