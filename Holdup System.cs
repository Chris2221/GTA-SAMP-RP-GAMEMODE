#define AIM_TIME 5.0
#define MONEY_PERCENTAGE 0.1

new aimedPlayer[MAX_PLAYERS];
new aimTimer[MAX_PLAYERS];

forward OnAimTimerExpired(playerid, victimid);// Function to handle the aiming timer expiration

    if (IsPlayerConnected(playerid))
    {
        new Float:aimedPlayerX, Float:aimedPlayerY, Float:aimedPlayerZ;
        GetPlayerPos(aimedPlayer[playerid], aimedPlayerX, aimedPlayerY, aimedPlayerZ);

        new Float:aimZ;
        GetPlayerFacingAngle(playerid, aimZ);

        new Float:playerX, Float:playerY, Float:playerZ;
        GetPlayerPos(playerid, playerX, playerY, playerZ);

        // Check if player A (bad guy) is aiming at player B (victim)
        if (aimZ > aimedPlayerZ && (playerX - aimedPlayerX) * (playerX - aimedPlayerX) + (playerY - aimedPlayerY) * (playerY - aimedPlayerY) <= 25.0)
        {
            // Start the aiming timer if it hasn't already started
            if (!IsTimerRunning(aimTimer[playerid]))
            {
                aimTimer[playerid] = SetTimerEx("OnAimTimerExpired", FloatToFixed(AIM_TIME * 1000.0), false, "dd", playerid, aimedPlayer[playerid]);
            }
        }
        else
        {
            // Reset the aiming timer if the conditions are not met
            if (IsTimerRunning(aimTimer[playerid]))
            {
                KillTimer(aimTimer[playerid]);
                aimTimer[playerid] = Timer_Invalid;
            }
        }
    }


public OnPlayerUpdate(playerid)
{	
	//holdup system
    if (IsPlayerConnected(playerid))
    {
        new Float:aimedPlayerX[MAX_PLAYERS];
        new Float:aimedPlayerY[MAX_PLAYERS];
        new Float:aimedPlayerZ[MAX_PLAYERS];

        GetPlayerPos(aimedPlayer[playerid], aimedPlayerX[playerid], aimedPlayerY[playerid], aimedPlayerZ[playerid]);

        new Float:aimZ;
        GetPlayerFacingAngle(playerid, aimZ);

        new Float:playerX, Float:playerY, Float:playerZ;
        GetPlayerPos(playerid, playerX, playerY, playerZ);

        // Check if player A (bad guy) is aiming at player B (victim)
        if (aimZ > aimedPlayerZ[playerid] && (playerX - aimedPlayerX[playerid]) * (playerX - aimedPlayerX[playerid]) + (playerY - aimedPlayerY[playerid]) * (playerY - aimedPlayerY[playerid]) <= 25.0)
        {
            // Start the aiming timer if it hasn't already started
            if (!IsTimerValid(aimTimer[playerid]))
            {
                aimTimer[playerid] = SetTimerEx("OnAimTimerExpired", FloatToFixed(AIM_TIME * 1000.0), false, "dd", playerid, aimedPlayer[playerid]);
            }
        }
        else
        {
            // Reset the aiming timer if the conditions are not met
            if (IsTimerValid(aimTimer[playerid]))
            {
                KillTimer(aimTimer[playerid]);
                aimTimer[playerid] = INVALID_TIMER_ID;
            }
        }
    }
}

public OnAimTimerExpired(playerid, victimid)
{
    // Check if both players are still connected
    if (IsPlayerConnected(playerid) && IsPlayerConnected(victimid))
    {
        // Make the victim player (Player B) raise their hands up
        SetPlayerFightingStyle(victimid, 15); // 15 is the fighting style for hands up

        // Calculate the money to transfer from Player B to Player A
        new Float:moneyAmount = GetPlayerMoney(victimid) * MONEY_PERCENTAGE;

        // Transfer the money from Player B to Player A
        GivePlayerMoney(playerid, moneyAmount);
        GivePlayerMoney(victimid, -moneyAmount);
    }
}
