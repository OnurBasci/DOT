using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class game_manager : MonoBehaviour
{
    public static bool start_game = false;
    public GameObject dot, chord_dot; //object to initiate
    public Vector2 max_min_generate_speed = new Vector2(0.5f, 2f); //how many dot for seconds
    private float generate_speed, chord_generate_speed;
    public static int dot_number = 1; //how many dots to generate and also the level
    private Vector3 pos, scale = Vector3.zero;
    public static int destroyed_dots, created_dots = 0;  //increased in dot script
    private float current_time, current_time2 = 0;
    public GameObject dot_text, first_ball,first_chord_dot ,canvas, level_text, reset_button;
    public static GameObject current_text;
    public Camera_shake camera_Shake;
    public Camera cam;
    public static bool can_increase, can_grow = false;
    private Sound s;
    bool touchin_chord = false;
    bool play_one_time = true;
    private Vector3 borders = Vector3.zero;
    private void Awake()
    {
        initilize_text_pos();
        check_borders();
        //take the save data
        dot_number = PlayerPrefs.GetInt("high score", 1);

        current_text = dot_text;
        generate_speed = choose_generate_speed(max_min_generate_speed);
        chord_generate_speed = choose_generate_speed(new Vector2(3.0f, 4.0f));
    }
    public void Update()
    {
        Touch_control();
        update_text();
        //if the game is started
        if(start_game)  //game is started if the first dot is touched. see dot_script
        {
            current_time += Time.deltaTime;
            reset_button.SetActive(false);
            if (current_time > generate_speed && created_dots < dot_number)
            {
                generate_dots();
                generate_speed = choose_generate_speed(max_min_generate_speed);
                current_time = 0;
            }
            if(dot_number >= 5)
            {
                current_time2 += Time.deltaTime;
                if (current_time2 > chord_generate_speed && created_dots < dot_number)
                {
                    generate_chord();
                    chord_generate_speed = choose_generate_speed(new Vector2(3.0f, 4.0f));
                    current_time2 = 0;
                }
            }
            if(destroyed_dots == dot_number)
            {
                open_menu(true);
            }
            if(dot_number >= 1)
            {
                increase_difficulties();
            }
        }
    }
    private void generate_dots()
    {
        //choose a random scale
        scale.x = Random.Range(0.5f, 1.5f);
        scale.y = scale.x;
        //choose a random position
        pos.x = Random.Range(-borders.x + scale.x/2, borders.x - scale.x/2); //-3 and 3 is the border so for not to get over the screen
        pos.y = Random.Range(-borders.y + scale.y/2, borders.y - scale.y/2);
        //generate basic dot
        GameObject @object = Instantiate(dot, pos, Quaternion.identity);
        @object.transform.localScale = scale;
        created_dots += 1;
    }
    private void generate_chord()
    {
        Vector3 pos2 = Vector3.zero;
        pos2.x = Random.Range(-borders.x + 0.5f, borders.x - 0.5f);
        pos2.y = Random.Range(-borders.y + 0.5f, borders.y - 0.5f);

        GameObject @object = Instantiate(chord_dot, pos2, Quaternion.identity);
    }
    private float choose_generate_speed(Vector2 vec)
    {
        float t;
        t = Random.Range(vec.x, vec.y);
        return t;
    }
    private void initilize_text_pos()
    {
        float height = Screen.height;
        level_text.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, height/2 - 0.1f * (height/2));
        dot_text.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, height / 2 - 0.6f * (height / 2));
        reset_button.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -(height / 2) + 0.1f * (height / 2));
    }
    private void check_borders()
    {
        borders = cam.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
    }
    public void open_menu(bool successfuly)
    {
        //destroy other existing dots
        GameObject[] other_dots = GameObject.FindGameObjectsWithTag("dot");
        foreach (GameObject @object in other_dots)
        {
            Destroy(@object);
        }
        //succesfully is to pass the next level
        current_text = Instantiate(dot_text, dot_text.transform.position, Quaternion.identity, canvas.transform); //create the dot text
        reset_button.SetActive(true);
        if(dot_number == 4)
        {
            Instantiate(first_chord_dot, first_chord_dot.transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(first_ball, first_ball.transform.position, Quaternion.identity);
        }
        destroyed_dots = 0;
        created_dots = 0;
        start_game = false;
        if(successfuly)
        {
            dot_number += 1;
            //lets save the date
            PlayerPrefs.SetInt("high score", dot_number);
            can_increase = true;  //when the level is sucessfuly passed can increase the difficulties
        }
        else
        {
            //camera shake
            StartCoroutine(camera_Shake.Shake(0.2f, 0.2f));
            //destroy other existing dots
            /*GameObject[] other_dots = GameObject.FindGameObjectsWithTag("dot");
            foreach(GameObject @object in other_dots)
            {
                Destroy(@object);
            }*/

        }
    }
    private void update_text()
    {
        TextMeshProUGUI txt = level_text.GetComponent<TextMeshProUGUI>();
        txt.text = (dot_number - destroyed_dots).ToString();
    }
    private void increase_difficulties()
    {
        if(can_increase && dot_number % 5 == 0)
        {
            max_min_generate_speed.x = max_min_generate_speed.x * 9/10;
            max_min_generate_speed.y = max_min_generate_speed.y * 9/10;
            can_increase = false;
        }
    }
    public Sound choose_random_sound()
    {
        Sound[] sounds = GameObject.Find("Audio manager").GetComponent<Audio_manager>().sounds;
        int rand = Random.Range(8, sounds.Length);
        return sounds[rand];
    }
    private void Touch_control()
    {
        foreach(Touch touch in Input.touches)
        {
            if(touch.phase == TouchPhase.Began)
            {
                Vector3 touch_point = Camera.main.ScreenToWorldPoint(touch.position);

                if(Physics2D.OverlapPoint(touch_point) != null)
                {
                    Collider2D col = Physics2D.OverlapPoint(touch_point);
                    //check if the dot component exist
                    if(col.GetComponent<dot_script>() != null)
                    {
                        StartCoroutine(col.GetComponent<dot_script>().destroy(0, true));
                    }
                    if (col.GetComponent<chord_script>() != null)
                    {
                        touchin_chord = true;
                        StartCoroutine(col.GetComponent<chord_script>().destroy(0, true, true));
                    }
                }
            }
        }
        if(Input.touches.Length > 0)
        {
            if (Input.touches[0].phase == TouchPhase.Stationary)
            {
                can_grow = true;
                //play sound
                if(touchin_chord && play_one_time)
                {
                    s = choose_random_sound();
                    FindObjectOfType<Audio_manager>().Play(s.name);
                    play_one_time = false;
                }
            }
            if (Input.touches[0].phase == TouchPhase.Ended && can_grow)
            {
                can_grow = false;
                if(s != null && touchin_chord)
                {
                    FindObjectOfType<Audio_manager>().Stop(s.name);
                    touchin_chord = false;
                    play_one_time = true;
                }
            }
        }
    }
}
