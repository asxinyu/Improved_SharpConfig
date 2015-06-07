![sharpconfig_logo.png](sharpconfig_logo.png)

SharpConfig is an easy-to-use CFG/INI configuration library for .NET.
This Version is Base SharpConfig 1.3,and Improved some details.

The original ：https://github.com/cemdervis/SharpConfig

My Blog URL：http://www.cnblogs.com/asxinyu/p/dotnet_Opensource_project_SharpConfig.html


You can use SharpConfig in your .NET applications to add the functionality
to read, modify and save configuration files and streams, in either text or binary format.

> If SharpConfig has helped you and you feel like donating, [feel free](https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=WWN94LMDN5HMC)!
> Donations help to keep the development of SharpConfig active.

A configuration file example:
```cfg
[General]
# a comment
SomeString = Hello World!
SomeInteger = 10 # an inline comment
SomeFloat = 20.05
ABoolean = true
```

To read these values, your C# code would look like:
```csharp
Configuration config = Configuration.LoadFromFile( "sample.cfg" );
Section section = config["General"];

string someString = section["SomeString"].Value;
int someInteger = section["SomeInteger"].GetValue<int>();
float someFloat = section["SomeFloat"].GetValue<float>();
```

Enumerations
---

SharpConfig is also able to parse enumerations.
For example you have a configuration like this:
```cfg
[DateInfo]
Day = Monday
```

It is now possible to read this value as a System.DayOfWeek enum, because Monday is present there.
An example of how to read it:

```csharp
DayOfWeek day = config["DateInfo"]["Day"].GetValue<DayOfWeek>();
```

Arrays
---

Arrays are also supported in SharpConfig.
For example you have a configuration like this:
```cfg
[General]
MyArray = {0,2,5,6}
```

This array can be interpreted as any type array that can be converted from 0, 2, 5 and 6, for example int, float, double, char, byte, string etc.

Reading this array is simple:
```csharp
object[] myArray = config["General"]["MyArray"].GetValue<object[]>();
```

Creating a Configuration in-memory
---

```csharp
// Create the configuration.
var myConfig = new Configuration();

// Set some values.
// This will automatically create the sections and settings.
myConfig["Video"]["Width"].Value = "1920";
myConfig["Video"]["Height"].Value = "1080";

// Set an array value.
myConfig["Video"]["Formats"].SetValue( new string[] { "RGB32", "RGBA32" } );
```

Iterating through a Configuration
---

```csharp
foreach ( var section in myConfig )
{
    foreach ( var setting in section )
    {
        ...
    }
}
```

Saving a Configuration
---

```csharp
myConfig.Save( "myConfig.cfg" ); // Save to a text-based file.
myConfig.Save( myStream ); // Save to a text-based stream.
myConfig.SaveBinary( "myConfig.cfg" ); // Save to a binary file.
myConfig.SaveBinary( myStream ); // Save to a binary stream.
```

Object Mapping
---

A nice-to-have in SharpConfig is the mapping of sections to objects.
If you have a class and enumeration in C# like this:
```csharp
class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
    public Gender Gender { get; set; }
}

enum Gender
{
    Male,
    Female
}
```

It is possible to create an object straight from the configuration file:
```cfg
[Person]
Name = Peter
Age = 50
Gender = Male
```
Like this:
```csharp
Person person = config["Person"].CreateObject<Person>();
```

Note: The mapping only works on classes, public properties and primitive data types (int, bool, enums ...).

If you already have a Person object and don't want to create a new one, you can use the MapTo method:
```csharp
config["Person"].MapTo(person);
```

Installing via NuGet
---

You can install SharpConfig via the following NuGet command:
> Install-Package sharpconfig

[NuGet Page](https://www.nuget.org/packages/sharpconfig/)
