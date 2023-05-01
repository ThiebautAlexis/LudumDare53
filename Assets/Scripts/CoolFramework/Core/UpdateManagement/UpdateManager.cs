using UnityEngine;
using Debug = UnityEngine.Debug;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using CoolFramework.SceneManagement; 

namespace CoolFramework.Core
{
    #region Interfaces and Registrations
    [Flags]
    public enum UpdateRegistration
    {
        Init        = 1 << 0,

        Early       = 1 << 8,
        Input       = 1 << 10,
        Update      = 1 << 12,
        Dynamic     = 1 << 18,
        Late        = 1 << 24,

        Permanent   = 1 << 28,
    }

    public interface IBaseUpdate { };
    public interface IInitUpdate : IBaseUpdate
    {
        bool HasBeenInitialized { get; set; }
        void Init();
    }
    public interface IPermanentUpdate   : IBaseUpdate { void Update(); }
    public interface IEarlyUpdate       : IBaseUpdate { void Update(); }
    public interface IInputUpdate       : IBaseUpdate { void Update(); }
    public interface IUpdate            : IBaseUpdate { void Update(); }
    public interface IDynamicUpdate     : IBaseUpdate { void Update(); }
    public interface ILateUpdate        : IBaseUpdate { void Update(); }
    #endregion

    public class UpdateManager : CoolSingleton<UpdateManager>
    {
        #region Fields and Properties
        public override UpdateRegistration UpdateRegistration => UpdateRegistration.Init;

        private List<KeyValuePair<IInitUpdate, UpdateRegistration>> initUpdates = new List<KeyValuePair<IInitUpdate, UpdateRegistration>>();
        
        private List<IPermanentUpdate> permanentUpdates = new List<IPermanentUpdate>();
        private List<IEarlyUpdate> earlyUpdates         = new List<IEarlyUpdate>();
        private List<IInputUpdate> inputUpdates         = new List<IInputUpdate>();
        private List<IUpdate> updates                   = new List<IUpdate>(); 
        private List<IDynamicUpdate> dynamicUpdates     = new List<IDynamicUpdate>();
        private List<ILateUpdate> lateUpdates           = new List<ILateUpdate>();
        
        private readonly Stopwatch stopWatch = new Stopwatch();
        private const int maxInitDuration = 100; // in milliseconds, about 0.1s 

        private bool isSuspended = false;
        [SerializeField] private bool suspendOnLoading = true;   

        /// <summary>
        /// Used to disable Unregistering when quitting the game
        /// </summary>
        public static bool IsQuitting { get; private set; } = false;
        #endregion


        #region Private Methods
        /// <summary>
        /// This method exists in order to cancel the unregistering of the behaviours when quitting the application. 
        /// This should be moved in another script in a near future.
        /// </summary>
        [RuntimeInitializeOnLoadMethod]
        private static void OnPreInit()
        {
            Application.quitting += OnQuit; 

            void OnQuit()
            {
                IsQuitting = true; 
            }
        }

        protected override void OnInit()
        {
            base.OnInit();

            CoolSceneManager.OnStartLoading += OnStartSuspension;
            CoolSceneManager.OnStopLoading += OnStopSuspension;

            // Local Methods \\
            void OnStartSuspension() => isSuspended = suspendOnLoading ? true : isSuspended;

            void OnStopSuspension() => isSuspended = suspendOnLoading ? false : isSuspended;
        }

        private void Update()
        {
            stopWatch.Restart();
            // Init Updates
            while (initUpdates.Count > 0 && (stopWatch.ElapsedMilliseconds < maxInitDuration)) // This line is used to avoid bottleneck
            {
                var _pair = initUpdates[0]; 
                var _initObject = _pair.Key;

                _initObject.Init();
                _initObject.HasBeenInitialized = true;

                initUpdates.RemoveAt(0);
                Register<IBaseUpdate>(_initObject, _pair.Value);
            }

            // Updates
            if(isSuspended)
            {
                PermanentUpdate();
                return;
            }

            PermanentUpdate();

            int i; 

            for (i = earlyUpdates.Count; i -- > 0;)
            {
                var _update = earlyUpdates[i];
                CallUpdate(_update.Update, _update);
            }

            for (i = inputUpdates.Count; i-- > 0;)
            {
                var _update = inputUpdates[i];
                CallUpdate(_update.Update, _update);
            }

            for (i = updates.Count; i-- > 0;)
            {
                var _update = updates[i];
                CallUpdate(_update.Update, _update);
            }

            for (i = dynamicUpdates.Count; i-- > 0;)
            {
                var _update = dynamicUpdates[i];
                CallUpdate(_update.Update, _update);
            }

            for (i = lateUpdates.Count; i-- > 0;)
            {
                var _update = lateUpdates[i];
                CallUpdate(_update.Update, _update);
            }

            // Local Methods \\
            void PermanentUpdate()
            {
                for (i = permanentUpdates.Count; i-- > 0;)
                {
                    var _update = permanentUpdates[i];
                    CallUpdate(_update.Update, _update);
                }
            }


            void CallUpdate(Action _action, IBaseUpdate _baseUpdate)
            {
                try {
                    _action(); 
                }
                catch (Exception _exception) {
                    Debug.LogError(_exception.ToString());
                }
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Register the object in the update list according to the Update Registration
        /// </summary>
        /// <typeparam name="T">Type of the Object to register</typeparam>
        /// <param name="_object">Object registers in the update(s)</param>
        /// <param name="_registration">Registration Type</param>
        public void Register<T>(T _object, UpdateRegistration _registration) where T : IBaseUpdate
        {
            // Init Registration
            if(_registration.HasFlag(UpdateRegistration.Init))
            {
                IInitUpdate _initUpdate = _object as IInitUpdate; 
                if(!_initUpdate.HasBeenInitialized)
                {
                    initUpdates.Add(new KeyValuePair<IInitUpdate, UpdateRegistration>(_initUpdate, _registration));
                    return;
                }
            }

            // Updates Registrations
            if (_registration.HasFlag(UpdateRegistration.Permanent))
                permanentUpdates.Add(_object as IPermanentUpdate);

            if (_registration.HasFlag(UpdateRegistration.Early))
                earlyUpdates.Add(_object as IEarlyUpdate);

            if (_registration.HasFlag(UpdateRegistration.Input))
                inputUpdates.Add(_object as IInputUpdate);

            if (_registration.HasFlag(UpdateRegistration.Update))
                updates.Add(_object as IUpdate);

            if (_registration.HasFlag(UpdateRegistration.Dynamic))
                dynamicUpdates.Add(_object as IDynamicUpdate);

            if (_registration.HasFlag(UpdateRegistration.Late))
                lateUpdates.Add(_object as ILateUpdate);

        }

        /// <summary>
        /// Unregister the object from the update list according to the Update Registration
        /// </summary>
        /// <typeparam name="T">Type of the Object to register</typeparam>
        /// <param name="_object">Object registers in the update(s)</param>
        /// <param name="_registration">Registration Type</param>
        public void Unregister<T>(T _object, UpdateRegistration _registration) where T : IBaseUpdate
        {
            // Init Registration
            if(_registration.HasFlag(UpdateRegistration.Init))
            {
                IInitUpdate _initUpdate = _object as IInitUpdate;
                if(!_initUpdate.HasBeenInitialized)
                {
                    initUpdates.Remove(new KeyValuePair<IInitUpdate, UpdateRegistration>(_initUpdate, _registration));
                    return;
                }
            }

            // Updates Registrations
            if (_registration.HasFlag(UpdateRegistration.Permanent))
                permanentUpdates.Remove(_object as IPermanentUpdate);

            if (_registration.HasFlag(UpdateRegistration.Early))
                earlyUpdates.Remove(_object as IEarlyUpdate);

            if (_registration.HasFlag(UpdateRegistration.Input))
                inputUpdates.Remove(_object as IInputUpdate);

            if (_registration.HasFlag(UpdateRegistration.Update))
                updates.Remove(_object as IUpdate);

            if (_registration.HasFlag(UpdateRegistration.Dynamic))
                dynamicUpdates.Remove(_object as IDynamicUpdate);

            if (_registration.HasFlag(UpdateRegistration.Late))
                lateUpdates.Remove(_object as ILateUpdate);
        }
        #endregion 

    }
}
