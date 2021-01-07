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

using System.Runtime.InteropServices;

namespace net.r_eg.DotNet.System.Reflection.Emit
{
    /// <summary>
    /// For internal m_signature processing with unmanaged EmitCalli.
    /// See <see cref="ILGeneratorExtension"/>.
    /// </summary>
    internal static class CallingConventionConverter
    {
        internal static MdSigCallingConvention GetValue(CallingConvention conv)
        {
            switch(conv)
            {
                case CallingConvention.Cdecl: {
                    return MdSigCallingConvention.C;
                }

                case CallingConvention.Winapi:
                case CallingConvention.StdCall: {
                    return MdSigCallingConvention.StdCall;
                }

                case CallingConvention.ThisCall: {
                    return MdSigCallingConvention.ThisCall;
                }

                case CallingConvention.FastCall: {
                    return MdSigCallingConvention.FastCall;
                }
            }

            return MdSigCallingConvention.Default;
        }

        internal static byte GetMdSigCallingConventionAsByte(CallingConvention conv) => (byte)GetValue(conv);
    }
}
