using UnityEngine;

public class Playroom : MonoBehaviour
{
    private bool wasActive = false;

    void Update()
    {
        if (gameObject.activeSelf && !wasActive)
        {
            wasActive = true;
            OnPlayroomActivated();
        }
        else if (!gameObject.activeSelf && wasActive)
        {
            wasActive = false; // Reset when deactivated
        }
    }

    private void OnPlayroomActivated()
    {
        Debug.Log("Playroom is now active!");
    }
}
