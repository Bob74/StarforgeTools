Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Diagnostics

Namespace InternalCaller
    Public Class Caller

        Private process As Process
        Private parameters As BinaryWriter
        Private [handles] As List(Of UInt32)
        Private stackparams As UInt32

        Public Sub New(ByVal p As Process)
            process = p
            parameters = New BinaryWriter(New MemoryStream())
            [handles] = New List(Of UInteger)()
            stackparams = 0
        End Sub

        ''' <summary>
        ''' Pushes value on the stack (remember about function calling convention).
        ''' </summary>
        Private Sub PushValue(ByVal value As UInt32)
            parameters.Write(CByte(&H68))
            parameters.Write(value)
            stackparams += 1
        End Sub

        ''' <summary>
        ''' Reserves memory, copies string and pushes it's address on the stack.
        ''' </summary>
        Private Sub PushString(ByVal str As String)
            PushValue(GetPointer(ASCIIEncoding.ASCII.GetBytes(str & vbNullChar)))
        End Sub

        ''' <summary>
        ''' Reserves memory, copies data and pushes it's address on the stack.
        ''' </summary>
        Private Sub PushPointer(ByVal data As Byte())
            PushValue(GetPointer(data))
        End Sub

        ''' <summary>
        ''' Clears call stack.
        ''' </summary>
        Private Sub ClearStack()
            parameters = New BinaryWriter(New MemoryStream())
        End Sub

        Friend Function [Call](ByVal address As IntPtr, ByVal convention As CallConvention, ByVal ParamArray parameters As Object()) As UInt32
            Array.Reverse(parameters)
            For Each o As Object In parameters
                Dim paramt As Type = o.[GetType]()
                If paramt Is GetType(System.String) Then
                    PushString(Convert.ToString(o))
                ElseIf paramt Is GetType(System.Int32) OrElse paramt Is GetType(System.Int32) OrElse paramt Is GetType(System.Int32) Then
                    PushValue(Convert.ToUInt32(o))
                ElseIf paramt Is GetType(System.Byte) Then
                    PushPointer(DirectCast(o, [Byte]()))
                Else
                    Return 0
                End If
            Next
            Return [Call](address, convention)
        End Function


        ''' <summary>
        ''' Calls the function, return it's exit code.
        ''' </summary>
        Friend Function [Call](ByVal address As IntPtr, ByVal convention As CallConvention) As UInt32
            Dim opcodes As New BinaryWriter(New MemoryStream())
            parameters.BaseStream.Position = 0
            opcodes.Write(New BinaryReader(parameters.BaseStream).ReadBytes(CInt(parameters.BaseStream.Length)))
            opcodes.Write(CByte(&HB8)) 'mov eax, -
            opcodes.Write(CInt(address))
            opcodes.Write(CUShort(&HD0FF)) 'call eax
            If convention = CallConvention.Cdecl Then
                opcodes.Write(CUShort(&HC481)) 'add esp, -
                opcodes.Write(CUInt(stackparams * 4))
            End If

            opcodes.Write(CByte(&HC2))
            opcodes.Write(CUShort(&H4)) 'retn 4
            opcodes.BaseStream.Position = 0

            Dim reader As New BinaryReader(opcodes.BaseStream)
            Dim ops As UInt32 = GetPointer(reader.ReadBytes(opcodes.BaseStream.Length))
            Dim threadId As UInteger = 0
            Dim lpExitCode As UInteger = 0
            Dim hThread As IntPtr = CreateRemoteThread(process.Handle, IntPtr.Zero, 0, New IntPtr(CInt(ops)), IntPtr.Zero, 0, threadId)
            [handles].Add(CUInt(hThread))
            'infinite
            WaitForSingleObject(hThread, &HFFFFFFFF)
            GetExitCodeThread(hThread, lpExitCode)
            CloseHandles()
            Return lpExitCode
        End Function

        Private Function GetPointer(ByVal data As Byte()) As UInt32
            Dim ret As IntPtr = VirtualAllocEx(process.Handle, IntPtr.Zero, CUInt(data.Length), AllocationType.Reserve Or AllocationType.Commit, MemoryProtection.ExecuteReadWrite)
            If ret <> IntPtr.Zero Then
                [handles].Add(CUInt(ret))
                Dim bytesWritten As Integer = 0
                WriteProcessMemory(process.Handle, ret, data, CUInt(data.Length) - 1, bytesWritten)
                Return CUInt(ret)
            Else
                Throw New Exception("Could not reserve memory!")
            End If
        End Function

        Private Sub CloseHandles()
            For Each handle As UInteger In [handles]
                CloseHandle(New IntPtr(handle))
            Next
            [handles].Clear()
        End Sub

        Private Declare Auto Function VirtualAllocEx Lib "kernel32.dll" (ByVal hProcess As IntPtr, ByVal lpAddress As IntPtr, ByVal dwSize As UInteger, ByVal flAllocationType As AllocationType, ByVal flProtect As MemoryProtection) As IntPtr

        <DllImport("kernel32.dll", SetLastError:=True)> _
        Private Shared Function WriteProcessMemory(ByVal hProcess As IntPtr, ByVal lpBaseAddress As IntPtr, ByVal lpBuffer As Byte(), ByVal nSize As UInteger, ByRef lpNumberOfBytesWritten As Integer) As Boolean
        End Function

        <DllImport("kernel32.dll")> _
        Private Shared Function CreateRemoteThread(ByVal hProcess As IntPtr, ByVal lpThreadAttributes As IntPtr, ByVal dwStackSize As UInteger, ByVal lpStartAddress As IntPtr, ByVal lpParameter As IntPtr, ByVal dwCreationFlags As UInteger, _
             ByRef dwThreadId As UInteger) As IntPtr
        End Function

        <DllImport("kernel32.dll")> _
        Private Shared Function GetExitCodeThread(ByVal hThread As IntPtr, ByRef lpExitCode As UInteger) As Boolean
        End Function

        <DllImport("kernel32.dll", SetLastError:=True)> _
        Private Shared Function WaitForSingleObject(ByVal hHandle As IntPtr, ByVal dwMilliseconds As Int32) As UInt32
        End Function

        <DllImport("kernel32.dll", SetLastError:=True)> _
        Private Shared Function CloseHandle(ByVal hObject As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean
        End Function

        <Flags()> _
        Private Enum AllocationType
            Commit = &H1000
            Reserve = &H2000
            Decommit = &H4000
            Release = &H8000
            Reset = &H80000
            Physical = &H400000
            TopDown = &H100000
            WriteWatch = &H200000
            LargePages = &H20000000
        End Enum

        <Flags()> _
        Private Enum MemoryProtection
            Execute = &H10
            ExecuteRead = &H20
            ExecuteReadWrite = &H40
            ExecuteWriteCopy = &H80
            NoAccess = &H1
            [ReadOnly] = &H2
            ReadWrite = &H4
            WriteCopy = &H8
            GuardModifierflag = &H100
            NoCacheModifierflag = &H200
            WriteCombineModifierflag = &H400
        End Enum
    End Class
    Public Enum CallConvention
        StdCall
        Cdecl
    End Enum
    Public Class InternalFunction
        Private _Caller As Caller
        Public Property Caller() As Caller
            Get
                Return _Caller
            End Get
            Set(ByVal value As Caller)
                _Caller = value
            End Set
        End Property
        Private _Address As IntPtr
        Public Property Address() As IntPtr
            Get
                Return _Address
            End Get
            Set(ByVal value As IntPtr)
                _Address = value
            End Set
        End Property
        Private _CallingConvention As CallConvention
        Public Property CallingConvention() As CallConvention
            Get
                Return _CallingConvention
            End Get
            Set(ByVal value As CallConvention)
                _CallingConvention = value
            End Set
        End Property
        Public Sub [Call](ByVal ParamArray parameters As Object())
            Caller.[Call](Address, CallingConvention, parameters)
        End Sub
    End Class
End Namespace