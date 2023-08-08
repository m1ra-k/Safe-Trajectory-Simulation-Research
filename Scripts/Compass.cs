using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compass : MonoBehaviour
{
    public Transform camera;
    Vector3 vector;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        vector.z = camera.eulerAngles.y;
        transform.localEulerAngles = vector;
    }
}
