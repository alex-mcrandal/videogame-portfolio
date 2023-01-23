/*
 * Constants contains readonly data useful to the entire program. 
 */

using System.Collections.Generic;

//====================================================================================
//File:             Constants.cs
//Author:           Revolution Gaming
//Email:            revolutiongaming8@gmail.com
//Last Modified:    12-21-2022
//Project:          Project Monster
//====================================================================================

public class Constants
{
    public const int maxPlayers = 5;

    public const string joinKey = "j";
    public const string difficultyKey = "d";

    public static readonly List<string> difficulties = new() { "Easy" };
}
