using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlight : MonoBehaviour
{
    // Reference to the shader.
    [SerializeField] Shader shader;

    // Highlight color
    //Color _highlightColor;

    // FOOD color
    [SerializeField] Color _foodColor;

    // Danger Color
    [SerializeField] Color _dangerColor;

    //Tool Color
    [SerializeField] Color _toolColor;

    // Environment Color
    [SerializeField] Color _environmentColor;

    // Starting Wave Color
    Color _startingWaveColor;

    // Is the Object tagged
    int tagged = 1;

    // Sonar Timers
    bool sonarOn = false;
    float currentTime = 0;
    [SerializeField] float maxTime = 4;

    // Player transform
    [SerializeField] Transform player;

    [SerializeField] SonarFx sonar;



    private void Awake()
    {
        this.GetComponent<Renderer>().material.SetColor("_SonarWaveColor", Color.black);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !sonarOn)
        {
            sonar.originArray[0] = new Vector4(
                player.position.x, player.position.y, player.position.z);

            this.GetComponent<Renderer>().material.SetVectorArray(
                "_SonarWaveVectorArray", sonar.originArray);

            if (transform.gameObject.tag == "FOOD")
            {
                this.GetComponent<Renderer>().material.SetColor("_SonarWaveColor", _foodColor);
            }

            else if (transform.gameObject.tag == "Tool")
            {
                this.GetComponent<Renderer>().material.SetColor("_SonarWaveColor", _toolColor);
            }

            else if (transform.gameObject.tag == "Danger")
            {
                this.GetComponent<Renderer>().material.SetColor("_SonarWaveColor", _dangerColor);
            }

            else
            {
                this.GetComponent<Renderer>().material.SetColor("_SonarWaveColor", _environmentColor);
            }

            sonarOn = true;
            currentTime = maxTime;

            _startingWaveColor = this.GetComponent<Renderer>().material.GetColor("_SonarWaveColor");
        }
    }

    private void FixedUpdate()
    {
        if (sonarOn)
        {
            this.GetComponent<Renderer>().material.SetColor
                ("_SonarWaveColor", _startingWaveColor * (currentTime/maxTime));

            currentTime -= Time.deltaTime;

            if(currentTime <= 0)
            {
                sonarOn = false;

                this.GetComponent<Renderer>().material.SetColor
                ("_SonarWaveColor", Color.black);
            }
        }
    }
}
