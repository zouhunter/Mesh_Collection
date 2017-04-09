//using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.Events;
//using System.Collections.Generic;

//public static class TextureUtility
//{
//    /// <summary>
//    /// 利用贴图获取网格贴图
//    /// </summary>
//    /// <param name="meshStruct"></param>
//    /// <param name="texture"></param>
//    /// <returns></returns>
//    public static Texture2D GetLineTexture(MeshStruct meshData, Texture2D texture, int lineWidth = 1)
//    {
//        int width = texture.width;
//        int height = texture.height;
//        Texture2D newTexture = new Texture2D(width, height, TextureFormat.ARGB32, false);
//        //newTexture.alphaIsTransparency = true;
//        Color[] colors = new Color[width * height];
//        for (int i = 0; i < colors.Length; i++)
//        {
//            colors[i] = Color.clear;
//        }
//        newTexture.SetPixels(colors);

//        for (int k = 0; k < meshData.triangles.Length - 2; k += 3)
//        {
//            Vector2[] _uvs = new Vector2[3];
//            int tID1 = meshData.triangles[k];
//            int tID2 = meshData.triangles[k + 1];
//            int tID3 = meshData.triangles[k + 2];

//            _uvs[0] = meshData.uvs[tID1];
//            _uvs[1] = meshData.uvs[tID2];
//            _uvs[2] = meshData.uvs[tID3];

//            for (int i = 0; i < _uvs.Length; ++i)
//            {
//                for (int j = i + 1; j < _uvs.Length; ++j)
//                {
//                    if (_uvs[i].x == _uvs[j].x || _uvs[i].y == _uvs[j].y)//轴向
//                    {
//                        float uvx1 = _uvs[i].x;
//                        float uvy1 = _uvs[i].y;
//                        float uvx2 = _uvs[j].x;
//                        float uvy2 = _uvs[j].y;

//                        int px1 = (int)(uvx1 * width);
//                        int py1 = (int)(uvy1 * height);
//                        int px2 = (int)(uvx2 * width);
//                        int py2 = (int)(uvy2 * height);

//                        int minpx = px1 < px2 ? px1 : px2;
//                        int minpy = py1 < py2 ? py1 : py2;

//                        int pw = Mathf.FloorToInt(Mathf.Abs((uvx1 - uvx2) * width));
//                        int ph = Mathf.FloorToInt(Mathf.Abs((uvy1 - uvy2) * height));

//                        if (pw == 0)
//                        {
//                            int sx = minpx - lineWidth;

//                            for (int m = 0; m <= ph; m++)
//                            {
//                                for (int n = 0; n <= lineWidth * 2; n++)
//                                {
//                                    if (sx + n >= 0 && sx + n <= width)
//                                    {
//                                        Color colorline = texture.GetPixel(sx + n, minpy + m);
//                                        newTexture.SetPixel(sx + n, minpy + m, colorline);
//                                    }
//                                }
//                            }
//                        }

//                        if (ph == 0)
//                        {
//                            int sy = minpy - lineWidth;

//                            for (int m = 0; m <= lineWidth * 2; m++)
//                            {
//                                for (int n = 0; n <= pw; n++)
//                                {
//                                    if (sy + m >= 0 && sy + m <= height)
//                                    {
//                                        Color colorline = texture.GetPixel(minpx + n, sy + m);
//                                        newTexture.SetPixel(minpx + n, sy + m, colorline);
//                                    }
//                                }
//                            }
//                        }

//                    }
//                }
//            }
//        }
//        newTexture.Apply();
//        return newTexture;
//    }

   
//}
