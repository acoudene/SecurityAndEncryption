using System;
using System.IO;
using System.Reflection;

namespace ObfuscationApp.net461
{
  internal class Program
  {
    static void Main(string[] args)
    {
      Console.WriteLine("1. Your secret from clear assembly:");
      string assemblyPath = "Libraries/ObfuscationLib.net461.dll";
      TryToLoadSecretFromAssembly(assemblyPath);

      Console.WriteLine("2. Your secret from community dotfuscated by renaming assembly:");
      string dotfuscatedByRenamingAssemblyPath = "Libraries/DotfuscatedByRenaming/Dotfuscated_ObfuscationLib.net461.dll";
      TryToLoadSecretFromAssembly(dotfuscatedByRenamingAssemblyPath);

      Console.WriteLine("3. Your secret from .Net Reactor protected assembly:");
      string dotNetReactorAssemblyPath = "Libraries/DotNetReactor/DotNetReactor_ObfuscationLib.net461.dll";
      TryToLoadSecretFromAssembly(dotNetReactorAssemblyPath);
    }

    static void TryToLoadSecretFromAssembly(string relativePath)
    {
      if (!File.Exists(relativePath))
      {
        Console.WriteLine($"Assembly {relativePath} does not exist.");
        return;
      }
      string absolutePath = Path.GetFullPath(relativePath);
      string assemblyDirectory = Path.GetDirectoryName(absolutePath);

      AppDomainSetup setup = new AppDomainSetup
      {
        ApplicationBase = assemblyDirectory
      };

      AppDomain domain = AppDomain.CreateDomain("IsolatedDomain", null, setup);

      try
      {

        var loader = (AssemblyLoader)domain.CreateInstanceFromAndUnwrap(
                typeof(AssemblyLoader).Assembly.Location,
                typeof(AssemblyLoader).FullName);

        loader.TryToLoadSecretFromAssembly(absolutePath);
      }
      finally
      {
        AppDomain.Unload(domain);
      }

    }
  }
}
