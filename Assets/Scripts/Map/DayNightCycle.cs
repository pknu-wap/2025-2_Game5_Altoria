using Common;
using UnityEngine;

public enum ETimeType
{
    Morning,
    Day,
    Sunset,
    Night,
}

public class DayNightCycle : MonoBehaviour
{
    [Header("References")]
    Light sunLight;
    Material skyboxMat;

    [Header("Time")]
    [Range(0, 24)] float time;
    float fullDayLength = 1200f;
    float timeSpeed;

    [Header("Sky Materials")]
    Material morningMat;
    Material dayMat;
    Material sunsetMat;
    Material nightMat;

    float latitude = 37f;   // 대한민국 위도(서울기준)
    float declination = 0f; // 태양 적위(봄 기준)

    void Start()
    {
        skyboxMat = RenderSettings.skybox;

        morningMat = Manager.Resource.Load<Material>("MorningMat");
        dayMat = Manager.Resource.Load<Material>("DayMat");
        sunsetMat = Manager.Resource.Load<Material>("SunsetMat");
        nightMat = Manager.Resource.Load<Material>("NightMat");

        // TODO: 세이브 파일에서 시간 데이터 가져와서 설정하기
        time = 7f;
        GameSystem.Instance.timeType = ETimeType.Day;
        timeSpeed = 24f / fullDayLength;
    }

    void Update()
    {
        if (skyboxMat == null) return;

        if (sunLight == null)
        {
            var lightGO = GameObject.Find("Directional Light");
            if (lightGO != null)
                sunLight = lightGO.GetComponent<Light>();
        }
        else
        {
            time += Time.deltaTime * timeSpeed; // TODO: 추후 Start로 옮길 것
            if (time >= 24f) time -= 24f;

            UpdateSunPosition();
            UpdateSkybox(time);
        }
    }

    void UpdateSunPosition()
    {
        float maxAltitude = 90f - Mathf.Abs(latitude - declination);                    // 최대 고도 53°
        float solarAngle = Mathf.Sin((time - 6f) / 12f * Mathf.PI) * maxAltitude;  // 태양 고도
        float azimuth = (time / 24f) * 360f;                                       // 태양 방위각
        sunLight.transform.rotation = Quaternion.Euler(solarAngle, azimuth, 0f);

        // 밤에는 어둡게
        float intensity = Mathf.Clamp01(Mathf.Sin((time / 24f) * Mathf.PI));
        sunLight.intensity = intensity * 1.2f;
    }

    void UpdateSkybox(float hour)
    {
        Material fromMat = null;
        Material toMat = null;
        float blend = 0f;

        // 4~6 : 새벽 / 6~18 : 낮 / 18~19 : 석양 / 19~4 : 밤
        if (hour < 6f) // Night -> Mornging
        {
            if(hour == 4)
                GameSystem.Instance.timeType = ETimeType.Morning;

            fromMat = nightMat;
            toMat = morningMat;
            blend = Mathf.InverseLerp(0f, 6f, hour);
        }
        else if (hour < 18f) // Mornging -> Day
        {
            if(hour == 6)
                GameSystem.Instance.timeType = ETimeType.Day;

            fromMat = morningMat;
            toMat = dayMat;
            blend = Mathf.InverseLerp(6f, 18f, hour);
        }
        else if (hour < 19f) // Day -> Sunset
        {
            if (hour == 18)
                GameSystem.Instance.timeType = ETimeType.Sunset;

            fromMat = dayMat;
            toMat = sunsetMat;
            blend = Mathf.InverseLerp(18f, 19f, hour);
        }
        else
        {
            if (hour == 19)
                GameSystem.Instance.timeType = ETimeType.Night;

            fromMat = sunsetMat;
            toMat = nightMat;
            blend = Mathf.InverseLerp(19f, 24f, hour);
        }

        // 두 머티리얼의 프로퍼티를 보간해서 skyboxMat에 적용
        BlendSkyboxMaterials(fromMat, toMat, blend);
    }

    void BlendSkyboxMaterials(Material from, Material to, float t)
    {
        if (from == null || to == null) return;

        skyboxMat.SetColor("_SkyGradientTop", Color.Lerp(from.GetColor("_SkyGradientTop"), to.GetColor("_SkyGradientTop"), t));
        skyboxMat.SetColor("_SkyGradientBottom", Color.Lerp(from.GetColor("_SkyGradientBottom"), to.GetColor("_SkyGradientBottom"), t));

        skyboxMat.SetColor("_SunHaloColor", Color.Lerp(from.GetColor("_SunHaloColor"), to.GetColor("_SunHaloColor"), t));
        skyboxMat.SetFloat("_SunHaloExponent", Mathf.Lerp(from.GetFloat("_SunHaloExponent"), to.GetFloat("_SunHaloExponent"), t));
        skyboxMat.SetFloat("_SunHaloContribution", Mathf.Lerp(from.GetFloat("_SunHaloContribution"), to.GetFloat("_SunHaloContribution"), t));

        skyboxMat.SetColor("_HorizonLineColor", Color.Lerp(from.GetColor("_HorizonLineColor"), to.GetColor("_HorizonLineColor"), t));
        skyboxMat.SetFloat("_HorizonLineExponent", Mathf.Lerp(from.GetFloat("_HorizonLineExponent"), to.GetFloat("_HorizonLineExponent"), t));
        skyboxMat.SetFloat("_HorizonLineContribution", Mathf.Lerp(from.GetFloat("_HorizonLineContribution"), to.GetFloat("_HorizonLineContribution"), t));

        skyboxMat.SetColor("_SunDiscColor", Color.Lerp(from.GetColor("_SunDiscColor"), to.GetColor("_SunDiscColor"), t));
        skyboxMat.SetFloat("_SunDiscMultiplier", Mathf.Lerp(from.GetFloat("_SunDiscMultiplier"), to.GetFloat("_SunDiscMultiplier"), t));
        skyboxMat.SetFloat("_SunDiscExponent", Mathf.Lerp(from.GetFloat("_SunDiscExponent"), to.GetFloat("_SunDiscExponent"), t));


        DynamicGI.UpdateEnvironment();
    }
}
