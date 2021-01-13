# ColorConverter
Convert System.Drawing.Color to RGB and Hex Value

```
using ColorConverter;
```

```
ColorConverting converting = new ColorConverting();

string _toHex = converting.ToHex(Color.Azure);
string _toRgb = converting.ToRGB(Color.Bisque);
string _rgbToHex = converting.RgbToHex(250, 160, 60);

Console.WriteLine(_toHex);
Console.WriteLine(_toRgb);
Console.WriteLine(_rgbToHex);
```