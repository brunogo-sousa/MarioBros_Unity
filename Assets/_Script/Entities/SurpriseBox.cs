using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MarioUnity.Entidades
{
    public class SurpriseBox : MonoBehaviour
    {
        Animator anima;
        private bool OpenBox;
        private int coinValueInBox;
        [SerializeField] private int index_BoxItem;
        [SerializeField] private List<GameObject> objsSurpriseBox;
        private void Awake()
        {
            coinValueInBox = 200;
            OpenBox = false;
            anima = GetComponent<Animator>();
        }
       
      
        private void OnTriggerEnter2D(Collider2D other)
        {
           
            if (other.transform.CompareTag("Head") && OpenBox == false && Mario.instance.isAlive==true)
            {
                GameObject item = Instantiate(ItemCaixaSupressa(index_BoxItem), transform);
                anima.Play("blocoVazioAmin");
                RevealsItem(other);
                StartCoroutine(DestroyObjet());
                OpenBox = true;
                
            }
            else if (other.transform.CompareTag("Head") && OpenBox == true && Mario.instance.isAlive == true)
            {
                SoundManager.instance.TocarFx(5);
            }
        }
        IEnumerator DestroyObjet()
        {
            yield return new WaitForSeconds(0.7f);
            Destroy(GameObject.Find("BoxCoin(Clone)"));
        }

        GameObject ItemCaixaSupressa(int numeroItem = 0)
        {
            return objsSurpriseBox[numeroItem];
        }

        private void RevealsItem(Collider2D collider2D)
        {
            if (index_BoxItem == 0)
            {
                SoundManager.instance.TocarFx(2);
                float xCaixa = transform.position.x - collider2D.transform.localPosition.x;
                float yCaixa = transform.position.y - collider2D.transform.localPosition.y;
                UIManager.instance.ShowValue(coinValueInBox.ToString(), xCaixa, yCaixa);
                UIManager.instance.AddValueTotalScore(coinValueInBox);
                UIManager.instance.AddValueTotalCoin();
            }
            else if (index_BoxItem == 1)
            {
                SoundManager.instance.TocarFx(4);
            }
        }
    }
}
