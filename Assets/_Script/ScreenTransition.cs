using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenTransition : MonoBehaviour
{
    void Start()
    {
        Invoke("NextScreen",2f);
    }

    void NextScreen()
    {
        SceneManager.LoadScene(2);
    }
}
