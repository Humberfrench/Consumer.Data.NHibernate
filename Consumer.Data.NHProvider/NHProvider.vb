Imports System.Configuration
Imports NHibernate
Imports NHibernate.Cfg
Imports NHibernate.Connection
Imports System.Reflection

''' <summary>
''' Classe Provider, de acesso a dados via NHibernate.
''' </summary>
''' <typeparam name="ClassName">Nome da Classe Base a ser usada</typeparam>
''' <typeparam name="ClassID">Tipo de ID da classe</typeparam>
''' <remarks></remarks>
Public Class NHProvider(Of ClassName, ClassID)
    Implements IDisposable

    Private disposedValue As Boolean = False        ' To detect redundant calls

    ''' <summary>
    ''' Construtor do Objeto
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()

    End Sub

    ''' <summary>
    ''' Efetua a inclusão de um registro
    ''' </summary>
    ''' <param name="oPersist">Objeto do Model contendo os dados</param>
    ''' <returns>Booleando para a função</returns>
    ''' <remarks></remarks>
    Public Overridable Function Add(ByVal oPersist As ClassName) As Boolean

        Dim oConfig As New Configuration
        Dim oFactory As ISessionFactory = Nothing
        Dim oSession As ISession = Nothing
        Dim oTransaction As ITransaction = Nothing

        Try

            'definindo o assembly para carregar os arquivo .hbm.xml  
            'que fazem parte do mesmo
            oConfig.AddAssembly(Assembly.GetCallingAssembly())

            'criando uma sessao e iniciando uma transação
            oFactory = oConfig.BuildSessionFactory
            oSession = oFactory.OpenSession
            oTransaction = oSession.BeginTransaction

            'persistindo os dados no banco de dados
            oSession.Save(oPersist)

            'fecha a transação e a sessão
            oTransaction.Commit()
            oSession.Close()

            oConfig = Nothing
            oFactory = Nothing
            oSession = Nothing
            oTransaction = Nothing

        Catch ex As Exception
            oTransaction.Rollback()
            oSession.Close()
            oConfig = Nothing
            oFactory = Nothing
            oSession = Nothing
            oTransaction = Nothing
            Throw ex
        End Try

    End Function

    ''' <summary>
    ''' Efetua a atualização de um registro
    ''' </summary>
    ''' <param name="oPersist">Objeto do Model contendo os dados</param>
    ''' <returns>Booleando para a função</returns>
    ''' <remarks></remarks>
    Public Overridable Function Update(ByVal oPersist As ClassName) As Boolean

        Dim oConfig As New Configuration
        Dim oFactory As ISessionFactory = Nothing
        Dim oSession As ISession = Nothing
        Dim oTransaction As ITransaction = Nothing

        Try

            'definindo o assembly para carregar os arquivo .hbm.xml  que fazem parte do mesmo
            oConfig.AddAssembly(Assembly.GetCallingAssembly())

            'criando uma sessao e iniciando uma transação
            oFactory = oConfig.BuildSessionFactory
            oSession = oFactory.OpenSession
            oTransaction = oSession.BeginTransaction

            'persistindo os dados no banco de dados
            oSession.Update(oPersist)

            'fecha a transação e a sessão
            oTransaction.Commit()
            oSession.Close()

            oConfig = Nothing
            oFactory = Nothing
            oSession = Nothing
            oTransaction = Nothing

        Catch ex As Exception
            oTransaction.Rollback()
            oSession.Close()
            oConfig = Nothing
            oFactory = Nothing
            oSession = Nothing
            oTransaction = Nothing
            Throw ex
        End Try

    End Function

    ''' <summary>
    ''' Efetua a buasca de um registro
    ''' </summary>
    ''' <param name="oValue">Valor do ID para a busca</param>
    ''' <returns>Booleando para a função</returns>
    ''' <remarks></remarks>
    Public Overridable Function [Get](ByVal oValue As ClassID) As ClassName

        Dim oConfig As New Configuration
        Dim oFactory As ISessionFactory = Nothing
        Dim oSession As ISession = Nothing
        Dim oModel As ClassName = Nothing

        Try
            'definindo o assembly para carregar os arquivo .hbm.xml  que fazem parte do mesmo
            oConfig.AddAssembly(Assembly.GetCallingAssembly())

            'criando uma sessao e iniciando uma transação
            oFactory = oConfig.BuildSessionFactory
            oSession = oFactory.OpenSession

            '???
            oModel = oSession.Load(GetType(ClassName), oValue)

            oSession.Flush()

            'fecha a transação e a sessão
            oSession.Close()

            oConfig = Nothing
            oFactory = Nothing
            oSession = Nothing

        Catch ex As Exception
            oSession.Close()
            oConfig = Nothing
            oFactory = Nothing
            oSession = Nothing
            oModel = Nothing
            Throw ex
        End Try

        Return oModel

    End Function

    ''' <summary>
    ''' Efetua a lista de registros
    ''' </summary>
    ''' <returns>Booleando para a função</returns>
    ''' <remarks></remarks>
    Public Overridable Function List() As List(Of ClassName)

        Dim oConfig As New Configuration
        Dim oFactory As ISessionFactory = Nothing
        Dim oSession As ISession = Nothing
        Dim iLstReturn As IList = Nothing
        Dim lstReturn As List(Of ClassName) = Nothing

        Try
            'definindo o assembly para carregar os arquivo .hbm.xml  que fazem parte do mesmo
            oConfig.AddAssembly(Assembly.GetCallingAssembly())

            'criando uma sessao e iniciando uma transação
            oFactory = oConfig.BuildSessionFactory
            oSession = oFactory.OpenSession

            'list = session.CreateCriteria(GetType(T)).List(Of T)()
            iLstReturn = oSession.CreateCriteria(GetType(ClassName)).List(Of ClassName)()

            lstReturn = New List(Of ClassName)

            For Each oObject As ClassName In iLstReturn
                lstReturn.Add(oObject)
            Next

            'fecha a transação e a sessão
            oSession.Close()

            oConfig = Nothing
            oFactory = Nothing
            oSession = Nothing

        Catch ex As Exception
            oSession.Close()
            oConfig = Nothing
            oFactory = Nothing
            oSession = Nothing
            Throw ex
        End Try

        Return lstReturn

    End Function

    ''' <summary>
    ''' Efetua a exclusão de um registro
    ''' </summary>
    ''' <param name="oPersist">Objeto do Model contendo os dados</param>
    ''' <returns>Booleando para a função</returns>
    ''' <remarks></remarks>
    Public Overridable Function Del(ByRef oPersist As ClassName) As Boolean

        Dim oConfig As New Configuration
        Dim oFactory As ISessionFactory = Nothing
        Dim oSession As ISession = Nothing
        Dim oTransaction As ITransaction = Nothing

        Try
            'definindo o assembly para carregar os arquivo .hbm.xml  que fazem parte do mesmo
            oConfig.AddAssembly(Assembly.GetCallingAssembly())

            'criando uma sessao e iniciando uma transação
            oFactory = oConfig.BuildSessionFactory
            oSession = oFactory.OpenSession
            oTransaction = oSession.BeginTransaction

            oSession.Delete(oPersist)

            oSession.Flush()

            'fecha a transação e a sessão
            oTransaction.Commit()
            oSession.Close()

            oConfig = Nothing
            oFactory = Nothing
            oSession = Nothing
            oTransaction = Nothing

        Catch ex As Exception
            oTransaction.Rollback()
            oSession.Close()
            oConfig = Nothing
            oFactory = Nothing
            oSession = Nothing
            oTransaction = Nothing
            Throw ex
        End Try

    End Function

    Public Overridable Function LoadByQuery(ByVal hsqlQuery As String) As IList(Of ClassName)
        Dim lstReturn As New List(Of ClassName)
        Dim iLstReturn As IList = Nothing
        Dim oConfig As New Configuration
        Dim oFactory As ISessionFactory = Nothing
        Dim oSession As ISession = Nothing

        Try
            'definindo o assembly para carregar os arquivo .hbm.xml  que fazem parte do mesmo
            oConfig.AddAssembly(Assembly.GetCallingAssembly())

            'criando uma sessao e iniciando uma transação
            oFactory = oConfig.BuildSessionFactory
            oSession = oFactory.OpenSession

            iLstReturn = oSession.CreateQuery(hsqlQuery).List

            lstReturn = New List(Of ClassName)

            For Each oData As ClassName In iLstReturn
                lstReturn.Add(oData)
            Next

            'fecha a transação e a sessão
            oSession.Close()

            oConfig = Nothing
            oFactory = Nothing
            oSession = Nothing
            iLstReturn = Nothing

        Catch ex As Exception
            oSession.Close()
            oConfig = Nothing
            oFactory = Nothing
            oSession = Nothing
            iLstReturn = Nothing
            Throw ex
        End Try

        Return lstReturn

    End Function


#Region " IDisposable Support "

    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: free other state (managed objects).
            End If

            ' TODO: free your own state (unmanaged objects).
            ' TODO: set large fields to null.
        End If
        Me.disposedValue = True
    End Sub
    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class
