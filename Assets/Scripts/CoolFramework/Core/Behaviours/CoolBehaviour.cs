using UnityEngine;

namespace CoolFramework.Core
{
    /// <summary>
    /// Base class to derivate from to handle Behaviours of the Cool Framework.
    /// This class will might be the most important one of this framework and will be modified as the framework progresses.
    /// </summary>
    public abstract class CoolBehaviour : MonoBehaviour, IInitUpdate
    {
        #region Fields and Properties
        bool IInitUpdate.HasBeenInitialized { get; set; } = false;
        public virtual UpdateRegistration UpdateRegistration => 0; 
        #endregion

        #region Private Methods
        /// <summary>
        /// Call this method when the Behaviour is enabled
        /// </summary>
        protected virtual void EnableBehaviour() 
        {
#if UNITY_EDITOR
            if (!Application.isPlaying) return;
#endif
            try
            {
                if (UpdateRegistration != 0) UpdateManager.Instance.Register(this, UpdateRegistration);
            }
            catch(System.NullReferenceException _e)

            {
                Debug.LogWarning("The Instance of the Update Manager has not been set yet. You must add this script to the Core Scene or change its script execution order to call it before this script.");
            }
        }
        /// <summary>
        /// Call this method when the Behaviour is disabled
        /// </summary>
        protected virtual void DisableBehaviour() 
        {
#if UNITY_EDITOR
            if (!Application.isPlaying) return;
#endif

            if (UpdateRegistration != 0) UpdateManager.Instance.Unregister(this, UpdateRegistration);
        }


        /// <summary>
        /// Base Unity method to handle the enabling of the behaviour.
        /// <b>Must not be overriden</b> 
        /// </summary>
        protected virtual void OnEnable() => EnableBehaviour();
        /// <summary>
        /// Base Unity method to handle the disabling of the behaviour.
        /// <b>Must not be overriden</b> 
        /// </summary>
        protected virtual void OnDisable()
        {
            if(UpdateManager.IsQuitting) return;
            DisableBehaviour();
        }
        protected virtual void OnInit() { }
        #endregion

        #region Public Methods
        void IInitUpdate.Init() => OnInit();
        #endregion
    }
}
