using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCharacter : MonoBehaviour
{
    public Vector2 turn;
    public GameObject player;
    bool userControl = false;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        player = GameObject.Find("drone_red");
        gameObject.transform.rotation = Quaternion.Euler(0, 90, 0);

    }

    // Update is called once per frame
    void Update()
    {
    
        if (Input.GetMouseButton(0)) {
            userControl = true;
        }
        else {
            userControl = false;
        }

        if (userControl) {
            turn.x += Input.GetAxis("Mouse X");
            turn.y += Input.GetAxis("Mouse Y");
            transform.localRotation = Quaternion.Euler(-turn.y, turn.x + 90, 0);
            gameObject.transform.position = new Vector3(player.transform.position.x - 3, player.transform.position.y + 1, player.transform.position.z);
        }
        else {
            gameObject.transform.position = new Vector3(player.transform.position.x - 3, player.transform.position.y + 1, player.transform.position.z);
            // gameObject.transform.rotation = Quaternion.Euler(0, 90, 0);        
        }
        

    }
}
