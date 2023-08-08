using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireGenerator : MonoBehaviour
{ 
    // parameterize later
    public int xSpacing = 2;
    public int zSpacing = 2;

    // add parameter of length:
    public int length = 125;
    // add parameter of width:
    public int width = 70;

    public GameObject fire;

    // Start is called before the first frame update
    void Start()
    {
       

        // create fires
        for (int x = 0; x < length; x += xSpacing) {
            for (int z = Random.Range(0, 65); z < width; z += zSpacing) {
            
            // picks a tree randomly
            Vector3 position = new Vector3(x, 0f, z);
            Vector3 offset = new Vector3(Random.Range(-0.75f, 0.75f), Random.Range(-0.75f, 0.75f));
            Vector3 rotation = new Vector3(Random.Range(0, 5f), Random.Range(0, 5f), Random.Range(0, 5f));

            GameObject newFire = Instantiate(fire);
            newFire.transform.SetParent(transform);
            newFire.transform.position = position;
            newFire.transform.eulerAngles = rotation;

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
