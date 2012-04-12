using UnityEngine;
using System.Collections;

public class TestbedGUI : MonoBehaviour
{
	public enum State
	{
		Home = 0,
		InitializingGoogleBilling,
		LoadingProducts,
		BrowsingProductList,
		PurchasingProduct,
		RestoringPurchases,
		LoggingOut
	}

	private Spinner spinner;
	private RivetyProductManager rivetyProductManager;
	private RivetyPaymentManager rivetyPaymentManager;

	private State _state = State.Home;
	public State state
	{
		get { return this._state; }
		set { this._state = value; }
	}

	private string _stateMessage = "";
	public string stateMessage
	{
		get { return this._stateMessage; }
		set { this._stateMessage = value; }
	}

	void Awake()
	{
		GameObject spinnerGO = GameObject.Find("Spinner");
		spinner = (Spinner)spinnerGO.GetComponent("Spinner");

		GameObject rivetyProductManagerGO = GameObject.Find("RivetyProductManager");
		rivetyProductManager = (RivetyProductManager)rivetyProductManagerGO.GetComponent("RivetyProductManager");

		GameObject rivetyPaymentManagerGO = GameObject.Find("RivetyPaymentManager");
		rivetyPaymentManager = (RivetyPaymentManager)rivetyPaymentManagerGO.GetComponent("RivetyPaymentManager");
	}

	void OnGUI()
	{
		switch (this._state)
		{
			case State.Home:
				GUILayout.BeginArea(new Rect(10, 10, Screen.width - 20, Screen.height - 20));
					GUILayout.Box("APPLE IAP TESTBED");
					GUILayout.Space(10.0f);
					if (GUILayout.Button("LET'S GO SHOPPING!"))
					{
						spinner.Show();
						this.LoadProducts();
					}
				GUILayout.EndArea();
				break;

			case State.BrowsingProductList:
				GUILayout.BeginArea(new Rect(10, 10, Screen.width - 20, Screen.height - 20));
					GUILayout.Box("WAR ROOM PRODUCTS");
					GUILayout.Space(10.0f);
					if (rivetyProductManager.HasProducts())
					{
						foreach (IDictionary product in rivetyProductManager.products)
						{
							GUILayout.Box(product["name"].ToString());

							// TODO: keep IsOwned booleans outside the gui loop to increase performance
							if (rivetyProductManager.IsOwned(product["sku"].ToString()))
							{
								GUILayout.Label("I OWN THIS");
							}
							else
							{
								if (GUILayout.Button("BUY THIS"))
								{
									if (Application.platform == RuntimePlatform.IPhonePlayer)
									{
										this._stateMessage = "";
										spinner.Show();
										this._state = State.PurchasingProduct;
										rivetyPaymentManager.PurchaseProduct("apl" + product["sku"].ToString());
									}
									else
									{
										this._stateMessage = "Actual Device Required";
									}
								}
							}
							GUILayout.Space(10.0f);
						} 
					}
					if (GUILayout.Button("CANCEL"))
					{
						this._stateMessage = "";
						this._state = State.Home;
					}
					GUILayout.Space(10.0f);
					if (GUILayout.Button("RESTORE PURCHASES"))
					{
						this._stateMessage = "";
						spinner.Show();
						this._state = State.RestoringPurchases;
						rivetyPaymentManager.RestorePurchases();
					}
				GUILayout.EndArea();
				break;

			default:
				break;
		}

		if (this._stateMessage != "") GUI.Box(new Rect(0, Screen.height - 40, Screen.width, 40), this._stateMessage);
	}

	public void LoadProducts()
	{
		this._state = State.LoadingProducts;
		this._stateMessage = "";
		if (Application.internetReachability != NetworkReachability.NotReachable)
		{
			rivetyProductManager.GetProducts();
		}
		else
		{
			this._stateMessage = "No Internet Connection";
			this._state = State.Home;
		}
	}

}
