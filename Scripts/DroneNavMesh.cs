using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DroneNavMesh : MonoBehaviour
{

    public Vector3 goal;
    private NavMeshAgent nma;

    // Start is called before the first frame update
    private void Awake()
    {
        nma = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    private void Update()
    {
        nma.destination = goal;
    }
}
