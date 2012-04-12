using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// put sample purchase list data here

public class AppleEventHandlers : MonoBehaviour
{
#if UNITY_IPHONE

	private Spinner spinner;
	private TestbedGUI testbedGui;
	private RivetyProductManager rivetyProductManager;
	private RivetyPaymentManager rivetyPaymentManager;

	void Awake()
	{
		GameObject spinnerGO = GameObject.Find("Spinner");
		spinner = (Spinner)spinnerGO.GetComponent("Spinner");

		GameObject testbedGuiGO = GameObject.Find("TestbedGUI");
		testbedGui = (TestbedGUI)testbedGuiGO.GetComponent("TestbedGUI");

		GameObject rivetyProductManagerGO = GameObject.Find("RivetyProductManager");
		rivetyProductManager = (RivetyProductManager)rivetyProductManagerGO.GetComponent("RivetyProductManager");

		GameObject rivetyPaymentManagerGO = GameObject.Find("RivetyPaymentManager");
		rivetyPaymentManager = (RivetyPaymentManager)rivetyPaymentManagerGO.GetComponent("RivetyPaymentManager");
	}

	void OnEnable()
	{
		StoreKitManager.productListReceived                  += this.ProductListReceivedEvent;
		StoreKitManager.productListRequestFailed             += this.ProductListRequestFailedEvent;
		StoreKitManager.restoreTransactionsFinished          += this.RestoreTransactionsFinishedEvent;
		StoreKitManager.restoreTransactionsFailed            += this.TransactionRestoreFailedEvent;
		StoreKitManager.purchaseSuccessful                   += this.PurchaseSuccessfulEvent;
		StoreKitManager.purchaseCancelled                    += this.PurchaseCancelledEvent;
		StoreKitManager.purchaseFailed                       += this.PurchaseFailedEvent;
		StoreKitManager.receiptValidationRawResponseReceived += this.ReceiptValidationRawResponseReceivedEvent;
		StoreKitManager.receiptValidationSuccessful          += this.ReceiptValidationSuccessfulEvent;
		StoreKitManager.receiptValidationFailed              += this.ReceiptValidationFailedEvent;
	}

	void OnDisable()
	{
		StoreKitManager.productListReceived                  -= this.ProductListReceivedEvent;
		StoreKitManager.productListRequestFailed             -= this.ProductListRequestFailedEvent;
		StoreKitManager.restoreTransactionsFinished          -= this.RestoreTransactionsFinishedEvent;
		StoreKitManager.restoreTransactionsFailed            -= this.TransactionRestoreFailedEvent;
		StoreKitManager.purchaseSuccessful                   -= this.PurchaseSuccessfulEvent;
		StoreKitManager.purchaseCancelled                    -= this.PurchaseCancelledEvent;
		StoreKitManager.purchaseFailed                       -= this.PurchaseFailedEvent;
		StoreKitManager.receiptValidationRawResponseReceived -= this.ReceiptValidationRawResponseReceivedEvent;
		StoreKitManager.receiptValidationSuccessful          -= this.ReceiptValidationSuccessfulEvent;
		StoreKitManager.receiptValidationFailed              -= this.ReceiptValidationFailedEvent;
	}

	public void ProductListReceivedEvent(List<StoreKitProduct> productList)
	{
	}

	public void ProductListRequestFailedEvent(string error)
	{
	}

	public void RestoreTransactionsFinishedEvent()
	{
		// spinner.Hide();
		// testbedGui.stateMessage = "Purchases Restored";
		// testbedGui.state = TestbedGUI.State.BrowsingProductList;

		// TODO: apple quivalent of this google code
		// IDictionary signedDataDict = (IDictionary)Json.Deserialize(this._signedData); ???
		// rivetyProductManager.applePurchases = ???
		rivetyProductManager.RestoreProductsForCurrentUser();
	}

	public void TransactionRestoreFailedEvent(string error)
	{
		spinner.Hide();
		Debug.Log("TransactionRestoreFailedEvent: " + error);
		testbedGui.stateMessage = "Restoring Purchases Failed";
		testbedGui.state = TestbedGUI.State.BrowsingProductList;
	}

	public void PurchaseSuccessfulEvent(StoreKitTransaction transaction)
	{
		// only do this if the user is making a single purchase
		if (rivetyPaymentManager.state == RivetyPaymentManager.State.MakingPurchase)
		{
			// TODO: apple quivalent of this google code

			// IDictionary signedDataDict = (IDictionary)Json.Deserialize(this._signedData);
			// IList orders = (IList)signedDataDict["orders"];
			// IDictionary order = (IDictionary)orders[0];
			// string productId2 = order["productId"].ToString();
			// string orderId = order["orderId"].ToString();
			// string purchaseTime = order["purchaseTime"].ToString();
			// if (productId2 == "android.test.purchased") Debug.Log("Skipped backing up test product.");
			// else rivetyProductManager.BackupProductForCurrentUser(productId2, orderId, purchaseTime);
		}
		// TODO: maybe this should happen in the backup response handler to verify total success
		if (testbedGui.state == TestbedGUI.State.PurchasingProduct)
		{
			spinner.Hide();
			testbedGui.stateMessage = "Purchase Succeeded";
			testbedGui.state = TestbedGUI.State.BrowsingProductList;
		}
	}

	public void PurchaseCancelledEvent(string error)
	{
		spinner.Hide();
		testbedGui.stateMessage = "Purchase Cancelled";
		testbedGui.state = TestbedGUI.State.BrowsingProductList;
	}

	public void PurchaseFailedEvent(string error)
	{
		spinner.Hide();
		testbedGui.stateMessage = "Purchase Failed";
		testbedGui.state = TestbedGUI.State.BrowsingProductList;
	}

	public void ReceiptValidationRawResponseReceivedEvent(string response)
	{
	}

	public void ReceiptValidationSuccessfulEvent()
	{
	}

	public void ReceiptValidationFailedEvent(string error)
	{
	}

#endif
}
