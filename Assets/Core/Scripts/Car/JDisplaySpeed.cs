using UnityEngine;
using System.Collections;

// actually displays speed AND distance, I know bad naming!
public class JDisplaySpeed : MonoBehaviour {

	public JCar car; // car to get speed and distance from
	public float factor = 1.2f; // angle of speed-hand is km/h * factor
	public Transform[] dials; // lowest index is .1km
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		// adjust speed hand
		transform.localRotation = Quaternion.Euler(0f, car.CurrentSpeed * factor, 0f);
		
		int dist = (int) car.TotalDistance;
		// we use simple trick to wrap from 000000 to 999999 when dist is negative
		if (dist < 0) dist = (100000000 + dist);
		
		// fraction is 0..99m, fraction is used for transition between to digits
		// fraction 0 is shown digit, 99 is almost shown next digit
		int frac = dist % 100;
		dist = dist / 100; // first digit shown is in .1km, so divide by 100m
		for (int i = 0; i < dials.Length; i++) {
			int v = dist % 10;
			
			// every digit is 36 degrees, fraction is .01 of a digit.
			// (10 digits is full circle, 360 degrees).
			float angle = ((float) v + (float) frac * .01f) * 36f;
			
			// fraction transfers to next digit if current digit is 9
			// so with display 9399 and fraction 50, the 3 will be shown
			// as halfway between 3 and 4, and the last two 9's as between
			// 9 and 0.
			if (v < 9) frac = 0;
			dist = dist / 10; // go to next digit
			
			// we go the 'wrong way round, so -angle instead of angle
			// (could have modified the model so we could have used
			// positive values of course)
			dials[i].localRotation = Quaternion.Euler(-angle, 0f, 0f);
		}
	}
}
