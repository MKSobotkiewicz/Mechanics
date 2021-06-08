using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace Project.Globe
{
    public class PlanetConditions : MonoBehaviour
    {
        //public float StarMass;
        public float StarTemperature;
        public Color StarColor;
        public Color PlantColor;
        public EStarType StarType;
        public EAtmosphericComposition AtmosphericComposition;

        private Light mainLight;
        private PhysicallyBasedSky physicallyBasedSky;
        private MeshRenderer globe;

        private static readonly Color baselineAirTint = new Color(0.9f, 0.9f, 1f);
        private static readonly Color redAirTint = new Color(1f, 0.9f, 0.9f);
        private static readonly Color yellowAirTint = new Color(1f, 1f, 0.9f);
        private static readonly Color yellowGreenAirTint = new Color(0.65f, 1f, 0.35f);
        private static readonly Color brownAirTint = new Color(0.5f, 0.4f, 0.3f);
        private static readonly string globeMaterialPlantColorName = "Color_063f584e3c224c738862528b5b271d6c";
        private static readonly System.Random random = new System.Random();

        static public PlanetConditions GenerateRandom()
        {
            var go = new GameObject("Planet Conditions");
            var planetConditions = go.AddComponent<PlanetConditions>();
            planetConditions.Setup();
            return planetConditions;
        }

        static public float TemperatureToWavelength(float temperature)
        {
            Debug.Log(2898000 / temperature);
            return 2898000 / temperature;
        }

        private void Setup()
        {
            GetMainLight();
            GetSky();
            GetGlobe();
            //StarMass = 0; 0.001f + (float)(System.Math.Pow(random.NextDouble() * 2.8, 3));
            StarTemperature = /*StarMassToTemperature(StarMass);*/2500 + (float)(System.Math.Pow(random.NextDouble() * 3.45, 8) +random.NextDouble() * 3000);
            StarColor = StarTemperatureToColor(StarTemperature);
            StarType = GetStarType(StarTemperature);
            mainLight.colorTemperature = StarTemperature;
            SetAtmosphericComposition();
            SetColor(AtmosphericComposition);
            PlantColor = WaveLengthToColor(TemperatureToWavelength(StarTemperature) * 0.9f);
            globe.material.SetColor("Color_063f584e3c224c738862528b5b271d6c", PlantColor);
        }

        private void GetMainLight()
        {
            foreach (var go in GameObject.FindGameObjectsWithTag("MainLight"))
            {
                var light = go.GetComponent<Light>();
                if (light != null)
                {
                    mainLight = light;
                    return;
                }
            }
            Debug.LogError("Missing Main Light.");
        }

        private void GetSky()
        {
            foreach (var go in GameObject.FindGameObjectsWithTag("Sky"))
            {
                var sky = go.GetComponent<Volume>();
                if (sky != null)
                {
                    sky.sharedProfile.TryGet(out physicallyBasedSky);
                    return;
                }
            }
            Debug.LogError("Missing Physically Based Sky.");
        }

        private void GetGlobe()
        {
            foreach (var go in GameObject.FindGameObjectsWithTag("Globe"))
            {
                var mr = go.GetComponent<MeshRenderer>();
                if (mr != null)
                {
                    globe = mr;
                    return;
                }
            }
            Debug.LogError("Missing Globe.");
        }

        private float StarMassToTemperature(float mass)
        {
            float a,m;
            if (mass < 0.43)
            {
                m = 0.23f;
                a = 2.3f;
            }
            else if (mass < 2)
            {
                m = 1f;
                a = 4f;
            }
            else if (mass < 55)
            {
                m = 1.4f;
                a = 3.5f;
            }
            else
            {
                m = 32000f;
                a = 1f;
            }
            return m*Mathf.Pow(mass,a);//<-that's luminosity, not temperature yet.
        }

        private void SetColor(EAtmosphericComposition atmosphericComposition)
        {
            switch (atmosphericComposition)
            {
                case EAtmosphericComposition.Breathable:
                    physicallyBasedSky.airTint.SetValue(new ColorParameter(baselineAirTint));
                    break;
                case EAtmosphericComposition.HighCarbonDioxide:
                    physicallyBasedSky.airTint.SetValue(new ColorParameter(baselineAirTint));
                    break;
                case EAtmosphericComposition.HighMethane:
                    physicallyBasedSky.airTint.SetValue(new ColorParameter(redAirTint));
                    break;
                case EAtmosphericComposition.HighNitrogenOxide:
                    physicallyBasedSky.airTint.SetValue(new ColorParameter(brownAirTint));
                    break;
                case EAtmosphericComposition.HighOzone:
                    physicallyBasedSky.airTint.SetValue(new ColorParameter(redAirTint));
                    break;
                case EAtmosphericComposition.HighSulfurGas:
                    physicallyBasedSky.airTint.SetValue(new ColorParameter(redAirTint));
                    break;
                case EAtmosphericComposition.HighChlorine:
                    physicallyBasedSky.airTint.SetValue(new ColorParameter(yellowGreenAirTint));
                    break;
            }
        }

        private void SetAtmosphericComposition()
        {
            var randomValue = random.Next(100);
            if (randomValue > 90)
            {
                AtmosphericComposition = EAtmosphericComposition.HighSulfurGas;
                return;
            }
            if (randomValue > 80)
            {
                AtmosphericComposition = EAtmosphericComposition.HighOzone;
                return;
            }
            if(randomValue > 70)
            {
                AtmosphericComposition = EAtmosphericComposition.HighNitrogenOxide;
                return;
            }
            if (randomValue > 60)
            {
                AtmosphericComposition = EAtmosphericComposition.HighMethane;
                return;
            }
            if (randomValue > 50)
            {
                AtmosphericComposition = EAtmosphericComposition.HighCarbonDioxide;
                return;
            }
            if (randomValue > 40)
            {
                AtmosphericComposition = EAtmosphericComposition.HighChlorine;
                return;
            }
            AtmosphericComposition = EAtmosphericComposition.Breathable;
        }

        private Color StarTemperatureToColor(float temperature)
        {
            temperature /= 100f;
            float red, green, blue;

            if (temperature <= 66)
            {
                red = 255;
            }
            else
            {
                red = temperature - 60;
                red = 329.698727446f * Mathf.Pow(red, -0.1332047592f);
                red = Mathf.Clamp(red, 0, 255);
            }
            if (temperature <= 66)
            {
                green = temperature;
                green = 99.4708025861f * Mathf.Log(green) - 161.1195681661f;
            }
            else
            {
                green = temperature - 60;
                green = 288.1221695283f * Mathf.Pow(red, -0.0755148492f);
            }
            green = Mathf.Clamp(green, 0, 255);

            if (temperature <= 66)
            {
                blue = 255;
            }
            else
            {
                if (temperature <= 19)
                {
                    blue = 0;
                }
                else
                {
                    blue = temperature - 10;
                    blue = 138.5177312231f * Mathf.Log(blue) - 305.0447927307f;
                    blue = Mathf.Clamp(blue, 0, 255);
                }
            }
            return new Color(red, green, blue, 255);
        }

        private EStarType GetStarType(float starTemperature)
        {
            if (starTemperature > 30000)
            {
                return EStarType.O;
            }
            else if (starTemperature > 10000)
            {
                return EStarType.B;
            }
            else if (starTemperature > 7500)
            {
                return EStarType.A;
            }
            else if (starTemperature > 6000)
            {
                return EStarType.F;
            }
            else if (starTemperature > 5200)
            {
                return EStarType.G;
            }
            else if (starTemperature > 3700)
            {
                return EStarType.K;
            }
            else if (starTemperature > 2400)
            {
                return EStarType.M;
            }
            Debug.LogError("Wrong star temperature color.");
            return 0;
        }

        public static Color WaveLengthToColor(float wavelength)
        {
            float gamma = 0.80f;
            float factor;
            float red, green, blue;

            if ((wavelength >= 380) && (wavelength < 440))
            {
                red = -(wavelength - 440) / (440 - 380);
                green = 0.0f;
                blue = 1.0f;
            }
            else if ((wavelength >= 440) && (wavelength < 490))
            {
                red = 0.0f;
                green = (wavelength - 440) / (490 - 440);
                blue = 1.0f;
            }
            else if ((wavelength >= 490) && (wavelength < 510))
            {
                red = 0.0f;
                green = 1.0f;
                blue = -(wavelength - 510) / (510 - 490);
            }
            else if ((wavelength >= 510) && (wavelength < 580))
            {
                red = (wavelength - 510) / (580 - 510);
                green = 1.0f;
                blue = 0.0f;
            }
            else if ((wavelength >= 580) && (wavelength < 645))
            {
                red = 1.0f;
                green = -(wavelength - 645) / (645 - 580);
                blue = 0.0f;
            }
            else if ((wavelength >= 645) && (wavelength < 781))
            {
                red = 1.0f;
                green = 0.0f;
                blue = 0.0f;
            }
            else if ((wavelength >= 781))
            {
                red = 1.0f;
                green = 0.0f;
                blue = 0.0f;
            }
            else
            {
                red = 0.0f;
                green = 0.0f;
                blue = 1.0f;
            }

            /*else
            {
                red = 0.0f;
                green = 0.0f;
                blue = 0.0f;
            }*/

            /*if ((wavelength >= 380) && (wavelength < 420))
            {
                factor = 0.3f + 0.7f * (wavelength - 380) / (420 - 380);
            }
            else if ((wavelength >= 420) && (wavelength < 701))
            {
                factor = 1.0f;
            }
            else if ((wavelength >= 701) && (wavelength < 781))
            {
                factor = 0.3f + 0.7f * (780 - wavelength) / (780 - 700);
            }
            else
            {
                factor = 0.0f;
            }*/
            factor =1f;
            Debug.Log(red+" "+green+" "+blue);
            red = Mathf.Clamp01(Mathf.Pow(red * factor, gamma));
            green = Mathf.Clamp01(Mathf.Pow(green * factor, gamma));
            blue = Mathf.Clamp01(Mathf.Pow(blue * factor, gamma));
            return new Color(red,green,blue);
        }


        public enum EStarType
        {
            O,
            B,
            A,
            F,
            G,
            K,
            M
        }

        public enum EAtmosphericComposition
        {
            Breathable,
            HighCarbonDioxide,
            HighMethane,
            HighOzone,
            HighNitrogenOxide,
            HighSulfurGas,
            HighChlorine
        }
    }
}
