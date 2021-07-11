using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Project.Organizations
{
    public class OrganizationObjective : MonoBehaviour,Time.IDaily
    {
        public Organization Organization { get; private set; }
        public virtual string Description { get { return ""; } protected set { } }
        public virtual EOrganizationObjectiveType OrganizationObjectiveType { get { return EOrganizationObjectiveType.None; } protected set { } }

        public virtual void DailyUpdate() { }

        public int Priority()
        {
            return 17;
        }

        public static OrganizationObjective Create(Organization organization, EOrganizationObjectiveType organizationType)
        {
            switch (organizationType)
            {
                case EOrganizationObjectiveType.ResourceGeneration:
                    var go = new GameObject();
                    var rg=go.AddComponent<ResourceGenerationObjective>();
                    rg.Organization = organization;
                    return rg;
            }
            return null;
        }

        public enum EOrganizationObjectiveType
        {
            None,
            ResourceGeneration
        }
    }
}
