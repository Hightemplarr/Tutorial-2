using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{

    private Rigidbody2D rd2d;
    public float speed;
    private int scoreValue;
    public Text score;
    public Text winMessage;
    public float impulse;
    private int livesValue;
    public Text lives;
    Animator anim;
    private float hozMovement;
    private float vertMovement;
    private bool facingRight;
    private bool isOnGround;
    public Transform groundcheck;
    public float checkRadius;
    public LayerMask allGround;
    public float JumpForce;
    private int level;
    public AudioSource musicSource;
    public AudioClip musicOne;
    public AudioClip musicTwo;

    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();
        scoreValue = 0;
        livesValue = 3;
        score.text = "Score: " + scoreValue.ToString();
        lives.text = "Lives: " + livesValue.ToString();
        anim = GetComponent<Animator>();
        facingRight = true;
        level = 1;
        musicSource.clip = musicOne;
        musicSource.Play();
        musicSource.loop = true;
    }

    private void Update()
    {
        hozMovement = Input.GetAxis("Horizontal");
        vertMovement = Input.GetAxis("Vertical");

        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }

        if (scoreValue >= 8 && level == 2)
        {
            winMessage.text = "You win! Game created by Nate Cowan.";
        }
        
        if (scoreValue == 4 && level == 1)
        {
            transform.position = new Vector2(57, 0);
            level++;
            livesValue = 3;
            lives.text = "Lives: " + livesValue.ToString();
        }

        if (livesValue == 0)
        {
            winMessage.text = "You lose. Game created by Nate Cowan.";
            Destroy(gameObject);
        }

        if (hozMovement > 0 || hozMovement < 0)
        {
            if (isOnGround == true)
            {
                anim.SetInteger("State", 1);
            }
        } else if (hozMovement == 0)
        {
            anim.SetInteger("State", 0);
        }

        if (facingRight == false && hozMovement > 0)
        {
            Flip();
        } else if (facingRight == true && hozMovement < 0)
        {
            Flip();
        }

        if (isOnGround == false || Input.GetKey(KeyCode.W))
        {
            anim.SetInteger("State", 2);
        } else if (isOnGround == true && hozMovement == 0)
        {
            anim.SetInteger("State", 0);
        }

        if (Input.GetKey(KeyCode.J))
        {
            scoreValue = 5;
            level = 2;
        }
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float vertMovement = Input.GetAxis("Vertical");
        rd2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));
        isOnGround = Physics2D.OverlapCircle(groundcheck.position, checkRadius, allGround);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Coin")
        {
            scoreValue += 1;
            score.text = "Score: " + scoreValue.ToString();
            Destroy(collision.gameObject);
            if (scoreValue == 8 && level == 2)
            {
                musicSource.loop = false;
                musicSource.clip = musicTwo;
                musicSource.Play();
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground" && isOnGround)
        {
            if (Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, JumpForce), ForceMode2D.Impulse);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            livesValue -= 1;
            lives.text = "Lives: " + livesValue.ToString();
            Destroy(collision.gameObject);
        }
    }
    private void Flip()
    {
        facingRight = !facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1;
        transform.localScale = Scaler;
    }
}
