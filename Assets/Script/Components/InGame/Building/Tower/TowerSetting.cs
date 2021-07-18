using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/TowerSetting")]
public class TowerSetting : ScriptableObject
{
    public float FindTargetTime = 0.5f;
    public float ShootEnemyTime = 0.5f;
    public float DetectRange = 10;
    public Sprite DetectCircle;
    public Color DetectColor;
}
