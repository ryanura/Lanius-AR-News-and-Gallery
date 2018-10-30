using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class ScrollViewAdapter : MonoBehaviour {
	public Texture2D[] availableIcons;

	public RectTransform _prefab;
	public Text _countText;
	public RectTransform _content;

	public List<ExampleItemView> _views = new List<ExampleItemView>();


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	public void UpdateItem () {
		int newCount;
		int .TryParse(_countText.text, out newCount);
		StartCoroutine(FetchItemModelsFromServer(newCount, results => OnReceivedNewModels(results)));
	}

	void OnReceivedNewModels (ExampleItemModel[] models)
	{
		foreach (Transform child in _content)
		{
			Destroy(child.gameObject);
		}

	//	views.Clear();

		int i = 0;
	 	
		foreach (var model in models)
		{
			var instance = GameObject.Instantiate(_prefab.gameObject) as GameObject;
			instance.transform.SetParent(_content, false);
			//initializeItemView();

			i++;

		}
	}
	
	ExampleItemView initializeItemView(GameObject viewGameObject, ExampleItemModel model )
	{
		ExampleItemView view = new ExampleItemView(viewGameObject.transform);

		view._titleText.text = model._title;
		view._thumbnailImg.texture = availableIcons[model._imgIndex];

		return view; 

	}

	IEnumerator FetchItemModelsFromServer(int count, Action<ExampleItemModel[]> onDone)
	{
		yield return new WaitForSeconds(2f);

		var results = new ExampleItemModel[count];

		for (int i = 0; i < count; i++)
		{
			results[i] = new ExampleItemModel();
			results[i]._title = "Feed " + i;
			//results[i]._imgIndex = UnityEngine.Random(0, )

		}

		onDone (results);
		
	}
	

	public class ExampleItemModel
	{
		public string _title, _subtitle;
		public int _imgIndex; 
	}

	public class ExampleItemView
	{
		public Text _titleText;
		public RawImage _thumbnailImg;

		public ExampleItemView (Transform rootView)
		{
			_titleText = rootView.Find("TitlePanel/TItleText").GetComponent<Text>();
			_thumbnailImg = rootView.Find("ThumnailImage").GetComponent<RawImage>();
		}
	}
}
