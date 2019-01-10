using System.Linq;

namespace CommandLineInterface
{
    internal class Startup
    {
        private static void Main(string[] args)
        {
            var cli = new CommandLineInterface();
            if (args.Count() > 0)
                cli.Start(args[0]); // args[0] is supposed to be a .dll path
            else
                cli.Start(null);
        }
    }
}