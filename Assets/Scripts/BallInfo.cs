using UnityEngine;

public class BallInfo : MonoBehaviour
{
    // el ultimo jugador que tira la pelota
    public int lastPlayerID = 0;
    public bool hasScored = false;
    public bool isFalling = false;
    public Vector3 realVelocity;
    private Vector3 lastPos;

    void FixedUpdate()
    {

        realVelocity = (transform.position - lastPos) / Time.fixedDeltaTime;
        lastPos = transform.position;
    }
}
