namespace AppSettingsStudio;

public partial class VariablesForm : Form
{
    public VariablesForm()
    {
        InitializeComponent();
        Icon = Res.MainIcon;
        UpdateListView();
        UpdateControls();
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        base.OnKeyDown(e);
        if (e.KeyCode == Keys.Escape)
        {
            Close();
            return;
        }

        if (e.KeyCode == Keys.F5)
        {
            UpdateListView();
        }
    }

    private void UpdateListView()
    {
        var existingVariableNames = listViewVariables.EnumerateTags<SettingsVariable>().Select(t => t.Name).ToHashSet(StringComparer.OrdinalIgnoreCase);

        foreach (var kv in Manager.Variables.OrderBy(kv => kv.Key))
        {
            existingVariableNames.Remove(kv.Key);
            var item = listViewVariables.Items.Find(kv.Key, false).FirstOrDefault();
            if (item == null)
            {
                item = new ListViewItem(kv.Key);
                item.Tag = kv.Value;
                item.Name = kv.Key;
                listViewVariables.Items.Add(item);
                item.SubItems.Add(kv.Value.Value);
            }
            item.SubItems[1].Text = kv.Value.Value;
        }

        foreach (var key in existingVariableNames)
        {
            listViewVariables.Items.RemoveByKey(key);
        }
        listViewVariables.AutoResizeColumnsWidth();
    }

    private void UpdateControls()
    {
        buttonModify.Enabled = listViewVariables.SelectedIndices.Count > 0;
        buttonRemove.Enabled = buttonModify.Enabled;
    }

    private void ButtonAdd_Click(object sender, EventArgs e)
    {
        var dlg = new VariableForm
        {
            Text = "Add new global variable"
        };
        if (dlg.ShowDialog(this) != DialogResult.OK)
            return;

        var variable = new SettingsVariable(dlg.textBoxName.Text, dlg.textBoxValue.Text);
        Manager.Variables[variable.Name] = variable;
        Manager.SaveVariables();
        UpdateListView();
    }

    private void ButtonModify_Click(object sender, EventArgs e)
    {
        var variable = listViewVariables.SelectedItems.EnumerateTags<SettingsVariable>().FirstOrDefault();
        if (variable == null)
            return;

        var dlg = new VariableForm()
        {
            Text = "Modify global variable"
        };

        dlg.textBoxName.Enabled = false;
        dlg.textBoxName.Text = variable.Name;
        dlg.textBoxValue.Text = variable.Value;
        if (dlg.ShowDialog(this) != DialogResult.OK)
            return;

        if (Manager.Variables.TryGetValue(variable.Name, out variable))
        {
            variable.Value = dlg.textBoxValue.Text;
            Manager.SaveVariables();
            UpdateListView();
        }
    }

    private void ButtonRemove_Click(object sender, EventArgs e)
    {
        var variable = listViewVariables.SelectedItems.EnumerateTags<SettingsVariable>().FirstOrDefault();
        if (variable == null)
            return;

        if (this.ShowConfirm($"Are you sure you want to delete the '{variable.Name}' global variable?") != DialogResult.Yes)
            return;

        if (Manager.Variables.Remove(variable.Name))
        {
            Manager.SaveVariables();
            UpdateListView();
        }
    }

    private void ListViewVariables_SelectedIndexChanged(object sender, EventArgs e) => UpdateControls();
    private void ListViewVariables_MouseDoubleClick(object sender, MouseEventArgs e) => ButtonModify_Click(sender, e);
}
