using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Project.Organizations
{
    public class OrganizationsManager : MonoBehaviour
    {
        public List<Organization> Organizations { get; private set; } = new List<Organization>();

        public void Start()
        {
            CreateOrganizations();
        }

        private void CreateOrganizations()
        {
        }
    }
}
