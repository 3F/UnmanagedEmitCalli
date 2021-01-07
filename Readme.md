# [UnmanagedEmitCalli](https://github.com/3F/UnmanagedEmitCalli)

A tiny **[hack](https://github.com/3F/UnmanagedEmitCalli/issues/13)** of the System.Private.CoreLib to provide missed *Unmanaged EmitCalli* implementation at the .NET Standard **2.0** layer.

[![Build status](https://ci.appveyor.com/api/projects/status/6m24rr5o891fluhb/branch/master?svg=true)](https://ci.appveyor.com/project/3Fs/unmanagedemitcalli/branch/master)
[![release-src](https://img.shields.io/github/release/3F/UnmanagedEmitCalli.svg)](https://github.com/3F/UnmanagedEmitCalli/releases/latest)
[![License](https://img.shields.io/badge/License-MIT-74A5C2.svg)](https://github.com/3F/UnmanagedEmitCalli/blob/master/License.txt)
[![NuGet package](https://img.shields.io/nuget/v/UnmanagedEmitCalli.svg)](https://www.nuget.org/packages/UnmanagedEmitCalli/) 


## Why UnmanagedEmitCalli ?

An *Unmanaged EmitCalli* is available only with .NET Core 2.1+ [[?](https://github.com/3F/Conari/issues/13)] That turns into a quest when you need .NET Standard 2.0 targeting.

This, however, does not require a full reimplementing or a huge dependencies like Cecil etc.

Because you can simply use the same techniques as we do https://github.com/3F/Conari/issues/13#issuecomment-554010927

[UnmanagedEmitCalli](https://github.com/3F/UnmanagedEmitCalli) provides most common implementation that can be used either through dependencies or copy-paste of a bit of source code. Enjoy!

## Where ?

This was used initially for 🧬 [Conari](https://github.com/3F/Conari) - Platform for working with unmanaged memory, pe-modules, related PInvoke features, and more for: Libraries, Executable Modules, enjoying of the unmanaged native C/C++ in .NET world, and other raw binary data. Even accessing to complex types like structures without their declaration at all.

## Example

Same for .NET Framework and .NET Core

```csharp
ILGenerator il = dyn.GetILGenerator();

for(int i = 0; i < mParams.Length; ++i) {
    il.Emit(OpCodes.Ldarg, i);
}

if(IntPtr.Size == sizeof(Int64)) {
    il.Emit(OpCodes.Ldc_I8, ptr.ToInt64());
}
else {
    il.Emit(OpCodes.Ldc_I4, ptr.ToInt32());
}

il.EmitCalliStd(CallingConvention.Cdecl, tret, mParams);
il.Emit(OpCodes.Ret);
```

## 🍰 Open and Free

Open Source project; MIT License, Yes! Enjoy!

## 🗸 License

The [MIT License (MIT)](https://github.com/3F/UnmanagedEmitCalli/blob/master/License.txt)

```
Copyright (c) 2019-2021  Denis Kuzmin <x-3F@outlook.com> github/3F
```

[ [ ☕ Donate ](https://3F.github.com/Donation/) ]

UnmanagedEmitCalli contributors https://github.com/3F/UnmanagedEmitCalli/graphs/contributors

Make your amazing contribution!