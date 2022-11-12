using UnityEngine;

public class reset : MonoBehaviour
{
    public void Reset()
    {
        PlayerPrefs.SetInt("high score", 1);
        game_manager.dot_number = 1;
    }
}
