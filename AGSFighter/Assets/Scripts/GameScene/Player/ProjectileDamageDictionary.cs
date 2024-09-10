using System.Collections.Generic;
using UnityEngine;

public class ProjectileDamageDictionary : MonoBehaviour
{
    public Dictionary<string, ProjectileInfo> projectileInfo = new Dictionary<string, ProjectileInfo>();

    [SerializeField] private ProjectileDamage[] projectileDamageArray;

    private void Awake()
    {
        foreach (ProjectileDamage pd in projectileDamageArray)
        {
            string prefabName = pd.projectilePrefab.name;
            if (!projectileInfo.ContainsKey(prefabName))
            {
                projectileInfo.Add(prefabName, new ProjectileInfo(pd.damage, pd.soundEffect, AttackLevel.High, AttackType.Strong)); // îÚÇ—ìπãÔÇÕÇ∑Ç◊Çƒè„íi
            }
        }
    }

    [System.Serializable]
    public struct ProjectileDamage
    {
        public GameObject projectilePrefab;
        public int damage;
        public string soundEffect;
    }

    public class ProjectileInfo
    {
        public int damage;
        public string soundEffect;
        public AttackLevel attackLevel;
        public AttackType attackType;

        public ProjectileInfo(int damage, string soundEffect, AttackLevel attackLevel,AttackType attackType)
        {
            this.damage = damage;
            this.soundEffect = soundEffect;
            this.attackLevel = attackLevel;
            this.attackType = attackType;
        }
    }
}
