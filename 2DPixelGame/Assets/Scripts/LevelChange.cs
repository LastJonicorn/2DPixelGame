using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChange : MonoBehaviour
{
    public int sceneIndex;

    private void OnTriggerEnter2D(Collider2D player)
    {
        if(player.tag == "Player")
        {
            SceneManager.LoadScene(sceneIndex);
        }
    }
}
