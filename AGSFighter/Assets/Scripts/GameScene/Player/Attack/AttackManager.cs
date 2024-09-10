using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
public Dictionary<string, (int damage, string sound, 
    AttackLevel attackLevel,AttackType attackType)> attackInfo = 
        new Dictionary<string, (int, string, AttackLevel,AttackType)>
    {
        ///{"攻撃アニメーションのbool","SEのファイル名",
        ///"上中下段の識別"}
        {"N_Weak",(300, "軽いパンチ1",AttackLevel.High,AttackType.Weak)},
        { "C_Weak", (200, "軽いキック1",AttackLevel.Low,AttackType.Weak) },
        //{ "NA_Weak", 350 },
        { "N_Mid", (600,"軽いパンチ2",AttackLevel.High,AttackType.Middle)},
        { "C_Mid", (500,"軽いキック1",AttackLevel.Low,AttackType.Middle) },
        //{ "NA_Mid", 600 },
        { "N_Stro", (800,"重いパンチ2",AttackLevel.High,AttackType.Strong) },
        { "Ashibarai", (900,"軽いキック1",AttackLevel.Low,AttackType.Huttobi) },
        { "JumpKick", (300,"軽いキック1",AttackLevel.Mid,AttackType.Weak) },
        { "JumpKick2", (500,"軽いキック1",AttackLevel.Mid,AttackType.Middle) },
        { "JumpPunch", (800,"軽いキック1",AttackLevel.Mid,AttackType.Strong) },
        { "Sakotsuwari", (600,"重いパンチ2",
                AttackLevel.Mid,AttackType.Middle) },
        //{ "NA_Stro", 900 },
        { "Syoryuken", (1400,"昇竜拳",
                AttackLevel.High,AttackType.Huttobi) },
        { "Tatsumaki", (900,"軽いキック1",
            AttackLevel.High,AttackType.Weak) },
        { "Throw", (1200,"軽いキック1",
            AttackLevel.High,AttackType.Strong) },
        { "Hasyogeki", (800,"波衝撃",
            AttackLevel.High,AttackType.Strong) },
    };


    // 攻撃の種類を設定し、そのダメージを返す
    public (int damage, string sound, AttackLevel attackLevel,AttackType attackType) GetAttackDetails(string attackType)
    {
        if (attackType != null && attackInfo.ContainsKey(attackType))
        {
            return attackInfo[attackType];
        }
        else
        {
            if (attackType != null)
            {
                Debug.LogWarning($"攻撃タイプ {attackType} に対応する詳細が見つかりませんでした。");
            }
            else
            {
                Debug.LogWarning("攻撃タイプがnullです。");
            }

            return (0, null, AttackLevel.NullLevel, AttackType.NullLevel);
        }
    }
}
