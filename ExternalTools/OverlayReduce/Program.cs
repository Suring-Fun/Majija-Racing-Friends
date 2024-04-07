extern alias CoreCompat;

using CoreCompat.System.Drawing;
using CoreCompat.System.Drawing.Imaging;

Console.WriteLine("[Overlay reducer]");
Console.WriteLine("Reduces large image (up to 8196x8196) with small color pallete (4 colors) to small image (512x512) using the advance alphatest algorythm. Moreover it supposes that objects of selected colors (color 1, color 2, color 3, color 4) is over each other in a color order.");
Console.WriteLine();
Console.WriteLine("Please, enter file path:");
string path = Console.ReadLine();

BitmapData LocksBits(Bitmap b, ImageLockMode lm)
{
    var r = new System.Drawing.Rectangle();
    r.Width = b.Width;
    r.Height = b.Height;
    return b.LockBits(r, lm, PixelFormat.Format32bppArgb);
}

unsafe Color GetPixel(BitmapData data, int x, int y)
{
    byte* byteVector = (byte*)data.Scan0;
    byteVector += y * data.Stride;
    byteVector += x * 4;

    var color = Color.FromArgb(byteVector[3], byteVector[2], byteVector[1], byteVector[0]);
    return color;
}

unsafe void SetPixel(BitmapData data, int x, int y, Color c)
{
    byte* byteVector = (byte*)data.Scan0;
    byteVector += y * data.Stride;
    byteVector += x * 4;

    byteVector[3] = c.A;
    byteVector[2] = c.R;
    byteVector[1] = c.G;
    byteVector[0] = c.B;
}

Bitmap bitmap = new(path);
var refData = LocksBits(bitmap, ImageLockMode.ReadOnly);

Console.WriteLine("Enter target size: _x_");
(var w, var h) = ParseSize(Console.ReadLine());

Bitmap target = new(w, h, PixelFormat.Format32bppArgb);
var editData = LocksBits(target, ImageLockMode.WriteOnly);

var c1 = ReadColor(1);
var c2 = ReadColor(2);
var c3 = ReadColor(3);
var c4 = ReadColor(4);

Console.WriteLine("Enter distance radius:");
int distanceFromBounds = int.Parse(Console.ReadLine());

int distanceFromBounds2 = distanceFromBounds * distanceFromBounds;

Console.WriteLine("Preparing");
// Preparing the offset indices.
List<(int x, int y, float dist)> _indices = new();

for (int y = -distanceFromBounds; y <= distanceFromBounds; ++y)
{
    for (int x = -distanceFromBounds; x <= distanceFromBounds; ++x)
    {
        int dist = x * x + y * y;
        if (dist <= distanceFromBounds2)
        {
            _indices.Add((x, y, MathF.Sqrt(dist)));
        }
    }
}

Console.WriteLine("Processing...");

float[] _dists = new float[4];

for (int x = 0; x < w; ++x)
{
    for (int y = 0; y < h; ++y)
    {
        int globX = x * bitmap.Width / w;
        int globY = y * bitmap.Height / h;

        (int fi, int se, int th, int fo) = GetColors(globX, globY);
        // Console.WriteLine(fi);
        SetPixel(editData, x, y, Color.FromArgb(fi, se, th, fo));
    }

    Console.WriteLine($"Col {x}/{w}");
}

Console.WriteLine("Finishing");

target.UnlockBits(editData);
bitmap.UnlockBits(refData);
target.Save(Path.Combine(Path.GetDirectoryName(path), "output.png"));


(int _1, int _2, int _3, int _4) GetColors(int x, int y)
{
    Color current = GetPixel(refData, x, y);
    // Console.WriteLine($"{current.A}, {current.R}, {current.G}, {current.B}");
    int currentIndex = GetMostSimilarColor(current, c1, c2, c3, c4);

    for (int _ = 0; _ < 4; ++_)
    {
        if (currentIndex < _)
        {
            _dists[_] = -distanceFromBounds;
        }
        else
        {
            _dists[_] = distanceFromBounds;
        }
    }

    foreach (var offset in _indices)
    {
        int newX = x + offset.x;
        int newY = y + offset.y;

        if (newX < 0 || newY < 0 || newX >= bitmap.Width || newY >= bitmap.Height)
            continue;

        int colorIndex = GetMostSimilarColor(GetPixel(refData, newX, newY), c1, c2, c3, c4);

        ProcessDist(0);
        ProcessDist(1);
        ProcessDist(2);
        ProcessDist(3);

        void ProcessDist(int distanceToProcess)
        {
            bool isEmpty = colorIndex < distanceToProcess;
            if (currentIndex < distanceToProcess)
            {
                if (!isEmpty)
                {
                    _dists[distanceToProcess] = MathF.Max(_dists[distanceToProcess], -offset.dist);
                }
            }
            else
            {
                if (isEmpty)
                {
                    _dists[distanceToProcess] = MathF.Min(_dists[distanceToProcess], offset.dist);
                }
            }
        }
    }

    int NormalizeDistance(int distIndex)
    {
        float normalized = (_dists[distIndex] + distanceFromBounds) / (distanceFromBounds * 2);
        normalized = MathF.Max(Math.Min(normalized, 1f), 0f);

        return (int)(normalized * 255);
    }

    return (NormalizeDistance(0), NormalizeDistance(1), NormalizeDistance(2), NormalizeDistance(3));
}

(int w, int h) ParseSize(string str)
{
    string[] parts = str.Trim().Split('x');
    return (int.Parse(parts[0]), int.Parse(parts[1]));
}

Color ReadColor(int index)
{
    Console.WriteLine($"Enter color {index}: _,_,_");
    return ParseColor(Console.ReadLine());
}

Color ParseColor(string str)
{
    string[] parts = str.Trim().Split(',');
    return Color.FromArgb(int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[2]));
}

int GetSimilarityIndex(Color a, Color b)
    => Math.Abs(a.R - b.R) + Math.Abs(a.G - b.G) + Math.Abs(a.B - b.B);

int GetMostSimilarColor(Color current, Color _1, Color _2, Color _3, Color _4)
{
    if (current.A < 128)
        return -1;

    int index = 0;
    int similarity = GetSimilarityIndex(current, _1);

    int _;
    if (similarity > (_ = GetSimilarityIndex(current, _2)))
    {
        similarity = _;
        index = 1;
    }

    if (similarity > (_ = GetSimilarityIndex(current, _3)))
    {
        similarity = _;
        index = 2;
    }

    if (similarity > (_ = GetSimilarityIndex(current, _4)))
    {
        similarity = _;
        index = 3;
    }

    return index;
}