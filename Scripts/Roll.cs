using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roll : MonoBehaviour
{
    private Rigidbody rb;

    // parametrized for user to change speed of rock roll
    public float vX = 1f;
    public float vZ = 1f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // fix later
        if ((transform.position.z > 70 || transform.position.z < 0) || (transform.position.x > 125 || transform.position.x < 0)) {
            if (transform.position.z > 70 || transform.position.z < 0) {
                vZ = -vZ;
                rb.velocity = new Vector3(vX, 0, vZ);
                print("changed direction");
            }
            
            if (transform.position.x > 125 || transform.position.x < 0) {
                vX = -vX;
                rb.velocity = new Vector3(vX, 0, vZ);
            }
        }
        else {
            rb.velocity = new Vector3(vX, 0, vZ);  
        }
    }
}
