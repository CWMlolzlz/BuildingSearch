using UnityEngine;
using ColossalFramework.UI;
using System.Reflection;

namespace BuildingSearch {
    public class TextureLoader {
        public static UITextureAtlas CreateTextureAtlas(string atlasName, string[] spriteNames, string assemblyPath) {
            int maxSize = 1024;
            Texture2D texture2D = new Texture2D(maxSize, maxSize, TextureFormat.ARGB32, false);
            Texture2D[] textures = new Texture2D[spriteNames.Length];
            Rect[] regions = new Rect[spriteNames.Length];

            for (int i = 0; i < spriteNames.Length; i++)
                textures[i] = TextureLoader.LoadTextureFromAssembly(assemblyPath + spriteNames[i] + ".png");

            regions = texture2D.PackTextures(textures, 2, maxSize);

            UITextureAtlas textureAtlas = ScriptableObject.CreateInstance<UITextureAtlas>();
            Material material = UnityEngine.Object.Instantiate<Material>(UIView.GetAView().defaultAtlas.material);
            material.mainTexture = texture2D;
            textureAtlas.material = material;
            textureAtlas.name = atlasName;

            for (int i = 0; i < spriteNames.Length; i++) {
                UITextureAtlas.SpriteInfo item = new UITextureAtlas.SpriteInfo {
                    name = spriteNames[i],
                    texture = textures[i],
                    region = regions[i],
                };

                textureAtlas.AddSprite(item);
            }

            return textureAtlas;
        }


        /**
         * Method sourced from SamsamTSs Airport Roads mod for Cities Skylines
         * It loads in a Texture2D from a file.
         * https://github.com/SamsamTS/CS-AirportRoads/blob/master/AirportRoads/AirportRoads.cs
         **/
        private static Texture2D LoadTextureFromAssembly(string path) {
            var assembly = Assembly.GetExecutingAssembly();
            System.IO.Stream manifestResourceStream = assembly.GetManifestResourceStream(path);
            byte[] array = new byte[manifestResourceStream.Length];
            manifestResourceStream.Read(array, 0, array.Length);

            Texture2D texture2D = new Texture2D(2, 2, TextureFormat.ARGB32, false);
            texture2D.LoadImage(array);

            return texture2D;
        }

        /**
         * Method sourced from SamsamTSs Airport Roads mod for Cities Skylines
         * Adds array of Texture2D objects to a UITextureAtlas
         * https://github.com/SamsamTS/CS-AirportRoads/blob/master/AirportRoads/AirportRoads.cs
         **/
        private static void AddTexturesInAtlas(UITextureAtlas atlas, Texture2D[] newTextures) {
            Texture2D[] textures = new Texture2D[atlas.count + newTextures.Length];

            for (int i = 0; i < atlas.count; i++) {
                // Locked textures workaround
                Texture2D texture2D = atlas.sprites[i].texture;

                RenderTexture renderTexture = RenderTexture.GetTemporary(texture2D.width, texture2D.height, 0);
                Graphics.Blit(texture2D, renderTexture);

                RenderTexture active = RenderTexture.active;
                texture2D = new Texture2D(renderTexture.width, renderTexture.height);
                RenderTexture.active = renderTexture;
                texture2D.ReadPixels(new Rect(0f, 0f, (float)renderTexture.width, (float)renderTexture.height), 0, 0);
                texture2D.Apply();
                RenderTexture.active = active;

                RenderTexture.ReleaseTemporary(renderTexture);

                textures[i] = texture2D;
                textures[i].name = atlas.sprites[i].name;
            }

            for (int i = 0; i < newTextures.Length; i++)
                textures[atlas.count + i] = newTextures[i];

            Rect[] regions = atlas.texture.PackTextures(textures, atlas.padding, 4096, false);

            atlas.sprites.Clear();

            for (int i = 0; i < textures.Length; i++) {
                UITextureAtlas.SpriteInfo spriteInfo = atlas[textures[i].name];
                atlas.sprites.Add(new UITextureAtlas.SpriteInfo {
                    texture = textures[i],
                    name = textures[i].name,
                    border = (spriteInfo != null) ? spriteInfo.border : new RectOffset(),
                    region = regions[i]
                });
            }

            atlas.RebuildIndexes();
        }
    }
}