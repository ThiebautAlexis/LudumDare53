using System.Collections; 
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CoolFramework.SceneManagement
{
    public abstract class LoadingBehaviour : ScriptableObject
    {
        /// <summary> Call this method before starting the loading of the scene bunlde</summary>
        public abstract IEnumerator OnPreLoading(CoolSceneManager _manager);
        /// <summary> Call this method when starting the loading of the scene bunlde</summary>
        public abstract IEnumerator OnStartLoading(SceneBundle _bundle, LoadSceneMode _mode);
        /// <summary> Call this method when the loading of the scene bunlde is stopped</summary>
        public abstract void OnStopLoading();
        /// <summary> Call this method after the loading of the scene bunlde</summary>
        public abstract IEnumerator OnPostLoading(CoolSceneManager _manager);

        /// <summary> Call this method before starting the unloading of the scene bunlde</summary>
        public abstract IEnumerator OnPreUnloading(CoolSceneManager _manager);
        /// <summary> Call this method when starting the unloading of the scene bunlde</summary>
        public abstract IEnumerator OnStartUnloading(SceneBundle _bundle, UnloadSceneOptions _options);
        /// <summary> Call this method when the unloading of the scene bunlde is stopped</summary>
        public abstract void OnStopUnloading();
        /// <summary> Call this method after the unloading of the scene bunlde</summary>
        public abstract IEnumerator OnPostUnloading(CoolSceneManager _manager);
    }
}
