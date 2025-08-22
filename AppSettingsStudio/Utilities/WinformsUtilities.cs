namespace AppSettingsStudio.Utilities;

public static class WinformsUtilities
{
    public const int WHERE_NOONE_CAN_SEE_ME = -32000; // from \windows\core\ntuser\kernel\userk.h

    public static TreeNode? FindByFullPath(this TreeView treeView, string fullPath)
    {
        ArgumentNullException.ThrowIfNull(treeView);
        ArgumentNullException.ThrowIfNull(fullPath);

        if (fullPath.StartsWith(Path.AltDirectorySeparatorChar.ToString()) || fullPath.StartsWith(Path.DirectorySeparatorChar.ToString()))
        {
            fullPath = fullPath[1..];
        }

        foreach (var child in treeView.Nodes.Cast<TreeNode>())
        {
            if (child.FullPath == fullPath)
                return child;

            var found = child.FindByFullPath(fullPath);
            if (found != null)
                return found;
        }
        return null;
    }

    public static void OpenFile(string fileName)
    {
        ArgumentNullException.ThrowIfNull(fileName);
        var psi = new ProcessStartInfo
        {
            FileName = fileName,
            UseShellExecute = true
        };
        Process.Start(psi);
    }

    public static void OpenExplorer(string? directoryPath)
    {
        if (directoryPath == null)
            return;

        if (!IOUtilities.PathIsDirectory(directoryPath))
            return;

        // see http://support.microsoft.com/kb/152457/en-us
        Process.Start("explorer.exe", "/e,/root,/select," + directoryPath);
    }

    public static void AutoResizeColumnsWidth(this ListView lv)
    {
        // https://learn.microsoft.com/en-us/dotnet/api/system.windows.forms.columnheader.width#remarks
        ArgumentNullException.ThrowIfNull(lv);
        for (var i = 0; i < lv.Columns.Count; i++)
        {
            lv.Columns[i].Width = -2;
        }
    }

    public static void RemoveUpToRootIfEmpty(this TreeNode node)
    {
        if (node == null)
            return;

        var parent = node.Parent;
        node.Remove();
        if (parent != null && parent.Nodes.Count == 0)
        {
            parent.RemoveUpToRootIfEmpty();
        }
    }

    public static IEnumerable<TreeNode> EnumerateAllChildNodes(this TreeNode node)
    {
        ArgumentNullException.ThrowIfNull(node);
        return node.Nodes.EnumerateAllChildNodes();
    }

    public static IEnumerable<TreeNode> EnumerateAllChildNodes(this TreeView treeView)
    {
        ArgumentNullException.ThrowIfNull(treeView);
        return treeView.Nodes.EnumerateAllChildNodes();
    }

    public static IEnumerable<TreeNode> EnumerateAllChildNodes(this TreeNodeCollection collection)
    {
        ArgumentNullException.ThrowIfNull(collection);
        foreach (var node in collection.Cast<TreeNode>())
        {
            yield return node;
            foreach (var child in node.Nodes.EnumerateAllChildNodes())
            {
                yield return child;
            }
        }
    }

    public static TreeNode? FindByFullPath(this TreeNode treeNode, string fullPath)
    {
        ArgumentNullException.ThrowIfNull(treeNode);
        ArgumentNullException.ThrowIfNull(fullPath);

        if (fullPath.StartsWith(Path.AltDirectorySeparatorChar.ToString()) || fullPath.StartsWith(Path.DirectorySeparatorChar.ToString()))
        {
            fullPath = fullPath[1..];
        }

        foreach (var child in treeNode.Nodes.Cast<TreeNode>())
        {
            if (child.FullPath == fullPath)
                return child;

            var found = child.FindByFullPath(fullPath);
            if (found != null)
                return found;
        }
        return null;
    }

    public static IEnumerable<T> EnumerateTags<T>(this ListView listView)
    {
        ArgumentNullException.ThrowIfNull(listView);
        return listView.Items.EnumerateTags<T>();
    }

    public static IEnumerable<T> EnumerateTags<T>(this ListView.ListViewItemCollection collection)
    {
        ArgumentNullException.ThrowIfNull(collection);
        foreach (ListViewItem item in collection)
        {
            if (item.Tag is T tag)
                yield return tag;
        }
    }

    public static IEnumerable<T> EnumerateTags<T>(this ListView.SelectedListViewItemCollection collection)
    {
        ArgumentNullException.ThrowIfNull(collection);
        foreach (ListViewItem item in collection)
        {
            if (item.Tag is T tag)
                yield return tag;
        }
    }

    public static IEnumerable<TreeNode> EnumerateNodeAndParents(this TreeNode? node)
    {
        if (node == null)
            yield break;

        yield return node;
        foreach (var parent in node.Parent.EnumerateNodeAndParents())
        {
            yield return parent;
        }
    }

    public static IEnumerable<object?> EnumerateNodeAndParentsTag(this TreeNode? node)
    {
        if (node == null)
            yield break;

        yield return node.Tag;
        foreach (var parent in node.Parent.EnumerateNodeAndParents())
        {
            yield return parent.Tag;
        }
    }

    public static IEnumerable<T?> EnumerateNodeAndParentsTag<T>(this TreeNode? node)
    {
        if (node == null)
            yield break;

        if (node.Tag != null)
        {
            if (typeof(T).IsAssignableFrom(node.Tag.GetType()))
                yield return (T)node.Tag;
        }

        foreach (var parent in node.Parent.EnumerateNodeAndParents())
        {
            if (parent.Tag != null)
            {
                if (typeof(T).IsAssignableFrom(parent.Tag.GetType()))
                    yield return (T)parent.Tag;
            }
        }
    }

    public static TreeNode? GetFirstNodeWhere(this TreeNode? node, Func<TreeNode, bool> predicate) => node.GetFirstNodeWhere(n => predicate(n) ? n : null);
    public static T? GetFirstNodeWhere<T>(this TreeNode? node, Func<TreeNode, T?> predicate) where T : class
    {
        ArgumentNullException.ThrowIfNull(predicate);
        if (node == null)
            return default;

        var item = predicate(node);
        if (item != default)
            return item;

        foreach (var child in node.Nodes.Cast<TreeNode>())
        {
            item = GetFirstNodeWhere(child, predicate);
            if (item != default)
                return item;
        }
        return default;
    }

    public static T? GetFirstTagWhere<T>(this TreeNode? node, Func<T, bool> predicate) where T : class
    {
        ArgumentNullException.ThrowIfNull(predicate);
        if (node == null)
            return default;

        T? item;
        if (node.Tag != null)
        {
            if (typeof(T).IsAssignableFrom(node.Tag.GetType()))
            {
                item = (T)node.Tag;
                if (predicate(item))
                    return item;
            }
        }

        foreach (var child in node.Nodes.Cast<TreeNode>())
        {
            item = GetFirstTagWhere(child, predicate);
            if (item != default)
                return item;
        }
        return default;
    }

    public static TreeNode? GetFirstNodeWhereTag<T>(this TreeNode? node, Func<T, bool> predicate) where T : class
    {
        ArgumentNullException.ThrowIfNull(predicate);
        if (node == null)
            return default;

        T? item;
        if (node.Tag != null)
        {
            if (typeof(T).IsAssignableFrom(node.Tag.GetType()))
            {
                item = (T)node.Tag;
                if (predicate(item))
                    return node;
            }
        }

        foreach (var child in node.Nodes.Cast<TreeNode>())
        {
            item = child.GetFirstTagWhere(predicate);
            if (item != default)
                return child;
        }
        return default;
    }

    public static void SetImageIndex(this TreeNode node, int index)
    {
        if (node == null)
            return;

        node.ImageIndex = index;
        node.SelectedImageIndex = index;
    }

    public static void SafeBeginInvoke(this Control control, Action action)
    {
        ArgumentNullException.ThrowIfNull(control);
        ArgumentNullException.ThrowIfNull(action);
        if (control.InvokeRequired)
        {
            control.BeginInvoke(action);
            return;
        }

        action();
    }

    public static IAsyncResult BeginInvoke(this Control control, Action action)
    {
        ArgumentNullException.ThrowIfNull(control);
        ArgumentNullException.ThrowIfNull(action);
        return control.BeginInvoke(action);
    }

    public static Task<T?> BeginInvoke<T>(this Control control, Func<T> action)
    {
        ArgumentNullException.ThrowIfNull(control);
        ArgumentNullException.ThrowIfNull(action);
        if (!control.IsHandleCreated)
            return Task.FromResult<T?>(default);

        return Task.Factory.FromAsync(control.BeginInvoke(action), r => (T?)control.EndInvoke(r));
    }
}
