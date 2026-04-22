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

   
    private bool IsBallInHands = false;
    private bool IsBallFlying = false;
    private float T = 0;

    private CharacterController characterController;
    private Collider ballCollider;
    private Collider playerCollider;
    //private Vector3 initialBallPosition;




    void Start()
    {
        characterController = GetComponent<CharacterController>();
        ballCollider = Ball.GetComponent<Collider>();
        playerCollider = GetComponent<Collider>();
        //initialBallPosition = Ball.position;


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

        Vector3 move = direction * MoveSpeed;
        move.y = -9.81f; // Aplicamos gravedad constante hacia abajo
        characterController.Move(move * Time.deltaTime);

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
                Ball.GetComponent<Rigidbody>().useGravity = true;
                ballCollider.isTrigger = false; // Reactivamos colisiones físicas al terminar el tiro

            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsBallInHands && !IsBallFlying && other.CompareTag("Ball"))
        {
            // Forzar a los demás jugadores a soltar la pelota si la tenían
            BasketController[] allPlayers = FindObjectsOfType<BasketController>();
            foreach (BasketController player in allPlayers)
            {
                player.IsBallInHands = false;
                player.IsBallFlying = false;
            }

            IsBallInHands = true;
            Ball.GetComponent<Rigidbody>().isKinematic = true;
            Ball.GetComponent<Rigidbody>().useGravity = false;

            // Hacer la pelota Trigger evita que actúe como un obstáculo físico (evita flotar),
            // pero permite que otro jugador pueda tocarla para robarla.
            ballCollider.isTrigger = true;
            
            Ball.position = PosDribble.position;
        }
    }
}
