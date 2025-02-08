using UnityEngine;

public class LookTama : MonoBehaviour
{
    public TamagochiUI tama;
    void Update()
    {
        transform.position = new Vector3(-tama.transform.position.x, transform.position.y, transform.position.z);

    }
}
