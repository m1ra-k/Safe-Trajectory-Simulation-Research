using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualMove : MonoBehaviour
{
    Rigidbody rb;
    
    public Transform orientation;
    
    float horizontal;
    float vertical;

    public int madeContact;
    private int madeContactFire;

    bool ready = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Move();
    }

    private void Move() {

        if (Input.GetKey("down")) {
            gameObject.transform.position += new Vector3(-0.5f, 0, 0);
        }
        else if (Input.GetKey("up")) {
            gameObject.transform.position += new Vector3(0.5f, 0, 0);
        }
        else if (Input.GetKey("right")) {
            gameObject.transform.position += new Vector3(0, 0, -0.5f);
        }
        else if (Input.GetKey("left")) {
            gameObject.transform.position += new Vector3(0, 0, 0.5f);
        }

    }
    
    void OnTriggerEnter(Collider col) 
    {
        if (col.gameObject.tag == "Tree") {
            madeContact++;
            print("Touched tree. Add a point to number of trees touched. :) Touch count: " + madeContact);
        }
        if (col.gameObject.tag == "Fire") {
            ready = false;
            madeContactFire++;
            print("Touched fire. Add a point to number of times fire touched. :) Touch count: " + madeContactFire);
            // StartCoroutine(Wait());
        }
    }

    void OnTriggerExit(Collider col) {
        // if (col.gameObject.tag == "Tree") {
        //     print("Left tree. :(");
        // }
    }

    void OnParticleCollision(GameObject other) {
        if (other.tag == "Fire" && ready == true) {
            ready = false;
            madeContactFire++;
            print("Touched fire. Add a point to number of times fire touched. :) Touch count: " + madeContactFire);
            StartCoroutine(Wait());
        }
    }

    IEnumerator Wait() {
        // print("im waiting");
        yield return new WaitForSeconds(1f);
        ready = true;
    
    }

    // void OnCollisionEnter(Collision col) 
    // {
    //     if (col.gameObject.tag == "Tree") {
    //         gameObject.GetComponent<SphereCollider>().isTrigger = true;
    //     }
    // }

    // void OnCollisionExit(Collision col) 
    // {
    //     if (col.gameObject.tag == "Tree") {
    //         gameObject.GetComponent<SphereCollider>().isTrigger = false;
    //     }
    // }
}
