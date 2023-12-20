using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationObject : MonoBehaviour
{
    public float interval = 0.1f;

    void Start()
    {
        StartCoroutine(ApplyRotationCoroutine());
    }

    IEnumerator ApplyRotationCoroutine()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("Assets/20231217114359_rotation.txt");
        if (textAsset == null)
        {
            Debug.LogError("Text asset not found.");
            yield break;
        }

        string[] lines = textAsset.text.Split('\n');
        foreach (var line in lines)
        {
            string[] values = line.Split(',');
            if (values.Length >= 4)
            {
                float x = float.Parse(values[0]);
                float y = float.Parse(values[1]);
                float z = float.Parse(values[2]);
                float w = float.Parse(values[3]);
                Quaternion rotation = new Quaternion(x, y, z, w);

                transform.rotation = rotation;
                yield return new WaitForSeconds(interval);
            }
        }
    }
}

