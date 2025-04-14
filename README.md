## Fluentish.InjectableStatic.Generator  
**When upstream doesn't like inversion of control**

`Fluentish.InjectableStatic.Generator` is a source generator for C#. It allows you to generate interface and service classes that wrap static members of your target classes. By applying attributes, you can control which static classes and members become “injectable,” making it easier to work with otherwise rigid static code through a dependency injection container.

### Features
- Automatically generate wrapper classes and corresponding interfaces
- Supported member types:
  - Constants
  - static Properties
  - static Fields
  - static Methods

- Filter which members are included in generated code
- Customize namespace prefix

### Usage
#### Basic usage:
```csharp
[assembly: Fluentish.InjectableStatic.Injectable(typeof(System.Console))]

Fluentish.Injectable.System.IConsole console = new Fluentish.Injectable.System.ConsoleService();

console.WriteLine("Hello, World!");
```
Will generate `Fluentish.Injectable.System.IConsole` and `Fluentish.Injectable.System.ConsoleService`, which contain all the static members.

#### Filtering members:
```csharp
[assembly: Fluentish.InjectableStatic.Injectable(
    typeof(System.Diagnostics.Debug),
    FilterType.Exclude,
    nameof(System.Diagnostics.Debug.Print)
)]
```
Will generate `Fluentish.Injectable.System.Diagnostics.IDebug` and `Fluentish.Injectable.System.Diagnostics.DebugService` without the `Print` method.

```csharp
[assembly: Fluentish.InjectableStatic.Injectable(
    typeof(System.Diagnostics.Debug),
    FilterType.Include,
    nameof(System.Diagnostics.Debug.Write),
    nameof(System.Diagnostics.Debug.WriteIf),
    nameof(System.Diagnostics.Debug.WriteLine),
    nameof(System.Diagnostics.Debug.WriteLineIf)
)]
```
Will generate `Fluentish.Injectable.System.Diagnostics.IDebug` and `Fluentish.Injectable.System.Diagnostics.DebugService` containing only the listed members.

## Configuration

By default, generated classes are prefixed with `Fluentish.Injectable.` to avoid name collisions. You can customize this behavior using the `InjectableStaticConfigurationAttribute` at the assembly level.

#### Available Options

```csharp
[assembly: Fluentish.InjectableStatic.InjectableStaticConfigurationAttribute(
    Namespace = "Your.Custom.Namespace",
    NamespaceMode = Fluentish.InjectableStatic.NamespaceMode.Prefix // or Fluentish.InjectableStatic.NamespaceMode.Const
)]
```

| Property        | Description |
|----------------|-------------|
| `Namespace`     | The base namespace for generated wrappers. Set to an empty string (`""`) to disable namespace prefixing entirely. |
| `NamespaceMode` | Determines how the namespace is applied. See below for details. |

#### Namespace Modes

- `NamespaceMode.Prefix` (default):  
  The value of `Namespace` is used as a prefix. For example, if set to `"My.Custom"`, generated classes will be in `"My.Custom.Original.Namespace"`.

- `NamespaceMode.Const`:  
  The value of `Namespace` replaces the entire namespace. All generated classes will reside in exactly the specified namespace.

#### Examples

**Remove the default prefix:**
```csharp
[assembly: Fluentish.InjectableStatic.InjectableStaticConfigurationAttribute(Namespace = "")]
```

**Use a custom namespace as a prefix:**
```csharp
[assembly: Fluentish.InjectableStatic.InjectableStaticConfigurationAttribute(
    Namespace = "My.Namespace",
    NamespaceMode = NamespaceMode.Prefix
)]
```

**Force all wrappers into a constant namespace:**
```csharp
[assembly: Fluentish.InjectableStatic.InjectableStaticConfigurationAttribute(
    Namespace = "My.Constant.Namespace",
    NamespaceMode = NamespaceMode.Const
)]
```

### Remarks
Please note that this project is in its early stages of development and may require significant improvements. While it is functional, it may not be suitable for production use in its current state. Any feedback, suggestions, or contributions to improve the source generator are highly appreciated.

## Contributing
Currently, I'm not accepting any code contributions because the project requires a major refactoring.

## Support
If you have any questions, issues, or suggestions, please create a new issue on the GitHub repository.

## License
This project is licensed under the MIT License. See the  [LICENSE](LICENSE.txt) file for more information.
