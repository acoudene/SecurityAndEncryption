
// See https://aka.ms/new-console-template for more information
using ObsucationApp;
using System.Reflection;


var myData = new MySensitiveData();
Console.WriteLine($"1. Your secret from code: {myData.PrintSecret()}");

Console.WriteLine("2. Your secret from clear assembly:");
string assemblyPath = "Libraries/ObfuscationApp.dll";
TryToLoadSecretFromAssembly(assemblyPath);

Console.WriteLine("3. Your secret from community dotfuscated by renaming assembly:");
string dotfuscatedByRenamingAssemblyPath = "Libraries/DotfuscatedByRenaming/ObfuscationApp.dll";
TryToLoadSecretFromAssembly(dotfuscatedByRenamingAssemblyPath);

void TryToLoadSecretFromAssembly(string path)
{
  Assembly assembly = Assembly.LoadFrom(path);
  Type ? type = assembly.GetType("ObsucationApp.MySensitiveData");

  if (type is not null)
  {   
    FieldInfo? fieldInfo = type.GetField("_mySecretKey", BindingFlags.Public | BindingFlags.Static);

    if (fieldInfo != null)
    {
      object? value = fieldInfo.GetValue(null);
      Console.WriteLine($"Your secret from assembly {path} is: {value}");
    }
    else
    {
      Console.WriteLine("No secret found.");
    }
  }
  else
  {
    Console.WriteLine("Instance cannot be loaded.");
  }
}


