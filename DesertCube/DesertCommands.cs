using MCGalaxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DesertCube
{
    public class DesertCommands
    {
        public static List<Command> Commands = new List<Command>();
        public static void LoadCommands()
        {

            var classes = Assembly.GetExecutingAssembly()
                       .GetTypes().Where(t => t.IsClass && t.Namespace != null && t.Namespace.StartsWith("DesertCube.Commands"))
                       .ToList();

            foreach (var type in classes.Where((x) => { return x.IsSubclassOf(typeof(Command2)); }))
                AddCommand(type);

        }

        public static void UnloadCommands()
        {
            foreach (var cmd in Commands)
                Command2.Unregister(cmd);

            Commands.Clear();
        }
        static void AddCommand(Type type)
        {
            Command2 module = (Command2)Activator.CreateInstance(type);
            Command2.Register(module);

            Commands.Add(module);

            Logger.Log(LogType.ConsoleMessage, "Loaded cmd " + type.Name);
        }
    }
}
