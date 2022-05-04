using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Resources
{
    [Serializable]
    public class ResourceValue
    {
        [SerializeField]
        public Resource Resource;
        [SerializeField]
        public ulong Value;

        public ResourceValue(Resource resource,ulong value=0)
        {
            Resource = resource;
            Value = value;
        }

        public ResourceValue Copy()
        {
            return new ResourceValue(Resource, Value);
        }

        public void Log()
        {
            Debug.Log("    "+Resource.name+" "+Value);
        }

        public static ResourceValue operator +(ResourceValue rv1, ResourceValue rv2)
        {
            if (rv1.Resource!=rv2.Resource)
            {
                throw new ArgumentException("Tried to add diffrent resources.");
            }
            return new ResourceValue(rv1.Resource,rv1.Value+rv2.Value);
        }

        public static ResourceValue operator *(float value, ResourceValue resource)
        {
            return new ResourceValue(resource.Resource, (uint)(resource.Value * value));
        }

        public static ResourceValue operator -(ResourceValue rv1, ResourceValue rv2)
        {
            if (rv1.Resource != rv2.Resource)
            {
                throw new ArgumentException("Tried to substract diffrent resources.");
            }
            if (rv1.Value <= rv2.Value)
            {
                return null;
            }
            return new ResourceValue(rv1.Resource, rv1.Value - rv2.Value);
        }

        public static bool operator >(ResourceValue rv1, ResourceValue rv2)
        {
            if (rv1.Resource != rv2.Resource)
            {
                throw new ArgumentException("Tried to compare diffrent resources.");
            }
            return rv1.Value > rv2.Value;
        }

        public static bool operator <(ResourceValue rv1, ResourceValue rv2)
        {
            if (rv1.Resource != rv2.Resource)
            {
                throw new ArgumentException("Tried to compare diffrent resources.");
            }
            return rv1.Value < rv2.Value;
        }
    }
}
