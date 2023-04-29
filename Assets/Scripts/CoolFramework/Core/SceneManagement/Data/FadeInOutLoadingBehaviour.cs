using System.Collections; 
using UnityEngine; 
using UnityEngine.SceneManagement;

namespace CoolFramework.SceneManagement
{
    [CreateAssetMenu(fileName = "Fade In/Out Behaviour", menuName = "Cool FrameWork/Scene Management/Loading Behaviour/Fade In & Out")]
    public class FadeInOutLoadingBehaviour : LoadingBehaviour
    {
        public override IEnumerator OnPreLoading(CoolSceneManager _manager) => null;
        public override IEnumerator OnPreUnloading(CoolSceneManager _manager) => null; 
        public override IEnumerator OnStartLoading(SceneBundle _bundle, LoadSceneMode _mode)
        {
            LoadSceneParameters _params = new LoadSceneParameters()
            {
                loadSceneMode = _mode,
                localPhysicsMode = LocalPhysicsMode.None
            }; 
            yield return new LoadSceneBundleAsyncOperation(_bundle, _params);
        }
        public override IEnumerator OnStartUnloading(SceneBundle _bundle, UnloadSceneOptions _options = UnloadSceneOptions.None)
        {
            yield return new UnloadSceneBundleAsyncOperation(_bundle, _options);
        }
        public override void OnStopLoading(){}
        public override void OnStopUnloading(){}
        public override IEnumerator OnPostLoading(CoolSceneManager _manager) => null; 
        public override IEnumerator OnPostUnloading(CoolSceneManager _manager) => null;
    }
}
