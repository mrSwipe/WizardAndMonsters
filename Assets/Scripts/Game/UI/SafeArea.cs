using UnityEngine.Serialization;

// Основано на CrystalFramework.SafeArea

namespace UnityEngine.UI.Extensions
{
	/// <summary>
	/// Safe area implementation for notched mobile devices. Usage:
	///  (1) Add this component to the top level of any GUI panel. 
	///  (2) If the panel uses a full screen background image, then create an immediate child and put the component on that instead, with all other elements childed below it.
	///      This will allow the background image to stretch to the full extents of the screen behind the notch, which looks nicer.
	///  (3) For other cases that use a mixture of full horizontal and vertical background stripes, use the Conform X & Y controls on separate elements as needed.
	/// </summary>
	public class SafeArea : MonoBehaviour
	{
		[FormerlySerializedAs("ConformX")]
		[SerializeField]
		private bool _conformX = true; // Conform to screen safe area on X-axis (default true, disable to ignore)
		[FormerlySerializedAs("ConformY")]
		[SerializeField]
		private bool _conformY = true; // Conform to screen safe area on Y-axis (default true, disable to ignore)
		private RectTransform Panel;
		private Rect LastSafeArea = new Rect(0, 0, 0, 0);

		public void ResetSafeArea()
		{
			LastSafeArea = new Rect();
		}

		private void Awake()
		{
			Panel = GetComponent<RectTransform>();

			if(Panel == null)
			{
				Debug.LogError("Cannot apply safe area - no RectTransform found on " + name);
				Destroy(gameObject);
			}

			Refresh();
		}

		private void Update()
		{
			Refresh();
		}

		private void Refresh()
		{
			Rect safeArea = GetSafeArea();

			if(safeArea != LastSafeArea)
			{
				ApplySafeArea(safeArea);
			}
		}

		private Rect GetSafeArea()
		{
			Rect safeArea = Screen.safeArea;
			return safeArea;
		}

		private void ApplySafeArea(Rect r)
		{
			LastSafeArea = r;

			// Ignore x-axis?
			if(!_conformX)
			{
				r.x = 0;
				r.width = Screen.width;
			}

			// Ignore y-axis?
			if(!_conformY)
			{
				r.y = 0;
				r.height = Screen.height;
			}

			// Convert safe area rectangle from absolute pixels to normalised anchor coordinates
			var anchorMin = r.position;
			var anchorMax = r.position + r.size;
			anchorMin.x /= Screen.width;
			anchorMin.y /= Screen.height;
			anchorMax.x /= Screen.width;
			anchorMax.y /= Screen.height;

			if(_conformX && _conformY)
			{
				Panel.anchorMin = anchorMin;
				Panel.anchorMax = anchorMax;
			}
			else if(_conformX)
			{
				Panel.anchorMin = new Vector2(anchorMin.x, Panel.anchorMin.y);
				Panel.anchorMax = new Vector2(anchorMax.x, Panel.anchorMax.y);
			}
			else if(_conformY)
			{
				Panel.anchorMin = new Vector2(Panel.anchorMin.x, anchorMin.y);
				Panel.anchorMax = new Vector2(Panel.anchorMax.x, anchorMax.y);
			}
		}
	}
}
