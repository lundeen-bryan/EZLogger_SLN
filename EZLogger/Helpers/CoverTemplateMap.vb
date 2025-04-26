Imports System.Collections.Generic

Namespace Helpers

    ''' <summary>
    ''' Represents template mapping information for each cover letter.
    ''' </summary>
    Public Class CoverTemplateInfo
        Public Property FileName As String
        Public Property NeedsMailMerge As Boolean
        Public Property MailMergeSourceKey As String ' JSON key from sp_filepath, if mail merge is needed
    End Class

    ''' <summary>
    ''' Provides mapping between cover letter codes (A-T) and their corresponding template details.
    ''' </summary>
    Public Module CoverTemplateMap

        Private ReadOnly _templateMap As New Dictionary(Of String, CoverTemplateInfo)(StringComparer.OrdinalIgnoreCase) From {
            {"A", New CoverTemplateInfo With {.FileName = "A. Fax Cover Sheet bm.dot", .NeedsMailMerge = False}},
            {"B", New CoverTemplateInfo With {.FileName = "B. Court Fax Cover Sheet.dot", .NeedsMailMerge = True, .MailMergeSourceKey = "court_contact"}},
            {"C", New CoverTemplateInfo With {.FileName = "C. Sheriff Fax Cover Sheet bm.dotx", .NeedsMailMerge = True, .MailMergeSourceKey = "sheriff_addresses"}},
            {"D", New CoverTemplateInfo With {.FileName = "D. CONREP Fax Cover Sheet bm.dot", .NeedsMailMerge = False}},
            {"E", New CoverTemplateInfo With {.FileName = "E. DA Fax Cover Sheet bm.dot", .NeedsMailMerge = True, .MailMergeSourceKey = "da_contact_database"}},
            {"F", New CoverTemplateInfo With {.FileName = "F. Periodic Progress Report 1026(f) Cover Letter.dotx", .NeedsMailMerge = False}},
            {"G", New CoverTemplateInfo With {.FileName = "G. WIC 6316.2 MDSO Ext Cover and Affidavit.dotx", .NeedsMailMerge = False}},
            {"H", New CoverTemplateInfo With {.FileName = "H. 1026.5(b)(1) NOT extending Cover.dotx", .NeedsMailMerge = False}},
            {"I", New CoverTemplateInfo With {.FileName = "I. 1026.5(b)(1) Ext Cover and Affidavit.dotx", .NeedsMailMerge = True, .MailMergeSourceKey = "da_contact_database"}},
            {"J", New CoverTemplateInfo With {.FileName = "J. 2972 Cover and Affidavit.dotx", .NeedsMailMerge = True, .MailMergeSourceKey = "da_contact_database"}},
            {"K", New CoverTemplateInfo With {.FileName = "K. 1370 90-Day Proximal 9-Month.dotx", .NeedsMailMerge = False}},
            {"L", New CoverTemplateInfo With {.FileName = "L. 1372 CERT Template - e.dot", .NeedsMailMerge = False}},
            {"M", New CoverTemplateInfo With {.FileName = "M. 1372 CERT Template.dot", .NeedsMailMerge = False}},
            {"N", New CoverTemplateInfo With {.FileName = "N. UNLIKELY (b)(1) Sheriff Cover Letter.dotx", .NeedsMailMerge = True, .MailMergeSourceKey = "sheriff_addresses"}},
            {"O", New CoverTemplateInfo With {.FileName = "O. UNLIKELY c1 Court cover bm.dotx", .NeedsMailMerge = False}},
            {"P", New CoverTemplateInfo With {.FileName = "P. UNLIKELY (c)(1) Sheriff Cover Letter.dotx", .NeedsMailMerge = True, .MailMergeSourceKey = "sheriff_addresses"}},
            {"Q", New CoverTemplateInfo With {.FileName = "Q. UNLIKELY b1 Court cover bm.dotx", .NeedsMailMerge = False}},
            {"R", New CoverTemplateInfo With {.FileName = "R. TCAR_Updated.dot", .NeedsMailMerge = False}},
            {"S", New CoverTemplateInfo With {.FileName = "S. Court Email.dot", .NeedsMailMerge = False}},
            {"T", New CoverTemplateInfo With {.FileName = "T. Sheriff Email.dot", .NeedsMailMerge = False}}
        }

        ''' <summary>
        ''' Gets the template information for the given cover letter code.
        ''' </summary>
        ''' <param name="letter">The letter representing the cover template (A-T).</param>
        ''' <returns>CoverTemplateInfo object if found; otherwise Nothing.</returns>
        Public Function GetTemplateInfo(letter As String) As CoverTemplateInfo
            If String.IsNullOrEmpty(letter) Then Return Nothing

            Dim info As CoverTemplateInfo = Nothing
            _templateMap.TryGetValue(letter.Trim.ToUpper(), info)
            Return info
        End Function

        ''' <summary>
        ''' Gets only the template file name for a given letter.
        ''' </summary>
        Public Function GetTemplateFileName(letter As String) As String
            Dim info = GetTemplateInfo(letter)
            Return If(info IsNot Nothing, info.FileName, String.Empty)
        End Function

        ''' <summary>
        ''' Gets the Mail Merge data source key for a given letter (only if mail merge is needed).
        ''' </summary>
        Public Function GetMailMergeDataSource(letter As String) As String
            Dim info = GetTemplateInfo(letter)
            If info IsNot Nothing AndAlso info.NeedsMailMerge Then
                Return info.MailMergeSourceKey
            End If
            Return String.Empty
        End Function

    End Module

End Namespace
