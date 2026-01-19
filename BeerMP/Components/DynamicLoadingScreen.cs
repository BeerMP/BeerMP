using UnityEngine;

namespace BeerMP.Components
{
	public class DynamicLoadingScreen : MonoBehaviour
	{
		private TextMesh loadingText;
		private GameObject loadingCamera;

		private void Awake()
		{
			loadingCamera = transform.GetChild( 1 ).gameObject;
			loadingText = transform.GetChild( 2 ).GetComponent<TextMesh>();
			SetVisibility( false );
		}

		public void SetVisibility( bool visible, string text = null )
		{
			loadingCamera?.gameObject.SetActive( visible );
			if ( !string.IsNullOrEmpty( text ) )
			{
				SetText( text );
			}
		}

		public void SetText( string text )
		{
			if ( loadingText != null && !string.IsNullOrEmpty( text ) )
			{
				loadingText.text = text;
			}
		}

		public void Hide()
		{
			SetVisibility( false );
		}

		public bool IsVisible()
		{
			return loadingCamera?.activeInHierarchy == true;
		}
	}
}
