using UnityEngine;

public class DebugGoals : MonoBehaviour
{
    private void Update()
    {
        if (ScoreManager.instance == null) return;

        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1)) ScoreManager.instance.AddGoal(1);
        if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2)) ScoreManager.instance.AddGoal(2);
        if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3)) ScoreManager.instance.AddGoal(3);
        if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4)) ScoreManager.instance.AddGoal(4);
    }
}
