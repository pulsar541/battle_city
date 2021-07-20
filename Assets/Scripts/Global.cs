using System.Collections; 
public class Global 
{ 
    public const int MaxPlayersCount = 2;
    public static int[] score = new int[MaxPlayersCount]; 
    public static int[,] destroyedTankTypesCounter = new int[MaxPlayersCount,(int)Tank.Type.MAX_TYPES]; 

    public static void Reset() 
    {
        for(int p = 0; p < MaxPlayersCount; p++){
            score[p] = 0;
            for(int type = 0; type < (int)Tank.Type.MAX_TYPES; type++) {
                destroyedTankTypesCounter[p,type] = 0;
            }
        }
    }
}
