#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using System.Collections.Generic;
using Oberon0.Compiler.Definitions;

namespace Oberon0.Compiler.Types
{
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
            if (!(sourceType is RecordTypeDefinition rt)) return false;

            if (Name == null || sourceType.Name == null
                             || Elements.Count != rt.Elements.Count)
                return false;

            return Name == sourceType.Name;
        }
    }
}