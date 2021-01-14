using System;
using System.Text;
using System.Threading.Tasks;

namespace JiguangYxsConsole
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("Game Start!游戏开始");
            var game = new GameTest();
            try
            {
                await game.RunGameLevel();
                Console.WriteLine("Game End!");
                Console.Read();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
