using UnityEngine;

namespace ExampleProject.Gameplay.Scenes
{
    [CreateAssetMenu(fileName = "SceneData", menuName = "ScriptableObjects/Scene/SceneData")]
    public class SceneData : ScriptableObject
    {
        #region Fields

        public SceneId id;
        public string sceneName;

        #endregion

        #region Properties


        #endregion
    }

    public enum SceneId
    {
        None = 0,
        Gameplay = 3,
        EmptyScene = 4,
    }
}
