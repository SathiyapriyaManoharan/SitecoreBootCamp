
// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Design",
    "CA1014:Mark assemblies with CLSCompliantAttribute",
    Justification = "Unit testing framework is c# only")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Performance",
    "CA1813:Avoid unsealed attributes",
    Justification = "Attributes are designed for extending")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Usage",
    "CA1019:Define accessors for attribute arguments",
    Justification = "Functionality maps to inherited implementation from base attributes")]