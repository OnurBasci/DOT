using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_shake : MonoBehaviour
{
    public IEnumerator Shake(float duration,  float magnitude)
    {
        Vector3 original_pos = transform.position;
        float elapsed = 0.0f;
        while(elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            float rotate_z = Random.Range(-5f, 5f);

            transform.localPosition = new Vector3(x, y, original_pos.z);
            transform.eulerAngles = new Vector3(
                transform.eulerAngles.x,
                transform.eulerAngles.y,
                rotate_z
            );

            elapsed += Time.deltaTime;

            yield return null;
        }
        transform.localPosition = original_pos;
        transform.eulerAngles = Vector3.zero;
    }
}
