using UnityEngine;

namespace CoolFramework.Core
{
    /// <summary>
    /// Class to inherit from in order to handle the Singleton Behaviour of an object.
    /// Be careful to keep just one Instance of this object.
    /// (Should I handle the case where there is multiple instances of this object?)
    /// </summary>
    /// <typeparam name="T">The Type of the Obejct you want to be a Singleton.</typeparam>
    public abstract class CoolSingleton<T> : CoolBehaviour where T : CoolSingleton<T>
    {
        #region Fields and Properties
        /// <summary> The static instance of this class.</summary>
        private static T instance = null; 

        /// <summary> Singleton Instance of this class. </summary>
        public static T Instance
        {
            get 
            {
                #if UNITY_EDITOR
                if(!Application.isPlaying && instance == null) 
                {
                    instance = FindObjectOfType<T>();
                }
                #endif
                return instance;
            }
            protected set { instance = value; }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Enabling Behaviour of the Singleton.
        /// If the Singleton Instance already exists, call the according method to handle that case.
        /// </summary>
        protected override void EnableBehaviour()
        {
            if (instance is null)
            {
                Instance = this as T;
            }
            else
            {
                OnSingletonInstanceAlreadyExists();
                return;
            }
            base.EnableBehaviour();
        }

        protected override void DisableBehaviour()
        {
            base.DisableBehaviour();

            if(instance is not null)
            {
                Instance = null;
            }
        }

        /// <summary>
        /// Method to handle the case where there is more than one instance of this Singleton.
        /// You should override this method to handle more cases.
        /// </summary>
        protected virtual void OnSingletonInstanceAlreadyExists()
        {
            Destroy(this); 
        }
        #endregion

        #region Public Methods
        #endregion
    }
}
