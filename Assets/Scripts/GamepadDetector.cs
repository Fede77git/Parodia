using UnityEngine;
using UnityEngine.InputSystem;

public class GamepadDetector : MonoBehaviour
{
    private void Start()
    {
        CheckGamepads();
    }

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            CheckGamepads();
        }
    }

    private void CheckGamepads()
    {
        int count = Gamepad.all.Count;
        if (count > 0)
        {
            Debug.Log("Cantidad de mandos conectados: " + count);
            foreach (Gamepad pad in Gamepad.all)
            {
                Debug.Log("Mando detectado: " + pad.displayName + " (ID: " + pad.deviceId + ")");
            }
        }
        else
        {
            Debug.LogWarning("No se ha detectado ningun mando conectado");
        }
    }
}
