using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float spinSpeed = 50f;           // Speed of the Y-axis spin

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0f, spinSpeed * Time.deltaTime, 0f, Space.World);
    }
}
