using Steamworks;
using UnityEngine;

public class SteamStatsAndAchievements : MonoBehaviour
{
#if !DISABLESTEAMWORKS
    protected static SteamStatsAndAchievements s_instance;

    private enum Achievement : int
    {
        ACH_FIRST_LEVEL,
        ACH_HALFWAY,
        ACH_ALL_BRONZE,
        ACH_ALL_SILVER,
        ACH_ALL_GOLD,
        ACH_CREDITS,
        ACH_OVERFLOW,
        ACH_END,
        ACH_RELOAD,
        ACH_CLOSE
    };

    private Achievement_t[] m_Achievements = new Achievement_t[] {
        new(Achievement.ACH_FIRST_LEVEL),
        new(Achievement.ACH_HALFWAY),
        new(Achievement.ACH_ALL_BRONZE),
        new(Achievement.ACH_ALL_SILVER),
        new(Achievement.ACH_ALL_GOLD),
        new(Achievement.ACH_CREDITS),
        new(Achievement.ACH_OVERFLOW),
        new(Achievement.ACH_END),
        new(Achievement.ACH_RELOAD),
        new(Achievement.ACH_CLOSE)
    };

    // Our GameID
    private CGameID m_GameID;

    // Should we store stats this frame?
    private bool m_bStoreStats;

    //Stats
    private static int totalLevels;
    private static int totalBronze;
    private static int totalSilver;
    private static int totalGold;
    private static int reloads;

    public static bool closeShave;
    public static bool creditsRead;
    public static bool overflow;

    void OnEnable()
    {
        if (s_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        s_instance = this;

        DontDestroyOnLoad(gameObject);

        if (!SteamManager.Initialized)
        {
            Debug.Log("[Enable] Steam manager not init");
            return;
        }

        Debug.Log("Init Stats & Achievements");

        // Cache the GameID for use in the Callbacks
        m_GameID = new CGameID(SteamUtils.GetAppID());
    }

    private void Update()
    {
        if (!SteamManager.Initialized)
        {
            //Debug.Log("[Update] Steam manager not init");
            return;
        }

        foreach (Achievement_t achievement in m_Achievements)
        {
            //Debug.Log("Current Achievement: " + achievement.m_eAchievementID.ToString());

            if (achievement.m_bAchieved)
                continue;

            switch (achievement.m_eAchievementID)
            {
                case Achievement.ACH_FIRST_LEVEL:
                    if (totalLevels >= 1)
                    {
                        Debug.Log("Unlocking First Level Ach");
                        UnlockAchievement(achievement);
                    }
                    break;
                case Achievement.ACH_HALFWAY:
                    if (totalLevels >= 35)
                    {
                        Debug.Log("Unlocking Halfway Ach");
                        UnlockAchievement(achievement);
                    }
                    break;
                case Achievement.ACH_ALL_BRONZE:
                    if (totalBronze == 70)
                    {
                        Debug.Log("Unlocking Bronze Ach");
                        UnlockAchievement(achievement);
                    }
                    break;
                case Achievement.ACH_ALL_SILVER:
                    if (totalSilver == 70)
                    {
                        Debug.Log("Unlocking Silver Ach");
                        UnlockAchievement(achievement);
                    }
                    break;
                case Achievement.ACH_ALL_GOLD:
                    if (totalGold == 70)
                    {
                        Debug.Log("Unlocking Goal Ach");
                        UnlockAchievement(achievement);
                    }
                    break;
                case Achievement.ACH_END:
                    if (totalLevels == 70)
                    {
                        Debug.Log("Unlocking End Ach");
                        UnlockAchievement(achievement);
                    }
                    break;
                case Achievement.ACH_CLOSE:
                    if (closeShave)
                    {
                        Debug.Log("Unlocking Close Shave Ach");
                        UnlockAchievement(achievement);
                    }
                    break;
                case Achievement.ACH_OVERFLOW:
                    if (overflow)
                    {
                        Debug.Log("Unlocking Overflow Ach");
                        UnlockAchievement(achievement);
                    }
                    break;
                case Achievement.ACH_RELOAD:
                    if (reloads >= 100)
                    {
                        Debug.Log("Unlocking Reload Ach");
                        UnlockAchievement(achievement);
                    }
                    break;
                case Achievement.ACH_CREDITS:
                    if (creditsRead)
                    {
                        Debug.Log("Unlocking Credits Read Ach");
                        UnlockAchievement(achievement);
                    }
                    break;
            }
        }

        //Store stats in the Steam database if necessary
        if (m_bStoreStats)
        {

            Debug.Log("Storing Stats");

            // already set any achievements in UnlockAchievement

            bool bSuccess = SteamUserStats.StoreStats();
            // If this failed, we never sent anything to the server, try
            // again later.
            m_bStoreStats = !bSuccess;
        }
    }

    private void UnlockAchievement(Achievement_t achievement)
    {
        if (!SteamManager.Initialized)
        {
            //Debug.Log("[Update] Steam manager not init");
            return;
        }

        achievement.m_bAchieved = true;

        // the icon may change once it's unlocked
        //achievement.m_iIconImage = 0;

        // mark it down
        SteamUserStats.SetAchievement(achievement.m_eAchievementID.ToString());

        // Store stats end of frame
        m_bStoreStats = true;
    }


    public static void UpdateStats(string stat, int val)
    {

        if (!SteamManager.Initialized)
        {
            //Debug.Log("[Update] Steam manager not init");
            return;
        }

        if (stat == "Reloads")
        {
            reloads = val;
            SteamUserStats.SetStat("stat_reload", reloads);
            if (reloads == 50)
            {
                SteamUserStats.IndicateAchievementProgress("ACH_RELOAD", 50, 100);
            }
        }
        else if (stat == "totalLevels")
        {
            totalLevels = val;
            SteamUserStats.SetStat("stat_total", totalLevels);
            SteamUserStats.SetStat("stat_bronze", totalLevels);
        }
        else if (stat == "totalSilver")
        {
            totalSilver = val;
            SteamUserStats.SetStat("stat_silver", totalSilver);
        }
        else if (stat == "totalGold")
        {
            totalGold = val;
            SteamUserStats.SetStat("stat_gold", totalGold);
        }

        SteamUserStats.StoreStats();
    }

    private class Achievement_t
    {
        public Achievement m_eAchievementID;
        public bool m_bAchieved;

        public Achievement_t(Achievement achievementID)
        {
            m_eAchievementID = achievementID;
            m_bAchieved = false;
        }
    }
#endif
}
