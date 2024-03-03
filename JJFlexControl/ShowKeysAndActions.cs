using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace JJFlexControl
{
    public partial class ShowKeysAndActions : Form
    {
        private FlexControl topLevel;
        internal List<FlexControl.KeyAction_t> actions;

        public ShowKeysAndActions(FlexControl top, List<FlexControl.KeyAction_t> initialActions)
        {
            InitializeComponent();

            topLevel = top;
            actions = initialActions;
        }

        private void ShowKeysAndActions_Activated(object sender, EventArgs e)
        {
            DialogResult = DialogResult.None;
            showDefinedKeys();
        }

        private void showDefinedKeys()
        {
            DefinedKeysList.Items.Clear();
            foreach (FlexControl.KeyAction_t ka in actions)
            {
                if (topLevel.FindAction(ka.ActionName, topLevel.ValidActions) != null)
                {
                    string txt = topLevel.ValidKeysMap[ka.Command].Description;
                    DefinedKeysList.Items.Add(txt);
                }
            }
        }

        private void DefinedKeysList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DefinedKeysList.SelectedIndex == -1) return;

            FlexControl.KeyAction_t ka = actions[DefinedKeysList.SelectedIndex];
            FlexControl.Action_t action = topLevel.FindAction(ka.ActionName, topLevel.ValidActions);
            if (action != null)
            {
                ActionBox.Text = action.Description;
                ValueBox.Text = (action.ActionValue != null) ? action.ActionValue(action) : "";
            }
        }

        private void UpdateButton_Click(object sender, EventArgs e)
        {
            if (DefinedKeysList.SelectedIndex == -1) return;

            string key = actions[DefinedKeysList.SelectedIndex].Command;
            int id = FlexControl.ValidKeys.IndexOf(topLevel.ValidKeysMap[key]);
            SetupKeysAndActions setupKeys = new SetupKeysAndActions(topLevel, actions, id);
            Form theForm = (Form)setupKeys;
            if (theForm.ShowDialog() == DialogResult.OK)
            {
                actions = setupKeys.KeyActions;
            }
            theForm.Dispose();
            showDefinedKeys();
        }

        private void DoneButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
