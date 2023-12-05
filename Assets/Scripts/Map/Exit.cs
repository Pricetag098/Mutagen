using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Exit : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		HitBox hitBox = other.GetComponent<HitBox>();
		MapManager.SavePlayer(hitBox.health.gameObject, transform);

		MapManager.LoadNext();
	}
	private void Quit()
	{
        MapManager.LoadNext();
    }
}

