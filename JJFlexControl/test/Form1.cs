using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using JJFlexControl;
using JJTrace;

namespace test
{
    public partial class Form1 : Form
    {
        internal FlexControl Knob { get; private set; }
        private static Handlers handlers;
        internal Dictionary<string,FlexControl.Action_t> KnobActionsMap
        {
            get
            {
                return Knob.KnobActionsMap;
            }
        }
        internal TextBox OutBox { get { return OutputBox; } }
        internal List<FlexControl.Action_t> allowedActions;
        private List<FlexControl.KeyAction_t> defaultKeys;

        private void Setup()
        {
            allowedActions = new List<FlexControl.Action_t>()
            {
                new FlexControl.Action_t("None","None",null),
                new FlexControl.Action_t("knobDown","Frequency down",handlers.KnobDown),
                new FlexControl.Action_t("knobUp","Frequency up",handlers.KnobUp),
                new FlexControl.Action_t("StepIncrease","Knob step size increase",handlers.IncreaseStepSize, handlers.ShowStepsize),
                new FlexControl.Action_t("StepDecrease","Knob step size decrease",handlers.DecreaseStepSize, handlers.ShowStepsize),
            };
            defaultKeys = new List<FlexControl.KeyAction_t>()
            {
                new FlexControl.KeyAction_t("D","knobDown"),
                new FlexControl.KeyAction_t("U","knobUp"),
                new FlexControl.KeyAction_t("X3S","StepIncrease"),
                new FlexControl.KeyAction_t("X3C","StepDecrease"),
            };
        }

        public Form1()
        {
            InitializeComponent();

            handlers = new Handlers(this);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Setup();
            //Tracing.ToConsole = true;
            Tracing.TheSwitch.Level = TraceLevel.Info;
            Tracing.On = true;
            Tracing.TraceLine("Test JJFlexControl" + DateTime.Now, TraceLevel.Info);

            string cfgPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\JJFlexControl\knob.xml";
            Knob = new FlexControl(cfgPath, allowedActions, defaultKeys, true);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Knob != null) Knob.Dispose();
        }

        private void RunButton_Click(object sender, EventArgs e)
        {
            OutputBox.Text = "";
            Knob.KnobOutput -= handlers.KeyHandler;
            Knob.KnobOutput += handlers.KeyHandler;
        }

        private void SelectPortButton_Click(object sender, EventArgs e)
        {
            Knob.SelectPort();
        }

        private void MapFunctionsButton_Click(object sender, EventArgs e)
        {
            Knob.KeyConfigure();
        }
    }
}
