using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallControl : MonoBehaviour
{
    // Rigidbody 2D bola
    private Rigidbody2D rigidBody2D;

    // Besarnya gaya awal yang diberikan untuk mendorong bola
    public float xInitialForce;
    public float yInitialForce;

    // Kecepatan awal yang sebenarnya
    public float initialSpeed;

    public PlayerControl player1;
    public PlayerControl player2;
    public BallControl ball;

    public AudioSource failSound;

    // Start is called before the first frame update
    void Start()
    {

        rigidBody2D = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    void ResetBall()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();

        transform.position = new Vector2(Random.Range(-11, 11), Random.Range(-3, 3));

        // Reset kecepatan menjadi (0,0)
        rigidBody2D.velocity = Vector2.zero;
    }

    void PushBall()
    {
        // Tentukan nilai komponen y dari gaya dorong antara -yInitialForce dan yInitialForce
        float yRandomInitialForce = Random.Range(-yInitialForce, yInitialForce);

        // Tentukan nilai acak antara 0 (inklusif) dan 2 (eksklusif)
        float randomDirection = Random.Range(0, 2);

        // Jika nilainya di bawah 1, bola bergerak ke kiri. 
        // Jika tidak, bola bergerak ke kanan.
        if (randomDirection < 1.0f)
        {
            // Gunakan gaya untuk menggerakkan bola ini.
            rigidBody2D.velocity = (new Vector2(-xInitialForce, yRandomInitialForce).normalized * initialSpeed);
        }
        else
        {
            rigidBody2D.velocity = (new Vector2(xInitialForce, yRandomInitialForce).normalized * initialSpeed);
        }

    }

    public void StartFireBall()
    {
        gameObject.SetActive(true);

        // Kembalikan bola ke posisi semula
        ResetBall();

        // Setelah 2 detik, berikan gaya ke bola
        PushBall();
    }

    void DisableFireBall()
    {
        gameObject.SetActive(false);
    }

    // Ketika bola beranjak dari sebuah tumbukan, rekam titik tumbukan tersebut
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name.Equals("Player1"))
        {
            player2.IncrementScore();
            ball.SendMessage("RestartGame", 2.0f, SendMessageOptions.RequireReceiver);
            failSound.Play();
            DisableFireBall();
        }
        else if (other.gameObject.name.Equals("Player2"))
        {
            player1.IncrementScore();
            ball.SendMessage("RestartGame", 2.0f, SendMessageOptions.RequireReceiver);
            failSound.Play();
            DisableFireBall();
        }
    }

    private void OnTriggerEnter2D(Collider2D anotherCollider)
    {
        if (anotherCollider.name == "LeftWall" || anotherCollider.name == "RightWall")
        {
            DisableFireBall();
        }
    }
}
