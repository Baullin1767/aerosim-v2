using UnityEditor;
using UnityEditor.SceneManagement;

namespace Editor
{
    public class SceneOpenEditor
    {
        [MenuItem("Tools/Scenes/Intro %#&1", false, 0)]
        private static void OpenEntryScene()
        {
            OpenScene("Intro");
        }
        private static void OpenScene(string sceneName)
        {
            EditorSceneManager.OpenScene($"Assets/Scenes/{sceneName}.unity");
        }
        
        [MenuItem("Tools/Scenes/Mission %#&2", false, 0)]
        private static void OpenMissionScene()
        {
            OpenScene("Mission");
        }
        
        [MenuItem("Tools/Scenes/MissionTutorial %#&3", false, 0)]
        private static void OpenMissionTutorialScene()
        {
            OpenScene("MissionTutorial");
        }
        
    }
}
