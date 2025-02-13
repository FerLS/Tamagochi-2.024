using UnityEngine;

public class LookTama : MonoBehaviour
{

    public static float xTama;
    void Update()
    {
        transform.position = new Vector3(-xTama, transform.position.y, transform.position.z);

    }
}
