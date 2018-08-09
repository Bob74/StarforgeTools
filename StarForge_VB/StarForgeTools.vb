Imports System.Threading

Public Class StarForgeTools

    Public _procName As String = "StarForge"
    Public _appName As String = "StarForge - Tools"
    'Public _reqDll() As String = {"BlackMagic.dll", "fasmdll_managed.dll"}
    Public _pid As New Integer
    Private DefRoundAddresses As UInteger() = {0, 0}
    Private trd As Thread

#Region "Multi-threading"

    ' Pré-requis :
    ' ------------
    Private Delegate Sub ThreadResetUICall()
    Private Delegate Sub ThreadControlStateCall(ByVal Ctrl As Control, ByVal state As Boolean)
    Private Delegate Sub ThreadToolStripLabelTextCall(ByVal Tsl As ToolStripLabel, ByVal Text As String)

    Private Sub ThreadResetUI()
        StarForgeToolsReset()
    End Sub

    Private Sub ThreadControlState(ByVal Ctrl As Control, ByVal state As Boolean)
        Ctrl.Enabled = state
    End Sub

    Private Sub ThreadToolStripLabelText(ByVal Tsl As ToolStripLabel, ByVal Text As String)
        Tsl.Text = Text
    End Sub

    ' Fonction :
    ' ----------
    Private Sub ThreadTask()
        Dim ResetUI As New ThreadResetUICall(AddressOf ThreadResetUI)
        Dim SetCtrlState As New ThreadControlStateCall(AddressOf ThreadControlState)
        Dim SetTlsText As New ThreadToolStripLabelTextCall(AddressOf ThreadToolStripLabelText)
        Dim ProcCurrent As Boolean = False
        Dim ProcList() As System.Diagnostics.Process
        Do
            ProcList = Process.GetProcessesByName(_procName)
            If ProcList.Length Then        ' Quand on a détecté un processus, on récupère le PID et on active la fenêtre

                For i = 0 To ProcList.Length - 1
                    If ProcList(i).Id = _pid Then ProcCurrent = True
                Next

                If Not ProcCurrent Then
                    _pid = ProcList(0).Id
                    Me.Invoke(SetTlsText, {Me.ToolStripStatusLabel_ProcFound, _pid.ToString})
                    Me.Invoke(SetCtrlState, {Me.GroupBox_Scan, True})
                    Me.Invoke(SetCtrlState, {Me.GroupBox_FunctionsCommon, True})
                End If

                ProcCurrent = False

            Else
                Me.Invoke(SetTlsText, {Me.ToolStripStatusLabel_ProcFound, "?"})
                Me.Invoke(SetCtrlState, {Me.GroupBox_Scan, False})
                Me.Invoke(SetCtrlState, {Me.GroupBox_FunctionsCommon, False})
                Me.Invoke(ResetUI)
            End If
            Thread.Sleep(100)
        Loop
    End Sub

#End Region

    Private Sub StarForgeTools_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        If trd.IsAlive Then
            Try
                trd.Abort()
            Catch ex As Exception
                ' Le thread n'a pas pu être arrêté
            End Try
        End If

        Dim ProcList = Process.GetProcessesByName(_procName)
        If ProcList.Length Then        ' Quand on a détecté un processus, on récupère le PID et on active la fenêtre
            If StarForge.OpenProcessAndThread(_pid) Then
                StarForge.WriteBytes(adrInfBlockFunc + infBlockFunc1ADD, StarForgeToolsFunc.infBlockFunc1ORG)
                StarForge.WriteByte(adrInfResFunc + infResFuncADD, StarForgeToolsFunc.infResFuncORG)
            End If
        End If

    End Sub

    Private Sub StarForgeTools_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        'For i = 0 To UBound(_reqDll) - 1
        '   If Not System.IO.File.Exists(Application.StartupPath & "\" & _reqDll(i)) Then
        '       MsgBox("Unable to find " & Application.StartupPath & _reqDll(i), MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "Missing file")
        '       Application.Exit()
        '   End If
        'Next

        StarForge.SetDebugPrivileges = False    ' False = Debug

        ' Activation du thread de detection du Processus
        trd = New Thread(AddressOf ThreadTask)
        trd.IsBackground = True
        trd.Start()

    End Sub

    Private Sub Button_Scan_Click(sender As System.Object, e As System.EventArgs) Handles Button_Scan.Click

        Dim result As New ArrayList
        Dim memStart As IntPtr = "&h06FFFFFF"
        Dim memStop As IntPtr = "&h7FFFFFFF"

        ' Bloquage de l'interface pendant le scan
        GroupBox_Scan.Enabled = False
        GroupBox_FunctionsCommon.Enabled = False

        If StarForge.OpenProcessAndThread(_pid) Then

            ' Detection : Infinite blocks
            ' -----------
            If Not adrInfBlockFunc Then
                Try
                    result = ScanForPattern(memStart.ToInt32, memStop.ToInt32, infBlockFunc1Pattern)
                Catch ex As Exception
                    ' Valeur introuvable (plage d'adresses non allouée / Pb syntaxe pattern ?)
                    MsgBox(ex.ToString, MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "Critical error")                                                ' ########## DEBUG
                End Try
                If result.Count Then
                    'MsgBox("Found at " & result.Item(0).ToString & " (" & Hex(result.Item(0)) & ")")                                                ' ########## DEBUG
                    Button_InfiniteBlocks.Enabled = True
                    adrInfBlockFunc = result.Item(0)
                Else
                    MsgBox("Unable to find the function for ""Infinite blocks""." _
                           & vbCrLf & "Make sure you opened your crafting menu (TAB) at least once.", _
                           MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, "Error")
                End If
            End If


            ' Detection : Unlimited resources
            ' -----------
            If Not adrInfBlockFunc Then
                Try
                    result = ScanForPattern(memStart.ToInt32, memStop.ToInt32, infResFuncPattern)
                Catch ex As Exception
                    ' Valeur introuvable (plage d'adresses non allouée / Pb syntaxe pattern ?)
                    MsgBox(ex.ToString, MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "Critical error")                                                ' ########## DEBUG
                End Try
                If result.Count Then
                    'MsgBox("Found at " & result.Item(0).ToString & " (" & Hex(result.Item(0)) & ")")                                                ' ########## DEBUG
                    Button_InfResources.Enabled = True
                    adrInfResFunc = result.Item(0)
                Else
                    MsgBox("Unable to find the function for ""Unlimited resources""." _
                           & vbCrLf & "Make sure you've already craft something.", _
                           MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, "Error")
                End If
            End If

        Else
            ' Le processus n'a pas pu être ouvert
            MsgBox("Cannot open the process." & vbCrLf & vbCrLf & "Try to run the program as administrator.", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "Critical error")
        End If

        ' Débloquage de l'interface après le scan
        GroupBox_Scan.Enabled = True
        GroupBox_FunctionsCommon.Enabled = True

    End Sub

    Private Sub Button_InfiniteBlocks_Click(sender As System.Object, e As System.EventArgs) Handles Button_InfiniteBlocks.Click
        If StarForge.ReadBytes(adrInfBlockFunc + infBlockFunc1ADD, 3)(0) = StarForgeToolsFunc.infBlockFunc1ORG(0) And _
           StarForge.ReadBytes(adrInfBlockFunc + infBlockFunc1ADD, 3)(1) = StarForgeToolsFunc.infBlockFunc1ORG(1) And _
           StarForge.ReadBytes(adrInfBlockFunc + infBlockFunc1ADD, 3)(2) = StarForgeToolsFunc.infBlockFunc1ORG(2) Then
            StarForge.WriteBytes(adrInfBlockFunc + infBlockFunc1ADD, StarForgeToolsFunc.infBlockFunc1MOD)
            Button_InfiniteBlocks.BackColor = Color.YellowGreen
        Else
            StarForge.WriteBytes(adrInfBlockFunc + infBlockFunc1ADD, StarForgeToolsFunc.infBlockFunc1ORG)
            Button_InfiniteBlocks.BackColor = Color.IndianRed
        End If
    End Sub

    Private Sub Button_InfResources_Click(sender As System.Object, e As System.EventArgs) Handles Button_InfResources.Click
        If StarForge.ReadByte(adrInfResFunc + infResFuncADD) = StarForgeToolsFunc.infResFuncORG Then
            StarForge.WriteByte(adrInfResFunc + infResFuncADD, StarForgeToolsFunc.infResFuncMOD)
            Button_InfResources.BackColor = Color.YellowGreen
        Else
            StarForge.WriteByte(adrInfResFunc + infResFuncADD, StarForgeToolsFunc.infResFuncORG)
            Button_InfResources.BackColor = Color.IndianRed
        End If
    End Sub

    Private Sub StarForgeToolsReset()
        Button_InfiniteBlocks.Enabled = False
        Button_InfiniteBlocks.BackColor = Color.IndianRed
        Button_InfResources.Enabled = False
        Button_InfResources.BackColor = Color.IndianRed
    End Sub

    Private Sub ToolStripStatusLabel_LinkForum_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripStatusLabel_LinkForum.Click
        System.Diagnostics.Process.Start("http://www.forgeforums.com")
    End Sub


End Class
