using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeColor : MonoBehaviour
{
    public Texture2D green;
    public Texture2D red;

    RawImage rawImage;

    // Start is called before the first frame update
    void Start()
    {
        rawImage = GetComponent<RawImage>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ChangeToGreen() {
        rawImage.texture = green;
    }

    void ChangeToRed() {
        rawImage.texture = red;
    }
}
