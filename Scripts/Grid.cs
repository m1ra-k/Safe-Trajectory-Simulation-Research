using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Grid : MonoBehaviour
{
    public int rows = 10;
    public int cols = 10;
    public int scale = 1;
    public GameObject grid;
    public Vector3 leftBottom = new Vector3(0, 0, 0);
    public GameObject[,] gridArray;
    public int obstacleDensity = 7;
    public Material obstacle;

    // Start is called before the first frame update
    void Start()
    {
        gridArray = new GameObject[rows, cols];

        if (grid) {
            GenerateGrid();
            CreateObstacle();
        }
        else {
            print("ERROR in Grid.cs");
        }
    }

    void GenerateGrid() {
        for (int i = 0; i < rows; i++) {
            for (int j = 0; j < cols; j++) {
                GameObject g = Instantiate(grid, new Vector3(leftBottom.x+scale*i, leftBottom.y, leftBottom.z+scale*j), Quaternion.identity);
                g.transform.SetParent(gameObject.transform);
                g.GetComponent<GridProperties>().x = i;
                g.GetComponent<GridProperties>().y = j;
                gridArray[i, j] = g;
            }
        }
    }

    void CreateObstacle() {

        // invalid density
        if (obstacleDensity <= 0 || obstacleDensity >= 16) {
            Debug.Log("ERROR: Obstacle density must be within 1-15");
            EditorApplication.ExitPlaymode();
        }
        else {
            for (int i = 0; i < rows; i++) {
                for (int j = 0; j < cols; j++) {
                    int rand = Random.Range(1, 50);
                    if (rand % (16 - obstacleDensity + 2) == 0) {
                        // change to red
                        gridArray[i, j].GetComponent<Renderer>().material = obstacle;
                        // make taller
                        Vector3 scale = gridArray[i, j].GetComponent<Transform>().localScale;
                        scale.y = 1.5f;
                        gridArray[i, j].GetComponent<Transform>().localScale = scale;
                    }
                }
            }
        }
        
    }
}
