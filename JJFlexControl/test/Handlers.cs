using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using JJFlexControl;
using JJTrace;
using MsgLib;

namespace test
{
    class Handlers
    {
        private uint _stepSize = 50;
        private uint stepSize
        {
            get { return (uint)parent.Knob.GetSavedIntegerValue("StepIncrease", (int)_stepSize); }
            set
            {
                _stepSize = value;
                parent.Knob.SaveIntegerValue("StepIncrease", (int)value);
                parent.Knob.SaveIntegerValue("StepDecrease", (int)value);
            }
        }
        private ulong frequency = 14050000;
        private Form1 parent;
        private TextBox outBox { get { return parent.OutBox; } }

        internal Handlers(Form1 p)
        {
            parent = p;
        }

        internal void KeyHandler(string key)
        {
            FlexControl.Action_t action = null;
            if (parent.KnobActionsMap.TryGetValue(key,out action) &&
                (action.Action != null))
            {
                action.Action(key);
            }
        }

        internal void KnobUp(object cmd)
        {
            frequency += stepSize;
            TextOut.DisplayText(outBox, "frequency:" + frequency.ToString());
        }

        internal void KnobDown(object cmd)
        {
            frequency -= stepSize;
            TextOut.DisplayText(outBox, "frequency:" + frequency.ToString());
        }

        internal void IncreaseStepSize(object cmd)
        {
            stepSize += 10;
            TextOut.DisplayText(outBox, "Step size:" + stepSize);
        }

        internal void DecreaseStepSize(object cmd)
        {
            stepSize -= 10;
            TextOut.DisplayText(outBox, "Step size:" + stepSize);
        }

        internal string ShowStepsize(FlexControl.Action_t action)
        {
            return stepSize.ToString();
        }
    }
}
