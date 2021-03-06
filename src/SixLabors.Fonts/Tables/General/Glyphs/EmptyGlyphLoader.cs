﻿// Copyright (c) Six Labors.
// Licensed under the Apache License, Version 2.0.

using System.Numerics;

namespace SixLabors.Fonts.Tables.General.Glyphs
{
    internal class EmptyGlyphLoader : GlyphLoader
    {
        private bool loop = false;
        private readonly Bounds fallbackEmptyBounds;
        private GlyphVector? glyph;

        public EmptyGlyphLoader(Bounds fallbackEmptyBounds)
        {
            this.fallbackEmptyBounds = fallbackEmptyBounds;
        }

        public override GlyphVector CreateGlyph(GlyphTable table)
        {
            if (this.loop)
            {
                if (this.glyph is null)
                {
                    this.glyph = new GlyphVector(new Vector2[0], new bool[0], new ushort[0], this.fallbackEmptyBounds);
                }

                return this.glyph.Value;
            }

            this.loop = true;
            return table.GetGlyph(0);
        }
    }
}
