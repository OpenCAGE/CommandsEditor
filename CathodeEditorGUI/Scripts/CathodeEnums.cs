using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//TODO: Flesh this out and then bring it over to CathodeLib

namespace CommandsEditor
{
    public enum CHECKPOINT_TYPE
    {
        CAMPAIGN,
        MANUAL, //confirmed via savestation
        CAMPAIGN_MISSION,
        MISSION_TEMP_STATE = -1, //perhaps?
    };

    public enum CHARACTER_STANCE
    {
        DONT_CHANGE,
        STAND,
        CROUCHED,
    }

    public enum CI_MESSAGE_TYPE 
    {
        MSG_NORMAL,
        MSG_IMPORTANT,
        MSG_ERROR,
    };

    public enum CHARACTER_CLASS
    {
        PLAYER,
        ALIEN,
        ANDROID,
        CIVILIAN,
        SECURITY,
        FACEHUGGER,
        INNOCENT,
        ANDROID_HEAVY,
        MOTION_TRACKER,
        MELEE_HUMAN,
    };

    public enum CHARACTER_CLASS_COMBINATION
    {
        NONE = 0x0,
        PLAYER_ONLY = 0x1,
        ALIEN_ONLY = 0x2,
        ANDROID_ONLY = 0x4,
        CIVILIAN_ONLY = 0x8,
        SECURITY_ONLY = 0x10,
        FACEHUGGER_ONLY = 0x20,
        INNOCENT_ONLY = 0x40,
        ANDROID_HEAVY_ONLY = 0x80,
        MOTION_TRACKER = 0x100,
        MELEE_HUMAN_ONLY = 0x200,
        ANDROIDS = ANDROID_ONLY | ANDROID_HEAVY_ONLY,
        ALIENS = ALIEN_ONLY | FACEHUGGER_ONLY,
        HUMAN_NPC = CIVILIAN_ONLY | SECURITY_ONLY | INNOCENT_ONLY | MELEE_HUMAN_ONLY,
        HUMAN = HUMAN_NPC | PLAYER_ONLY,
        HUMANOID_NPC = HUMAN_NPC | ANDROIDS,
        HUMANOID = HUMANOID_NPC | PLAYER_ONLY,
        ANDROIDS_AND_ALIEN = ANDROIDS | ALIENS,
        PLAYER_AND_ALIEN = PLAYER_ONLY | ALIEN_ONLY,
        ALL = ALIENS | HUMANOID | MOTION_TRACKER,
    }
}
