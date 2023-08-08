using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoMove : MonoBehaviour
{
    // randomize?
    public int numTests = 1;
    public List<Point> points;
    public float droneSpeed = 2;

    private int moveX = 1;
    private int moveY = 1;
    private int moveZ = 1;

    private int counter = 0;
    private Vector3 current;
    private bool start = false;
    private bool stop = false;

    private IEnumerator finished;

    public bool readFromJSON = false;
    public static string filename = @"Trajectory.JSON";

    // make arrays with points for the drone to visit, each index in array would be xyz position to move drone to with time
    // make another class Point xyz

    // complex environment - fire, tree falling, random trees fall at given intervals, weather (rain, hail), night/day
    // more complex, the better

    // Start is called before the first frame update
    void Start()
    {
        if (readFromJSON) {
            List<Point> trajectory = new List<Point>();
            for (int i = 0; i < numTests; i++) {
                trajectory.Add(new Point());
            }
            FileHandler.ListSaveToJSON<Point> (trajectory, filename);
            points = FileHandler.ReadListFromJSON<Point> (filename);
            print("I got these values from JSON!");
            
        }
        else {
            points = new List<Point>();
            for (int i = 0; i < numTests; i++) {
                points.Add(new Point());
            }
            print("I didn't get this from JSON, but I still work.");
        }
        gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
    }

    // Update is called once per frame
    void Update()
    {   
        // rewrite - can't detect collision if moving by position, have to do it by velocity
        if (Input.GetKeyDown("up") && counter < numTests && !stop) {
            StartCoroutine(Move());
        }
        
        if (counter > 0 || stop) {
            if (stop) {
                if (Vector3.Distance(gameObject.transform.position, points[counter].getVector()) < 2.0f) {
                    gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                }
            }
            else if (counter != numTests) {
                if (Vector3.Distance(gameObject.transform.position, points[counter-1].getVector()) < 2.0f) {
                    gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                }
            }
        }

        IEnumerator Move() {
            if (counter < numTests) {
                while (points[counter].getVector() != gameObject.transform.position) {
                    if (counter < numTests) {
                        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, points[counter].getVector(), droneSpeed);
                    }
                    if (points[counter].getVector() == gameObject.transform.position) {
                        if (counter + 1 != numTests) {
                            counter++;
                        }
                        else {
                            stop = true;
                        }
                        break;
                    }
                    yield return null;
                }
            }
        }

        // triggers
     
        // fix a lot later, see ManualMove
        void OnCollisionEnter(Collision c) 
        {
            if (c.gameObject.name == "Carr's Hill Field") {
                print("Collision detected! Touching terrain.");
            }
        }

    }
}
