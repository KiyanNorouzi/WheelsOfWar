using UnityEngine;
using System.Collections;

public class JSpawnGhosts : MonoBehaviour {

	public JGhostCar original;
	public JRecordRoute record;
	
	int lastSaved = -1;
	
	void Spawn(int startStamp, int endStamp) {
		JGhostCar car = (JGhostCar) Instantiate(original);
		car.Activate(startStamp, endStamp);
		// Debug.Log("recording length: " + (endStamp - startStamp));
	}
	
	public void DoSpawn() {
		int stamp = record.GetPos();
		if (lastSaved >= 0) {
			Spawn(lastSaved, stamp);
		}
		lastSaved = stamp;
	}
}
