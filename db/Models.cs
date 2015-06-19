#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

#endregion

[Serializable, XmlRoot("Chars")]
public class Chars
{
    private XmlSerializerNamespaces _namespaces;

    public Chars()
    {
        _namespaces = new XmlSerializerNamespaces(new[]
        {
            new XmlQualifiedName(string.Empty, "rotmg")
        });
    }

    [XmlElement("Char")]
    public List<Char> Characters { get; set; }

    [XmlAttribute("nextCharId")]
    public int NextCharId { get; set; }

    [XmlAttribute("maxNumChars")]
    public int MaxNumChars { get; set; }

    public Account Account { get; set; }

    [XmlArray("News")]
    [XmlArrayItem("Item")]
    public List<NewsItem> News { get; set; }

    [XmlArray("Servers")]
    [XmlArrayItem("Server")]
    public List<ServerItem> Servers { get; set; }

    public string OwnedSkins { get; set; }
    [XmlElement("TOSPopup")]
    public string TOSPopup { get; set; }
    public string Lat { get; set; }
    public string Long { get; set; }

    [XmlArray("ClassAvailabilityList")]
    [XmlArrayItem("ClassAvailability")]
    public List<ClassAvailabilityItem> ClassAvailabilityList { get; set; }

    [XmlNamespaceDeclarations]
    public XmlSerializerNamespaces Namespaces
    {
        get { return _namespaces; }
    }

    [XmlArray("ItemCosts")]
    [XmlArrayItem("ItemCost")]
    public List<ItemCostItem> ItemCostsList
    {
        get
        {
            return new List<ItemCostItem>
            {
                new ItemCostItem {Type = "900", Puchasable = 0, Expires = 0, Price = 90000},
                new ItemCostItem {Type = "902", Puchasable = 0, Expires = 0, Price = 90000},
                new ItemCostItem {Type = "834", Puchasable = 1, Expires = 0, Price = 900},
                new ItemCostItem {Type = "835", Puchasable = 1, Expires = 0, Price = 600},
                new ItemCostItem {Type = "836", Puchasable = 1, Expires = 0, Price = 900},
                new ItemCostItem {Type = "837", Puchasable = 1, Expires = 0, Price = 600},
                new ItemCostItem {Type = "838", Puchasable = 1, Expires = 0, Price = 900},
                new ItemCostItem {Type = "839", Puchasable = 1, Expires = 0, Price = 900},
                new ItemCostItem {Type = "840", Puchasable = 1, Expires = 0, Price = 900},
                new ItemCostItem {Type = "841", Puchasable = 1, Expires = 0, Price = 900},
                new ItemCostItem {Type = "842", Puchasable = 1, Expires = 0, Price = 900},
                new ItemCostItem {Type = "843", Puchasable = 1, Expires = 0, Price = 900},
                new ItemCostItem {Type = "844", Puchasable = 1, Expires = 0, Price = 900},
                new ItemCostItem {Type = "845", Puchasable = 1, Expires = 0, Price = 900},
                new ItemCostItem {Type = "846", Puchasable = 1, Expires = 0, Price = 900},
                new ItemCostItem {Type = "847", Puchasable = 1, Expires = 0, Price = 900},
                new ItemCostItem {Type = "848", Puchasable = 1, Expires = 0, Price = 900},
                new ItemCostItem {Type = "849", Puchasable = 1, Expires = 0, Price = 900},
                new ItemCostItem {Type = "850", Puchasable = 0, Expires = 1, Price = 900},
                new ItemCostItem {Type = "851", Puchasable = 0, Expires = 1, Price = 900},
                new ItemCostItem {Type = "852", Puchasable = 1, Expires = 0, Price = 900},
                new ItemCostItem {Type = "853", Puchasable = 1, Expires = 0, Price = 900},
                new ItemCostItem {Type = "854", Puchasable = 1, Expires = 0, Price = 900},
                new ItemCostItem {Type = "855", Puchasable = 1, Expires = 0, Price = 900},
                new ItemCostItem {Type = "856", Puchasable = 0, Expires = 0, Price = 90000},
                new ItemCostItem {Type = "883", Puchasable = 0, Expires = 0, Price = 90000}
            };
        }
    }

    [XmlArray("MaxClassLevelList")]
    [XmlArrayItem("MaxClassLevel")]
    public List<MaxClassLevelItem> MaxClassLevelList
    {
        get
        {
            return new List<MaxClassLevelItem>
            {
                new MaxClassLevelItem {ClassType = "768", MaxLevel = "20"},
                new MaxClassLevelItem {ClassType = "800", MaxLevel = "20"},
                new MaxClassLevelItem {ClassType = "802", MaxLevel = "20"},
                new MaxClassLevelItem {ClassType = "803", MaxLevel = "20"},
                new MaxClassLevelItem {ClassType = "804", MaxLevel = "20"},
                new MaxClassLevelItem {ClassType = "805", MaxLevel = "20"},
                new MaxClassLevelItem {ClassType = "806", MaxLevel = "20"},
                new MaxClassLevelItem {ClassType = "775", MaxLevel = "20"},
                new MaxClassLevelItem {ClassType = "782", MaxLevel = "20"},
                new MaxClassLevelItem {ClassType = "797", MaxLevel = "20"},
                new MaxClassLevelItem {ClassType = "784", MaxLevel = "20"},
                new MaxClassLevelItem {ClassType = "801", MaxLevel = "20"},
                new MaxClassLevelItem {ClassType = "798", MaxLevel = "20"},
                new MaxClassLevelItem {ClassType = "799", MaxLevel = "20"}
            };
        }
    }

    public string SalesForce { get; set; }
}

[Serializable, XmlRoot("Account")]
public class Account
{
    [XmlIgnore]
    public int Rank { get; set; }
    private XmlSerializerNamespaces _namespaces;

    public Account()
    {
        _namespaces = new XmlSerializerNamespaces(new[]
        {
            new XmlQualifiedName(string.Empty, "rotmg")
        });
    }

    //Because the getProdAccount feature, prod has that many accounts that maybe a long isnt enough anymore
    public string AccountId { get; set; }
    public string Name { get; set; }

    [XmlIgnore]
    public bool IsProdAccount { get; set; }

    [XmlElement("NameChosen")]
    public string _NameChosen { get; set; }

    [XmlIgnore]
    public bool NameChosen
    {
        get { return _NameChosen != null; }
        set { _NameChosen = value ? "True" : null; }
    }

    [XmlElement("Converted")]
    public string _Converted { get; set; }

    [XmlIgnore]
    public bool Converted
    {
        get { return _Converted != null; }
        set { _Converted = value ? "True" : null; }
    }

    [XmlElement("Admin")]
    public string _Admin { get; set; }

    [XmlIgnore]
    public bool Admin
    {
        get { return _Admin != null; }
        set { _Admin = value ? "True" : null; }
    }

    [XmlElement("Banned")]
    public string _Banned { get; set; }

    [XmlIgnore]
    public List<int> OwnedSkins { get; set; }

    [XmlIgnore]
    public string Email { get; set; }
    [XmlIgnore]
    public string Password { get; set; }

    [XmlIgnore]
    public bool VisibleMuledump { get; set; }

    [XmlIgnore]
    public bool Banned
    {
        get { return _Banned != null; }
        set { _Banned = value ? "True" : null; }
    }

    [XmlIgnore]
    public string AuthToken { get; set; }
    [XmlIgnore]
    public string NotAcceptedNewTos { get; set; }

    [XmlElement("VerifiedEmail")]
    public string _VerifiedEmail
    {
        get { return VerifiedEmail ? "" : null; }
        set { VerifiedEmail = value == null ? false : true; }
    }

    [XmlIgnore]
    public bool VerifiedEmail { get; set; }

    //[XmlIgnore]
    //public bool VerifiedEmail
    //{
    //    get { return _VerifiedEmail != null; }
    //    set { _VerifiedEmail = value ? "True" : null; }
    //}

    [XmlIgnore] //[XmlElement("StarredAccounts")]
    public string _StarredAccounts { get; set; }

    [XmlIgnore]
    public List<string> Locked
    {
        get
        {
            return (_StarredAccounts != null
                ? _StarredAccounts.Split(',').ToList()
                : new List<string>());
        }
        set { _StarredAccounts = string.Join(",", value); }
    }

    [XmlIgnore] //[XmlElement("IgnoredAccounts")]
    public string _IgnoredAccounts { get; set; }

    [XmlIgnore]
    public List<string> Ignored
    {
        get
        {
            return (_IgnoredAccounts != null
                ? _IgnoredAccounts.Split(',').ToList()
                : new List<string>());
        }
        set { _IgnoredAccounts = string.Join(",", value); }
    }

    [XmlIgnore] //[XmlElement("Commands")]
    public string _Commands { get; set; }

    [XmlIgnore]
    public List<string> Commands
    {
        get { return (_Commands != null ? _Commands.Split(',').ToList() : new List<string>()); }
        set { _Commands = string.Join(",", value); }
    }

    public int Credits { get; set; }
    public int FortuneTokens { get; set; }
    public int NextCharSlotPrice { get; set; }
    public int? BeginnerPackageTimeLeft { get; set; }

    public VaultData Vault { get; set; }
    public Guild Guild { get; set; }

    [XmlNamespaceDeclarations]
    public XmlSerializerNamespaces Namespaces
    {
        get { return _namespaces; }
    }

    [XmlElement("Gifts")]
    public string _Gifts
    {
        get
        {
            if (Gifts == null) Gifts = new List<int>();
            return Utils.GetCommaSepString(Gifts.ToArray());
        }
        set { Gifts = Utils.FromCommaSepString32(value).ToList(); }
    }

    [XmlIgnore]
    public List<int> Gifts { get; set; }

    [XmlElement]
    public string Originating
    {
        get { return "None"; }
    }

    [XmlElement("cleanPasswordStatus")]
    public int CleanPasswordStatus
    {
        get { return 1; }
    }

    public int PetYardType { get; set; }
    public int ArenaTickets { get; set; }
    public QuestItem DailyQuest { get; set; }
    public int IsAgeVerified { get; set; }
    public Stats Stats { get; set; }

    [XmlIgnore]
    public bool IsGuestAccount { get; set; }
    [XmlIgnore]
    public int DailyQuestId { get; set; }

    [XmlIgnore]
    internal List<string> GiftCodes { get; set; }

    public string NextGiftCode()
    {
        if (GiftCodes.Count <= 0) return null;
        var gc = GiftCodes[0];
        GiftCodes.Remove(gc);
        return gc;
    }
}

public class IP
{
    public string Address { get; set; }
    public bool Banned { get; set; }

    public override string ToString()
    {
        return Address;
    }
}

[Serializable, XmlRoot]
public class VaultData
{
    [XmlElement("Chest")]
    public List<VaultChest> Chests { get; set; }
}

public class VaultChest
{
    [XmlIgnore]
    public int ChestId { get; set; }

    [XmlText]
    public string _Items { get; set; }

    [XmlIgnore]
    public int[] Items
    {
        get { return Utils.FromCommaSepString32(_Items); }
        set { _Items = Utils.GetCommaSepString(value); }
    }
}

[Serializable, XmlRoot]
public class Stats
{
    [XmlElement("ClassStats")]
    public List<ClassStats> ClassStates { get; set; }

    public int BestCharFame { get; set; }
    public int TotalFame { get; set; }
    public int Fame { get; set; }
}

[Serializable, XmlRoot]
public class Guild
{
    [XmlAttribute("id")]
    public long Id { get; set; }

    public int Rank { get; set; }
    public string Name { get; set; }

    public int Fame { get; set; }
}

[Serializable, XmlRoot]
public class GuildStruct
{
    [XmlAttribute("id")]
    public int Id { get; set; }

    public string Name { get; set; }
    public int Level { get; set; }
    public string[] Members { get; set; }
    public int GuildFame { get; set; }
    public int TotalGuildFame { get; set; }
}

[Serializable, XmlRoot]
public class ClassStats
{
    [XmlAttribute("objectType")]
    public string ObjectType { get; set; }

    public int BestLevel { get; set; }
    public int BestFame { get; set; }
}

[Serializable, XmlRoot("Item")]
public class NewsItem
{
    public string Icon { get; set; }
    public string Title { get; set; }
    public string TagLine { get; set; }
    public string Link { get; set; }
    public int Date { get; set; }
}

[Serializable, XmlRoot("Server")]
public class ServerItem
{
    public string Name { get; set; }
    public string DNS { get; set; }
    public double Lat { get; set; }
    public double Long { get; set; }
    public double Usage { get; set; }
    public int RankRequired { get; set; }

    [XmlElement("AdminOnly")]
    private string _AdminOnly { get; set; }

    [XmlIgnore]
    public bool AdminOnly
    {
        get { return _AdminOnly != null; }
        set { _AdminOnly = value ? "True" : null; }
    }
}

[Serializable, XmlRoot("Char")]
public class Char
{
    [XmlAttribute("id")]
    public int CharacterId { get; set; }

    public int ObjectType { get; set; }
    public int Level { get; set; }
    public int Exp { get; set; }
    public int CurrentFame { get; set; }
    public int HealthStackCount { get; set; }
    public int MagicStackCount { get; set; }

    [XmlElement("Equipment")]
    public string _Equipment { get; set; }

    [XmlIgnore]
    public int[] Equipment
    {
        get { return Utils.FromCommaSepString32(_Equipment); }
        set { _Equipment = Utils.GetCommaSepString(value); }
    }

    [XmlIgnore]
    public int[] Backpack { get; set; }

    public int HasBackpack { get; set; }
    public int MaxHitPoints { get; set; }
    public int HitPoints { get; set; }
    public int MaxMagicPoints { get; set; }
    public int MagicPoints { get; set; }
    public int Attack { get; set; }
    public int Defense { get; set; }
    public int Speed { get; set; }
    public int Dexterity { get; set; }
    public int HpRegen { get; set; }
    public int MpRegen { get; set; }
    public int Tex1 { get; set; }
    public int Tex2 { get; set; }
    public bool XpBoosted { get; set; }
    public int XpTimer { get; set; }
    public int LDTimer { get; set; }
    public int LTTimer { get; set; }
    public string PCStats { get; set; }

    [XmlElement("casToken")]
    public string CasToken { get; set; }

    [XmlElement("Texture")]
    public int Skin { get; set; }

    [XmlIgnore]
    public FameStats FameStats { get; set; }

    public bool Dead { get; set; }

    public PetItem Pet { get; set; }
}

[Serializable, XmlRoot]
public class PetItem
{
    [XmlAttribute("name")]
    public string SkinName { get; set; }

    [XmlAttribute("type")]
    public int Type { get; set; }

    [XmlAttribute("instanceId")]
    public int InstanceId { get; set; }

    [XmlAttribute("maxAbilityPower")]
    public int MaxAbilityPower { get; set; }

    [XmlAttribute("skin")]
    public int Skin { get; set; }

    [XmlAttribute("rarity")]
    public int Rarity { get; set; }

    [XmlArray("Abilities")]
    [XmlArrayItem("Ability")]
    public List<AbilityItem> Abilities { get; set; }
}

[Serializable]
public class AbilityItem
{
    [XmlAttribute("type")]
    public int Type { get; set; }

    [XmlAttribute("power")]
    public int Power { get; set; }

    [XmlAttribute("points")]
    public int Points { get; set; }
}

[Serializable]
public class MaxClassLevelItem
{
    [XmlAttribute("classType")]
    public string ClassType { get; set; }

    [XmlAttribute("maxLevel")]
    public string MaxLevel { get; set; }
}

[Serializable]
public class ItemCostItem
{
    //<ItemCost type="854" purchasable="1" expires="0">900</ItemCost>
    [XmlAttribute("type")]
    public string Type { get; set; }

    [XmlAttribute("purchasable")]
    public int Puchasable { get; set; }

    [XmlAttribute("expires")]
    public int Expires { get; set; }

    [XmlText]
    public int Price { get; set; }
}

[Serializable]
public class ClassAvailabilityItem
{
    [XmlAttribute("id")]
    public string Class { get; set; }

    [XmlText]
    public string Restricted { get; set; }
}

[Serializable]
public class QuestItem
{
    [XmlAttribute("tier")]
    public int Tier { get; set; }
    [XmlAttribute("goal")]
    public string Goal { get; set; }
    public string Description { get; set; }
    public string Image { get; set; }

    [XmlIgnore]
    public int Id { get; set; }
    [XmlIgnore]
    public DateTime Time { get; set; }
}