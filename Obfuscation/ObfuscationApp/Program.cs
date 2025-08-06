
// See https://aka.ms/new-console-template for more information
using ObfuscationApp;
using System.Reflection;

Console.WriteLine("1. Your secret from clear assembly:");
string assemblyPath = "Libraries/ObfuscationLib.dll";
TryToLoadSecretFromAssembly(assemblyPath);

Console.WriteLine("2. Your secret from community dotfuscated by renaming assembly:");
string dotfuscatedByRenamingAssemblyPath = "Libraries/DotfuscatedByRenaming/Dotfuscated_ObfuscationLib.dll";
TryToLoadSecretFromAssembly(dotfuscatedByRenamingAssemblyPath);

Console.WriteLine("3. Your secret from .Net Reactor protected assembly:");
string dotNetReactorAssemblyPath = "Libraries/DotNetReactor/DotNetReactor_ObfuscationLib.dll";
TryToLoadSecretFromAssembly(dotNetReactorAssemblyPath);

static void TryToLoadSecretFromAssembly(string relativePath)
{
  if (!File.Exists(relativePath))
  {
    Console.WriteLine($"Assembly {relativePath} does not exist.");
    return;
  }
  string absolutePath = Path.GetFullPath(relativePath);
  var alc = new IsolatedLoadContext();
 
  try
  {
    
    Assembly assembly = alc.LoadFromAssemblyPath(absolutePath);
    
    string typeName = "ObfuscationLib.MySensitiveData";
    Type? type = assembly.GetType(typeName);
    if (type is null)
    {
      Console.WriteLine($"Type {typeName} is not found.");
      return;
    }

    FieldInfo? fieldInfo = type.GetField("_mySecretKey", BindingFlags.Public | BindingFlags.Static);
    if (fieldInfo is null)
    {
      Console.WriteLine("No secret found.");
      return;
    }

    object? value = fieldInfo.GetValue(null);
    Console.WriteLine($"Your secret from assembly {relativePath} is: {value}");
  }
  catch(Exception ex)
  {
    Console.WriteLine($"Error loading assembly {relativePath}: {ex.Message}");
  }

  finally
  {
    alc.Unload();
    // Uncomment the following lines if you want to force garbage collection
    //GC.Collect();
    //GC.WaitForPendingFinalizers();
  }

}


