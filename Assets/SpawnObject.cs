using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnObject : MonoBehaviour {
    private float _currentScale = initScale;
    private const float maxScale2 = 0.1f;
    private const float maxScale = 0.1f;
    private const float initScale = 0f;
    private const float FramesCount = 100;
    private const float AnimationTimeSecond = 2f;
    private float _deltaTime = AnimationTimeSecond/100;
    private float _dx = (maxScale - initScale)/FramesCount;
    private bool _upScale = true;
    public GameObject _gameobj2;


     void Start()
        {
           // _gameobj2 = GetComponentInChildren<GameObject>();
           //GameObject a = GameObject.FindGameObjectWithTag 
            _gameobj2.transform.localScale = Vector3.zero;
			StartCoroutine(ResizeGameObject(_gameobj2));
        }

    private IEnumerator ResizeGameObject(GameObject _gameobj)
    {
       
        Debug.Log("Resizing");
        while (_upScale)
        {
            _currentScale += _dx;
            //change GO Size

            if (_currentScale > maxScale)
            {
                _upScale = false;
                _currentScale = maxScale;
            }

            _gameobj.transform.localScale = Vector3.one * _currentScale;
            yield return new WaitForSeconds (_deltaTime);
        }
    }
}