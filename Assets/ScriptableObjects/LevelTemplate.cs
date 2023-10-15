using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "New Level", menuName = "Level Scriptable Object")]
public class LevelTemplate : ScriptableObject
{ 
    [SerializeField] private string _levelNumber;
    [SerializeField] private string _levelName;
    [SerializeField] private Scene _levelScene;
    private int _levelIndex;
}
