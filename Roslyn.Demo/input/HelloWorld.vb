Imports System
Imports System.Linq

Module Program

    Sub Main(args As String())

        Console.WriteLine($"Hello {If(args.FirstOrDefault(), "world")}!")

    End Sub

End Module
