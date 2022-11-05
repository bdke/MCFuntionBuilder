using MCFBuilder.MCData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCFBuilder.Utility.BuiltIn.Enum
{
    internal class EffectTypes : Class.BuiltInClass
    {
        public const string SPEED = "speed";
        public const string HASTE = "haste";
        public const string MINING_FATIGUE = "mining_fatigue";
        public const string STRENGTH = "strength";
        public const string INSTANT_HEALTH = "instant_health";
        public const string INSTANT_DAMAGE = "instant_damage";
        public const string JUMP_BOOST = "jump_boost";
        public const string NAUSEA = "nausea";
        public const string REGENERATION = "regeneration";
        public const string FIRE_RESISTANCE = "fire_resistance";
        public const string WATER_BREATHING = "water_breathing";
        public const string INVISIBILITY = "invisibility";
        public const string BLINDNESS = "blindness";
        public const string NIGHT_VISION = "night_vision";
        public const string HUNGER = "hunger";
        public const string WEAKNESS = "weakness";
        public const string POSION = "posion";
        public const string WITHER = "wither";
        public const string HEALTH_BOOST = "health_bppst";
        public const string ABSORPTION = "absorption";
        public const string SATURATION = "saturation";
        public const string GLOWING = "glowing";
        public const string LEVITATIOPN = "levitation";
        public const string LUCK = "luck";
        public const string BAD_LUCK = "bad_luck";
        public const string SLOW_FLALLING = "slow_falling";
        public const string CONDUIT_POWER = "conduit_power";
        public const string DOLPHINS_GRACE = "dolphins_grace";
        public const string BAD_OMEN = "bad_omen";
        public const string HERO_OF_THE_VILLAGE = "hero_of_the_village";
        public const string DARKNESS = "darkness";
        public EffectTypes() : base(typeof(EffectTypes))
        {

        }
    }
}
