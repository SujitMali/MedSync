Directory structure:
└── sujitmali-medsync/
    ├── Backend/
    │   └── MedSync-API/
    │       ├── MedSync-API.sln
    │       ├── MedSync-API/
    │       │   ├── Global.asax
    │       │   ├── Global.asax.cs
    │       │   ├── MedSync-API.csproj
    │       │   ├── packages.config
    │       │   ├── Web.config
    │       │   ├── Web.Debug.config
    │       │   ├── Web.Release.config
    │       │   ├── App_Start/
    │       │   │   ├── BundleConfig.cs
    │       │   │   ├── FilterConfig.cs
    │       │   │   ├── RouteConfig.cs
    │       │   │   └── WebApiConfig.cs
    │       │   ├── Areas/
    │       │   │   └── HelpPage/
    │       │   │       ├── ApiDescriptionExtensions.cs
    │       │   │       ├── HelpPage.css
    │       │   │       ├── HelpPageAreaRegistration.cs
    │       │   │       ├── HelpPageConfigurationExtensions.cs
    │       │   │       ├── XmlDocumentationProvider.cs
    │       │   │       ├── App_Start/
    │       │   │       │   └── HelpPageConfig.cs
    │       │   │       ├── Controllers/
    │       │   │       │   └── HelpController.cs
    │       │   │       ├── ModelDescriptions/
    │       │   │       │   ├── CollectionModelDescription.cs
    │       │   │       │   ├── ComplexTypeModelDescription.cs
    │       │   │       │   ├── DictionaryModelDescription.cs
    │       │   │       │   ├── EnumTypeModelDescription.cs
    │       │   │       │   ├── EnumValueDescription.cs
    │       │   │       │   ├── IModelDocumentationProvider.cs
    │       │   │       │   ├── KeyValuePairModelDescription.cs
    │       │   │       │   ├── ModelDescription.cs
    │       │   │       │   ├── ModelDescriptionGenerator.cs
    │       │   │       │   ├── ModelNameAttribute.cs
    │       │   │       │   ├── ModelNameHelper.cs
    │       │   │       │   ├── ParameterAnnotation.cs
    │       │   │       │   ├── ParameterDescription.cs
    │       │   │       │   └── SimpleTypeModelDescription.cs
    │       │   │       ├── Models/
    │       │   │       │   └── HelpPageApiModel.cs
    │       │   │       ├── SampleGeneration/
    │       │   │       │   ├── HelpPageSampleGenerator.cs
    │       │   │       │   ├── HelpPageSampleKey.cs
    │       │   │       │   ├── ImageSample.cs
    │       │   │       │   ├── InvalidSample.cs
    │       │   │       │   ├── ObjectGenerator.cs
    │       │   │       │   ├── SampleDirection.cs
    │       │   │       │   └── TextSample.cs
    │       │   │       └── Views/
    │       │   │           ├── _ViewStart.cshtml
    │       │   │           ├── Web.config
    │       │   │           ├── Help/
    │       │   │           │   ├── Api.cshtml
    │       │   │           │   ├── Index.cshtml
    │       │   │           │   ├── ResourceModel.cshtml
    │       │   │           │   └── DisplayTemplates/
    │       │   │           │       ├── ApiGroup.cshtml
    │       │   │           │       ├── CollectionModelDescription.cshtml
    │       │   │           │       ├── ComplexTypeModelDescription.cshtml
    │       │   │           │       ├── DictionaryModelDescription.cshtml
    │       │   │           │       ├── EnumTypeModelDescription.cshtml
    │       │   │           │       ├── HelpPageApiModel.cshtml
    │       │   │           │       ├── ImageSample.cshtml
    │       │   │           │       ├── InvalidSample.cshtml
    │       │   │           │       ├── KeyValuePairModelDescription.cshtml
    │       │   │           │       ├── ModelDescriptionLink.cshtml
    │       │   │           │       ├── Parameters.cshtml
    │       │   │           │       ├── Samples.cshtml
    │       │   │           │       ├── SimpleTypeModelDescription.cshtml
    │       │   │           │       └── TextSample.cshtml
    │       │   │           └── Shared/
    │       │   │               └── _Layout.cshtml
    │       │   ├── bin/
    │       │   │   ├── BCrypt.Net-Next.xml
    │       │   │   ├── MedSync-API.dll.config
    │       │   │   ├── Microsoft.CodeDom.Providers.DotNetCompilerPlatform.xml
    │       │   │   ├── Microsoft.IdentityModel.JsonWebTokens.xml
    │       │   │   ├── Microsoft.IdentityModel.Logging.xml
    │       │   │   ├── Microsoft.IdentityModel.Tokens.xml
    │       │   │   ├── Newtonsoft.Json.xml
    │       │   │   ├── System.Buffers.xml
    │       │   │   ├── System.IdentityModel.Tokens.Jwt.xml
    │       │   │   ├── System.Memory.xml
    │       │   │   ├── System.Net.Http.Formatting.xml
    │       │   │   ├── System.Numerics.Vectors.xml
    │       │   │   ├── System.Runtime.CompilerServices.Unsafe.xml
    │       │   │   ├── System.Web.Helpers.xml
    │       │   │   ├── System.Web.Http.WebHost.xml
    │       │   │   ├── System.Web.Http.xml
    │       │   │   ├── System.Web.Mvc.xml
    │       │   │   ├── System.Web.Optimization.xml
    │       │   │   ├── System.Web.Razor.xml
    │       │   │   ├── System.Web.WebPages.Deployment.xml
    │       │   │   ├── System.Web.WebPages.Razor.xml
    │       │   │   ├── System.Web.WebPages.xml
    │       │   │   └── roslyn/
    │       │   │       ├── csc.exe.config
    │       │   │       ├── csc.rsp
    │       │   │       ├── csi.exe.config
    │       │   │       ├── csi.rsp
    │       │   │       ├── Microsoft.CSharp.Core.targets
    │       │   │       ├── Microsoft.Managed.Core.targets
    │       │   │       ├── Microsoft.VisualBasic.Core.targets
    │       │   │       ├── vbc.exe.config
    │       │   │       ├── vbc.rsp
    │       │   │       └── VBCSCompiler.exe.config
    │       │   ├── Content/
    │       │   │   ├── bootstrap-grid.css
    │       │   │   ├── bootstrap-grid.rtl.css
    │       │   │   ├── bootstrap-reboot.css
    │       │   │   ├── bootstrap-reboot.rtl.css
    │       │   │   ├── bootstrap-utilities.css
    │       │   │   ├── bootstrap-utilities.rtl.css
    │       │   │   ├── bootstrap.css
    │       │   │   ├── bootstrap.rtl.css
    │       │   │   └── Site.css
    │       │   ├── Controllers/
    │       │   │   ├── AccountController.cs
    │       │   │   ├── AppointmentController.cs
    │       │   │   ├── BaseApiController.cs
    │       │   │   ├── DoctorController.cs
    │       │   │   ├── HomeController.cs
    │       │   │   ├── ScheduleController.cs
    │       │   │   └── ValuesController.cs
    │       │   ├── Filter/
    │       │   │   └── JwtAuthenticateAttribute.cs
    │       │   ├── obj/
    │       │   │   └── Debug/
    │       │   │       ├── DesignTimeResolveAssemblyReferences.cache
    │       │   │       ├── DesignTimeResolveAssemblyReferencesInput.cache
    │       │   │       ├── MedSync-.47A18AE0.Up2Date
    │       │   │       ├── MedSync-API.csproj.AssemblyReference.cache
    │       │   │       ├── MedSync-API.csproj.CoreCompileInputs.cache
    │       │   │       ├── MedSync-API.csproj.FileListAbsolute.txt
    │       │   │       └── .NETFramework,Version=v4.8.AssemblyAttributes.cs
    │       │   ├── Properties/
    │       │   │   └── AssemblyInfo.cs
    │       │   ├── Scripts/
    │       │   │   ├── bootstrap.bundle.js
    │       │   │   ├── bootstrap.esm.js
    │       │   │   ├── bootstrap.js
    │       │   │   ├── jquery-3.7.0.intellisense.js
    │       │   │   ├── jquery-3.7.0.js
    │       │   │   ├── jquery-3.7.0.slim.js
    │       │   │   └── modernizr-2.8.3.js
    │       │   ├── Uploads/
    │       │   │   ├── AppointmentFiles/
    │       │   │   │   ├── 10/
    │       │   │   │   ├── 101/
    │       │   │   │   ├── 102/
    │       │   │   │   ├── 105/
    │       │   │   │   ├── 106/
    │       │   │   │   │   └── ebf09f1a-1181-4b3f-b906-5263b4d1d747
    │       │   │   │   ├── 107/
    │       │   │   │   ├── 108/
    │       │   │   │   ├── 11/
    │       │   │   │   ├── 110/
    │       │   │   │   ├── 112/
    │       │   │   │   ├── 113/
    │       │   │   │   ├── 114/
    │       │   │   │   ├── 115/
    │       │   │   │   │   └── 401f3391-ac36-4f01-b9c4-330ea232f23c.cs
    │       │   │   │   ├── 116/
    │       │   │   │   │   └── de1f0e6f-0a54-422b-acb1-a8d0e47abc82.cs
    │       │   │   │   ├── 117/
    │       │   │   │   ├── 118/
    │       │   │   │   ├── 119/
    │       │   │   │   ├── 12/
    │       │   │   │   ├── 120/
    │       │   │   │   ├── 121/
    │       │   │   │   ├── 122/
    │       │   │   │   ├── 123/
    │       │   │   │   ├── 125/
    │       │   │   │   ├── 126/
    │       │   │   │   ├── 127/
    │       │   │   │   ├── 129/
    │       │   │   │   ├── 13/
    │       │   │   │   ├── 131/
    │       │   │   │   ├── 132/
    │       │   │   │   ├── 133/
    │       │   │   │   ├── 134/
    │       │   │   │   ├── 135/
    │       │   │   │   ├── 136/
    │       │   │   │   ├── 137/
    │       │   │   │   ├── 138/
    │       │   │   │   ├── 139/
    │       │   │   │   ├── 14/
    │       │   │   │   ├── 15/
    │       │   │   │   ├── 16/
    │       │   │   │   ├── 17/
    │       │   │   │   ├── 18/
    │       │   │   │   ├── 19/
    │       │   │   │   │   └── 37cc0d32-2be6-40d2-9f9b-88bbccb9e65b.docx
    │       │   │   │   ├── 20/
    │       │   │   │   ├── 21/
    │       │   │   │   ├── 22/
    │       │   │   │   ├── 24/
    │       │   │   │   ├── 25/
    │       │   │   │   ├── 26/
    │       │   │   │   ├── 30/
    │       │   │   │   ├── 31/
    │       │   │   │   │   └── b62f58dd-c332-4251-b6e9-dc1e97aa62b8.docx
    │       │   │   │   ├── 32/
    │       │   │   │   ├── 33/
    │       │   │   │   ├── 34/
    │       │   │   │   ├── 35/
    │       │   │   │   ├── 36/
    │       │   │   │   ├── 37/
    │       │   │   │   ├── 38/
    │       │   │   │   ├── 39/
    │       │   │   │   ├── 40/
    │       │   │   │   ├── 41/
    │       │   │   │   ├── 6/
    │       │   │   │   ├── 7/
    │       │   │   │   ├── 70/
    │       │   │   │   ├── 75/
    │       │   │   │   ├── 76/
    │       │   │   │   ├── 78/
    │       │   │   │   ├── 79/
    │       │   │   │   ├── 8/
    │       │   │   │   ├── 82/
    │       │   │   │   ├── 84/
    │       │   │   │   ├── 85/
    │       │   │   │   ├── 86/
    │       │   │   │   ├── 9/
    │       │   │   │   ├── 90/
    │       │   │   │   ├── 91/
    │       │   │   │   ├── 92/
    │       │   │   │   ├── 93/
    │       │   │   │   ├── 95/
    │       │   │   │   ├── 96/
    │       │   │   │   ├── 97/
    │       │   │   │   ├── 98/
    │       │   │   │   └── 99/
    │       │   │   └── DoctorProfilePictures/
    │       │   ├── Validators/
    │       │   │   └── DoctorValidator.cs
    │       │   ├── Views/
    │       │   │   ├── _ViewStart.cshtml
    │       │   │   ├── Web.config
    │       │   │   ├── Emails/
    │       │   │   │   ├── AppointmentAccepted_Doctor.html
    │       │   │   │   ├── AppointmentAccepted_Patient.html
    │       │   │   │   ├── AppointmentPendingConfirmation_Patient.html
    │       │   │   │   ├── AppointmentRejected_Doctor.html
    │       │   │   │   ├── AppointmentRejected_Patient.html
    │       │   │   │   ├── AppointmentRequestReceived_Doctor.html
    │       │   │   │   ├── AppointmentRequestSubmitted_Patient.html
    │       │   │   │   ├── RescheduledAppointmentAccepted_Doctor.html
    │       │   │   │   ├── RescheduledAppointmentAccepted_Patient.html
    │       │   │   │   ├── RescheduledAppointmentCancelled_Doctor.html
    │       │   │   │   └── RescheduledAppointmentCancelled_Patient.html
    │       │   │   ├── Home/
    │       │   │   │   └── Index.cshtml
    │       │   │   └── Shared/
    │       │   │       ├── _Layout.cshtml
    │       │   │       └── Error.cshtml
    │       │   └── .vs/
    │       │       └── MedSync-API.csproj.dtbcache.json
    │       ├── MedSync-ClassLibraries/
    │       │   ├── MedSync-ClassLibraries.csproj
    │       │   ├── bin/
    │       │   │   └── Debug/
    │       │   │       ├── BCrypt.Net-Next.xml
    │       │   │       ├── Microsoft.IdentityModel.JsonWebTokens.xml
    │       │   │       ├── Microsoft.IdentityModel.Logging.xml
    │       │   │       ├── Microsoft.IdentityModel.Tokens.xml
    │       │   │       ├── System.Buffers.xml
    │       │   │       ├── System.IdentityModel.Tokens.Jwt.xml
    │       │   │       ├── System.Memory.xml
    │       │   │       ├── System.Numerics.Vectors.xml
    │       │   │       └── System.Runtime.CompilerServices.Unsafe.xml
    │       │   ├── DAL/
    │       │   │   ├── Appointment.cs
    │       │   │   ├── AppointmentStatus.cs
    │       │   │   ├── BloodGroup.cs
    │       │   │   ├── District.cs
    │       │   │   ├── Doctor.cs
    │       │   │   ├── DoctorSchedule.cs
    │       │   │   ├── ErrorLogs.cs
    │       │   │   ├── Gender.cs
    │       │   │   ├── Qualification.cs
    │       │   │   ├── SlotDuration.cs
    │       │   │   ├── Specialization.cs
    │       │   │   ├── State.cs
    │       │   │   ├── Taluka.cs
    │       │   │   └── User.cs
    │       │   ├── Helpers/
    │       │   │   ├── DbErrorLogger.cs
    │       │   │   ├── EmailHelper.cs
    │       │   │   └── JwtTokenManager.cs
    │       │   ├── Models/
    │       │   │   ├── AppointmentFileModel.cs
    │       │   │   ├── AppointmentFilterModel.cs
    │       │   │   ├── AppointmentModel.cs
    │       │   │   ├── AppointmentStatusModel.cs
    │       │   │   ├── AppointmentUpdateResult.cs
    │       │   │   ├── BloodGroupModel.cs
    │       │   │   ├── BookAppointmentRequestModel.cs
    │       │   │   ├── DistrictModel.cs
    │       │   │   ├── DoctorAppointmentsViewModel.cs
    │       │   │   ├── DoctorScheduleModel.cs
    │       │   │   ├── DoctorSlotModel.cs
    │       │   │   ├── DoctorsModel.cs
    │       │   │   ├── GenderModel.cs
    │       │   │   ├── PatientModel.cs
    │       │   │   ├── QualificationModel.cs
    │       │   │   ├── SlotDurationModel.cs
    │       │   │   ├── SpecializationModel.cs
    │       │   │   ├── StateModel.cs
    │       │   │   ├── TalukaModel.cs
    │       │   │   └── UserModel.cs
    │       │   ├── obj/
    │       │   │   └── Debug/
    │       │   │       ├── DesignTimeResolveAssemblyReferencesInput.cache
    │       │   │       ├── MedSync-.AE55B885.Up2Date
    │       │   │       ├── MedSync-ClassLibraries.csproj.AssemblyReference.cache
    │       │   │       ├── MedSync-ClassLibraries.csproj.CoreCompileInputs.cache
    │       │   │       ├── MedSync-ClassLibraries.csproj.FileListAbsolute.txt
    │       │   │       └── .NETFramework,Version=v4.8.AssemblyAttributes.cs
    │       │   └── Properties/
    │       │       └── AssemblyInfo.cs
    │       ├── packages/
    │       │   ├── Antlr.3.5.0.2/
    │       │   │   ├── .signature.p7s
    │       │   │   └── lib/
    │       │   ├── bootstrap.5.2.3/
    │       │   │   ├── .signature.p7s
    │       │   │   ├── content/
    │       │   │   │   ├── Content/
    │       │   │   │   │   ├── bootstrap-grid.css
    │       │   │   │   │   ├── bootstrap-grid.rtl.css
    │       │   │   │   │   ├── bootstrap-reboot.css
    │       │   │   │   │   ├── bootstrap-reboot.rtl.css
    │       │   │   │   │   ├── bootstrap-utilities.css
    │       │   │   │   │   ├── bootstrap-utilities.rtl.css
    │       │   │   │   │   ├── bootstrap.css
    │       │   │   │   │   └── bootstrap.rtl.css
    │       │   │   │   └── Scripts/
    │       │   │   │       ├── bootstrap.bundle.js
    │       │   │   │       ├── bootstrap.esm.js
    │       │   │   │       └── bootstrap.js
    │       │   │   └── contentFiles/
    │       │   │       └── any/
    │       │   │           └── any/
    │       │   │               └── wwwroot/
    │       │   │                   ├── css/
    │       │   │                   │   ├── bootstrap-grid.css
    │       │   │                   │   ├── bootstrap-grid.rtl.css
    │       │   │                   │   ├── bootstrap-reboot.css
    │       │   │                   │   ├── bootstrap-reboot.rtl.css
    │       │   │                   │   ├── bootstrap-utilities.css
    │       │   │                   │   ├── bootstrap-utilities.rtl.css
    │       │   │                   │   ├── bootstrap.css
    │       │   │                   │   └── bootstrap.rtl.css
    │       │   │                   └── js/
    │       │   │                       ├── bootstrap.bundle.js
    │       │   │                       ├── bootstrap.esm.js
    │       │   │                       └── bootstrap.js
    │       │   ├── jQuery.3.7.0/
    │       │   │   ├── .signature.p7s
    │       │   │   ├── Content/
    │       │   │   │   └── Scripts/
    │       │   │   │       ├── jquery-3.7.0-vsdoc.js
    │       │   │   │       ├── jquery-3.7.0.js
    │       │   │   │       └── jquery-3.7.0.slim.js
    │       │   │   └── Tools/
    │       │   │       ├── common.ps1
    │       │   │       ├── install.ps1
    │       │   │       ├── jquery-3.7.0.intellisense.js
    │       │   │       └── uninstall.ps1
    │       │   ├── Microsoft.AspNet.Mvc.5.2.9/
    │       │   │   ├── NET_Library_EULA_ENU.txt
    │       │   │   ├── .signature.p7s
    │       │   │   ├── Content/
    │       │   │   │   ├── Web.config.install.xdt
    │       │   │   │   └── Web.config.uninstall.xdt
    │       │   │   └── lib/
    │       │   │       └── net45/
    │       │   │           └── System.Web.Mvc.xml
    │       │   ├── Microsoft.AspNet.Razor.3.2.9/
    │       │   │   ├── NET_Library_EULA_ENU.txt
    │       │   │   ├── .signature.p7s
    │       │   │   └── lib/
    │       │   │       └── net45/
    │       │   │           └── System.Web.Razor.xml
    │       │   ├── Microsoft.AspNet.Web.Optimization.1.1.3/
    │       │   │   ├── .signature.p7s
    │       │   │   └── lib/
    │       │   │       └── net40/
    │       │   │           └── system.web.optimization.xml
    │       │   ├── Microsoft.AspNet.WebApi.5.2.9/
    │       │   │   ├── NET_Library_EULA_ENU.txt
    │       │   │   └── .signature.p7s
    │       │   ├── Microsoft.AspNet.WebApi.Client.5.2.9/
    │       │   │   ├── NET_Library_EULA_ENU.txt
    │       │   │   ├── .signature.p7s
    │       │   │   └── lib/
    │       │   │       ├── net45/
    │       │   │       │   └── System.Net.Http.Formatting.xml
    │       │   │       ├── netstandard2.0/
    │       │   │       │   └── System.Net.Http.Formatting.xml
    │       │   │       └── portable-wp8+netcore45+net45+wp81+wpa81/
    │       │   │           └── System.Net.Http.Formatting.xml
    │       │   ├── Microsoft.AspNet.WebApi.Core.5.2.9/
    │       │   │   ├── NET_Library_EULA_ENU.txt
    │       │   │   ├── .signature.p7s
    │       │   │   ├── Content/
    │       │   │   │   └── web.config.transform
    │       │   │   └── lib/
    │       │   │       └── net45/
    │       │   │           └── System.Web.Http.xml
    │       │   ├── Microsoft.AspNet.WebApi.HelpPage.5.2.9/
    │       │   │   ├── NET_Library_EULA_ENU.txt
    │       │   │   ├── .signature.p7s
    │       │   │   └── Content/
    │       │   │       └── Areas/
    │       │   │           └── HelpPage/
    │       │   │               ├── ApiDescriptionExtensions.cs.pp
    │       │   │               ├── HelpPage.css.pp
    │       │   │               ├── HelpPageAreaRegistration.cs.pp
    │       │   │               ├── HelpPageConfigurationExtensions.cs.pp
    │       │   │               ├── XmlDocumentationProvider.cs.pp
    │       │   │               ├── App_Start/
    │       │   │               │   └── HelpPageConfig.cs.pp
    │       │   │               ├── Controllers/
    │       │   │               │   └── HelpController.cs.pp
    │       │   │               ├── ModelDescriptions/
    │       │   │               │   ├── CollectionModelDescription.cs.pp
    │       │   │               │   ├── ComplexTypeModelDescription.cs.pp
    │       │   │               │   ├── DictionaryModelDescription.cs.pp
    │       │   │               │   ├── EnumTypeModelDescription.cs.pp
    │       │   │               │   ├── EnumValueDescription.cs.pp
    │       │   │               │   ├── IModelDocumentationProvider.cs.pp
    │       │   │               │   ├── KeyValuePairModelDescription.cs.pp
    │       │   │               │   ├── ModelDescription.cs.pp
    │       │   │               │   ├── ModelDescriptionGenerator.cs.pp
    │       │   │               │   ├── ModelNameAttribute.cs.pp
    │       │   │               │   ├── ModelNameHelper.cs.pp
    │       │   │               │   ├── ParameterAnnotation.cs.pp
    │       │   │               │   ├── ParameterDescription.cs.pp
    │       │   │               │   └── SimpleTypeModelDescription.cs.pp
    │       │   │               ├── Models/
    │       │   │               │   └── HelpPageApiModel.cs.pp
    │       │   │               ├── SampleGeneration/
    │       │   │               │   ├── HelpPageSampleGenerator.cs.pp
    │       │   │               │   ├── HelpPageSampleKey.cs.pp
    │       │   │               │   ├── ImageSample.cs.pp
    │       │   │               │   ├── InvalidSample.cs.pp
    │       │   │               │   ├── ObjectGenerator.cs.pp
    │       │   │               │   ├── SampleDirection.cs.pp
    │       │   │               │   └── TextSample.cs.pp
    │       │   │               └── Views/
    │       │   │                   ├── _ViewStart.cshtml.pp
    │       │   │                   ├── Web.config
    │       │   │                   ├── Help/
    │       │   │                   │   ├── Api.cshtml.pp
    │       │   │                   │   ├── Index.cshtml.pp
    │       │   │                   │   ├── ResourceModel.cshtml.pp
    │       │   │                   │   └── DisplayTemplates/
    │       │   │                   │       ├── ApiGroup.cshtml.pp
    │       │   │                   │       ├── CollectionModelDescription.cshtml.pp
    │       │   │                   │       ├── ComplexTypeModelDescription.cshtml.pp
    │       │   │                   │       ├── DictionaryModelDescription.cshtml.pp
    │       │   │                   │       ├── EnumTypeModelDescription.cshtml.pp
    │       │   │                   │       ├── HelpPageApiModel.cshtml.pp
    │       │   │                   │       ├── ImageSample.cshtml.pp
    │       │   │                   │       ├── InvalidSample.cshtml.pp
    │       │   │                   │       ├── KeyValuePairModelDescription.cshtml.pp
    │       │   │                   │       ├── ModelDescriptionLink.cshtml.pp
    │       │   │                   │       ├── Parameters.cshtml.pp
    │       │   │                   │       ├── Samples.cshtml.pp
    │       │   │                   │       ├── SimpleTypeModelDescription.cshtml.pp
    │       │   │                   │       └── TextSample.cshtml.pp
    │       │   │                   └── Shared/
    │       │   │                       └── _Layout.cshtml.pp
    │       │   ├── Microsoft.AspNet.WebApi.WebHost.5.2.9/
    │       │   │   ├── NET_Library_EULA_ENU.txt
    │       │   │   ├── .signature.p7s
    │       │   │   └── lib/
    │       │   │       └── net45/
    │       │   │           └── System.Web.Http.WebHost.xml
    │       │   ├── Microsoft.AspNet.WebPages.3.2.9/
    │       │   │   ├── NET_Library_EULA_ENU.txt
    │       │   │   ├── .signature.p7s
    │       │   │   ├── Content/
    │       │   │   │   ├── Web.config.install.xdt
    │       │   │   │   └── Web.config.uninstall.xdt
    │       │   │   └── lib/
    │       │   │       └── net45/
    │       │   │           ├── System.Web.Helpers.xml
    │       │   │           ├── System.Web.WebPages.Deployment.xml
    │       │   │           ├── System.Web.WebPages.Razor.xml
    │       │   │           └── System.Web.WebPages.xml
    │       │   ├── Microsoft.CodeDom.Providers.DotNetCompilerPlatform.2.0.1/
    │       │   │   ├── .signature.p7s
    │       │   │   ├── build/
    │       │   │   │   ├── net45/
    │       │   │   │   │   ├── Microsoft.CodeDom.Providers.DotNetCompilerPlatform.Extensions.props
    │       │   │   │   │   └── Microsoft.CodeDom.Providers.DotNetCompilerPlatform.props
    │       │   │   │   └── net46/
    │       │   │   │       ├── Microsoft.CodeDom.Providers.DotNetCompilerPlatform.Extensions.props
    │       │   │   │       └── Microsoft.CodeDom.Providers.DotNetCompilerPlatform.props
    │       │   │   ├── content/
    │       │   │   │   ├── net45/
    │       │   │   │   │   ├── app.config.install.xdt
    │       │   │   │   │   ├── app.config.uninstall.xdt
    │       │   │   │   │   ├── web.config.install.xdt
    │       │   │   │   │   └── web.config.uninstall.xdt
    │       │   │   │   └── net46/
    │       │   │   │       ├── app.config.install.xdt
    │       │   │   │       ├── app.config.uninstall.xdt
    │       │   │   │       ├── web.config.install.xdt
    │       │   │   │       └── web.config.uninstall.xdt
    │       │   │   ├── lib/
    │       │   │   │   └── net45/
    │       │   │   │       └── Microsoft.CodeDom.Providers.DotNetCompilerPlatform.xml
    │       │   │   └── tools/
    │       │   │       ├── net45/
    │       │   │       │   ├── install.ps1
    │       │   │       │   └── uninstall.ps1
    │       │   │       ├── Roslyn45/
    │       │   │       │   ├── csc.exe.config
    │       │   │       │   ├── csc.rsp
    │       │   │       │   ├── csi.rsp
    │       │   │       │   ├── Microsoft.CSharp.Core.targets
    │       │   │       │   ├── Microsoft.VisualBasic.Core.targets
    │       │   │       │   ├── vbc.exe.config
    │       │   │       │   ├── vbc.rsp
    │       │   │       │   └── VBCSCompiler.exe.config
    │       │   │       └── RoslynLatest/
    │       │   │           ├── csc.exe.config
    │       │   │           ├── csc.rsp
    │       │   │           ├── csi.exe.config
    │       │   │           ├── csi.rsp
    │       │   │           ├── Microsoft.CSharp.Core.targets
    │       │   │           ├── Microsoft.Managed.Core.targets
    │       │   │           ├── Microsoft.VisualBasic.Core.targets
    │       │   │           ├── vbc.exe.config
    │       │   │           ├── vbc.rsp
    │       │   │           └── VBCSCompiler.exe.config
    │       │   ├── Microsoft.Web.Infrastructure.2.0.0/
    │       │   │   ├── NET_Library_EULA_ENU.txt
    │       │   │   ├── .signature.p7s
    │       │   │   └── lib/
    │       │   │       └── net40/
    │       │   ├── Modernizr.2.8.3/
    │       │   │   ├── .signature.p7s
    │       │   │   ├── Content/
    │       │   │   │   └── Scripts/
    │       │   │   │       └── modernizr-2.8.3.js
    │       │   │   └── Tools/
    │       │   │       ├── common.ps1
    │       │   │       ├── install.ps1
    │       │   │       └── uninstall.ps1
    │       │   ├── Newtonsoft.Json.13.0.3/
    │       │   │   ├── README.md
    │       │   │   ├── LICENSE.md
    │       │   │   ├── .signature.p7s
    │       │   │   └── lib/
    │       │   │       ├── net20/
    │       │   │       │   └── Newtonsoft.Json.xml
    │       │   │       ├── net35/
    │       │   │       │   └── Newtonsoft.Json.xml
    │       │   │       ├── net40/
    │       │   │       │   └── Newtonsoft.Json.xml
    │       │   │       ├── net45/
    │       │   │       │   └── Newtonsoft.Json.xml
    │       │   │       ├── net6.0/
    │       │   │       │   └── Newtonsoft.Json.xml
    │       │   │       ├── netstandard1.0/
    │       │   │       │   └── Newtonsoft.Json.xml
    │       │   │       ├── netstandard1.3/
    │       │   │       │   └── Newtonsoft.Json.xml
    │       │   │       └── netstandard2.0/
    │       │   │           └── Newtonsoft.Json.xml
    │       │   └── WebGrease.1.6.0/
    │       │       ├── .signature.p7s
    │       │       ├── lib/
    │       │       └── tools/
    │       │           └── WG.EXE
    │       └── .vs/
    │           └── MedSync-API/
    │               ├── config/
    │               │   └── applicationhost.config
    │               ├── FileContentIndex/
    │               │   ├── 30fc716f-d5ae-4ac1-85c6-54a59e064771.vsidx
    │               │   ├── 45305dd5-b59b-4f74-b26b-4092a6c9175c.vsidx
    │               │   ├── 53247fc2-1831-42d1-86e9-dae1500cf3d8.vsidx
    │               │   ├── ba755525-3da0-4722-84ff-b9febfa47957.vsidx
    │               │   └── bf52b7f0-d9dd-49a0-96cc-67834f786633.vsidx
    │               └── v17/
    │                   ├── DocumentLayout.backup.json
    │                   └── DocumentLayout.json
    └── Frontend/
        ├── MedSync-Frontend/
        │   ├── README.md
        │   ├── angular.json
        │   ├── package-lock.json
        │   ├── package.json
        │   ├── tsconfig.app.json
        │   ├── tsconfig.json
        │   ├── tsconfig.spec.json
        │   ├── .editorconfig
        │   ├── .gitignore
        │   ├── public/
        │   ├── src/
        │   │   ├── index.html
        │   │   ├── main.ts
        │   │   ├── styles.scss
        │   │   ├── app/
        │   │   │   ├── app-routing.module.ts
        │   │   │   ├── app.component.html
        │   │   │   ├── app.component.scss
        │   │   │   ├── app.component.ts
        │   │   │   ├── app.module.ts
        │   │   │   ├── admin/
        │   │   │   │   ├── admin-routing.module.ts
        │   │   │   │   ├── admin.module.ts
        │   │   │   │   ├── admin.service.ts
        │   │   │   │   ├── add-doctors/
        │   │   │   │   │   ├── add-doctors.component.html
        │   │   │   │   │   ├── add-doctors.component.scss
        │   │   │   │   │   └── add-doctors.component.ts
        │   │   │   │   ├── admin-layout/
        │   │   │   │   │   ├── admin-layout.component.html
        │   │   │   │   │   ├── admin-layout.component.scss
        │   │   │   │   │   └── admin-layout.component.ts
        │   │   │   │   ├── dashboard/
        │   │   │   │   │   ├── dashboard.component.html
        │   │   │   │   │   ├── dashboard.component.scss
        │   │   │   │   │   └── dashboard.component.ts
        │   │   │   │   ├── manage-credentials/
        │   │   │   │   │   ├── manage-credentials.component.html
        │   │   │   │   │   ├── manage-credentials.component.scss
        │   │   │   │   │   └── manage-credentials.component.ts
        │   │   │   │   ├── manage-slots/
        │   │   │   │   │   ├── manage-slots.component.html
        │   │   │   │   │   ├── manage-slots.component.scss
        │   │   │   │   │   └── manage-slots.component.ts
        │   │   │   │   └── view-doctors/
        │   │   │   │       ├── view-doctors.component.html
        │   │   │   │       ├── view-doctors.component.scss
        │   │   │   │       └── view-doctors.component.ts
        │   │   │   ├── core/
        │   │   │   │   ├── guards/
        │   │   │   │   │   └── auth.guard.ts
        │   │   │   │   ├── interceptors/
        │   │   │   │   │   └── auth.interceptor.ts
        │   │   │   │   └── services/
        │   │   │   │       ├── auth.service.ts
        │   │   │   │       └── login.service.ts
        │   │   │   ├── doctor/
        │   │   │   │   ├── doctor-routing.module.ts
        │   │   │   │   ├── doctor.module.ts
        │   │   │   │   ├── doctor.service.ts
        │   │   │   │   ├── doctor-layout/
        │   │   │   │   │   ├── doctor-layout.component.html
        │   │   │   │   │   ├── doctor-layout.component.scss
        │   │   │   │   │   └── doctor-layout.component.ts
        │   │   │   │   └── view-appointment-request/
        │   │   │   │       ├── view-appointment-request.component.html
        │   │   │   │       ├── view-appointment-request.component.scss
        │   │   │   │       └── view-appointment-request.component.ts
        │   │   │   ├── patient/
        │   │   │   │   ├── patient-routing.module.ts
        │   │   │   │   ├── patient.module.ts
        │   │   │   │   ├── patient.service.ts
        │   │   │   │   ├── book-appointment/
        │   │   │   │   │   ├── book-appointment.component.html
        │   │   │   │   │   ├── book-appointment.component.scss
        │   │   │   │   │   └── book-appointment.component.ts
        │   │   │   │   ├── landing-page/
        │   │   │   │   │   ├── landing-page.component.html
        │   │   │   │   │   ├── landing-page.component.scss
        │   │   │   │   │   └── landing-page.component.ts
        │   │   │   │   ├── patient-layout/
        │   │   │   │   │   ├── patient-layout.component.html
        │   │   │   │   │   ├── patient-layout.component.scss
        │   │   │   │   │   └── patient-layout.component.ts
        │   │   │   │   └── view-doctors-list/
        │   │   │   │       ├── view-doctors-list.component.html
        │   │   │   │       ├── view-doctors-list.component.scss
        │   │   │   │       └── view-doctors-list.component.ts
        │   │   │   └── Shared/
        │   │   │       ├── Components/
        │   │   │       │   ├── login/
        │   │   │       │   │   ├── login.component.html
        │   │   │       │   │   ├── login.component.scss
        │   │   │       │   │   └── login.component.ts
        │   │   │       │   ├── not-authorized/
        │   │   │       │   │   ├── not-authorized.component.html
        │   │   │       │   │   ├── not-authorized.component.scss
        │   │   │       │   │   └── not-authorized.component.ts
        │   │   │       │   └── server-offline/
        │   │   │       │       ├── server-offline.component.html
        │   │   │       │       ├── server-offline.component.scss
        │   │   │       │       └── server-offline.component.ts
        │   │   │       ├── Models/
        │   │   │       │   ├── appointment-file-model.model.ts
        │   │   │       │   ├── appointment-filter.model.ts
        │   │   │       │   ├── appointment-model.model.ts
        │   │   │       │   ├── appointment.model.ts
        │   │   │       │   ├── doctor-appointments-view-model.model.ts
        │   │   │       │   ├── doctor-filter.model.ts
        │   │   │       │   ├── doctor-schedule.model.ts
        │   │   │       │   ├── doctor.model.ts
        │   │   │       │   └── slot-duration.model.ts
        │   │   │       └── Services/
        │   │   │           ├── alert.service.ts
        │   │   │           └── toaster.service.ts
        │   │   ├── assets/
        │   │   │   └── images/
        │   │   ├── environments/
        │   │   │   ├── environment.development.ts
        │   │   │   └── environment.ts
        │   │   └── styles/
        │   │       └── _variables.scss
        │   └── .vscode/
        │       ├── extensions.json
        │       ├── launch.json
        │       └── tasks.json
        └── .vscode/
            └── settings.json
