// Copyright (c) Six Labors.
// Licensed under the Apache License, Version 2.0.

using System.Collections.Generic;
using System.Numerics;

using SixLabors.Fonts.Tables.General.Kern;

namespace SixLabors.Fonts.Tables.General
{
    [TableName(TableName)]
    internal sealed class KerningTable : Table
    {
        private const string TableName = "kern";
        private readonly KerningSubTable[] kerningSubTable;

        public KerningTable(KerningSubTable[] kerningSubTable)
        {
            this.kerningSubTable = kerningSubTable;
        }

        public static KerningTable Load(FontReader reader)
        {
            using (BinaryReader? binaryReader = reader.TryGetReaderAtTablePosition(TableName))
            {
                if (binaryReader is null)
                {
                    // this table is optional.
                    return new KerningTable(new KerningSubTable[0]);
                }

                // move to start of table
                return Load(binaryReader);
            }
        }

        public static KerningTable Load(BinaryReader reader)
        {
            // Type   | Field    | Description
            // -------|----------|-----------------------------------------
            // uint16 | version  | Table version number(0)
            // uint16 | nTables  | Number of subtables in the kerning table.
            ushort version = reader.ReadUInt16();
            ushort subtableCount = reader.ReadUInt16();

            var tables = new List<KerningSubTable>(subtableCount);
            for (int i = 0; i < subtableCount; i++)
            {
                var t = KerningSubTable.Load(reader); // returns null for unknown/supported table format
                if (t != null)
                {
                    tables.Add(t);
                }
            }

            return new KerningTable(tables.ToArray());
        }

        public Vector2 GetOffset(ushort left, ushort right)
        {
            Vector2 result = Vector2.Zero;
            foreach (KerningSubTable sub in this.kerningSubTable)
            {
                sub.ApplyOffset(left, right, ref result);
            }

            return result;
        }
    }
}
