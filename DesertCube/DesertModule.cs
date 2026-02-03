using MCGalaxy;
using MCGalaxy.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DesertCube
{
    public abstract class DesertModule
    {
        public abstract void Load();
        public abstract void Unload();
        public virtual void Tick() { }

        public static Dictionary<Type, DesertModule> LoadedModules = new Dictionary<Type, DesertModule>();

        public static SchedulerTask TickTask;
        public static void LoadModules()
        {

            var classes = Assembly.GetExecutingAssembly()
                       .GetTypes()
                       .Where(t => t.IsClass && t.Namespace.StartsWith("DesertCube.Modules"))
                       .ToList();

            foreach (var type in classes.Where((x) => { return x.IsSubclassOf(typeof(DesertModule)); }))
                AddModule(type);

            TickTask = MCGalaxy.Server.MainScheduler.QueueRepeat(TickModules, null, TimeSpan.FromMilliseconds(100));
        }

        public static void UnloadModules()
        {
            Server.MainScheduler.Cancel(TickTask);

            foreach (var m in LoadedModules.Values)
                m.Unload();

            LoadedModules.Clear();

        }

        static void TickModules(SchedulerTask t)
        {
            TickTask = t;

            foreach (var m in LoadedModules.Values)
            {
                try
                {
                    m.Tick();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
        }

        static void AddModule(Type type)
        {
            var module = Activator.CreateInstance(type);
            ((DesertModule)module).Load();
            LoadedModules.Add(type, (DesertModule)module);
            Logger.Log(LogType.ConsoleMessage, "Loaded module " + type.Name);
        }

        public static DesertModule GetInstance(Type type)
        {
            if (LoadedModules.ContainsKey(type))
                return LoadedModules[type];
            return null;
        }

    }
}
