using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using MiniJSON;

public class RivetyProductManager : MonoBehaviour
{
	public enum State
	{
		Default = 0,
		DownloadingProductData
	}

	private IList _products;
	public IList products
	{
		get { return this._products; }
		set { this._products = value; }
	}

	private IList _localPurchases;

	private IList _applePurchases;
	public IList applePurchases
	{
		get { return this._applePurchases; }
		set { this._applePurchases = value; }
	}

	public static event Action ProductsDownloadedEvent;

	public bool IsOwned(string sku)
	{
		// this only checks PlayerPrefs and verifies hashes
		// to check with the server, the user needs to use the Restore Purchases feature
		string hash = PlayerPrefs.GetString(sku, "");
		string orderId = PlayerPrefs.GetString(sku + "_order_id", "");
		if (hash == "" || orderId == "") return false;

// Debug.Log("hash = " + HashHelper.getMd5Hash(sku + orderId)); // cygdeck001 + google12345 = 176a2911b8591fd36c6f092da107ef8d

		return HashHelper.verifyMd5Hash(sku + orderId, hash);
	}

	public bool HasProducts()
	{
		return (this._products != null);
	}

	public static string ParseSku(string productId)
	{
		// take in a platform-specific product ID and give back a SKU
		// a product ID has a platform-specific prefix
		// a sku has no prefix
		string sku = productId;
		if (productId.IndexOf("apl") == 0 || productId.IndexOf("ggl") == 0) sku = productId.Substring(3);
		return sku;
	}

	public void GetProducts()
	{
		// TODO: get product information from storekit

		if (ProductsDownloadedEvent != null) ProductsDownloadedEvent();
	}

	public void RestoreProductsForCurrentUser()
	{
		// TODO: restore products from storekit
	}

}
