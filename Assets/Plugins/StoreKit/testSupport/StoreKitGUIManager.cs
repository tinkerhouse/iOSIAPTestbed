using UnityEngine;
using System.Collections.Generic;



public class StoreKitGUIManager : MonoBehaviour
{
#if UNITY_IPHONE
	void OnGUI()
	{
		float yPos = 5.0f;
		float xPos = 5.0f;
		float width = ( Screen.width >= 960 || Screen.height >= 960 ) ? 320 : 160;
		float height = ( Screen.width >= 960 || Screen.height >= 960 ) ? 80 : 40;
		float heightPlus = height + 10.0f;
		
		if( GUI.Button( new Rect( xPos, yPos, width, height ), "Get Can Make Payments" ) )
		{
			bool canMakePayments = StoreKitBinding.canMakePayments();
			Debug.Log( "StoreKit canMakePayments: " + canMakePayments );
		}
		
		
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Get Product Data" ) )
		{
			// comma delimited list of product ID's from iTunesConnect.  MUST match exactly what you have there!
			string productIdentifiers = "anotherProduct,tt,testProduct,sevenDays,oneMonthSubsciber";
			StoreKitBinding.requestProductData( productIdentifiers );
		}
		
		
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Restore Completed Transactions" ) )
		{
			StoreKitBinding.restoreCompletedTransactions();
		}
		
		
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Validate Receipt" ) )
		{
			// grab the transactions, then just validate the first one
			List<StoreKitTransaction> transactionList = StoreKitBinding.getAllSavedTransactions();
			if( transactionList.Count > 0 )
				StoreKitBinding.validateReceipt( transactionList[0].base64EncodedTransactionReceipt, true );
		}
		
		// Second column
		xPos = Screen.width - width - 5.0f;
		yPos = 5.0f;
		
		if( GUI.Button( new Rect( xPos, yPos, width, height ), "Purchase Product 1" ) )
		{
			StoreKitBinding.purchaseProduct( "testProduct", 1 );
		}
		
		
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Purchase Consumable" ) )
		{
			// assumes anotherProduct is a consumable
			StoreKitBinding.purchaseProduct( "anotherProduct", 1 );
		}


		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Purchase Subscription" ) )
		{
			StoreKitBinding.purchaseProduct( "sevenDays", 1 );
		}
		
		
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Validate Subscription" ) )
		{
			// grab the transactions and if we have a subscription in there validate it
			List<StoreKitTransaction> transactionList = StoreKitBinding.getAllSavedTransactions();
			foreach( var t in transactionList )
			{
				if( t.productIdentifier == "sevenDays" )
				{
					StoreKitBinding.validateAutoRenewableReceipt( t.base64EncodedTransactionReceipt, "YOUR_SECRET_FROM_ITC", true );
					break;
				}
			}
		}

		
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Get Saved Transactions" ) )
		{
			List<StoreKitTransaction> transactionList = StoreKitBinding.getAllSavedTransactions();
			
			// Print all the transactions to the console
			Debug.Log( "\ntotal transaction received: " + transactionList.Count );
			
			foreach( StoreKitTransaction transaction in transactionList )
				Debug.Log( transaction.ToString() + "\n" );
		}
		
	}
#endif
}
