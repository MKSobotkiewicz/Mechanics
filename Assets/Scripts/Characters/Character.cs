using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Project.Characters
{
    public class Character : MonoBehaviour
    {
        public PersonalDataType PersonalData { get; private set; }
        public RelationshipsList RelationshipsList { get; private set; } = new RelationshipsList();

        private static readonly List<Character> allCharacters = new List<Character>();
        
        private bool AddParents(Character[] parents)
        {
            if (parents == null)
            {
                return true;
            }
            if (parents.Length > 2)
            {
                return false;
            }
            foreach (var parent in parents)
            {
                RelationshipsList.Add(new Relationship(parent,Relationship.EType.Parent));
                parent.RelationshipsList.Add(new Relationship(this, Relationship.EType.Child));
                foreach (var sibling in parent.RelationshipsList[Relationship.EType.Child])
                {
                    if (RelationshipsList[Relationship.EType.HalfSibling].Contains(sibling))
                    {
                        RelationshipsList.Remove(sibling);
                        RelationshipsList.Add(new Relationship(sibling, Relationship.EType.Sibling));
                        sibling.RelationshipsList.Remove(this);
                        sibling.RelationshipsList.Add(new Relationship(this, Relationship.EType.Sibling));
                    }
                    else
                    {
                        RelationshipsList.Add(new Relationship(sibling, Relationship.EType.HalfSibling));
                        sibling.RelationshipsList.Add(new Relationship(this, Relationship.EType.HalfSibling));
                    }
                }
            }
            return true;
        }

        public static Character Create(PersonalDataType personalData, Character[] parents=null, Transform parentTransform = null)
        {
            var go = new GameObject("Character "+ allCharacters.Count);
            go.transform.parent = parentTransform;
            go.transform.localEulerAngles = new Vector3();
            go.transform.localPosition = new Vector3();
            var character=go.AddComponent<Character>();
            character.PersonalData = personalData;
            character.AddParents(parents);
            return character;

        }

        [System.Serializable]
        public class PersonalDataType
        {
            public string FirstName { get; private set; }
            public string Surname { get; private set; }
            public EGender Gender { get; private set; }
            public DateTime DateOfBirth { get; private set; }

            public PersonalDataType(EGender gender, DateTime dateOfBirth, string firstName, string surname="")
            {
                FirstName = firstName;
                Surname = surname;
                Gender = gender;
                DateOfBirth = dateOfBirth;
            }

            public uint Age(DateTime currentDate)
            {
                if (currentDate.Month > DateOfBirth.Month)
                {
                    return (uint)(currentDate.Year - DateOfBirth.Year);
                }
                return (uint)(currentDate.Year - DateOfBirth.Year-1);
            }

            public enum EGender
            {
                Female,
                Male,
                Other
            }
        }
    }
}
