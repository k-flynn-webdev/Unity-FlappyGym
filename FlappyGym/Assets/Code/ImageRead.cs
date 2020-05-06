using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageRead : MonoBehaviour
{
    private static Texture2D _tmpImg;
    private static Color32[] _cache;

    public static void SetImage(Texture2D image)
    {
        if (_tmpImg.Equals(image))
        {
            return;
        }

        _tmpImg = image;
        _cache = image.GetPixels32();
    }

    public static Color GetPixelXY(int x, int y)
    {
        if (x < 0 || y < 0)
        {
            return Color.white;
        }

        int tmpCoord = x + (y * _tmpImg.width);

        if (tmpCoord > _cache.Length)
        {
            return Color.white;
        }

        return _cache[tmpCoord];
    }


    public static Color[] GetPixelStripX(int x)
    {
        Color[] _tmp = new Color[_tmpImg.height];

        for (int i = 0; i < _tmp.Length; i++)
        {
            _tmp[i] = Color.white;
        }

        if (x < 0)
        {
            return _tmp;
        }

        for (int i = 0; i < _tmp.Length; i++)
        {
            _tmp[i] = GetPixelXY(x, i);
        }

        return _tmp;
    }

    public static Color[] GetPixelStripY(int y)
    {
        Color[] _tmp = new Color[_tmpImg.width];

        for (int i = 0; i < _tmp.Length; i++)
        {
            _tmp[i] = Color.white;
        }

        if (y < 0)
        {
            return _tmp;
        }

        for (int i = 0; i < _tmp.Length; i++)
        {
            _tmp[i] = _cache[i + (y * _tmpImg.width)];
        }

        return _tmp;
    }

    public static Color[] GetUniqueColours(Texture2D image)
    {
        Color32[] _tmpCache = image.GetPixels32();

        List<Color> _colours = new List<Color>();

        for (int i = 0, max = _tmpCache.Length; i < max; i++)
        {
            if (!_colours.Contains(_tmpCache[i]))
            {
                _colours.Add(_tmpCache[i]);
            }
        }

        Color[] _uniqueColours = new Color[_colours.Count];

        for (int i = 0, max = _colours.Count; i < max; i++)
        {
            _uniqueColours[i] = _colours[i];
        }


        return _uniqueColours;
    }
}
