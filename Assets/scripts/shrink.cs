using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shrink : MonoBehaviour
{
    public void dest()
    {
        game_manager manager = GameObject.Find("scene_manager").GetComponent<game_manager>();
        manager.open_menu(false);
        Destroy(gameObject.transform.parent.gameObject);
    }
}
