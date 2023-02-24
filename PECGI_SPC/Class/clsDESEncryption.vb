'########################################################################################################################################
'System         : PGM PIS
'Program        : Class 3DES Encryption (Triple Data Encryption Standard)
'Overview       : This program about
'                 Decrypt and Encrypt
'Parameter Input: 
'Created By     : Edi
'Created Date   : 14 Oct 2013
'Last Update By : 
'Last Update    :
'Modify Update  ([Date],[Editor],[Summary],[Version])
'########################################################################################################################################

Imports System.Security.Cryptography
Imports System.IO

Public Class clsDESEncryption
    Private TripleDes As New TripleDESCryptoServiceProvider

    Public Function EncryptData(clearText As String, EncryptionKey As String) As String
        EncryptionKey = EncryptionKey + "WebPECGI2020"
        Dim clearBytes() As Byte = Encoding.Unicode.GetBytes(clearText)
        Using encryptor As Aes = Aes.Create
            Dim pdb As New Rfc2898DeriveBytes(EncryptionKey, New Byte() {&H49, &H76, &H61, &H6E, &H20, &H4D, &H65, &H64, &H76, &H65, &H64, &H65, &H76})
            encryptor.Key = pdb.GetBytes(32)
            encryptor.IV = pdb.GetBytes(16)
            Using Ms As New MemoryStream
                Using cs As New CryptoStream(Ms, encryptor.CreateEncryptor, CryptoStreamMode.Write)
                    cs.Write(clearBytes, 0, clearBytes.Length)
                    cs.Close()
                End Using
                clearText = Convert.ToBase64String(Ms.ToArray)
            End Using
        End Using
        Return clearText
    End Function

    Public Function Decrypt(cipherText As String, EncryptionKey As String) As String
        If cipherText.Length < 5 Then
            Return cipherText
        End If
        EncryptionKey = EncryptionKey + "WebPECGI2020"
        Dim cipherBytes() As Byte = System.Convert.FromBase64String(cipherText)
        Using encryptor As Aes = Aes.Create
            Dim pdb As New Rfc2898DeriveBytes(EncryptionKey, New Byte() {&H49, &H76, &H61, &H6E, &H20, &H4D, &H65, &H64, &H76, &H65, &H64, &H65, &H76})
            encryptor.Key = pdb.GetBytes(32)
            encryptor.IV = pdb.GetBytes(16)
            Using Ms As New MemoryStream
                Using cs As New CryptoStream(Ms, encryptor.CreateDecryptor, CryptoStreamMode.Write)
                    cs.Write(cipherBytes, 0, cipherBytes.Length)
                    cs.Close()
                End Using
                cipherText = Encoding.Unicode.GetString(Ms.ToArray)
            End Using
        End Using
        Return cipherText
    End Function

    Sub New(ByVal key As String)
        ' Initialize the crypto provider.
        TripleDes.Key = TruncateHash(key, TripleDes.KeySize \ 8)
        TripleDes.IV = TruncateHash("", TripleDes.BlockSize \ 8)
    End Sub

    Private Function TruncateHash(ByVal key As String, ByVal length As Integer) As Byte()

        Dim sha1 As New SHA1CryptoServiceProvider

        ' Hash the key. 
        Dim keyBytes() As Byte =
            System.Text.Encoding.Unicode.GetBytes(key)
        Dim hash() As Byte = sha1.ComputeHash(keyBytes)

        ' Truncate or pad the hash. 
        ReDim Preserve hash(length - 1)
        Return hash
    End Function

    Public Function EncryptData(ByVal plaintext As String) As String

        ' Convert the plaintext string to a byte array. 
        Dim plaintextBytes() As Byte =
            System.Text.Encoding.Unicode.GetBytes(plaintext)

        ' Create the stream. 
        Dim ms As New System.IO.MemoryStream
        ' Create the encoder to write to the stream. 
        Dim encStream As New CryptoStream(ms,
            TripleDes.CreateEncryptor(),
            System.Security.Cryptography.CryptoStreamMode.Write)

        ' Use the crypto stream to write the byte array to the stream.
        encStream.Write(plaintextBytes, 0, plaintextBytes.Length)
        encStream.FlushFinalBlock()

        ' Convert the encrypted stream to a printable string. 
        Return Convert.ToBase64String(ms.ToArray)
    End Function

    Public Function DecryptData(ByVal encryptedtext As String) As String

        ' Convert the encrypted text string to a byte array. 
        Dim encryptedBytes() As Byte = Convert.FromBase64String(encryptedtext)

        ' Create the stream. 
        Dim ms As New System.IO.MemoryStream
        ' Create the decoder to write to the stream. 
        Dim decStream As New CryptoStream(ms,
            TripleDes.CreateDecryptor(),
            System.Security.Cryptography.CryptoStreamMode.Write)

        ' Use the crypto stream to write the byte array to the stream.
        decStream.Write(encryptedBytes, 0, encryptedBytes.Length)
        decStream.FlushFinalBlock()

        ' Convert the plaintext stream to a string. 
        Return System.Text.Encoding.Unicode.GetString(ms.ToArray)
    End Function
End Class
