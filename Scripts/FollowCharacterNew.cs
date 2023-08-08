using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCharacterNew : MonoBehaviour
{
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        
        player = GameObject.Find("drone_red");
        gameObject.transform.rotation = Quaternion.Euler(90, 0, 0);

    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = new Vector3(player.transform.position.x, 15, player.transform.position.z);
        // gameObject.transform.rotation = Quaternion.Euler(0, 90, 0);
    }
}
