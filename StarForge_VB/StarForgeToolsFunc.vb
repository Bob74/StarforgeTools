Imports Magic

Module StarForgeToolsFunc

    Public StarForge As New BlackMagic           'Création de l'instance de la class BlackMagic (attributs+fonctions accessibles seulement en mode Instance (cf. help sur "BlackMagic Class"))

    ' Constantes "Infinite Blocks"
    Public Const infBlockFunc1Pattern As String = "83 C4 10 89 45 F4 8B 4D F4 8B 45 F8 89 48 18 EB 0D"
    Public Const infBlockFunc1ADD As Integer = 12               '0C (h)         -> Adresse du "mov [eax+18],ecx"

    Public infBlockFunc1ORG As Byte() = {137, 72, 24}           '89 48 18 (h)   -> mov [eax+18],ecx
    Public infBlockFunc1MOD As Byte() = {144, 144, 144}         '90 90 90 (h)   -> nop


    ' Constantes "Unlimited Resources"
    Public Const infResFuncPattern As String = "75 09 8B 47 08 03 45 10 89 47 08 43"
    Public Const infResFuncADD As Integer = 0               '0 (h)      -> Adresse du "jne"

    Public infResFuncORG As Byte = 117                      '75 (h)     -> jne
    Public infResFuncMOD As Byte = 116                      '74 (h)     -> je


    ' Déclaration des adresses
    Public adrInfBlockFunc As UInteger = 0
    Public adrInfResFunc As UInteger = 0

    Private maxMemBuffer As Integer = 20480

    Public Function ScanForPattern(ByVal _memStart As Integer, ByVal _memStop As Integer, ByVal szPattern As String) As ArrayList

        'Résultat renvoyé (liste des @ potentielles)
        Dim _res As New ArrayList

        'Taille mémoire à parcourir
        Dim _memSize As Integer = _memStop - _memStart

        'Bytes perdus ?
        Dim _lostBytes As Boolean = CBool(_memStop Mod maxMemBuffer)

        'Adresse en cours
        Dim _CurrAddr As Integer = _memStart

        ' Pattern de bytes
        Dim bPattern() As Byte = StringToBytes(szPattern)

        If _memSize >= maxMemBuffer Then    ' Si la zone mémoire est > à maxMemBuffer, alors on découpe
            Dim _maxloop As Integer = CInt(Fix(_memSize / maxMemBuffer))
            Dim _ar As Byte()
            For i = 0 To _maxloop - 1   ' - 1 car on commence à 0
                _ar = StarForge.ReadBytes(CUInt(_CurrAddr), maxMemBuffer)   ' On lit la taille maximale
                If Not (_ar Is Nothing) Then '.. on cherche le pattern
                    Dim _patternPos = FindPatternInBytes(bPattern, _ar)
                    If _patternPos.Count Then   ' Pattern trouvé à un ou plusieurs endroits !
                        For j = 0 To _patternPos.Count - 1  ' -1 car Arraylist est 0 based
                            'MsgBox(_CurrAddr + _patternPos(j))                                                             ' ############ DEBUG
                            _res.Add(_CurrAddr + _patternPos(j))    ' address = address actuelle (= start + nb buffers) + nb de bytes jusqu'au pattern
                            Return _res
                        Next
                    Else
                        'MsgBox("Pattern introuvable !")
                    End If
                End If
                _CurrAddr += maxMemBuffer
            Next

            ' Lecture des bytes perdus
            If _lostBytes Then  ' Si la mémoire totale n'est pas un multiple de maxMemBuffer, il y'aura des restes (et donc des bytes non lu à la fin)
                _CurrAddr = _memStop - maxMemBuffer
                _ar = StarForge.ReadBytes(CUInt(_CurrAddr), maxMemBuffer)   ' On lit un buffer en partant de la fin
                If Not (_ar Is Nothing) Then '.. on cherche le pattern
                    Dim _patternPos = FindPatternInBytes(bPattern, _ar)
                    If _patternPos.Count Then   ' Pattern trouvé à un ou plusieurs endroits !
                        For i = 0 To _patternPos.Count - 1  ' - 1 car Arraylist est 0 based
                            'MsgBox(Hex(_CurrAddr + _patternPos(i)))                                                               ' ############ DEBUG
                            _res.Add(_CurrAddr + _patternPos(i))    ' address = address actuelle + nb de buffers passés + nb de bytes jusqu'au pattern
                            Return _res
                        Next
                    Else
                        'MsgBox("Pattern introuvable !")
                    End If
                End If
            End If

        Else ' Sinon, on peut lire d'un bloc

            Dim _ar As Byte() = StarForge.ReadBytes(CUInt(_CurrAddr), _memSize) ' On lit la totalité du bloc

            If Not (_ar Is Nothing) Then    ' Si on a lut quelque chose...
                Dim _patternPos = FindPatternInBytes(bPattern, _ar) '.. on cherche le pattern
                If _patternPos.Count Then   ' Pattern trouvé !
                    For i = 0 To _patternPos.Count - 1 ' - 1 car Arraylist est 0 based
                        'MsgBox(_CurrAddr + _patternPos(i))                                                                   ' ############ DEBUG
                        _res.Add(_CurrAddr + _patternPos(i)) ' address = address actuelle + nb de bytes jusqu'au pattern
                    Next
                Else    ' Pattern introuvable dans la plage d'adresses
                    ' Pattern introuvable !
                    'MsgBox("Pattern introuvable")                                                                           ' ############ DEBUG
                End If
            Else    ' Sinon (tableau vide)...
                MsgBox("Impossible de lire la mémoire")                                                                     ' ############ DEBUG
                ' Error pendant la lecture mémoire (return 0 + msg disant de cliquer avec un bloc ?)
            End If

        End If

        Return _res

    End Function

    Private Function FindPatternInBytes(bPattern As Byte(), memory As Byte()) As ArrayList
        ' Contient la position du premier byte du pattern dans la zone mémoire fournie
        ' (et ce, chaque fois que le pattern a été trouvé (Ubound(ar) = nb d'adresses potentielles))
        Dim _ar = New ArrayList
        ' Precomputing this shaves some seconds from the loop execution
        Dim maxloop As Integer = memory.Length - bPattern.Length

        For i = 0 To maxloop
            If (bPattern(0) = memory(i)) Then

                Dim ismatch As Boolean = True

                For j = 1 To bPattern.Length - 2    ' - 1 car length = 0 based / - 1 car premier byte déjà passé
                    If Not (memory(i + j) = bPattern(j)) Then
                        ismatch = False
                        Exit For
                    End If
                Next

                If (ismatch) Then
                    ' Si on a trouvé le pattern, on indique à quel Byte (currAddr + index = @ mémoire)
                    _ar.Add(i)
                    ' Et on saute aux prochains bytes
                    i += bPattern.Length - 1
                End If

            End If

        Next

        Return _ar

    End Function

    Public Function StringToBytes(ByVal _sz As String, Optional _delimit As Char = " ") As Byte()
        'Conversion d'une string en byte()

        Dim arPattern = _sz.Split(_delimit)
        Dim bPattern(arPattern.Length) As Byte

        For i = 0 To UBound(arPattern)
            bPattern(i) = CByte(CInt("&h" & arPattern(i)))
        Next

        Return bPattern

    End Function

    Public Sub ArrayDisplay(ByVal _ar As Array, Optional _h As Boolean = True, Optional _delimit As Char = " ")
        Dim _msg As String = ""
        Dim _val As String = ""

        For i = 0 To UBound(_ar) - 1
            If i = UBound(_ar) - 1 Then
                _delimit = ""
            End If
            If _h Then
                _val = Hex(_ar(i))
            Else
                _val = _ar(i)
            End If
            _msg = _msg & _val & _delimit

        Next
        MsgBox(_msg)
    End Sub

End Module
