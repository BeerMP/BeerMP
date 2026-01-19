using UnityEngine;

namespace BeerMP.Extensions
{
	public static class GameObjectExtensions
	{
		public static GameObject Instantiate( this GameObject original, Transform parent, string name, bool worldPositionStays = false )
		{
			var clone = original.Instantiate( parent, worldPositionStays );
			if ( clone != null ) clone.name = name;
			return clone;
		}

		public static GameObject Instantiate( this GameObject original, Transform parent, bool worldPositionStays = false )
		{
			if ( original == null ) return null;

			var clone = GameObject.Instantiate( original );
			clone.transform.SetParent( parent, worldPositionStays );
			return clone;
		}

		public static GameObject Instantiate( this GameObject original, Transform parent, Vector3 localPosition, Quaternion localRotation )
		{
			var clone = original.Instantiate( parent, false );
			if ( clone != null )
			{
				clone.transform.localPosition = localPosition;
				clone.transform.localRotation = localRotation;
			}

			return clone;
		}

		public static GameObject Instantiate( this GameObject original, Transform parent, string name, Vector3 localPosition, Quaternion localRotation )
		{
			var clone = original.Instantiate( parent, localPosition, localRotation );
			if ( clone != null ) clone.name = name;
			return clone;
		}
	}
}
