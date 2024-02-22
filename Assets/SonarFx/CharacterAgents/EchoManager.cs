using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EchoManager : MonoBehaviour
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
    bool _sonarOn = false;
    public bool sonarOn { get { return _sonarOn; } set { _sonarOn = value; } }

    bool _FOODFound = false;

    float currentTime = 0;
    //float currentFOODTime = 0;
    [SerializeField] float maxTime = 4;
    //[SerializeField] float maxFOODTime = 10;

    // Past Sonar Counter
    public int pastSonarCounter;

    // Player transform
    [SerializeField] Transform player;

    // SonarFx object
    [SerializeField] SonarFx sonar;

    // Objects in the scene
    [SerializeField] GameObject[] tools;
    [SerializeField] GameObject[] environment;
    [SerializeField] GameObject[] danger;
    [SerializeField] GameObject[] FOOD;

    //Materials in Scene
    //[SerializeField] Material[] toolMaterialArray;
    //[SerializeField] Material[] enviroMaterialArray;
    //[SerializeField] Material[] dangerMaterialArray;
    //[SerializeField] Material[] FOODMaterialArray;

    // Colors of Materials in Scene
    [SerializeField] Color[] toolMaterialColors;
    [SerializeField] Color[] FOODMaterialColors;
    [SerializeField] Color[] dangerMaterialColors;
    [SerializeField] Color[] enviroMaterialColors;


    // Sonar Radius
    [SerializeField] int localRadius;
    int rangedRadius;
    bool ranged = true;

    //Debug
    //[SerializeField] int originArraySize;

    private void Awake()
    {

        pastSonarCounter = 0;

        tools = GameObject.FindGameObjectsWithTag("Tool");
        environment = GameObject.FindGameObjectsWithTag("Environment");
        danger = GameObject.FindGameObjectsWithTag("Danger");
        FOOD = GameObject.FindGameObjectsWithTag("FOOD");

        Debug.Log("Tools: " + tools);
        Debug.Log("Environment: " + environment);
        Debug.Log("Danger: " + danger);
        Debug.Log("FOOD: " + FOOD);

        toolMaterialColors = new Color[tools.Length];
        FOODMaterialColors = new Color[FOOD.Length];
        dangerMaterialColors = new Color[danger.Length];
        enviroMaterialColors = new Color[environment.Length];

        rangedRadius = localRadius*2 / 3;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !_sonarOn)
        {
            sonar.originArray[0] = new Vector4(
                player.position.x, player.position.y, player.position.z, 1);

            pastSonarCounter += 1;

            ranged = false;

            Echo();
        }

        if (pastSonarCounter >= sonar.originArray.Length)
        {
            pastSonarCounter = sonar.originArray.Length - 1;
        }


        if (Input.GetKeyDown(KeyCode.X) && !_sonarOn)
        {
            for (int i = 1; i < pastSonarCounter + 1; i++)
            {
                sonar.originArray[i].w = 1;

                Echo();
            }
        }

        //originArraySize = sonar.GetOriginArraySize();
    }

    private void FixedUpdate()
    {
        if (_sonarOn)
        {
            for (int i = 0; i < tools.Length; i++)
            {
                tools[i].GetComponent<Renderer>().material.SetColor
                        ("_SonarWaveColor", toolMaterialColors[i] * (currentTime / maxTime));
            }

            //for (int i = 0; i < FOOD.Length; i++)
            //{
            //    FOOD[i].GetComponent<Renderer>().material.SetColor
            //            ("_SonarWaveColor", FOODMaterialColors[i] * (currentTime / maxTime));
            //}

            for (int i = 0; i < danger.Length; i++)
            {
                danger[i].GetComponent<Renderer>().material.SetColor
                        ("_SonarWaveColor", dangerMaterialColors[i] * (currentTime / maxTime));
            }

            for (int i = 0; i < environment.Length; i++)
            {
                environment[i].GetComponent<Renderer>().material.SetColor
                        ("_SonarWaveColor", enviroMaterialColors[i] * (currentTime / maxTime));
            }

            currentTime -= Time.deltaTime;

            if(currentTime <= 0)
            {
                _sonarOn = false;

                Vector4 carry = Vector4.zero; // Store previous vector in the array for appending
                bool appending = false;

                for (int i = 0; i < sonar.originArray.Length; i++)
                {
                    if (sonar.originArray[i].w == 1)
                    {
                        sonar.originArray[i].w = 0;

                        if (i == 0)
                        {
                            appending = true; // New sonar was created. Append it to the array
                        }
                    }

                    if (appending)
                    {
                        Vector4 temp = sonar.originArray[i];
                        sonar.originArray[i] = carry;

                        carry = temp;
                    }
                }

                //Debug.Log(sonar.originArray);

                for (int i = 0; i < tools.Length; i++)
                {
                    tools[i].GetComponent<Renderer>().material.SetColor
                            ("_SonarWaveColor", Color.black);

                    tools[i].GetComponent<Renderer>().material.SetVectorArray(
                        "_SonarWaveVectorArray", sonar.originArray);

                    tools[i].GetComponent<Renderer>().material.SetInt
                                ("_SonarArraySize", sonar.GetOriginArraySize());
                }


                for (int i = 0; i < FOOD.Length; i++)
                {
                    FOOD[i].GetComponent<Renderer>().material.SetColor
                            ("_SonarWaveColor", Color.black);

                    FOOD[i].GetComponent<Renderer>().material.SetVectorArray(
                        "_SonarWaveVectorArray", sonar.originArray);

                    FOOD[i].GetComponent<Renderer>().material.SetInt
                                ("_SonarArraySize", sonar.GetOriginArraySize());
                }

                for (int i = 0; i < danger.Length; i++)
                {
                    danger[i].GetComponent<Renderer>().material.SetColor
                            ("_SonarWaveColor", Color.black);

                    danger[i].GetComponent<Renderer>().material.SetVectorArray(
                        "_SonarWaveVectorArray", sonar.originArray);

                    danger[i].GetComponent<Renderer>().material.SetInt
                                ("_SonarArraySize", sonar.GetOriginArraySize());
                }

                for (int i = 0; i < environment.Length; i++)
                {
                    environment[i].GetComponent<Renderer>().material.SetColor
                            ("_SonarWaveColor", Color.black);

                    environment[i].GetComponent<Renderer>().material.SetVectorArray(
                        "_SonarWaveVectorArray", sonar.originArray);

                    environment[i].GetComponent<Renderer>().material.SetInt
                                ("_SonarArraySize", sonar.GetOriginArraySize());
                }
                

                //Debug.Log("Stop Origin Array Size: " + sonar.GetOriginArraySize());
            }
        }
    }

    public void Echo()
    {
        int thisRadius;

        if (ranged)
        {
            thisRadius = rangedRadius;

            //Debug.Log("Ranged: " + rangedRadius);
        }

        else
        {
            thisRadius = localRadius;
            ranged = true;

            //Debug.Log("Local: " + localRadius) ;
        }

        if (FOOD.Length > 0)
        {
            for (int i = 0; i < FOOD.Length; i++)
            {
                FOOD[i].GetComponent<Renderer>().material.SetColor("_SonarWaveColor", _foodColor);

                FOOD[i].GetComponent<Renderer>().material.SetVectorArray(
                        "_SonarWaveVectorArray", sonar.originArray);

                FOOD[i].GetComponent<Renderer>().material.SetInt
                                ("_SonarArraySize", sonar.GetOriginArraySize());

                FOOD[i].GetComponent<Renderer>().material.SetInt
                                ("_Radius", thisRadius);

                FOODMaterialColors[i] = FOOD[i].GetComponent<Renderer>().material.
                    GetColor("_SonarWaveColor");

            }

        }

        if (tools.Length > 0)
        {
            for (int i = 0; i < tools.Length; i++)
            {
                tools[i].GetComponent<Renderer>().material.SetColor(
                    "_SonarWaveColor", _toolColor);

                tools[i].GetComponent<Renderer>().material.SetVectorArray(
                        "_SonarWaveVectorArray", sonar.originArray);

                tools[i].GetComponent<Renderer>().material.SetInt
                                ("_SonarArraySize", sonar.GetOriginArraySize());

                tools[i].GetComponent<Renderer>().material.SetInt
                                ("_Radius", thisRadius);

                toolMaterialColors[i] = tools[i].GetComponent<Renderer>().material.
                    GetColor("_SonarWaveColor");
            }
        }

        if (danger.Length > 0)
        {
            for (int i = 0; i < danger.Length; i++)
            {
                danger[i].GetComponent<Renderer>().material.SetColor
                    ("_SonarWaveColor", _dangerColor);

                danger[i].GetComponent<Renderer>().material.SetVectorArray(
                        "_SonarWaveVectorArray", sonar.originArray);

                danger[i].GetComponent<Renderer>().material.SetInt
                                ("_SonarArraySize", sonar.GetOriginArraySize());

                danger[i].GetComponent<Renderer>().material.SetInt
                                ("_Radius", thisRadius);

                dangerMaterialColors[i] = danger[i].GetComponent<Renderer>().material.
                    GetColor("_SonarWaveColor");
            }  
        }

        if(environment.Length > 0)
        {
            for (int i = 0; i < environment.Length; i++)
            {
                environment[i].GetComponent<Renderer>().material.SetColor(
                    "_SonarWaveColor", _environmentColor);

                environment[i].GetComponent<Renderer>().material.SetVectorArray(
                    "_SonarWaveVectorArray", sonar.originArray);

                environment[i].GetComponent<Renderer>().material.SetInt
                    ("_SonarArraySize", sonar.GetOriginArraySize());

                environment[i].GetComponent<Renderer>().material.SetInt
                                ("_Radius", thisRadius);

                enviroMaterialColors[i] = environment[i].GetComponent<Renderer>().material.
                    GetColor("_SonarWaveColor");
            }
        }

        //Debug.Log("Echo Origin Array Size: " + sonar.GetOriginArraySize());
        Debug.Log("Past Echoes: " + pastSonarCounter);

        _sonarOn = true;
        currentTime = maxTime;


    }
}
