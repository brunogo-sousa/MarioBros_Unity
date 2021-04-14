using UnityEngine;
using UnityEngine.SceneManagement;
using MarioUnity.Entidades;

public class UndergroundPipeEntry : MonoBehaviour
{
    private int playOnceMusic;
    private GameObject player;
    private bool enterThroughPipe = false;
    void Start()
    {
        player = GameObject.Find("Mario");
        playOnceMusic = 0;
    }

    void Update()
    {
        if (enterThroughPipe==true)
            player.transform.Translate(Vector2.down * Time.deltaTime);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        UndergroundEntryCheck(other);
    }
   

    private void UndergroundLoading()
    {
        SceneManager.LoadScene((int)ManagerStage.Screen.Underground_Phase1);
        Mario.instance.EnableJoystick();
       
    }

    private void LoadCongratulations()
    {
        SceneManager.LoadScene(ManagerStage.Screen.Congratulations.ToString());
    }

    private void UndergroundEntryCheck(Collider2D other)
    {
        if (other.transform.CompareTag("Player")) 
        {
            if ((Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) && ManagerStage.instance.WhichStage() != (int)ManagerStage.Screen.Underground_Phase1 && Mario.instance.IsMove() == false)
            {
                UIManager.instance.SaveGeneralData();
                Mario.instance.DisableJoystick();
                if (playOnceMusic == 0)
                    SoundManager.instance.TocarFx(6);
                playOnceMusic = 1;
                player.GetComponent<SpriteRenderer>().sortingOrder = 1;
                player.GetComponent<Collider2D>().enabled = false;
                Time.timeScale = 0.1f;
                enterThroughPipe = true;
                Invoke("UndergroundLoading", 0.2f);
            }
            else if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && ManagerStage.instance.WhichStage() == (int)ManagerStage.Screen.Underground_Phase1)
            {
                if (playOnceMusic == 0)
                    SoundManager.instance.TocarFx(6);
                playOnceMusic = 1;

                Invoke("LoadCongratulations", 1f);
            }
        }
    }
}
