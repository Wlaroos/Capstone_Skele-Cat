using UnityEngine.SceneManagement;
using UnityEngine;

public class WinZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {

        PlayerController playerController = other.GetComponent<PlayerController>();
        
        if (playerController != null && playerController.IsAlive)
        {
            Debug.Log("Level Complete");
            playerController.NextLevel();
            Invoke(nameof(NextScene), 1f);
            //AudioHelper.PlayClip2D(_winSFX01, 1f);
        }
    }
    
    public void NextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
