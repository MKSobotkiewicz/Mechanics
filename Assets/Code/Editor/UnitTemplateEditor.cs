using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using Project.Units;

namespace Project.Units
{
    [CustomEditor(typeof(UnitTemplate))]
    [CanEditMultipleObjects]
    class UnitTemplateEditor : Editor
    {
        private List<Dices.EDice> manpowerDices = new List<Dices.EDice>();
        private List<Dices.EDice> cohesionDices = new List<Dices.EDice>();
        private Dices.DiceFactory diceFactory = new Dices.DiceFactory();

        void OnEnable()
        {
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            UnitTemplate unitTemplate = (UnitTemplate)target;
            unitTemplate.LoadFromXml();

            unitTemplate.Icon = (Sprite)EditorGUILayout.ObjectField("Icon",unitTemplate.Icon,typeof(Sprite));

            EditorGUILayout.HelpBox("Checks if it's standalone unit or enchancement to other units.", MessageType.None);
            unitTemplate.Enchancement = EditorGUILayout.Toggle("Enchancement", unitTemplate.Enchancement);

            EditorGUILayout.LabelField("Movement", EditorStyles.miniLabel);
            EditorGUI.indentLevel++;
            EditorGUILayout.HelpBox("Unit speed in areas/hour(on road).", MessageType.None);
            unitTemplate.Speed = EditorGUILayout.FloatField("Speed", unitTemplate.Speed);
            EditorGUILayout.HelpBox("Ignore Terrain Modifiers defines if unit should ignore terrain modifiers while moving. Good for things like air mobile units.", MessageType.None);
            unitTemplate.IgnoreTerrain = EditorGUILayout.Toggle("Ignore Terrain Modifiers", unitTemplate.IgnoreTerrain);
            EditorGUI.indentLevel--;

            EditorGUILayout.LabelField("Combat stats", EditorStyles.miniLabel);
            EditorGUI.indentLevel++;
            EditorGUILayout.HelpBox("Manpower represents unit soldiers. Lower manpower makes all unit attacks proportionally lower. When Manpower reaches zero the unit is destroyed.", MessageType.None);
            unitTemplate.MaxManpower = (uint)EditorGUILayout.IntSlider("Max Manpower", (int)unitTemplate.MaxManpower, 10, 500);
            EditorGUILayout.HelpBox("Cohesion represents unit capability to fight. Does not affect unit attacks, but cohesion of 0 makes unit rout.", MessageType.None);
            unitTemplate.MaxCohesion = (uint)EditorGUILayout.IntSlider("Max Cohesion", (int)unitTemplate.MaxCohesion, 10, 500);

            EditorGUILayout.LabelField("Attack", EditorStyles.miniLabel);
            EditorGUI.indentLevel++;

            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Manpower Attack Dices");
            manpowerDices = new List<Dices.EDice>();
            foreach (var dice in unitTemplate.Attack.ManpowerAttackDices)
            {
                manpowerDices.Add(dice.ToEnum());
            }
            if (GUILayout.Button("-") && manpowerDices.Count > 0)
            {
                manpowerDices.RemoveAt(manpowerDices.Count - 1);
            }
            if (GUILayout.Button("+"))
            {
                manpowerDices.Add(Dices.EDice.D4);
            }
            GUILayout.EndHorizontal();
            EditorGUILayout.HelpBox("Attacks  against enemy manpower. Neutralizes opposing forces, making their attacks proportionally lower. When Manpower reaches zero enemy unit is destroyed.", MessageType.None);
            for (int i=0;i< manpowerDices.Count;i++)
            {
                manpowerDices[i] = (Dices.EDice)EditorGUILayout.EnumPopup(manpowerDices[i]);
            }
            unitTemplate.Attack.ManpowerAttackDices = new List<Dices.Dice>();
            for (int i=0;i< manpowerDices.Count;i++)
            {
                unitTemplate.Attack.ManpowerAttackDices.Add(diceFactory.Create(manpowerDices[i]));
            }
            EditorGUILayout.HelpBox("Base bonus to attacks against enemy manpower.", MessageType.None);
            unitTemplate.Attack.ManpowerAttackBonus = EditorGUILayout.IntSlider("Manpower Attack Bonus", unitTemplate.Attack.ManpowerAttackBonus, 0, 20);
            EditorGUILayout.HelpBox("Piercing negates enemy armor(manpower defense).", MessageType.None);
            unitTemplate.Attack.Piercing = EditorGUILayout.IntSlider("Piercing", unitTemplate.Attack.Piercing, 0, 20);
            EditorGUILayout.HelpBox("Breakthrough negates enemy entrenchment(manpower defense).", MessageType.None);

            unitTemplate.Attack.Breakthrough = EditorGUILayout.IntSlider("Breakthrough", unitTemplate.Attack.Breakthrough, 0, 20);

            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Cohesion Attack Dices");
            cohesionDices = new List<Dices.EDice>();
            foreach (var dice in unitTemplate.Attack.CohesionAttackDices)
            {
                cohesionDices.Add(dice.ToEnum());
            }
            if (GUILayout.Button("-") && cohesionDices.Count > 0)
            {
                cohesionDices.RemoveAt(cohesionDices.Count - 1);
            }
            if (GUILayout.Button("+"))
            {
                cohesionDices.Add(Dices.EDice.D4);
            }
            GUILayout.EndHorizontal();
            EditorGUILayout.HelpBox("Attacks against enemy cohesion. Does not neutralize enemy forces, but cohesion of 0 makes unit rout.", MessageType.None);
            for (int i = 0; i < cohesionDices.Count; i++)
            {
                cohesionDices[i] = (Dices.EDice)EditorGUILayout.EnumPopup(cohesionDices[i]);
            }
            unitTemplate.Attack.CohesionAttackDices = new List<Dices.Dice>();
            for (int i = 0; i < cohesionDices.Count; i++)
            {
                unitTemplate.Attack.CohesionAttackDices.Add(diceFactory.Create(cohesionDices[i]));
            }
            EditorGUILayout.HelpBox("Base bonus to attacks against enemy cohesion.", MessageType.None);
            unitTemplate.Attack.CohesionAttackBonus = EditorGUILayout.IntSlider("Cohesion Atack Bonus", unitTemplate.Attack.CohesionAttackBonus, 0, 20);
            EditorGUILayout.HelpBox("Terror negates enemy morale(cohesion defense).", MessageType.None);
            unitTemplate.Attack.Terror = EditorGUILayout.IntSlider("Terror", unitTemplate.Attack.Terror, 0, 20);
            EditorGUI.indentLevel--;
            
            EditorGUILayout.LabelField("Defense", EditorStyles.miniLabel);
            EditorGUI.indentLevel++;
            EditorGUILayout.HelpBox("Armor is a defense against attacks targeting manpower.", MessageType.None);
            unitTemplate.Defense.Armor = EditorGUILayout.IntSlider("Armor", unitTemplate.Defense.Armor, 0, 20);
            EditorGUILayout.HelpBox("Entrenchment is a defense against attacks targeting manpower. It needs time to fill, increasing by 1 every day, starting from 0.", MessageType.None);
            unitTemplate.Defense.MaxEntrenchment = EditorGUILayout.IntSlider("Max Entrenchment", unitTemplate.Defense.MaxEntrenchment, 0, 20);
            EditorGUILayout.HelpBox("Morale is a defense against attacks targeting cohesion.", MessageType.None);
            unitTemplate.Defense.Morale = EditorGUILayout.IntSlider("Morale", unitTemplate.Defense.Morale, 0, 20);
            EditorGUI.indentLevel = 0;

            unitTemplate.SaveAsXml();
            serializedObject.ApplyModifiedProperties();
            EditorApplication.update.Invoke();
        }
    }
}
