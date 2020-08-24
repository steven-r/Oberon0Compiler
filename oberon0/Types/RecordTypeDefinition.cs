#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Oberon0.Compiler.Definitions;

namespace Oberon0.Compiler.Types
{
    public class RecordTypeDefinition : TypeDefinition
    {
        public RecordTypeDefinition()
            : base(BaseTypes.Record)
        {
            Elements = new List<Declaration>();
        }

        private RecordTypeDefinition(string name)
            : base(BaseTypes.Record)
        {
            Name = name;
            Elements = new List<Declaration>();
        }

        public List<Declaration> Elements { get; }

        public override TypeDefinition Clone(string name)
        {
            var r = new RecordTypeDefinition(name);
            r.Elements.AddRange(Elements);
            return r;
        }

        public override bool IsAssignable(TypeDefinition sourceType)
        {
            if (!(sourceType is RecordTypeDefinition rt))
            {
                return false;
            }

            if (Name == null || sourceType.Name == null
             || Elements.Count != rt.Elements.Count)
            {
                return false;
            }

            return Name == sourceType.Name;
        }

        [ExcludeFromCodeCoverage]
        public override string ToString()
        {
            return $"RECORD {Name}";
        }
    }
}
