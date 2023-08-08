using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random=UnityEngine.Random;
using UnityEngine.UIElements;

public class TreeGenerator : MonoBehaviour
{

    public int seed = 0;
    public int xSpacing = 2;
    public int zSpacing = 2;
    // parameter tree types to include:
    public GameObject[] trees;

    // add parameter of length:
    public int length = 125;
    // add parameter of width:
    public int width = 70;
    // add parameter of height:
    public float minHeight = 0.50f;
    public float maxHeight = 1.50f;
    // add parameter of density: (improve later, for now larger number means less dense)
    public int density = 0;
    
    // weather

    // boolean of flat or hilly - smooth randomness, much more natural (later problem)
    // try randomizing elevation in a different script

    // make a table of what's user defined and random

    // Start is called before the first frame update
    void Start()
    {
        Random.seed = seed;
        // create trees
        for (int x = 0; x < length; x += xSpacing) {
            for (int z = Random.Range(0, 20); z < width; z += zSpacing) {
            // these are distributions
            // picks a tree randomly
            GameObject tree = trees[Random.Range(0, 3)];

            Vector3 position = new Vector3(x, 0f, z);
            Vector3 offset = new Vector3(Random.Range(-0.75f, 0.75f), Random.Range(-0.75f, 0.75f));
            Vector3 rotation = new Vector3(Random.Range(0, 5f), Random.Range(0, 5f), Random.Range(0, 5f));
            Vector3 scale = Vector3.one * Random.Range(minHeight, maxHeight);

            GameObject newTree = Instantiate(tree);
            newTree.transform.SetParent(transform);
            newTree.transform.position = position + offset;
            newTree.transform.eulerAngles = rotation;
            newTree.transform.localScale = scale;

            zSpacing = Random.Range(density + 10, density + 20);

            }   
            xSpacing = Random.Range(density/10 + 1, density/4 + 5);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
