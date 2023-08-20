using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingTextTest : MonoBehaviour
{
    public FloatingTextManager txtmanager;
    public Enemy enemy;

    // Start is called before the first frame update
    void Start()
    {
        //txtmanager = FindObjectOfType<FloatingTextManager>();
        //player = FindObjectOfType<PlayerAbilityCaster>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            txtmanager.Show("Working", 20, Color.red, enemy.transform.position, Vector3.up * 50, 0.7f);
        }
    }
}
