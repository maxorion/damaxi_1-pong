using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpanderControl : MonoBehaviour
{
    // Rigidbody 2D
    private Rigidbody2D rigidBody2D;

    private int randMoveThreshold;
    private int randMoveCounter;

    private int lifeTime;

    public AudioSource powerSound;

    // Start is called before the first frame update
    void Start()
    {

        rigidBody2D = GetComponent<Rigidbody2D>();
        randMoveThreshold = Random.Range(120, 300);
    }

    // Update is called once per frame
    void Update()
    {
        randMoveCounter++;
        if (randMoveCounter >= randMoveThreshold)
        {
            randMoveCounter = 0;
            PushExpander();
        }

        lifeTime++;
        if (lifeTime > 6000)
        {
            DisableExpander();
        }
    }

    void ResetExpander()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();

        // Reset posisi menjadi (0,0)
        transform.position = new Vector2(Random.Range(-11, 11), Random.Range(-3, 3));


        // Reset kecepatan menjadi (0,0)
        rigidBody2D.velocity = Vector2.zero;
    }

    void PushExpander()
    {
        float yInitialForce = Random.Range(15, 50);
        float xInitialForce = Random.Range(15, 50);
        int initialSpeed = Random.Range(5, 10);

        // Tentukan nilai komponen y dari gaya dorong antara -yInitialForce dan yInitialForce
        float yRandomInitialForce = Random.Range(-yInitialForce, yInitialForce);

        // Tentukan nilai acak antara 0 (inklusif) dan 2 (eksklusif)
        float randomDirection = Random.Range(0, 2);

        // Jika nilainya di bawah 1, bola bergerak ke kiri. 
        // Jika tidak, bola bergerak ke kanan.
        if (randomDirection < 1.0f)
        {
            // Gunakan gaya untuk menggerakkan bola ini.
            // rigidBody2D.AddForce(new Vector2(-xInitialForce, yRandomInitialForce));
            rigidBody2D.velocity = (new Vector2(-xInitialForce, yRandomInitialForce).normalized * initialSpeed);
        }
        else
        {
            // rigidBody2D.AddForce(new Vector2(xInitialForce, yRandomInitialForce));
            rigidBody2D.velocity = (new Vector2(xInitialForce, yRandomInitialForce).normalized * initialSpeed);
        }

    }

    public void StartExpander()
    {
        gameObject.SetActive(true);

        // Kembalikan bola ke posisi semula
        ResetExpander();

        // Setelah 2 detik, berikan gaya ke bola
        PushExpander();
    }

    void DisableExpander()
    {
        gameObject.SetActive(false);
    }

    // Ketika bola beranjak dari sebuah tumbukan, rekam titik tumbukan tersebut
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name.Equals("Player1") || other.gameObject.name.Equals("Player2"))
        {
            // lastContactPoint = collision.GetContact(0);
            // player2.IncrementScore();
            other.gameObject.SendMessage("StartExpand", 2.0f, SendMessageOptions.RequireReceiver);

            powerSound.Play();

            DisableExpander();
        }
    }

    private void OnTriggerEnter2D(Collider2D anotherCollider)
    {
        // Jika objek tersebut bernama "Ball":
        if (anotherCollider.name == "LeftWall" || anotherCollider.name == "RightWall")
        {
            rigidBody2D.velocity = (new Vector2(-rigidBody2D.velocity.x, rigidBody2D.velocity.y).normalized * rigidBody2D.velocity.magnitude);
        }
    }
}
