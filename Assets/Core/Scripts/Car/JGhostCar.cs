/**
 * A simple car physics script using wheel colliders.
 * Jaap Kreijkamp jaap@ctrl-j.com.au
 *
 * orientation should be that front of car is in direction of
 * the 'blue arrow' in Unity, the roof should be in direction of
 * the green angle. Thus with rotation 0, 0, 0, adding 1 to Z
 * will move car 1m forward, adding 1 to Y will move car 1m
 * upward. The wheels should be children of the car object this
 * script is added to and connected to the wheelFL, wheelFR, ...
 * variables.
 *
 * Please modify script and do whatever you like with it,
 * in it's current state it should give a working car,
 * but by no means perfect (or even close) behavior.
 * It's my first attempt and don't really need in my current
 * project so haven't put too much effort into it to perfect
 * it. As often people are looking for help to getting
 * a car working with wheel colliders, I'd appreciate when
 * improvements are posted back on the unity forums.
 *
 * Lastly, thanks to all the people helping me on forums (especially
 * the order of initialisation problem and other example
 * code that helped me much in learning how to do stuff like
 * this).
 */

using UnityEngine;
using System.Collections;

public class JGhostCar : JCar {	

	public JRecordRoute playback;
	public int playbackPos;
	public int endPos;
	
	public void Activate(int startStamp, int endStamp) {
		playbackPos = startStamp;
		endPos = endStamp;
		if (playback.IsValid(playbackPos)) {
			JCarPoint d = playback.GetHistory(playbackPos);
			transform.localPosition = d.pos;
			transform.localRotation = d.rot;
			gameObject.SetActive(true);
		}
		else {
			Destroy(gameObject);
		}
	}
	
	// handle the physics of the engine
	void FixedUpdate () {
		if (playback == null) return;
		
		float steer = 0; // steering -1.0 .. 1.0
		float accel = 0; // accelerating -1.0 .. 1.0
		int gear = CurrentGear;

		if (playbackPos > endPos) {
			Destroy(gameObject);
			return;
		}
		JCarPoint d = playback.GetHistory(playbackPos++);
		steer = d.steer;
		accel = d.accel;
		gear = d.gear;
			
		transform.localPosition = d.pos;
		transform.localRotation = d.rot;
		rigidbody.velocity = d.vel;
		if (gear > CurrentGear) {
			ShiftUp();
		}
		else if (gear < CurrentGear) {
			ShiftDown();
		}
		HandleMotor(steer, accel);				
	}
}
