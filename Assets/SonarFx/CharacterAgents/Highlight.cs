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

    // Sonar Timer
    bool sonarOn = false;
    float currentTime = 0;
    [SerializeField] float maxTime = 4;

    // Past Sonar Counter
    int pastSonarCounter;

    // Player transform
    [SerializeField] Transform player;

    // SonarFx object
    [SerializeField] SonarFx sonar;



    private void Awake()
    {
        this.GetComponent<Renderer>().material.SetColor("_SonarWaveColor", Color.black);

        pastSonarCounter = 0;
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
                player.position.x, player.position.y, player.position.z, 1);

            pastSonarCounter += 1;

            if(pastSonarCounter >= sonar.originArray.Length)
            {
                pastSonarCounter = sonar.originArray.Length - 1;
            }

            Echo();
        }

        if (Input.GetKeyDown(KeyCode.X) && !sonarOn)
        {
            for (int i = 1; i < pastSonarCounter + 1; i++)
            {
                sonar.originArray[i].w = 1;

                Echo();
            }
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

                Vector4 carry = Vector4.zero; // Store previous vector in the array for appending
                bool appending = false;

                for (int i = 0; i < sonar.originArray.Length; i++)
                {
                    if (sonar.originArray[i].w == 1)
                    {
                        sonar.originArray[i].w = 0;

                        if (i == 0)
                        {
                            appending = true;
                        }
                    }

                    if (appending)
                    {
                        Vector4 temp = sonar.originArray[i];
                        sonar.originArray[i] = carry;

                        carry = temp;
                    }
                }

                Debug.Log(sonar.originArray);

                this.GetComponent<Renderer>().material.SetColor
                ("_SonarWaveColor", Color.black);

                this.GetComponent<Renderer>().material.SetVectorArray(
                "_SonarWaveVectorArray", sonar.originArray);

                this.GetComponent<Renderer>().material.SetInt
                            ("_SonarArraySize", sonar.GetOriginArraySize());

                //Debug.Log("Stop Origin Array Size: " + sonar.GetOriginArraySize());
            }
        }
    }

    private void Echo()
    {
        this.GetComponent<Renderer>().material.SetVectorArray(
                "_SonarWaveVectorArray", sonar.originArray);

        this.GetComponent<Renderer>().material.SetInt
                    ("_SonarArraySize", sonar.GetOriginArraySize());

        Debug.Log("Echo Origin Array Size: " + sonar.GetOriginArraySize());

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
