using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random=UnityEngine.Random;

[Serializable]
public class Point
{
    public float x = 0.0f;
    public float y = 0.0f; 
    public float z = 0.0f;
    
    // tree detection + points for trajectory week of 3/22, change to size of soccer field, add fire?

    // randomized Point
    public Point() {
        randomizePoint();
    }


    public Point(float xCoord, float yCoord, float zCoord) {
        x = xCoord;
        y = yCoord;
        z = zCoord;
    }

    public void randomizePoint() {
        // Carr's Hill Field
        x = Random.Range(10, 115);
        y = Random.Range(5, 35);
        z = Random.Range(10, 60);
        // Debug.Log("coordinates of random point are " + x + ", " + y + ", " + z);
    }

    public float getX() {
        return x;
    }

    public float getY() {
        return y;
    }

    public float getZ() {
        return z;
    }

    public Vector3 getVector() {
        return new Vector3(x, y, z);
    }
}
