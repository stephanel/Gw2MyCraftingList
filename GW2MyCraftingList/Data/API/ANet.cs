using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;

namespace GW2ExplorerCraftTool.Data.API
{
    interface IANetObject
    {
        bool ContainsText(string pattern);
    }

    public class ANet
    {
        public sealed class ItemType
        {
            public static readonly List<string> Values = new List<string>();

            static ItemType()
            {
                Values.Add("Armor");
                Values.Add("Back");
                Values.Add("Bag");
                Values.Add("Consumable");
                Values.Add("Container");
                Values.Add("CraftingMaterial");
                Values.Add("Gathering");
                Values.Add("Gizmo");
                Values.Add("MiniPet");
                Values.Add("Tool");
                Values.Add("Trinket");
                Values.Add("Trophy");
                Values.Add("UpgradeComponent");
                Values.Add("Weapon");
            }
        }

        [DataContract]
        public class Armor : IANetObject
        {
            [DataMember]
            public string type;
            [DataMember]
            public string weight_class;
            [DataMember]
            public string defense;
            [DataMember]
            public List<InfusionSlot> infusion_slots;
            [DataMember]
            public InfixUpgrade infix_upgrade;
            [DataMember]
            public string suffix_item_id;

            public override string ToString()
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                if (!String.IsNullOrEmpty(defense))
                    sb.AppendLine(String.Format("Défense : {0}", defense));

                if (infix_upgrade != null)
                    sb.AppendLine(infix_upgrade.ToString());

                for (int i = 0; i < infusion_slots.Count; i++)
                {
                    if (infusion_slots[i] != null)
                        sb.AppendLine(infusion_slots[i].ToString());
                }

                if (!String.IsNullOrEmpty(type))
                    sb.AppendLine(this.GetObjectType(type)); // type

                if (!String.IsNullOrEmpty(weight_class))
                    sb.AppendLine(Config.GetLocalizedName(Data.WeightClass.GetWeightClass(weight_class)));

                return sb.ToString();
            }

            private string GetObjectType(string key)
            {
                Data.RecipeType rt = null;
                Data.RecipeType.RecipeTypes.TryGetValue(key, out rt);
                return Config.GetLocalizedName(rt);
            }

            public bool ContainsText(string pattern)
            {
                try
                {
                    if (infusion_slots != null)
                        foreach (InfusionSlot slot in infusion_slots)
                            if (slot.ContainsText(pattern))
                                return true;

                    if (infix_upgrade != null)
                        if (infix_upgrade.ContainsText(pattern))
                            return true;

                    return false;
                }
                catch
                {
                    throw;
                }
            }
        }

        [DataContract]
        public class Attribute : IANetObject
        {
            [DataMember]
            public string attribute;
            [DataMember]
            public string modifier;

            public override string ToString()
            {
                //return String.Format("{0}: {1}", Config.GetLocalizedName(Data.AttributeType.GetAttributeType(attribute)), modifier);
                Data.AttributeType type = Data.AttributeType.GetAttributeType(attribute);
                return String.Format(type.Scheme, Config.GetLocalizedName(type), modifier);
            }

            public bool ContainsText(string pattern)
            {
                try
                {
                    return StringHelper.Contains(
                        Config.GetLocalizedName(Data.AttributeType.GetAttributeType(attribute)),
                        pattern);
                }
                catch
                {
                    throw;
                }
            }
        }

        [DataContract]
        public class Bag : IANetObject
        {
            [DataMember]
            public string no_sell_or_sort;
            [DataMember]
            public string size;

            public override string ToString()
            {
                return base.ToString();
            }

            public bool ContainsText(string pattern)
            {
                try
                {
                    if (size != null)
                        if (StringHelper.Contains(size, pattern))
                            return true;

                    return false;
                }
                catch
                {
                    throw;
                }
            }
        }
        
        [DataContract]
        public class Buff : IANetObject
        {
            [DataMember]
            public string skill_id;
            [DataMember]
            public string description;

            public override string ToString()
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                if (!String.IsNullOrEmpty(description))
                    sb.AppendLine(description.ClearHtmlTags());
                return sb.ToString();
            }

            public bool ContainsText(string pattern)
            {
                try
                {
                    if (StringHelper.Contains(description, pattern))
                        return true;

                    return false;
                }
                catch
                {
                    throw;
                }
            }
        }

        [DataContract]
        public class Consumable : IANetObject
        {
            [DataMember]
            public string type;
            [DataMember]
            public string duration_ms;
            [DataMember]
            public string description;

            public override string ToString()
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                if (!String.IsNullOrEmpty(description))
                    sb.AppendLine(description.ClearHtmlTags());
                return sb.ToString();
            }

            public bool ContainsText(string pattern)
            {
                try
                {
                    if (description != null)
                        if (StringHelper.Contains(description, pattern))
                            return true;

                    return false;
                }
                catch
                {
                    throw;
                }
            }
        }

        [DataContract]
        public class Container : IANetObject
        {
            [DataMember]
            public string type;

            public override string ToString()
            {
                return base.ToString();
            }

            public bool ContainsText(string pattern)
            {
                return false;
            }
        }

        [DataContract]
        public class CraftingMaterial : IANetObject
        {
            public override string ToString()
            {
                return base.ToString();
            }

            public bool ContainsText(string pattern)
            {
                return false;
            }
        }

        [DataContract]
        public class Gizmo : IANetObject
        {
            public string type;

            public override string ToString()
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();

                if (!String.IsNullOrEmpty(type))
                    sb.AppendLine(type);

                return sb.ToString();
            }

            public bool ContainsText(string pattern)
            {
                return false;
            }
        }

        [DataContract]
        public class Ingredient
        {
            [DataMember]
            public string item_id;
            [DataMember]
            public string count;

            public API.ANet.Item Item { get; set; }

        }

        [DataContract]
        public class InfixUpgrade : IANetObject
        {
            [DataMember]
            public Buff buff;
            [DataMember]
            public List<Attribute> attributes;

            public override string ToString()
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();

                for (int i = 0; i < attributes.Count; i++)
                {
                    if(attributes[i]!=null)
                        sb.AppendLine(attributes[i].ToString());
                }

                if (buff != null)
                    sb.AppendLine(buff.ToString());
                
                return sb.ToString();
            }

            public bool ContainsText(string pattern)
            {
                try
                {
                    if (buff != null)
                        if (buff.ContainsText(pattern))
                            return true;

                    if (attributes != null)
                        foreach (Attribute a in attributes)
                            if (a.ContainsText(pattern))
                                return true;

                    return false;
                }
                catch
                {
                    throw;
                }
            }

        }
        
        [DataContract]
        public class InfusionSlot : IANetObject
        {
            [DataMember]
            public List<string> flags;
            [DataMember]
            public string item;

            public override string ToString()
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();

                //foreach (var flag in this.flags)
                //    sb.Append(String.Format("Infusion slot: {0}",flag));

                if(this.flags.Count>0)
                    sb.Append(String.Format("{0}: {1}", Properties.Resources.InfusionSlotLabel, this.flags[0]));

                return sb.ToString();
            }

            public bool ContainsText(string pattern)
            {
                try
                {
                    if (StringHelper.Contains(item, pattern))
                        return true;

                    if (flags != null)
                        foreach (string flag in flags)
                            if (StringHelper.Contains(flag, pattern))
                                return true;

                    return false;
                }
                catch
                {
                    throw;
                }
            }
        }

        [DataContract]
        public class InfusionUpgradeFlags : IANetObject
        {
            public override string ToString()
            {
                return base.ToString();
            }

            public bool ContainsText(string pattern)
            {
                return false;
            }

        }

        [DataContract]
        public class Item : IANetObject
        {
            [DataMember]
            public string item_id;
            [DataMember]
            public string name;
            [DataMember]
            public string description;
            [DataMember]
            public string type;
            [DataMember]
            public int icon_file_id;
            [DataMember]
            public string icon_file_signature;
            [DataMember]
            public List<string> flags;
            [DataMember]
            public string level;
            [DataMember]
            public string rarity;
            [DataMember]
            public string vendor_value;
            //[DataMember]
            //public List<string> game_types;
            //[DataMember]
            //public List<string> flags;
            //[DataMember]
            //public List<string> restrictions;
            [DataMember]
            public Armor armor;
            [DataMember]
            public Bag bag;
            [DataMember]
            public Consumable consumable;
            [DataMember]
            public Container container;
            [DataMember]
            public CraftingMaterial crafting_material;
            [DataMember]
            public Gizmo gizmo;
            [DataMember]
            public Tool tool;
            [DataMember]
            public Trinket trinket;
            [DataMember]
            public Trophy trophy;
            [DataMember]
            public UpgradeComponent upgrade_component;
            [DataMember]
            public Weapon weapon;

            public string img;
            public string gw2db_external_id;

            [DataMember]
            public string error;
            [DataMember]
            public string product;
            [DataMember]
            public string module;
            [DataMember]
            public string line;
            [DataMember]
            public string text;

            [DataMember]
            public string line_number;

            public string IconPath
            {
                get
                {
                    return String.Format("https://render.guildwars2.com/file/{0}/{1}.{2}", this.icon_file_signature, this.icon_file_id, Config.AssetIconFormat);
                }
            }

            public bool ContainsText(string pattern)
            {
                try
                {
                    if (StringHelper.Contains(name, pattern))
                        return true;

                    if (StringHelper.Contains(description, pattern))
                        return true;

                    if (this.armor != null)
                        if (this.armor.ContainsText(pattern))
                            return true;

                    if (this.bag != null)
                        if (this.bag.ContainsText(pattern))
                            return true;

                    if (this.consumable != null)
                        if (this.consumable.ContainsText(pattern))
                            return true;

                    if (this.container != null)
                        if (this.container.ContainsText(pattern))
                            return true;

                    if (this.crafting_material != null)
                        if (this.crafting_material.ContainsText(pattern))
                            return true;

                    if (this.gizmo != null)
                        if (this.gizmo.ContainsText(pattern))
                            return true;

                    if (this.tool != null)
                        if (this.tool.ContainsText(pattern))
                            return true;

                    if (this.trinket != null)
                        if (this.trinket.ContainsText(pattern))
                            return true;

                    if (this.trophy != null)
                        if (this.trophy.ContainsText(pattern))
                            return true;

                    if (this.upgrade_component != null)
                        if (this.upgrade_component.ContainsText(pattern))
                            return true;

                    if (this.weapon != null)
                        if (this.weapon.ContainsText(pattern))
                            return true;

                    return false;
                }
                catch
                {
                    throw;
                }
            }
            public bool EqualsText(string pattern)
            {
                try
                {
                    if (StringHelper.Equals(name, pattern))
                        return true;

                    return false;
                }
                catch
                {
                    throw;
                }
            }

            public override string ToString()
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();

                if (!String.IsNullOrEmpty(description))
                    sb.AppendLine(description.ClearHtmlTags());

                if (armor != null)
                    sb.Append(armor.ToString());

                // do nohting if item is a bag
                //if (bag != null)
                //sb.Append(bag.ToString());

                if (consumable != null)
                    sb.Append(consumable.ToString());

                // do nohting if item is a container
                //if (container != null)
                //sb.Append(container.ToString());

                if (crafting_material != null)
                    sb.Append(crafting_material.ToString());

                if (gizmo != null)
                    sb.Append(gizmo.ToString());

                if (trinket != null)
                    sb.Append(trinket.ToString());


                if (trophy != null)
                    sb.Append(trophy.ToString());

                if (upgrade_component != null)
                    sb.Append(upgrade_component.ToString());

                if (weapon != null)
                    sb.Append(weapon.ToString());

                //sb.AppendLine();

                if (!String.IsNullOrEmpty(rarity))
                    sb.AppendLine(Config.GetLocalizedName(Data.Rarity.GetRatity(rarity)));

                if (!String.IsNullOrEmpty(level))
                    sb.Append(String.Format("{0}: {1}", Properties.Resources.ItemDescriptionRequiredLevelLabel, level));

                return sb.ToString();
            }
        }

        [DataContract]
        public class Recipe : IANetObject
        {
            [DataMember]
            public string recipe_id;
            [DataMember]
            public string type;
            [DataMember]
            public string[] disciplines;
            [DataMember]
            public string output_item_id;
            [DataMember]
            public string output_item_count;
            [DataMember]
            public string min_rating;
            [DataMember]
            public string time_to_craft_ms;
            [DataMember]
            public List<Ingredient> ingredients;

            public API.ANet.Item Item { get; set; }

            [DataMember]
            public string error;
            [DataMember]
            public string product;
            [DataMember]
            public string module;
            [DataMember]
            public string line;
            [DataMember]
            public string text;

            //[DataMember]
            //public string line_number;

            public bool ContainsText(string pattern)
            {
                try
                {
                    if (Item != null)
                        if (Item.ContainsText(pattern))
                            return true;

                    return false;
                }
                catch
                {
                    throw;
                }
            }
            public bool EqualsText(string pattern)
            {
                try
                {
                    if (Item != null)
                        if (Item.EqualsText(pattern))
                            return true;

                    return false;
                }
                catch
                {
                    throw;
                }
            }

            private string GetDiscipline(string key)
            {
                Data.Discipline d = Data.Discipline.GetDiscipline(key);
                if (d == null)
                    return null;
                return Config.GetLocalizedName(d);
            }

            public override string ToString()
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();

                if (!String.IsNullOrEmpty(this.Item.description))
                    sb.AppendLine(this.Item.description.ClearHtmlTags());

                if (this.Item.armor != null)
                    sb.Append(this.Item.armor.ToString());

                // do nohting if item is a bag
                //if (bag != null)
                //sb.Append(bag.ToString());

                if (this.Item.consumable != null)
                    sb.Append(this.Item.consumable.ToString());

                // do nohting if item is a container
                //if (container != null)
                //sb.Append(container.ToString());

                if (this.Item.crafting_material != null)
                    sb.Append(this.Item.crafting_material.ToString());

                if (this.Item.gizmo != null)
                    sb.Append(this.Item.gizmo.ToString());

                if (this.Item.trinket != null)
                    sb.Append(this.Item.trinket.ToString());


                if (this.Item.trophy != null)
                    sb.Append(this.Item.trophy.ToString());

                if (this.Item.upgrade_component != null)
                    sb.Append(this.Item.upgrade_component.ToString());

                if (this.Item.weapon != null)
                    sb.Append(this.Item.weapon.ToString());

                //sb.AppendLine();

                //if (!String.IsNullOrEmpty(Item.rarity))
                //    sb.AppendLine(Config.GetLocalizedName(Data.Rarity.GetRatity(Item.rarity)));
                //if (!String.IsNullOrEmpty(this.type))
                //    sb.AppendLine(Config.GetLocalizedName(Data.RecipeType.GetRecipeType(this.type)));

                sb.AppendLine();
                if (this.disciplines != null && this.disciplines.Length > 0)
                    sb.AppendLine(
                        String.Format("{0}: {1}",
                        Properties.Resources.ItemDescriptionDisciplinesLabel,
                            String.Join(", ", this.disciplines.AsQueryable().Select(p => this.GetDiscipline(p)).ToArray())));

                sb.Append(String.Format("{0}: {1}", Properties.Resources.ItemDescriptionRequiredLevelLabel, min_rating));

                return sb.ToString();
            }
        }

        [DataContract]
        public class Recipes
        {
            [DataMember]
            public List<int> recipes;
        }

        [DataContract]
        public class Tool : IANetObject
        {
            [DataMember]
            public string type;
            [DataMember]
            public string charges;

            public override string ToString()
            {
                return base.ToString();
            }

            public bool ContainsText(string pattern)
            {
                return false;
            }
        }

        [DataContract]
        public class Trinket : IANetObject
        {
            [DataMember]
            public string type;
            [DataMember]
            public List<InfusionSlot> infusion_slots;
            [DataMember]
            public InfixUpgrade infix_upgrade;
            [DataMember]
            public string suffix_item_id;

            public override string ToString()
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();

                if (infix_upgrade!=null)
                    sb.AppendLine(infix_upgrade.ToString());
                for (int i = 0; i < infusion_slots.Count; i++)
                {
                    if (infusion_slots[i]!=null)
                        sb.AppendLine(infusion_slots[i].ToString());
                }
                //if (!String.IsNullOrEmpty(type))
                //    sb.AppendLine(this.GetObjectType(type)); // type

                return sb.ToString();
            }

            //private string GetObjectType(string key)
            //{
            //    Data.RecipeType rt = null;
            //    Data.RecipeType.RecipeTypes.TryGetValue(key, out rt);
            //    return Config.GetLocalizedName(rt);
            //}

            public bool ContainsText(string pattern)
            {
                try
                {
                    if (infix_upgrade != null)
                        if (infix_upgrade.ContainsText(pattern))
                            return true;

                    if (infusion_slots != null)
                        foreach (InfusionSlot slot in infusion_slots)
                            if (slot.ContainsText(pattern))
                                return true;

                    return false;
                }
                catch
                {
                    throw;
                }
            }

        }

        [DataContract]
        public class Trophy : IANetObject
        {
            public override string ToString()
            {
                return base.ToString();
            }

            public bool ContainsText(string pattern)
            {
                return false;
            }
        }

        [DataContract]
        public class UpgradeComponent : IANetObject
        {
            [DataMember]
            public List<string> bonuses;
            [DataMember]
            public List<string> flags;
            [DataMember]
            public List<InfusionUpgradeFlags> infusion_upgrade_flags;
            [DataMember]
            public InfixUpgrade infix_upgrade;
            [DataMember]
            public string suffix;
            [DataMember]
            public string type;

            public override string ToString()
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();

                if (infix_upgrade!=null)
                    sb.AppendLine(infix_upgrade.ToString());
                
                if(infusion_upgrade_flags!=null)
                    for (int i = 0; i < infusion_upgrade_flags.Count; i++)
                        if (infusion_upgrade_flags[i]!=null)
                            sb.AppendLine(infusion_upgrade_flags[i].ToString());

                if (bonuses != null)
                    for (int i = 0; i < bonuses.Count; i++)
                        sb.AppendLine(bonuses[i]);
   
                return sb.ToString();
            }

            public bool ContainsText(string pattern)
            {
                try
                {
                    if(suffix!=null)
                        if (StringHelper.Contains(suffix, pattern))
                            return true;

                    if (infix_upgrade != null)
                        if (infix_upgrade.ContainsText(pattern))
                            return true;

                    if (infusion_upgrade_flags != null)
                        foreach (InfusionUpgradeFlags flag in infusion_upgrade_flags)
                            if (flag.ContainsText(pattern))
                                return true;

                    return false;
                }
                catch
                {
                    throw;
                }
            }
        }

        [DataContract]
        public class Weapon : IANetObject
        {
            [DataMember]
            public string type;
            [DataMember]
            public string damage_type;
            [DataMember]
            public string min_power;
            [DataMember]
            public string max_power;
            [DataMember]
            public string defense;
            [DataMember]
            public List<InfusionSlot> infusion_slots;
            [DataMember]
            public InfixUpgrade infix_upgrade;
            [DataMember]
            public string suffix_item_id;

            public override string ToString()
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();

                sb.AppendLine(
                    String.Format("{0}: {1}-{2}",
                    Config.GetLocalizedName(Data.AttributeType.GetAttributeType(Data.AttributeType.POWER)),
                    min_power, 
                    max_power));


                if (!String.IsNullOrEmpty(defense) && defense != "0")
                    sb.AppendLine(
                        String.Format("{0}: {1}",
                        Config.GetLocalizedName(Data.AttributeType.GetAttributeType(Data.AttributeType.DEFENSE)),
                        defense));

                if (infix_upgrade != null)
                    sb.AppendLine(infix_upgrade.ToString());

                if (this.infusion_slots != null && this.infusion_slots.Count>0)
                {
                    for (int i = 0; i < infusion_slots.Count; i++)
                    {
                        if (infusion_slots[i] != null)
                            sb.AppendLine(infusion_slots[i].ToString());
                    }
                    sb.AppendLine();
                }

                if (!String.IsNullOrEmpty(type))
                    sb.AppendLine(Config.GetLocalizedName(Data.RecipeType.GetRecipeType(type)));

                return sb.ToString();
            }

            public bool ContainsText(string pattern)
            {
                try
                {
                    if (infix_upgrade != null)
                        if (infix_upgrade.ContainsText(pattern))
                        return true;

                    if (infusion_slots != null)
                        foreach (InfusionSlot slot in infusion_slots)
                        if (slot.ContainsText(pattern))
                            return true;

                    return false;
                }
                catch
                {
                    throw;
                }
            }
        }

    }
}