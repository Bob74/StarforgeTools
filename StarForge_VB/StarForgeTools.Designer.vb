<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class StarForgeTools
    Inherits System.Windows.Forms.Form

    'Form remplace la méthode Dispose pour nettoyer la liste des composants.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Requise par le Concepteur Windows Form
    Private components As System.ComponentModel.IContainer

    'REMARQUE : la procédure suivante est requise par le Concepteur Windows Form
    'Elle peut être modifiée à l'aide du Concepteur Windows Form.  
    'Ne la modifiez pas à l'aide de l'éditeur de code.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(StarForgeTools))
        Me.Button_Scan = New System.Windows.Forms.Button()
        Me.Button_InfiniteBlocks = New System.Windows.Forms.Button()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.ToolStripStatusLabel_LinkForum = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripStatusLabel_Credits = New System.Windows.Forms.ToolStripStatusLabel()
        Me.StatusStrip2 = New System.Windows.Forms.StatusStrip()
        Me.ToolStripStatusLabel_Version = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripStatusLabel_ProcLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripStatusLabel_ProcFound = New System.Windows.Forms.ToolStripStatusLabel()
        Me.Label_infBlocks = New System.Windows.Forms.Label()
        Me.GroupBox_FunctionsCommon = New System.Windows.Forms.GroupBox()
        Me.Label_InfRes = New System.Windows.Forms.Label()
        Me.Label_InfBlock = New System.Windows.Forms.Label()
        Me.Label_infResources = New System.Windows.Forms.Label()
        Me.Button_InfResources = New System.Windows.Forms.Button()
        Me.GroupBox_Scan = New System.Windows.Forms.GroupBox()
        Me.StatusStrip1.SuspendLayout()
        Me.StatusStrip2.SuspendLayout()
        Me.GroupBox_FunctionsCommon.SuspendLayout()
        Me.GroupBox_Scan.SuspendLayout()
        Me.SuspendLayout()
        '
        'Button_Scan
        '
        Me.Button_Scan.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button_Scan.Location = New System.Drawing.Point(6, 14)
        Me.Button_Scan.Name = "Button_Scan"
        Me.Button_Scan.Size = New System.Drawing.Size(333, 42)
        Me.Button_Scan.TabIndex = 2
        Me.Button_Scan.Text = "Find these functions !"
        Me.Button_Scan.UseVisualStyleBackColor = True
        '
        'Button_InfiniteBlocks
        '
        Me.Button_InfiniteBlocks.BackColor = System.Drawing.Color.IndianRed
        Me.Button_InfiniteBlocks.Enabled = False
        Me.Button_InfiniteBlocks.Location = New System.Drawing.Point(6, 15)
        Me.Button_InfiniteBlocks.Name = "Button_InfiniteBlocks"
        Me.Button_InfiniteBlocks.Size = New System.Drawing.Size(26, 26)
        Me.Button_InfiniteBlocks.TabIndex = 3
        Me.Button_InfiniteBlocks.UseVisualStyleBackColor = False
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel_LinkForum, Me.ToolStripStatusLabel_Credits})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 317)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.StatusStrip1.Size = New System.Drawing.Size(369, 22)
        Me.StatusStrip1.SizingGrip = False
        Me.StatusStrip1.TabIndex = 4
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'ToolStripStatusLabel_LinkForum
        '
        Me.ToolStripStatusLabel_LinkForum.IsLink = True
        Me.ToolStripStatusLabel_LinkForum.Name = "ToolStripStatusLabel_LinkForum"
        Me.ToolStripStatusLabel_LinkForum.Size = New System.Drawing.Size(124, 17)
        Me.ToolStripStatusLabel_LinkForum.Text = "StarForge Community"
        '
        'ToolStripStatusLabel_Credits
        '
        Me.ToolStripStatusLabel_Credits.Name = "ToolStripStatusLabel_Credits"
        Me.ToolStripStatusLabel_Credits.Size = New System.Drawing.Size(143, 17)
        Me.ToolStripStatusLabel_Credits.Text = "Created by Bob_74 for the"
        '
        'StatusStrip2
        '
        Me.StatusStrip2.Dock = System.Windows.Forms.DockStyle.Top
        Me.StatusStrip2.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel_Version, Me.ToolStripStatusLabel_ProcLabel, Me.ToolStripStatusLabel_ProcFound})
        Me.StatusStrip2.Location = New System.Drawing.Point(0, 0)
        Me.StatusStrip2.Name = "StatusStrip2"
        Me.StatusStrip2.Size = New System.Drawing.Size(369, 22)
        Me.StatusStrip2.SizingGrip = False
        Me.StatusStrip2.TabIndex = 5
        Me.StatusStrip2.Text = "StatusStrip2"
        '
        'ToolStripStatusLabel_Version
        '
        Me.ToolStripStatusLabel_Version.Name = "ToolStripStatusLabel_Version"
        Me.ToolStripStatusLabel_Version.Size = New System.Drawing.Size(79, 17)
        Me.ToolStripStatusLabel_Version.Text = "(Alpha v0.4.6)"
        '
        'ToolStripStatusLabel_ProcLabel
        '
        Me.ToolStripStatusLabel_ProcLabel.Name = "ToolStripStatusLabel_ProcLabel"
        Me.ToolStripStatusLabel_ProcLabel.Size = New System.Drawing.Size(106, 17)
        Me.ToolStripStatusLabel_ProcLabel.Text = "StarForge process :"
        Me.ToolStripStatusLabel_ProcLabel.TextDirection = System.Windows.Forms.ToolStripTextDirection.Horizontal
        '
        'ToolStripStatusLabel_ProcFound
        '
        Me.ToolStripStatusLabel_ProcFound.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.ToolStripStatusLabel_ProcFound.ForeColor = System.Drawing.Color.ForestGreen
        Me.ToolStripStatusLabel_ProcFound.Name = "ToolStripStatusLabel_ProcFound"
        Me.ToolStripStatusLabel_ProcFound.Size = New System.Drawing.Size(12, 17)
        Me.ToolStripStatusLabel_ProcFound.Text = "?"
        '
        'Label_infBlocks
        '
        Me.Label_infBlocks.AutoSize = True
        Me.Label_infBlocks.Location = New System.Drawing.Point(38, 15)
        Me.Label_infBlocks.Name = "Label_infBlocks"
        Me.Label_infBlocks.Size = New System.Drawing.Size(242, 26)
        Me.Label_infBlocks.TabIndex = 6
        Me.Label_infBlocks.Text = "Infinite blocks" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "[Allow you to place blocks without spending them]"
        '
        'GroupBox_FunctionsCommon
        '
        Me.GroupBox_FunctionsCommon.Controls.Add(Me.Label_InfRes)
        Me.GroupBox_FunctionsCommon.Controls.Add(Me.Label_InfBlock)
        Me.GroupBox_FunctionsCommon.Controls.Add(Me.Label_infResources)
        Me.GroupBox_FunctionsCommon.Controls.Add(Me.Button_InfResources)
        Me.GroupBox_FunctionsCommon.Controls.Add(Me.Label_infBlocks)
        Me.GroupBox_FunctionsCommon.Controls.Add(Me.Button_InfiniteBlocks)
        Me.GroupBox_FunctionsCommon.Enabled = False
        Me.GroupBox_FunctionsCommon.Location = New System.Drawing.Point(12, 94)
        Me.GroupBox_FunctionsCommon.Name = "GroupBox_FunctionsCommon"
        Me.GroupBox_FunctionsCommon.Size = New System.Drawing.Size(345, 123)
        Me.GroupBox_FunctionsCommon.TabIndex = 8
        Me.GroupBox_FunctionsCommon.TabStop = False
        '
        'Label_InfRes
        '
        Me.Label_InfRes.AutoSize = True
        Me.Label_InfRes.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_InfRes.ForeColor = System.Drawing.Color.Red
        Me.Label_InfRes.Location = New System.Drawing.Point(6, 99)
        Me.Label_InfRes.Name = "Label_InfRes"
        Me.Label_InfRes.Size = New System.Drawing.Size(234, 13)
        Me.Label_InfRes.TabIndex = 10
        Me.Label_InfRes.Text = "Require to have crafted something at least once"
        '
        'Label_InfBlock
        '
        Me.Label_InfBlock.AutoSize = True
        Me.Label_InfBlock.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_InfBlock.ForeColor = System.Drawing.Color.Red
        Me.Label_InfBlock.Location = New System.Drawing.Point(6, 44)
        Me.Label_InfBlock.Name = "Label_InfBlock"
        Me.Label_InfBlock.Size = New System.Drawing.Size(285, 13)
        Me.Label_InfBlock.TabIndex = 9
        Me.Label_InfBlock.Text = "Require to have your inventory opened (TAB) at least once"
        '
        'Label_infResources
        '
        Me.Label_infResources.AutoSize = True
        Me.Label_infResources.Location = New System.Drawing.Point(38, 70)
        Me.Label_infResources.Name = "Label_infResources"
        Me.Label_infResources.Size = New System.Drawing.Size(233, 26)
        Me.Label_infResources.TabIndex = 8
        Me.Label_infResources.Text = "Unlimited resources" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "[Craft anything without spending your resources]"
        '
        'Button_InfResources
        '
        Me.Button_InfResources.BackColor = System.Drawing.Color.IndianRed
        Me.Button_InfResources.Enabled = False
        Me.Button_InfResources.Location = New System.Drawing.Point(6, 70)
        Me.Button_InfResources.Name = "Button_InfResources"
        Me.Button_InfResources.Size = New System.Drawing.Size(26, 26)
        Me.Button_InfResources.TabIndex = 7
        Me.Button_InfResources.UseVisualStyleBackColor = False
        '
        'GroupBox_Scan
        '
        Me.GroupBox_Scan.Controls.Add(Me.Button_Scan)
        Me.GroupBox_Scan.Enabled = False
        Me.GroupBox_Scan.Location = New System.Drawing.Point(12, 25)
        Me.GroupBox_Scan.Name = "GroupBox_Scan"
        Me.GroupBox_Scan.Size = New System.Drawing.Size(345, 63)
        Me.GroupBox_Scan.TabIndex = 1
        Me.GroupBox_Scan.TabStop = False
        '
        'StarForgeTools
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(369, 339)
        Me.Controls.Add(Me.GroupBox_Scan)
        Me.Controls.Add(Me.GroupBox_FunctionsCommon)
        Me.Controls.Add(Me.StatusStrip2)
        Me.Controls.Add(Me.StatusStrip1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "StarForgeTools"
        Me.Text = "StarForge - Tools"
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.StatusStrip2.ResumeLayout(False)
        Me.StatusStrip2.PerformLayout()
        Me.GroupBox_FunctionsCommon.ResumeLayout(False)
        Me.GroupBox_FunctionsCommon.PerformLayout()
        Me.GroupBox_Scan.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Button_Scan As System.Windows.Forms.Button
    Friend WithEvents Button_InfiniteBlocks As System.Windows.Forms.Button
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents ToolStripStatusLabel_Credits As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents StatusStrip2 As System.Windows.Forms.StatusStrip
    Friend WithEvents ToolStripStatusLabel_ProcLabel As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents ToolStripStatusLabel_ProcFound As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents Label_infBlocks As System.Windows.Forms.Label
    Friend WithEvents ToolStripStatusLabel_LinkForum As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents GroupBox_FunctionsCommon As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox_Scan As System.Windows.Forms.GroupBox
    Friend WithEvents Label_infResources As System.Windows.Forms.Label
    Friend WithEvents Button_InfResources As System.Windows.Forms.Button
    Friend WithEvents ToolStripStatusLabel_Version As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents Label_InfRes As System.Windows.Forms.Label
    Friend WithEvents Label_InfBlock As System.Windows.Forms.Label

End Class
