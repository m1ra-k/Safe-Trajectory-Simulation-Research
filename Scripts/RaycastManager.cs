using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RaycastManager : MonoBehaviour
{
    public List<Ray> rayList;
    public List<Ray> rayList2;
    public List<float> distanceList;
    public List<float> distanceList2;
    public int numRays = 8;
    private int angle = 0;
    private const float MAX_VALUE = 150f;

    Vector3 goal;
    public bool navigate;
    public bool forwardClear = false;
    public bool rightClear = false;
    public bool behindClear = false;
    public bool leftClear = false;

    public int madeContactTree;
    public int madeContactFire;
    bool ready = true;


    public int rows;
    public int cols;

    // -1 unknown, 0 unit clear, 1 unit blocked
    public int[,] dotArray;
    // false for not visited yet, true for visited
    public int[,] visitedArray;

    // for use in DTN (check to see if past root)
    public bool pastRoot = false;
    public DirectionalTreeNode root;

    // Start is called before the first frame update
    void Start()
    {
        // DotManager = gameObject.GetComponent<DotManager>();
        // dotArray = DotManager.dotArray;

        // parameterize later
        rows = (int) (70 / 0.1);
        cols = (int) (125 / 0.1);

        print("rows: " + rows + "; cols: " + cols);

        // should be 700 by 1250
        dotArray = new int[rows, cols];
        visitedArray = new int[rows, cols];

        // do all the columns of each row
        for (int r = 0; r < rows; r++) {
            // instantiate all ? for each col
            for (int c = 0; c < cols; c++) {
                dotArray[r, c] = -1;
            }
        }

        Application.targetFrameRate = 60;

        rayList = new List<Ray>();
        // rayList2 = new List<Ray>();
        distanceList = new List<float>();
        // distanceList2 = new List<float>();

        for (int i = 0; i < numRays; i++) {
            rayList.Add(new Ray(gameObject.transform.position, new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), 0, Mathf.Sin(angle * Mathf.Deg2Rad))));
            // what is the math doing; also make angle a parameter
            // make more than you need and index them
            // rayList2.Add(new Ray(gameObject.transform.position, new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad) , 0, Mathf.Sin(angle * Mathf.Deg2Rad))));
            // rayList2.Add(new Ray(gameObject.transform.position, new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), 0, Mathf.Sin(angle * Mathf.Deg2Rad))));
            distanceList.Add(0f);
            // distanceList2.Add(0f);
            // distanceList2.Add(0f);            
            angle += 360/numRays;
        }

        if (rayList.Count == 4) {
            navigate = true;
        }

    }

    // Update is called once per frame
    void Update()
    {
        angle = 0;
        
        updateAll();

        // move AI; directions are clear if there is at least 3m between drone and obstacle
        forwardClear = (distanceList[0] > 3);
        rightClear = (distanceList[3] > 3);
        behindClear = (distanceList[2] > 3);
        leftClear = (distanceList[1] > 3);

        // print("forward is: " + ((int) (transform.position.z * 10) - 1) + ", " + (int) (transform.position.x * 10) + "; left is: " + ((int) (transform.position.z * 10) - 2 ) + ", " + ((int) (transform.position.x * 10) - 1) + "; behind is: " + ((int) (transform.position.z * 10) - 1) + ", " + ((int) (transform.position.x * 10) - 2) + "; right is: " + (int) (transform.position.z * 10) + ", " + ((int) (transform.position.x * 10) - 1));


        // checking to see if in bounds before setting (don't want to set a point in the 2D array that DNE)
        // 0 = unit clear, 1 = unit blocked

        // add upper bound later (CHANGE)
        // forward and behind
        if ((int) (transform.position.z * 10) - 1 >= 0) {
            // setting forward is possible
            if ((int) (transform.position.x * 10) >= 0) {
                // && dotArray[(int) (transform.position.z * 10) - 1, (int) (transform.position.x * 10)] != 1
                if (forwardClear) {
                    dotArray[(int) (transform.position.z * 10) - 1, (int) (transform.position.x * 10)] = 0;
                }
                else {
                    dotArray[(int) (transform.position.z * 10) - 1, (int) (transform.position.x * 10)] = 1;
                }
            }
            // setting behind is possible
            if ((int) (transform.position.x * 10) - 2 >= 0) {
                // && dotArray[(int) (transform.position.z * 10) - 1, (int) (transform.position.x * 10) - 2] != 1
                if (behindClear) {
                    dotArray[(int) (transform.position.z * 10) - 1, (int) (transform.position.x * 10) - 2] = 0;
                }
                else {
                    dotArray[(int) (transform.position.z * 10) - 1, (int) (transform.position.x * 10) - 2] = 1;
                }
            }
        }

        // left and right
        if ((int) (transform.position.x * 10) - 1 >= 0) {
            // setting left is possible
            if ((int) (transform.position.z * 10) - 2 >= 0) {
                // && dotArray[(int) (transform.position.z * 10) - 2, (int) (transform.position.x * 10) - 1] != 1
                if (leftClear) {
                    dotArray[(int) (transform.position.z * 10) - 2, (int) (transform.position.x * 10) - 1] = 0;
                    
                }
                else {
                    dotArray[(int) (transform.position.z * 10) - 2, (int) (transform.position.x * 10) - 1] = 1;
                }
            }
            // setting right is possible
            if ((int) (transform.position.z * 10) >= 0) {
                // && dotArray[(int) (transform.position.z * 10), (int) (transform.position.x * 10) - 1] != 1
                if (rightClear) {
                    dotArray[(int) (transform.position.z * 10), (int) (transform.position.x * 10) - 1] = 0;
                }
                else {
                    dotArray[(int) (transform.position.z * 10), (int) (transform.position.x * 10) - 1] = 1;
                }
            }
        }
        
        try {
            // print("rough array position: " + ((int) (transform.position.z * 10) - 1) + "," + ((int) (transform.position.x * 10) - 1) + "; forward is: " + dotArray[(int) (transform.position.z * 10) - 1, (int) (transform.position.x * 10)] + "; left is: " + dotArray[(int) (transform.position.z * 10) - 2, (int) (transform.position.x * 10) - 1] + "; behind is: " + dotArray[(int) (transform.position.z * 10) - 1, (int) (transform.position.x * 10) - 2] + "; right is: " + dotArray[(int) (transform.position.z * 10), (int) (transform.position.x * 10) - 1]);
        }
        catch (Exception e) {
            print("out of bounds");
        }

        // new AI 4-18-23
        if (navigate) {
            // default forward
            if (dotArray[(int) (transform.position.z * 10) - 1, (int) (transform.position.x * 10)] == 0 && dotArray[(int) (transform.position.z * 10) - 2, (int) (transform.position.x * 10) - 1] == 0 && dotArray[(int) (transform.position.z * 10), (int) (transform.position.x * 10) - 1] == 0) {
                gameObject.transform.position += new Vector3(0.1f, 0, 0);
                // visitedArray[(int) (transform.position.z * 10) - 1, (int) (transform.position.x * 10)] = true;
                print("forward");
            }
            else {
                // prevent from going straight until other positions modified
                dotArray[(int) (transform.position.z * 10) - 1, (int) (transform.position.x * 10)] = 1;
                // try left
                if (dotArray[(int) (transform.position.z * 10) - 2, (int) (transform.position.x * 10) - 1] == 0) {
                    gameObject.transform.position += new Vector3(0, 0, 0.1f);
                    // visitedArray[(int) (transform.position.z * 10) - 2, (int) (transform.position.x * 10) - 1] = true;
                    print("left");
                }
                // try right
                else if (dotArray[(int) (transform.position.z * 10), (int) (transform.position.x * 10) - 1] == 0) {
                    // prevent from going that way again
                    dotArray[(int) (transform.position.z * 10) - 2, (int) (transform.position.x * 10) - 1] = 1;
                    gameObject.transform.position += new Vector3(0, 0, -0.1f);
                    // visitedArray[(int) (transform.position.z * 10), (int) (transform.position.x * 10) - 1] = true;
                    print("right");
                }
                // backtrack if all else fails
                else if (dotArray[(int) (transform.position.z * 10) - 1, (int) (transform.position.x * 10) - 2] == 0) {
                    // prevent from going that way again
                    dotArray[(int) (transform.position.z * 10) - 2, (int) (transform.position.x * 10) - 1] = 1;
                    dotArray[(int) (transform.position.z * 10), (int) (transform.position.x * 10) - 1] = 1;
                    gameObject.transform.position += new Vector3(-0.1f, 0, 0);
                    // visitedArray[(int) (transform.position.z * 10) - 1, (int) (transform.position.x * 10) - 2] = true;
                    print("behind");
                }
            }
            
        }
        
    }

    public void updateAll() {
        // update origin of all Ray
        for (int i = 0; i < rayList.Count; i++) {
            rayList[i] = new Ray(gameObject.transform.position, new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), 0, Mathf.Sin(angle * Mathf.Deg2Rad)));
            // rayList2[2*i] = new Ray(gameObject.transform.position, new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad) - 15, 0, Mathf.Sin(angle * Mathf.Deg2Rad) - 15));
            // rayList2[2*i + 1] = new Ray(gameObject.transform.position, new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad) + 15, 0, Mathf.Sin(angle * Mathf.Deg2Rad) + 15));

            // angle += 45;
            angle += 360/numRays;
        }

        for (int i = 0; i < rayList.Count; i++) {
            RaycastHit hit;
            if(Physics.Raycast(rayList[i], out hit)) {
                if(!hit.collider.isTrigger) {
                    distanceList[i] = hit.distance;
                    // print("HIT! " + i + ", " + distanceList[i]);
                    Debug.DrawRay(rayList[i].origin, rayList[i].direction * hit.distance, Color.red);
                }
            }
            else {
                distanceList[i] = MAX_VALUE;
                Debug.DrawRay(rayList[i].origin, rayList[i].direction * MAX_VALUE, Color.green);
            }
        }   

                    // print(rayList2.Count);

                    // for (int i = 0; i < rayList2.Count; i++) {
                    //     RaycastHit hit;
                    //     if(Physics.Raycast(rayList2[i], out hit)) {
                    //         if(!hit.collider.isTrigger) {
                    //             distanceList2[i] = hit.distance;
                    //             // print(i + ", " + distanceList[i]);
                    //             Debug.DrawRay(rayList2[i].origin, rayList2[i].direction * hit.distance, Color.red);
                    //         }
                    //     }
                    //     else {
                    //         distanceList2[i] = MAX_VALUE;
                    //         Debug.DrawRay(rayList2[i].origin, rayList2[i].direction * MAX_VALUE, Color.green);
                    //     }
                    // }    
        
    }

    // collision detection

    void OnTriggerEnter(Collider col) 
    {
        if (col.gameObject.tag == "Tree") {
            madeContactTree++;
        }
        if (col.gameObject.tag == "Fire") {
            ready = false;
            madeContactFire++;
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
            StartCoroutine(Wait());
        }
    }

    IEnumerator Wait() {
        // print("im waiting");
        yield return new WaitForSeconds(1f);
        ready = true;
    
    }

}
