﻿using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Resources;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("E.Deezer")]
[assembly: AssemblyDescription(".NET Deezer API wrapper.")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("BigE")]
[assembly: AssemblyProduct("E.Deezer")]
[assembly: AssemblyCopyright("Copyright ©  2015")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("146f0316-9a69-426b-87e8-20ef79416f2d")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("2.3.0.313")]
[assembly: AssemblyFileVersion("2.3.0.313")]
[assembly: NeutralResourcesLanguageAttribute("en-GB")]

// Make sure we can access some of the internals to the testing library.
[assembly: InternalsVisibleTo("E.Deezer.Tests")]
