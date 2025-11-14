export class Doctor {
    DoctorID?: number = 0;
    FirstName: string = '';
    LastName: string = '';
    Email: string = '';
    PhoneNumber: string = '';
    GenderID: number | null = null;
    BloodGroupID: number | null = null;
    QualificationID: number | null = null;
    SpecializationIDs: number[] = [];
    CRRIStartDate?: string | null = null;
    ConsultationFee: number = 0;
    DateOfBirth?: string | null = null;
    Address?: string = '';
    StateID?: number | null = null;
    DistrictID?: number | null = null;
    TalukaID?: number | null = null;
    IsActive: boolean = true;
    CreatedOn?: string = new Date().toISOString();
    CreatedBy?: number | null = null;
    ModifiedOn?: string | null = null;
    ModifiedBy?: number | null = null;
    ProfilePicOriginalName?: string | null = null;
}
