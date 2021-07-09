using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Resources
{
    [Serializable]
    public class ResourceValueList:List<ResourceValue>
    {
        public ResourceValueList Copy()
        {
            var rvl= new ResourceValueList();
            foreach (var rv in this)
            {
                rvl.Add(rv);
            }
            return rvl;
        }

        public bool Contains(Resource resource)
        {
            foreach (var rv in this)
            {
                if (rv.Resource == resource)
                {
                    return true;
                }
            }
            return false;
        }

        public ResourceValue Get(Resource resource)
        {
            foreach (var rv in this)
            {
                if (rv.Resource == resource)
                {
                    return rv;
                }
            }
            return null;
        }

        public static ResourceValueList operator *(float value, ResourceValueList rvl)
        {
            var newRvl = new ResourceValueList();
            foreach (var resource in rvl)
            {
                newRvl.Add(value * resource);
            }
            return newRvl;
        }

        public static ResourceValueList operator +(ResourceValueList rvl1, ResourceValueList rvl2)
        {
            var rvl = rvl1.Copy();
            foreach (var rv2 in rvl2)
            {
                var rv = rvl.Get(rv2.Resource);
                if (rv != null)
                {
                    rv += rv2;
                }
                else
                {
                    rvl.Add(rv2.Copy());
                }
            }
            return rvl;
        }

        public static ResourceValueList operator -(ResourceValueList rvl1, ResourceValueList rvl2)
        {
            var rvl = rvl1.Copy();
            foreach (var rv2 in rvl2)
            {
                var rv = rvl.Get(rv2.Resource);
                if (rv != null)
                {
                    rv -= rv2;
                    if (rv == null)
                    {
                        rvl.Remove(rv);
                    }
                }
                else
                {
                }
            }
            return rvl;
        }
    }
}
