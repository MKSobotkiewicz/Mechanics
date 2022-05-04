using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Characters
{
    public class RelationshipsList : List<Relationship>
    {
        public new bool Add(Relationship relationship)
        {
            foreach (var item in this)
            {
                if (item.Character == relationship.Character)
                {
                    return false;
                }
            }
            base.Add(relationship);
            return true;
        }

        public bool Remove(Character character)
        {
            for (int i = 0; i < Count; i++)
            {
                if (this[i].Character == character)
                {
                    RemoveAt(i);
                    return true;
                }
            }
            return false;
        }

        public List<Character> GetValue(Relationship.EType type)
        {
            var list = new List<Character>();
            foreach(var relationship in this)
            {
                if (relationship.Type == type)
                {
                    list.Add(relationship.Character);
                }
            }
            return list;
        }

    public List<Character> this[Relationship.EType type]
        {
            get => GetValue(type);
        }
    }
}
