using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		HitBox hitBox = other.GetComponent<HitBox>();
		MapManager.SavePlayer(hitBox.health.gameObject, transform);
	}
}
