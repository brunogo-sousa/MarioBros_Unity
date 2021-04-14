using System.Collections;
using UnityEngine;

namespace MarioUnity.Entidades
{
    public class Enemy : MonoBehaviour
    {
        private Rigidbody2D rbPlayer;
        private Animator anima;
        [SerializeField] GameObject groundCheck;
        [SerializeField] LayerMask thisFloor;
        bool indoEsquerda = false;
        private bool isOnGround;
        private int valueGoomba;
        void Awake()
        {
            anima = GetComponent<Animator>();
            valueGoomba = 100;
            groundCheck = transform.GetChild(1).gameObject;
        }

        void Update()
        {
            isOnGround = Physics2D.OverlapCircle(groundCheck.transform.position, 0.05f, thisFloor);
            if (!isOnGround)
            {
                GetComponent<Rigidbody2D>().gravityScale = 1;
            }
            Patrol();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.transform.CompareTag("Pipe"))
            {
                indoEsquerda = !indoEsquerda;

            }

            Death(other);
            KeepOnFloor(other);

        }

        IEnumerator Afastar(Rigidbody2D rb)
        {
            rb.velocity = new Vector2(0, 300 * Time.deltaTime);
            yield return new WaitForSeconds(0.10f);
            AutoDeath();
        }

        void AutoDeath()
        {
            Destroy(gameObject);
           
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            if(Application.isPlaying)
                Gizmos.DrawSphere(groundCheck.transform.position, 0.05f);
        }


        private void KeepOnFloor(Collider2D collider2d)
        {
            if (collider2d.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                GetComponent<Rigidbody2D>().gravityScale = 0;
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
        }
        
        private void Patrol()
        {
            if (indoEsquerda == true)
            {
                transform.Translate(Vector2.right * 2 * Time.deltaTime);
                gameObject.GetComponent<SpriteRenderer>().flipX = true;

            }
            else if (indoEsquerda == false)
            {
                transform.Translate(Vector2.left * 2 * Time.deltaTime);
                gameObject.GetComponent<SpriteRenderer>().flipX = false;

            }
        }
        private void Death(Collider2D attacker)
        {
            if (attacker.transform.name == "PointCheckGround" && Mario.instance.IsTouchingGround() == false && Mario.instance.isAlive == true)
            {

                rbPlayer = attacker.GetComponentInParent<Rigidbody2D>();

                if (Mario.instance.ThisVulnerable() == false && Mario.instance.isAlive==true && Mario.instance.IsTouchingGround() == false)
                {
                    SoundManager.instance.TocarFx(8);
                    float xCaixa = groundCheck.transform.position.x - attacker.transform.localPosition.x;
                    float yCaixa = groundCheck.transform.position.y - attacker.transform.localPosition.y;
                    UIManager.instance.ShowValue(valueGoomba.ToString(), xCaixa, yCaixa);
                    UIManager.instance.AddValueTotalScore(valueGoomba);
                    anima.Play("Death_Goomba");
                    StartCoroutine(Afastar(rbPlayer));
                }

            }
        }
    }
}