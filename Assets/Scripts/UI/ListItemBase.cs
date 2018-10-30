using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace AddComponent{
	public class ListItemBase : MonoBehaviour {
		
		#region HANDLER Selected
		public delegate void OnSelectHandler (ListItemBase item);
		public OnSelectHandler onSelected;

		public void Selected (bool clear = false){
			if (onSelected != null){
				onSelected(this);
				if (clear){
					onSelected = null;
				}
			}
		}

		#endregion
		
		[SerializeField]
		private RectTransform _rectTransform;

		public int index
		{
			get ;
			set ;
		}	

		public Vector2 Size {
			get 
			{
				return _rectTransform.anchoredPosition;
			}
			set
			{
				_rectTransform.sizeDelta = value;
			}
		}

		public Vector2 Position
		{
			get 
			{
				return _rectTransform.anchoredPosition;
			}

			set
			{
				_rectTransform.anchoredPosition = value;
			}
		}

	}

}