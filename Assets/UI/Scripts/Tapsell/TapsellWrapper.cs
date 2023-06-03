using UnityEngine;
using System.Collections;
using System.Runtime;
using System;
using System.Collections.Generic;

public class TapsellWrapper : MonoBehaviour{
	Action<String, String> action;
	Dictionary<String, Action<Boolean, Boolean>> consumeProductAction = new Dictionary<String, Action<Boolean, Boolean>>();

	public void setPurchaseNotifier(Action<String, String> action){
		this.action = action;
	}

	public void notifyPurchased(string str){
		String sku = str.Substring(0, str.LastIndexOf(" "));
		String purchasedIt = str.Substring (sku.Length + 1);
		action (sku, purchasedIt);
	}

	public void consumeProduct(String sku, Action<Boolean, Boolean> action){
		if (consumeProductAction.ContainsKey (sku))
			consumeProductAction.Remove (sku);
		consumeProductAction.Add(sku, action);
	}

	public void notifyConsumeProduct(String ans){
		Boolean first, second;
		if (ans [0] == 't')
			first = true;
		else
			first = false;

		if (ans [1] == 't')
			second = true;
		else
			second = false;
		String newSku = ans.Substring (2);
		if (consumeProductAction.ContainsKey(newSku)){
			consumeProductAction[newSku](first, second);
		}
	}

	public void notifyCtaAvailability(String str){
		DeveloperCtaInterface.getInstance().notifyCtaAvailability (str);
	}

	public void notifyDirectAd(String str){
		DeveloperCtaInterface.getInstance().notifyDirectAd(str);
	}
}
