using System; 
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace CoolFramework.SceneManagement
{
    /// <summary>
    /// Class to inherit from to handle the asynchronous operations of the Scenes Bundle. 
    /// </summary>
    public abstract class SceneBundleAsyncOperation : CustomYieldInstruction 
    {
        #region Fields and Properties
        protected AsyncOperationHandle<SceneInstance> currentOperation;
        protected SceneBundle scenesBundle = null;
        protected int sceneIndex = 0;

        protected bool isCompleted = false;
        protected float progress = 0f;
        protected int priority = 0; 

        public abstract bool IsCompleted { get; }

        /// <summary>
        /// Total Progress of all the operation of the bundle.
        /// </summary>
        public float Progress
        {
            get
            {
                return isCompleted ? 1.0f : (progress + (currentOperation.PercentComplete / scenesBundle.ScenesReferences.Length)); 
            }
        }

        public int Priority
        {
            get => priority; 
            set => priority = value;
        }

        public override bool keepWaiting => !isCompleted;
        #endregion

        #region Private Methods
        protected abstract void OnOperationCompleted(AsyncOperationHandle<SceneInstance> _operation);

        #endregion
    }

    /// <summary>
    /// Class to handle the loading of a scene bundle as an asynchronous operation.
    /// </summary>
    public class LoadSceneBundleAsyncOperation : SceneBundleAsyncOperation
    {
        #region Fields and Properties
        public override bool IsCompleted => isCompleted;
        /// <summary> Load Scene paramters, override to Additive in order to keep the core scene and all scenes in the bundle</summary>
        private LoadSceneParameters parameters = default; 
        #endregion

        #region Events 
        /// <summary> Callback when a scene of the bundle is loaded</summary>
        public event Action<Scene> OnSceneLoaded = null; 
        /// <summary> Callback when the complete bundle has been loaded and the operation is completed.</summary>
        public event Action<LoadSceneBundleAsyncOperation> OnLoadSceneBundleCompleted = null;
        #endregion

        #region Constructor 
        internal LoadSceneBundleAsyncOperation()
        {
            isCompleted = true;
            progress = 1.0f;
        }

        internal LoadSceneBundleAsyncOperation(SceneBundle _sceneBundle, LoadSceneParameters _parameters)
        {
            scenesBundle = _sceneBundle;
            parameters = new LoadSceneParameters(LoadSceneMode.Additive, _parameters.localPhysicsMode);

            if (LoadNextScene(parameters))
                currentOperation.Completed += OnOperationCompleted; 
        }
        #endregion 

        #region Private Methods
        /// <summary>
        /// Call the OnsceneLoaded Callback.
        /// Then increase the scene index and call complete the operation if the complete bundle has been loaded.
        /// Else load the next scene and update the progress of the operation.
        /// </summary>
        /// <param name="_operation">The completed operation</param>
        protected override void OnOperationCompleted(AsyncOperationHandle<SceneInstance> _operation)
        {
            // Loaded Scene callback.
            OnSceneLoaded?.Invoke(_operation.Result.Scene);

            sceneIndex++;

            // All scenes of the bundle are loaded here.
            if (sceneIndex == scenesBundle.ScenesReferences.Length)
            {
                isCompleted = true;
                progress = 1.0f;

                OnLoadSceneBundleCompleted?.Invoke(this);
                return; 
            }

            // Load the next scene.
            if(LoadNextScene(parameters))
            {
                currentOperation.Completed += OnOperationCompleted;
                progress = (float)scenesBundle.ScenesReferences.Length / sceneIndex;
            }
        }

        /// <summary>
        /// Load the next scene of the bundle.
        /// If the scene can't be loaded, skip to the next scene, unless it was the last one of the budle. 
        /// Then complete the operation and call the according callbacks.
        /// </summary>
        /// <param name="_parameters"></param>
        /// <returns></returns>
        protected bool LoadNextScene(LoadSceneParameters _parameters)
        {
            while (!scenesBundle.LoadSceneAsyncAt(out currentOperation, _parameters, sceneIndex))
            {
                sceneIndex++;

                if(sceneIndex == scenesBundle.ScenesReferences.Length)
                {
                    isCompleted = true;
                    progress = 1.0f;

                    OnLoadSceneBundleCompleted?.Invoke(this);
                    return false;
                }
            }
            return true;
        }
        #endregion
    }

    /// <summary>
    /// Class to handle the unloading of a scene bundle as an asynchronous operation.
    /// </summary>
    public class UnloadSceneBundleAsyncOperation : SceneBundleAsyncOperation
    {
        #region Fields and Properties
        /// <summary>Unload scene options.</summary>
        private UnloadSceneOptions options = default; 
        public override bool IsCompleted => isCompleted;
        #endregion

        #region Events
        /// <summary>Callback when a scene has been unloaded.</summary>
        public event Action<Scene> OnSceneUnloaded;
        /// <summary> Callback when the bundle has been completed and all scene has been unloaded.</summary>
        public event Action<UnloadSceneBundleAsyncOperation> OnUnloadSceneBundleCompleted;
        #endregion

        #region Constructor
        internal UnloadSceneBundleAsyncOperation() 
        {
            isCompleted = true;
            progress = 1.0f; 
        }

        internal UnloadSceneBundleAsyncOperation(SceneBundle _bundle, UnloadSceneOptions _options = default)
        {
            scenesBundle = _bundle;
            options = _options;

            if (UnloadNextScene(options))
                currentOperation.Completed += OnOperationCompleted;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Unload the next scene of the bundle.
        /// If the scene can't be unloaded or if there is no more loaded scenes, skip to the next scene, unless it was the last one of the budle. 
        /// Then complete the operation and call the according callbacks.
        /// </summary>
        /// <param name="_parameters"></param>
        /// <returns></returns>
        private bool UnloadNextScene(UnloadSceneOptions _options)
        {
            bool _canUnloadScene;
            while (!(_canUnloadScene = SceneManager.sceneCount > 1) || !scenesBundle.UnloadSceneAsyncAt(out currentOperation, _options, sceneIndex))
            {
                sceneIndex++;
                if(!_canUnloadScene || (sceneIndex == scenesBundle.ScenesReferences.Length))
                {
                    isCompleted = true; 
                    progress = 1.0f;

                    OnUnloadSceneBundleCompleted?.Invoke(this);
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Call the OnsceneUnloaded Callback.
        /// Then increase the scene index and call complete the operation if the complete bundle has been unloaded.
        /// Else unload the next scene and update the progress of the operation.
        /// </summary>
        /// <returns>Can the scene be unloaded?</returns>
        protected override void OnOperationCompleted(AsyncOperationHandle<SceneInstance> _operation)
        {
            // Unloaded Scene callback.
            OnSceneUnloaded?.Invoke(_operation.Result.Scene);

            sceneIndex++;

            // All scenes of the bundle are unloaded here.
            if (sceneIndex == scenesBundle.ScenesReferences.Length)
            {
                isCompleted = true;
                progress = 1.0f;

                OnUnloadSceneBundleCompleted?.Invoke(this);
                return;
            }

            // Unload the next scene.
            if (UnloadNextScene(options))
            {
                currentOperation.Completed += OnOperationCompleted;
                progress = (float)scenesBundle.ScenesReferences.Length / sceneIndex;
            }
        }
        #endregion
    }
}
