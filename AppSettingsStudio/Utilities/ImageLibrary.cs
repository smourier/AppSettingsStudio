namespace AppSettingsStudio.Utilities;

public static class ImageLibrary
{
    private const int _size = 16;
    private static readonly Lazy<ImageList> _images = new(GetImages);
    public static ImageList Images => _images.Value;
    private static readonly Lock _lock = new();

    private static readonly Dictionary<string, int> _customIndices = [];

    public static void SetImageIndex(this TreeNode node, ImageLibraryIndex index) => node.SetImageIndex((int)index);
    public static int AddOrGetImageIndex(string iconFilePath)
    {
        ArgumentNullException.ThrowIfNull(iconFilePath);

        lock (_lock)
        {
            if (!_customIndices.TryGetValue(iconFilePath, out var index))
            {
                Bitmap bitmap;
                var ext = Path.GetExtension(iconFilePath);
                if (ext.EqualsIgnoreCase(".dll") || ext.EqualsIgnoreCase(".exe"))
                {
                    using var icon = Icon.ExtractIcon(iconFilePath, 0, true);
                    if (icon == null)
                    {
                        index = -1;
                        _customIndices[iconFilePath] = index;
                        return index;
                    }

                    bitmap = Bitmap.FromHicon(icon.Handle);

                    if (bitmap.Size.Width != _size && bitmap.Size.Height != _size)
                    {
                        var sized = bitmap.Resize(_size, _size);
                        bitmap.Dispose();
                        bitmap = sized;
                    }
                }
                else
                {
                    bitmap = new Bitmap(iconFilePath);
                }

                Images.Images.AddStrip(bitmap);
                index = Images.Images.Count - 1;
                _customIndices[iconFilePath] = index;
            }
            return index;
        }
    }

    public static void ConsolidateImages(TreeNode? node)
    {
        if (node == null || node.Nodes.Count == 0)
            return;

        if (node.Nodes.Count == 1)
        {
            if (node.Nodes[0].ImageIndex >= 0)
            {
                node.SetImageIndex(node.Nodes[0].ImageIndex);
            }
            return;
        }

        var indices = node.Nodes.Cast<TreeNode>().Select(n => n.ImageIndex).ToArray();
        if (indices.Any(i => i < 0))
            return;

        if (indices.ToHashSet().Count == 1) // all sames?
        {
            node.SetImageIndex(node.Nodes[0].ImageIndex);
            return;
        }

        _ = Task.Run(() =>
        {
            lock (_lock)
            {
                var bytesList = new List<byte[]>();
                var width = 0;
                var height = 0;
                for (var i = 0; i < indices.Length; i++)
                {
                    if (node.TreeView?.ImageList?.Images[indices[i]] is not Bitmap bmp)
                        return;

                    var data = bmp.LockBits(new Rectangle(new(), bmp.Size), ImageLockMode.ReadOnly, bmp.PixelFormat);
                    if (i == 0)
                    {
                        width = data.Width;
                        height = data.Height;
                    }

                    // same size?
                    if (i > 0 && (data.Height * data.Stride != bytesList[0].Length || width != data.Width || height != data.Height))
                    {
                        bmp.UnlockBits(data);
                        return;
                    }

                    var bytes = new byte[data.Height * data.Stride];
                    Marshal.Copy(data.Scan0, bytes, 0, bytes.Length);
                    bmp.UnlockBits(data);
                    bytesList.Add(bytes);
                }

                // don't use stride in comparisons
                for (var i = 1; i < bytesList.Count; i++)
                {
                    for (var r = 0; r < height; r++)
                    {
                        for (var c = 0; c < width; c++)
                        {
                            var idx = r * width + c;
                            if (bytesList[0][idx] != bytesList[i][idx])
                                return;
                        }
                    }
                }

                node.TreeView?.BeginInvoke(() => node.SetImageIndex(indices[0]));
            }
        });
    }

    private static ImageList GetImages()
    {
        using var stream = typeof(ImageLibrary).Assembly!.GetManifestResourceStream(typeof(Resources.Res).Namespace + ".ImageLibrary32.bmp");
        return stream == null ? throw new InvalidOperationException() : GetImageList(stream);
    }

    private static ImageList GetImageList(Stream imageStream)
    {
        var list = new ImageList
        {
            ColorDepth = ColorDepth.Depth32Bit,
            ImageSize = new Size(_size, _size),
        };

        var bitmap = new Bitmap(imageStream);
        list.Images.AddStrip(bitmap);
        return list;
    }
}
