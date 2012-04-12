using UnityEngine;
using System;
using System.Text;
using System.Collections;
using System.Security.Cryptography;

public class HashHelper
{

	private static string _masterSalt = "lavakitten";

	public static bool verifyMd5Hash(string input, string hash)
	{
		StringComparer comparer = StringComparer.OrdinalIgnoreCase;
		return (0 == comparer.Compare(getMd5Hash(input), hash));
	}

	public static string getMd5Hash(string input = "")
	{
		if (input == "") return "";
		input = _masterSalt + input + _masterSalt;
		MD5 md5Hasher = MD5.Create();
		byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));
		StringBuilder sBuilder = new StringBuilder();
		for (int i = 0; i < data.Length; i++)
		{
			sBuilder.Append(data[i].ToString("x2"));
		}
		return sBuilder.ToString();
	}

}
