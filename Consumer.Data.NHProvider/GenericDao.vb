Imports NHibernate
Imports System.Collections.Generic

''' <summary>
''' Classe genérica para acesso a dados utilizando NHibernate. Como utiliza generics, é possível persistir
''' qualquer objeto, bastando definir os tipos a serem usados no lugar de Tipo e ID.
''' <example>
''' Dim usuario As New Usuario
''' Dim usuarioDAO As New GenericDAO(Of Usuario, Integer) 
''' 
''' usuario.Nome = "Rogério"
''' usuario.Telefone = "19 32612649"
''' usuario.Email = "bragil@gmail.com"
''' 
''' If usuarioDAO.Insert(usuario) Then
'''      Console.WriteLine("Sucesso!")
''' Else
'''      Console.WriteLine("Erro ao inserir registro!")
''' End If
''' </example>
''' </summary>
''' <typeparam name="Tipo">Tipo do objeto persistível</typeparam>
''' <typeparam name="ID">Tipo de dado do identificador único (chave primária)</typeparam>
''' 
Public Class GenericDao(Of Tipo, ID)
    Implements IGenericDao(Of Tipo, ID), IDisposable

    Private oSession As ISession
    Private oTransaction As ITransaction
    Private blnHasTransaction As Boolean
    'Private oNhTemplate As 

    ''' <summary>
    ''' Construtor, obtém a sessão do NHibernateHelper e inicia a transação.
    ''' </summary>
    Public Sub New(Optional ByVal blnWithTransaction As Boolean = False)
        Try
            oSession = NHibernateHelper.GetCurrentSession(GetType(Tipo).Assembly)
            blnHasTransaction = blnWithTransaction
            If blnWithTransaction Then
                oTransaction = oSession.BeginTransaction()
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    ''' <summary>
    ''' Persiste um novo objeto.
    ''' </summary>
    ''' <param name="Obj">Objeto a ser persistido</param>
    ''' <returns>True para sucesso, False para erro.</returns>
    Public Overridable Function Insert(ByVal Obj As Tipo) As Boolean Implements IGenericDao(Of Tipo, ID).Insert
        Dim oRetorno As Object
        Try
            oRetorno = oSession.Save(Obj)
            Finish()
        Catch ex As Exception
            Return False
        End Try
        Return True
    End Function

    ''' <summary>
    ''' Atualiza um objeto existente.
    ''' </summary>
    ''' <param name="Obj">Objeto a ser atualizado</param>
    ''' <returns>True para sucesso, False para erro.</returns>
    Public Overridable Function Update(ByVal Obj As Tipo) As Boolean Implements IGenericDao(Of Tipo, ID).Update
        Try
            oSession.Update(Obj)
            Finish()
        Catch ex As Exception
            Return False
        End Try
        Return True
    End Function

    ''' <summary>
    ''' Exclui um objeto existente.
    ''' </summary>
    ''' <param name="Obj">Objeto a ser atualizado</param>
    ''' <returns>True para sucesso, False para erro.</returns>
    Public Overridable Function Delete(ByVal Obj As Tipo) As Boolean Implements IGenericDao(Of Tipo, ID).Delete
        Try
            oSession.Delete(Obj)
            Finish()
        Catch ex As Exception
            Return False
        End Try
        Return True
    End Function

    ''' <summary>
    ''' Recupera um objeto persistido na base de dados.
    ''' </summary>
    ''' <param name="Identifier">Chave primária do objeto</param>
    ''' <returns>Objeto</returns>
    Public Overridable Function Find(ByVal Identifier As ID) As Tipo Implements IGenericDao(Of Tipo, ID).Find
        Dim oTipo As Tipo
        Try

            oTipo = CType(oSession.Load(GetType(Tipo), Identifier), Tipo)
            Return oTipo
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    ''' <summary>
    ''' Retorna todos os objetos persistidos na base de dados.
    ''' </summary>
    ''' <returns>Coleção de objetos</returns>
    Public Overridable Function LoadAll() As IList(Of Tipo) Implements IGenericDao(Of Tipo, ID).LoadAll
        Dim lTipo As IList(Of Tipo)
        Try
            'Prepare()
            lTipo = oSession.CreateCriteria(GetType(Tipo)).List(Of Tipo)()
            Return lTipo
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    ''' <summary>
    ''' Confirma a transação corrente (commit)
    ''' </summary>
    Protected Overridable Sub Commit()
        oTransaction.Commit()
    End Sub

    ''' <summary>
    ''' Desfaz a transação corrente (rollback)
    ''' </summary>
    Protected Overridable Sub BeginTransaction()
        oTransaction = oSession.BeginTransaction()
    End Sub

    ''' <summary>
    ''' Begin Trans
    ''' </summary>
    ''' <remarks></remarks>
    Protected Overridable Sub Rollback()
        oTransaction.Rollback()
    End Sub

    ''' <summary>
    ''' Reinicia a Sessão
    ''' </summary>
    ''' <remarks></remarks>
    Protected Overridable Sub Finish()
        oSession.Flush()
        oSession.Clear()
    End Sub

    ''' <summary>
    ''' Verifica se há ou não transação
    ''' </summary>
    ''' <value>Boolean</value>
    ''' <returns>Boolean</returns>
    ''' <remarks></remarks>
    Public Property HasTransaction() As Boolean
        Get
            Return blnHasTransaction
        End Get
        Set(ByVal value As Boolean)
            blnHasTransaction = value
        End Set
    End Property

    ''' <summary>
    ''' Retorna dados através de uma query
    ''' </summary>
    ''' <param name="strQuery">Query</param>
    ''' <param name="strValue">Valor a Filtrar</param>
    ''' <param name="strParName">Nome do Parametro</param>
    ''' <returns>Lista Tipada</returns>
    ''' <remarks></remarks>
    Function LoadByQuery(ByVal strQuery As String, _
                         ByVal strParName As String, _
                         ByVal strValue As String) As IList(Of Tipo)

        Dim oReturn As IList(Of Tipo)
        Dim oQuery As NHibernate.IQuery

        Try
            oQuery = oSession.CreateQuery(strQuery) _
            .SetString(strParName, strValue)
            oReturn = oQuery.List(Of Tipo)()

        Catch ex As Exception
            oReturn = Nothing
            Throw ex
        End Try

        Return oReturn

    End Function


#Region "other Methods"

    'public virtual List<T> LoadByQuery(string hsqlQuery, object[] values)
    '{

    '    List<T> listReturn = new List<T>();
    '    IList oArray = null;

    '    oArray = nhTemplate.Find(hsqlQuery, values) as IList;
    '    listReturn = new List<T>();
    '    foreach (T oData in oArray)
    '    {
    '        listReturn.Add(oData);
    '    }

    '    return listReturn;
    '}

    'public virtual List<T> LoadByQuery(string hsqlQuery)
    '{

    '    List<T> listReturn = new List<T>();
    '    IList oArray = null;

    '    oArray = nhTemplate.Find(hsqlQuery) as IList;
    '    listReturn = new List<T>();
    '    foreach (T oData in oArray)
    '    {
    '        listReturn.Add(oData);
    '    }

    '    return listReturn;
    '}
#End Region

#Region " IDisposable Support "

    Private disposedValue As Boolean = False        ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                If oSession.Transaction.IsActive Then
                    oSession.Transaction.Commit()
                End If
                oTransaction.Dispose()
                oSession.Close()
                oSession.Dispose()
            End If
            ' TODO: free shared unmanaged resources
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