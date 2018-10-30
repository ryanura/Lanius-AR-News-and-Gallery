using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Vuforia{
public class DefaultTrackableEventHandlerCustom : MonoBehaviour, ITrackableEventHandler {
	
	/// <summary>
    /// A custom handler that implements the ITrackableEventHandler interface.
    /// </summary>
    //public class DefaultTrackableEventHandler : MonoBehaviour, ITrackableEventHandler
    
        #region PRIVATE_MEMBER_VARIABLES
 
        private TrackableBehaviour mTrackableBehaviour;
    
        #endregion // PRIVATE_MEMBER_VARIABLES


		#region CUSTOM_VARIABLES
		private float _currentScale = initScale;
		private const float maxScale2 = 0.1f;
		private const float maxScale = 1f;
		private const float initScale = 0f;
		private const float FramesCount = 100;
		private const float AnimationTimeSecond = 2f;
		private float _deltaTime = AnimationTimeSecond/FramesCount;
		private float _dx = (maxScale - initScale)/FramesCount;
		private bool _upScale = true;
        public bool _playanim = false;
        public Text debug;

		
		#endregion // CUSTOM_VARIABLES


        #region UNTIY_MONOBEHAVIOUR_METHODS
    
        public void Start()
        {
            mTrackableBehaviour = GetComponent<TrackableBehaviour>();
            if (mTrackableBehaviour)
            {
                mTrackableBehaviour.RegisterTrackableEventHandler(this);
			}
        }
        #endregion // UNTIY_MONOBEHAVIOUR_METHODS

        public void PlayAnimOnce()
        {
            Animator[] animatorComponents = GetComponentsInChildren<Animator>(true);


            foreach (Animator component in animatorComponents)
            {
                component.enabled = true;
                component.Play("Loop");
            }
       
        }

		IEnumerator ResizeGameObject(GameObject _gameobj)
		{
            _gameobj.transform.localScale = Vector3.zero;
            debug.text = "Resizing";
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
            debug.text = "Resizing done";
        }

        #region PUBLIC_METHODS
        /// <summary>
        /// Implementation of the ITrackableEventHandler function called when the
        /// tracking state changes.
        /// </summary>
        public void OnTrackableStateChanged(
                                        TrackableBehaviour.Status previousStatus,
                                        TrackableBehaviour.Status newStatus)
        {
            if (newStatus == TrackableBehaviour.Status.DETECTED ||
                newStatus == TrackableBehaviour.Status.TRACKED ||
                newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
            {
                OnTrackingFound();
            }
            else
            {
                OnTrackingLost();
            }
        }
        #endregion // PUBLIC_METHODS


        #region PRIVATE_METHODS

        private void OnTrackingFound()
        {
            Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
            Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);
            Canvas[] canvasComponents = GetComponentsInChildren<Canvas>(true);
            Animator[] animatorComponents = GetComponentsInChildren<Animator>(true);
       
            //GameObject gameObjectComponents = this.transform.GetChild(0).gameObject;
            
			//GameObject[] gameObjectComponents = GetComponentsInChildren <GameObject>(true);
            
            // Enable rendering:
            foreach (Renderer component in rendererComponents)
            {
                component.enabled = true;
            }

            // Enable colliders:
            foreach (Collider component in colliderComponents)
            {
                component.enabled = true;
            }

            foreach (Canvas component in canvasComponents)
            {
                component.enabled = true;
            }

            foreach (Animator component in animatorComponents)
            {
                component.enabled = true;
                component.Play("Loop");
            }

            debug.text = "Trackable " + mTrackableBehaviour.TrackableName + " found";
        }


        private void OnTrackingLost()
        {
            
            Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
            Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);
            Canvas[] canvasComponents = GetComponentsInChildren<Canvas>(true);
            Animator[] animatorComponents = GetComponentsInChildren<Animator>(true);
	
            // Disable rendering:
            foreach (Renderer component in rendererComponents)
            {
                component.enabled = false;
            }

            // Disable colliders:
            foreach (Collider component in colliderComponents)
            {
                component.enabled = false;
            }

            foreach (Canvas component in canvasComponents)
            {
                component.enabled = false;
            }

            foreach (Animator component in animatorComponents)
            {
                
            }

            debug.text = "Trackable " + mTrackableBehaviour.TrackableName + " lost";
            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");
        }

        #endregion // PRIVATE_METHODS
    }
}