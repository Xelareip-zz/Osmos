using UnityEngine;
using System;

public class TriggerSignal : MonoBehaviour
{
	public event Action<TriggerSignal, Collider> collisionStay;
	public event Action<Collider> collisionEnter;
	public event Action<Collider> collisionExit;

	public event Action<TriggerSignal, Collider2D> collisionStay2D;
	public event Action<Collider2D> collisionEnter2D;
	public event Action<Collider2D> collisionExit2D;

	void OnTriggerEnter(Collider coll)
	{
		if (collisionEnter != null)
		{
			collisionEnter(coll);
		}
	}

	void OnTriggerStay(Collider coll)
	{
		if (collisionStay != null)
		{
			collisionStay(this, coll);
		}
	}

	void OnTriggerExit(Collider coll)
	{
		if (collisionExit != null)
		{
			collisionExit(coll);
		}
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (collisionEnter2D != null)
		{
			collisionEnter2D(coll);
		}
	}

	void OnTriggerStay2D(Collider2D coll)
	{
		if (collisionStay2D != null)
		{
			collisionStay2D(this, coll);
		}
	}

	void OnTriggerExit2D(Collider2D coll)
	{
		if (collisionExit2D != null)
		{
			collisionExit2D(coll);
		}
	}
}
