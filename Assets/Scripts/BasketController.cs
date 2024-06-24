using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class BasketController : MonoBehaviour
{
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

        
        Vector3 direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector3 move = direction * MoveSpeed * Time.deltaTime;

        //transform.position += direction * MoveSpeed * Time.deltaTime;
        //transform.LookAt(transform.position + direction);
        characterController.Move(move);

        if (direction != Vector3.zero)
        {
            transform.forward = direction;
        }


        if (IsBallInHands)
        {

            
            if (Input.GetKey(KeyCode.Space))
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

           
            if (Input.GetKeyUp(KeyCode.Space))
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
