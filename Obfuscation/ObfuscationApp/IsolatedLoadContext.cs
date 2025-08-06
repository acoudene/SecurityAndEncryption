using System.Reflection;
using System.Runtime.Loader;

namespace ObfuscationApp;

class IsolatedLoadContext : AssemblyLoadContext
{
  public IsolatedLoadContext() : base(isCollectible: true) { }

  protected override Assembly Load(AssemblyName assemblyName)
  {
    // Note: This method is called when the assembly is not found in the default load context.
    return null!;
  }
}
