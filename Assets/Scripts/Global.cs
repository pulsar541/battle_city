using System.Collections; 
public class Global 
{ 
    public const int MaxPlayersCount = 2;
    public static int[] score = new int[MaxPlayersCount]; 
    public static int[,] destroyedTankTypesCounter = new int[MaxPlayersCount,2]; 
}
