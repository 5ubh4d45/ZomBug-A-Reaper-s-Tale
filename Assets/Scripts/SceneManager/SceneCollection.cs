using UnityEngine;

namespace Game.Scenes
{
    [CreateAssetMenu(fileName = "New Scene Collection", menuName = "Scenes/Scene Collection", order = 0)]
    public class SceneCollection : ScriptableObject
    {
        public SceneReference[] SceneReferences;

        public int ActiveSceneIndex;
    }
}