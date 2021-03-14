using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rd2d;
    public float speed;
    public Text score;
    private int scoreValue = 0;
    public Text winText;
    private int lives = 3;
    public Text livesText;
    public Text loseText;
    public AudioClip musicClipOne;
    public AudioClip musicClipTwo;
    public AudioSource musicSource;
    private bool isOnGround;
    public Transform groundcheck;
    public float checkRadius;
    public LayerMask allGround;
    private bool facingRight;
    
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();
        Setscore ();
        winText.text ="";
        loseText.text ="";
        SetLivesText ();
        anim = gameObject.GetComponent<Animator>();
        bool facingRight = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float verMovement = Input.GetAxis("Vertical");

        rd2d.AddForce(new Vector2(hozMovement * speed, verMovement * speed));
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
        isOnGround = Physics2D.OverlapCircle(groundcheck.position, checkRadius, allGround);

        if (facingRight == true && hozMovement > 0)
        {
            Flip();
        }
        else if (facingRight == false && hozMovement < 0)
        {
            Flip();
        }
        if (scoreValue == 4)
        {
            lives = 3;
            SetLivesText ();
        }
    }

   private void OnCollisionEnter2D(Collision2D collision)
    {
       if (collision.collider.tag == "Coin")
       {
           scoreValue += 1;
           score.text = scoreValue.ToString();
           Destroy(collision.collider.gameObject);
       }
        if (collision.collider.tag == ("Enemy"))
        {
            lives = lives - 1;
            SetLivesText ();
            Destroy(collision.collider.gameObject);
        }
        if (scoreValue >= 8)
        {
            winText.text = "You win! Game by Anabeth Koopman";
            musicSource.clip = musicClipOne;
            musicSource.Play();
            musicSource.loop = false;
        }

        if (scoreValue <= 7)
        {
            musicSource.clip = musicClipTwo;
            musicSource.Play();
            musicSource.loop = true;
        }
   }

private void OnCollisionExit2D(Collision2D collision)
{
        if (scoreValue == 4)
        {
            transform.position = new Vector3(50.0f, 1.0f, 0.0f);
            lives = 3;
            SetLivesText ();
        }
}

    void Setscore ()
    {
        score.text = scoreValue.ToString ();
    }

    void SetLivesText ()
    {
        livesText.text = "Lives: " + lives.ToString ();
        if (lives<=0)
        {
            loseText.text = "Nice try";
        }
        if (lives ==0)
        {
            Destroy(gameObject);
        }
    }

    void Flip()
   {
     facingRight = !facingRight;
     Vector2 Scaler = transform.localScale;
     Scaler.x = Scaler.x * -1;
     transform.localScale = Scaler;
   }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground" && isOnGround)
        {
            if (Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, 3), ForceMode2D.Impulse);
            }
        }
    }

       void Update()
    { 
        float hozMovement = Input.GetAxis("Horizontal");
        if (!isOnGround)
        {
            anim.SetInteger("State", 2);
        }
        if (isOnGround)
        {
            anim.SetInteger("State", 1);
        }
        if (hozMovement == 0 && isOnGround)
        {
            anim.SetInteger("State", 0);
        }
    }
}