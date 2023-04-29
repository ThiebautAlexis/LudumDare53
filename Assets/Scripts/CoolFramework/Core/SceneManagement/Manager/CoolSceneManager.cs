using UnityEngine; 
using UnityEngine.SceneManagement; 
using System;
using System.Collections;
using CoolFramework.Core;

namespace CoolFramework.SceneManagement
{
    public class CoolSceneManager : CoolSingleton<CoolSceneManager>
    {
        #region Fields and Properties
        public override UpdateRegistration UpdateRegistration => UpdateRegistration.Init ;

        [SerializeField] private bool loadFirstScene; 
        [SerializeField] private SceneBundle firstLoadedSceneBundle;
        [SerializeField] private LoadingBehaviour defaultLoadingBehaviour;
        #endregion

        #region Events
        public static event Action OnStartLoading;
        public static event Action OnStopLoading;

        public static event Action OnStartUnloading;
        public static event Action OnStopUnloading;

        #endregion

        #region Private Methods

        #region Loading
        /// <summary>
        /// Load the scene bundle.
        /// Apply the global and the loading behaviour callbacks before and after loading the scene.
        /// </summary>
        /// <param name="_bundle">Loaded Bundle</param>
        /// <param name="_mode">LoadSceneMode (Single or additive)</param>
        public IEnumerator LoadSceneBundle(SceneBundle _bundle, LoadSceneMode _mode)
        {
            yield return defaultLoadingBehaviour.OnPreLoading(this);

            OnStartLoading?.Invoke();
            yield return defaultLoadingBehaviour.OnStartLoading(_bundle, _mode);

            yield return defaultLoadingBehaviour.OnPostLoading(this);

            defaultLoadingBehaviour.OnStopLoading();
            OnStopLoading?.Invoke();
        }

        /// <summary>
        /// Load the scene bundle.
        /// Apply the global and the loading behaviour callbacks before and after loading the scene.
        /// </summary>
        /// <param name="_bundle">Loaded Bundle</param>
        /// <param name="_mode">LoadSceneMode (Single or additive)</param>
        /// <param name="_loadingBehaviour">Loading behaviour</param>
        /// <returns></returns>
        public IEnumerator LoadSceneBundle(SceneBundle _bundle, LoadSceneMode _mode, LoadingBehaviour _loadingBehaviour)
        {
            yield return _loadingBehaviour.OnPreLoading(this);

            OnStartLoading?.Invoke();
            yield return _loadingBehaviour.OnStartLoading(_bundle, _mode);

            yield return _loadingBehaviour.OnPostLoading(this);

            _loadingBehaviour.OnStopLoading();
            OnStopLoading?.Invoke();
        }
        #endregion

        #region Unloading 
        /// <summary>
        /// Unload the scene bundle.
        /// Apply the global and the unloading behaviour callbacks before and after unloading the scene.
        /// </summary>
        /// <param name="_bundle">Scene Bundle to Unload</param>
        /// <param name="_options">Unloading options</param>
        public IEnumerator UnloadSceneBundle(SceneBundle _bundle, UnloadSceneOptions _options)
        {
            yield return defaultLoadingBehaviour.OnPreUnloading(this);

            OnStartUnloading?.Invoke();
            yield return defaultLoadingBehaviour.OnStartUnloading(_bundle, _options);

            yield return defaultLoadingBehaviour.OnPostUnloading(this);

            defaultLoadingBehaviour.OnStopUnloading();
            OnStopUnloading?.Invoke();
        }

        /// <summary>
        /// Unload the scene bundle.
        /// Apply the global and the unloading behaviour callbacks before and after unloading the scene.
        /// </summary>
        /// <param name="_bundle">Scene Bundle to Unload</param>
        /// <param name="_options">Unloading options</param>
        /// <param name="_unloadingBehaviour">Unloading Behaviour</param>
        public IEnumerator UnloadSceneBundle(SceneBundle _bundle, UnloadSceneOptions _options, LoadingBehaviour _unloadingBehaviour)
        {
            yield return _unloadingBehaviour.OnPreUnloading(this);

            OnStartUnloading?.Invoke();
            yield return _unloadingBehaviour.OnStartUnloading(_bundle, _options);

            yield return _unloadingBehaviour.OnPostUnloading(this);

            _unloadingBehaviour.OnStopUnloading();
            OnStopUnloading?.Invoke();
        }
        #endregion

        #endregion

        #region Public Methods
        protected override void OnInit()
        {
            base.OnInit();
            if(loadFirstScene)
                StartCoroutine(LoadSceneBundle(firstLoadedSceneBundle, LoadSceneMode.Additive));
        }
        #endregion
    }
}
