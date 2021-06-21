using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceNode : MonoBehaviour
{
    public ResourceTypeEnum ResourceTypeEnum => resourceTypeEnum;
    [SerializeField] private ResourceTypeEnum resourceTypeEnum;
}
