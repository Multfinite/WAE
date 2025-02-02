﻿using Microsoft.Xna.Framework;
using TSMapEditor.GameMath;
using TSMapEditor.Models;

namespace TSMapEditor.Rendering.ObjectRenderers
{
    public sealed class AnimRenderer : ObjectRenderer<Animation>
    {
        public AnimRenderer(RenderDependencies renderDependencies) : base(renderDependencies)
        {
        }

        protected override Color ReplacementColor => Color.Orange;

        protected override CommonDrawParams GetDrawParams(Animation gameObject)
        {
            return new CommonDrawParams(TheaterGraphics.AnimTextures[gameObject.AnimType.Index], gameObject.AnimType.ININame);
        }

        protected override bool ShouldRenderReplacementText(Animation gameObject)
        {
            // Never draw this for animations
            return false;
        }

        protected override void Render(Animation gameObject, int yDrawPointWithoutCellHeight, Point2D drawPoint, CommonDrawParams commonDrawParams)
        {
            if (commonDrawParams.Graphics == null)
                return;

            int frameIndex = gameObject.AnimType.ArtConfig.Start;
            if (gameObject.IsTurretAnim)
            {
                // Turret anims have their facing frames reversed
                byte facing = (byte)(255 - gameObject.Facing - 31);
                frameIndex = facing / (512 / commonDrawParams.Graphics.Frames.Length);
            }

            float alpha = 1.0f;

            // Translucency values don't seem to directly map into MonoGame alpha values,
            // this will need some investigating into
            switch (gameObject.AnimType.ArtConfig.Translucency)
            {
                case 75:
                    alpha = 0.1f;
                    break;
                case 50:
                    alpha = 0.2f;
                    break;
                case 25:
                    alpha = 0.5f;
                    break;
            }

            DrawShadow(gameObject, commonDrawParams, drawPoint, yDrawPointWithoutCellHeight);

            DrawObjectImage(gameObject, commonDrawParams, commonDrawParams.Graphics,
                frameIndex, Color.White * alpha,
                gameObject.IsBuildingAnim, gameObject.GetRemapColor() * alpha,
                drawPoint, yDrawPointWithoutCellHeight);
        }

        protected override void DrawShadow(Animation gameObject, CommonDrawParams drawParams, Point2D drawPoint, int initialYDrawPointWithoutCellHeight)
        {
            if (!Constants.DrawBuildingAnimationShadows && gameObject.IsBuildingAnim)
                return;

            int shadowFrameIndex = gameObject.GetShadowFrameIndex(drawParams.Graphics.Frames.Length);
            if (gameObject.IsTurretAnim)
            {
                // Turret anims have their facing frames reversed
                byte facing = (byte)(255 - gameObject.Facing - 31);
                shadowFrameIndex += facing / (512 / drawParams.Graphics.Frames.Length);
            }

            if (shadowFrameIndex > 0 && shadowFrameIndex < drawParams.Graphics.Frames.Length)
            {
                DrawObjectImage(gameObject, drawParams, drawParams.Graphics, shadowFrameIndex,
                    new Color(0, 0, 0, 128), false, Color.White, drawPoint, initialYDrawPointWithoutCellHeight);
            }
        }
    }
}
