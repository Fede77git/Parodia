using System.Collections;
using System.Collections.Generic;
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

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {


        Vector3 direction = Vector3.zero;

        if (playerNumber == PlayerNumber.Player1)
        {
            direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

            if (Input.GetKey(KeyCode.V))
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

            if (Input.GetKeyUp(KeyCode.V))
            {
                IsBallInHands = false;
                IsBallFlying = true;
                T = 0;
            }
        }
        else if (playerNumber == PlayerNumber.Player2)
        {
            direction = new Vector3(Input.GetAxisRaw("Horizontal2"), 0, Input.GetAxisRaw("Vertical2"));

            if (Input.GetKey(KeyCode.B))
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

            if (Input.GetKeyUp(KeyCode.B))
            {
                IsBallInHands = false;
                IsBallFlying = true;
                T = 0;
            }
        }

        Vector3 move = direction * MoveSpeed * Time.deltaTime;
        characterController.Move(move);

        if (direction != Vector3.zero)
        {
            transform.forward = direction;
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
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsBallInHands && !IsBallFlying)
        {
            IsBallInHands = true;
            Ball.GetComponent<Rigidbody>().isKinematic = true;
        }
    }
}
