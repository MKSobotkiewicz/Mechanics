using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Project.Organizations
{
    public class OrganizationObjective : MonoBehaviour,Time.IDaily
    {
        public virtual string Description { get { return ""; } protected set { } }

        public virtual void DailyUpdate() { }

        public int Priority()
        {
            return 17;
        }

    }
}
