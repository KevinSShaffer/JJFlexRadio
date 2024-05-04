Imports System
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Linq
Imports System.Text
Imports System.Windows.Forms
Imports JJFlexControl
Imports JJTrace
Imports Radios

Friend Class FlexKnob
    Private knob As FlexControl

    Private ReadOnly Property configFileName As String
        Get
            Return BaseConfigDir + "\"c + PersonalData.UniqueOpName(CurrentOp) & "_FlexKnob.xml"
        End Get
    End Property

    Friend KnobStatus As Boolean
    Friend Class KnobOnOffArgs
        Inherits EventArgs
        Public Status As Boolean
        Public Sub New(st As Boolean)
            Status = st
        End Sub
    End Class
    Friend Shared Event KnobOnOffEvent As EventHandler(Of KnobOnOffArgs)
    Private Sub raiseKnobOnOff(status As Boolean)
        KnobStatus = status
        RaiseEvent KnobOnOffEvent(Me, New KnobOnOffArgs(status))
    End Sub

    Private ReadOnly Property knobActionsMap As Dictionary(Of String, FlexControl.Action_t)
        Get
            Return knob.KnobActionsMap
        End Get
    End Property

    Private allowedActions As List(Of FlexControl.Action_t)
    Private defaultKeys As List(Of FlexControl.KeyAction_t) = New List(Of FlexControl.KeyAction_t)() From {
            New FlexControl.KeyAction_t("D", "KnobDown"),
            New FlexControl.KeyAction_t("U", "KnobUp"),
            New FlexControl.KeyAction_t("S", "RITOffOnLeave"),
            New FlexControl.KeyAction_t("C", "RITOnOnLeave"),
            New FlexControl.KeyAction_t("L", "RITClear"),
            New FlexControl.KeyAction_t("X1S", "NextVFO"),
            New FlexControl.KeyAction_t("X1C", "KnobOnOff"),
            New FlexControl.KeyAction_t("X2S", "NextValue1"),
            New FlexControl.KeyAction_t("X3S", "StepIncrease"),
            New FlexControl.KeyAction_t("X3C", "StepDecrease")
        }

    Private Sub setupAllowedActions()
        allowedActions = New List(Of FlexControl.Action_t)()
        allowedActions.Add(New FlexControl.Action_t("None", "None", Nothing))
        allowedActions.Add(New FlexControl.Action_t("KnobDown", "Frequency down", AddressOf knobDown))
        allowedActions.Add(New FlexControl.Action_t("KnobUp", "Frequency up", AddressOf knobUp))
        allowedActions.Add(New FlexControl.Action_t("StepIncrease", "Knob step size increase", AddressOf increaseStepSize, Function(ByVal action As FlexControl.Action_t) stepsize.Value.ToString()))
        allowedActions.Add(New FlexControl.Action_t("StepDecrease", "Knob step size decrease", AddressOf decreaseStepSize, Function(ByVal action As FlexControl.Action_t) stepsize.Value.ToString()))
        allowedActions.Add(New FlexControl.Action_t("NextVFO", "Next VFO", AddressOf NextVFO))
        If OpenParms.NextValue1 IsNot Nothing Then
            allowedActions.Add(New FlexControl.Action_t("NextValue1", OpenParms.NextValue1Description, AddressOf NextValue1))
        End If
        allowedActions.Add(New FlexControl.Action_t("RITOffOnLeave", "Start/stop changing the RIT, turn off if stopping", AddressOf RITOffOnLeave))
        allowedActions.Add(New FlexControl.Action_t("RITOnOnLeave", "Start/stop changing the RIT, leave on if stopping", AddressOf RITOnOnLeave))
        allowedActions.Add(New FlexControl.Action_t("RITClear", "Clear the RIT", AddressOf RITClear))
        allowedActions.Add(New FlexControl.Action_t("KnobOnOff", "Knob enable",
            AddressOf knobOnOff, Function(ByVal action As FlexControl.Action_t) knobOn.ToString(), True))
    End Sub

    Friend Sub New()
        setupAllowedActions()
        knob = New FlexControl(configFileName, allowedActions, defaultKeys, False)
        AddHandler knob.KnobOutput, AddressOf keyHandler
        raiseKnobOnOff(knobOn)
    End Sub

    Friend Sub Dispose()
        If knob IsNot Nothing Then knob.Dispose()
    End Sub

    Friend Sub Config()
        knob.SelectPort()
        knob.KeyConfigure()
    End Sub

    Private Sub keyHandler(ByVal cmd As String)
        Dim action As FlexControl.Action_t = Nothing

        If knobActionsMap.ContainsKey(cmd) Then
            action = knobActionsMap(cmd)
            If (action.Action IsNot Nothing) And
               (action.AlwaysActive Or knobOn) Then
                action.Action(action)
            End If
        End If
    End Sub

    Private Sub knobUp(ByVal parm As Object)
        Tracing.TraceLine("knobUp:", TraceLevel.Info)

        If Not RigControl.Transmit Then
            If inRIT Then
                Dim r As AllRadios.RITData = RigControl.RIT
                r.Value += stepsize.Value
                RigControl.RIT = r
            Else
                RigControl.VirtualRXFrequency += CUInt(stepsize.Value)
            End If
        End If
    End Sub

    Private Sub knobDown(ByVal parm As Object)
        Tracing.TraceLine("knobDown:", TraceLevel.Info)

        If Not RigControl.Transmit Then
            If inRIT Then
                Dim r As AllRadios.RITData = RigControl.RIT
                r.Value -= stepsize.Value
                RigControl.RIT = r
            Else
                RigControl.VirtualRXFrequency -= CUInt(stepsize.Value)
            End If
        End If
    End Sub

    Private Class stepsize_t
        Public ID As Integer
        Public Change As Integer
        Public Value As Integer

        Public Sub New(ByVal i As Integer, ByVal c As Integer, ByVal v As Integer)
            ID = i
            Change = c
            Value = v
        End Sub
    End Class

    Private Shared modeToStepsize As Dictionary(Of String, stepsize_t) = New Dictionary(Of String, stepsize_t)() From {
            {"cw", New stepsize_t(0, 10, 10)},
            {"am", New stepsize_t(1, 1000, 5000)},
            {"dflt", New stepsize_t(2, 50, 50)}
        }

    ' Default stepsize values, one per mode, colon separated.
    Private Shared ReadOnly Property stepsizeConfig As String
        Get
            Dim rv As String = ""
            Dim e = modeToStepsize.Values.GetEnumerator()

            While e.MoveNext()
                If rv <> "" Then rv += ":"c
                rv += e.Current.Value.ToString()
            End While

            Return rv
        End Get
    End Property

    ' Get the stepsize_t for the current mode.
    Private ReadOnly Property stepsize As stepsize_t
        Get
            Dim cfg As String() = knob.GetSavedStringValue("StepIncrease", stepsizeConfig).Split(New Char() {":"c})
            Dim e = modeToStepsize.GetEnumerator()

            While e.MoveNext()
                e.Current.Value.Value = Int32.Parse(cfg(e.Current.Value.ID))
            End While

            Dim modeString As String = RigControl.Mode.ToString()
            Dim s As stepsize_t = Nothing

            If modeToStepsize.TryGetValue(modeString, s) Then
                Return s
            Else
                Return modeToStepsize("dflt")
            End If
        End Get
    End Property

    Private Sub increaseStepSize(ByVal parm As Object)
        Tracing.TraceLine("increaseStepSize:", TraceLevel.Info)
        Dim s As stepsize_t = stepsize
        s.Value += s.Change
        Dim cfg As String = stepsizeConfig
        knob.SaveStringValue("StepIncrease", cfg)
        knob.SaveStringValue("StepDecrease", cfg)
    End Sub

    Private Sub decreaseStepSize(ByVal parm As Object)
        Tracing.TraceLine("decreaseStepSize:", TraceLevel.Info)
        Dim s As stepsize_t = stepsize
        s.Value -= s.Change
        If s.Value < 0 Then s.Value = 0
        Dim cfg As String = stepsizeConfig
        knob.SaveStringValue("StepIncrease", cfg)
        knob.SaveStringValue("StepDecrease", cfg)
    End Sub

    ' This only works if transmit mode.
    Private Sub NextVFO(ByVal parm As Object)
        Tracing.TraceLine("NextVFO:", TraceLevel.Info)
        If RigControl.Transmit Then Return
        If Not RigControl.Split Then
            RigControl.RXVFO = RigControl.NextVFO(RigControl.RXVFO)
            RigControl.TXVFO = RigControl.RXVFO
        End If
    End Sub

    Private Sub NextValue1(ByVal parm As Object)
        Tracing.TraceLine("NextValue1:", TraceLevel.Info)
        Commands.toggle1()
    End Sub

    Private inRIT As Boolean = False

    Private Sub RITOffOnLeave(ByVal parm As Object)
        Tracing.TraceLine("RITOffOnLeave:", TraceLevel.Info)
        Dim r As AllRadios.RITData = RigControl.RIT
        r.Active = Not r.Active
        inRIT = r.Active
        RigControl.RIT = r
    End Sub

    Private Sub RITOnOnLeave(ByVal parm As Object)
        Tracing.TraceLine("RITOffOnLeave:", TraceLevel.Info)
        Dim r As AllRadios.RITData = RigControl.RIT

        If Not inRIT Then
            r.Active = True
        End If

        inRIT = Not inRIT
        RigControl.RIT = r
    End Sub

    Private Sub RITClear(ByVal parm As Object)
        Tracing.TraceLine("RITClear:", TraceLevel.Info)
        Dim r As AllRadios.RITData = RigControl.RIT
        r.Value = 0
        RigControl.RIT = r
    End Sub

    Private Const knobOnDefault As Boolean = True

    Private Property knobOn As Boolean
        Get
            Return knob.GetSavedBooleanValue("KnobOnOff", knobOnDefault)
        End Get
        Set(ByVal value As Boolean)
            If value <> knob.GetSavedBooleanValue("KnobOnOff", knobOnDefault) Then
                knob.SaveBooleanValue("KnobOnOff", value)
                raiseKnobOnOff(value)
            End If
        End Set
    End Property

    Private Sub knobOnOff(ByVal parm As Object)
        knobOn = Not knobOn
        Tracing.TraceLine("knobOnOff:" & knobOn.ToString(), TraceLevel.Info)
    End Sub
End Class
