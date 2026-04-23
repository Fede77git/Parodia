using UnityEngine;

public class RotatingHoop : MonoBehaviour
{
    [Tooltip("Velocidad de rotación en grados por segundo")]
    public float rotationSpeed = 45f;

    void Update()
    {
        // Gira el objeto alrededor de su eje Y (Vertical) a la velocidad indicada
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }
}
