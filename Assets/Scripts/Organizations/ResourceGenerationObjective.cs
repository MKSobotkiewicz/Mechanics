using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Project.Organizations
{
    class ResourceGenerationObjective : OrganizationObjective
    {
        public override string Description { get { return "Organization objective focused on generation of resources, by mining or by producing."; } protected set { } }
        public override EOrganizationObjectiveType OrganizationObjectiveType { get { return EOrganizationObjectiveType.ResourceGeneration; } protected set { } }

        public List<Resources.ResourceValue> ResourcesToProduce = new Resources.ResourceValueList();

        public override void DailyUpdate()
        {
            base.DailyUpdate();
        }

        private bool CheckIfProductionSatisfied()
        {
            var diffrence = ResourcesToProduce as Resources.ResourceValueList - Organization.GetDailyNetResourceProduction();
            if (diffrence.Count <= 0)
            {
                return true;
            }
            return false;
        }
    }
}
