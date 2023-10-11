using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnDummies : MonoBehaviour
{
    public List<GameObject> creatures;
    public List<Transform> spawnPos;
    private int i = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Yay");
            foreach (GameObject creature in creatures)
            {
                Instantiate(creature, spawnPos[i]);
                i++;
            }
        }
        else
        {
            Debug.Log("HAHAhA :)");
        }
    }
}
