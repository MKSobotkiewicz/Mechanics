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

        public int At(Resource resource)
        {
            for (int i=0;i< Count;i++)
            {
                if (this[i].Resource == resource)
                {
                    return i;
                }
            }
            return -1;
        }

        public void Log()
        {
            Debug.Log("ResourceValueList:");
            foreach (var rv in this)
            {
                rv.Log();
            }
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
                var index = rvl.At(rv2.Resource);
                if (index != -1)
                {
                    rvl[index] += rv2;
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
                var index = rvl.At(rv2.Resource);
                if (index != -1)
                {
                    rvl[index] -= rv2;
                    if (rvl[index] == null)
                    {
                        rvl.RemoveAt(index);
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
