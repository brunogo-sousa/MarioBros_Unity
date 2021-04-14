using UnityEngine;

namespace MarioUnity.Entidades
{
    public class BreakableWall : MonoBehaviour
    {
        [SerializeField] GameObject particulaMuro;
        [SerializeField] Transform muro;
        public static bool estaNoMuro;
        AudioSource controlAudio;
        void Start()
        {
            controlAudio = gameObject.GetComponent<AudioSource>();
        }

        private void OnTriggerEnter2D(Collider2D outro)
        {
            if (outro.transform.name == "PointCheckGround" || Mario.instance.isAlive == false)
            {
                estaNoMuro = true;
                GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;

            }
            else
            {
                GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            }
            if (outro.transform.CompareTag("Head") && estaNoMuro == false && Mario.instance.IsBig() == true)
            {
                Soundplay(9);
                Instantiate(particulaMuro, muro);
                Invoke("DestroyObj", 0.1f);
               
            }
            

            if (outro.transform.CompareTag("Head") && estaNoMuro == false && Mario.instance.IsBig() == false)
            {
                Soundplay(5);
            }
        }

        private void OnTriggerExit2D(Collider2D outro)
        {
            if (outro.transform.name == "PointCheckGround")
            {
                estaNoMuro = false;
            }
        }

        private void OnTriggerStay2D(Collider2D outro)
        {
            if (outro.transform.name == "PointCheckGround")
            {
                estaNoMuro = true;
            }
            else
            {
                estaNoMuro = false;

            }
        }


        private void DestroyObj()
        {
            Destroy(this.gameObject);

        }

        private void Soundplay(int index)
        {
            controlAudio.clip = SoundManager.instance.GetClipFx(index);
            controlAudio.Play();
        }
    }
}
