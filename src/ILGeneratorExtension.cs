/*
 * The MIT License (MIT)
 *
 * Copyright (c) 2019-2021  Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) UnmanagedEmitCalli contributors https://github.com/3F/UnmanagedEmitCalli
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
*/

using System;
using System.Reflection.Emit;
using System.Runtime.InteropServices;

#if NETSTD
using System.Collections.Generic;
using System.Reflection;
#endif

namespace net.r_eg.DotNet.System.Reflection.Emit
{
    public static class ILGeneratorExtension
    {
        /// <summary>
        /// Puts a <see cref="OpCodes.Calli"/> instruction onto the CIL stream, 
        /// specifying an unmanaged calling convention for the indirect call.
        /// </summary>
        /// <remarks>
        /// Only netstandard2.0 provides the hack that was based on indirect analysis: https://github.com/3F/Conari/issues/13#issuecomment-554010927
        /// Please note that instructions in implemented method (System.Private.CoreLib.dll) are not fully the same to unmanaged EmitCalli (mscorlib or netcoreapp2.1+).
        /// </remarks>
        /// <param name="cil">CIL stream.</param>
        /// <param name="unmanagedCallConv">The unmanaged calling convention to be used.</param>
        /// <param name="returnType">The System.Type of the result.</param>
        /// <param name="parameterTypes">The types of the required arguments to the instruction.</param>
        public static void EmitCalliStd(this ILGenerator cil, CallingConvention unmanagedCallConv, Type returnType, Type[] parameterTypes)
        {
            cil.EmitCalliStd(OpCodes.Calli, unmanagedCallConv, returnType, parameterTypes);
        }

        /// <summary>
        /// Puts a <see cref="OpCodes.Calli"/> instruction onto the CIL stream, 
        /// specifying an unmanaged calling convention for the indirect call.
        /// </summary>
        /// <remarks>
        /// Only netstandard2.0 provides the hack that was based on indirect analysis: https://github.com/3F/Conari/issues/13#issuecomment-554010927
        /// Please note that instructions in implemented method (System.Private.CoreLib.dll) are not fully the same to unmanaged EmitCalli (mscorlib or netcoreapp2.1+).
        /// </remarks>
        /// <param name="cil">CIL stream.</param>
        /// <param name="opcode">The CIL instruction to be emitted onto the stream. Must be <see cref="OpCodes.Calli"/>.</param>
        /// <param name="unmanagedCallConv">The unmanaged calling convention to be used.</param>
        /// <param name="returnType">The System.Type of the result.</param>
        /// <param name="parameterTypes">The types of the required arguments to the instruction.</param>
        public static void EmitCalliStd(this ILGenerator cil, OpCode opcode, CallingConvention unmanagedCallConv, Type returnType, Type[] parameterTypes)
        {
            if(cil == null) throw new ArgumentNullException(nameof(cil));

            // NOTE: (System.Private.CoreLib.dll) An unmanaged EmitCalli is avaialble only with netcoreapp2.1+: https://github.com/dotnet/corefx/issues/9800
            // System.Reflection.Emit.ILGeneration is just metadata while mscorlib/src/System/Reflection/Emit/DynamicILGenerator implements this since https://github.com/dotnet/coreclr/pull/16546

#if NETSTD

            // https://github.com/3F/Conari/issues/13#issuecomment-554010927

            cil.EmitCalli(opcode, CallingConventions.Standard, returnType, parameterTypes, null);

            if(cil.TryGetFieldValue(out object m_scope, "m_scope", true))
            {
                if(m_scope.TryGetFieldValue(out object m_tokens, "m_tokens", true))
                {
                    var tksig = (List<object>)m_tokens;
                    ((byte[])tksig[tksig.Count - 1])[0] = CallingConventionConverter.GetMdSigCallingConventionAsByte(unmanagedCallConv);
                }
            }

#else
            cil.EmitCalli(opcode, unmanagedCallConv, returnType, parameterTypes);
#endif
        }

#if NETSTD

        private static bool TryGetFieldValue(this object obj, out object value, string name, bool nonPublic = false)
        {
            FieldInfo fi = obj?.GetType().GetField(name, GetDefaultFlags(nonPublic, false));

            if(fi == null)
            {
                value = null;
                return false;
            }

            value = fi.GetValue(obj);
            return true;
        }

        private static BindingFlags GetDefaultFlags(bool nonPublic = false, bool isStatic = false)
        {
            var flags = BindingFlags.Public | (isStatic ? BindingFlags.Static : BindingFlags.Instance);

            if(nonPublic)
            {
                flags |= BindingFlags.NonPublic;
            }

            return flags;
        }

#endif
    }
}
