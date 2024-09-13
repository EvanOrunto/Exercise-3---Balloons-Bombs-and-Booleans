using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour {
    private bool gameOver;
    private bool isLowEnough = true;
    private float upBound = 14f;
    private float gravityModifier = 1.5f;

    private Rigidbody playerRb;

    [Header("Force")]
    [SerializeField] private float floatForce;

    [Header("Particle Systems")]
    [SerializeField] private ParticleSystem explosionParticle;
    [SerializeField] private ParticleSystem fireworksParticle;

    private AudioSource playerAudio;
    [Header("Sound Effects")]
    [SerializeField] private AudioClip moneySound;
    [SerializeField] private AudioClip explodeSound;
    [SerializeField] private AudioClip bounceSound;

    private void Awake() {
        playerRb = GetComponent<Rigidbody>();
        playerAudio = GetComponent<AudioSource>();
    }
    // Start is called before the first frame update
    void Start() {
        Physics.gravity *= gravityModifier;
        // Apply a small upward force at the start of the game
        playerRb.AddForce(Vector3.up * 5, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update() {
        CheckBound();

        // While space is pressed and player is low enough, float up
        if (Input.GetKey(KeyCode.Space) && !gameOver && isLowEnough) {
            playerRb.AddForce(Vector3.up * floatForce);
        }
    }

    private void OnCollisionEnter(Collision other) {
        // if player collides with bomb, explode and set gameOver to true
        if (other.gameObject.CompareTag("Bomb")) {
            explosionParticle.Play();
            playerAudio.PlayOneShot(explodeSound, 1.0f);
            gameOver = true;
            Debug.Log("Game Over!");
            Destroy(other.gameObject);
        }
        // if player collides with money, fireworks
        else if (other.gameObject.CompareTag("Money")) {
            fireworksParticle.Play();
            playerAudio.PlayOneShot(moneySound, 1.0f);
            Destroy(other.gameObject);
        }
        // If player hits the ground add an impulse force
        else if (other.gameObject.CompareTag("Ground")) {
            playerRb.AddForce(Vector3.up * 7, ForceMode.Impulse);
            playerAudio.PlayOneShot(bounceSound, 1.0f);
        }

    }

    private void CheckBound() {
        isLowEnough = transform.position.y < upBound;
    }

    public bool GetGameOver() {
        return gameOver;
    }
}
