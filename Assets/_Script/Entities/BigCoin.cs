using UnityEngine;

public class BigCoin : MonoBehaviour
{
    private int coinValue = 200;
   
    private void OnTriggerEnter2D(Collider2D outro)
    {
   
        if (outro.CompareTag("Player"))
        {
            UIManager.instance.AddValueTotalCoin();
            UIManager.instance.AddValueTotalScore(coinValue);
            SoundManager.instance.TocarFx(2);
            Destroy(gameObject);
          
        }
    }
}
