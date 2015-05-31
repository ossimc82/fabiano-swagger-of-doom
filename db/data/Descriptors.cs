#region

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

#endregion

public enum Rarity : uint
{
    Common,
    Uncommon,
    Rare,
    Legendary,
    Divine
}

public enum Family : uint
{
    Aquatic,
    Automaton,
    Avian,
    Canine,
    Exotic,
    Farm,
    Feline,
    Humanoid,
    Insect,
    Penguin,
    Reptile,
    Spooky,
    Unknown, //this is the ? ? ? ? family
    Woodland
}

public enum Ability : uint
{
    AttackClose = 402,
    AttackMid = 404,
    AttackFar = 405,
    Electric = 406,
    Heal = 407,
    MagicHeal = 408,
    Savage = 409,
    Decoy = 410,
    RisingFury = 411,
}

[Flags]
public enum ConditionEffects : ulong
{
    Dead = 1 << 0,
    Quiet = 1 << 1,
    Weak = 1 << 2,
    Slowed = 1 << 3,
    Sick = 1 << 4,
    Dazed = 1 << 5,
    Stunned = 1 << 6,
    Blind = 1 << 7,
    Hallucinating = 1 << 8,
    Drunk = 1 << 9,
    Confused = 1 << 10,
    StunImmume = 1 << 11,
    Invisible = 1 << 12,
    Paralyzed = 1 << 13,
    Speedy = 1 << 14,
    Bleeding = 1 << 15,
    ArmorBreakImmune = 1 << 16,
    Healing = 1 << 17,
    Damaging = 1 << 18,
    Berserk = 1 << 19,
    Paused = 1 << 20,
    Stasis = 1 << 21,
    StasisImmune = 1 << 22,
    Invincible = 1 << 23,
    Invulnerable = 1 << 24,
    Armored = 1 << 25,
    ArmorBroken = 1 << 26,
    Hexed = 1 << 27,
    NinjaSpeedy = 1 << 28,
    Unstable = 1 << 29,
    Darkness = 1 << 30,
    SlowedImmune = (ulong)1 << 31,
    DazedImmune = (ulong)1 << 32,
    ParalyzeImmune = (ulong)1 << 33,
    Petrify = (ulong)1 << 34,
    PetrifyImmune = (ulong)1 << 35,
    PetDisable = (ulong)1 << 36,
    Curse = (ulong)1 << 37,
    CurseImmune = (ulong)1 << 38,
    HPBoost = (ulong)1 << 39,
    MPBoost = (ulong)1 << 40,
    AttBoost = (ulong)1 << 41,
    DefBoost = (ulong)1 << 42,
    SpdBoost = (ulong)1 << 43,
    VitBoost = (ulong)1 << 44,
    WisBoost = (ulong)1 << 45,
    DexBoost = (ulong)1 << 46
}

public enum ConditionEffectIndex
{
    Dead = 0,
    Quiet = 1,
    Weak = 2,
    Slowed = 3,
    Sick = 4,
    Dazed = 5,
    Stunned = 6,
    Blind = 7,
    Hallucinating = 8,
    Drunk = 9,
    Confused = 10,
    StunImmune = 11,
    Invisible = 12,
    Paralyzed = 13,
    Speedy = 14,
    Bleeding = 15,
    ArmorBreakImmune = 16,
    Healing = 17,
    Damaging = 18,
    Berserk = 19,
    Paused = 20,
    Stasis = 21,
    StasisImmune = 22,
    Invincible = 23,
    Invulnerable = 24,
    Armored = 25,
    ArmorBroken = 26,
    Hexed = 27,
    NinjaSpeedy = 28,
    Unstable = 29,
    Darkness = 30,
    SlowedImmune = 31,
    DazedImmune = 32,
    ParalyzeImmune = 33,
    Petrify = 34,
    PetrifyImmune = 35,
    PetDisable = 36,
    Curse = 37,
    CurseImmune = 38,
    HPBoost = 39,
    MPBoost = 40,
    AttBoost = 41,
    DefBoost = 42,
    SpdBoost = 43,
    VitBoost = 44,
    WisBoost = 45,
    DexBoost = 46
}

public class PetStruct
{
    public PetStruct(ushort type, XElement elem)
    {
        ObjectId = elem.Attribute("id").Value;
        ObjectType = type;
        if (elem.Element("Family").Value == "? ? ? ?")
            PetFamily = Family.Unknown;
        else
            PetFamily = (Family)Enum.Parse(typeof(Family), elem.Element("Family").Value);
        PetRarity = (Rarity)Enum.Parse(typeof(Rarity), elem.Element("Rarity").Value);
        if(elem.Element("FirstAbility") != null)
            FirstAbility = (Ability)Enum.Parse(typeof(Ability), elem.Element("FirstAbility").Value.Replace(" ", String.Empty));
        DefaultSkin = elem.Element("DefaultSkin").Value;
        Size = int.Parse(elem.Element("Size").Value);
        DisplayId = elem.Element("DisplayId").Value;
    }

    public string ObjectId { get; private set; }
    public ushort ObjectType { get; private set; }
    public string DisplayId { get; private set; }
    public Family PetFamily { get; private set; }
    public Rarity PetRarity { get; private set; }
    public Ability? FirstAbility { get; private set; }
    public string DefaultSkin { get; private set; }
    public int Size { get; private set; }
}

public class PetSkin
{
    public PetSkin(ushort type, XElement elem)
    {
        ObjectId = elem.Attribute("id").Value;
        ObjectType = type;
        DisplayId = elem.Element("DisplayId").Value;
    }

    public string ObjectId { get; private set; }
    public ushort ObjectType { get; private set; }
    public string DisplayId { get; private set; }
}

public class ConditionEffect
{
    public ConditionEffect()
    {
    }

    public ConditionEffect(XElement elem)
    {
        CultureInfo ci = (CultureInfo) CultureInfo.CurrentCulture.Clone();
        ci.NumberFormat.CurrencyDecimalSeparator = ".";
        Effect = (ConditionEffectIndex) Enum.Parse(typeof (ConditionEffectIndex), elem.Value.Replace(" ", ""));
        if (elem.Attribute("duration") != null)
            DurationMS = (int) (float.Parse(elem.Attribute("duration").Value, NumberStyles.Any, ci)*1000);
        if (elem.Attribute("range") != null)
            Range = float.Parse(elem.Attribute("range").Value, NumberStyles.Any, ci);
        if(elem.Attribute("target") != null)
            Target = int.Parse(elem.Attribute("target").Value, NumberStyles.Any, ci);
    }

    public ConditionEffectIndex Effect { get; set; }
    public int DurationMS { get; set; }
    public int Target { get; set; }
    public float Range { get; set; }
}

public class ProjectileDesc
{
    public ProjectileDesc(XElement elem)
    {
        CultureInfo ci = (CultureInfo) CultureInfo.CurrentCulture.Clone();
        ci.NumberFormat.CurrencyDecimalSeparator = ".";
        XElement n;
        if (elem.Attribute("id") != null)
            BulletType = Utils.FromString(elem.Attribute("id").Value);
        ObjectId = elem.Element("ObjectId").Value;
        LifetimeMS = Utils.FromString(elem.Element("LifetimeMS").Value);
        Speed = float.Parse(elem.Element("Speed").Value, NumberStyles.Any, ci);
        if ((n = elem.Element("Size")) != null)
            Size = Utils.FromString(n.Value);

        XElement dmg = elem.Element("Damage");
        if (dmg != null)
            MinDamage = MaxDamage = Utils.FromString(dmg.Value);
        else
        {
            MinDamage = Utils.FromString(elem.Element("MinDamage").Value);
            MaxDamage = Utils.FromString(elem.Element("MaxDamage").Value);
        }

        Effects = elem.Elements("ConditionEffect").Select(i => new ConditionEffect(i)).ToArray();

        MultiHit = elem.Element("MultiHit") != null;
        PassesCover = elem.Element("PassesCover") != null;
        ArmorPiercing = elem.Element("ArmorPiercing") != null;
        ParticleTrail = elem.Element("ParticleTrail") != null;
        Wavy = elem.Element("Wavy") != null;
        Parametric = elem.Element("Parametric") != null;
        Boomerang = elem.Element("Boomerang") != null;

        n = elem.Element("Amplitude");
        Amplitude = n != null ? float.Parse(n.Value, NumberStyles.Any, ci) : 0;
        n = elem.Element("Frequency");
        Frequency = n != null ? float.Parse(n.Value, NumberStyles.Any, ci) : 1;
        n = elem.Element("Magnitude");
        Magnitude = n != null ? float.Parse(n.Value, NumberStyles.Any, ci) : 3; 
    }

    public int BulletType { get; private set; }
    public string ObjectId { get; private set; }
    public int LifetimeMS { get; private set; }
    public float Speed { get; private set; }
    public int Size { get; private set; }
    public int MinDamage { get; private set; }
    public int MaxDamage { get; private set; }

    public ConditionEffect[] Effects { get; private set; }

    public bool MultiHit { get; private set; }
    public bool PassesCover { get; private set; }
    public bool ArmorPiercing { get; private set; }
    public bool ParticleTrail { get; private set; }
    public bool Wavy { get; private set; }
    public bool Parametric { get; private set; }
    public bool Boomerang { get; private set; }

    public float Amplitude { get; private set; }
    public float Frequency { get; private set; }
    public float Magnitude { get; private set; }
}

public enum ActivateEffects
{
    Shoot,
    StatBoostSelf,
    StatBoostAura,
    BulletNova,
    ConditionEffectAura,
    ConditionEffectSelf,
    Heal,
    HealNova,
    Magic,
    MagicNova,
    Teleport,
    VampireBlast,
    Trap,
    StasisBlast,
    Decoy,
    Lightning,
    PoisonGrenade,
    RemoveNegativeConditions,
    RemoveNegativeConditionsSelf,
    IncrementStat,
    Pet,
    PermaPet,
    Create,
    UnlockPortal,
    DazeBlast,
    ClearConditionEffectAura,
    ClearConditionEffectSelf,
    Dye,
    CreatePet,
    ShurikenAbility,
    UnlockSkin,
    MysteryPortal,
    GenericActivate
}

public class ActivateEffect
{
    public ActivateEffect(XElement elem)
    {
        CultureInfo ci = (CultureInfo) CultureInfo.CurrentCulture.Clone();
        ci.NumberFormat.CurrencyDecimalSeparator = ".";
        Effect = (ActivateEffects) Enum.Parse(typeof (ActivateEffects), elem.Value);
        if (elem.Attribute("stat") != null)
            Stats = Utils.FromString(elem.Attribute("stat").Value);

        if (elem.Attribute("amount") != null)
            Amount = Utils.FromString(elem.Attribute("amount").Value);

        if (elem.Attribute("range") != null)
            Range = float.Parse(elem.Attribute("range").Value, NumberStyles.Any, ci);
        if (elem.Attribute("duration") != null)
        {
            DurationSec = float.Parse(elem.Attribute("duration").Value, NumberStyles.Any, ci);
            DurationMS = (int)(DurationSec * 1000);
        }
        if (elem.Attribute("duration2") != null)
            DurationMS2 = (int) (float.Parse(elem.Attribute("duration2").Value, NumberStyles.Any, ci)*1000);
        if (elem.Attribute("effect") != null)
            ConditionEffect =
                (ConditionEffectIndex) Enum.Parse(typeof (ConditionEffectIndex), elem.Attribute("effect").Value);
        if (elem.Attribute("condEffect") != null)
            ConditionEffect =
                (ConditionEffectIndex) Enum.Parse(typeof (ConditionEffectIndex), elem.Attribute("condEffect").Value);
        if (elem.Attribute("condDuration") != null)
            EffectDuration = float.Parse(elem.Attribute("condDuration").Value, NumberStyles.Any, ci);

        if (elem.Attribute("maxDistance") != null)
            MaximumDistance = Utils.FromString(elem.Attribute("maxDistance").Value);

        if (elem.Attribute("radius") != null)
            Radius = float.Parse(elem.Attribute("radius").Value, NumberStyles.Any, ci);

        if (elem.Attribute("totalDamage") != null)
            TotalDamage = Utils.FromString(elem.Attribute("totalDamage").Value);

        if (elem.Attribute("objectId") != null)
            ObjectId = elem.Attribute("objectId").Value;

        if (elem.Attribute("angleOffset") != null)
            AngleOffset = Utils.FromString(elem.Attribute("angleOffset").Value);

        if (elem.Attribute("maxTargets") != null)
            MaxTargets = Utils.FromString(elem.Attribute("maxTargets").Value);

        if (elem.Attribute("id") != null)
            Id = elem.Attribute("id").Value;

        if (elem.Attribute("dungeonName") != null)
            DungeonName = elem.Attribute("dungeonName").Value;

        if (elem.Attribute("skinType") != null)
            SkinType = int.Parse(elem.Attribute("skinType").Value);

        if (elem.Attribute("lockedName") != null)
            LockedName = elem.Attribute("lockedName").Value;

        if (elem.Attribute("color") != null)
            Color = uint.Parse(elem.Attribute("color").Value.Substring(2), NumberStyles.AllowHexSpecifier);
        if (elem.Attribute("target") != null)
            Target = elem.Attribute("target").Value;
        UseWisMod = elem.Attribute("useWisMod") != null;
        if (elem.Attribute("visualEffect") != null)
            VisualEffect = float.Parse(elem.Attribute("visualEffect").Value, NumberStyles.Any, ci);
        if (elem.Attribute("center") != null)
            Center = elem.Attribute("center").Value;
    }

    public ActivateEffects Effect { get; private set; }
    public int Stats { get; private set; }
    public int Amount { get; private set; }
    public float Range { get; private set; }
    public float DurationSec { get; private set; }
    public int DurationMS { get; private set; }
    public int DurationMS2 { get; private set; }
    public ConditionEffectIndex? ConditionEffect { get; private set; }
    public float EffectDuration { get; private set; }
    public int MaximumDistance { get; private set; }
    public float Radius { get; private set; }
    public int TotalDamage { get; private set; }
    public string ObjectId { get; private set; }
    public int AngleOffset { get; private set; }
    public int MaxTargets { get; private set; }
    public string Id { get; private set; }
    public int SkinType { get; private set; }
    public string DungeonName { get; private set; }
    public string LockedName { get; private set; }
    public string Target { get; private set; }
    public string Center { get; private set; }
    public bool UseWisMod { get; private set; }
    public float VisualEffect { get; private set; }
    public uint? Color { get; private set; }
}

public class PortalDesc
{
    public PortalDesc(ushort type, XElement elem)
    {
        XElement n;
        ObjectType = type;
        ObjectId = elem.Attribute("id").Value;
        DisplayId = elem.Element("DisplayId") != null ? elem.Element("DisplayId").Value : String.Empty;
        if ((n = elem.Element("NexusPortal")) != null) //<NexusPortal/>
        {
            NexusPortal = true;
        }
        if ((n = elem.Element("DungeonName")) != null) //<NexusPortal/>
        {
            DungeonName = elem.Element("DungeonName").Value;
        }
        TimeoutTime = ObjectId == "The Shatters" ? 70 : 30;
    }

    public ushort ObjectType { get; private set; }
    public string DisplayId { get; private set; }
    public string ObjectId { get; }
    public string DungeonName { get; set; }
    public int TimeoutTime { get; private set; }
    public bool NexusPortal { get; private set; }
}

public class Item : IFeedable
{
    private const bool DISABLE_SOULBOUND_UT = false;

    public Item(ushort type, XElement elem)
    {
        try
        {
            CultureInfo ci = (CultureInfo) CultureInfo.CurrentCulture.Clone();
            ci.NumberFormat.CurrencyDecimalSeparator = ".";
            XElement n;
            ObjectType = type;
            ObjectId = elem.Attribute(XName.Get("id")).Value;
            SetType = elem.Attribute("setType") != null ? Utils.FromString(elem.Attribute("setType").Value) : -1;
            SlotType = Utils.FromString(elem.Element("SlotType").Value);
            if ((n = elem.Element("Tier")) != null)
                try
                {
                    Tier = Utils.FromString(n.Value);
                }
                catch
                {
                    Tier = -1;
                }
            else
                Tier = -1;
            Description = elem.Element("Description").Value;
            RateOfFire = (n = elem.Element("RateOfFire")) != null ? float.Parse(n.Value, NumberStyles.Any, ci) : 1;
            Usable = elem.Element("Usable") != null;
            BagType = (n = elem.Element("BagType")) != null ? Utils.FromString(n.Value) : 0;
            MpCost = (n = elem.Element("MpCost")) != null ? Utils.FromString(n.Value) : 0;
            FeedPower = (n = elem.Element("feedPower")) != null ? (ushort) Utils.FromString(n.Value) : (ushort) 0;
            FameBonus = (n = elem.Element("FameBonus")) != null ? Utils.FromString(n.Value) : 0;
            NumProjectiles = (n = elem.Element("NumProjectiles")) != null ? Utils.FromString(n.Value) : 1;
            ArcGap = (n = elem.Element("ArcGap")) != null ? Utils.FromString(n.Value) : 11.25f;
            Consumable = elem.Element("Consumable") != null;
            Potion = elem.Element("Potion") != null;
            DisplayId = (n = elem.Element("DisplayId")) != null ? n.Value : null;
            Doses = (n = elem.Element("Doses")) != null ? Utils.FromString(n.Value) : 0;
            SuccessorId = (n = elem.Element("SuccessorId")) != null ? n.Value : null;
            if (elem.Element("Soulbound") != null)
            {
                var s = Utils.FromString(elem.Element("SlotType").Value);
                if (s == 10 || s == 26 || elem.Element("ActivateOnEquip") == null || !DISABLE_SOULBOUND_UT)
                    Soulbound = true;
                else Soulbound = false;
            }
            Secret = elem.Element("Secret") != null;
            IsBackpack = elem.Element("Backpack") != null;
            Cooldown = (n = elem.Element("Cooldown")) != null ? float.Parse(n.Value, NumberStyles.Any, ci) : 0;
            Resurrects = elem.Element("Resurrects") != null;
            Texture1 = (n = elem.Element("Tex1")) != null ? Convert.ToInt32(n.Value, 16) : 0;
            Texture2 = (n = elem.Element("Tex2")) != null ? Convert.ToInt32(n.Value, 16) : 0;
            Class = elem.Element("Class").Value;
            Family = (n = elem.Element("PetFamily")) != null
                ? n.Value == "? ? ? ?"
                    ? (Family)Enum.Parse(typeof(Family), "Unknown", true)
                    : (Family)Enum.Parse(typeof(Family), n.Value, true)
                : null as Family?;
            Rarity = (n = elem.Element("Rarity")) != null
                ? (Rarity)Enum.Parse(typeof(Rarity), n.Value, true)
                : null as Rarity?;

            StatsBoost =
                elem.Elements("ActivateOnEquip")
                    .Select(
                        i =>
                            new KeyValuePair<int, int>(int.Parse(i.Attribute("stat").Value),
                                int.Parse(i.Attribute("amount").Value)))
                    .ToArray();

            ActivateEffects = elem.Elements("Activate").Select(i => new ActivateEffect(i)).ToArray();
            Projectiles = elem.Elements("Projectile").Select(i => new ProjectileDesc(i)).ToArray();
            MpEndCost = (n = elem.Element("MpEndCost")) != null ? int.Parse(elem.Element("MpEndCost").Value) : null as int?;
            Timer = (n = elem.Element("Timer")) != null ? float.Parse(elem.Element("Timer").Value) : null as float?;
            XpBooster = elem.Element("XpBoost") != null;
            LootDropBooster = elem.Element("LDBoosted") != null;
            LootTierBooster = elem.Element("LTBoosted") != null;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            Console.ReadLine();
        }
    }

    public ushort ObjectType { get; private set; }
    public string ObjectId { get; private set; }
    public int SlotType { get; private set; }
    public ushort FeedPower { get; set; }
    public int Tier { get; private set; }
    public string Description { get; private set; }
    public float RateOfFire { get; private set; }
    public bool Usable { get; private set; }
    public int BagType { get; private set; }
    public int MpCost { get; private set; }
    public int FameBonus { get; private set; }
    public int NumProjectiles { get; private set; }
    public float ArcGap { get; private set; }
    public bool Consumable { get; private set; }
    public bool Potion { get; private set; }
    public string DisplayId { get; private set; }
    public string SuccessorId { get; private set; }
    public bool Soulbound { get; private set; }
    public float Cooldown { get; private set; }
    public bool Resurrects { get; private set; }
    public int Texture1 { get; private set; }
    public int Texture2 { get; private set; }
    public bool Secret { get; private set; }
    public bool IsBackpack { get; private set; }
    public Rarity? Rarity { get; private set; }
    public Family? Family { get; private set; }
    public string Class { get; private set; }

    public int Doses { get; set; }

    public KeyValuePair<int, int>[] StatsBoost { get; private set; }
    public ActivateEffect[] ActivateEffects { get; private set; }
    public ProjectileDesc[] Projectiles { get; private set; }

    public int? MpEndCost { get; private set; }
    public float? Timer { get; private set; }
    public bool XpBooster { get; private set; }
    public bool LootDropBooster { get; private set; }
    public bool LootTierBooster { get; private set; }
    public int SetType { get; private set; }
}

public class SpawnCount
{
    public SpawnCount(XElement elem)
    {
        Mean = Utils.FromString(elem.Element("Mean").Value);
        StdDev = Utils.FromString(elem.Element("StdDev").Value);
        Min = Utils.FromString(elem.Element("Min").Value);
        Max = Utils.FromString(elem.Element("Max").Value);
    }

    public int Mean { get; private set; }
    public int StdDev { get; private set; }
    public int Min { get; private set; }
    public int Max { get; private set; }
}

public class ObjectDesc
{
    public ObjectDesc(ushort type, XElement elem)
    {
        CultureInfo ci = (CultureInfo) CultureInfo.CurrentCulture.Clone();
        ci.NumberFormat.CurrencyDecimalSeparator = ".";
        XElement n;
        ObjectType = type;
        ObjectId = elem.Attribute("id").Value;
        XElement xElement = elem.Element("Class");
        if (xElement != null) Class = xElement.Value;
        Group = (n = elem.Element("Group")) != null ? n.Value : null;
        DisplayId = (n = elem.Element("DisplayId")) != null ? n.Value : null;
        Player = elem.Element("Player") != null;
        Enemy = elem.Element("Enemy") != null;
        OccupySquare = elem.Element("OccupySquare") != null;
        FullOccupy = elem.Element("FullOccupy") != null;
        EnemyOccupySquare = elem.Element("EnemyOccupySquare") != null;
        Static = elem.Element("Static") != null;
        NoMiniMap = elem.Element("NoMiniMap") != null;
        ProtectFromGroundDamage = elem.Element("ProtectFromGroundDamage") != null;
        ProtectFromSink = elem.Element("ProtectFromSink") != null;
        Flying = elem.Element("Flying") != null;
        ShowName = elem.Element("ShowName") != null;
        DontFaceAttacks = elem.Element("DontFaceAttacks") != null;
        BlocksSight = elem.Element("BlocksSight") != null;

        if ((n = elem.Element("Size")) != null)
        {
            MinSize = MaxSize = Utils.FromString(n.Value);
            SizeStep = 0;
        }
        else
        {
            MinSize = (n = elem.Element("MinSize")) != null ? Utils.FromString(n.Value) : 100;
            MaxSize = (n = elem.Element("MaxSize")) != null ? Utils.FromString(n.Value) : 100;
            SizeStep = (n = elem.Element("SizeStep")) != null ? Utils.FromString(n.Value) : 0;
        }

        Projectiles = elem.Elements("Projectile").Select(i => new ProjectileDesc(i)).ToArray();

        if ((n = elem.Element("UnlockCost")) != null)
            UnlockCost = int.Parse(n.Value);
        if ((n = elem.Element("MaxHitPoints")) != null)
        {
            MaxHitPoints = n.Attribute("max") != null ? Utils.FromString(n.Attribute("max").Value) : -1;
            MaxHP = Utils.FromString(n.Value);
        }
        if ((n = elem.Element("MaxMagicPoints")) != null)
            MaxMagicPoints = n.Attribute("max") != null ? Utils.FromString(n.Attribute("max").Value) : -1;

        if ((n = elem.Element("Attack")) != null)
            MaxAttack = n.Attribute("max") != null ? Utils.FromString(n.Attribute("max").Value) : -1;
        if ((n = elem.Element("Dexterity")) != null)
            MaxDexterity = n.Attribute("max") != null ? Utils.FromString(n.Attribute("max").Value) : -1;
        if ((n = elem.Element("Speed")) != null)
            MaxSpeed = n.Attribute("max") != null ? Utils.FromString(n.Attribute("max").Value) : -1;
        if ((n = elem.Element("HpRegen")) != null)
            MaxHpRegen = n.Attribute("max") != null ? Utils.FromString(n.Attribute("max").Value) : -1;
        if ((n = elem.Element("MpRegen")) != null)
            MaxMpRegen = n.Attribute("max") != null ? Utils.FromString(n.Attribute("max").Value) : -1;

        if ((n = elem.Element("Defense")) != null)
        {
            Defense = Utils.FromString(n.Value);
            MaxDefense = n.Attribute("max") != null ? Utils.FromString(n.Attribute("max").Value) : -1;
        }
        if ((n = elem.Element("Terrain")) != null)
            Terrain = n.Value;
        if ((n = elem.Element("SpawnProbability")) != null)
            SpawnProbability = float.Parse(n.Value, NumberStyles.Any, ci);
        if ((n = elem.Element("Spawn")) != null)
            Spawn = new SpawnCount(n);

        God = elem.Element("God") != null;
        Cube = elem.Element("Cube") != null;
        Quest = elem.Element("Quest") != null;
        if ((n = elem.Element("Level")) != null)
            Level = Utils.FromString(n.Value);
        else
            Level = null;

        Tags = new TagList();
        if (elem.Elements("Tag").Any())
            foreach (XElement i in elem.Elements("Tag"))
                Tags.Add(new Tag(i));

        StasisImmune = elem.Element("StasisImmune") != null;
        StunImmune = elem.Element("StunImmune") != null;
        ParalyzedImmune = elem.Element("ParalyzeImmune") != null;
        DazedImmune = elem.Element("DazedImmune") != null;
        Oryx = elem.Element("Oryx") != null;
        Hero = elem.Element("Hero") != null;

        if ((n = elem.Element("PerRealmMax")) != null)
            PerRealmMax = Utils.FromString(n.Value);
        else
            PerRealmMax = null;
        if ((n = elem.Element("XpMult")) != null)
            ExpMultiplier = float.Parse(n.Value, NumberStyles.Any, ci);
        else
            ExpMultiplier = null;
    }

    public int UnlockCost { get; private set; }
    public int MaxHitPoints { get; private set; }
    public int MaxMagicPoints { get; private set; }
    public int MaxAttack { get; private set; }
    public int MaxDefense { get; private set; }
    public int MaxSpeed { get; private set; }
    public int MaxDexterity { get; private set; }
    public int MaxHpRegen { get; private set; }
    public int MaxMpRegen { get; private set; }

    public ushort ObjectType { get; private set; }
    public string ObjectId { get; private set; }
    public string DisplayId { get; private set; }
    public string Group { get; private set; }
    public string Class { get; private set; }
    public bool Player { get; private set; }
    public bool Enemy { get; private set; }
    public bool OccupySquare { get; private set; }
    public bool FullOccupy { get; private set; }
    public bool EnemyOccupySquare { get; private set; }
    public bool Static { get; private set; }
    public bool NoMiniMap { get; private set; }
    public bool ProtectFromGroundDamage { get; private set; }
    public bool ProtectFromSink { get; private set; }
    public bool Flying { get; private set; }
    public bool ShowName { get; private set; }
    public bool DontFaceAttacks { get; private set; }
    public bool BlocksSight { get; private set; }
    public int MinSize { get; private set; }
    public int MaxSize { get; private set; }
    public int SizeStep { get; private set; }
    public TagList Tags { get; private set; }
    public ProjectileDesc[] Projectiles { get; private set; }


    public double MaxHP { get; private set; }
    public int Defense { get; private set; }
    public string Terrain { get; private set; }
    public float SpawnProbability { get; private set; }
    public SpawnCount Spawn { get; private set; }
    public bool Cube { get; private set; }
    public bool God { get; private set; }
    public bool Quest { get; private set; }
    public int? Level { get; private set; }
    public bool StasisImmune { get; private set; }
    public bool StunImmune { get; private set; }
    public bool ParalyzedImmune { get; private set; }
    public bool DazedImmune { get; private set; }
    public bool Oryx { get; private set; }
    public bool Hero { get; private set; }
    public int? PerRealmMax { get; private set; }
    public float? ExpMultiplier { get; private set; } //Exp gained = level total / 10 * multi
}

public class TagList : List<Tag>
{
    public bool ContainsTag(string name)
    {
        return this.Any(i => i.Name == name);
    }

    public string TagValue(string name, string value)
    {
        return
            (from i in this where i.Name == name where i.Values.ContainsKey(value) select i.Values[value])
                .FirstOrDefault();
    }
}

public class Tag
{
    public Tag(XElement elem)
    {
        Name = elem.Attribute("name").Value;
        Values = new Dictionary<string, string>();
        foreach (XElement i in elem.Elements())
        {
            if (Values.ContainsKey(i.Name.ToString()))
                Values.Remove(i.Name.ToString());
            Values.Add(i.Name.ToString(), i.Value);
        }
    }

    public string Name { get; private set; }
    public Dictionary<string, string> Values { get; private set; }
}

public class TileDesc
{
    public TileDesc(ushort type, XElement elem)
    {
        CultureInfo ci = (CultureInfo) CultureInfo.CurrentCulture.Clone();
        ci.NumberFormat.CurrencyDecimalSeparator = ".";
        XElement n;
        ObjectType = type;
        ObjectId = elem.Attribute("id").Value;
        NoWalk = elem.Element("NoWalk") != null;
        if ((n = elem.Element("MinDamage")) != null)
        {
            MinDamage = Utils.FromString(n.Value);
            Damaging = true;
        }
        if ((n = elem.Element("MaxDamage")) != null)
        {
            MaxDamage = Utils.FromString(n.Value);
            Damaging = true;
        }
        if ((n = elem.Element("Speed")) != null)
            Speed = float.Parse(n.Value.StartsWith(".") ? "0" + n.Value : n.Value);
        Push = elem.Element("Push") != null;
        if (Push)
        {
            XElement anim = elem.Element("Animate");
            if (anim.Attribute("dx") != null)
                PushX = float.Parse(anim.Attribute("dx").Value, NumberStyles.Any, ci);
            if (elem.Attribute("dy") != null)
                PushY = float.Parse(anim.Attribute("dy").Value, NumberStyles.Any, ci);
        }
    }

    public ushort ObjectType { get; private set; }
    public string ObjectId { get; private set; }
    public bool NoWalk { get; private set; }
    public bool Damaging { get; private set; }
    public int MinDamage { get; private set; }
    public int MaxDamage { get; private set; }
    public float Speed { get; private set; }
    public bool Push { get; private set; }
    public float PushX { get; private set; }
    public float PushY { get; private set; }
}

public class DungeonDesc
{
    public DungeonDesc(XElement elem)
    {
        CultureInfo ci = (CultureInfo) CultureInfo.CurrentCulture.Clone();
        ci.NumberFormat.CurrencyDecimalSeparator = ".";
        Name = elem.Attribute("name").Value;
        PortalId = (ushort) Utils.FromString(elem.Attribute("type").Value);
        Background = Utils.FromString(elem.Element("Background").Value);
        AllowTeleport = elem.Element("AllowTeleport") != null;
        Json = elem.Element("Json").Value;
    }

    public string Name { get; private set; }
    public ushort PortalId { get; private set; }
    public int Background { get; private set; }
    public bool AllowTeleport { get; private set; }
    public string Json { get; private set; }
}

public class SetTypeSkin
{
    public ushort ObjectType { get; private set; }
    public string ObjectId { get; private set; }
    public ushort SkinType { get; private set; }
    public int Size { get; private set; }
    public uint Color { get; private set; }
    public string BulletType { get; private set; }
    public KeyValuePair<int, int>[] StatsBoost { get; private set; }
    public Dictionary<int, int> Setpiece { get; private set; }

    public SetTypeSkin(XElement elem, ushort type)
    {
        ObjectType = type;
        ObjectId = elem.Attribute("id").Value;

        SkinType = elem.Elements("ActivateOnEquipAll").Where(i => i.Attribute("skinType") != null).Select(i => (ushort)Utils.FromString(i.Attribute("skinType").Value)).FirstOrDefault();
        Size = elem.Elements("ActivateOnEquipAll").Where(i => i.Attribute("size") != null).Select(i => int.Parse(i.Attribute("size").Value)).FirstOrDefault();
        Color = elem.Elements("ActivateOnEquipAll").Where(i => i.Attribute("color") != null).Select(i => (uint)Utils.FromString(i.Attribute("color").Value)).FirstOrDefault();
        BulletType = elem.Elements("ActivateOnEquipAll").Where(i => i.Attribute("bulletType") != null).Select(i => i.Attribute("bulletType").Value).FirstOrDefault();

        StatsBoost = elem.Elements("ActivateOnEquipAll").Where(i => i.Attribute("stat") != null && i.Attribute("amount") != null)
                    .Select(i => new KeyValuePair<int, int>(int.Parse(i.Attribute("stat").Value),
                                int.Parse(i.Attribute("amount").Value))).ToArray();

        Setpiece = elem.Elements("Setpiece").ToDictionary(i => int.Parse(i.Attribute("slot").Value), i => Utils.FromString(i.Attribute("itemtype").Value));
    }
}