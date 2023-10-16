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
            if (spawnPos[0].childCount != 0 && spawnPos[1].childCount != 0)
            {
                Debug.Log("Lentils");
                Destroy(spawnPos[0].GetChild(0));
                Destroy(spawnPos[1].GetChild(0));

            }
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
        i = 0;
    }
}
