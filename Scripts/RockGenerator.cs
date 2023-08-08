using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockGenerator : MonoBehaviour
{ 
    // parameterize later
    public int xSpacing = 2;
    public int zSpacing = 2;

    // add parameter of length:
    public int length = 125;
    // add parameter of width:
    public int width = 70;

    public float minSize = 0.1f;
    public float maxSize = 3f;

    public float minVX = 3f; 
    public float maxVX = 9f;
    public float minVZ = 3f;
    public float maxVZ = 9f;


    public GameObject rock;

    // Start is called before the first frame update
    void Start()
    {
       

        // create rocks
        for (int x = 0; x < length; x += xSpacing) {
            for (int z = Random.Range(0, 65); z < width; z += zSpacing) {
            
            // picks a tree randomly
            Vector3 position = new Vector3(x, 1f, z);
            Vector3 offset = new Vector3(Random.Range(-0.75f, 0.75f), Random.Range(-0.75f, 0.75f));
            Vector3 rotation = new Vector3(Random.Range(0, 5f), Random.Range(0, 5f), Random.Range(0, 5f));
            Vector3 scale = Vector3.one * Random.Range(minSize, maxSize);

            GameObject newRock = Instantiate(rock);
            newRock.transform.SetParent(transform);
            newRock.transform.position = position;
            newRock.transform.eulerAngles = rotation;
            newRock.transform.localScale = scale;
            newRock.GetComponent<Roll>().vX = Random.Range(minVX, maxVX);
            newRock.GetComponent<Roll>().vZ = Random.Range(minVZ, maxVZ);

            zSpacing = 70;

            }   
            xSpacing = Random.Range(3, 6);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
