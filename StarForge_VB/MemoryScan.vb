'Imports System
'Imports System.Collections.Generic
'Imports System.Text
'Imports System.Diagnostics
Imports System.Threading
'Imports System.Runtime.InteropServices

Public Class MemoryScan

    ' // Constantes :
    ' // -----------
    ' //Maximum memory block size to read in every read process.

    ' //Experience tells me that,
    ' //if ReadStackSize be bigger than 20480, there will be some problems
    ' //retrieving correct blocks of memory values.
    Const ReadStackSize As Integer = 20480

    ' //A 16 bit variable (Like char) is made up of 16 bits of memory and so 16/8 = 2 bytes of memory.
    Const Int16BytesCount As Integer = 16 / 8
    ' //A 32 bit variable (Like int) is made up of 32 bits of memory and so 32/8 = 4 bytes of memory.
    Const Int32BytesCount As Integer = 32 / 8
    ' //A 34 bit variable (Like long) is made up of 64 bits of memory and so 64/8 = 8 bytes of memory.
    Const Int64BytesCount As Integer = 64 / 8


    ' // Global fields :
    ' // --------------
    ' //Instance of ProcessMemoryReader class to be used to read the memory.
    Dim reader As ProcessMemoryReader

    ' //Start and End addresses to be scaned.
    Dim baseAddress As IntPtr
    Dim lastAddress As IntPtr

    ' //New thread object to run the scan in
    Dim thread As thread


    ' // Delegates :
    ' // ----------
    '//Delegate and Event objects for raising the ScanProgressChanged event.
    Public Delegate Sub ScanProgressedEventHandler(sender As Object, e As ScanProgressChangedEventArgs)
    Public Event ScanProgressChanged As ScanProgressedEventHandler

    '//Delegate and Event objects for raising the ScanCompleted event.
    Public Delegate Sub ScanCompletedEventHandler(sender As Object, e As ScanCompletedEventArgs)
    Public Event ScanCompleted As ScanCompletedEventHandler

    '//Delegate and Event objects for raising the ScanCanceled event.
    Public Delegate Sub ScanCanceledEventHandler(sender As Object, e As ScanCanceledEventArgs)
    Public Event ScanCanceled As ScanCanceledEventHandler


    ' //Class entry point.
    ' //The process, StartAddress and EndAdrress will be defined in the class definition.
    Public Sub RegularMemoryScan(Proc As Process, StartAddress As Integer, EndAddress As Integer)

        ' //Set the reader object an instant of the ProcessMemoryReader class.
        Dim reader As New ProcessMemoryReader()

        ' //Set the ReadProcess of the reader object to process passed to this method
        ' //to define the process we are going to scan its memory.
        reader.ReadProcess = Proc

        ' //Set the Start and End addresses of the scan to what is wanted.
        baseAddress = New IntPtr(StartAddress)
        lastAddress = New IntPtr(EndAddress)    'The scan starts from baseAddress,
        ' //and progresses up to EndAddress.

    End Sub


    ' //Get ready to scan the memory for the 64 bit value.
    Public Function StartScanForInt64(Int64Value As Long) As Integer
        Return Int64Scaner(Int64Value)
    End Function


    ' //The memory scan method for the 32 bit values.
    Private Function Int64Scaner(int64Object As Object) As Integer
        ' //The difference of scan start point in all loops except first loop,
        ' //that doesn't have any difference, is type's Bytes count minus 1.
        Dim arraysDifference As Integer = Int64BytesCount - 1

        ' //Get the int value out of the object to look for it.
        Dim int64Value As Long = CLng(int64Object)

        ' //Define a List object to hold the found memory addresses.
        'List<int> finalList = new List<int>();
        Dim finalList() As Integer

        ' //Open the pocess to read the memory.
        reader.OpenProcess()

        ' //Calculate the size of memory to scan.
        Dim memorySize As Integer = CInt(lastAddress) - CInt(baseAddress)

        ' //If more that one block of memory is requered to be read,
        If (memorySize >= ReadStackSize) Then

            ' //Count of loops to read the memory blocks.
            Dim loopsCount As Integer = memorySize / ReadStackSize

            ' //Look to see if there is any other bytes let after the loops.
            Dim outOfBounds As Integer = memorySize Mod ReadStackSize

            ' //Set the currentAddress to first address.
            Dim currentAddress As Integer = CInt(baseAddress)

            ' //This will be used to check if any bytes have been read from the memory.
            Dim bytesReadSize As Integer

            ' //Set the size of the bytes blocks.
            Dim bytesToRead As Integer = ReadStackSize

            ' //An array to hold the bytes read from the memory.
            Dim array As Byte()

            ' //Progress percentage.
            Dim progress As Integer

            For i = 0 To loopsCount
                ' //Calculte and set the progress percentage.
                progress = CInt((CDbl(currentAddress - CInt(baseAddress)) / CDbl(memorySize)) * CDbl(100))

                ' //Read the bytes from the memory.
                array = reader.ReadProcessMemory(New IntPtr(currentAddress), CUInt(bytesToRead), bytesReadSize)

                ' //If any byte is read from the memory (there has been any bytes in the memory block),
                If (bytesReadSize > 0) Then

                    ' //Loop through the bytes one by one to look for the values.
                    For j = 0 To array.Length - arraysDifference

                        ' //If any value is equal to what we are looking for,
                        If (BitConverter.ToInt64(array, j) = int64Value) Then
                            ' //add it's memory address to the finalList.
                            ' //finalList.Add(j + (int)currentAddress);
                            Return j + CInt(currentAddress)
                        End If
                    Next
                End If
                ' //Move currentAddress after the block already scaned, but
                ' //move it back some steps backward (as much as arraysDifference)
                ' //to avoid loosing any values at the end of the array.
                currentAddress += array.Length - arraysDifference

                ' //Set the size of the read block, bigger, to  the steps backward.
                ' //Set the size of the read block, to fit the back steps.
                bytesToRead = ReadStackSize + arraysDifference
            Next
            ' //If there is any more bytes than the loops read,
            If (outOfBounds > 0) Then

                ' //Read the additional bytes.
                Dim outOfBoundsBytes As Byte() = reader.ReadProcessMemory(New IntPtr(currentAddress), CUInt(CInt(lastAddress) - CInt(currentAddress)), bytesReadSize)

                ' //If any byte is read from the memory (there has been any bytes in the memory block),
                If (bytesReadSize > 0) Then

                    ' //Loop through the bytes one by one to look for the values.
                    For j = 0 To outOfBoundsBytes.Length - arraysDifference

                        ' //If any value is equal to what we are looking for,
                        If (BitConverter.ToInt64(outOfBoundsBytes, j) = int64Value) Then

                            ' //add it's memory address to the finalList.
                            ' //finalList.Add(j + (int)currentAddress);
                            Return j + CInt(currentAddress)
                        End If
                    Next
                End If
            End If

            ' //If the block could be read in just one read,
        Else

            ' //Calculate the memory block's size.
            Dim blockSize As Integer = memorySize Mod ReadStackSize

            ' //Set the currentAddress to first address.
            Dim currentAddress As Integer = CInt(baseAddress)

            ' //Holds the count of bytes read from the memory.
            Dim bytesReadSize As Integer

            ' //If the memory block can contain at least one 64 bit variable.
            If (blockSize > Int32BytesCount) Then

                ' //Read the bytes to the array.
                Dim array As Byte() = reader.ReadProcessMemory(New IntPtr(currentAddress), CUInt(blockSize), bytesReadSize)

                ' //If any byte is read,
                If (bytesReadSize > 0) Then

                    ' //Loop through the array to find the values.
                    For j = 0 To array.Length - arraysDifference
                        ' //If any value equals the value we are looking for,
                        If (BitConverter.ToInt64(array, j) = int64Value) Then
                            ' //add it to the finalList.
                            ' //finalList.Add(j + (int)currentAddress);
                            Return j + CInt(currentAddress)
                        End If
                    Next
                End If
            End If
        End If

        ' //Close the handle to the process to avoid process errors.
        reader.CloseHandle()
        Return 0
    End Function











End Class
