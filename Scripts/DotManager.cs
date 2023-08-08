using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DotManager : MonoBehaviour
{
    public int rows;
    public int cols;

    public int[,] dotArray;

    // Start is called before the first frame update
    void Start()
    {
        // parameterize later
        rows = (int) (70 / 0.1);
        cols = (int) (125 / 0.1);

        print("rows: " + rows + "; cols: " + cols);

        // should be 700 by 1250
        dotArray = new int[rows, cols];
        
        int i = 0;
        
        // do all the columns of each row
        for (int r = 0; r < rows; r++) {
            // instantiate all ? for each col
            for (int c = 0; c < cols; c++) {
                dotArray[r, c] = -1;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
