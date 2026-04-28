using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; 
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class BasketController : MonoBehaviour
{
    public float MoveSpeed = 7;
    public int playerID = 1;
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
    public InputActionReference dashAction;
   
    public Slider powerSlider;
    public float maxShotForce = 2f;
    public float chargeRate = 2f;
    public float dashForce = 30f;
    public float knockbackForce = 20f;
    public float maxThrowDistance = 25f;
    private float currentShotForce = 0f;
    private float shotForceApplied = 0f;
    public float blockMoveTimer = 0f;
    private float pickupCooldown = 0f;
    private Vector3 targetFlyPos;

    private Vector2 moveInput;
    private bool isShooting;
    private bool wasShooting;
    private Vector3 startFlyPos;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY; 
        
        ballCollider = Ball.GetComponent<Collider>();
        playerCollider = GetComponent<Collider>();

        if (powerSlider != null)
        {
            powerSlider.gameObject.SetActive(false);
            powerSlider.value = 0f;
        }
    }

    private void OnEnable()
    {
        if (moveAction != null) moveAction.action.Enable();
        if (shootAction != null) shootAction.action.Enable();
        if (dashAction != null) dashAction.action.Enable();
    }

    private void OnDisable()
    {
        if (moveAction != null) moveAction.action.Disable();
        if (shootAction != null) shootAction.action.Disable();
        if (dashAction != null) dashAction.action.Disable();
    }

    void Update()
    {
        if (pickupCooldown > 0f)
        {
            pickupCooldown -= Time.deltaTime;
        }

        if (moveAction != null && moveAction.action != null)
        {
            moveInput = moveAction.action.ReadValue<Vector2>();
        }
        
        if (shootAction != null && shootAction.action != null)
        {
            isShooting = shootAction.action.IsPressed();
        }

        if (dashAction != null && dashAction.action != null)
        {
            if (dashAction.action.WasPressedThisFrame() && blockMoveTimer <= 0)
            {
                rb.velocity = Vector3.zero;
                rb.AddForce(transform.forward * dashForce, ForceMode.Impulse);
                blockMoveTimer = 0.5f;
            }
        }

        if (IsBallInHands)
        {
            if (isShooting)
            {
                Ball.position = PosOverHead.position;
                Arms.localEulerAngles = Vector3.right * 180;
                
                Vector3 lookPos = Target.position;
                lookPos.y = transform.position.y;
                transform.LookAt(lookPos);

                currentShotForce += chargeRate * Time.deltaTime;
                currentShotForce = Mathf.Clamp(currentShotForce, 0f, maxShotForce);
                if (powerSlider != null)
                {
                    powerSlider.gameObject.SetActive(true);
                    powerSlider.value = currentShotForce / maxShotForce;
                }
            }
            else
            {
                Ball.position = PosDribble.position + Vector3.up * Mathf.Abs(Mathf.Sin(Time.time * 5));
                Arms.localEulerAngles = Vector3.right * 0;

                if (!wasShooting)
                {
                    currentShotForce = 0f;
                    if (powerSlider != null)
                    {
                        powerSlider.value = 0f;
                        powerSlider.gameObject.SetActive(false);
                    }
                }
            }

            if (wasShooting && !isShooting)
            {
                IsBallInHands = false;
                IsBallFlying = true;
                T = 0;
                startFlyPos = PosOverHead.position;
                shotForceApplied = Mathf.Max(0.01f, currentShotForce);
                
                Vector3 horizDir = Target.position - startFlyPos;
                horizDir.y = 0;
                float idealDist = horizDir.magnitude;
                if (idealDist > 0.001f) horizDir.Normalize();
                
                float powerRatio = shotForceApplied / maxShotForce;
                float actualDist = powerRatio * maxThrowDistance;
                
                float tolerance = Mathf.Clamp(idealDist * 0.15f, 0.2f, 2f);
                
                if (Mathf.Abs(actualDist - idealDist) <= tolerance)
                {
                    targetFlyPos = Target.position;
                }
                else
                {
                    targetFlyPos = startFlyPos + horizDir * actualDist;
                    targetFlyPos.y = 0.5f; 
                }

                currentShotForce = 0f;
                if (powerSlider != null)
                {
                    powerSlider.value = 0f;
                    powerSlider.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            Arms.localEulerAngles = Vector3.right * 0;
            if (powerSlider != null && powerSlider.gameObject.activeSelf)
            {
                powerSlider.value = 0f;
                powerSlider.gameObject.SetActive(false);
            }
        }

        wasShooting = isShooting; 

        if (IsBallFlying)
        {
            T += Time.deltaTime;
            float duration = Mathf.Lerp(0.8f, 0.64f, shotForceApplied / maxShotForce);
            float t01 = T / duration;

            Vector3 pos = Vector3.Lerp(startFlyPos, targetFlyPos, t01);
            float arcHeight = Mathf.Lerp(4f, 5.1f, shotForceApplied / maxShotForce);
            Vector3 arc = Vector3.up * arcHeight * Mathf.Sin(t01 * 3.14f);

            Ball.position = pos + arc;

            if (t01 >= 1)
            {
                IsBallFlying = false;
                Rigidbody ballRb = Ball.GetComponent<Rigidbody>();
                ballRb.isKinematic = false;
                ballRb.useGravity = true;
                
                Vector3 horizVel = (targetFlyPos - startFlyPos) / duration;
                float vertVel = -arcHeight * Mathf.PI / duration;
                
                ballRb.velocity = new Vector3(horizVel.x, vertVel, horizVel.z) * 0.15f;
                
                ballCollider.isTrigger = false; 
            }
        }
    }

    void FixedUpdate()
    {
        if (blockMoveTimer > 0)
        {
            blockMoveTimer -= Time.fixedDeltaTime;
            return;
        }

        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;

        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();


        Vector3 direction = camForward * moveInput.y + camRight * moveInput.x;
        
        float currentSpeed = MoveSpeed;
        if (IsBallInHands && isShooting)
        {
            currentSpeed *= 0.5f;
        }

        Vector3 targetVelocity = direction * currentSpeed;
        targetVelocity.y = rb.velocity.y; 
        rb.velocity = targetVelocity;

        if (direction != Vector3.zero)
        {

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, Time.fixedDeltaTime * 15f));
        }
    }

    public void DropBall()
    {
        if (IsBallInHands)
        {
            IsBallInHands = false;
            isShooting = false;
            wasShooting = false;
            currentShotForce = 0f;
            pickupCooldown = 1.5f;
            if (powerSlider != null)
            {
                powerSlider.value = 0f;
                powerSlider.gameObject.SetActive(false);
            }
            
            Rigidbody ballRb = Ball.GetComponent<Rigidbody>();
            if (ballRb != null)
            {
                ballRb.isKinematic = false;
                ballRb.useGravity = true;
            }
            if (ballCollider != null)
            {
                ballCollider.isTrigger = false;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        BasketController otherPlayer = collision.gameObject.GetComponent<BasketController>();
        if (otherPlayer != null)
        {

            if (collision.relativeVelocity.magnitude > 2f && blockMoveTimer > 0) 
            {
                if (otherPlayer.IsBallInHands)
                {
                    otherPlayer.DropBall();
                }
                
                Vector3 knockbackDir = (otherPlayer.transform.position - transform.position).normalized;
                knockbackDir.y = 0; 
                
                Rigidbody otherRb = otherPlayer.GetComponent<Rigidbody>();
                otherRb.velocity = Vector3.zero;
                otherRb.AddForce(knockbackDir * knockbackForce, ForceMode.Impulse);
                otherPlayer.blockMoveTimer = 0.5f;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsBallInHands && !IsBallFlying && other.CompareTag("Ball") && pickupCooldown <= 0f)
        {
            BasketController[] allPlayers = FindObjectsOfType<BasketController>();
            foreach (BasketController player in allPlayers)
            {
                player.IsBallInHands = false;
                player.IsBallFlying = false;
            }

            IsBallInHands = true;
            
            BallInfo bInfo = Ball.GetComponent<BallInfo>();
            if (bInfo != null) 
            {
                bInfo.lastPlayerID = this.playerID;
                bInfo.hasScored = false; 
            }

            Ball.GetComponent<Rigidbody>().isKinematic = true;
            Ball.GetComponent<Rigidbody>().useGravity = false;

            ballCollider.isTrigger = true;
            
            Ball.position = PosDribble.position;
        }
    }
}
