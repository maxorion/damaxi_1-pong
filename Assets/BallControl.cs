using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallControl : MonoBehaviour
{
    // Rigidbody 2D bola
    private Rigidbody2D rigidBody2D;

    // Besarnya gaya awal yang diberikan untuk mendorong bola
    public float xInitialForce;
    public float yInitialForce;

    // Kecepatan awal yang sebenarnya
    public float initialSpeed;

    // Titik asal lintasan bola saat ini
    private Vector2 trajectoryOrigin;

    public AudioSource startGameSound;
    public AudioSource ballHitSound;
    public AudioSource failSound;

    // Start is called before the first frame update
    void Start()
    {
        // TODO : HAPUS INI
        // Ini main-main doang :v
        // addSpeedCounter = 0;

        trajectoryOrigin = transform.position;

        rigidBody2D = GetComponent<Rigidbody2D>();

        // Mulai game
        RestartGame();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void ResetBall()
    {
        // Reset posisi menjadi (0,0)
        transform.position = Vector2.zero;

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
            // rigidBody2D.AddForce(new Vector2(-xInitialForce, yRandomInitialForce));
            rigidBody2D.velocity = (new Vector2(-xInitialForce, yRandomInitialForce).normalized * initialSpeed);
        }
        else
        {
            // rigidBody2D.AddForce(new Vector2(xInitialForce, yRandomInitialForce));
            rigidBody2D.velocity = (new Vector2(xInitialForce, yRandomInitialForce).normalized * initialSpeed);
        }
    }

    void RestartGame()
    {
        // Kembalikan bola ke posisi semula
        ResetBall();


        startGameSound.Play();
        // Setelah 2 detik, berikan gaya ke bola
        Invoke("PushBall", 2);
    }

    // Ketika bola beranjak dari sebuah tumbukan, rekam titik tumbukan tersebut
    private void OnCollisionExit2D(Collision2D collision)
    {
        trajectoryOrigin = transform.position;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        
        ballHitSound.Play();
        
        // Keeps the ball speed constant... I apparently need to do this???
        rigidBody2D.velocity = (new Vector2(rigidBody2D.velocity.x, rigidBody2D.velocity.y).normalized * initialSpeed);
    }

    // Untuk mengakses informasi titik asal lintasan
    public Vector2 TrajectoryOrigin
    {
        get { return trajectoryOrigin; }
    }
}
