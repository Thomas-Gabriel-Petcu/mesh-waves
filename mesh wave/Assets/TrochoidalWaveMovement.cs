using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrochoidalWaveMovement : MonoBehaviour
{
    public GameObject point;
    public int numberOfPoints;
    public int xSpacing;
    public float amplitude;
    public float speed;
    public float waveLength;

    private GameObject[] points;
    private float currentXPos = 0;
    private float k;
    private float f;
    // Start is called before the first frame update
    void Start()
    {
        points = new GameObject[numberOfPoints];
        for (int i = 0; i < numberOfPoints; i++)
        {
            float rand = Random.Range(0.3f, 2.0f);
            points[i] = Instantiate(point, new Vector3(currentXPos, transform.position.y, transform.position.z), Quaternion.identity);
            currentXPos += (xSpacing + rand);
        }
        //Time.timeScale = 0.1f;
    }

    // Update is called once per frame
    private void Update()
    {
        UpdatePosition(Time.timeSinceLevelLoad);
    }
    
    private void UpdatePosition(float time)
    {
        for (int i = 0; i < numberOfPoints; i++)
        {
            Vector3 l_position;
            l_position = points[i].transform.position;

            k = 2 * Mathf.PI / waveLength;
            f = k * (l_position.x-speed * time);

            l_position.y = amplitude * Mathf.Sin(f);
            l_position.x = amplitude * Mathf.Cos(f);
            //l_position.x = (Mathf.Cos(time * speed) + i);
            points[i].transform.position = l_position;
        }
    }
}
