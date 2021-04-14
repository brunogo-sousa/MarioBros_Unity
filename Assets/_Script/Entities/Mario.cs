using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MarioUnity.Entidades
{
    public class Mario : MonoBehaviour
    {
        public static Mario instance;
        private bool isOnGround;
        [SerializeField] private GameObject pointCheckGround;
        [SerializeField] private LayerMask thisFloor;
        [SerializeField] public Rigidbody2D rb;
        [SerializeField] Collider2D bodyCollider2d;
        [SerializeField] private int jumpForce = 10;
        [SerializeField] private bool CanJump;
        [SerializeField] private float moveH, moveY;
        [SerializeField] private float maximumSpeed, minimumSpeed;
        [SerializeField] private SpriteRenderer controlImage;
        [SerializeField] private Animator anima;
        public bool isAlive = true;
        [SerializeField] private RuntimeAnimatorController animatorMarioBig;
        [SerializeField] private RuntimeAnimatorController animatorMarioSmall;
        [SerializeField] private int numberMaxJump = 1, currentJumpAmount;
        private bool isBig,isHole;
        [SerializeField] private int quantityLife;
        bool releasePlayerControl = true;
        private bool thisVulnerable = false;
        private Vector2 vetorSprite;//vector representing the player's sprite
        bool foiAtigindo = false;
        private int executionNumber, changeOne;
        [SerializeField]private float offsetValueYcheckGround;
        private int touchSound;
        private AudioSource controlAudio;
       
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

            anima = gameObject.GetComponent<Animator>();
            controlImage = GetComponent<SpriteRenderer>();
            pointCheckGround = transform.GetChild(1).gameObject;
            rb = gameObject.GetComponent<Rigidbody2D>();
            currentJumpAmount = numberMaxJump;
            controlAudio = GetComponent<AudioSource>();
            bodyCollider2d = GetComponent<Collider2D>();
        }

        void Start()
        {
            quantityLife = 1;
            changeOne = 0;
            executionNumber = 0;
            isAlive = true;
            CanJump = false;
            maximumSpeed = 8;
            minimumSpeed = 4;    
            isBig = false;
            //new
            isHole = false;
        }

        void Update()
        {      
            isOnGround = Physics2D.OverlapCircle(pointCheckGround.transform.position, 0.1f, thisFloor);
            //new
            Death();           

            //Adjusts collider automatically
            AutoAjustCollider();

            //Control of activation of the main animations
            CharacterAction();

            ChangeAnimationControl();
        }

        private void FixedUpdate()
        {
            if (ThisVulnerable() == true && isAlive)
            {
                StartCoroutine(LigaDesligaImg());
            }
        }
        void Load(Scene cena, LoadSceneMode modo)
        {
            touchSound = 1;
        }


        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            if(Application.isPlaying)
                Gizmos.DrawSphere(pointCheckGround.transform.position, 0.1f);
        }


        void VelocityZero()
        {
            rb.velocity = Vector2.zero;
        }
        private void FlipX(float moveh)
        {
            if (moveh < 0)
            {
                controlImage.flipX = true;
            }
            else if (moveh > 0)
            {
                controlImage.flipX = false;
            }

        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            DanageSuffered(other);
            //new
            if (other.CompareTag("Finish"))
            {

                isHole = true;
            }

            if (other.transform.CompareTag("Mushroom"))
            {
                
                isBig = true;
                SoundManager.instance.TocarFx(7);
                        
                if (quantityLife >= 2)
                {
                    quantityLife = 2;
                }
                else
                {
                    quantityLife += 1;
                }
                
                Destroy(other.gameObject);

            }

        }

        void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Enemy"))
            {
                if (!isBig && isAlive==true)
                {                    
                    if (changeOne == 0)
                    {
                        SoundManager.instance.TocarFx(6);
                        changeOne++;
                    }
                }
            }


            if (other.gameObject.layer == 8)
            {
                isOnGround = false;
            }

            if (other.CompareTag("Finish") && quantityLife > 0)
            {
                quantityLife = 0;
                isAlive = false;
                SoundManager.instance.StopSoundBG();
                SoundManager.instance.TocarFx(0);

            }
        }

        private void Death()
        {
            if (UIManager.instance.CurrentTime() <= 0 || quantityLife <= 0 || isHole == true)
            {

                if (touchSound > 0 && ManagerStage.instance.WhichStage() == (int)ManagerStage.Screen.Phase1)
                {
                    isAlive = false;

                    rb.velocity = Vector3.zero;
                    bodyCollider2d.enabled = false;
                    releasePlayerControl = false;
                    StartCoroutine(DeathJump());

                    SoundManager.instance.StopSoundBG();
                    SoundManager.instance.TocarFx(0);
                    touchSound--;
                }
            }
        }
        IEnumerator DeathJump()
        {
            rb.velocity = new Vector2(0,10f);
            yield return new WaitForSeconds(0.7f);
            rb.velocity =  new Vector2(0, -10f);
        }
        IEnumerator Invulneravel()
        {
            foiAtigindo = true;
            thisVulnerable = true;
            yield return new WaitForSeconds(2f);
            thisVulnerable = false;
            foiAtigindo = false;
        }
        //informação visual da invesibilidade
        IEnumerator LigaDesligaImg()
        {
            GetComponent<SpriteRenderer>().enabled = false;
            yield return new WaitForSeconds(0.01f);
            GetComponent<SpriteRenderer>().enabled = true;
        }

        private void Soundplay(int index)
        {
            controlAudio.clip = SoundManager.instance.GetClipFx(index);
            controlAudio.Play();
        }

        private void AutoAjustCollider()
        {
            if (isBig)
            {
                rb.simulated = true;
                vetorSprite = gameObject.GetComponent<SpriteRenderer>().sprite.pivot;
                gameObject.GetComponent<BoxCollider2D>().size = vetorSprite / 10;
                gameObject.GetComponent<BoxCollider2D>().offset = new Vector2((vetorSprite.x / 140), -0.18f);

            }
            else
            {
                vetorSprite = gameObject.GetComponent<SpriteRenderer>().sprite.pivot;
                gameObject.GetComponent<BoxCollider2D>().size = vetorSprite / 10;
                gameObject.GetComponent<BoxCollider2D>().offset = new Vector2((vetorSprite.x / 140), -0.12f);
            }

        }


        //todos animações que Mario pode executar
        public enum Animations
        {
            Walk_Mario,
            IDLE_Mario,
            Jump_Mario,
            Run_Mario,
            Death_Mario,
            GrowUpMario
        };
        public void AnimationCall(Animations animation)
        {
            anima.Play(animation.ToString());
        }
        private void CharacterAction()
        {

            if (isAlive == true && GameManager.instance.IsGameOver() == false)
            {
                if (releasePlayerControl == true)
                {
                    MoveCharacter();
                   
                    if (moveH != 0 && isOnGround == true && !Input.GetButtonDown("Jump") && !Input.GetKey(KeyCode.E))
                    {
                        AnimationCall(Animations.Walk_Mario);
                    }
                    else if (RealeasedJump() == true)
                    {
                        if (currentJumpAmount > 0 && Input.GetButtonDown("Jump"))
                        {
                            Soundplay(3);
                            currentJumpAmount--;
                            rb.velocity = new Vector2(rb.velocity.x, jumpForce);

                        }

                        AnimationCall(Animations.Jump_Mario);

                    }
                    else if (Input.GetKey(KeyCode.E) && moveH != 0 && isOnGround)
                    {
                        AnimationCall(Animations.Run_Mario);
                        rb.velocity = new Vector2(moveH * maximumSpeed, rb.velocity.y);
                    }
                    else if (moveH == 0 && moveY == 0 || moveY > 0 && BreakableWall.estaNoMuro == true || isOnGround)
                    {
                        AnimationCall(Animations.IDLE_Mario);
                    }
                    if (Input.GetKeyUp(KeyCode.E) && moveY==0)
                    {
                        VelocityZero(); 
                    }

                    if (isOnGround == true)
                    {
                        currentJumpAmount = numberMaxJump;
                        CanJump = true;
                    }
                    else
                    {
                        currentJumpAmount = 0;
                        CanJump = false;
                    }
                }
            }
            else
            {
                if (quantityLife <= 0 || GameManager.instance.IsGameOver() == true)
                {
                    AnimationCall(Animations.Death_Mario);
                }
            }
        }

        //Refatoração

        private bool RealeasedJump()
        {
            if (currentJumpAmount <= 0)
            {
                return CanJump = false;
            }

            if (Input.GetButton("Jump"))
            {
                return CanJump = true;
            }

            return CanJump = false;
        }
        private void MoveCharacter()
        {
            moveH = Input.GetAxis("Horizontal");
            moveY = rb.velocity.y;
            FlipX(moveH);
            rb.velocity = new Vector2(moveH * minimumSpeed, rb.velocity.y);
        }

        public bool IsTouchingGround()
        {
            return isOnGround;
        }

        public bool ThisVulnerable()
        {
            return thisVulnerable;
        }

       

       

        private void DanageSuffered(Collider2D collider2d)
        {

            if (collider2d.transform.CompareTag("Enemy") && isAlive == true)
            {
                if (quantityLife > 0)
                {
                    isBig = false;
                    if (ThisVulnerable() == false)
                    {
                        Time.timeScale = 0.1f;
                        Invoke("RemoveSlow", 0.02f);
                        quantityLife -= 1;
                    }
                    anima.runtimeAnimatorController = animatorMarioSmall;
                    StartCoroutine(Invulneravel());
                    StopCoroutine(Invulneravel());
                }
                else if (quantityLife <= 0)
                {
                    GameObject.Find("Death_Enemy").GetComponent<Collider2D>().enabled = false;
                }

            }

            
        }

        private void ChangeAnimationControl()
        {

            if (quantityLife >= 2)
            {   //BiG Mario 
                anima.runtimeAnimatorController = animatorMarioBig;
                isBig = true;
                offsetValueYcheckGround = 0.4f;
                pointCheckGround.transform.position = new Vector2(transform.position.x - gameObject.GetComponent<SpriteRenderer>().sprite.bounds.center.x, (transform.position.y - gameObject.GetComponent<SpriteRenderer>().sprite.bounds.max.y) + offsetValueYcheckGround);
            }
            else if (quantityLife == 1)
            {
                //small Mario 
                anima.runtimeAnimatorController = animatorMarioSmall;
                isBig = false;
                offsetValueYcheckGround = -0.84f;
                pointCheckGround.transform.position = new Vector2(transform.position.x - gameObject.GetComponent<SpriteRenderer>().sprite.bounds.center.x, (transform.position.y - gameObject.GetComponent<SpriteRenderer>().sprite.bounds.min.y) + offsetValueYcheckGround);
            }

        }
        IEnumerator TrocaAnimacao()
        {
            anima.Play("growUpMario");
            yield return new WaitForSeconds(1f);
            if (isBig==true)
            {
                anima.runtimeAnimatorController = animatorMarioBig;
            }
            else
            {
                anima.runtimeAnimatorController = animatorMarioSmall;
            }
        }
        private void RemoveSlow()
        {
            Time.timeScale = 1f;
        }

        public bool IsBig()
        {
            return this.isBig;
        }

        public void DisableJoystick()
        {
            releasePlayerControl = false;
        }

        public void EnableJoystick()
        {
            releasePlayerControl = true;
        }
        public bool IsMove()
        {
            if (moveH > 0)
            {
                return true;
            }
            return false;
        }
    }

}
