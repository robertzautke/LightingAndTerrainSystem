using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class EnvironmentLight : MonoBehaviour
{

    private FatherTime ft;


    // Options to set the light properties at different GameTimes
    [System.Serializable]
    public class EnvironmentLightOptions
    {
        [Range(0, 86400)]
        public float time = 0;

        [Range(0, 10000)]
        public float range = 100;
        [Range(0, 8)]
        public float intensity = 0;
        public Color color = Color.white;
    }

    public List<EnvironmentLightOptions> ListOfLightChanges;

    void Start()
    {
        ft = GameObject.Find("FatherTime").GetComponent<FatherTime>();
    }

    // Update to the current EnvironmentLightOption based on the time
    // settings of the EnvironmentLightOptions.
    void Update()
    {
        if (ft == null)
            return;

        EnvironmentLightOptions currentELO = new EnvironmentLightOptions();

        foreach (EnvironmentLightOptions ELO in ListOfLightChanges)
        {
            if (ft.GameTime > ELO.time && currentELO.time <= ELO.time)
            {
                this.GetComponent<Light>().range = ELO.range;
                this.GetComponent<Light>().intensity = ELO.intensity;
                this.GetComponent<Light>().color = ELO.color;
                currentELO = ELO;
            }

            if (ft.GameTime < currentELO.time)
            {
                currentELO = null;
            }
        }
    }
}

