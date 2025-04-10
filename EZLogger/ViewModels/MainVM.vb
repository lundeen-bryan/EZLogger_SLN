Imports System.ComponentModel

Namespace ViewModels
    Public Class MainVM
        Implements INotifyPropertyChanged

        ' Existing field
        Private _courtNumbers As String
        Public Property CourtNumbers As String
            Get
                Return _courtNumbers
            End Get
            Set(value As String)
                _courtNumbers = value
                OnPropertyChanged(NameOf(CourtNumbers))
            End Set
        End Property

        ' --- Patient fields ---
        Public Property PatientNumber As String
            Get
                Return _patientNumber
            End Get
            Set(value As String)
                _patientNumber = value
                OnPropertyChanged(NameOf(PatientNumber))
            End Set
        End Property
        Private _patientNumber As String

        Public Property FullName As String
            Get
                Return _fullName
            End Get
            Set(value As String)
                _fullName = value
                OnPropertyChanged(NameOf(FullName))
            End Set
        End Property
        Private _fullName As String

        Public Property FName As String
            Get
                Return _fName
            End Get
            Set(value As String)
                _fName = value
                OnPropertyChanged(NameOf(FName))
            End Set
        End Property
        Private _fName As String

        Public Property LName As String
            Get
                Return _lName
            End Get
            Set(value As String)
                _lName = value
                OnPropertyChanged(NameOf(LName))
            End Set
        End Property
        Private _lName As String

        Public Property Program As String
            Get
                Return _program
            End Get
            Set(value As String)
                _program = value
                OnPropertyChanged(NameOf(Program))
            End Set
        End Property
        Private _program As String

        Public Property Unit As String
            Get
                Return _unit
            End Get
            Set(value As String)
                _unit = value
                OnPropertyChanged(NameOf(Unit))
            End Set
        End Property
        Private _unit As String

        Public Property Classification As String
            Get
                Return _classification
            End Get
            Set(value As String)
                _classification = value
                OnPropertyChanged(NameOf(Classification))
            End Set
        End Property
        Private _classification As String

        Public Property County As String
            Get
                Return _county
            End Get
            Set(value As String)
                _county = value
                OnPropertyChanged(NameOf(County))
            End Set
        End Property
        Private _county As String

        Public Property DOB As String
            Get
                Return _dob
            End Get
            Set(value As String)
                _dob = value
                OnPropertyChanged(NameOf(DOB))
            End Set
        End Property
        Private _dob As String

        Public Property Age As String
            Get
                Return _age
            End Get
            Set(value As String)
                _age = value
                OnPropertyChanged(NameOf(Age))
            End Set
        End Property
        Private _age As String

        Public Property CommitmentDate As String
            Get
                Return _commitmentDate
            End Get
            Set(value As String)
                _commitmentDate = value
                OnPropertyChanged(NameOf(CommitmentDate))
            End Set
        End Property
        Private _commitmentDate As String

        Public Property AdmissionDate As String
            Get
                Return _admissionDate
            End Get
            Set(value As String)
                _admissionDate = value
                OnPropertyChanged(NameOf(AdmissionDate))
            End Set
        End Property
        Private _admissionDate As String

        Public Property Expiration As String
            Get
                Return _expiration
            End Get
            Set(value As String)
                _expiration = value
                OnPropertyChanged(NameOf(Expiration))
            End Set
        End Property
        Private _expiration As String

        Public Property AssignedTo As String
            Get
                Return _assignedTo
            End Get
            Set(value As String)
                _assignedTo = value
                OnPropertyChanged(NameOf(AssignedTo))
            End Set
        End Property
        Private _assignedTo As String

        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

        Protected Sub OnPropertyChanged(propertyName As String)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
        End Sub
    End Class
End Namespace
