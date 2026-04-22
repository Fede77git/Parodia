using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; 

[RequireComponent(typeof(Rigidbody))] 
public class BasketController : MonoBehaviour
{
    public float MoveSpeed = 7;
    public Transform Ball;
    public Transform PosDribble;
    public Transform PosOverHead;
    public Transform Arms;
    public Transform Target;
   
    private bool IsBallInHands = false;
    private bool IsBallFlying = false;
    private float T = 0;

    private Rigidbody rb;
    private Collider ballCollider;
    private Collider playerCollider;

    public InputActionReference moveAction;
    public InputActionReference shootAction;
   
    private Vector2 moveInput;
    private bool isShooting;
    private bool wasShooting;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation; 
        
        ballCollider = Ball.GetComponent<Collider>();
        playerCollider = GetComponent<Collider>();

    }

    private void OnEnable()
    {
        if (moveAction != null) moveAction.action.Enable();
        if (shootAction != null) shootAction.action.Enable();
    }

    private void OnDisable()
    {
        if (moveAction != null) moveAction.action.Disable();
        if (shootAction != null) shootAction.action.Disable();
    }

    void Update()
    {
        if (moveAction != null && shootAction != null)
        {
            moveInput = moveAction.action.ReadValue<Vector2>();
            isShooting = shootAction.action.IsPressed();
        }

        if (IsBallInHands)
        {
            if (isShooting)
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

         
            if (wasShooting && !isShooting)
            {
                IsBallInHands = false;
                IsBallFlying = true;
                T = 0;
            }
        }

        wasShooting = isShooting; 

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
                ballCollider.isTrigger = false; 
            }
        }
    }

    void FixedUpdate()
    {

        Vector3 direction = new Vector3(moveInput.x, 0, moveInput.y);
        
        Vector3 targetVelocity = direction * MoveSpeed;
        targetVelocity.y = rb.velocity.y; 
        rb.velocity = targetVelocity;

        if (direction != Vector3.zero)
        {

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, Time.fixedDeltaTime * 15f));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsBallInHands && !IsBallFlying && other.CompareTag("Ball"))
        {
            BasketController[] allPlayers = FindObjectsOfType<BasketController>();
            foreach (BasketController player in allPlayers)
            {
                player.IsBallInHands = false;
                player.IsBallFlying = false;
            }

            IsBallInHands = true;
            Ball.GetComponent<Rigidbody>().isKinematic = true;
            Ball.GetComponent<Rigidbody>().useGravity = false;

            ballCollider.isTrigger = true;
            
            Ball.position = PosDribble.position;
        }
    }
}
