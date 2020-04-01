using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/******************************************************/
/*****************   Singleton Class  *****************/
/******************************************************/

// A singleton is used to conveniently get access to the private 
// component informations from other classes
// It's used as the inherit base class
public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T instance;

	public static T Instance
	{
		get
		{
			if (instance == null)
			{
				instance = FindObjectOfType<T>();
			}
			return instance;
		}
	}
}
