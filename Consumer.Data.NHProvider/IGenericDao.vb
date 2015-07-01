Public Interface IGenericDao(Of Tipo, ID)

    Function Insert(ByVal oTipo As Tipo) As Boolean
    Function Update(ByVal oTipo As Tipo) As Boolean
    Function Delete(ByVal oTipo As Tipo) As Boolean
    Function Find(ByVal Identifier As ID) As Tipo
    Function LoadAll() As IList(Of Tipo)

End Interface