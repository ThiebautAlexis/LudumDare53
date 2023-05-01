using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace CoolFramework.SceneManagement
{
    [CreateAssetMenu(fileName = "SceneBundle_", menuName = "Cool FrameWork/Scene Management/Scene Bundle")]
    public class SceneBundle : ScriptableObject
    {
        #region Fields and Properites
        private AsyncOperationHandle<SceneInstance>[] scenesInstances = null;
        public AssetReference[] ScenesReferences = new AssetReference[] { };
        public bool CanBeUnloaded = true; 
        #endregion

        #region Private Methods
        #endregion

        #region Public Methods
        /// <summary>
        /// Load the scene at the corresponding index of the <see cref="ScenesReferences"/> array.
        /// Save the Operation in the <see cref="scenesInstances"/> array at the correct index to unload it properly.
        /// </summary>
        /// <param name="_operation">The current operation to load the scene.</param>
        /// <param name="_parameters">Loading Scene Parameters.</param>
        /// <param name="_loadedIndex">Loaded Scene Index.</param>
        /// <param name="_activateOnLoad">Is the scene active on load.</param>
        /// <param name="_priority">Priority of the Async Operation.</param>
        /// <returns>Return true if the Scene can be loaded.</returns>
        public bool LoadSceneAsyncAt(out AsyncOperationHandle<SceneInstance> _operation, LoadSceneParameters _parameters, int _loadedIndex, bool _activateOnLoad = true, int _priority = 100)
        {
            if (scenesInstances == null)
                scenesInstances = new AsyncOperationHandle<SceneInstance>[ScenesReferences.Length];

            _operation = Addressables.LoadSceneAsync(ScenesReferences[_loadedIndex], _parameters.loadSceneMode, _activateOnLoad, _priority);
            scenesInstances[_loadedIndex] = _operation;
            return true;
        }

        /// <summary>
        /// Unload the scene at the corresponding index of the <see cref="scenesInstances"/> array.
        /// </summary>
        /// <param name="_operation">The current operation to unload the scene.</param>
        /// <param name="_options">The unload scenes options.</param>
        /// <param name="_unloadedIndex">The index of the unloaded scene.</param>
        /// <returns>Return true if the scene can be unloaded.</returns>
        public bool UnloadSceneAsyncAt(out AsyncOperationHandle<SceneInstance> _operation, UnloadSceneOptions _options, int _unloadedIndex)
        {
            if (scenesInstances == null) Debug.LogError("Scenes Instances are null");
            _operation = Addressables.UnloadSceneAsync(scenesInstances[_unloadedIndex], _options);
            return true;
        }
        #endregion
    }
}
