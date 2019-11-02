using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Tells Unity If I drage a TapController on my Object
    it's going to Auto-Create a Rigidbody2D
 */
[RequireComponent(typeof(Rigidbody2D))]
public class TapController : MonoBehaviour
{
    public delegate void PlayerDelegate();
    public static event PlayerDelegate OnPlayerDied;
    public static event PlayerDelegate OnPlayerScored;

    public float tapForce = 10;
    public float tiltSmooth = 5;
    public Vector3 startPos;

    public AudioSource tapAudio;
    public AudioSource scoreAudio;
    public AudioSource dieAudio;

    new Rigidbody2D rigidbody;
    Quaternion downRotation; //Rotation, it has 4 Values (Vector4) XYZW from 0-1 Having secure Rotation
    Quaternion forwardRotation;

    GameManager game;

    void Start() {
        rigidbody = GetComponent<Rigidbody2D>();
        downRotation = Quaternion.Euler(0, 0, -90);
        forwardRotation = Quaternion.Euler(0, 0, 35);
        game = GameManager.Instance;
        rigidbody.simulated = true; //False to allow user to hit Start | However, wont work start for some reason
    }

    void OnEnable()
    {
        GameManager.OnGameStarted += OnGameStarted;
        GameManager.OnGameOverConfirmed += OnGameOverConfirmed;
    }

    void OnDisable()
    {
        GameManager.OnGameStarted -= OnGameStarted;
        GameManager.OnGameOverConfirmed -= OnGameOverConfirmed;
    }

    void OnGameStarted()
    {
        rigidbody.velocity = Vector3.zero;
        rigidbody.simulated = true; 
    }

    void OnGameOverConfirmed()
    {
        transform.localPosition = startPos;
        transform.rotation = Quaternion.identity;
    }

    //0 Indicates Left Click | 1 Indicates Right Click
    void Update() {
        if (game.GameOver) return;

        if (Input.GetMouseButtonDown(0)) {
            tapAudio.Play();
            transform.rotation = forwardRotation;
            rigidbody.velocity = Vector3.zero;
            rigidbody.AddForce(Vector2.up * tapForce, ForceMode2D.Force);
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, downRotation, tiltSmooth * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.tag == "ScoreZone")
        {
            //Insert Score Event
            OnPlayerScored(); //event sent to GameManager;
            //Make a Sound ?
            scoreAudio.Play();
        }

        if (collider.gameObject.tag == "DeadZone")
        {
            rigidbody.simulated = false; //Basically, freeze it as it stands
            //Insert a dead event
            OnPlayerDied(); //event sent to GaeManger;
            //Make a Sound ?
            dieAudio.Play();

        }

    }
}
