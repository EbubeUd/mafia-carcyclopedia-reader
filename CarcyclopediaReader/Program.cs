using System;

namespace CarcyclopediaReader
{
    class Program
    {
        static void Main(string[] args)
        {

            string binPath = "D:/Games/Mafia/tables/carcyclopedia.def";
            string jsonPath = "D:/RemoteWork/Mafia/carcyclopedia.json";
            BinReaderService.ReadBinFileAsync(binPath, jsonPath, true);
            Console.ReadLine();
        }
    }
}
