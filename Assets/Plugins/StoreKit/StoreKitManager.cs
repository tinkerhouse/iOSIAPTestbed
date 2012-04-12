using UnityEngine;
using System;
using System.Collections.Generic;



public class StoreKitManager : MonoBehaviour
{
#if UNITY_IPHONE
	// Fired when a product is successfully paid for.  returnValue will hold the productIdentifer and receipt of the purchased product.
	public static event Action<StoreKitTransaction> purchaseSuccessful;
	
	// Fired when the product list your required returns.  Automatically serializes the productString into StoreKitProduct's.
	public static event Action<List<StoreKitProduct>> productListReceived;
	
	// Fired when requesting product data fails
	public static event Action<string> productListRequestFailed;
	
	// Fired when a product purchase fails
	public static event Action<string> purchaseFailed;
	
	// Fired when a product purchase is cancelled by the user or system
	public static event Action<string> purchaseCancelled;
	
	// Fired when the validateReceipt call fails
	public static event Action<string> receiptValidationFailed;
	
	// Fired when receive validation completes and returns the raw receipt data
	public static event Action<string> receiptValidationRawResponseReceived;
	
	// Fired when the validateReceipt method finishes.  It does not automatically mean success.
	public static event Action receiptValidationSuccessful;
	
	// Fired when an error is encountered while adding transactions from the user's purchase history back to the queue
	public static event Action<string> restoreTransactionsFailed;
	
	// Fired when all transactions from the user's purchase history have successfully been added back to the queue
	public static event Action restoreTransactionsFinished;
	
	
    void Awake()
    {
		// Set the GameObject name to the class name for easy access from Obj-C
		gameObject.name = this.GetType().ToString();
		DontDestroyOnLoad( this );
    }
	
	
	public void productPurchased( string returnValue )
	{
		if( purchaseSuccessful != null )
		{
			var transaction = StoreKitTransaction.transactionFromString( returnValue );
			purchaseSuccessful( transaction );
		}
	}
	
	
	public void productPurchaseFailed( string error )
	{
		if( purchaseFailed != null )
			purchaseFailed( error );
	}
	
		
	public void productPurchaseCancelled( string error )
	{
		if( purchaseCancelled != null )
			purchaseCancelled( error );
	}
	
	
	public void productsReceived( string productString )
	{
        List<StoreKitProduct> productList = new List<StoreKitProduct>();

		// parse out the products
        string[] productParts = productString.Split( new string[] { "||||" }, StringSplitOptions.RemoveEmptyEntries );
        for( int i = 0; i < productParts.Length; i++ )
            productList.Add( StoreKitProduct.productFromString( productParts[i] ) );
		
		if( productListReceived != null )
			productListReceived( productList );
	}
	
	
	public void productsRequestDidFail( string error )
	{
		if( productListRequestFailed != null )
			productListRequestFailed( error );
	}
	
	
	public void validateReceiptFailed( string error )
	{
		if( receiptValidationFailed != null )
			receiptValidationFailed( error );
	}
	
	
	public void validateReceiptRawResponse( string response )
	{
		if( receiptValidationRawResponseReceived != null )
			receiptValidationRawResponseReceived( response );
	}
	
	
	public void validateReceiptFinished( string statusCode )
	{
		if( statusCode == "0" )
		{
			if( receiptValidationSuccessful != null )
				receiptValidationSuccessful();
		}
		else
		{
			if( receiptValidationFailed != null )
				receiptValidationFailed( "Receipt validation failed with statusCode: " + statusCode );
		}
	}
	
	
	public void restoreCompletedTransactionsFailed( string error )
	{
		if( restoreTransactionsFailed != null )
			restoreTransactionsFailed( error );
	}
	
	
	public void restoreCompletedTransactionsFinished( string empty )
	{
		if( restoreTransactionsFinished != null )
			restoreTransactionsFinished();
	}
#endif
}

