using UnityEngine;

public class Mushroom : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rbMushroom;
    [SerializeField] private GameObject checkFloor;
    [SerializeField] private LayerMask thisFloor;
    [SerializeField]private bool releaseMove;
    private bool GoingToLeft = false;
    private bool isOnGround = false;
    int pointValue = 1000;

    private void Awake()
    {
        rbMushroom = GetComponentInParent<Rigidbody2D>();
        releaseMove = false; 
    }
   
    void Update()
    {
        IsOnGround();
        ApplyGravity();
        Move();
    }
  
    //used at the end of the mushroom animation timeline
    void EventReleaseMove()
    {
        releaseMove = true;
    }
    void EventEnableCollider2D()
    {
        GetComponent<Collider2D>().enabled = true;
        
    }

    void OnTriggerEnter2D(Collider2D outro)
    {
        if (outro.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            GetComponentInParent<Rigidbody2D>().gravityScale = 0;
            GetComponentInParent<Rigidbody2D>().velocity = Vector2.zero;

        }
        if (outro.CompareTag("Pipe"))
        {
            GoingToLeft = !GoingToLeft;
        }
        if (outro.CompareTag("Player"))
        {
            float xPlayer = outro.transform.localPosition.x ;
            float yPlayer = outro.transform.localPosition.y;
            UIManager.instance.ShowValue(pointValue.ToString(), xPlayer, yPlayer);
            UIManager.instance.AddValueTotalScore(pointValue);
        }
    }
    
    public bool IsOnGround()
    {
        isOnGround = Physics2D.OverlapCircle(checkFloor.transform.position, 0.05f, thisFloor);
        return isOnGround;
    }

    private void Move()
    {
        if (CanMove() == true && GoingToLeft == false)
        {
            transform.parent.Translate(Vector2.right * 2 * Time.deltaTime);
            rbMushroom.bodyType = RigidbodyType2D.Dynamic;

        }
        else if (GoingToLeft)
        {
            transform.parent.Translate(Vector2.left * 2 * Time.deltaTime);
            rbMushroom.bodyType = RigidbodyType2D.Dynamic;
        }
    }

    private void ApplyGravity()
    {
        if (!isOnGround)
        {
            GetComponentInParent<Rigidbody2D>().gravityScale = 1;
        }
    }

    private bool CanMove()
    {
        return releaseMove;
    }
}
