using UnityEngine;

public class Destructible : MonoBehaviour
{
    public float destructionTime = 1f;

    void Start() => Destroy(gameObject, destructionTime);

}
