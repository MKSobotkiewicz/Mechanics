using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Characters
{
    public class Relationship
    {
        public Character Character { get; private set; }
        public EType Type { get; private set; }

        public Relationship(Character character, EType type)
        {
            Character = character;
            Type = type;
        }

        public static string Gendered(EType type, Character.PersonalDataType.EGender gender)
        {
            switch (type)
            {
                case EType.Parent:
                    switch (gender)
                    {
                        case Character.PersonalDataType.EGender.Female:
                            return "Mother";
                        case Character.PersonalDataType.EGender.Male:
                            return "Father";
                        default:
                            break;
                    }
                    break;
                case EType.Sibling:
                    switch (gender)
                    {
                        case Character.PersonalDataType.EGender.Female:
                            return "Sister";
                        case Character.PersonalDataType.EGender.Male:
                            return "Brother";
                        default:
                            break;
                    }
                    break;
                case EType.HalfSibling:
                    switch (gender)
                    {
                        case Character.PersonalDataType.EGender.Female:
                            return "HalfSister";
                        case Character.PersonalDataType.EGender.Male:
                            return "HalfBrother";
                        default:
                            break;
                    }
                    break;
                case EType.Auncle:
                    switch (gender)
                    {
                        case Character.PersonalDataType.EGender.Female:
                            return "Aunt";
                        case Character.PersonalDataType.EGender.Male:
                            return "Uncle";
                        default:
                            break;
                    }
                    break;
                case EType.Nibling:
                    switch (gender)
                    {
                        case Character.PersonalDataType.EGender.Female:
                            return "Niece";
                        case Character.PersonalDataType.EGender.Male:
                            return "Nephew";
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
            return type.ToString();
        }

        public enum EType
        {
            Acquaintance,
            Friend,
            Enemy,
            Parent,
            Child,
            Sibling,
            HalfSibling,
            Auncle,
            Nibling,
            Spouse,
            Lover
        }
    }
}
