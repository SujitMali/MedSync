// export interface AppointmentModel {
//     [x: string]: any;
//     appointmentID: number;
//     doctorID: number;
//     patientID: number;
//     patientName: string;
//     doctorName: string;
//     appointmentDate: string;
//     startTime: string;
//     endTime: string;
//     reasonForVisit: string;
//     status: string;
//     createdOn: string;
//     modifiedOn?: string;
//     isActive: boolean;
//     medicalHistory: string;
//     medicalConcern: string;
//     insuranceDetails: string;
//     patientDateOfBirth: string,
//     patientGender: string,
//     patientBloodGroup: string
// }

export interface AppointmentModel {
    [x: string]: any;

    AppointmentID: number;
    DoctorID: number;
    PatientID: number;

    PatientName: string;
    DoctorName: string;

    AppointmentDate: string;
    StartTime: string;
    EndTime: string;

    ReasonForVisit: string;
    StatusName: string;

    CreatedOn: string;
    ModifiedOn?: string;

    IsActive: boolean;

    MedicalHistory: string;
    MedicalConcern: string;
    InsuranceDetails: string;

    PatientDOB: string;
    PatientGender: string;
    PatientBloodGroup: string;

    PatientEmail: string;
    PatientPhone: string;
}
