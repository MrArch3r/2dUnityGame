using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class AttachGameObjectsToParticles : MonoBehaviour
{
    public UnityEngine.Rendering.Universal.Light2D m_ParticleLight; // 2D Light prefab to attach to particles

    private ParticleSystem m_ParticleSystem;
    private List<GameObject> m_Instances = new List<GameObject>();
    private ParticleSystem.Particle[] m_Particles;

    // Start is called before the first frame update
    void Start()
    {
        m_ParticleSystem = GetComponent<ParticleSystem>();
        m_Particles = new ParticleSystem.Particle[m_ParticleSystem.main.maxParticles];
    }

    // Update is called once per frame
    void LateUpdate()
    {
        int count = m_ParticleSystem.GetParticles(m_Particles);

        while (m_Instances.Count < count)
        {
            GameObject instance = new GameObject("ParticleInstance");
            m_Instances.Add(instance);

            // Add 2D Light component to the instantiated game object
            if (m_ParticleLight != null)
            {
                UnityEngine.Rendering.Universal.Light2D lightComponent = instance.AddComponent<UnityEngine.Rendering.Universal.Light2D>();
                lightComponent.lightType = m_ParticleLight.lightType;
                lightComponent.color = m_ParticleLight.color;
                lightComponent.intensity = m_ParticleLight.intensity;
                lightComponent.pointLightOuterRadius = m_ParticleLight.pointLightOuterRadius;
                lightComponent.pointLightInnerRadius = m_ParticleLight.pointLightInnerRadius;
                lightComponent.pointLightInnerAngle = m_ParticleLight.pointLightInnerAngle;
                lightComponent.pointLightOuterAngle = m_ParticleLight.pointLightOuterAngle;
                lightComponent.shadowIntensity = m_ParticleLight.shadowIntensity;
                lightComponent.shadowVolumeIntensity = m_ParticleLight.shadowVolumeIntensity;
                lightComponent.lightCookieSprite = m_ParticleLight.lightCookieSprite;
            }
        }

        bool worldSpace = (m_ParticleSystem.main.simulationSpace == ParticleSystemSimulationSpace.World);
        for (int i = 0; i < m_Instances.Count; i++)
        {
            if (i < count)
            {
                if (worldSpace)
                    m_Instances[i].transform.position = m_Particles[i].position;
                else
                    m_Instances[i].transform.localPosition = m_Particles[i].position;

                // Update the position of the attached light
                if (m_ParticleLight != null)
                {
                    m_Instances[i].GetComponent<UnityEngine.Rendering.Universal.Light2D>().transform.position = m_Instances[i].transform.position;
                }

                m_Instances[i].SetActive(true);
            }
            else
            {
                m_Instances[i].SetActive(false);
            }
        }
    }
}