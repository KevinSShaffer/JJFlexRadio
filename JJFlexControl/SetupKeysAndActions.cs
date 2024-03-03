using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using JJTrace;

namespace JJFlexControl
{
    public partial class SetupKeysAndActions : Form
    {
        // Output
        public List<FlexControl.KeyAction_t> KeyActions;

        private FlexControl topLevel;
        private int initialIndex;
        private bool keysReady;
        private bool actionsReady;

        public SetupKeysAndActions(FlexControl top, List<FlexControl.KeyAction_t> initialActions,
            int initialIndex = -1)
        {
            InitializeComponent();

            topLevel = top;
            this.initialIndex = initialIndex;
            // Make a copy of the actions.
            KeyActions = new List<FlexControl.KeyAction_t>();
            foreach(FlexControl.KeyAction_t k in initialActions)
            {
                KeyActions.Add(new FlexControl.KeyAction_t(k));
            }
        }

        private void SetupKeysAndActions_Load(object sender, EventArgs e)
        {
            keysReady = actionsReady = false;
            // List of valid keys.
            KeysListBox.DataSource = FlexControl.ValidKeys;
            KeysListBox.DisplayMember = "Description";
            KeysListBox.ValueMember = "Command";
            KeysListBox.SelectedIndex = initialIndex;

            ActionsBox.DataSource = topLevel.ValidActions;
            ActionsBox.DisplayMember = "Description";
            ActionsBox.ValueMember = "Name";

            DialogResult = DialogResult.None;
            keysReady = true;
        }

        private void KeysListBox_Enter(object sender, EventArgs e)
        {
            Tracing.TraceLine("KeysListBox_Enter", TraceLevel.Info);
            ActionsBox.Enabled = false;
            ActionsBox.Visible = false;
        }

        private void KeysListBox_Leave(object sender, EventArgs e)
        {
            bool keepOn = (KeysListBox.SelectedIndex != -1);
            Tracing.TraceLine("KeysListBox_Leave:" + keepOn.ToString(), TraceLevel.Info);
            if (keepOn)
            {
                actionsReady = true;
                ActionsBox.Enabled = true;
                ActionsBox.Visible = true;
                ActionsBox.SelectedIndexChanged += ActionsBox_SelectedIndexChanged;
                ActionsBox.Leave += ActionsBox_Leave;
                FlexControl.KeyAction_t k = topLevel.FindKeyAction((string)KeysListBox.SelectedValue, KeyActions);
                if (k != null)
                {
                    // This key has a value assigned.
                    // Set ActionsBox accordingly.
                    for (int i = 0; i < topLevel.ValidActions.Count; i++)
                    {
                        if (k.ActionName == topLevel.ValidActions[i].Name)
                        {
                            ActionsBox.SelectedIndex = i;
                            break;
                        }
                    }
                }
                else
                {
                    // This key's value not yet assigned, use the null action, action 0.
                    ActionsBox.SelectedIndex = 0;
                }
                ActionsBox.Focus();
            }
        }

        private void KeysListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool keepOn = (keysReady & (KeysListBox.SelectedIndex != -1));
            Tracing.TraceLine("KeysListBox_SelectedIndexChanged:" + keepOn.ToString(), TraceLevel.Info);
            if (!keepOn) return;

            //SuspendLayout();
            // Get ActionsBox position
            int y = Cursor.Position.Y + 20;
            ActionsBox.Location = new Point(100, y);

            // Setup the choices.
            actionsReady = false;
        }

        private void KeysListBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (KeysListBox.SelectedIndex == -1) return;

            // handle deletion
            if (e.KeyCode == Keys.Delete)
            {
                string key = (string)KeysListBox.SelectedValue;
                Tracing.TraceLine("KeysListBox_KeyDown:deleting:" + key, TraceLevel.Info);
                FlexControl.KeyAction_t k = topLevel.FindKeyAction(key, KeyActions);
                if (k != null)
                {
                    KeyActions.Remove(k);
                }
                e.Handled = true;
            }
        }

        private void ActionsBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool keepOn = (actionsReady & (ActionsBox.SelectedIndex != -1));
            Tracing.TraceLine("ActionsBox_SelectedIndexChanged:" + keepOn.ToString(), TraceLevel.Info);
            if (!keepOn) return;

            string key = (string)KeysListBox.SelectedValue;
            string value = (string)ActionsBox.SelectedValue;
            FlexControl.KeyAction_t k = topLevel.FindKeyAction(key, KeyActions);
            // Change or add the action.
            if (k != null)
            {
                // key already defined.
                k.ActionName = value;
            }
            else
            {
                KeyActions.Add(new FlexControl.KeyAction_t(key, value));
            }
        }

        private void ActionsBox_Leave(object sender, EventArgs e)
        {
            Tracing.TraceLine("ActionsBox_Leave", TraceLevel.Info);
            actionsReady = false;
            ActionsBox.Enabled = false;
            ActionsBox.Visible = false;
        }

        private void DoneButton_Click(object sender, EventArgs e)
        {
            Tracing.TraceLine("DoneButton_Click", TraceLevel.Info);
            DialogResult = DialogResult.OK;
        }

        private void CnclButton_Click(object sender, EventArgs e)
        {
            Tracing.TraceLine("CnclButton_Click", TraceLevel.Info);
            DialogResult = DialogResult.Cancel;
        }
    }
}
