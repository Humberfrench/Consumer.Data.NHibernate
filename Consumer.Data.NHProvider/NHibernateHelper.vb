Imports NHibernate
Imports NHibernate.Cfg

'Public Class NHibernateHelper
'    Private Shared oSessionFactory As ISessionFactory

'    Public Shared Function GetSession(ByVal assembly As System.Reflection.Assembly)
'        If Not oSessionFactory Is Nothing Then
'            Return oSessionFactory.OpenSession()
'        End If

'        Dim oNHConfig As New Configuration()
'        oNHConfig.AddAssembly(assembly)

'        'Omitindo a configuração do NHibernate
'        oSessionFactory = oNHConfig.BuildSessionFactory()
'        Return oSessionFactory.OpenSession()

'    End Function

'    Private Sub New()

'    End Sub

'End Class

Public NotInheritable Class NHibernateHelper
    Private Const CurrentSessionKey As String = "nhibernate.current_session"
    Private Shared ReadOnly sessionFactory As ISessionFactory

    Shared Sub New()
        sessionFactory = New Configuration().Configure().BuildSessionFactory()
    End Sub

    Public Shared Function GetCurrentSession(ByVal assembly As System.Reflection.Assembly) As ISession
        Dim currentSession As ISession = TryCast(assembly, ISession)
        Try
            If currentSession Is Nothing Then
                currentSession = sessionFactory.OpenSession()
            End If
        Catch ex As Exception
            Throw ex
        End Try

        Return currentSession
    End Function

    Public Shared Sub CloseSession(ByVal assembly As System.Reflection.Assembly)
        Dim currentSession As ISession = TryCast(assembly, ISession)
        Try
            If currentSession Is Nothing Then
                ' No current session
                Return
            End If

            currentSession.Close()
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Shared Sub CloseSessionFactory()
        If sessionFactory IsNot Nothing Then
            sessionFactory.Close()
        End If
    End Sub
End Class