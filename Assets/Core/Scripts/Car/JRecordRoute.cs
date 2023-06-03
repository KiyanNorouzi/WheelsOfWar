using UnityEngine;
using System.Collections;
using System;

public class JRecordRoute : MonoBehaviour {

	public int maxSamples = 1000;
		
	JCarPoint[] trail;
	int index = 0;
	int len = 0;
	
	// Use this for initialization
	void Start () {
		trail = new JCarPoint[maxSamples];
	}
	
	// Update is called once per frame
	public void Add(GameObject go, float accel, float steer, int gear) {
		int i = index % maxSamples;
		trail[i].pos = go.transform.localPosition;
		trail[i].rot = go.transform.localRotation;
		trail[i].vel = go.rigidbody.velocity;
		trail[i].accel = accel;
		trail[i].steer = steer;
		trail[i].gear = gear;
		index++;
		if (len < maxSamples) len++;	
	}
	
	public int GetLen() {
		return len;
	}
	
	public int GetPos() {
		return index;
	}
	
	public bool IsValid(int stamp) {
		return !((stamp >= index) || (stamp < index - len));
	}
	
	public JCarPoint GetHistory(int stamp) {
		if ((stamp >= index) || (stamp < index - len)) {
			throw new Exception("trying to read at offset " + stamp + " but have only " + len + " elements, pos " + index);
		}
		return trail[stamp % maxSamples];
	}
}
