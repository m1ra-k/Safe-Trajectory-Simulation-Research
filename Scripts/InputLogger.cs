using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputLogger : MonoBehaviour
{
    private StreamWriter writer;
    private float elapsedTime = 0f;
    // edit to make new files that log input to be later used with machine learning
    public string testName = "";
    string command = "N/A";

    // Start is called before the first frame update
    void Start()
    {
        string filePath = "Assets/ML/Input Data/" + testName + ".csv";
        writer = new StreamWriter(filePath);
        writer.WriteLine("Time : Frame : Command");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.DownArrow)) {
            command = "Backward";
            print("1");
        }
        else if (Input.GetKey(KeyCode.UpArrow)) {
            command = "Forward";
            print("2");
        }
        else if (Input.GetKey(KeyCode.RightArrow)) {
            command = "Right";
            print("3");
        }
        else if (Input.GetKey(KeyCode.LeftArrow)) {
            command = "Left";
            print("4");
        }
        writer.WriteLine(elapsedTime.ToString("F3") + " : " + Time.frameCount + " : " + command);
        elapsedTime += Time.deltaTime;
        if (Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow)) {
            command = "N/A";
            print("5");
        }
    }

    void OnDestroy() 
    {
        writer.Close();
    }
}
