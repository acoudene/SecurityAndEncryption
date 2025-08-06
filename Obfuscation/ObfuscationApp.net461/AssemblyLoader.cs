using System;
using System.Reflection;

namespace ObfuscationApp.net461
{
  public class AssemblyLoader : MarshalByRefObject
  {
    public void TryToLoadSecretFromAssembly(string path)
    {
      try
      {
        Assembly assembly = Assembly.LoadFrom(path);

        string typeName = "ObfuscationLib.net461.MySensitiveData";
        Type type = assembly.GetType(typeName);
        if (type == null)
        {
          Console.WriteLine($"Type {typeName} is not found.");
          return;
        }

        FieldInfo fieldInfo = type.GetField("_mySecretKey", BindingFlags.Public | BindingFlags.Static);
        if (fieldInfo == null)
        {
          Console.WriteLine("No secret found.");
          return;
        }

        object value = fieldInfo.GetValue(null);
        Console.WriteLine($"Your secret from assembly {path} is: {value}");
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Error loading assembly {path}: {ex.Message}");
      }
    }
  }

}
