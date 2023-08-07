# Appi
What is `Appi`? Appi is short for **App i**nformation.  
  
The goal is to query multiple sources for information at once. Reachable by using your favorite shell from within your keyboard, without even touching your mouse.  
Because your information sources will be different from mine, go start building your first plugin and get started using your command line or try combining it with `PowerToys Run`.

## Installation
- Install via [NuGet](https://www.nuget.org/packages/Appi): `dotnet tool install --global Appi`
- Build from your own

## Infrastructure
Some infrastructure classes are already provided. You can build up from given classes like:
- **File** (see `sources.json` after running `appi config open` and change the path of your text file)
- **More to come** out of the box (want to collaborate?)

## Plugins
This app is highly extensable by adding own plugins. You can fetch data from any source you can imagine, e. g. from your SharePoint Server or any database.

Just follow these simple steps:
1. Create a .NET 7 class library
2. Add the `Appi` NuGet package as a dependency  
   (e. g. `PM> Install-Package Appi`)
3. Create a class that implements `ISource` (and `ResultItemBase`)
4. Register the new assembly by calling `Appi.exe config register-lib "pathToAssembly.dll"`
 
### Example for implementing `ISource`
The class implementing `ISource` must have a parameterless constructor.  
The `ReadAsync()` method must pass the `FindItemsOptions` object which contains the query and returns the found data.

``` csharp
using Core.Abstractions;
using Core.Models;

namespace ExternalSourceDemo
{
    public class ExternalDemoSource : ISource
    {
        public string TypeName { get; set; } = typeof(ExternalDemoSource).Name;
        public string Name { get; set; } = "Demo Assembly";
        public string Description { get; set; } = "Returns hard-coded hello world.";
        public bool IsActive { get; set; } = true;
        public int SortOrder { get; set; } = 50;
        public string? Path { get; set; } = null;

        public async Task<IEnumerable<ResultItemBase>> ReadAsync(FindItemsOptions options)
        {
            var output = new List<ExternalDemoResult>()
            {
                new() { Name = "Hello", Description = options?.Query ?? "World" }
            };

            return await Task.FromResult(output);
        }
    }
}
```
  
### Example for implementing ResultItemBase
This class controls the output of an item by overriding the `ToString()` method and displays the possible actions with the result of `GetActions()` method if an item of this source gets selected. You can easily interact with **predefined services** like using the `ClipboardService` or `ProcessService` for most frequent used actions.  
By using the `Result` attribute you can define the displayed properties in the output table.

``` csharp
using Core.Abstractions;
using Core.Attributes;
using Core.Models;

namespace ExternalSourceDemo
{
    public class ExternalDemoResult : ResultItemBase
    {
        [Result]
        public override string Name { get => base.Name; set => base.Name = value; }

        [Result]
        public override string Description { get => base.Description; set => base.Description = value; }

        public override IEnumerable<ActionItem> GetActions()
        {
            return Enumerable.Empty<ActionItem>();
        }

        public override string ToString()
        {
            return $"{Name} {Description}!";
        }
    }
}

```

## Up next
Build more infrastructure classes like 
- Microsoft SQL
- MySQL / MariaDB
- SQlite