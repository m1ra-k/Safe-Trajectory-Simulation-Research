using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RaycastManager0 : MonoBehaviour
{
    // add button later
    public bool start = true;

    public List<Ray> rayList;
    public List<float> distanceList;
    public int numRays = 8;
    private int angle = 0;
    private const float MAX_VALUE = 150f;

    Vector3 goal;

    // change these to public later if wanted?
    bool navigate;
    public bool forwardClear = false;
    public bool rightClear = false;
    public bool behindClear = false;
    public bool leftClear = false;
    int madeContact;
    int madeContactFire;
    int rows;
    int cols;

    public int[,] visitCountArray;

    // for use in DTN (check to see if past root)
    bool pastRoot = false;
    DirectionalTreeNode root;
    DirectionalTreeNode node;

    bool coroutineFinished = true;

    NodeTracker nodeTracker;

    public float waitSpeed = 0.1f;
    public int visitsAllowed = 2;


    // Start is called before the first frame update
    void Start()
    {
        print(1.0f / Time.deltaTime);
        root = ScriptableObject.CreateInstance<DirectionalTreeNode>();
        root.posX = gameObject.transform.position.x;
        root.posZ = gameObject.transform.position.z;

        // parameterize later
        rows = (int) (700);
        cols = (int) (1250);

        print("rows: " + rows + "; cols: " + cols);

        Application.targetFrameRate = 60;

        rayList = new List<Ray>();
        distanceList = new List<float>();

        for (int i = 0; i < numRays; i++) {
            rayList.Add(new Ray(gameObject.transform.position, new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), 0, Mathf.Sin(angle * Mathf.Deg2Rad))));
            distanceList.Add(0f);           
            angle += 360/numRays;
        }

        if (rayList.Count == 4) {
            navigate = true;
        }

        // ScriptableObject manager set-up (NodeTracker)
        nodeTracker = ScriptableObject.CreateInstance<NodeTracker>();
        nodeTracker.nodes = new HashSet<DirectionalTreeNode>();

        visitCountArray = new int[rows, cols];
    }

    // Update is called once per frame
    void Update()
    {

        node = ScriptableObject.CreateInstance<DirectionalTreeNode>();
        node.posX = gameObject.transform.position.x;
        node.posZ = gameObject.transform.position.z;

        angle = 0;
        
        UpdateRaycasts();
        UpdateClears(node);
        
        if (!navigate) {
            StopAllCoroutines();
        }
        
        if (start && navigate) {

            // 5-24-23 dfs method
            if (coroutineFinished) {
                StartCoroutine(dfs(node));
            }
            
        }

    }

    public void UpdateRaycasts() {
        // update origin of all Ray
        for (int i = 0; i < rayList.Count; i++) {
            rayList[i] = new Ray(gameObject.transform.position, new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), 0, Mathf.Sin(angle * Mathf.Deg2Rad)));
            angle += 360/numRays;
        }

        for (int i = 0; i < rayList.Count; i++) {
            RaycastHit hit;
            if(Physics.Raycast(rayList[i], out hit)) {
                if(!hit.collider.isTrigger) {
                    distanceList[i] = hit.distance;
                    Debug.DrawRay(rayList[i].origin, rayList[i].direction * hit.distance, Color.red);
                }
            }
            else {
                distanceList[i] = MAX_VALUE;
                Debug.DrawRay(rayList[i].origin, rayList[i].direction * MAX_VALUE, Color.green);
            }

        }   
        
    }

    public void UpdateClears(DirectionalTreeNode n) {
        // move AI; directions are clear if there is at least 1m between drone and obstacle
        forwardClear = (distanceList[0] > 0.2) && (n.posX + 1 < cols);
        rightClear = (distanceList[3] > 0.2) && (n.posZ - 1 > 0);
        behindClear = (distanceList[2] > 0.2) && (n.posX - 1 > 0);
        leftClear = (distanceList[1] > 0.2) && (n.posZ + 1 < rows);
        
    }

    IEnumerator dfs(DirectionalTreeNode n) {
        // adding to HashSet so that individual nodes that have been tried can be counted
        // instead of making new nodes, see if it already exists and update the visit count by 1
        // doesn't contain, so add it
        if (!nodeTracker.nodes.Contains(n)) {
            n.SetVisitCount(1);
            visitCountArray[(int) n.posZ, (int) n.posX] = n.visitCount;
            // print("added to nodes hashset!");
            nodeTracker.nodes.Add(n);
        }
        // contains, so increment visit count and put it back in
        else {
            nodeTracker.nodes.Remove(n);
            visitCountArray[(int) n.posZ, (int) n.posX] += 1;
            n.SetVisitCount(visitCountArray[(int) n.posZ, (int) n.posX]);
            // print("updated node: " + n);
            nodeTracker.nodes.Add(n);
        }      

        print("current node: " + n);

        // bool prevents coroutine from running multiple times, only one allowed
        coroutineFinished = false;
        
        // with each recursive call:
        // update raycast data
        // update clear data
        UpdateRaycasts();
        UpdateClears(n);

        // confused on how i should get it to backtrack 
        // if (n.visitCount >= 3) {
        //     yield break;
        // }

        // stop operation if root is reached again; this means there's no way out
        // ASK would this work? as in, exit completely?
        if (n.Equals(root) && pastRoot) {
            print("no way out, went back to start!");
            yield break;
        }

        n.forward = n.FindChildPos("forward");
        n.left = n.FindChildPos("left");
        n.right = n.FindChildPos("right");
        n.behind = n.FindChildPos("behind");

        if (n.forward.posX >= 124) {
            print("done: " + n);
            navigate = false;
            yield break;
        }

        // find way to clear previous raycasts
        if (forwardClear && visitCountArray[(int) n.forward.posZ, (int) n.forward.posX] < visitsAllowed) {
            print("going forward");
            gameObject.transform.position += new Vector3(0.1f, 0, 0);

            // with each recursive call:
            // update raycast data
            // update clear data
            UpdateRaycasts();
            UpdateClears(n);

            // wait time
            yield return new WaitForSeconds(waitSpeed);    

            print("MIRAAAAAA" + Mathf.Approximately((float) n.forward.posX, (float) gameObject.transform.position.x) + n.forward.posX + " " + gameObject.transform.position.x);

            if (Mathf.Approximately((float) n.forward.posX, (float) gameObject.transform.position.x) && Mathf.Approximately((float) n.forward.posZ, (float) gameObject.transform.position.z)) {
                yield return StartCoroutine(dfs(n.forward));
            }
        }
        if (leftClear && visitCountArray[(int) n.left.posZ, (int) n.left.posX] < visitsAllowed) {
            print("going left");
            gameObject.transform.position += new Vector3(0, 0, 0.1f);

            // with each recursive call:
            // update raycast data
            // update clear data
            UpdateRaycasts();
            UpdateClears(n);

            // wait time
            yield return new WaitForSeconds(waitSpeed);

            // Debug.Log("proof of recursion LEFT");
            if (Mathf.Approximately((float) n.left.posX, (float) gameObject.transform.position.x) && Mathf.Approximately((float) n.left.posZ, (float) gameObject.transform.position.z)) {
                yield return StartCoroutine(dfs(n.left));
            }
        }
        // make 2 a constant
        // based on how dense the grid is, the constant should be smaller/larger
        if (rightClear && visitCountArray[(int) n.right.posZ, (int) n.right.posX] < visitsAllowed) {
            print("going right");
            gameObject.transform.position += new Vector3(0, 0, -0.1f);

            // with each recursive call:
            // update raycast data
            // update clear data
            UpdateRaycasts();
            UpdateClears(n);

            // wait time
            yield return new WaitForSeconds(waitSpeed);    

            // Debug.Log("proof of recursion RIGHT");
            if (Mathf.Approximately((float) n.right.posX, (float) gameObject.transform.position.x) && Mathf.Approximately((float) n.right.posZ, (float) gameObject.transform.position.z)) {
                yield return StartCoroutine(dfs(n.right));
            }
        }
        if (behindClear && visitCountArray[(int) n.behind.posZ, (int) n.behind.posX] < visitsAllowed) {
            print("going behind");
            gameObject.transform.position += new Vector3(-0.1f, 0, 0);

            // with each recursive call:
            // update raycast data
            // update clear data
            UpdateRaycasts();
            UpdateClears(n);

            // wait time
            yield return new WaitForSeconds(waitSpeed);    

            // add a while loop?

            // Debug.Log("proof of recursion BEHIND");
            if (Mathf.Approximately((float) n.behind.posX, (float) gameObject.transform.position.x) && Mathf.Approximately((float) n.behind.posZ, (float) gameObject.transform.position.z)) {
                yield return StartCoroutine(dfs(n.behind));
            }
        }
        
    }

}
