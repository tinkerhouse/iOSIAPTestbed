using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MiniJSON;

public class RivetyPaymentManager : MonoBehaviour
{
	public enum State
	{
		Default = 0,
		Initializing,
		MakingPurchase,
		RestoringPurchases
	}

	private RivetyProductManager rivetyProductManager;

	private State _state = State.Default;
	public State state
	{
		get { return this._state; }
		set { this._state = value; }
	}

	private bool _billingEnabled = false;
	public bool billingEnabled
	{
		get { return this._billingEnabled; }
		set { this._billingEnabled = value; }
	}

	void Awake()
	{
		GameObject rivetyProductManagerGO = GameObject.Find("RivetyProductManager");
		rivetyProductManager = (RivetyProductManager)rivetyProductManagerGO.GetComponent("RivetyProductManager");
	}

	public void PurchaseProduct(string productID)
	{
		this._state = State.MakingPurchase;

		// TODO: make the store kit call
	}

	public void RestorePurchases()
	{
		this._state = State.RestoringPurchases;

		// 1: retrieve purchases from Apple/Google
		// 2: retrieve purchases from Rivety (which includes purchases made from outside the current platform)
		// 3: load purchases from PlayerPrefs
		// reconcile all three lists in this way:
		// - Google is canon - if it exists on Google, it must exist in Rivety and PlayerPrefs (add if necessary)
		// - Rivety is next - if it exists in Rivety, it must exist in PlayerPrefs (add if necessary)
		// - for now, never delete a purchase from Rivety or PlayerPrefs
		// 4: write all purchases back to PlayerPrefs

		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			// retrieve transactions from Apple if we're on an actual iOS device

			// TODO: make the store kit call
		}
		else
		{
			// otherwise jump past the Apple step to the Rivety step - this is for testing in the Unity editor
			rivetyProductManager.RestoreProductsForCurrentUser();
		}
	}

}
