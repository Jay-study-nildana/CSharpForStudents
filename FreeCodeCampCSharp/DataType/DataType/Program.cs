// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

//value types and reference types

//What is a data type?
//A data type is a way a programming language defines how much memory to save for a value. There are many data types in the C# language to be used for many different applications and sizes of data.

//For most of the applications you build in your career, you'll settle on a small subset of all the available data types. However, it's still vital to know others exist and why.

//Value vs. reference types
//This module focuses on the two kinds of types in C#: reference types and value types.

//Variables of reference types store references to their data (objects), that is they point to data values stored somewhere else. In comparison, variables of value types directly contain their data. As you learn more about C#, new details emerge related to the fundamental difference between value and reference types.

Console.WriteLine("Signed integral types:");

Console.WriteLine($"sbyte  : {sbyte.MinValue} to {sbyte.MaxValue}");
Console.WriteLine($"short  : {short.MinValue} to {short.MaxValue}");
Console.WriteLine($"int    : {int.MinValue} to {int.MaxValue}");
Console.WriteLine($"long   : {long.MinValue} to {long.MaxValue}");

Console.WriteLine("");
Console.WriteLine("Unsigned integral types:");

Console.WriteLine($"byte   : {byte.MinValue} to {byte.MaxValue}");
Console.WriteLine($"ushort : {ushort.MinValue} to {ushort.MaxValue}");
Console.WriteLine($"uint   : {uint.MinValue} to {uint.MaxValue}");
Console.WriteLine($"ulong  : {ulong.MinValue} to {ulong.MaxValue}");

Console.WriteLine("");
Console.WriteLine("Floating point types:");
Console.WriteLine($"float  : {float.MinValue} to {float.MaxValue} (with ~6-9 digits of precision)");
Console.WriteLine($"double : {double.MinValue} to {double.MaxValue} (with ~15-17 digits of precision)");
Console.WriteLine($"decimal: {decimal.MinValue} to {decimal.MaxValue} (with 28-29 digits of precision)");

//reference types are different from value types

//Value Type (int): In this example, val_A and val_B are integer value types.

int val_A = 2;
int val_B = val_A;
val_B = 5;

Console.WriteLine("--Value Types--");
Console.WriteLine($"val_A: {val_A}");
Console.WriteLine($"val_B: {val_B}");

// Reference Type(array): In this example, ref_A and ref_B are array reference types.

int[] ref_A = new int[1];
ref_A[0] = 2;
int[] ref_B = ref_A;
ref_B[0] = 5;

Console.WriteLine("--Reference Types--");
Console.WriteLine($"ref_A[0]: {ref_A[0]}");
Console.WriteLine($"ref_B[0]: {ref_B[0]}");

//When in doubt, stick with the basics
//While you've looked at several considerations, as you're getting started, for simplicity's sake you should prefer a subset of basic data types, including:

//int for most whole numbers
//decimal for numbers representing money
//bool for true or false values
//string for alphanumeric value
