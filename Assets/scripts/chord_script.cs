using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chord_script : MonoBehaviour
{
    public static float chord_dest_time = 3;
    public GameObject brown_circle, Particules;
    public bool destroy_byself = true;
    public Animator animator;
    private Vector3 scaleChange = new Vector3(0.02f, 0.02f, 02f);

    private void Awake()
    {
        StartCoroutine(destroy(chord_dest_time, false, false));
    }
    public IEnumerator destroy(float d_time, bool spawn_particules, bool keepgrowing)
    {
        yield return new WaitForSeconds(d_time);
        if (spawn_particules)
        {
            /*
            if (!destroy_byself)  //if it is the first dot
            {
                //then start the game
                game_manager.start_game = true;
                GameObject.Find(game_manager.current_text.name).GetComponent<Animator>().SetBool("end", true); //destroy the text with after the animation
            }
            else
            {
                game_manager.destroyed_dots += 1; //if it is not the first dot increase the destoyed dot number
            }*/
            //make the brow circle bigger
            //if the object destroyed by the player

            while(game_manager.can_grow && brown_circle.transform.localScale.x < 1f)
            {
                brown_circle.transform.localScale += scaleChange;
                yield return new WaitForSeconds(0.02f);
            }
            if(brown_circle.transform.localScale.x > 1f)
            {
                //if the brown part is big enough
                if (!destroy_byself)
                {
                    game_manager.start_game = true;
                    GameObject.Find(game_manager.current_text.name).GetComponent<Animator>().SetBool("end", true); //destroy the text with after the animation
                }
                else
                {
                    game_manager.destroyed_dots += 1; //if it is not the first dot increase the destoyed dot number
                }
                GameObject @object = Instantiate(Particules, gameObject.transform.position, Quaternion.identity);
                @object.transform.localScale = gameObject.transform.localScale;
                Destroy(gameObject);
            }

        }
        else
        {
            if (destroy_byself)
            {
                //if it's destroyed by time
                animator.SetBool("death", true);
            }
        }

        //Destroy(gameObject);
    }
}
