// ***********************************************************************
// Assembly         : Oberon0.Generator.ORisc
// Author           : stephen@stephenreindl.net
// Created          : 10-22-2016
//
// Last Modified By : stephen@stephenreindl.net
// Last Modified On : 10-22-2016
// ***********************************************************************
// <copyright file="Operations.cs" company="Reindl IT">
//     Copyright © 2016 Stephen Reindl
// </copyright>
// <summary></summary>
// ***********************************************************************
// ReSharper disable InconsistentNaming

namespace Oberon0.Generator.ORisc
{

    /// <summary>
    /// Instructions for registers (Format F0 &amp; F1)
    /// </summary>
    public enum RegisterInstructions
    {
        /// <summary>
        /// <c>MOV a, n    R.a := n</c>
        /// </summary>
        MOV = 0,

        /// <summary>
        /// <c>LSL a, b, n R.a := R.b &lt;&lt; n</c>
        /// </summary>
        LSL = 1,

        /// <summary>
        /// <c>ASR a, b, n R.a := R.b &gt;&gt; n</c>
        /// </summary>
        ASR = 2,

        /// <summary>
        /// <c>ROR a, b, n R.a := R.b rot n</c>
        /// </summary>
        ROR = 3,

        /// <summary>
        /// <c>AND a, b, n R.a := R.b &amp; n</c>
        /// </summary>
        AND = 4,

        /// <summary>
        /// <c>ANN a, b, n R.a := R.b &amp; ~n</c>
        /// </summary>
        ANN = 5,

        /// <summary>
        /// <c>IOR a, b, n R.a := R.b or n</c>
        /// </summary>
        IOR = 6,

        /// <summary>
        /// <c>XOR a, b, n R.a := R.b xor n</c>
        /// </summary>
        XOR = 7,

        /// <summary>
        /// <c>ADD a, b, n R.a := R.b + n</c>
        /// </summary>
        ADD = 8,

        /// <summary>
        /// <c>SUB a, b, n R.a := R.b - n</c>
        /// </summary>
        SUB = 9,

        /// <summary>
        /// <c>MUL a, b, n R.a := R.b * n</c>
        /// </summary>
        MUL = 10,

        /// <summary>
        /// <c>DIV a, b, n R.a := R.b / n</c>
        /// </summary>
        DIV = 11,
    }
}