using UnityEngine;
using UnityEngine.SceneManagement;

/*
Cheap method to ensure that manager scene is already loaded.
This will try to loadGameData the manager scene anyways even if the scene is loaded
e.g after changing levels
As long as the root objects in the manager scene are singletons(which they should be),
having the manager scene already loaded is not a problem
In general, ensure that everything in the manager scene goes to don'tdestroyonload
and that they are singletons
*/
namespace Bdeshi.Helpers.levelloading
{
    public class ManagerLoadEnsurer : MonoBehaviour
    {
        public static bool LoadedManager = false;
        public string ManagerSceneName = "ManagerScene";
        private void Awake()
        {
            EnsureLoad();
        }

        public void EnsureLoad()
        {
            if (!LoadedManager)
            {
                LoadedManager = true;
                SceneManager.LoadScene(ManagerSceneName, LoadSceneMode.Additive);
            }
        }
    }
}
