using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using MarioUnity.Entidades;
public class GameManager : MonoBehaviour
{
  
    public static GameManager instance;
    [SerializeField]private bool gameOver;
    private bool pause;
    GameObject camVirtual; 
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        camVirtual = GameObject.Find("CamVirtual");
       
    }
    void Start()
    {
        IniciarJogo();     
    }


    void Update()
    {       
        GameOver();
        PausaGame();  
    }

    void GameOver()
    {
        if (Mario.instance.isAlive == false)
        {
            StartCoroutine(TransicaoScene(3, 2.2f)); 
            camVirtual.SetActive(false);
            gameOver = true;
            Time.timeScale = 0.7f;
           
        }
       
    }

    void IniciarJogo()
    {
        camVirtual.SetActive(true);
        SoundManager.instance.TocarBG(0);
        pause = false;
        gameOver = false;
        Time.timeScale = 1f;
        StopAllCoroutines();
        UIManager.instance.ResetTime();
    }
    
    IEnumerator TransicaoScene(int sceneInbluid, float tempo)
    {
        yield return new WaitForSeconds(tempo);
        SceneManager.LoadScene(sceneInbluid);
    }

    void PausaGame() 
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            pause = !pause;
            if (pause)
            {
                Time.timeScale = 0;
                SoundManager.instance.PauseSoundBG();
                SoundManager.instance.TocarFx(10);
            }
            else
            {
                Time.timeScale = 1;
                SoundManager.instance.TocarBG(0);
                SoundManager.instance.TocarFx(10);
            }
        }   
    }

    public bool IsGameOver()
    {
        return gameOver;
    }
}
