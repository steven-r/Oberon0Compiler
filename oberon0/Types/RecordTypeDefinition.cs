#region copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecordTypeDefinition.cs" company="Stephen Reindl">
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>
// <summary>
//     Part of oberon0 - Oberon0Compiler/RecordTypeDefinition.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Oberon0.Compiler.Types
{
    using System.Collections.Generic;
    using System.Linq;

    using Oberon0.Compiler.Definitions;

    public class RecordTypeDefinition : TypeDefinition
    {
        public RecordTypeDefinition()
            : base(BaseTypes.Record)
        {
            this.Elements = new List<Declaration>();
        }

        public RecordTypeDefinition(string name)
            : base(BaseTypes.Record)
        {
            this.Name = name;
            this.Elements = new List<Declaration>();
        }

        public List<Declaration> Elements { get; }

        public override TypeDefinition Clone(string name)
        {
            var r = new RecordTypeDefinition(name);
            r.Elements.AddRange(this.Elements);
            return r;
        }

        public override bool IsAssignable(TypeDefinition sourceType)
        {
            if (!(sourceType is RecordTypeDefinition rt))
            {
                return false;
            }

            if ((Name == null || sourceType.Name == null) 
                || (Elements.Count != rt.Elements.Count))
            {
                return false;
            }

            if (Elements.Where((t, i) => !t.Type.IsAssignable(rt.Elements[i].Type)).Any())
            {
                return false;
            }

            return true;
        }
    }
}