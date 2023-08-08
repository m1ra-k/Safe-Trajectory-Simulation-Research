using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTouch : MonoBehaviour
{
    private int madeContact;

    void OnParticleTrigger() 
    {
        madeContact++;
        print("Touched fire. Add a point to number of fires touched. :) Touch count: " + madeContact);
        
    }

}
