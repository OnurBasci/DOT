using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dot_script : MonoBehaviour
{
    public static float destroy_time = 2f;
    public GameObject Particules;
    public bool destroy_byself = true;
    public Animator animator;

    private void Awake()
    {
        if(game_manager.can_increase && game_manager.dot_number % 7 == 0)
        {
            //increase the death tima
            destroy_time = destroy_time * 9 / 10;
            game_manager.can_increase = false;
        }
        StartCoroutine(destroy(destroy_time, false));
    }
    public IEnumerator destroy(float d_time, bool spawn_particules)
    {
        yield return new WaitForSeconds(d_time);
        if(spawn_particules)
        {
            if (!destroy_byself)  //if it is the first dot
            {
                //then start the game
                game_manager.start_game = true;
                GameObject.Find(game_manager.current_text.name).GetComponent<Animator>().SetBool("end", true); //destroy the text with after the animation
            }
            else
            {
                game_manager.destroyed_dots += 1; //if it is not the first dot increase the destoyed dot number
                Sound s = choose_random_sound();
                FindObjectOfType<Audio_manager>().Play(s.name);
            }
            //if the object destroyed by the player
            GameObject @object = Instantiate(Particules, gameObject.transform.position, Quaternion.identity);
            @object.transform.localScale = gameObject.transform.localScale;
            Destroy(gameObject);
        }
        else
        {
            if(destroy_byself)
            {
                //if it's destroyed by time
                animator.SetBool("death", true);
            }
        }

        //Destroy(gameObject);
    }
    public Sound choose_random_sound()
    {
        Sound[] sounds = GameObject.Find("Audio manager").GetComponent<Audio_manager>().sounds;
        int rand = Random.Range(0, 7); //7 for the number of notes
        return sounds[rand];
    }
}
