﻿'------------------------------------------------------------------------------
' <auto-generated>
'    此代码是根据模板生成的。
'
'    手动更改此文件可能会导致应用程序中发生异常行为。
'    如果重新生成代码，则将覆盖对此文件的手动更改。
' </auto-generated>
'------------------------------------------------------------------------------

Imports System
Imports System.Data.Entity
Imports System.Data.Entity.Infrastructure

Partial Public Class HPMDEntities
    Inherits DbContext

    Public Sub New()
        MyBase.New("name=HPMDEntities")
    End Sub

    Protected Overrides Sub OnModelCreating(modelBuilder As DbModelBuilder)
        Throw New UnintentionalCodeFirstException()
    End Sub

    Public Property sysInfo() As DbSet(Of sysInfo)
    Public Property 用户表() As DbSet(Of 用户表)

End Class