using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Project.Units
{
    public class Unit : MonoBehaviour, IMovable, Utility.IClicable, UI.IFollowed,Time.IDaily
    {
        public bool Selected { get; private set; } = false;
        public int CurrentEntrenchment { get; private set; }
        public int MaxEntrenchment { get; private set; }
        public uint MaxManpower { get; private set; }
        public uint MaxCohesion { get; private set; }
        public UnitPath UnitPath;
        public Sprite Icon;
        public UnitTemplate Template;
        public List<UnitTemplate> Enchancements=new List<UnitTemplate>();

        private Transform unitPaths;
        private UI.Unit uiElement;
        private UI.UnitBar unitBar;
        private Player.Player player;
        private Organizations.Organization organization;
        private Map.Area location;
        private List<Map.Area> path;
        private float remainingTravelToNextArea;
        private List<Animator> animators=new List<Animator>();
        private LayerMask layerMask;
        private GameObject soldiers;
        private uint manpower;
        private uint cohesion;

        public static HashSet<Unit> AllUnits { get; private set; } = new HashSet<Unit>();

        public void Init(Map.Area _location, Player.Player _player, Organizations.Organization _organization, Transform _unitPaths,Time.Time time)
        { 
            animators = GetComponentsInChildren<Animator>().ToList();
            time.AddHourly(this);
            time.AddDaily(this);
            unitPaths =_unitPaths;
            player = _player;
            location = _location;
            organization = _organization;
            manpower = Template.MaxManpower;
            cohesion = Template.MaxCohesion;
            MaxEntrenchment = Template.Defense.MaxEntrenchment;
            MaxManpower = Template.MaxManpower;
            MaxCohesion = Template.MaxCohesion;
            foreach (var enchancement in Enchancements)
            {
                MaxEntrenchment += enchancement.Defense.MaxEntrenchment;
                MaxManpower += enchancement.MaxManpower;
                MaxCohesion += enchancement.MaxCohesion;
            }
            UpdatePosition(true);
            AllUnits.Add(this);
            transform.LookAt(location.Neighbours[0].Position, transform.position.normalized);
            CreateUIBar();
            foreach (var animator in animators)
            {
                animator.SetBool("Moving", false);
            }
            layerMask = ~LayerMask.GetMask("Unit","Outlined");
            var soldiersLM = LayerMask.NameToLayer("Soldiers");
            foreach (var tr in transform.GetComponentsInChildren<Transform>())
            {
                if (tr.gameObject.layer == soldiersLM)
                {
                    soldiers = tr.gameObject;
                    break;
                }
            }
        }

        public string Name()
        {
            return name;
        }

        public Vector3 FollowedPosition()
        {
            return Vector3.RotateTowards(transform.position, Vector3.down, 0.01f, 0);
        }

        public Map.Area Location()
        {
            return location;
        }

        public List<Map.Area> Path()
        {
            return path;
        }

        public void Click()
        {
        }

        public void Order()
        {
        }

        public AttackInfo Attack(Unit target)
        {
            var attackInfo = new AttackInfo
            {
                Piercing = Template.Attack.Piercing,
                Breakthrough = Template.Attack.Breakthrough,
                Terror = Template.Attack.Terror,
                ManpowerAttack = Template.Attack.ManpowerAttackBonus,
                CohesionAttack = Template.Attack.CohesionAttackBonus
            };
            foreach (var enchancement in Enchancements)
            {
                attackInfo.Piercing += enchancement.Attack.Piercing;
                attackInfo.Breakthrough += enchancement.Attack.Breakthrough;
                attackInfo.Terror += enchancement.Attack.Terror;
                attackInfo.ManpowerAttack += enchancement.Attack.ManpowerAttackBonus;
                attackInfo.CohesionAttack += enchancement.Attack.CohesionAttackBonus;
            }

            attackInfo.Armor = target.Template.Defense.Armor;
            attackInfo.Morale = target.Template.Defense.Morale;
            foreach (var targetEnchancement in target.Enchancements)
            {
                attackInfo.Armor += targetEnchancement.Defense.Armor;
                attackInfo.Morale += targetEnchancement.Defense.Morale;
            }

            attackInfo.UnpenetratedArmor = attackInfo.Armor - attackInfo.Piercing;
            attackInfo.UnpenetratedArmor = attackInfo.UnpenetratedArmor < 0 ? 0 : attackInfo.UnpenetratedArmor;
            attackInfo.UnbrokenEntrenchment = target.CurrentEntrenchment - attackInfo.Breakthrough;
            attackInfo.UnbrokenEntrenchment = attackInfo.UnbrokenEntrenchment < 0 ? 0 : attackInfo.UnbrokenEntrenchment;
            attackInfo.ManpowerAttack = attackInfo.ManpowerAttack - attackInfo.UnbrokenEntrenchment - attackInfo.UnpenetratedArmor;
            foreach (var dice in Template.Attack.ManpowerAttackDices)
            {
                attackInfo.ManpowerAttack += dice.Roll();
            }
            foreach (var enchancement in Enchancements)
            {
                foreach (var dice in enchancement.Attack.ManpowerAttackDices)
                {
                    attackInfo.ManpowerAttack += dice.Roll();
                }
            }
            attackInfo.ManpowerAttack = attackInfo.ManpowerAttack < 0 ? 0 : attackInfo.ManpowerAttack;

            attackInfo.Steadfastness = attackInfo.Morale - attackInfo.Terror;
            attackInfo.Steadfastness = attackInfo.Steadfastness < 0 ? 0 : attackInfo.Steadfastness;
            attackInfo.CohesionAttack = attackInfo.CohesionAttack - attackInfo.Steadfastness;
            foreach (var dice in Template.Attack.CohesionAttackDices)
            {
                attackInfo.CohesionAttack += dice.Roll();
            }
            foreach (var enchancement in Enchancements)
            {
                foreach (var dice in enchancement.Attack.CohesionAttackDices)
                {
                    attackInfo.CohesionAttack += dice.Roll();
                }
            }
            attackInfo.CohesionAttack = attackInfo.CohesionAttack < 0 ? 0 : attackInfo.CohesionAttack;

            return attackInfo;
        }

        public void Unclick()
        {
            if (player.SelectedUnits.Count == 0)
            {
                player.SelectUnit(this);
            }
        }

        public void SetSelected(bool value)
        {
            /*if (value == Selected)
            {
                return;
            }*/
            var outlinedLM = LayerMask.NameToLayer("Outlined");
            var unitLM = LayerMask.NameToLayer("Unit");
            var tranparentLM = LayerMask.NameToLayer("Transparent");
            Selected = value;
            if (Selected)
            {
                UnitPath.Show();
                player.SelectedUnits.Add(this);
                Debug.Log("Selected");
                foreach (var tr in transform.GetComponentsInChildren<Transform>())
                {
                    if (tr.gameObject.layer != tranparentLM)
                    {
                        tr.gameObject.layer = outlinedLM;
                    }
                }
            }
            else
            {
                UnitPath.Hide();
                player.SelectedUnits.Remove(this);
                foreach (var tr in transform.GetComponentsInChildren<Transform>())
                {
                    if (tr.gameObject.layer != tranparentLM)
                    {
                        tr.gameObject.layer = unitLM;
                    }
                }
            }
        }

        public float Speed()
        {
            var speed = Template.Speed;
            foreach (var enchancement in Enchancements)
            {
                speed += enchancement.Speed;
            }
            return speed;
        }

        public Organizations.Organization GetOrganization()
        {
            return organization;
        }

        public float RemainingTravelToNextArea()
        {
            return remainingTravelToNextArea;
        }

        public bool Move(Map.Area target)
        {
            path = Utility.Pathfinder.FindPath(location,target, Template.IgnoreTerrain);
            if (path!=null)
            {
                remainingTravelToNextArea = Template.IgnoreTerrain ? 1 : path[0].Weight();
                //uiElement.PathSuccess(path);
                UpdatePosition(false);
                foreach (var animator in animators)
                {
                    animator.SetBool("Moving", true);
                }
                if (soldiers != null)
                {
                    soldiers.SetActive(false);
                }
                return true;
            }
            //uiElement.PathFail();
            return false;
        }

        public uint Priority()
        {
            return 12;
        }

        public void DailyUpdate()
        {
            CurrentEntrenchment++;
            CurrentEntrenchment = CurrentEntrenchment > MaxEntrenchment ? MaxEntrenchment : CurrentEntrenchment;
        }

        public void HourlyUpdate()
        {
            if (path == null)
            {
                return;
            }
            if (path.Count == 0)
            {
                return;
            }
            if (path != null)
            {
                remainingTravelToNextArea -= Speed();
            }
            while (remainingTravelToNextArea <= 0)
            {
                if (path== null)
                { 
                    break;
                }
                Debug.Log("MOVE");
                path.RemoveAt(0);
                location = path[0];
                if (path.Count > 1)
                {
                    remainingTravelToNextArea += Template.IgnoreTerrain?1: path[0].Weight();
                }
                else if (path.Count == 1)
                {
                    UnitPath.Destroy();
                    foreach (var animator in animators)
                    {
                        animator.SetBool("Moving", false);
                    }

                    if (soldiers != null)
                    {
                        soldiers.SetActive(true);
                        SetSelected(Selected);
                    }
                    path = null;
                }
                UpdatePosition(false);
            }
        }

        public void UpdatePosition(bool instant)
        {
            var hit = new RaycastHit();
            if (path != null)
            {
                if (path.Count > 1)
                {
                    if (Physics.Raycast(Vector3.Lerp(location.Position,path[1].Position,0.25f) * 1.1f, -location.Position.normalized, out hit, Mathf.Infinity, layerMask))
                    {
                        if (!instant)
                        {
                            transform.LookAt(hit.point, transform.position.normalized);
                            LeanTween.move(gameObject, hit.point, 0.5f).setEaseInOutSine().setOnComplete(() => { LeanTween.rotate(gameObject, Quaternion.LookRotation(path[1].Position - location.Position, transform.position.normalized).eulerAngles,0.2f); });
                        }
                        else
                        {
                            transform.LookAt(path[1].Position, transform.position.normalized);
                            transform.position = hit.point;
                        }
                        UnitPath.Destroy();
                        UnitPath.Create(hit.point, path, unitPaths);
                    }
                    return;
                }
            }
            if (Physics.Raycast(location.Position * 1.1f, -location.Position.normalized, out hit, Mathf.Infinity))
            {
                if (!instant)
                {
                    transform.LookAt(hit.point, transform.position.normalized);
                    LeanTween.move(gameObject, hit.point, 0.5f).setEaseInOutSine();
                }
                else
                {
                    transform.LookAt(hit.point, transform.position.normalized);
                    transform.position = hit.point;
                }
            }
        }

        public void CreateUIBar()
        {
            unitBar = UI.UnitBar.Create(UnityEngine.Camera.main.GetComponentInChildren<Canvas>(),this);
        }

        public void UpdateUIBarValues()
        {

        }
    }
}
