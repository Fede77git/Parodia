using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class BasketController : MonoBehaviour
{

    public enum PlayerNumber { Player1, Player2 }
    public PlayerNumber playerNumber;


    public float MoveSpeed = 7;
    public Transform Ball;
    public Transform PosDribble;
    public Transform PosOverHead;
    public Transform Arms;
    public Transform Target;

   
    private bool IsBallInHands = true;
    private bool IsBallFlying = false;
    private float T = 0;

    private CharacterController characterController;
    private Collider ballCollider;
    private Collider playerCollider;
    private Vector3 initialBallPosition;




    void Start()
    {
        characterController = GetComponent<CharacterController>();
        ballCollider = Ball.GetComponent<Collider>();
        playerCollider = GetComponent<Collider>();
        initialBallPosition = Ball.position;


    }

    void Update()
    {

        Vector3 direction = Vector3.zero;
        KeyCode shootKey = KeyCode.None;

        if (playerNumber == PlayerNumber.Player1)
        {
            direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            shootKey = KeyCode.V;
        }
        else if (playerNumber == PlayerNumber.Player2)
        {
            direction = new Vector3(Input.GetAxisRaw("Horizontal2"), 0, Input.GetAxisRaw("Vertical2"));
            shootKey = KeyCode.M;
        }

        Vector3 move = direction * MoveSpeed * Time.deltaTime;
        characterController.Move(move);

        if (direction != Vector3.zero)
        {
            transform.forward = direction;
        }

        if (IsBallInHands)
        {
            if (Input.GetKey(shootKey))
            {
                Ball.position = PosOverHead.position;
                Arms.localEulerAngles = Vector3.right * 180;
                transform.LookAt(Target.parent.position);
            }
            else
            {
                Ball.position = PosDribble.position + Vector3.up * Mathf.Abs(Mathf.Sin(Time.time * 5));
                Arms.localEulerAngles = Vector3.right * 0;
            }

            if (Input.GetKeyUp(shootKey))
            {
                IsBallInHands = false;
                IsBallFlying = true;
                T = 0;
            }
        }

        if (IsBallFlying)
        {
            T += Time.deltaTime;
            float duration = 0.66f;
            float t01 = T / duration;

            Vector3 A = PosOverHead.position;
            Vector3 B = Target.position;
            Vector3 pos = Vector3.Lerp(A, B, t01);
            Vector3 arc = Vector3.up * 5 * Mathf.Sin(t01 * 3.14f);

            Ball.position = pos + arc;

            if (t01 >= 1)
            {
                IsBallFlying = false;
                Ball.GetComponent<Rigidbody>().isKinematic = false;
                StartCoroutine(RespawnBallCoroutine());

            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsBallInHands && !IsBallFlying && other.CompareTag("Ball"))
        {
            IsBallInHands = true;
            Ball.GetComponent<Rigidbody>().isKinematic = true;
            ballCollider.enabled = false;
            playerCollider.enabled = false;
            StartCoroutine(EnableCollidersAfterDelay());
        }
    }

    private IEnumerator RespawnBallCoroutine()
    {
        yield return new WaitForSeconds(0.1f); 
        ballCollider.enabled = false;
        playerCollider.enabled = false;


        //Ball.position = initialBallPosition;

        yield return new WaitForSeconds(0.1f); 
        ballCollider.enabled = true; 
        playerCollider.enabled = true; 
    }

    private IEnumerator EnableCollidersAfterDelay()
    {
        yield return new WaitForSeconds(0.1f);
        ballCollider.enabled = true;
        playerCollider.enabled = true;
    }
}
