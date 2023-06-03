using UnityEngine;
using System.Collections;

public class ScrInGarageCar : MonoBehaviour
{
	public Rigidbody myRigidBody;
	//public Animator myAnimator;

	bool doesAnimationPlayed;

	void OnEnable()
	{
		doesAnimationPlayed = false;
		lastVelocityY = float.MaxValue;
	}

	float lastVelocityY;
	void Update()
	{
		if (!doesAnimationPlayed) 
		{
			if (myRigidBody.velocity.y > lastVelocityY && myRigidBody.velocity.y >= -0.5f)
			{
				doesAnimationPlayed = true;
				//myAnimator.SetTrigger("move");

               // ScrGarageController.Instance.audioPlayer.Play(ScrGarageController.Instance.audioPlayer.carFall);
               // ScrGarageController.Instance.CarModelLoaded(this);
			}

			lastVelocityY = myRigidBody.velocity.y;
		} 
		//else
			//Debug.Log ("already " + Time.time);
	}
}